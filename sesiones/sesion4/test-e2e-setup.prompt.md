---
description: 'Setup y configuración de frameworks de testing E2E (Cypress o Playwright) para proyectos Angular'
---

# Guía de Setup de Framework E2E Testing

## 🎯 Propósito
Proveer instrucciones completas de setup y configuración para frameworks de testing E2E (Cypress o Playwright) en proyectos Angular.

## 🔍 Selección de Framework

### Cypress vs Playwright - Comparación

| **Feature** | **Cypress** | **Playwright** |
|-------------|-------------|----------------|
| **Curva de Aprendizaje** | Fácil | Moderada |
| **Velocidad** | Rápido | Más rápido |
| **Browser Support** | Chrome, Firefox, Edge | Chrome, Firefox, Safari, Edge |
| **Ejecución Paralela** | Pago (Dashboard) | Gratis (built-in) |
| **Time Travel Debugging** | ✅ Sí | ❌ No |
| **Network Stubbing** | ✅ Excelente | ✅ Bueno |
| **Auto-waiting** | ✅ Sí | ✅ Sí |
| **Comunidad** | Grande | Creciendo |
| **Mejor Para** | Angular, React, Vue | Testing cross-browser |

**Recomendación 2025**: Cypress para Angular (mejor DX, docs excelentes)

---

## 🚀 Setup Cypress (Recomendado)

### 1. Instalación

Instalar packages:
```bash
npm install --save-dev cypress
npm install --save-dev @cypress/webpack-preprocessor ts-loader
npm install --save-dev @testing-library/cypress  # Opcional
```

### 2. Configuración Cypress

Crear `cypress.config.ts` con:

**Configuraciones Principales**:
- `baseUrl`: URL base de la app (ej: `http://localhost:4200`)
- `viewportWidth/Height`: Resolución de browser
- `defaultCommandTimeout`: Timeout para comandos
- `video`: Grabar videos de tests
- `screenshotOnRunFailure`: Screenshots en fallos
- `e2e.specPattern`: Pattern de archivos de test
- `e2e.setupNodeEvents`: Setup de plugins

**Configuraciones Recomendadas**:
- `video: false` (para desarrollo local)
- `screenshotOnRunFailure: true`
- `baseUrl: 'http://localhost:4200'`
- `viewportWidth: 1280, viewportHeight: 720`
- `defaultCommandTimeout: 10000`

### 3. Scripts de Package.json

Agregar scripts:
```json
{
  "scripts": {
    "cy:open": "cypress open",
    "cy:run": "cypress run",
    "cy:run:chrome": "cypress run --browser chrome",
    "cy:run:headless": "cypress run --headless"
  }
}
```

### 4. Estructura de Carpetas

Crear estructura:
```
cypress/
├── e2e/
│   ├── auth/
│   │   └── login.cy.ts
│   └── tickets/
│       └── ticket-management.cy.ts
├── fixtures/
│   └── test-data.json
├── support/
│   ├── commands.ts
│   └── e2e.ts
└── downloads/
```

### 5. Custom Commands

En `cypress/support/commands.ts`, crear commands reutilizables:

**Commands útiles**:
- `cy.login(username, password)` - Login automático
- `cy.logout()` - Logout y limpiar session
- `cy.seedDatabase()` - Seed data para tests
- `cy.clearDatabase()` - Limpiar DB después de tests
- `cy.getByDataCy(selector)` - Shortcut para data-cy

### 6. TypeScript Support

Crear `cypress/tsconfig.json`:
```json
{
  "extends": "../tsconfig.json",
  "compilerOptions": {
    "types": ["cypress", "@testing-library/cypress"]
  }
}
```

### 7. Environment Variables

Configurar en `cypress.config.ts`:
- `env.apiUrl`: URL del backend
- `env.testUser`: Credenciales de usuario de test
- `env.testAdmin`: Credenciales de admin de test

---

## 🎭 Setup Playwright

### 1. Instalación

Instalar Playwright:
```bash
npm install --save-dev @playwright/test
npx playwright install  # Instala browsers
```

### 2. Configuración Playwright

Crear `playwright.config.ts` con:

**Configuraciones Principales**:
- `baseURL`: URL base de la app
- `testDir`: Directorio de tests
- `use.headless`: Modo headless o con UI
- `use.screenshot`: Captura de screenshots
- `use.video`: Grabación de videos
- `projects`: Configuración multi-browser
- `webServer`: Auto-start del dev server

