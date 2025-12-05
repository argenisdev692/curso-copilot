import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '@core/services';
import { UserRole } from '@core/models';

/**
 * Guard factory that checks if user has required role
 * @param allowedRoles Roles that are allowed to access the route
 */
export const roleGuard = (allowedRoles: UserRole[]): CanActivateFn => {
  return () => {
    const authService = inject(AuthService);
    const router = inject(Router);

    const user = authService.currentUser();

    if (!user) {
      router.navigate(['/auth/login']);
      return false;
    }

    // Handle optional role - if role is undefined, deny access
    if (user.role && allowedRoles.includes(user.role)) {
      return true;
    }

    router.navigate(['/unauthorized']);
    return false;
  };
};

/**
 * Admin only guard
 */
export const adminGuard: CanActivateFn = roleGuard([UserRole.Admin]);
