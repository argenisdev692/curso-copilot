---
description: 'Genera tests E2E completos para Angular usando Cypress o Playwright'
---

# Generador de Tests E2E Frontend - Cypress & Playwright

## 🎯 Propósito
Generar tests end-to-end (E2E) listos para producción que simulen interacciones reales de usuarios y validen flujos completos del sistema.

## 📋 Detección de Framework

Auto-detectar framework desde estructura de proyecto:
- **Cypress**: Carpeta `cypress/` existe, `cypress.config.ts` presente
- **Playwright**: `playwright.config.ts` presente, carpeta `e2e/` o `tests/`
- **Protractor** (legacy): `protractor.conf.js` presente (migrar a Cypress/Playwright)

Por defecto usar **Cypress** si es ambiguo (recomendado para 2025).

## 🧪 Principios de Tests E2E

### Qué Testear en E2E

**✅ Incluir:**
- Flujos críticos de usuario (login, registro, checkout, features core)
- Happy paths de journeys importantes
- Escenarios de error relevantes (inputs inválidos, fallos de red simulados)
- Navegación entre páginas (routing, redirects, navegación back/forward)
- Interacciones de formularios (input, validación, submission)
- Integración con API real o mockeada
- Estados de UI (loading states, mensajes de error, feedback de éxito)
- Flujos de autorización y permisos

**❌ No incluir:**
- Lógica a nivel unitario (usar tests unitarios Jasmine/Jest)
- Componentes individuales aislados (usar component tests)
- Detalles de estilos CSS precisos
- Testing exhaustivo de compatibilidad de browsers (solo smoke tests)

## 📐 Estándares de Estructura

### Convención de Nomenclatura

**describe()**: Nombre de feature o página
- Ejemplos: `'User Authentication'`, `'Ticket Management'`, `'Dashboard - Admin View'`

**it()**: Formato `should [acción] when/with [condición]`
- ✅ `'should login successfully with valid credentials'`
- ✅ `'should display error message when credentials are invalid'`
- ✅ `'should redirect to login when accessing protected route unauthorized'`
- ❌ `'test login'`, `'check if works'`, `'login test 1'`

### Organización de Tests

**Por Feature**: Agrupar tests por funcionalidad de negocio
**Por Flujo de Usuario**: Agrupar tests por user journey completo
**Nesting Lógico**: Usar `describe` anidados para sub-features

## 🔧 Para Cypress

### Selectors

**Best Practice**: Usar `data-cy` attributes
- Selectores estables e independientes de CSS/estructura
- No afectados por cambios de estilos
- Formato: `data-cy="login-button"`, `data-cy="ticket-title-input"`

**Evitar:**
- Classes CSS (pueden cambiar con estilos)
- IDs dinámicos generados por frameworks
- Texto que puede cambiar o ser traducido

### Comandos Principales

- `cy.visit()` - Navegar a URL
- `cy.get('[data-cy="selector"]')` - Obtener elemento
- `cy.contains()` - Buscar por texto visible
- `cy.type()` - Ingresar texto en input
- `cy.click()` - Click en elemento
- `cy.should()` - Assertion
- `cy.intercept()` - Interceptar y mockear requests HTTP
- `cy.wait()` - Esperar alias de request interceptado

### Assertions

- `.should('be.visible')` - Elemento es visible
- `.should('have.text', 'Expected')` - Texto exacto
- `.should('contain', 'Partial')` - Texto parcial
- `.should('have.class', 'active')` - Clase CSS presente
- `.should('have.value', 'input')` - Valor de input
- `.should('have.length', 5)` - Cantidad de elementos

### Network Mocking

- Interceptar requests: `cy.intercept('GET', '/api/tickets', fixture('tickets.json'))`
- Simular errores: `cy.intercept('POST', '/api/tickets', { statusCode: 500, body: error })`
- Verificar requests: `cy.wait('@ticketsRequest')`
- Delay responses: `cy.intercept('GET', '/api/data', { delay: 1000, body: data })`

### Custom Commands

Crear comandos reutilizables en `cypress/support/commands.ts`:
- Login flows repetitivos
- Setup de datos comunes
- Assertions complejas reutilizables

## 🎭 Para Playwright

### Selectors

