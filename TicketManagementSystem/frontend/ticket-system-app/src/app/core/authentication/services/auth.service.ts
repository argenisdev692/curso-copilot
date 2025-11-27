import { inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { timer } from 'rxjs';
import { AuthState } from '../state/auth.state';
import {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  RefreshTokenRequest,
  RefreshTokenResponse
} from '../../../shared/models/common/auth.interface';
import { User } from '../../../shared/models/common/user.interface';
import { AuthClient } from '../../http/api-client';
import { StorageService } from '../../services/storage.service';

export class AuthService {
  private readonly router = inject(Router);
  private readonly authState = inject(AuthState);
  private readonly storage = inject(StorageService);
  private readonly authClient = inject(AuthClient);

  // Token management signals
  private readonly _token = signal<string | null>(null);
  private readonly _refreshToken = signal<string | null>(null);
  private readonly _tokenExpiry = signal<Date | null>(null);

  readonly token = this._token.asReadonly();
  readonly refreshToken = this._refreshToken.asReadonly();
  readonly tokenExpiry = this._tokenExpiry.asReadonly();

  constructor() {
    this.initializeAuthState();
    this.setupTokenRefreshTimer();
  }

  /**
   * Initialize authentication state from storage
   */
  private initializeAuthState(): void {
    const token = this.storage.getItem<string>('token');
    const refreshToken = this.storage.getItem<string>('refreshToken');
    const user = this.storage.getItem<User>('user');
    const expiryStr = this.storage.getItem<string>('tokenExpiry');

    if (token && user) {
      try {
        const expiry = expiryStr ? new Date(expiryStr) : null;

        this._token.set(token);
        this._refreshToken.set(refreshToken);
        this._tokenExpiry.set(expiry);
        this.authState.setCurrentUser(user);
        this.authState.setAuthenticated(true);
      } catch (error) {
        console.error('Error parsing stored auth data:', error);
        this.clearStoredAuth();
      }
    }
  }

  /**
   * Setup automatic token refresh timer
   */
  private setupTokenRefreshTimer(): void {
    // Refresh token 5 minutes before expiry
    const refreshBuffer = 5 * 60 * 1000; // 5 minutes

    timer(60000, 60000).subscribe(() => { // Check every minute
      const expiry = this._tokenExpiry();
      if (expiry && expiry.getTime() - Date.now() < refreshBuffer) {
        this.refreshTokenIfNeeded();
      }
    });
  }

  /**
   * Login user
   */
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    this.authState.setLoading(true);
    this.authState.setError(null);

    try {
      const response = await this.authClient.login(credentials);
      this.handleLoginSuccess(response);
      return response;
    } catch (error) {
      throw this.handleLoginError(error as HttpErrorResponse);
    }
  }

  /**
   * Register new user
   */
  async register(userData: RegisterRequest): Promise<RegisterResponse> {
    this.authState.setLoading(true);
    this.authState.setError(null);

    try {
      const response = await this.authClient.register(userData);
      this.handleRegisterSuccess(response);
      return response;
    } catch (error) {
      throw this.handleRegisterError(error as HttpErrorResponse);
    }
  }

  /**
   * Refresh authentication token
   */
  async performRefreshToken(): Promise<RefreshTokenResponse> {
    const refreshToken = this._refreshToken();
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }

    const request: RefreshTokenRequest = { refreshToken };

    try {
      const response = await this.authClient.refresh(request);
      this.handleTokenRefreshSuccess(response);
      return response;
    } catch (error) {
      throw this.handleTokenRefreshError(error as HttpErrorResponse);
    }
  }

  /**
   * Logout user
   */
  logout(): void {
    this.clearStoredAuth();
    this.authState.clearState();
    this._token.set(null);
    this._refreshToken.set(null);
    this._tokenExpiry.set(null);
    this.router.navigate(['/auth/login']);
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    const token = this._token();
    const expiry = this._tokenExpiry();

    if (!token || !expiry) {
      return false;
    }

    return expiry.getTime() > Date.now();
  }

  /**
   * Get current JWT token
   */
  getToken(): string | null {
    return this._token();
  }

  // Private methods

  private handleLoginSuccess(response: LoginResponse): void {
    this.storeAuthData(response);
    // Convert UserBasicDto to User by adding missing properties
    const user: User = {
      ...response.user,
      role: response.user.role as 'Admin' | 'Agent' | 'User',
      isActive: true, // Assume active by default
      createdAt: new Date(), // Default to now
      updatedAt: new Date() // Default to now
    };
    this.authState.setCurrentUser(user);
    this.authState.setAuthenticated(true);
    this.authState.setLoading(false);
  }

  private handleLoginError(error: HttpErrorResponse): Error {
    let errorMessage = 'Error de autenticación';

    if (error.error?.title) {
      errorMessage = error.error.title;
    } else if (error.error?.message) {
      errorMessage = error.error.message;
    }

    this.authState.setError(errorMessage);
    this.authState.setLoading(false);
    return new Error(errorMessage);
  }

  private handleRegisterSuccess(_response: RegisterResponse): void {
    this.authState.setLoading(false);
    // Optionally auto-login after registration
    // this.handleLoginSuccess(_response);
  }

  private handleRegisterError(error: HttpErrorResponse): Error {
    let errorMessage = 'Error de registro';

    // Handle ProblemDetails format from backend
    if (error.error?.detail) {
      errorMessage = error.error.detail;
    } else if (error.error?.title) {
      errorMessage = error.error.title;
    } else if (error.error?.message) {
      errorMessage = error.error.message;
    } else if (error.message) {
      errorMessage = error.message;
    }

    this.authState.setError(errorMessage);
    this.authState.setLoading(false);
    return new Error(errorMessage);
  }

  private handleTokenRefreshSuccess(response: RefreshTokenResponse): void {
    this.storeAuthData(response);
  }

  private handleTokenRefreshError(error: HttpErrorResponse): Error {
    console.error('Token refresh failed:', error);
    this.logout(); // Force logout on refresh failure
    return error;
  }

  private async refreshTokenIfNeeded(): Promise<void> {
    if (this.isAuthenticated() && this._refreshToken()) {
      try {
        await this.performRefreshToken();
      } catch (error) {
        console.error('Token refresh failed:', error);
      }
    }
  }

  private storeAuthData(response: LoginResponse | RefreshTokenResponse): void {
    const expiry = new Date(response.expiresAt);

    this._token.set(response.token);
    this._refreshToken.set(response.refreshToken || this._refreshToken()!);
    this._tokenExpiry.set(expiry);

    this.storage.setItem('token', response.token);
    this.storage.setItem('refreshToken', response.refreshToken || this._refreshToken()!);
    this.storage.setItem('tokenExpiry', expiry.toISOString());

    if ('user' in response) {
      this.storage.setItem('user', response.user);
    }
  }

  private clearStoredAuth(): void {
    this.storage.removeItem('token');
    this.storage.removeItem('refreshToken');
    this.storage.removeItem('user');
    this.storage.removeItem('tokenExpiry');
  }

}