**Configuraciones Recomendadas**:
- `baseURL: 'http://localhost:4200'`
- `testDir: './e2e'`
- `fullyParallel: true`
- `retries: 2` (solo en CI)
- `workers: 4` (ejecución paralela)

### 3. Scripts de Package.json

Agregar scripts:
```json
{
  "scripts": {
    "pw:test": "playwright test",
    "pw:test:headed": "playwright test --headed",
    "pw:test:debug": "playwright test --debug",
    "pw:test:ui": "playwright test --ui",
    "pw:report": "playwright show-report"
  }
}
```

### 4. Estructura de Carpetas

Crear estructura:
```
e2e/
├── auth/
│   └── login.spec.ts
├── tickets/
│   └── ticket-management.spec.ts
└── fixtures/
    └── test-data.ts
```

### 5. Page Object Model

Crear POMs en `e2e/pages/`:
- `LoginPage.ts` - Página de login
- `DashboardPage.ts` - Página principal
- `TicketPage.ts` - Página de tickets

### 6. Fixtures y Test Data

Crear fixtures reutilizables para:
- Authentication state
- Database seed data
- Mock API responses

---

## 🔧 Configuración Angular para E2E

### 1. Data Attributes para Testing

Agregar en componentes Angular:
```
<button [attr.data-cy]="'login-button'" 
        [attr.data-testid]="'login-button'">
  Login
</button>
```

### 2. API Mocking

**Cypress**: Usar `cy.intercept()`
**Playwright**: Usar `page.route()`

### 3. Test Database

Configurar base de datos separada para tests:
- SQLite en memoria (rápido)
- Docker container (aislado)
- DB dedicada de test (persistente)

### 4. CI/CD Integration

**GitHub Actions** - ejemplo workflow:
```yaml
- name: Run E2E Tests
  run: |
    npm run start &
    npm run cy:run
```

**Variables de entorno**:
- `CI=true` - Detectar ambiente CI
- `BASE_URL` - URL de staging/test
- `API_URL` - URL del backend

---

## 📊 Best Practices

### General
1. **Usar selectores estables**: `data-cy`, `data-testid`, no CSS classes
2. **Tests independientes**: Cada test debe poder ejecutarse solo
3. **Limpiar estado**: Reset DB/storage entre tests
4. **Auto-waiting**: Confiar en auto-waiting del framework
5. **Assertions específicas**: Verificar texto, clases, estados

### Performance
1. **Paralelización**: Ejecutar tests en paralelo cuando sea posible
2. **Mock APIs**: Mockear APIs externas para velocidad
3. **Seed data**: Pre-cargar data en lugar de crearla en cada test
4. **Selectivo**: No testear todo E2E, solo flows críticos

### Mantenibilidad
1. **Page Objects**: Encapsular lógica de página
2. **Custom commands**: Reutilizar acciones comunes
3. **Fixtures**: Centralizar test data
4. **DRY**: No repetir setup en cada test

---

## ✅ Checklist de Setup Completo

Verificar que el setup incluya:

- [ ] Framework instalado (Cypress o Playwright)
- [ ] Archivo de configuración creado
- [ ] Scripts en package.json agregados
- [ ] Estructura de carpetas establecida
- [ ] TypeScript configurado
- [ ] Custom commands/helpers creados
- [ ] Data attributes agregados en componentes
- [ ] Environment variables configuradas
- [ ] CI/CD workflow configurado (opcional)
- [ ] Test database configurada
- [ ] Documentación de setup en README

## 🎯 Salida Final

Generar setup completo con:

1. **Archivo de configuración**: `cypress.config.ts` o `playwright.config.ts`
2. **Scripts package.json**: Para ejecutar tests
3. **Estructura de carpetas**: Organizada y escalable
4. **Custom commands**: Helpers reutilizables
5. **Ejemplos de tests**: 2-3 tests de ejemplo funcionando
6. **Documentación**: Sección en README con instrucciones de uso
7. **CI/CD template**: Workflow básico de GitHub Actions

**Tiempo estimado de setup**: 30-45 minutos

---

**Listo para configurar framework E2E con GitHub Copilot siguiendo estas instrucciones.**
