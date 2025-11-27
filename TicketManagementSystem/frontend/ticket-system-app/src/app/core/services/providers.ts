import { AuthService } from '../authentication/services/auth.service';
import { DashboardService } from './dashboard.service';
import { provideLocalStorageService } from './storage.service';

/**
 * Functional provider for AuthService
 */
export function provideAuthService(): {
  provide: typeof AuthService;
  useFactory: () => AuthService;
} {
  return {
    provide: AuthService,
    useFactory: () => new AuthService()
  };
}

/**
 * Functional provider for DashboardService
 */
export function provideDashboardService(): {
  provide: typeof DashboardService;
  useFactory: () => DashboardService;
} {
  return {
    provide: DashboardService,
    useFactory: () => new DashboardService()
  };
}

/**
 * Functional provider for StorageService
 */
export const provideStorageService = provideLocalStorageService;
