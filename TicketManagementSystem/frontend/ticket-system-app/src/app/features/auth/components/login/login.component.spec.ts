import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthState } from '../../../core/authentication/state/auth.state';
import { AuthService } from '../../../core/authentication/services/auth.service';
import { LoginComponent } from './login.component';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let authState: jasmine.SpyObj<AuthState>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    const authStateSpy = jasmine.createSpyObj('AuthState', [
      'setLoading',
      'setError',
      'setCurrentUser',
      'setAuthenticated',
      'isAuthenticated',
      'currentUser',
      'loading',
      'error'
    ]);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, LoginComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: AuthState, useValue: authStateSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    authState = TestBed.inject(AuthState) as jasmine.SpyObj<AuthState>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    // Mock signals
    authState.loading.and.returnValue(of(false));
    authState.error.and.returnValue(of(null));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Form Initialization', () => {
    it('should initialize form with required controls', () => {
      // Act
      component.ngOnInit();

      // Assert
      expect(component.loginForm).toBeDefined();
      expect(component.loginForm.get('email')).toBeDefined();
      expect(component.loginForm.get('password')).toBeDefined();
    });

    it('should set email from query params', () => {
      // Arrange
      spyOn(component['route'].snapshot.queryParams, 'get').and.callFake((key: string) => {
        if (key === 'email') return 'test@example.com';
        if (key === 'registered') return 'true';
        return null;
      });

      // Act
      component.ngOnInit();

      // Assert
      expect(component.loginForm.get('email')?.value).toBe('test@example.com');
      expect(component.successMessage()).toBe('✅ Registration successful! Please sign in with your credentials.');
    });
  });

  describe('Form Validation', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should validate email as required', () => {
      // Arrange
      const emailControl = component.loginForm.get('email');

      // Act
      emailControl?.setValue('');

      // Assert
      expect(emailControl?.valid).toBeFalsy();
      expect(emailControl?.errors?.['required']).toBeTruthy();
    });

    it('should validate email format', () => {
      // Arrange
      const emailControl = component.loginForm.get('email');

      // Act
      emailControl?.setValue('invalid-email');

      // Assert
      expect(emailControl?.valid).toBeFalsy();
      expect(emailControl?.errors?.['email']).toBeTruthy();
    });

    it('should validate password as required', () => {
      // Arrange
      const passwordControl = component.loginForm.get('password');

      // Act
      passwordControl?.setValue('');

      // Assert
      expect(passwordControl?.valid).toBeFalsy();
      expect(passwordControl?.errors?.['required']).toBeTruthy();
    });

    it('should validate password minimum length', () => {
      // Arrange
      const passwordControl = component.loginForm.get('password');

      // Act
      passwordControl?.setValue('12345');

      // Assert
      expect(passwordControl?.valid).toBeFalsy();
      expect(passwordControl?.errors?.['minlength']).toBeTruthy();
    });
  });

  describe('onSubmit', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should not submit if form is invalid', () => {
      // Arrange
      component.loginForm.get('email')?.setValue('');
      component.loginForm.get('password')?.setValue('');

      // Act
      component.onSubmit();

      // Assert
      expect(authService.login).not.toHaveBeenCalled();
    });

    it('should submit valid form and handle success', () => {
      // Arrange
      const credentials = {
        email: 'test@example.com',
        password: 'password123'
      };
      const mockResponse = {
        token: 'jwt-token',
        refreshToken: 'refresh-token',
        expiresAt: new Date(),
        user: { id: 1, email: 'test@example.com', fullName: 'Test User', role: 'User' }
      };

      component.loginForm.setValue(credentials);
      authService.login.and.returnValue(of(mockResponse));
      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      component.onSubmit();

      // Assert
      expect(authService.login).toHaveBeenCalledWith(credentials);
      expect(authState.setLoading).toHaveBeenCalledWith(true);
      expect(authState.setError).toHaveBeenCalledWith(null);
    });

    it('should handle login error', () => {
      // Arrange
      const credentials = {
        email: 'test@example.com',
        password: 'wrongpassword'
      };
      const mockError = new Error('Invalid credentials');

      component.loginForm.setValue(credentials);
      authService.login.and.returnValue(throwError(() => mockError));
      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      component.onSubmit();

      // Assert
      expect(authService.login).toHaveBeenCalledWith(credentials);
      expect(authState.setLoading).toHaveBeenCalledWith(true);
      expect(authState.setError).toHaveBeenCalledWith('Invalid credentials');
      expect(authState.setLoading).toHaveBeenCalledWith(false);
    });

    it('should navigate after successful login', (done) => {
      // Arrange
      const credentials = {
        email: 'test@example.com',
        password: 'password123'
      };
      const mockResponse = {
        token: 'jwt-token',
        refreshToken: 'refresh-token',
        expiresAt: new Date(),
        user: { id: 1, email: 'test@example.com', fullName: 'Test User', role: 'User' }
      };

      component.loginForm.setValue(credentials);
      authService.login.and.returnValue(of(mockResponse));
      authState.setAuthenticated.and.returnValue(true);
      authState.currentUser.and.returnValue(mockResponse.user);

      // Act
      component.onSubmit();

      // Assert navigation happens after timeout
      setTimeout(() => {
        expect(router.navigate).toHaveBeenCalledWith(['/dashboard']);
        done();
      }, 150);
    });

    it('should navigate to returnUrl if provided', (done) => {
      // Arrange
      const credentials = {
        email: 'test@example.com',
        password: 'password123'
      };
      const mockResponse = {
        token: 'jwt-token',
        refreshToken: 'refresh-token',
        expiresAt: new Date(),
        user: { id: 1, email: 'test@example.com', fullName: 'Test User', role: 'User' }
      };

      component.loginForm.setValue(credentials);
      authService.login.and.returnValue(of(mockResponse));
      authState.setAuthenticated.and.returnValue(true);
      authState.currentUser.and.returnValue(mockResponse.user);

      // Mock query params with returnUrl
      spyOn(component['route'].snapshot.queryParams, 'get').and.callFake((key: string) => {
        if (key === 'returnUrl') return '/tickets';
        return null;
      });

      // Act
      component.onSubmit();

      // Assert navigation happens after timeout
      setTimeout(() => {
        expect(router.navigate).toHaveBeenCalledWith(['/tickets']);
        done();
      }, 150);
    });
  });

  describe('UI Interactions', () => {
    beforeEach(() => {
      component.ngOnInit();
    });

    it('should toggle password visibility', () => {
      // Arrange
      expect(component.showPassword).toBe(false);

      // Act
      component.togglePasswordVisibility();

      // Assert
      expect(component.showPassword).toBe(true);

      // Act again
      component.togglePasswordVisibility();

      // Assert
      expect(component.showPassword).toBe(false);
    });

    it('should mark form group as touched when submitting invalid form', () => {
      // Arrange
      component.loginForm.get('email')?.setValue('');
      component.loginForm.get('password')?.setValue('');

      spyOn(component.loginForm.get('email')!, 'markAsTouched');
      spyOn(component.loginForm.get('password')!, 'markAsTouched');

      // Act
      component.onSubmit();

      // Assert
      expect(component.loginForm.get('email')?.markAsTouched).toHaveBeenCalled();
      expect(component.loginForm.get('password')?.markAsTouched).toHaveBeenCalled();
    });
  });

  describe('Component Properties', () => {
    it('should expose form controls', () => {
      // Arrange
      component.ngOnInit();

      // Act & Assert
      expect(component.email).toBe(component.loginForm.get('email'));
      expect(component.password).toBe(component.loginForm.get('password'));
    });
  });
});
