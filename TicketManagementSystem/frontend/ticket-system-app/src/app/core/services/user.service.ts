import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError, tap, finalize } from 'rxjs/operators';
import { User } from '../../shared/models/common/user.interface';

/**
 * Standalone service for user management operations
 * Implements CRUD operations with reactive state management using Angular signals
 * Follows SOLID principles and includes security best practices
 */
@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/users';

  // Reactive state signals (private for internal use, expose readonly versions)
  private readonly usersSignal = signal<User[]>([]);
  private readonly isLoadingSignal = signal<boolean>(false);
  private readonly errorSignal = signal<string | null>(null);
  private readonly selectedUserSignal = signal<User | null>(null);

  // Public readonly signals for components to subscribe
  readonly users = this.usersSignal.asReadonly();
  readonly isLoading = this.isLoadingSignal.asReadonly();
  readonly error = this.errorSignal.asReadonly();
  readonly selectedUser = this.selectedUserSignal.asReadonly();

  /**
   * Get all users with reactive state updates
   * @returns Observable of User array
   */
  getUsers(): Observable<User[]> {
    this.setLoading(true);
    this.clearError();

    return this.http.get<User[]>(this.apiUrl, { headers: this.getAuthHeaders() }).pipe(
      map(users => users.map(user => this.transformUserDates(user))),
      tap(users => this.usersSignal.set(users)),
      catchError(error => this.handleError(error)),
      finalize(() => this.setLoading(false))
    );
  }

  /**
   * Get a specific user by ID
   * @param id User ID
   * @returns Observable of User
   */
  getUser(id: number): Observable<User> {
    if (!id || id <= 0) {
      return throwError(() => new Error('Invalid user ID'));
    }

    this.setLoading(true);
    this.clearError();

    return this.http.get<User>(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() }).pipe(
      map(user => this.transformUserDates(user)),
      tap(user => this.selectedUserSignal.set(user)),
      catchError(error => this.handleError(error)),
      finalize(() => this.setLoading(false))
    );
  }

  /**
   * Create a new user
   * @param userData User data without ID and timestamps
   * @returns Observable of created User
   */
  createUser(userData: Omit<User, 'id' | 'createdAt' | 'updatedAt'>): Observable<User> {
    if (!this.validateUserData(userData)) {
      return throwError(() => new Error('Invalid user data'));
    }

    this.setLoading(true);
    this.clearError();

    return this.http.post<User>(this.apiUrl, userData, { headers: this.getAuthHeaders() }).pipe(
      map(user => this.transformUserDates(user)),
      tap(newUser => {
        // Add to local state
        const currentUsers = this.usersSignal();
        this.usersSignal.set([...currentUsers, newUser]);
      }),
      catchError(error => this.handleError(error)),
      finalize(() => this.setLoading(false))
    );
  }

  /**
   * Update an existing user
   * @param id User ID
   * @param userData Partial user data to update
   * @returns Observable of updated User
   */
  updateUser(id: number, userData: Partial<Omit<User, 'id' | 'createdAt' | 'updatedAt'>>): Observable<User> {
    if (!id || id <= 0) {
      return throwError(() => new Error('Invalid user ID'));
    }

    this.setLoading(true);
    this.clearError();

    return this.http.put<User>(`${this.apiUrl}/${id}`, userData, { headers: this.getAuthHeaders() }).pipe(
      map(user => this.transformUserDates(user)),
      tap(updatedUser => {
        // Update in local state
        const currentUsers = this.usersSignal();
        const index = currentUsers.findIndex(u => u.id === id);
        if (index !== -1) {
          const updatedUsers = [...currentUsers];
          updatedUsers[index] = updatedUser;
          this.usersSignal.set(updatedUsers);
        }
        // Update selected user if it's the same
        if (this.selectedUserSignal()?.id === id) {
          this.selectedUserSignal.set(updatedUser);
        }
      }),
      catchError(error => this.handleError(error)),
      finalize(() => this.setLoading(false))
    );
  }

  /**
   * Delete a user
   * @param id User ID to delete
   * @returns Observable of void
   */
  deleteUser(id: number): Observable<void> {
    if (!id || id <= 0) {
      return throwError(() => new Error('Invalid user ID'));
    }

    this.setLoading(true);
    this.clearError();

    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getAuthHeaders() }).pipe(
      tap(() => {
        // Remove from local state
        const currentUsers = this.usersSignal();
        this.usersSignal.set(currentUsers.filter(u => u.id !== id));
        // Clear selected user if it's the deleted one
        if (this.selectedUserSignal()?.id === id) {
          this.selectedUserSignal.set(null);
        }
      }),
      catchError(error => this.handleError(error)),
      finalize(() => this.setLoading(false))
    );
  }

  /**
   * Clear all users from local state
   */
  clearUsers(): void {
    this.usersSignal.set([]);
    this.selectedUserSignal.set(null);
    this.clearError();
  }

  /**
   * Clear error state
   */
  clearError(): void {
    this.errorSignal.set(null);
  }

  /**
   * Get authentication headers with JWT token
   * @returns HttpHeaders with authorization
   */
  private getAuthHeaders(): HttpHeaders {
    const token = this.getAuthToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      ...(token && { 'Authorization': `Bearer ${token}` })
    });
  }

  /**
   * Get authentication token from storage
   * @returns JWT token or null
   */
  private getAuthToken(): string | null {
    try {
      return localStorage.getItem('authToken');
    } catch {
      return null;
    }
  }

  /**
   * Transform date strings to Date objects
   * @param user User object with date strings
   * @returns User with Date objects
   */
  private transformUserDates(user: any): User {
    return {
      ...user,
      createdAt: new Date(user.createdAt),
      updatedAt: new Date(user.updatedAt)
    };
  }

  /**
   * Validate user data before sending to server
   * @param userData User data to validate
   * @returns True if valid
   */
  private validateUserData(userData: Omit<User, 'id' | 'createdAt' | 'updatedAt'>): boolean {
    return !!(
      userData.email &&
      userData.fullName &&
      userData.role &&
      ['Admin', 'Agent', 'User'].includes(userData.role) &&
      typeof userData.isActive === 'boolean'
    );
  }

  /**
   * Set loading state
   * @param loading Loading state
   */
  private setLoading(loading: boolean): void {
    this.isLoadingSignal.set(loading);
  }

  /**
   * Handle HTTP errors
   * @param error HTTP error response
   * @returns Observable that throws error
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 401) {
        errorMessage = 'Unauthorized access. Please login again.';
        // Could trigger logout here
      } else if (error.status === 403) {
        errorMessage = 'Forbidden access.';
      } else if (error.status === 404) {
        errorMessage = 'Resource not found.';
      } else if (error.error?.message) {
        errorMessage = error.error.message;
      } else {
        errorMessage = `Server error: ${error.status}`;
      }
    }

    this.errorSignal.set(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
