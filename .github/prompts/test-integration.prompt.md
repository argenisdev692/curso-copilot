---
description: 'Genera tests de integración para validar interacción entre múltiples capas del sistema'
---

# Generador de Tests de Integración - Backend y Frontend

## 🎯 Propósito
Generar tests de integración que validen la correcta comunicación entre componentes, capas y servicios externos, verificando el comportamiento del sistema como un conjunto integrado.

## 🔍 Diferencia con Otros Tests

### Tests Unitarios vs Integration vs E2E

| **Aspecto** | **Unit Tests** | **Integration Tests** | **E2E Tests** |
|-------------|----------------|------------------------|---------------|
| **Scope** | Función/clase aislada | Múltiples componentes | Sistema completo |
| **Mocking** | Todo excepto SUT | Solo externos | Mínimo o nada |
| **Speed** | Muy rápido (<100ms) | Moderado (100ms-2s) | Lento (2s-30s+) |
| **DB** | Mockeada | In-memory o test DB | DB real o staging |
| **HTTP** | Mockeado | Test server real | Backend real |
| **Objetivo** | Lógica interna | Integración capas | User journey |

## 🔧 Tests de Integración - Backend (.NET)

### Qué Testear

**✅ Incluir:**
- Controller → Service → Repository → Database real
- Autenticación y autorización completa
- Validación de DTOs y ModelState
- Serialización/deserialización JSON
- Manejo de errores end-to-end
- Transacciones de base de datos
- Middleware pipeline completo

**❌ No incluir:**
- Lógica de negocio aislada (unit tests)
- UI rendering (frontend tests)
- Servicios externos reales (mockear si son lentos/caros)

### WebApplicationFactory

**Setup Base:**
- Crear `CustomWebApplicationFactory<Program>` que:
  - Configure base de datos de test (In-Memory o Testcontainers)
  - Override configuraciones (appsettings.Test.json)
  - Mockee servicios externos si necesario
  - Configure autenticación de test

**Ventajas:**
- Test server HTTP real en memoria
- Configuración completa de DI
- Middleware pipeline real
- No puerto HTTP externo

### Base de Datos de Test

**Opciones:**

1. **In-Memory Database (SQLite)**
   - Pros: Rápido, fácil setup
   - Cons: Limitaciones de compatibilidad con SQL Server features

2. **Testcontainers**
   - Pros: Base de datos real (SQL Server, PostgreSQL), fidelidad total
   - Cons: Requiere Docker, más lento

**Recomendación:** In-Memory para CI/CD rápido, Testcontainers para casos complejos

### Patrón de Test

**Estructura:**
1. **Arrange**: Setup de datos en DB + HttpClient configurado
2. **Act**: Request HTTP real al endpoint
3. **Assert**: Validar status code + response body + estado de DB

### Autenticación en Tests

**JWT Mock:**
- Crear helper para generar JWTs de test
- Configurar roles/claims según escenario
- Incluir en headers: `Authorization: Bearer {token}`

**Alternative:** Usar `TestAuthHandler` que valida sin JWT real

## 🎨 Tests de Integración - Frontend (Angular)

### Qué Testear

**✅ Incluir:**
- Component + Service + HttpClient juntos
- Routing y navegación
- Forms con validación completa
- Guards y resolvers
- Interceptors (auth, error handling)
- State management (NgRx, Signals) con efectos reales

**❌ No incluir:**
- Backend real (mockear HTTP con HttpTestingController)
- DOM rendering detallado (component tests)
- E2E user flows completos (E2E tests)

### TestBed Completo

**Setup:**
- Importar módulos completos (no shallow)
- Configurar RouterTestingModule con rutas
- Proveer servicios reales (no mocks cuando integras)
- HttpClientTestingModule para mockear HTTP

### HttpTestingController

**Uso:**
- Mockear responses de API
- Verificar requests (URL, método, body, headers)
- Simular errores de red
- Controlar timing de respuestas

### Forms y Validación

**Testear:**
- Validadores síncronos y asíncronos
- Cross-field validation
- Error messages rendering
- Submit habilitado/deshabilitado correctamente

### Routing

**Testear:**
- Navegación programática funciona
- Guards permiten/previenen navegación
- Resolvers cargan datos antes de activar ruta
- Lazy loading de módulos

## 📐 Estructura de Tests de Integración

### Naming Convention

**Backend (.NET):**
- `[Feature]IntegrationTests.cs`
- `Should[Action]_When[Condition]`
- Ejemplos: 
  - `TicketsControllerIntegrationTests.cs`
  - `ShouldCreateTicket_WhenDataIsValid`

**Frontend (Angular):**
- `[feature].integration.spec.ts`
- `should [action] when [condition]`
- Ejemplos:
  - `ticket-form.integration.spec.ts`
  - `should create ticket and navigate when form is valid`

### Organización

**Backend:**
- Carpeta `tests/IntegrationTests/`
- Subcarpetas por feature/controller
- Base class compartida con setup común

