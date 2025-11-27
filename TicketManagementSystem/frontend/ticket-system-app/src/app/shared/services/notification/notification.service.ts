import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message?: string;
  duration?: number;
  action?: {
    label: string;
    callback: () => void;
  };
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications$ = new BehaviorSubject<Notification[]>([]);
  private counter = 0;

  readonly notifications = this.notifications$.asObservable();

  /**
   * Show success notification
   */
  showSuccess(message: string, title = 'Éxito', duration = 5000): void {
    this.addNotification({
      type: 'success',
      title,
      message,
      duration
    });
  }

  /**
   * Show error notification
   */
  showError(message: string, title = 'Error', duration = 7000): void {
    this.addNotification({
      type: 'error',
      title,
      message,
      duration
    });
  }

  /**
   * Show warning notification
   */
  showWarning(message: string, title = 'Advertencia', duration = 6000): void {
    this.addNotification({
      type: 'warning',
      title,
      message,
      duration
    });
  }

  /**
   * Show info notification
   */
  showInfo(message: string, title = 'Información', duration = 5000): void {
    this.addNotification({
      type: 'info',
      title,
      message,
      duration
    });
  }

  /**
   * Show notification with action
   */
  showWithAction(
    message: string,
    actionLabel: string,
    actionCallback: () => void,
    type: Notification['type'] = 'info',
    title = 'Notificación',
    duration = 10000
  ): void {
    this.addNotification({
      type,
      title,
      message,
      duration,
      action: {
        label: actionLabel,
        callback: actionCallback
      }
    });
  }

  /**
   * Remove notification by ID
   */
  removeNotification(id: string): void {
    const current = this.notifications$.value;
    const filtered = current.filter(n => n.id !== id);
    this.notifications$.next(filtered);
  }

  /**
   * Clear all notifications
   */
  clearAll(): void {
    this.notifications$.next([]);
  }

  /**
   * Clear notifications by type
   */
  clearByType(type: Notification['type']): void {
    const current = this.notifications$.value;
    const filtered = current.filter(n => n.type !== type);
    this.notifications$.next(filtered);
  }

  private addNotification(notification: Omit<Notification, 'id'>): void {
    const id = `notification-${++this.counter}`;
    const fullNotification: Notification = {
      id,
      ...notification
    };

    const current = this.notifications$.value;
    this.notifications$.next([...current, fullNotification]);

    // Auto-remove after duration
    if (notification.duration && notification.duration > 0) {
      setTimeout(() => {
        this.removeNotification(id);
      }, notification.duration);
    }
  }
}
