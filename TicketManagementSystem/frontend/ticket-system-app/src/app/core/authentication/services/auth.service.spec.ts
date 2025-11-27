import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { AuthState } from '../state/auth.state';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, RefreshTokenRequest, RefreshTokenResponse } from '../../../shared/models/common/auth.interface';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  let authState: jasmine.SpyObj<AuthState>;

  beforeEach(() => {
    const authStateSpy = jasmine.createSpyObj('AuthState', [
      'setCurrentUser',
      'setAuthenticated',
      'setLoading',
      'setError',
      'clearState',
      'isAuthenticated',
      'currentUser'
    ]);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: AuthState, useValue: authStateSpy }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    authState = TestBed.inject(AuthState) as jasmine.SpyObj<AuthState>;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('login', () => {
    it('should login successfully and update auth state', (done) => {
      // Arrange
      const loginRequest: LoginRequest = {
        email: 'test@example.com',
        password: 'password123'
      };

      const mockResponse: LoginResponse = {
        token: 'jwt-token',
        refreshToken: 'refresh-token',
        expiresAt: new Date(Date.now() + 3600000).toISOString(), // 1 hour from now
        user: {
          id: 1,
          email: 'test@example.com',
          fullName: 'Test User',
          role: 'User' as 'Admin' | 'Agent' | 'User'
        }
      };

      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      service.login(loginRequest).subscribe({
        next: (response) => {
          // Assert
          expect(response).toEqual(mockResponse);
          expect(authState.setLoading).toHaveBeenCalledWith(true);
          expect(authState.setError).toHaveBeenCalledWith(null);
          expect(authState.setCurrentUser).toHaveBeenCalled();
          expect(authState.setAuthenticated).toHaveBeenCalledWith(true);
          expect(authState.setLoading).toHaveBeenCalledWith(false);
          done();
        },
        error: () => fail('Should not have failed')
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/login');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(loginRequest);
      req.flush(mockResponse);
    });

    it('should handle login error and update auth state', (done) => {
      // Arrange
      const loginRequest: LoginRequest = {
        email: 'test@example.com',
        password: 'wrongpassword'
      };

      const mockError = {
        status: 401,
        statusText: 'Unauthorized',
        error: {
          title: 'Invalid Credentials',
          detail: 'The email or password is incorrect'
        }
      };

      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      service.login(loginRequest).subscribe({
        next: () => fail('Should have failed'),
        error: (error) => {
          // Assert
          expect(error.message).toBe('Invalid Credentials');
          expect(authState.setLoading).toHaveBeenCalledWith(true);
          expect(authState.setError).toHaveBeenCalledWith('Invalid Credentials');
          expect(authState.setLoading).toHaveBeenCalledWith(false);
          done();
        }
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/login');
      expect(req.request.method).toBe('POST');
      req.flush(mockError.error, mockError);
    });

    it('should handle network error during login', (done) => {
      // Arrange
      const loginRequest: LoginRequest = {
        email: 'test@example.com',
        password: 'password123'
      };

      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      service.login(loginRequest).subscribe({
        next: () => fail('Should have failed'),
        error: (error) => {
          // Assert
          expect(error.message).toBe('Error de autenticación');
          expect(authState.setLoading).toHaveBeenCalledWith(true);
          expect(authState.setError).toHaveBeenCalledWith('Error de autenticación');
          expect(authState.setLoading).toHaveBeenCalledWith(false);
          done();
        }
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/login');
      expect(req.request.method).toBe('POST');
      req.error(new ErrorEvent('network error'));
    });
  });

  describe('register', () => {
    it('should register successfully', (done) => {
      // Arrange
      const registerRequest: RegisterRequest = {
        email: 'newuser@example.com',
        password: 'Password123!',
        fullName: 'New User',
        role: 'User' as 'Admin' | 'Agent' | 'User'
      };

      const mockResponse: RegisterResponse = {
        message: 'User registered successfully',
        user: {
          id: 2,
          email: 'newuser@example.com',
          fullName: 'New User',
          role: 'User' as 'Admin' | 'Agent' | 'User'
        }
      };

      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      service.register(registerRequest).subscribe({
        next: (response) => {
          // Assert
          expect(response).toEqual(mockResponse);
          expect(authState.setLoading).toHaveBeenCalledWith(true);
          expect(authState.setError).toHaveBeenCalledWith(null);
          expect(authState.setLoading).toHaveBeenCalledWith(false);
          done();
        },
        error: () => fail('Should not have failed')
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/register');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(registerRequest);
      req.flush(mockResponse);
    });

    it('should handle registration error', (done) => {
      // Arrange
      const registerRequest: RegisterRequest = {
        email: 'existing@example.com',
        password: 'Password123!',
        fullName: 'Existing User',
        role: 'User' as 'Admin' | 'Agent' | 'User'
      };

      const mockError = {
        status: 400,
        statusText: 'Bad Request',
        error: {
          detail: 'User with this email already exists'
        }
      };

      authState.setLoading.and.callThrough();
      authState.setError.and.callThrough();

      // Act
      service.register(registerRequest).subscribe({
        next: () => fail('Should have failed'),
        error: (error) => {
          // Assert
          expect(error.message).toBe('User with this email already exists');
          expect(authState.setLoading).toHaveBeenCalledWith(true);
          expect(authState.setError).toHaveBeenCalledWith('User with this email already exists');
          expect(authState.setLoading).toHaveBeenCalledWith(false);
          done();
        }
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/register');
      expect(req.request.method).toBe('POST');
      req.flush(mockError.error, mockError);
    });
  });

  describe('performRefreshToken', () => {
    it('should refresh token successfully', (done) => {
      // Arrange
      const refreshRequest: RefreshTokenRequest = {
        refreshToken: 'valid-refresh-token'
      };

      const mockResponse: RefreshTokenResponse = {
        token: 'new-jwt-token',
        refreshToken: 'new-refresh-token',
        expiresAt: new Date(Date.now() + 3600000).toISOString()
      };

      // Act
      service.performRefreshToken().subscribe({
        next: (response) => {
          // Assert
          expect(response).toEqual(mockResponse);
          done();
        },
        error: () => fail('Should not have failed')
      });

      // Assert HTTP request
      const req = httpMock.expectOne('http://localhost:5000/api/auth/refresh');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(refreshRequest);
      req.flush(mockResponse);
    });
  });

  describe('logout', () => {
    it('should clear auth data', () => {
      // Arrange
      spyOn(localStorage, 'removeItem');
      spyOn(authState, 'clearState');

      // Act
      service.logout();

      // Assert
      expect(localStorage.removeItem).toHaveBeenCalledWith('token');
      expect(localStorage.removeItem).toHaveBeenCalledWith('refreshToken');
      expect(localStorage.removeItem).toHaveBeenCalledWith('user');
      expect(localStorage.removeItem).toHaveBeenCalledWith('tokenExpiry');
      expect(authState.clearState).toHaveBeenCalled();
    });
  });

  describe('isAuthenticated', () => {
    it('should return boolean', () => {
      // Act
      const result = service.isAuthenticated();

      // Assert
      expect(typeof result).toBe('boolean');
    });
  });

  describe('getToken', () => {
    it('should return token or null', () => {
      // Act
      const result = service.getToken();

      // Assert
      expect(result === null || typeof result === 'string').toBe(true);
    });
  });
});
