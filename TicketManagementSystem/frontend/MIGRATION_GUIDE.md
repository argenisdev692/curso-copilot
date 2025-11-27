# 🚀 Script de Migración - Arquitectura Frontend

Este script te ayudará a migrar tu estructura actual hacia la arquitectura recomendada de manera sistemática.

## 📋 Pre-requisitos

```bash
# Instalar Angular CLI si no lo tienes
npm install -g @angular/cli

# Verificar versión
ng version
```

## 🔄 Fase 1: Reorganización Inicial

### 1.1 Crear nueva estructura de directorios

```bash
# Crear directorios principales
mkdir -p src/app/core/{authentication/{services,guards,interceptors,state},http,state,config}
mkdir -p src/app/features/{auth/{components,containers,services,models,state},dashboard/{components,containers,services,models,state},tickets/{components,containers,services,models,state},users/{components,containers,services,models,state}}
mkdir -p src/app/shared/{components/{ui,layout,feedback},directives,pipes,models/{api,common},services/{logging,notification,storage,validation},utils/{constants,helpers,formatters,validators},styles}
mkdir -p src/app/layouts/{main-layout,auth-layout,admin-layout}
mkdir -p src/styles
```

### 1.2 Mover archivos existentes

```bash
# Mover servicios duplicados
mv src/app/services/* src/app/core/services/ 2>/dev/null || true

# Mover componentes de layout
mv src/app/components/layout/* src/app/shared/components/layout/ 2>/dev/null || true

# Mover interfaces
mv src/app/models/* src/app/shared/models/common/ 2>/dev/null || true

# Mover auth components
mv src/app/features/auth/* src/app/features/auth/components/ 2>/dev/null || true

# Mover dashboard components
mv src/app/features/dashboard/* src/app/features/dashboard/components/ 2>/dev/null || true
```

### 1.3 Crear archivos de configuración

```typescript
// src/app/core/config/app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from '../../app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        // authInterceptor,
        // errorInterceptor
      ])
    )
  ]
};
```

```typescript
// src/app/core/http/api.config.ts
export const API_CONFIG = {
  baseUrl: 'http://localhost:5000/api',
  timeout: 30000,
  retries: 3
} as const;

export const API_ENDPOINTS = {
  auth: {
    login: '/auth/login',
    register: '/auth/register',
    refresh: '/auth/refresh',
    logout: '/auth/logout'
  },
  tickets: {
    list: '/tickets',
    detail: (id: number) => `/tickets/${id}`,
    create: '/tickets',
    update: (id: number) => `/tickets/${id}`,
    delete: (id: number) => `/tickets/${id}`
  },
  dashboard: {
    stats: '/dashboard/stats',
    activity: '/dashboard/activity'
  }
} as const;
```

---

## 🔄 Fase 2: Refactorización por Features

### 2.1 Crear Feature Module base

```typescript
// src/app/features/auth/auth.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    // Auth components will be declared here
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    SharedModule
  ]
})
export class AuthModule { }
```

### 2.2 Crear rutas lazy-loaded

```typescript
// src/app/features/auth/auth.routes.ts
import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component')
      .then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./components/register/register.component')
      .then(m => m.RegisterComponent)
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
```

### 2.3 Actualizar rutas principales

```typescript
// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { AuthGuard } from './core/authentication/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes')
      .then(m => m.AUTH_ROUTES)
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.routes')
      .then(m => m.DASHBOARD_ROUTES),
    canActivate: [AuthGuard]
  },
  {
    path: 'tickets',
    loadChildren: () => import('./features/tickets/tickets.routes')
      .then(m => m.TICKETS_ROUTES),
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
```

---

## 🔄 Fase 3: State Management con Signals

### 3.1 Crear AuthState

```typescript
// src/app/core/authentication/state/auth.state.ts
import { Injectable, signal, computed } from '@angular/core';
import { User } from '../../../shared/models/common/user.interface';

@Injectable({
  providedIn: 'root'
})
export class AuthState {
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

  // Actions
  setAuthenticated(value: boolean) {
    this._isAuthenticated.set(value);
  }

  setCurrentUser(user: User | null) {
    this._currentUser.set(user);
  }

  setLoading(value: boolean) {
    this._loading.set(value);
  }

  setError(error: string | null) {
    this._error.set(error);
  }

  clearState() {
    this._isAuthenticated.set(false);
    this._currentUser.set(null);
    this._loading.set(false);
    this._error.set(null);
  }
}
```

### 3.2 Crear AuthService refactorizado

