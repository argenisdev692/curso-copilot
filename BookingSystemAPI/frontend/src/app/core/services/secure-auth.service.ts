import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, BehaviorSubject, timer, Subscription } from 'rxjs';
import { environment } from '@environments/environment';
import {
  ApiResponse,
  User,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from '@core/models';

/**
 * Servicio de autenticación seguro con soporte para HttpOnly cookies.
 *
 * ## Características de Seguridad
 * - Tokens almacenados en cookies HttpOnly (inmune a XSS)
 * - Protección CSRF mediante tokens
 * - Refresh automático antes de expiración
 * - Limpieza de estado en logout
 *
 * ## Flujo de Autenticación
 * 1. Login: Backend envía tokens como cookies HttpOnly
 * 2. Requests: Cookies se envían automáticamente con `withCredentials: true`
 * 3. Refresh: Se ejecuta automáticamente antes de expiración
 * 4. Logout: Backend invalida tokens y limpia cookies
 */
@Injectable({
  providedIn: 'root',
})
export class SecureAuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly apiUrl = `${environment.apiUrl}/auth`;

  /** Current user signal - solo información pública, no tokens */
  private readonly _currentUser = signal<User | null>(null);

  /** Loading state signal */
  private readonly _isLoading = signal<boolean>(false);

  /** Subscription para el timer de refresh automático */
  private refreshTimerSubscription: Subscription | null = null;

  /** Duración del access token en milisegundos (del servidor) */
  private tokenExpiresIn: number = 0;

  /** Public computed signals */
  readonly currentUser = this._currentUser.asReadonly();
  readonly isLoading = this._isLoading.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());

  constructor() {
    // Intentar recuperar sesión al iniciar
    this.checkAuthStatus();
  }

  /**
   * Login con credenciales.
   * Los tokens se almacenan como cookies HttpOnly por el servidor.
   *
   * @param credentials Credenciales de login
   * @returns Observable con la respuesta (solo datos del usuario, sin tokens)
   */
  login(credentials: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    this._isLoading.set(true);

    return this.http.post<ApiResponse<LoginResponse>>(
      `${this.apiUrl}/login`,
      credentials,
      { withCredentials: true } // ⚠️ Crítico: habilita envío/recepción de cookies
    ).pipe(
      tap((response) => {
        if (response.success && response.data) {
          this._currentUser.set(response.data.user);

          // Configurar refresh automático usando expiresAt del backend
          if (response.data.expiresAt) {
            const expiresAt = new Date(response.data.expiresAt).getTime();
            this.tokenExpiresIn = expiresAt - Date.now();
            this.scheduleTokenRefresh();
          }
        }
        this._isLoading.set(false);
      }),
      catchError((error: HttpErrorResponse) => {
        this._isLoading.set(false);
        return throwError(() => this.handleAuthError(error));
      })
    );
  }

  /**
   * Registro de nuevo usuario.
   *
   * @param userData Datos de registro
   * @returns Observable con respuesta
   */
  register(userData: RegisterRequest): Observable<ApiResponse<User>> {
    this._isLoading.set(true);

    return this.http.post<ApiResponse<User>>(
      `${this.apiUrl}/register`,
      userData,
      { withCredentials: true }
    ).pipe(
      tap(() => this._isLoading.set(false)),
      catchError((error: HttpErrorResponse) => {
        this._isLoading.set(false);
        return throwError(() => this.handleAuthError(error));
      })
    );
  }

  /**
   * Refresh del access token.
   * El refresh token se envía automáticamente desde la cookie.
   *
   * @returns Observable con nueva respuesta de login
   */
  refreshToken(): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(
      `${this.apiUrl}/refresh`,
      {}, // No enviamos nada en el body, el refresh token está en la cookie
      { withCredentials: true }
    ).pipe(
      tap((response) => {
        if (response.success && response.data) {
          this._currentUser.set(response.data.user);

          // Reprogramar el siguiente refresh usando expiresAt del backend
          if (response.data.expiresAt) {
            const expiresAt = new Date(response.data.expiresAt).getTime();
            this.tokenExpiresIn = expiresAt - Date.now();
            this.scheduleTokenRefresh();
          }
        }
      }),
      catchError((error: HttpErrorResponse) => {
        // Si el refresh falla, hacer logout
        this.logout();
        return throwError(() => error);
      })
    );
  }

  /**
   * Logout del usuario.
   * El servidor invalida los tokens y limpia las cookies.
   */
  logout(): void {
    // Cancelar timer de refresh
    this.cancelRefreshTimer();

    this.http.post(
      `${this.apiUrl}/revoke`,
      {},
      { withCredentials: true }
    ).subscribe({
      next: () => {
        this.clearAuthState();
        this.router.navigate(['/auth/login']);
      },
      error: () => {
        // Aún si falla la revocación, limpiar estado local
        this.clearAuthState();
        this.router.navigate(['/auth/login']);
      }
    });
  }

  /**
   * Verifica el estado de autenticación actual.
   * Útil para recuperar sesión al recargar la página.
   */
  checkAuthStatus(): void {
    this.http.get<ApiResponse<User>>(
      `${this.apiUrl}/me`,
      { withCredentials: true }
    ).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this._currentUser.set(response.data);
          this.scheduleTokenRefresh();
        }
      },
      error: () => {
        // No hay sesión activa, eso está bien
        this._currentUser.set(null);
      }
    });
  }

  /**
   * Programa el refresh automático del token antes de que expire.
   * Se ejecuta 1 minuto antes de la expiración.
   */
  private scheduleTokenRefresh(): void {
    this.cancelRefreshTimer();

    if (this.tokenExpiresIn <= 0) {
      return;
    }

    // Refrescar 1 minuto antes de expiración
    const refreshTime = Math.max(this.tokenExpiresIn - 60000, 30000);

    this.refreshTimerSubscription = timer(refreshTime).subscribe(() => {
      this.refreshToken().subscribe({
        error: (err) => console.error('Error en refresh automático:', err)
      });
    });
  }

  /**
   * Cancela el timer de refresh automático.
   */
  private cancelRefreshTimer(): void {
    if (this.refreshTimerSubscription) {
      this.refreshTimerSubscription.unsubscribe();
      this.refreshTimerSubscription = null;
    }
  }

  /**
   * Limpia el estado de autenticación local.
   */
  private clearAuthState(): void {
    this._currentUser.set(null);
    this.tokenExpiresIn = 0;
    this.cancelRefreshTimer();
  }

  /**
   * Maneja errores de autenticación de forma segura.
   * No revela información sensible al usuario.
   */
  private handleAuthError(error: HttpErrorResponse): Error {
    let message = 'Error de autenticación. Por favor, intente nuevamente.';

    if (error.status === 401) {
      message = 'Credenciales inválidas.';
    } else if (error.status === 429) {
      message = 'Demasiados intentos. Por favor, espere antes de intentar nuevamente.';
    } else if (error.status === 403) {
      message = 'Acceso denegado.';
    } else if (error.status === 0) {
      message = 'Error de conexión. Verifique su conexión a internet.';
    }

    return new Error(message);
  }

  /**
   * Obtiene el token CSRF de la cookie.
   * Este token debe enviarse en el header X-XSRF-TOKEN para requests mutables.
   */
  getCsrfToken(): string | null {
    return this.getCookie('XSRF-TOKEN');
  }

  /**
   * Utilidad para leer cookies (solo las accesibles por JavaScript).
   */
  private getCookie(name: string): string | null {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? decodeURIComponent(match[2]) : null;
  }
}
