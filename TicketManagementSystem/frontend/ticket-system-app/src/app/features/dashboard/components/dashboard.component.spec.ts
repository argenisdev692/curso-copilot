import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { DashboardComponent } from './dashboard.component';
import { DashboardService } from '../../../core/services/dashboard.service';
import { AuthService } from '../../../../core/auth/auth.service';
import { DashboardStats, ActivityItem } from '../../models/dashboard.interface';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let dashboardServiceSpy: jasmine.SpyObj<DashboardService>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;

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

  const mockUser = {
    id: 1,
    email: 'john@example.com',
    fullName: 'John Doe',
    role: 'User'
  };

  beforeEach(async () => {
    const dashboardSpy = jasmine.createSpyObj('DashboardService', [], {
      stats: of(mockStats),
      activities: of(mockActivities),
      loading: of(false),
      error: of(null),
      loadDashboardData: jasmine.createSpy().and.returnValue(of({ stats: mockStats, recentActivity: mockActivities, userRole: 'User' })),
      refreshData: jasmine.createSpy()
    });

    const authSpy = jasmine.createSpyObj('AuthService', [], {
      currentUser: of(mockUser)
    });

    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        { provide: DashboardService, useValue: dashboardSpy },
        { provide: AuthService, useValue: authSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    dashboardServiceSpy = TestBed.inject(DashboardService) as jasmine.SpyObj<DashboardService>;
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load dashboard data on init', () => {
    component.ngOnInit();
    expect(dashboardServiceSpy.loadDashboardData).toHaveBeenCalled();
  });

  it('should display user name in welcome message', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Welcome back, John Doe!');
  });

  it('should display stats cards with correct values', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.textContent).toContain('24'); // totalTickets
    expect(compiled.textContent).toContain('8');  // pendingTickets
    expect(compiled.textContent).toContain('12'); // resolvedTickets
    expect(compiled.textContent).toContain('4');  // criticalTickets
  });

  it('should display recent activities', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.textContent).toContain('New ticket created');
    expect(compiled.textContent).toContain('John Doe');
  });

  it('should show loading state', () => {
    Object.defineProperty(dashboardServiceSpy, 'loading', { value: of(true) });
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('.animate-spin')).toBeTruthy();
  });

  it('should show error state with retry button', () => {
    Object.defineProperty(dashboardServiceSpy, 'error', { value: of('Test error') });
    Object.defineProperty(dashboardServiceSpy, 'loading', { value: of(false) });
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.textContent).toContain('Test error');
    const retryButton = compiled.querySelector('button');
    expect(retryButton?.textContent?.trim()).toBe('Retry');
  });

  it('should call refreshData when retry button is clicked', () => {
    Object.defineProperty(dashboardServiceSpy, 'error', { value: of('Test error') });
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;

    const retryButton = compiled.querySelector('button') as HTMLButtonElement;
    retryButton.click();

    expect(dashboardServiceSpy.refreshData).toHaveBeenCalled();
  });

  describe('Activity formatting', () => {
    it('should format timestamps correctly', () => {
      const recentTimestamp = new Date(Date.now() - 2 * 60 * 60 * 1000).toISOString(); // 2 hours ago
      const result = component.formatTimestamp(recentTimestamp);
      expect(result).toBe('2 hours ago');
    });

    it('should return "Just now" for very recent timestamps', () => {
      const recentTimestamp = new Date(Date.now() - 30 * 1000).toISOString(); // 30 seconds ago
      const result = component.formatTimestamp(recentTimestamp);
      expect(result).toBe('Just now');
    });

    it('should format days correctly', () => {
      const oldTimestamp = new Date(Date.now() - 2 * 24 * 60 * 60 * 1000).toISOString(); // 2 days ago
      const result = component.formatTimestamp(oldTimestamp);
      expect(result).toBe('2 days ago');
    });
  });

  describe('Activity colors', () => {
    it('should return correct color for ticket_created', () => {
      expect(component.getActivityColor('ticket_created')).toBe('bg-blue-500');
    });

    it('should return correct color for ticket_resolved', () => {
      expect(component.getActivityColor('ticket_resolved')).toBe('bg-green-500');
    });

    it('should return correct color for ticket_updated', () => {
      expect(component.getActivityColor('ticket_updated')).toBe('bg-yellow-500');
    });

    it('should return correct color for user_registered', () => {
      expect(component.getActivityColor('user_registered')).toBe('bg-purple-500');
    });

    it('should return default color for unknown types', () => {
      expect(component.getActivityColor('unknown')).toBe('bg-gray-500');
    });
  });

  it('should handle API errors gracefully', () => {
    dashboardServiceSpy.loadDashboardData.and.returnValue(throwError(() => new Error('API Error')));
    component.ngOnInit();

    // Component should still be created and handle the error
    expect(component).toBeTruthy();
  });
});
