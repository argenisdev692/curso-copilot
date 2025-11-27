/// <reference types="jasmine" />

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DashboardService } from './dashboard.service';
import { DashboardStats, ActivityItem } from '../../shared/models/common';

describe('DashboardService', () => {
  let service: DashboardService;
  let httpMock: HttpTestingController;

  const mockStats: DashboardStats = {
    totalTickets: 24,
    pendingTickets: 8,
    resolvedTickets: 12,
    criticalTickets: 4,
    totalUsers: 15,
    activeUsers: 12
  };

  const mockActivities: ActivityItem[] = [
    {
      id: 1,
      type: 'ticket_created',
      description: 'New ticket created',
      timestamp: '2025-11-18T10:00:00Z',
      user: 'John Doe',
      ticketId: 123
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DashboardService]
    });

    service = TestBed.inject(DashboardService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getStats', () => {
    it('should return stats from API', () => {
      service.getStats().subscribe(stats => {
        expect(stats).toEqual(mockStats);
      });

      const req = httpMock.expectOne('/api/dashboard/stats');
      expect(req.request.method).toBe('GET');
      req.flush(mockStats);
    });

    it('should return mock data when API fails', () => {
      service.getStats().subscribe(stats => {
        expect(stats.totalTickets).toBe(24);
        expect(stats.pendingTickets).toBe(8);
      });

      const req = httpMock.expectOne('/api/dashboard/stats');
      req.error(new ErrorEvent('network error'));
    });
  });

  describe('getRecentActivity', () => {
    it('should return activities from API', () => {
      service.getRecentActivity().subscribe(activities => {
        expect(activities).toEqual(mockActivities);
      });

      const req = httpMock.expectOne('/api/dashboard/activity');
      expect(req.request.method).toBe('GET');
      req.flush(mockActivities);
    });

    it('should return mock data when API fails', () => {
      service.getRecentActivity().subscribe(activities => {
        expect(activities.length).toBe(3);
        expect(activities[0].type).toBe('ticket_created');
      });

      const req = httpMock.expectOne('/api/dashboard/activity');
      req.error(new ErrorEvent('network error'));
    });
  });

  describe('loadDashboardData', () => {
    it('should load complete dashboard data', () => {
      service.loadDashboardData();

      const statsReq = httpMock.expectOne('/api/dashboard/stats');
      const activityReq = httpMock.expectOne('/api/dashboard/activity');

      statsReq.flush(mockStats);
      activityReq.flush(mockActivities);

      // Wait for the resource to load
      expect(service.loading()).toBeFalse();
      expect(service.dashboard()?.stats).toEqual(mockStats);
      expect(service.dashboard()?.recentActivity).toEqual(mockActivities);
      expect(service.dashboard()?.userRole).toBe('User');
    });

    it('should handle API errors gracefully', () => {
      service.loadDashboardData();

      const statsReq = httpMock.expectOne('/api/dashboard/stats');
      const activityReq = httpMock.expectOne('/api/dashboard/activity');

      statsReq.error(new ErrorEvent('network error'));
      activityReq.error(new ErrorEvent('network error'));

      // Check that error is handled
      expect(service.loading()).toBeFalse();
      expect(service.error()).toBeDefined();
    });
  });

  describe('Signals', () => {
    it('should expose readonly signals', () => {
      expect(service.dashboard).toBeDefined();
      expect(service.loading).toBeDefined();
      expect(service.error).toBeDefined();
    });

    it('should update loading state during API calls', () => {
      service.loadDashboardData();

      const statsReq = httpMock.expectOne('/api/dashboard/stats');
      const activityReq = httpMock.expectOne('/api/dashboard/activity');

      // Initially loading should be true
      expect(service.loading()).toBe(true);

      statsReq.flush(mockStats);
      activityReq.flush(mockActivities);

      // Loading should be false after completion
      expect(service.loading()).toBe(false);
    });
  });

  describe('Error Handling', () => {
    it('should handle 401 errors', () => {
      service.getStats().subscribe(
        () => fail('Should have failed'),
        error => expect(error.message).toContain('Unauthorized')
      );

      const req = httpMock.expectOne('/api/dashboard/stats');
      req.flush('Unauthorized', { status: 401, statusText: 'Unauthorized' });
    });

    it('should handle 404 errors', () => {
      service.getStats().subscribe(
        () => fail('Should have failed'),
        error => expect(error.message).toContain('not found')
      );

      const req = httpMock.expectOne('/api/dashboard/stats');
      req.flush('Not Found', { status: 404, statusText: 'Not Found' });
    });

    it('should handle network errors', () => {
      service.getStats().subscribe(
        () => fail('Should have failed'),
        error => expect(error.message).toContain('Network error')
      );

      const req = httpMock.expectOne('/api/dashboard/stats');
      req.error(new ErrorEvent('network error'));
    });
  });
});
