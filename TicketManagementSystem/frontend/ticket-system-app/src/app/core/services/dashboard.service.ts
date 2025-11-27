import { inject, signal, resource } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { DashboardStats, ActivityItem, DashboardData } from '@shared/models/common';

/**
 * Service for managing dashboard data and statistics
 * Implements caching and error handling for dashboard operations
 */
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/dashboard';

  // rxResource for reactive dashboard data loading
  private readonly dashboardResource = resource({
    loader: async () => {
      const stats$ = this.getStats();
      const activities$ = this.getRecentActivity();

      try {
        const [stats, recentActivity] = await Promise.all([
          stats$.toPromise(),
          activities$.toPromise()
        ]);

        return {
          stats,
          recentActivity,
          userRole: this.getCurrentUserRole()
        } as DashboardData;
      } catch (error) {
        throw this.handleError(error as HttpErrorResponse);
      }
    }
  });

  // Public readonly signals
  public readonly dashboard = this.dashboardResource.value;
  public readonly loading = this.dashboardResource.isLoading;
  public readonly error = signal<string | null>(null);

  /**
   * Load complete dashboard data using rxResource
   */
  loadDashboardData(): void {
    this.dashboardResource.reload();
  }

  /**
   * Get dashboard statistics
   */
  getStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.apiUrl}/stats`).pipe(
      catchError(error => {
        // Fallback to mock data if API fails
        console.warn('Failed to load stats from API, using fallback data:', error);
        return this.getMockStats();
      })
    );
  }

  /**
   * Get recent activity
   */
  getRecentActivity(): Observable<ActivityItem[]> {
    return this.http.get<ActivityItem[]>(`${this.apiUrl}/activity`).pipe(
      catchError(error => {
        // Fallback to mock data if API fails
        console.warn('Failed to load activity from API, using fallback data:', error);
        return this.getMockActivity();
      })
    );
  }

  /**
   * Refresh dashboard data
   */
  refreshData(): void {
    this.dashboardResource.reload();
  }

  /**
   * Mock data for development/fallback
   */
  private getMockStats(): Observable<DashboardStats> {
    return new Observable(subscriber => {
      subscriber.next({
        totalTickets: 24,
        pendingTickets: 8,
        resolvedTickets: 12,
        criticalTickets: 4,
        totalUsers: 15,
        activeUsers: 12
      });
      subscriber.complete();
    });
  }

  private getMockActivity(): Observable<ActivityItem[]> {
    return new Observable(subscriber => {
      subscriber.next([
        {
          id: 1,
          type: 'ticket_created',
          description: 'New ticket created: "System login issue"',
          timestamp: new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(),
          user: 'John Doe',
          ticketId: 123
        },
        {
          id: 2,
          type: 'ticket_resolved',
          description: 'Ticket #123 resolved by John Doe',
          timestamp: new Date(Date.now() - 4 * 60 * 60 * 1000).toISOString(),
          user: 'John Doe',
          ticketId: 123
        },
        {
          id: 3,
          type: 'ticket_updated',
          description: 'Priority updated for ticket #456',
          timestamp: new Date(Date.now() - 6 * 60 * 60 * 1000).toISOString(),
          user: 'Jane Smith',
          ticketId: 456
        }
      ]);
      subscriber.complete();
    });
  }

  private getCurrentUserRole(): string {
    // This should come from AuthService
    return 'User';
  }

  private handleError(error: HttpErrorResponse): string {
    if (error.error && typeof error.error === 'object' && 'message' in error.error) {
      return `Network error: ${(error.error as any).message}`;
    }

    switch (error.status) {
      case 401:
        return 'Unauthorized access. Please login again.';
      case 403:
        return 'Access forbidden. Insufficient permissions.';
      case 404:
        return 'Dashboard data not found.';
      case 500:
        return 'Server error. Please try again later.';
      default:
        return `Error loading dashboard: ${error.message}`;
    }
  }
}
