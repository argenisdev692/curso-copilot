import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthState } from '../authentication/state/auth.state';
import { AuthGuard } from './auth.guard';

describe('AuthGuard', () => {
  let authState: jasmine.SpyObj<AuthState>;
  let router: jasmine.SpyObj<Router>;
  let mockRoute: ActivatedRouteSnapshot;
  let mockState: RouterStateSnapshot;

  beforeEach(() => {
    const authStateSpy = jasmine.createSpyObj('AuthState', ['isAuthenticated', 'currentUser']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthState, useValue: authStateSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    authState = TestBed.inject(AuthState) as jasmine.SpyObj<AuthState>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    mockRoute = {} as ActivatedRouteSnapshot;
    mockState = { url: '/protected' } as RouterStateSnapshot;
  });

  it('should allow access when user is authenticated', () => {
    // Arrange
    authState.isAuthenticated.and.returnValue(true);
    authState.currentUser.and.returnValue({ id: 1, email: 'test@example.com' });

    // Act
    const result = TestBed.runInInjectionContext(() => AuthGuard(mockRoute, mockState));

    // Assert
    expect(result).toBe(true);
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should deny access and redirect to login when user is not authenticated', () => {
    // Arrange
    authState.isAuthenticated.and.returnValue(false);
    authState.currentUser.and.returnValue(null);

    // Act
    const result = TestBed.runInInjectionContext(() => AuthGuard(mockRoute, mockState));

    // Assert
    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/auth/login'], {
      queryParams: { returnUrl: '/protected' }
    });
  });

  it('should deny access when isAuthenticated returns false even with user data', () => {
    // Arrange
    authState.isAuthenticated.and.returnValue(false);
    authState.currentUser.and.returnValue({ id: 1, email: 'test@example.com' });

    // Act
    const result = TestBed.runInInjectionContext(() => AuthGuard(mockRoute, mockState));

    // Assert
    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/auth/login'], {
      queryParams: { returnUrl: '/protected' }
    });
  });

  it('should pass returnUrl in query params', () => {
    // Arrange
    authState.isAuthenticated.and.returnValue(false);
    authState.currentUser.and.returnValue(null);
    mockState.url = '/dashboard/admin';

    // Act
    const result = TestBed.runInInjectionContext(() => AuthGuard(mockRoute, mockState));

    // Assert
    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/auth/login'], {
      queryParams: { returnUrl: '/dashboard/admin' }
    });
  });

  it('should handle root path as returnUrl', () => {
    // Arrange
    authState.isAuthenticated.and.returnValue(false);
    authState.currentUser.and.returnValue(null);
    mockState.url = '/';

    // Act
    const result = TestBed.runInInjectionContext(() => AuthGuard(mockRoute, mockState));

    // Assert
    expect(result).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/auth/login'], {
      queryParams: { returnUrl: '/' }
    });
  });
});