**Frontend:**
- Junto a features en `*.integration.spec.ts`
- Separado de `*.spec.ts` (unit tests)

## 📋 Checklist de Integration Tests

Para cada suite de integration tests, verificar:

- [ ] **Backend:**
  - [ ] WebApplicationFactory configurado
  - [ ] Base de datos de test (in-memory o container)
  - [ ] HttpClient con base URL correcta
  - [ ] Autenticación/autorización testeada
  - [ ] Validación de DTOs verificada
  - [ ] Estado de DB validado después de operaciones
  - [ ] Cleanup de datos entre tests

- [ ] **Frontend:**
  - [ ] TestBed con módulos completos
  - [ ] HttpTestingController para mockear API
  - [ ] Routing configurado
  - [ ] Forms con validación completa
  - [ ] Guards/interceptors incluidos
  - [ ] No dependencia de servicios externos reales

- [ ] **General:**
  - [ ] Tests independientes (no orden)
  - [ ] Naming descriptivo
  - [ ] Assertions específicas
  - [ ] Performance aceptable (<2s por test)

## 🎯 Formato de Prompt para Copilot

### Backend (.NET)

```
Genera tests de integración para el siguiente endpoint:

**Endpoint**: [HTTP method] /api/[resource]
**Controller**: [ControllerName]
**Feature**: [descripción de la funcionalidad]

**Escenarios a testear:**
- Happy path: [flujo exitoso con datos válidos]
- Validación: [inputs inválidos esperados]
- Autorización: [roles/permisos requeridos]
- Edge cases: [casos límite]

**Configuración:**
- Usar WebApplicationFactory<Program>
- Base de datos: [In-Memory SQLite / Testcontainers]
- Autenticación: [JWT con roles X, Y]

**Salida esperada:**
- Tests con Arrange-Act-Assert claros
- Setup de datos de test en DB
- Validación de status codes
- Validación de response bodies
- Verificación de estado final de DB

Código del controller: #file
```

### Frontend (Angular)

```
Genera tests de integración para el siguiente componente/feature:

**Feature**: [nombre del feature]
**Component**: [ComponentName]
**Servicios integrados**: [lista de servicios]

**Escenarios a testear:**
- Flujo completo: [user action → service → HTTP → UI update]
- Error handling: [manejo de errores de API]
- Navegación: [redirects después de acciones]
- Forms: [validación y submit]

**Configuración:**
- TestBed con módulos completos
- HttpTestingController para mockear API
- RouterTestingModule con rutas
- Guards/interceptors si aplican

**Salida esperada:**
- Tests que validen integración Component + Service + HTTP
- Mockeo de responses HTTP
- Verificación de requests HTTP (URL, body)
- Validación de navegación
- Verificación de estado de UI

Código del componente: #file
```

## 📝 Consideraciones Especiales

### Performance

**Backend:**
- Usar base de datos en memoria cuando sea posible
- Paralelizar tests (CollectionDefinitions en xUnit)
- Cleanup eficiente (truncate tables vs recreate DB)

**Frontend:**
- Evitar delays innecesarios
- Mockear HTTP en lugar de servicios completos
- No incluir animaciones en tests

### Data Management

**Backend:**
- Seed data común en setup
- Cleanup después de cada test (IDisposable)
- Usar transacciones que se rollbackean
- Considerar DatabaseFixture compartida

**Frontend:**
- Fixtures para responses HTTP
- Reset de estado entre tests
- No compartir instancias de TestBed

### Debugging

**Backend:**
- Logging a console durante tests
- Inspeccionar DB después de fallos
- Usar Testcontainers con persistencia temporal

**Frontend:**
- DebugElement para inspeccionar DOM
- HttpTestingController.verify() para requests pendientes
- Console logs de services durante tests

### CI/CD

**Backend:**
- In-memory DB para pipelines rápidos
- Testcontainers solo en environments con Docker
- Timeout razonable (5min max)

**Frontend:**
- Headless por defecto
- Paralelización cuando sea posible
- Cache de node_modules

## 🚫 Anti-Patterns a Evitar

**Backend:**
- **NO mockear repositorios** en integration tests (usar DB real)
- **NO usar base de datos de desarrollo** (crear DB de test)
- **NO compartir datos** entre tests (cleanup siempre)
- **NO ignorar estado de DB** (validar cambios persistidos)

**Frontend:**
- **NO mockear todo** (integrar componentes reales)
- **NO usar backend real** (mockear HTTP)
- **NO shallow rendering** (TestBed completo)
- **NO tests dependientes del orden**

**General:**
- **NO mezclar unit y integration concerns**
- **NO tests lentos** (>5s es señal de mal diseño)
- **NO ignorar flakiness**
- **NO sobre-testear** (balance con unit tests)

## ✨ Valor de Integration Tests

Los integration tests proveen:
- Confianza en la comunicación entre capas
- Detección de errores de configuración
- Validación de serialización/deserialización
- Verificación de autenticación/autorización completa
- Complemento perfecto entre unit y E2E tests
