import { ApplicationConfig, provideZoneChangeDetection, ErrorHandler, isDevMode } from '@angular/core';
import { provideRouter, withPreloading, PreloadAllModules } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { authInterceptor } from './core/authentication/interceptors/auth.interceptor';
import { GlobalErrorHandler } from './core/http/error-handler';
import { provideServiceWorker } from '@angular/service-worker';
import { provideAuthService, provideDashboardService } from './core/services/providers';
import { provideLocalStorageService } from './core/services/storage.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withPreloading(PreloadAllModules)),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideLocalStorageService(),
    provideAuthService(),
    provideDashboardService(),
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    }, provideServiceWorker('ngsw-worker.js', {
            enabled: !isDevMode(),
            registrationStrategy: 'registerWhenStable:30000'
          })
  ]
};
