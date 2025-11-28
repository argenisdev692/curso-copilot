import { Injectable } from '@angular/core';

/**
 * Logger service for structured logging.
 * Provides logging with context and correlation ID.
 */
@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  private correlationId: string;

  constructor() {
    this.correlationId = this.generateCorrelationId();
  }

  /**
   * Logs a message with the specified level, message, and optional context.
   * @param level The log level (info, warn, error).
   * @param message The log message.
   * @param context Optional context object.
   */
  log(level: 'info' | 'warn' | 'error', message: string, context?: any): void {
    const logEntry = {
      timestamp: new Date().toISOString(),
      level,
      message,
      correlationId: this.correlationId,
      context
    };

    switch (level) {
      case 'info':
        console.log(`[${logEntry.timestamp}] INFO [${logEntry.correlationId}] ${message}`, context || '');
        break;
      case 'warn':
        console.warn(`[${logEntry.timestamp}] WARN [${logEntry.correlationId}] ${message}`, context || '');
        break;
      case 'error':
        console.error(`[${logEntry.timestamp}] ERROR [${logEntry.correlationId}] ${message}`, context || '');
        break;
    }
  }

  /**
   * Generates a unique correlation ID.
   * @returns A unique string ID.
   */
  private generateCorrelationId(): string {
    return 'corr-' + Math.random().toString(36).substr(2, 9);
  }

  /**
   * Sets a new correlation ID (useful for requests).
   * @param id The correlation ID to set.
   */
  setCorrelationId(id: string): void {
    this.correlationId = id;
  }
}
