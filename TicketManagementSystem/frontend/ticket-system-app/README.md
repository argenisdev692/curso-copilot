# 🎫 TicketManagementSystem - Professional Angular Application

[![Angular](https://img.shields.io/badge/Angular-19.2.0-red.svg)](https://angular.io/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.7.2-blue.svg)](https://www.typescriptlang.org/)
[![ESLint](https://img.shields.io/badge/ESLint-9.0.0-purple.svg)](https://eslint.org/)
[![Prettier](https://img.shields.io/badge/Prettier-3.3.0-pink.svg)](https://prettier.io/)
[![Testing](https://img.shields.io/badge/Testing-Jasmine/Karma-green.svg)](https://jasmine.github.io/)
[![CI/CD](https://img.shields.io/badge/CI/CD-GitHub%20Actions-orange.svg)](https://github.com/features/actions)

Una aplicación Angular enterprise-level que implementa las mejores prácticas profesionales de desarrollo, siguiendo Clean Architecture, Feature-Driven Development y principios SOLID.

## ✨ Características Principales

### 🏗️ Arquitectura Moderna
- **Clean Architecture** con separación clara de capas
- **Feature-Driven Development** con módulos autocontenidos
- **Angular Signals** para estado reactivo
- **Standalone Components** con lazy loading

### 🔒 Seguridad y Calidad
- **TypeScript Strict Mode** al máximo nivel
- **ESLint + Prettier** con reglas estrictas
- **Content Security Policy** y headers de seguridad
- **Validación robusta** en cliente y servidor
- **Manejo de errores** centralizado y estructurado

### ⚡ Performance Optimizada
- **OnPush Change Detection** en todos los componentes
- **Lazy Loading** de rutas y módulos
- **Virtual Scrolling** para listas grandes
- **Bundle Analysis** y monitoreo de tamaño
- **Web Vitals** tracking

### ♿ Accesibilidad (WCAG 2.1 AA)
- **Navegación por teclado** completa
- **Screen Reader Support** con ARIA labels
- **Color Contrast** que cumple estándares
- **Focus Management** inteligente
- **Skip Links** para navegación rápida

### 🧪 Testing Completo
- **Unit Tests** con Jasmine/Karma
- **Integration Tests** preparados
- **E2E Tests** configurados para Cypress
- **Coverage mínimo** del 80%
- **CI/CD** con tests automatizados

## 🚀 Inicio Rápido

### Prerrequisitos
- Node.js 20+
- npm 10+
- Angular CLI 19+

### Instalación

```bash
# Clonar el repositorio
git clone <repository-url>
cd ticket-system-app

# Instalar dependencias
npm install

# Inicializar Husky hooks
npm run prepare

# Iniciar servidor de desarrollo
npm start
```

La aplicación estará disponible en `http://localhost:4200/`

## 📜 Scripts Disponibles

```bash
# Desarrollo
npm start                    # Servidor de desarrollo con HMR
npm run build               # Build de producción
npm run watch               # Build con watch mode

# Calidad de código
npm run lint                # Ejecutar ESLint
npm run lint:fix            # Corregir errores automáticamente
npm run format              # Formatear código con Prettier
npm run format:check        # Verificar formato del código
npm run type-check          # Verificación de tipos TypeScript

# Testing
npm test                    # Tests unitarios con Karma
npm run test:ci             # Tests para CI (sin watch)
npm run test:coverage       # Tests con reporte de cobertura

# Performance
npm run analyze             # Análisis de bundle con Webpack Bundle Analyzer
npm run build:prod          # Build optimizado para producción

# Utilidades
npm run security:audit      # Auditoría de seguridad de dependencias
npm run pre-commit          # Validación manual pre-commit

# Documentación
npm run compodoc            # Generar documentación técnica
npm run compodoc:serve      # Servir documentación localmente

```

## 🏛️ Arquitectura del Proyecto

```
src/app/
├── core/                    # Capa core (inmutable)
│   ├── authentication/     # Sistema de autenticación
│   ├── http/               # Configuración HTTP
│   ├── state/              # Estado global reactivo
│   ├── services/           # Servicios core
│   ├── guards/             # Guards de navegación
│   ├── config/             # Configuración global
│   ├── security/           # Utilidades de seguridad
│   ├── performance/        # Utilidades de performance
│   └── accessibility/      # Utilidades de accesibilidad
├── features/               # Features autocontenidas
│   ├── auth/              # Autenticación
│   ├── dashboard/         # Dashboard principal
│   ├── tickets/           # Gestión de tickets
│   └── users/             # Gestión de usuarios
├── shared/                 # Componentes compartidos
│   ├── components/        # UI components
│   ├── directives/        # Directivas custom
│   ├── pipes/             # Pipes custom
│   ├── models/            # Modelos comunes
│   ├── services/          # Servicios compartidos
│   └── utils/             # Utilidades
├── layouts/                # Layouts de aplicación
│   ├── main-layout/       # Layout principal
│   └── auth-layout/       # Layout de autenticación
└── app.config.ts          # Configuración principal
```

## 🔧 Configuración de Desarrollo

### VSCode Settings
El proyecto incluye configuración automática de VSCode para:
- Formateo automático al guardar
- Fix automático de ESLint
- Import sorting automático
- TypeScript strict mode

### Pre-commit Hooks
Husky configura hooks automáticos para:
- Linting de código modificado
- Verificación de tipos
- Formateo de código
- Tests básicos

### CI/CD Pipeline
GitHub Actions incluye:
- ✅ Linting automático
- ✅ Tests unitarios
- ✅ Build verification
- ✅ Bundle size monitoring
- ✅ Accessibility testing
- 🚀 Deploy automático a staging/production

## 📊 Métricas de Calidad

### Coverage de Tests
```bash
Statements   : 80% (min)
Branches     : 80% (min)
Functions    : 80% (min)
Lines        : 80% (min)
```

### Performance Budgets
- **Bundle Size**: < 500KB (gzipped)
- **First Contentful Paint**: < 1.5s
- **Largest Contentful Paint**: < 2.5s
- **Cumulative Layout Shift**: < 0.1

### Accessibility Standards
- ✅ **WCAG 2.1 AA** Compliance
- ✅ **Color Contrast** ≥ 4.5:1
- ✅ **Keyboard Navigation**
- ✅ **Screen Reader Support**

## 🔒 Seguridad

### Implementado
- **Content Security Policy** (CSP)
- **XSS Prevention** con sanitización
- **CSRF Protection** con tokens
- **Secure Headers** (HSTS, X-Frame-Options, etc.)
- **Input Validation** en cliente y servidor
- **JWT Token Security** con refresh automático

### Mejores Prácticas
- Nunca loggear datos sensibles
- Validación en múltiples capas
- HTTPS obligatorio en producción
- Auditorías de seguridad regulares

## ♿ Accesibilidad

### Características Implementadas
- **Skip Links** para navegación rápida
- **ARIA Labels** descriptivos
- **Focus Management** inteligente
- **Keyboard Navigation** completa
- **Screen Reader** announcements
- **High Contrast** mode support
- **Reduced Motion** preferences

### Testing de Accesibilidad
- Tests automatizados con axe-core
- Verificación de color contrast
- Navegación por teclado validada
- Screen reader testing

## 🧪 Testing Strategy

### Unit Tests
```typescript
// Ejemplo de test con TestBed
describe('AuthService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthService]
    });
    service = TestBed.inject(AuthService);
  });

  it('should login user', (done) => {
    // Test implementation
  });
});
```

### Integration Tests
- Componentes con dependencias reales
- Servicios con mocks inteligentes
- Formularios con validación completa

### E2E Tests (Preparado)
```typescript
// Ejemplo con Cypress
describe('Authentication', () => {
  it('should login user', () => {
    cy.visit('/auth/login');
    cy.get('[data-cy=email]').type('user@example.com');
    cy.get('[data-cy=password]').type('password');
    cy.get('[data-cy=login-btn]').click();
    cy.url().should('include', '/dashboard');
  });
});
```

## 🚀 Despliegue

### Staging
```bash
npm run build
# Deploy to staging environment
```

### Production
```bash
npm run build:prod
# Deploy to production with optimizations
```

### Docker (Opcional)
```dockerfile
FROM nginx:alpine
COPY dist/ticket-system-app /usr/share/nginx/html
EXPOSE 80
```

## 📚 Documentación Adicional

- [🏗️ Arquitectura Detallada](PROFESSIONAL_BEST_PRACTICES.md)
- [🔒 Guía de Seguridad](docs/SECURITY.md)
- [♿ Guía de Accesibilidad](docs/ACCESSIBILITY.md)
- [🚀 Guía de Despliegue](docs/DEPLOYMENT.md)
- [🧪 Guía de Testing](docs/TESTING.md)

## 🤝 Contribución

1. Fork el proyecto
2. Crea una feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la branch (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Estándares de Contribución
- ✅ Código pasa todos los linting rules
- ✅ Tests incluidos para nuevas features
- ✅ Documentación actualizada
- ✅ Commits siguen conventional commits
- ✅ PR incluye descripción detallada

## 📝 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 👥 Equipo

- **Desarrollador Principal**: [Tu Nombre]
- **Arquitectura**: Clean Architecture + Feature-Driven
- **Stack**: Angular 19 + TypeScript 5.7 + Tailwind CSS

---

**⭐ Si este proyecto te resulta útil, por favor dale una estrella en GitHub!**

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
