import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthState } from '../authentication/state/auth.state';

/**
 * Auth Guard to protect authenticated routes
 * Redirects to login if user is not authenticated
 */
export const AuthGuard: CanActivateFn = (_route, state) => {
  const authState = inject(AuthState);
  const router = inject(Router);

  const isAuthenticated = authState.isAuthenticated();
  const currentUser = authState.currentUser();

  console.log('🛡️ AuthGuard checking:', {
    url: state.url,
    isAuthenticated,
    hasUser: !!currentUser,
    user: currentUser
  });

  if (isAuthenticated) {
    console.log('✅ AuthGuard: Access granted to', state.url);
    return true;
  }

  console.warn('⛔ AuthGuard: Access denied. Redirecting to login');
  
  // Store the attempted URL for redirecting after login
  const returnUrl = state.url;
  router.navigate(['/auth/login'], { queryParams: { returnUrl } });
  return false;
};
