import { Component, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/authentication/services/auth.service';
import { RegisterRequest } from '../../../../shared/models/common/auth.interface';

/**
 * Register component for new user registration.
 * Handles user sign-up with full name, email, and password.
 */
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="min-h-screen flex items-center justify-center bg-linear-to-br from-emerald-600 via-teal-600 to-cyan-600 animate-gradient-x relative overflow-hidden py-12">
      <!-- Decorative background elements -->
      <div class="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PGRlZnM+PHBhdHRlcm4gaWQ9ImdyaWQiIHdpZHRoPSI2MCIgaGVpZ2h0PSI2MCIgcGF0dGVyblVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+PHBhdGggZD0iTSAxMCAwIEwgMCAwIDAgMTAiIGZpbGw9Im5vbmUiIHN0cm9rZT0id2hpdGUiIHN0cm9rZS1vcGFjaXR5PSIwLjA1IiBzdHJva2Utd2lkdGg9IjEiLz48L3BhdHRlcm4+PC9kZWZzPjxyZWN0IHdpZHRoPSIxMDAlIiBoZWlnaHQ9IjEwMCUiIGZpbGw9InVybCgjZ3JpZCkiLz48L3N2Zz4=')] opacity-30"></div>
      <div class="absolute top-20 left-20 w-72 h-72 bg-teal-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob"></div>
      <div class="absolute bottom-20 right-20 w-72 h-72 bg-cyan-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-2000"></div>
      <div class="absolute top-40 right-40 w-72 h-72 bg-emerald-400 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob animation-delay-4000"></div>

      <div class="relative backdrop-blur-2xl bg-white/10 border border-white/30 rounded-3xl p-10 w-full max-w-md shadow-2xl hover:shadow-emerald-500/20 transition-all duration-500 max-h-[90vh] overflow-y-auto custom-scrollbar">
        <div class="text-center mb-8">
          <!-- Icon/Logo -->
          <div class="inline-flex items-center justify-center w-20 h-20 mb-6 bg-linear-to-br from-emerald-400 to-teal-500 rounded-2xl shadow-lg transform hover:scale-105 transition-transform duration-300">
            <svg class="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18 9v3m0 0v3m0-3h3m-3 0h-3m-2-5a4 4 0 11-8 0 4 4 0 018 0zM3 20a6 6 0 0112 0v1H3v-1z"></path>
            </svg>
          </div>
          <h1 class="text-4xl font-bold text-white mb-3 tracking-tight">Crear Cuenta</h1>
          <p class="text-white/70 text-lg">Únete a nuestro sistema de tickets</p>
        </div>

        <form [formGroup]="registerForm" (ngSubmit)="onSubmit()" class="space-y-5">
          <!-- Full Name Field -->
          <div>
            <label for="fullName" class="block text-sm font-medium text-white mb-2">Full Name</label>
            <input
              id="fullName"
              type="text"
              formControlName="fullName"
              data-cy="name-input"
              placeholder="Ingresa tu nombre completo"
              class="w-full px-4 py-3.5 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-teal-400 focus:border-teal-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
              [class.border-red-400]="fullName?.invalid && fullName?.touched"
            />
            <div *ngIf="fullName?.invalid && fullName?.touched" class="mt-1 text-red-300 text-sm">
              <div *ngIf="fullName?.errors?.['required']">Full name is required</div>
              <div *ngIf="fullName?.errors?.['maxlength']">Full name must be less than 100 characters</div>
            </div>
          </div>

          <!-- Email Field -->
          <div>
            <label for="email" class="block text-sm font-medium text-white mb-2">Email</label>
            <input
              id="email"
              type="email"
              formControlName="email"
              data-cy="email-input"
              placeholder="Ingresa tu correo electrónico"
              class="w-full px-4 py-3.5 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-teal-400 focus:border-teal-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
              [class.border-red-400]="email?.invalid && email?.touched"
            />
            <div *ngIf="email?.invalid && email?.touched" class="mt-1 text-red-300 text-sm">
              <div *ngIf="email?.errors?.['required']">Email is required</div>
              <div *ngIf="email?.errors?.['email']">Please enter a valid email</div>
            </div>
          </div>

          <!-- Role Field -->
          <div>
            <label for="role" class="block text-sm font-medium text-white mb-2">Role</label>
            <select
              id="role"
              formControlName="role"
              class="w-full px-4 py-3.5 bg-white/10 border border-white/30 rounded-xl text-white focus:outline-none focus:ring-2 focus:ring-teal-400 focus:border-teal-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner cursor-pointer"
            >
              <option value="User" class="bg-gray-800">User</option>
              <option value="Agent" class="bg-gray-800">Agent</option>
              <option value="Admin" class="bg-gray-800">Admin</option>
            </select>
          </div>

          <!-- Password Field -->
          <div>
            <label for="password" class="block text-sm font-medium text-white mb-2">Password</label>
            <input
              id="password"
              type="password"
              formControlName="password"
              data-cy="password-input"
              placeholder="Ingresa tu contraseña"
              class="w-full px-4 py-3.5 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-teal-400 focus:border-teal-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
              [class.border-red-400]="password?.invalid && password?.touched"
            />
            <!-- Password Strength Indicator -->
            <div *ngIf="password?.value" class="mt-2">
              <div class="flex space-x-1">
                <div [class]="getStrengthClass(1)" class="h-2 flex-1 rounded"></div>
                <div [class]="getStrengthClass(2)" class="h-2 flex-1 rounded"></div>
                <div [class]="getStrengthClass(3)" class="h-2 flex-1 rounded"></div>
                <div [class]="getStrengthClass(4)" class="h-2 flex-1 rounded"></div>
                <div [class]="getStrengthClass(5)" class="h-2 flex-1 rounded"></div>
              </div>
              <p class="text-xs mt-1" [class]="getStrengthTextClass()">
                {{ getStrengthText() }}
              </p>
            </div>
            <div *ngIf="password?.invalid && password?.touched" class="mt-1 text-red-300 text-sm">
              <div *ngIf="password?.errors?.['required']">Password is required</div>
              <div *ngIf="password?.errors?.['minlength']">Password must be at least 8 characters</div>
              <div *ngIf="password?.errors?.['maxlength']">Password cannot exceed 128 characters</div>
              <div *ngIf="password?.errors?.['pattern']">Password must contain uppercase, lowercase, number and special character (&#64;, #, $, etc.)</div>
            </div>
            <!-- Password Requirements Helper -->
            <div class="mt-2 text-xs text-white/60">
              <p>Password must contain:</p>
              <ul class="list-disc list-inside ml-2">
                <li>At least 8 characters</li>
                <li>Uppercase letter (A-Z)</li>
                <li>Lowercase letter (a-z)</li>
                <li>Number (0-9)</li>
                <li>Special character (&#64;, #, $, %, etc.)</li>
              </ul>
            </div>
          </div>

          <!-- Confirm Password Field -->
          <div>
            <label for="confirmPassword" class="block text-sm font-medium text-white mb-2">Confirm Password</label>
            <input
              id="confirmPassword"
              type="password"
              formControlName="confirmPassword"
              data-cy="confirm-password-input"
              placeholder="Confirma tu contraseña"
              class="w-full px-4 py-3.5 bg-white/10 border border-white/30 rounded-xl text-white placeholder-white/50 focus:outline-none focus:ring-2 focus:ring-teal-400 focus:border-teal-400 focus:bg-white/15 hover:bg-white/15 transition-all duration-300 shadow-inner"
              [class.border-red-400]="confirmPassword?.invalid && confirmPassword?.touched"
            />
            <div *ngIf="confirmPassword?.invalid && confirmPassword?.touched" data-cy="password-error" class="mt-1 text-red-300 text-sm">
              <div *ngIf="confirmPassword?.errors?.['required']">Please confirm your password</div>
              <div *ngIf="confirmPassword?.errors?.['passwordMismatch']">Passwords do not match</div>
            </div>
          </div>

          <!-- Error Message -->
          <div *ngIf="errorMessage()" data-cy="error-message" class="bg-red-500/20 border border-red-400 text-red-300 px-4 py-3 rounded-lg">
            {{ errorMessage() }}
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            data-cy="register-button"
            [disabled]="registerForm.invalid || loading()"
            class="w-full bg-linear-to-r from-emerald-500 via-teal-500 to-cyan-500 hover:from-emerald-600 hover:via-teal-600 hover:to-cyan-600 disabled:from-gray-500 disabled:to-gray-600 text-white font-bold py-4 px-6 rounded-xl transition-all duration-300 transform hover:scale-[1.02] hover:shadow-2xl hover:shadow-teal-500/50 disabled:transform-none disabled:cursor-not-allowed disabled:opacity-60 active:scale-95"
          >
            <span *ngIf="!loading()">Create Account</span>
            <span *ngIf="loading()" class="flex items-center justify-center">
              <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Creating Account...
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
            ¿Ya tienes una cuenta?
            <a routerLink="/auth/login" class="text-white hover:text-teal-200 font-semibold transition-colors underline decoration-2 underline-offset-4 decoration-teal-400 hover:decoration-teal-200">Inicia sesión aquí</a>
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

    .custom-scrollbar::-webkit-scrollbar {
      width: 8px;
    }

    .custom-scrollbar::-webkit-scrollbar-track {
      background: rgba(255, 255, 255, 0.05);
      border-radius: 10px;
    }

    .custom-scrollbar::-webkit-scrollbar-thumb {
      background: rgba(255, 255, 255, 0.2);
      border-radius: 10px;
    }

    .custom-scrollbar::-webkit-scrollbar-thumb:hover {
      background: rgba(255, 255, 255, 0.3);
    }
  `]
})
export class RegisterComponent {
  registerForm: FormGroup;
  loading = signal(false);
  errorMessage = signal('');

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      role: ['User', Validators.required],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(128),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])/)  // Requiere: minúscula, mayúscula, número y carácter especial
      ]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  get fullName() { return this.registerForm.get('fullName'); }
  get email() { return this.registerForm.get('email'); }
  get role() { return this.registerForm.get('role'); }
  get password() { return this.registerForm.get('password'); }
  get confirmPassword() { return this.registerForm.get('confirmPassword'); }

  passwordMatchValidator(control: AbstractControl) {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }

    return null;
  }

  getStrengthClass(level: number): string {
    const password = this.password?.value || '';
    const strength = this.calculatePasswordStrength(password);

    if (level <= strength) {
      if (strength === 1) return 'bg-red-500';
      if (strength === 2) return 'bg-orange-500';
      if (strength === 3) return 'bg-yellow-500';
      if (strength === 4) return 'bg-lime-500';
      return 'bg-green-500';
    }
    return 'bg-gray-600';
  }

  getStrengthText(): string {
    const password = this.password?.value || '';
    const strength = this.calculatePasswordStrength(password);

    switch (strength) {
      case 1: return 'Very Weak';
      case 2: return 'Weak';
      case 3: return 'Moderate';
      case 4: return 'Good';
      case 5: return 'Strong';
      default: return '';
    }
  }

  getStrengthTextClass(): string {
    const password = this.password?.value || '';
    const strength = this.calculatePasswordStrength(password);

    switch (strength) {
      case 1: return 'text-red-400';
      case 2: return 'text-orange-400';
      case 3: return 'text-yellow-400';
      case 4: return 'text-lime-400';
      case 5: return 'text-green-400';
      default: return 'text-gray-400';
    }
  }

  private calculatePasswordStrength(password: string): number {
    let strength = 0;
    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/\d/.test(password)) strength++;
    if (/[\W_]/.test(password)) strength++;  // Caracteres especiales
    return strength;
  }

  async onSubmit() {
    if (this.registerForm.valid) {
      this.loading.set(true);
      this.errorMessage.set('');

      const userData: RegisterRequest = {
        email: this.registerForm.value.email,
        password: this.registerForm.value.password,
        fullName: this.registerForm.value.fullName,
        role: this.registerForm.value.role
      };

      console.log('📝 Attempting registration:', userData.email);

      try {
        const response = await this.authService.register(userData);
        console.log('✅ Registration successful:', response);
        this.loading.set(false);

        // Navigate to login page
        console.log('🚀 Navigating to login page');
        this.router.navigate(['/auth/login'], {
          queryParams: { registered: 'true', email: userData.email }
        }).then(
          success => console.log('✅ Navigation successful:', success),
          error => console.error('❌ Navigation failed:', error)
        );
      } catch (error) {
        console.error('❌ Registration failed:', error);
        this.loading.set(false);

        // Extract error message from different possible formats
        let errorMsg = 'Registration failed';
        if ((error as any)?.error?.detail) {
          errorMsg = (error as any).error.detail; // ProblemDetails format
        } else if ((error as any)?.message) {
          errorMsg = (error as any).message;
        } else if ((error as any)?.error?.message) {
          errorMsg = (error as any).error.message;
        }

        this.errorMessage.set(errorMsg);
      }
    } else {
      console.warn('⚠️ Form is invalid');
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched() {
    Object.keys(this.registerForm.controls).forEach(key => {
      const control = this.registerForm.get(key);
      control?.markAsTouched();
    });
  }
}
