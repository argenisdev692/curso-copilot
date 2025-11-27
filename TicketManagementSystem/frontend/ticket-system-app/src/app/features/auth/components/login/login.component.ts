import { Component, OnInit, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { AuthState } from '../../../../core/authentication/state/auth.state';
import { AuthService } from '../../../../core/authentication/services/auth.service';
import { LoginRequest } from '../../../../shared/models/common/auth.interface';

/**
 * Login component for user authentication.
 * Handles user login with email and password.
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="min-h-screen flex items-center justify-center bg-linear-to-br from-indigo-600 via-purple-600 to-pink-600 animate-gradient-x relative overflow-hidden">
      <!-- Decorative background elements -->
      <div class="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHBhdHRlcm4gaWQ9ImdyaWQiIHdpZHRoPSI2MCIgaGVpZ2h0PSI2MCIgcGF0dGVyblVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+PHBhdGggZD0iTSAxMCAwIEwgMCAwIDAgMTAiIGZpbGw9Im5vbmUiIHN0cm9rZT0id2hpdGUiIHN0cm9rZS1vcGFjaXR5PSIwLjA1IiBzdHJva2Utd2lkdGg9IjEiLz48L3BhdHRlcm4+PC9kZWZzPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9InVybCgjZ3JpZCkiLz48L3N2Zz4=')] opacity-30"></div>
      <div class="absolute top-20 left-20 w-72 h-72 bg-purple-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob"></div>
      <div class="absolute bottom-20 right-20 w-72 h-72 bg-pink-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-2000"></div>
      <div class="absolute top-40 right-40 w-72 h-72 bg-indigo-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-4000"></div>

      <div class="relative backdrop-blur-2xl bg-white/10 border border-white/30 rounded-3xl p-10 w-full max-w-md shadow-2xl hover:shadow-purple-500/20 transition-all duration-500">
        <div class="text-center mb-10">
          <!-- Icon/Logo -->
          <div class="inline-flex items-center justify-center w-20 h-20 mb-6 bg-linear-to-br from-indigo-400 to-purple-500 rounded-2xl shadow-lg transform hover:scale-105 transition-transform duration-300">
            <svg class="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z"></path>
            </svg>
          </div>
          <h1 class="text-4xl font-bold text-white mb-3 tracking-tight">Bienvenido de Nuevo</h1>
          <p class="text-white/70 text-lg">Inicia sesión en tu cuenta</p>
        </div>

        <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" data-cy="login-form" class="space-y-6">
          <!-- Email Field -->
          <div>
            <label for="email" class="block text-sm font-medium text-white mb-2">Email</label>
            <div class="relative">
              <input
                id="email"
                type="email"
                formControlName="email"
                data-cy="email-input"
                placeholder="Ingresa tu correo electrónico"
                class="w-full px-4 py-3.5 pl-12 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-purple-400 focus:border-purple-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
                [class.border-red-400]="email?.invalid && email?.touched"
              />
              <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-white/60" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207"></path>
                </svg>
              </div>
            </div>
            <div *ngIf="email?.invalid && email?.touched" data-cy="email-error" class="mt-1 text-red-300 text-sm">
              <div *ngIf="email?.errors?.['required']">Email is required</div>
              <div *ngIf="email?.errors?.['email']">Please enter a valid email</div>
            </div>
          </div>

          <!-- Password Field -->
          <div>
            <label for="password" class="block text-sm font-medium text-white mb-2">Password</label>
            <div class="relative">
              <input
                id="password"
                [type]="showPassword ? 'text' : 'password'"
                formControlName="password"
                data-cy="password-input"
                placeholder="Ingresa tu contraseña"
                class="w-full px-4 py-3.5 pl-12 pr-12 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-purple-400 focus:border-purple-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
                [class.border-red-400]="password?.invalid && password?.touched"
              />
              <div class="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none">
                <svg class="h-5 w-5 text-white/60" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path>
                </svg>
              </div>
              <button
                type="button"
                (click)="togglePasswordVisibility()"
                class="absolute inset-y-0 right-0 pr-4 flex items-center text-white/60 hover:text-white transition-colors z-10"
              >
                <svg *ngIf="!showPassword" class="h-5 w-5 text-white/50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"></path>
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z"></path>
                </svg>
                <svg *ngIf="showPassword" class="h-5 w-5 text-white/50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.878 9.878L3 3m6.878 6.878L21 21"></path>
                </svg>
              </button>
            </div>
            <div *ngIf="password?.invalid && password?.touched" data-cy="password-error" class="mt-1 text-red-300 text-sm">
              <div *ngIf="password?.errors?.['required']">Password is required</div>
              <div *ngIf="password?.errors?.['minlength']">Password must be at least 6 characters</div>
            </div>
          </div>

          <!-- Success Message -->
          <div *ngIf="successMessage()" data-cy="success-message" class="bg-green-500/20 border border-green-400 text-green-300 px-4 py-3 rounded-lg animate-fade-in">
            {{ successMessage() }}
          </div>

          <!-- Error Message -->
          <div *ngIf="errorMessage()" data-cy="error-message" class="bg-red-500/20 border border-red-400 text-red-300 px-4 py-3 rounded-lg">
            {{ errorMessage() }}
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            data-cy="login-button"
            [disabled]="loginForm.invalid || loading()"
            class="w-full bg-linear-to-r from-indigo-500 via-purple-500 to-pink-500 hover:from-indigo-600 hover:via-purple-600 hover:to-pink-600 disabled:from-gray-500 disabled:to-gray-600 text-white font-bold py-4 px-6 rounded-xl transition-all duration-300 transform hover:scale-[1.02] hover:shadow-2xl hover:shadow-purple-500/50 disabled:transform-none disabled:cursor-not-allowed disabled:opacity-60 active:scale-95"
          >
            <span *ngIf="!loading()">Sign In</span>
            <span *ngIf="loading()" class="flex items-center justify-center">
              <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Signing In...
            </span>
          </button>
        </form>

        <div class="mt-8 text-center">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-white/20"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-4 bg-transparent text-white/60">o</span>
            </div>
          </div>
          <p class="mt-6 text-white/80">
            ¿No tienes una cuenta?
            <a routerLink="/auth/register" data-cy="register-link" class="text-white hover:text-purple-200 font-semibold transition-colors underline decoration-2 underline-offset-4 decoration-purple-400 hover:decoration-purple-200">Regístrate aquí</a>
          </p>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .animate-gradient-x {
      background-size: 400% 400%;
      animation: gradient-x 15s ease infinite;
    }

    @keyframes gradient-x {
      0%, 100% { background-position: 0% 50%; }
      50% { background-position: 100% 50%; }
    }

    .animate-fade-in {
      animation: fade-in 0.5s ease-in;
    }

    @keyframes fade-in {
      from { opacity: 0; transform: translateY(-10px); }
      to { opacity: 1; transform: translateY(0); }
    }

    .animate-blob {
      animation: blob 7s infinite;
    }

    @keyframes blob {
      0%, 100% { transform: translate(0, 0) scale(1); }
      33% { transform: translate(30px, -50px) scale(1.1); }
      66% { transform: translate(-20px, 20px) scale(0.9); }
    }

    .animation-delay-2000 {
      animation-delay: 2s;
    }

    .animation-delay-4000 {
      animation-delay: 4s;
    }
  `]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  showPassword = false;
  successMessage = signal<string | null>(null);

  // Reactive state from AuthState
  loading = this.authState.loading;
  errorMessage = this.authState.error;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private authState: AuthState,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
    // Check if user just registered
    this.route.queryParams.subscribe(params => {
      if (params['registered'] === 'true') {
        this.successMessage.set('✅ Registration successful! Please sign in with your credentials.');

        // Pre-fill email if provided
        if (params['email']) {
          this.loginForm.patchValue({ email: params['email'] });
        }

        // Clear query params after reading
        setTimeout(() => {
          this.router.navigate([], {
            relativeTo: this.route,
            queryParams: {},
            replaceUrl: true
          });
        }, 100);
      }
    });
  }

  get email() { return this.loginForm.get('email'); }
  get password() { return this.loginForm.get('password'); }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  async onSubmit() {
    if (this.loginForm.valid) {
      this.authState.setLoading(true);
      this.authState.setError(null);

      const credentials: LoginRequest = this.loginForm.value;

      console.log('🔐 Attempting login with:', credentials.email);

      try {
        const response = await this.authService.login(credentials);
        console.log('✅ Login successful:', response);
        console.log('🔍 Auth state after login:', {
          isAuthenticated: this.authState.isAuthenticated(),
          user: this.authState.currentUser()
        });

        // Use setTimeout to ensure state is fully updated before navigation
        setTimeout(() => {
          this.authState.setLoading(false);

          // Navigate to returnUrl or dashboard
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
          console.log('🚀 Navigating to:', returnUrl);
          console.log('🔍 Auth state before navigation:', {
            isAuthenticated: this.authState.isAuthenticated(),
            hasUser: !!this.authState.currentUser()
          });

          this.router.navigate([returnUrl]).then(
            success => {
              console.log('✅ Navigation result:', success);
              if (!success) {
                console.error('❌ Navigation was blocked. Auth state:', {
                  isAuthenticated: this.authState.isAuthenticated(),
                  user: this.authState.currentUser()
                });
              }
            },
            error => console.error('❌ Navigation failed with error:', error)
          );
        }, 100);
      } catch (error) {
        console.error('❌ Login failed:', error);
        this.authState.setLoading(false);
        this.authState.setError((error as any).message || 'Login failed');
      }
    } else {
      console.warn('⚠️ Form is invalid');
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched() {
    Object.keys(this.loginForm.controls).forEach(key => {
      const control = this.loginForm.get(key);
      control?.markAsTouched();
    });
  }
}
