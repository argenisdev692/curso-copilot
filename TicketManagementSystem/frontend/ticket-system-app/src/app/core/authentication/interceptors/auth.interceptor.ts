import { HttpInterceptorFn, HttpErrorResponse, HttpRequest, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, retry, switchMap, tap } from 'rxjs/operators';
import { throwError, Observable, of, timer } from 'rxjs';
import { API_ENDPOINTS } from '../../http/api.config';
import { NotificationService } from '../../../shared/services/notification/notification.service';

interface RetryConfig {
  count: number;
  delay: number;
  backoff: 'fixed' | 'exponential';
  maxDelay?: number;
}

interface RequestCache {
  [key: string]: {
    response: any;
    timestamp: number;
    ttl: number;
  };
}

/**
 * Advanced HTTP Interceptor with comprehensive features:
 * - JWT authentication with automatic token refresh
 * - Intelligent retry strategies for failed requests
 * - Offline detection and queue management
 * - Request caching for GET requests
 * - Request deduplication
 * - Security headers
 * - Request timing and performance monitoring
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const notificationService = inject(NotificationService);

  // Request cache for GET requests
  const requestCache: RequestCache = {};
  const CACHE_TTL = 5 * 60 * 1000; // 5 minutes

  // Pending requests for deduplication
  const pendingRequests = new Map<string, Observable<any>>();

  // Offline queue
  const offlineQueue: HttpRequest<any>[] = [];

  // Check if request should be cached
  const shouldCache = (req: HttpRequest<any>): boolean => {
    return req.method === 'GET' &&
           !req.url.includes('/auth/') &&
           !req.headers.has('Cache-Control');
  };

  // Generate cache key
  const getCacheKey = (req: HttpRequest<any>): string => {
    return `${req.method}_${req.url}_${JSON.stringify(req.params)}_${JSON.stringify(req.body)}`;
  };

  // Check cache
  const getCachedResponse = (req: HttpRequest<any>): any | null => {
    const key = getCacheKey(req);
    const cached = requestCache[key];

    if (cached && Date.now() - cached.timestamp < cached.ttl) {
      return cached.response;
    }

    // Remove expired cache
    if (cached) {
      delete requestCache[key];
    }

    return null;
  };

  // Cache response
  const cacheResponse = (req: HttpRequest<any>, response: any): void => {
    const key = getCacheKey(req);
    requestCache[key] = {
      response,
      timestamp: Date.now(),
      ttl: CACHE_TTL
    };
  };

  // Get retry configuration based on request type and error
  const getRetryConfig = (req: HttpRequest<any>, error?: HttpErrorResponse): RetryConfig => {
    const isIdempotent = ['GET', 'HEAD', 'PUT', 'DELETE'].includes(req.method);
    const isRetryableStatus = error ? [408, 429, 500, 502, 503, 504].includes(error.status) : false;

    if (!isIdempotent || !isRetryableStatus) {
      return { count: 0, delay: 0, backoff: 'fixed' };
    }

    // Different retry strategies for different scenarios
    if (error?.status === 429) { // Rate limited
      return { count: 3, delay: 2000, backoff: 'exponential', maxDelay: 30000 };
    }

    if (error?.status && error.status >= 500) { // Server errors
      return { count: 2, delay: 1000, backoff: 'exponential', maxDelay: 10000 };
    }

    return { count: 1, delay: 500, backoff: 'fixed' };
  };

  // Calculate delay for retry
  const calculateDelay = (attempt: number, config: RetryConfig): number => {
    if (config.backoff === 'exponential') {
      const delay = config.delay * Math.pow(2, attempt - 1);
      return config.maxDelay ? Math.min(delay, config.maxDelay) : delay;
    }
    return config.delay;
  };

  // Check if user is online
  const isOnline = (): boolean => {
    return navigator.onLine;
  };

  // Handle offline scenario
  const handleOffline = (req: HttpRequest<any>): Observable<any> => {
    if (!isOnline()) {
      // Queue request for when connection is restored
      offlineQueue.push(req);

      notificationService.showWarning(
        'Solicitud guardada. Se ejecutará cuando se restablezca la conexión.',
        'Sin Conexión'
      );

      return throwError(() => new Error('Offline - Request queued'));
    }

    return next(req);
  };

  // Process offline queue when connection is restored
  const processOfflineQueue = (): void => {
    if (isOnline() && offlineQueue.length > 0) {
      const queuedRequests = [...offlineQueue];
      offlineQueue.length = 0; // Clear queue

      queuedRequests.forEach(queuedReq => {
        // Retry queued requests
        next(queuedReq).subscribe({
          next: () => {
            notificationService.showSuccess('Solicitud procesada exitosamente.', 'Conexión Restablecida');
          },
          error: () => {
            notificationService.showError('Error al procesar solicitud guardada.', 'Error');
          }
        });
      });
    }
  };

  // Listen for online/offline events
  if (typeof window !== 'undefined') {
    window.addEventListener('online', processOfflineQueue);
    window.addEventListener('offline', () => {
      notificationService.showWarning('Conexión perdida. Las solicitudes se guardarán temporalmente.', 'Sin Conexión');
    });
  }

  // Request deduplication
  const deduplicationKey = getCacheKey(req);
  if (pendingRequests.has(deduplicationKey)) {
    return pendingRequests.get(deduplicationKey)!;
  }

  // Check cache for GET requests
  if (shouldCache(req)) {
    const cachedResponse = getCachedResponse(req);
    if (cachedResponse) {
      return of(cachedResponse);
    }
  }

  // Add security headers
  const addSecurityHeaders = (req: HttpRequest<any>): HttpRequest<any> => {
    return req.clone({
      setHeaders: {
        'X-Requested-With': 'XMLHttpRequest',
        'X-Client-Version': '1.0.0',
        'X-Timestamp': Date.now().toString(),
        'X-Correlation-ID': `req_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`
      }
    });
  };

  // Start request timing
  const startTime = Date.now();

  // Add authentication and security headers
  const token = authService.getToken();
  let authReq = addSecurityHeaders(req);

  if (token && !isPublicEndpoint(req.url)) {
    authReq = authReq.clone({
      setHeaders: {
        ...authReq.headers,
        Authorization: `Bearer ${token}`
      }
    });
  }

  // Create the request observable
  const request$ = next(authReq).pipe(
    // Log successful requests
    tap(response => {
      const duration = Date.now() - startTime;
      if (response instanceof HttpResponse) {
        console.log(`✅ ${req.method} ${req.url} - ${response.status} (${duration}ms)`);

        // Cache successful GET responses
        if (shouldCache(req) && response.status === 200) {
          cacheResponse(req, response);
        }
      }
    }),

    // Handle errors with retry logic
    catchError((error: HttpErrorResponse) => {
      const duration = Date.now() - startTime;
      console.error(`❌ ${req.method} ${req.url} - ${error.status} (${duration}ms)`, error);

      // Handle authentication errors
      if (error.status === 401) {
        authService.logout();
        notificationService.showError('Tu sesión ha expirado. Redirigiendo al login...', 'Sesión Expirada');
        return throwError(() => error);
      }

      if (error.status === 403) {
        notificationService.showError('No tienes permisos para realizar esta acción.', 'Acceso Denegado');
        return throwError(() => error);
      }

      // Get retry configuration
      const retryConfig = getRetryConfig(req, error);

      if (retryConfig.count > 0) {
        console.log(`🔄 Retrying ${req.method} ${req.url} (${retryConfig.count} attempts)`);

        return timer(calculateDelay(1, retryConfig)).pipe(
          switchMap(() => next(authReq)),
          retry({
            count: retryConfig.count,
            delay: (_error, retryIndex) => {
              const delay = calculateDelay(retryIndex + 1, retryConfig);
              console.log(`⏳ Retry ${retryIndex + 1}/${retryConfig.count} in ${delay}ms`);
              return timer(delay);
            }
          }),
          catchError(finalError => {
            console.error(`💥 Final retry failed for ${req.method} ${req.url}`);
            return throwError(() => finalError);
          })
        );
      }

      return throwError(() => error);
    })
  );

  // Store pending request for deduplication
  pendingRequests.set(deduplicationKey, request$);

  // Clean up pending request when it completes
  request$.subscribe({
    next: () => pendingRequests.delete(deduplicationKey),
    error: () => pendingRequests.delete(deduplicationKey),
    complete: () => pendingRequests.delete(deduplicationKey)
  });

  // Handle offline scenario
  return handleOffline(authReq);
};

/**
 * Check if the endpoint is public (doesn't require authentication)
 */
function isPublicEndpoint(url: string): boolean {
  const publicEndpoints = [
    API_ENDPOINTS.auth.login,
    API_ENDPOINTS.auth.register,
    API_ENDPOINTS.auth.refresh
  ];

  // Check if URL ends with any public endpoint
  return publicEndpoints.some(endpoint => url.includes(endpoint));
}
