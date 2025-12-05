import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse,
  HttpEvent
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, catchError, switchMap, throwError, BehaviorSubject, filter, take } from 'rxjs';
import { SecureAuthService } from '@core/services/secure-auth.service';
import { environment } from '@environments/environment';

/**
 * Estado del refresh en curso para evitar múltiples refreshes simultáneos.
 */
let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<boolean | null>(null);

/**
 * Interceptor de autenticación seguro con soporte para:
 * - HttpOnly cookies (withCredentials)
 * - Tokens CSRF para requests mutables
 * - Refresh automático de tokens
 * - Cola de requests durante refresh
 *
 * ## Flujo de Seguridad
 * 1. Todas las requests a la API incluyen `withCredentials: true`
 * 2. Requests POST/PUT/DELETE incluyen token CSRF
 * 3. Si recibe 401, intenta refresh y reintenta la request original
 * 4. Si refresh falla, redirige a login
 */
export const secureAuthInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const authService = inject(SecureAuthService);

  // Solo interceptar requests a nuestra API
  if (!req.url.includes(environment.apiUrl)) {
    return next(req);
  }

  // Clonar request con las configuraciones de seguridad
  req = addSecurityToRequest(req, authService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Manejar error 401 (no autorizado)
      if (error.status === 401 && !isAuthEndpoint(req.url)) {
        return handleUnauthorized(req, next, authService);
      }

      return throwError(() => error);
    })
  );
};

/**
 * Agrega configuraciones de seguridad a la request.
 */
function addSecurityToRequest(
  req: HttpRequest<unknown>,
  authService: SecureAuthService
): HttpRequest<unknown> {
  const headers: { [key: string]: string } = {};

  // Agregar token CSRF para requests mutables
  if (isMutableRequest(req.method)) {
    const csrfToken = authService.getCsrfToken();
    if (csrfToken) {
      headers['X-XSRF-TOKEN'] = csrfToken;
    }
  }

  return req.clone({
    withCredentials: true, // ⚠️ Crítico: habilita cookies HttpOnly
    setHeaders: headers
  });
}

/**
 * Maneja respuestas 401 intentando refresh del token.
 */
function handleUnauthorized(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: SecureAuthService
): Observable<HttpEvent<unknown>> {

  // Si ya hay un refresh en progreso, esperar a que termine
  if (isRefreshing) {
    return refreshTokenSubject.pipe(
      filter(result => result !== null),
      take(1),
      switchMap((success) => {
        if (success) {
          // Refresh exitoso, reintentar request original
          return next(addSecurityToRequest(req, authService));
        }
        return throwError(() => new Error('Refresh failed'));
      })
    );
  }

  // Iniciar proceso de refresh
  isRefreshing = true;
  refreshTokenSubject.next(null);

  return authService.refreshToken().pipe(
    switchMap(() => {
      isRefreshing = false;
      refreshTokenSubject.next(true);

      // Reintentar request original con nuevo token
      return next(addSecurityToRequest(req, authService));
    }),
    catchError((refreshError) => {
      isRefreshing = false;
      refreshTokenSubject.next(false);

      // Refresh falló, hacer logout
      authService.logout();

      return throwError(() => refreshError);
    })
  );
}

/**
 * Verifica si es una request mutable que requiere CSRF.
 */
function isMutableRequest(method: string): boolean {
  return ['POST', 'PUT', 'DELETE', 'PATCH'].includes(method.toUpperCase());
}

/**
 * Verifica si la URL es un endpoint de autenticación.
 * Estos no deben intentar refresh para evitar loops infinitos.
 */
function isAuthEndpoint(url: string): boolean {
  const authEndpoints = ['/auth/login', '/auth/register', '/auth/refresh', '/auth/revoke'];
  return authEndpoints.some(endpoint => url.includes(endpoint));
}

/**
 * Interceptor de errores con logging seguro.
 * No expone información sensible en los logs.
 */
export const secureErrorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Log seguro - no incluir tokens ni datos sensibles
      console.error(`[HTTP Error] ${error.status} - ${req.method} ${sanitizeUrl(req.url)}`);

      return throwError(() => error);
    })
  );
};

/**
 * Sanitiza la URL para logging, removiendo parámetros sensibles.
 */
function sanitizeUrl(url: string): string {
  try {
    const urlObj = new URL(url);
    // Remover parámetros potencialmente sensibles
    ['token', 'key', 'secret', 'password', 'auth'].forEach(param => {
      if (urlObj.searchParams.has(param)) {
        urlObj.searchParams.set(param, '[REDACTED]');
      }
    });
    return urlObj.toString();
  } catch {
    return url.split('?')[0]; // Fallback: solo el path
  }
}
