import { ErrorHandler, Injectable, inject, NgZone } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { NotificationService } from '../../shared/services/notification/notification.service';
import { AuthState } from '../authentication/state/auth.state';

export interface ErrorContext {
  timestamp: string;
  userId?: string;
  userAgent: string;
  url: string;
  userRole?: string;
  correlationId: string;
}

export interface StructuredError {
  message: string;
  stack?: string | undefined;
  context: ErrorContext;
  type: 'http' | 'client' | 'unknown';
  severity: 'low' | 'medium' | 'high' | 'critical';
  retryable: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandler implements ErrorHandler {
  private readonly router = inject(Router);
  private readonly notificationService = inject(NotificationService);
  private readonly authState = inject(AuthState);
  private readonly ngZone = inject(NgZone);

  private errorQueue: StructuredError[] = [];
  private readonly maxQueueSize = 50;
  private readonly retryableStatuses = [408, 429, 500, 502, 503, 504];

  handleError(error: any): void {
    const structuredError = this.structureError(error);

    // Log structured error
    this.logError(structuredError);

    // Queue error for potential batch reporting
    this.queueError(structuredError);

    // Handle error based on type
    this.ngZone.run(() => {
      if (error instanceof HttpErrorResponse) {
        this.handleHttpError(error, structuredError);
      } else if (error instanceof Error) {
        this.handleClientError(error, structuredError);
      } else {
        this.handleUnknownError(error, structuredError);
      }
    });
  }

  private structureError(error: any): StructuredError {
    const user = this.authState.currentUser();
    const userRole = this.authState.userRole();

    const context: ErrorContext = {
      timestamp: new Date().toISOString(),
      userAgent: navigator.userAgent,
      url: window.location.href,
      correlationId: this.generateCorrelationId(),
      ...(user?.id && { userId: user.id.toString() }),
      ...(userRole && { userRole })
    };

    if (error instanceof HttpErrorResponse) {
      return {
        message: error.message,
        stack: error.error?.stack || undefined,
        context,
        type: 'http',
        severity: this.getHttpErrorSeverity(error.status),
        retryable: this.retryableStatuses.includes(error.status)
      };
    } else if (error instanceof Error) {
      return {
        message: error.message,
        stack: error.stack,
        context,
        type: 'client',
        severity: 'high',
        retryable: false
      };
    } else {
      return {
        message: String(error),
        context,
        type: 'unknown',
        severity: 'medium',
        retryable: false
      };
    }
  }

  private getHttpErrorSeverity(status: number): 'low' | 'medium' | 'high' | 'critical' {
    if (status >= 500) return 'critical';
    if (status >= 400) return 'high';
    if (status >= 300) return 'medium';
    return 'low';
  }

  private generateCorrelationId(): string {
    return `err_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
  }

  private logError(error: StructuredError): void {
    const logData = {
      level: error.severity.toUpperCase(),
      message: error.message,
      type: error.type,
      correlationId: error.context.correlationId,
      userId: error.context.userId,
      url: error.context.url,
      timestamp: error.context.timestamp,
      stack: error.stack
    };

    switch (error.severity) {
      case 'critical':
        console.error('🚨 CRITICAL ERROR:', logData);
        break;
      case 'high':
        console.error('❌ HIGH ERROR:', logData);
        break;
      case 'medium':
        console.warn('⚠️ MEDIUM ERROR:', logData);
        break;
      case 'low':
        console.info('ℹ️ LOW ERROR:', logData);
        break;
    }

    // Send to error tracking service in production
    if (this.isProduction()) {
      this.sendToErrorTracking(error);
    }
  }

  private queueError(error: StructuredError): void {
    this.errorQueue.push(error);

    // Keep queue size manageable
    if (this.errorQueue.length > this.maxQueueSize) {
      this.errorQueue.shift();
    }

    // Batch send errors periodically
    if (this.errorQueue.length >= 10) {
      this.flushErrorQueue();
    }
  }

  private flushErrorQueue(): void {
    if (this.errorQueue.length === 0) return;

    // Send batch to error tracking service
    const batch = [...this.errorQueue];
    this.errorQueue = [];

    if (this.isProduction()) {
      this.sendBatchToErrorTracking(batch);
    }
  }

  private handleHttpError(error: HttpErrorResponse, _structuredError: StructuredError): void {
    const { status, error: errorBody } = error;

    // Check if offline
    if (!navigator.onLine) {
      this.handleOfflineError();
      return;
    }

    switch (status) {
      case 400:
        this.notificationService.showError(
          errorBody?.message || 'Los datos proporcionados no son válidos',
          'Datos Inválidos'
        );
        break;

      case 401:
        this.notificationService.showError(
          'Tu sesión ha expirado. Por favor, inicia sesión nuevamente.',
          'Sesión Expirada'
        );
        this.authState.clearState();
        this.router.navigate(['/auth/login'], {
          queryParams: { returnUrl: this.router.url }
        });
        break;

      case 403:
        this.notificationService.showError(
          'No tienes permisos suficientes para realizar esta acción.',
          'Acceso Denegado'
        );
        break;

      case 404:
        this.notificationService.showError(
          'El recurso solicitado no fue encontrado.',
          'Recurso No Encontrado'
        );
        break;

      case 408:
        this.notificationService.showWarning(
          'La solicitud tardó demasiado tiempo. Reintentando...',
          'Tiempo de Espera Agotado'
        );
        // Could implement retry logic here
        break;

      case 409:
        this.notificationService.showError(
          errorBody?.message || 'Conflicto de datos. Los datos pueden haber sido modificados por otro usuario.',
          'Conflicto de Datos'
        );
        break;

      case 429:
        this.notificationService.showWarning(
          'Demasiadas solicitudes. Por favor, espera un momento antes de intentar nuevamente.',
          'Límite de Solicitudes Excedido'
        );
        break;

      case 500:
      case 502:
      case 503:
      case 504:
        this.notificationService.showError(
          'Error interno del servidor. Nuestro equipo ha sido notificado.',
          'Error del Servidor'
        );
        break;

      default:
        this.notificationService.showError(
          'Error de conexión. Verifica tu conexión a internet.',
          'Error de Conexión'
        );
    }
  }

  private handleClientError(error: Error, _structuredError: StructuredError): void {
    // Log client-side errors with more detail
    console.error('Client Error:', error);

    // Show user-friendly message based on error type
    let userMessage = 'Ha ocurrido un error inesperado en la aplicación.';
    let title = 'Error de Aplicación';

    if (error.name === 'ChunkLoadError') {
      userMessage = 'La aplicación se actualizó. Recargando página...';
      title = 'Aplicación Actualizada';
      // Auto-reload after a short delay
      setTimeout(() => window.location.reload(), 2000);
    } else if (error.message.includes('network')) {
      userMessage = 'Error de conexión de red. Verifica tu conexión a internet.';
      title = 'Error de Red';
    }

    this.notificationService.showError(userMessage, title);

    // Send to error tracking service
    this.sendToErrorTracking(_structuredError);
  }

  private handleUnknownError(_error: any, structuredError: StructuredError): void {
    console.error('Unknown Error:', _error);
    this.notificationService.showError(
      'Ha ocurrido un error desconocido. Si el problema persiste, contacta al soporte.',
      'Error Desconocido'
    );

    this.sendToErrorTracking(structuredError);
  }

  private handleOfflineError(): void {
    this.notificationService.showWarning(
      'No hay conexión a internet. Las funcionalidades estarán limitadas hasta que se restablezca la conexión.',
      'Modo Sin Conexión'
    );

    // Could implement offline queue for requests
    // this.offlineQueue.addPendingRequest(request);
  }

  private sendToErrorTracking(_error: StructuredError): void {
    // Implement your error tracking service here
    // Examples: Sentry, LogRocket, Bugsnag, etc.
    /*
    this.errorTrackingService.captureException(error, {
      tags: {
        type: error.type,
        severity: error.severity,
        userId: error.context.userId
      },
      extra: {
        correlationId: error.context.correlationId,
        url: error.context.url,
        userAgent: error.context.userAgent
      }
    });
    */
  }

  private sendBatchToErrorTracking(_errors: StructuredError[]): void {
    // Batch send errors to tracking service
    /*
    this.errorTrackingService.captureBatch(errors.map(error => ({
      exception: error,
      tags: { batch: true }
    })));
    */
  }

  private isProduction(): boolean {
    return !window.location.hostname.includes('localhost') &&
           !window.location.hostname.includes('127.0.0.1');
  }

  // Public method to get error queue for debugging
  getErrorQueue(): readonly StructuredError[] {
    return this.errorQueue;
  }

  // Public method to manually flush error queue
  flushErrors(): void {
    this.flushErrorQueue();
  }
}