**Best Practice**: Usar `data-testid` attributes
- Auto-waiting built-in para la mayoría de acciones
- Formato: `data-testid="login-button"`

**Selectores Semánticos**:
- `page.getByRole()` - Por role ARIA (button, link, heading)
- `page.getByText()` - Por texto visible
- `page.getByLabel()` - Por label asociado
- `page.getByPlaceholder()` - Por placeholder

### Comandos Principales

- `page.goto()` - Navegar a URL
- `page.locator('[data-testid="selector"]')` - Obtener elemento
- `page.getByRole('button', { name: 'Submit' })` - Selector semántico
- `page.fill()` - Llenar input
- `page.click()` - Click en elemento
- `expect()` - Assertions con `@playwright/test`
- `page.route()` - Interceptar y mockear requests

### Assertions

- `await expect(locator).toBeVisible()` - Visible
- `await expect(locator).toHaveText('Expected')` - Texto
- `await expect(locator).toContainText('Partial')` - Texto parcial
- `await expect(locator).toHaveClass(/active/)` - Clase
- `await expect(locator).toHaveValue('input')` - Valor

### Network Mocking

- `await page.route('/api/tickets', route => route.fulfill({ body: data }))`
- Simular errores: `route.fulfill({ status: 500, body: error })`
- Esperar requests: `await page.waitForResponse('/api/tickets')`

## 📋 Checklist de Tests E2E

Para cada suite de tests E2E, verificar:

- [ ] Flujos críticos de usuario cubiertos
- [ ] Happy path testeado
- [ ] Error cases relevantes incluidos
- [ ] Selectors estables (`data-cy` o `data-testid`)
- [ ] Naming descriptivo de tests
- [ ] Network mocking para APIs externas
- [ ] No hay waits hardcodeados innecesarios
- [ ] Tests independientes (no orden dependiente)
- [ ] Cleanup después de cada test
- [ ] Assertions específicas (no solo "existe")
- [ ] Manejo de estados de loading
- [ ] Navegación entre páginas validada

## 🎯 Formato de Prompt para Copilot

```
Genera tests E2E completos para la siguiente funcionalidad:

**Framework**: [Cypress / Playwright]
**Feature**: [nombre de la funcionalidad]

**User Journey a testear:**
1. [Paso 1 del flujo]
2. [Paso 2 del flujo]
3. [Paso 3 del flujo]

**Escenarios requeridos:**
- Happy path: [descripción del flujo exitoso]
- Error handling: [qué errores validar]
- Edge cases: [casos límite o especiales]

**Configuración:**
- Base URL: [url del frontend]
- API endpoints a mockear: [lista de endpoints]
- Datos de test: [usuarios, fixtures necesarios]

**Selectors:**
- Usar [data-cy / data-testid] para elementos principales
- Identificar elementos por: [button text, labels, etc.]

**Salida esperada:**
- Suite de tests E2E completa
- Setup de fixtures si necesario
- Network mocking configurado
- Assertions específicas y relevantes
- Tests independientes y ejecutables

Funcionalidad: [descripción o #file]
```

## 📝 Consideraciones Especiales

### Performance de Tests E2E
- Paralelizar cuando sea posible
- Mockear llamadas a APIs lentas
- Usar fixtures en lugar de setup complejo
- Cache de login states

### Flakiness (Tests Intermitentes)
- Evitar waits hardcodeados (usar auto-waiting)
- Selectores estables
- Manejar race conditions
- Retry logic para operaciones de red

### Data Management
- Reset de base de datos entre tests
- Fixtures para datos consistentes
- Evitar dependencia de datos en ambientes compartidos
- Cleanup después de cada test

### Debugging
- Screenshots automáticos en fallos
- Videos de ejecución
- Trace files (Playwright)
- Cypress Dashboard para análisis

## 🚫 Anti-Patterns a Evitar

- **NO usar waits hardcodeados**: `cy.wait(5000)` (usar auto-waiting)
- **NO selectores frágiles**: CSS classes que cambian
- **NO tests dependientes**: Cada test debe ser independiente
- **NO assertions vagas**: Verificar estado específico
- **NO sobre-testear**: E2E para flujos críticos, no todo
- **NO ignorar flakiness**: Investigar y corregir tests intermitentes