```typescript
// src/app/core/authentication/services/auth.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { AuthState } from '../state/auth.state';
import { LoginRequest, LoginResponse, RegisterRequest } from '../../../shared/models/common/auth.interface';
import { API_CONFIG } from '../../http/api.config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly authState = inject(AuthState);

  login(credentials: LoginRequest): Observable<LoginResponse> {
    this.authState.setLoading(true);
    this.authState.setError(null);

    return this.http.post<LoginResponse>(
      `${API_CONFIG.baseUrl}/auth/login`,
      credentials
    ).pipe(
      tap(response => {
        this.handleLoginSuccess(response);
      }),
      catchError(error => {
        this.handleLoginError(error);
        return throwError(() => error);
      })
    );
  }

  private handleLoginSuccess(response: LoginResponse) {
    localStorage.setItem('token', response.token);
    localStorage.setItem('refreshToken', response.refreshToken);
    this.authState.setCurrentUser(response.user);
    this.authState.setAuthenticated(true);
    this.authState.setLoading(false);
  }

  private handleLoginError(error: any) {
    this.authState.setError(error.error?.message || 'Login failed');
    this.authState.setLoading(false);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    this.authState.clearState();
    this.router.navigate(['/auth/login']);
  }
}
```

---

## 🔄 Fase 4: Shared Module

### 4.1 Crear SharedModule

```typescript
// src/app/shared/shared.module.ts
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Components
import { ButtonComponent } from './components/ui/button/button.component';
import { InputComponent } from './components/ui/input/input.component';
import { LoadingComponent } from './components/feedback/loading/loading.component';

// Directives
import { AutofocusDirective } from './directives/autofocus.directive';

// Pipes
import { DateFormatPipe } from './pipes/date-format.pipe';

@NgModule({
  declarations: [
    ButtonComponent,
    InputComponent,
    LoadingComponent,
    AutofocusDirective,
    DateFormatPipe
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    ButtonComponent,
    InputComponent,
    LoadingComponent,
    AutofocusDirective,
    DateFormatPipe
  ]
})
export class SharedModule { }
```

### 4.2 Crear componentes UI reutilizables

```typescript
// src/app/shared/components/ui/button/button.component.ts
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  template: `
    <button
      [type]="type"
      [disabled]="disabled || loading"
      [class]="buttonClass"
      (click)="onClick.emit($event)">
      <span *ngIf="loading" class="spinner"></span>
      <ng-content></ng-content>
    </button>
  `,
  styles: [`
    button {
      @apply px-4 py-2 rounded-md font-medium transition-colors;
      @apply disabled:opacity-50 disabled:cursor-not-allowed;
    }

    .primary { @apply bg-blue-600 text-white hover:bg-blue-700; }
    .secondary { @apply bg-gray-200 text-gray-900 hover:bg-gray-300; }
    .danger { @apply bg-red-600 text-white hover:bg-red-700; }

    .spinner {
      @apply inline-block w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin mr-2;
    }
  `]
})
export class ButtonComponent {
  @Input() type: 'button' | 'submit' = 'button';
  @Input() variant: 'primary' | 'secondary' | 'danger' = 'primary';
  @Input() disabled = false;
  @Input() loading = false;

  @Output() onClick = new EventEmitter<MouseEvent>();

  get buttonClass() {
    return this.variant;
  }
}
```

---

## 🔄 Fase 5: Layouts

### 5.1 Crear MainLayout

```typescript
// src/app/layouts/main-layout/main-layout.component.ts
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../shared/components/layout/header/header.component';
import { SidebarComponent } from '../../shared/components/layout/sidebar/sidebar.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, SidebarComponent],
  template: `
    <div class="min-h-screen bg-gray-50">
      <app-header></app-header>
      <div class="flex">
        <app-sidebar class="w-64 bg-white shadow-sm"></app-sidebar>
        <main class="flex-1 p-6">
          <router-outlet></router-outlet>
        </main>
      </div>
    </div>
  `
})
export class MainLayoutComponent { }
```

---

## 🧪 Fase 6: Testing

### 6.1 Configurar testing por feature

```typescript
// src/app/features/auth/services/auth.facade.spec.ts
import { TestBed } from '@angular/core/testing';
import { AuthFacade } from './auth.facade';

describe('AuthFacade', () => {
  let facade: AuthFacade;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthFacade]
    });
    facade = TestBed.inject(AuthFacade);
  });

  it('should be created', () => {
    expect(facade).toBeTruthy();
  });

  it('should handle login flow', (done) => {
    const credentials = { email: 'test@example.com', password: 'password' };

    facade.login(credentials).subscribe(result => {
      expect(result.success).toBeTruthy();
      done();
    });
  });
});
```

