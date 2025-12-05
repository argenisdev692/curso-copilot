import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, BehaviorSubject } from 'rxjs';
import { environment } from '@environments/environment';
import {
  ApiResponse,
  User,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RefreshTokenRequest,
} from '@core/models';

/**
 * Authentication service for handling user authentication
 */
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly apiUrl = `${environment.apiUrl}/auth`;

  /** Current user signal */
  private readonly _currentUser = signal<User | null>(null);

  /** Loading state signal */
  private readonly _isLoading = signal<boolean>(false);

  /** Public computed signals */
  readonly currentUser = this._currentUser.asReadonly();
  readonly isLoading = this._isLoading.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());

  constructor() {
    this.loadStoredUser();
  }

  /**
   * Login user with credentials
   * @param credentials Login credentials
   * @returns Observable with login response
   */
  login(credentials: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    this._isLoading.set(true);

    return this.http.post<ApiResponse<LoginResponse>>(`${this.apiUrl}/login`, credentials).pipe(
      tap((response) => {
        if (response.success) {
          this.storeTokens(response.data);
          this._currentUser.set(response.data.user);
        }
        this._isLoading.set(false);
      }),
      catchError((error) => {
        this._isLoading.set(false);
        return throwError(() => error);
      })
    );
  }

  /**
   * Register new user
   * @param userData Registration data
   * @returns Observable with user data
   */
  register(userData: RegisterRequest): Observable<ApiResponse<User>> {
    this._isLoading.set(true);

    return this.http.post<ApiResponse<User>>(`${this.apiUrl}/register`, userData).pipe(
      tap(() => this._isLoading.set(false)),
      catchError((error) => {
        this._isLoading.set(false);
        return throwError(() => error);
      })
    );
  }

  /**
   * Refresh access token
   * @returns Observable with new tokens
   */
  refreshToken(): Observable<ApiResponse<LoginResponse>> {
    const refreshToken = localStorage.getItem(environment.refreshTokenKey);

    if (!refreshToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    const request: RefreshTokenRequest = { refreshToken };

    return this.http.post<ApiResponse<LoginResponse>>(`${this.apiUrl}/refresh`, request).pipe(
      tap((response) => {
        if (response.success) {
          this.storeTokens(response.data);
        }
      })
    );
  }

  /**
   * Logout user
   */
  logout(): void {
    this.clearTokens();
    this._currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }

  /**
   * Get stored access token
   * @returns Access token or null
   */
  getAccessToken(): string | null {
    return localStorage.getItem(environment.tokenKey);
  }

  /**
   * Check if token is expired
   * @returns True if token is expired
   */
  isTokenExpired(): boolean {
    const expiration = localStorage.getItem(environment.tokenExpirationKey);
    if (!expiration) return true;
    return new Date().getTime() > parseInt(expiration, 10);
  }

  /**
   * Store tokens in local storage
   * Uses 'token' and 'expiresAt' from backend AuthResponseDto
   */
  private storeTokens(data: LoginResponse): void {
    localStorage.setItem(environment.tokenKey, data.token);
    if (data.refreshToken) {
      localStorage.setItem(environment.refreshTokenKey, data.refreshToken);
    }

    // expiresAt is an ISO date string from the backend
    const expirationTime = new Date(data.expiresAt).getTime();
    localStorage.setItem(environment.tokenExpirationKey, expirationTime.toString());
  }

  /**
   * Clear tokens from local storage
   */
  private clearTokens(): void {
    localStorage.removeItem(environment.tokenKey);
    localStorage.removeItem(environment.refreshTokenKey);
    localStorage.removeItem(environment.tokenExpirationKey);
  }

  /**
   * Load user from stored token
   */
  private loadStoredUser(): void {
    const token = this.getAccessToken();
    if (token && !this.isTokenExpired()) {
      // Optionally decode JWT to get user info or fetch from API
      // For now, we'll need to fetch user profile on app init
    }
  }
}
