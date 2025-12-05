# BookingSystem Frontend

Frontend Angular para el sistema de reservas de salas.

## Tecnologías

- **Angular 21** (standalone components)
- **Angular Material** (componentes UI)
- **Tailwind CSS** (utilities: spacing, flex, grid)
- **Jest** (unit testing)
- **Cypress** (E2E testing)
- **ESLint + Prettier** (linting y formateo)

## Estructura del Proyecto

```
src/app/
├── core/                    # Servicios singleton, guards, interceptors
│   ├── constants/           # Constantes de la aplicación
│   ├── guards/              # Route guards (auth, role)
│   ├── interceptors/        # HTTP interceptors
│   ├── models/              # Interfaces y tipos
│   └── services/            # Servicios (auth, room, booking)
├── shared/                  # Componentes y utilidades compartidas
│   ├── components/          # Componentes reutilizables
│   ├── directives/          # Directivas personalizadas
│   ├── pipes/               # Pipes personalizados
│   └── validators/          # Validadores de formularios
├── features/                # Módulos de features (lazy loaded)
│   ├── auth/                # Login, Register
│   ├── dashboard/           # Dashboard principal
│   ├── rooms/               # Gestión de salas
│   └── bookings/            # Gestión de reservas
├── layouts/                 # Layouts de la aplicación
│   ├── main-layout/         # Layout principal con sidebar
│   └── auth-layout/         # Layout para autenticación
└── environments/            # Configuración por ambiente
```

## Scripts Disponibles

```bash
# Desarrollo
npm start                 # Iniciar servidor de desarrollo (con proxy)
npm run build             # Build de desarrollo
npm run build:prod        # Build de producción

# Testing
npm test                  # Ejecutar tests unitarios
npm run test:watch        # Tests en modo watch
npm run test:coverage     # Tests con cobertura

# E2E Testing
npm run e2e               # Ejecutar tests E2E
npm run e2e:open          # Abrir Cypress UI

# Linting y Formateo
npm run lint              # Ejecutar ESLint
npm run lint:fix          # Corregir errores de ESLint
npm run format            # Formatear código con Prettier
npm run format:check      # Verificar formato
```

## Path Aliases

Configurados en `tsconfig.json`:

```typescript
import { AuthService } from '@core/services';
import { TruncatePipe } from '@shared/pipes';
import { MainLayoutComponent } from '@layouts/main-layout';
import { environment } from '@environments/environment';
```

## Configuración del Proxy

El archivo `proxy.conf.json` redirige las llamadas `/api` al backend:

```json
{
  "/api": {
    "target": "http://localhost:5000",
    "secure": false,
    "changeOrigin": true
  }
}
```

## Convenciones

### Componentes
- Usar **standalone components**
- Usar **OnPush change detection**
- Usar **signals** para estado reactivo
- Usar **inject()** en lugar de constructor injection

### Estilos
- **Angular Material**: Componentes UI principales
- **Tailwind CSS**: Utilities (spacing, flexbox, grid)
- **SCSS**: Estilos específicos de componentes

### Testing
- **Jest**: Unit tests con coverage mínimo del 70%
- **Cypress**: E2E tests con comandos personalizados
- Naming: `componente.component.spec.ts`

## Environments

- `environment.ts`: Desarrollo
- `environment.prod.ts`: Producción

## Próximos Pasos

1. Implementar componentes de features (auth, dashboard, rooms, bookings)
2. Configurar CI/CD
3. Añadir internacionalización (i18n)
4. Implementar PWA