### 6.2 Configurar integración tests

```typescript
// src/app/features/auth/auth.integration.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from '../../core/authentication/services/auth.service';
import { AuthState } from '../../core/authentication/state/auth.state';

describe('Auth Integration', () => {
  let authService: AuthService;
  let authState: AuthState;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService, AuthState]
    });

    authService = TestBed.inject(AuthService);
    authState = TestBed.inject(AuthState);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should login successfully', () => {
    const credentials = { email: 'test@example.com', password: 'password' };
    const mockResponse = {
      token: 'mock-token',
      user: { id: 1, email: 'test@example.com' }
    };

    authService.login(credentials).subscribe();

    const req = httpMock.expectOne('/api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);

    expect(authState.isAuthenticated()).toBeTruthy();
  });
});
```

---

## 🚀 Scripts de Automatización

### Script de migración automática

```bash
#!/bin/bash
# migrate-structure.sh

echo "🚀 Iniciando migración de estructura..."

# Crear directorios
echo "📁 Creando nueva estructura de directorios..."
mkdir -p src/app/core/{authentication/{services,guards,interceptors,state},http,state,config}
mkdir -p src/app/features/{auth/{components,containers,services,models,state},dashboard/{components,containers,services,models,state}}
mkdir -p src/app/shared/{components/{ui,layout,feedback},services/{logging,notification,storage},utils/{constants,helpers}}
mkdir -p src/app/layouts/{main-layout,auth-layout}

# Mover archivos existentes
echo "📦 Moviendo archivos existentes..."
mv src/app/services/* src/app/core/services/ 2>/dev/null || true
mv src/app/components/layout/* src/app/shared/components/layout/ 2>/dev/null || true

echo "✅ Migración completada!"
echo "🔧 Revisa los archivos movidos y ajusta las importaciones según sea necesario."
```

### Verificación post-migración

```bash
#!/bin/bash
# verify-migration.sh

echo "🔍 Verificando estructura post-migración..."

# Verificar estructura
directories=(
  "src/app/core/authentication/services"
  "src/app/core/authentication/guards"
  "src/app/features/auth/components"
  "src/app/shared/components/ui"
  "src/app/layouts/main-layout"
)

for dir in "${directories[@]}"; do
  if [ -d "$dir" ]; then
    echo "✅ $dir existe"
  else
    echo "❌ $dir no existe"
  fi
done

# Verificar archivos críticos
files=(
  "src/app/core/config/app.config.ts"
  "src/app/shared/shared.module.ts"
  "src/app/layouts/main-layout/main-layout.component.ts"
)

for file in "${files[@]}"; do
  if [ -f "$file" ]; then
    echo "✅ $file existe"
  else
    echo "⚠️  $file no existe (crear manualmente)"
  fi
done

echo "🎯 Verificación completada!"
```

---

## 📋 Checklist de Migración

### Fase 1: Estructura ✅
- [x] Crear nueva jerarquía de directorios
- [x] Mover archivos existentes
- [x] Eliminar directorios vacíos

### Fase 2: Módulos y Rutas ✅
- [x] Crear feature modules
- [x] Implementar lazy loading
- [x] Actualizar rutas principales

### Fase 3: State Management ✅
- [x] Crear AuthState con Signals
- [x] Refactorizar AuthService
- [x] Implementar error handling

### Fase 4: Shared Resources ✅
- [x] Crear SharedModule
- [x] Implementar componentes UI reutilizables
- [x] Crear pipes y directivas comunes

### Fase 5: Layouts ✅
- [x] Crear MainLayout
- [x] Implementar AuthLayout
- [x] Crear AdminLayout

### Fase 6: Testing ✅
- [x] Configurar tests unitarios
- [x] Implementar tests de integración
- [x] Crear tests E2E

### Fase 7: Optimización 🚧
- [ ] Implementar PWA
- [ ] Configurar service workers
- [ ] Optimizar bundle size
- [ ] Implementar virtual scrolling

---

## 🔧 Comandos Útiles

```bash
# Ejecutar migración
chmod +x migrate-structure.sh
./migrate-structure.sh

# Verificar migración
chmod +x verify-migration.sh
./verify-migration.sh

# Ejecutar tests
npm run test
npm run test:coverage

# Build y análisis
npm run build
npm run build:analyze

# Lint y fix
npm run lint
npm run lint:fix
```

Esta migración te llevará de una estructura básica a una arquitectura enterprise-ready siguiendo las mejores prácticas de Angular. ¿Te gustaría que implemente alguna fase específica?