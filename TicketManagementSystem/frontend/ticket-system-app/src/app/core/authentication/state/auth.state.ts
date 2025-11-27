import { Injectable, signal, computed } from '@angular/core';
import { User } from '../../../shared/models/common/user.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthState {
  // Private signals
  private readonly _isAuthenticated = signal(false);
  private readonly _currentUser = signal<User | null>(null);
  private readonly _loading = signal(false);
  private readonly _error = signal<string | null>(null);

  // Public readonly signals
  readonly isAuthenticated = this._isAuthenticated.asReadonly();
  readonly currentUser = this._currentUser.asReadonly();
  readonly loading = this._loading.asReadonly();
  readonly error = this._error.asReadonly();

  // Computed signals
  readonly userName = computed(() =>
    this._currentUser()?.fullName || ''
  );

  readonly userRole = computed(() =>
    this._currentUser()?.role || null
  );

  readonly userInitials = computed(() => {
    const user = this._currentUser();
    if (!user?.fullName) return '';
    return user.fullName
      .split(' ')
      .map((name: string) => name.charAt(0))
      .join('')
      .toUpperCase()
      .slice(0, 2);
  });

  readonly isAdmin = computed(() =>
    this._currentUser()?.role === 'Admin'
  );

  readonly isUser = computed(() =>
    this._currentUser()?.role === 'User'
  );

  // Actions
  setAuthenticated(value: boolean): void {
    this._isAuthenticated.set(value);
  }

  setCurrentUser(user: User | null): void {
    this._currentUser.set(user);
  }

  setLoading(value: boolean): void {
    this._loading.set(value);
  }

  setError(error: string | null): void {
    this._error.set(error);
  }

  updateUser(updates: Partial<User>): void {
    const currentUser = this._currentUser();
    if (currentUser) {
      this._currentUser.set({ ...currentUser, ...updates });
    }
  }

  clearState(): void {
    this._isAuthenticated.set(false);
    this._currentUser.set(null);
    this._loading.set(false);
    this._error.set(null);
  }

  // Utility methods
  hasRole(role: string): boolean {
    return this._currentUser()?.role === role;
  }

  hasAnyRole(roles: string[]): boolean {
    const userRole = this._currentUser()?.role;
    return userRole ? roles.includes(userRole) : false;
  }
}
