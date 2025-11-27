---
description: 'Refactoriza y moderniza tests legacy a estándares actuales y best practices'
---

# Asistente de Modernización de Tests Legacy

## 🎯 Propósito
Analizar y refactorizar código de tests legacy a estándares modernos, mejorando mantenibilidad, legibilidad y confiabilidad mientras se preserva la intención original de los tests.

## 🔍 Fase de Análisis

Al analizar tests legacy, identificar:

### 1. Code Smells en Tests
- ❌ Nombres genéricos y poco descriptivos (Test1, TestMethod, CheckMethod)
- ❌ Sin estructura AAA clara (Arrange-Act-Assert mezclados)
- ❌ Magic numbers y strings sin contexto
- ❌ Assertions débiles (solo NotNull checks, sin verificaciones detalladas)
- ❌ Sin verificación de interacciones con mocks
- ❌ Interdependencias entre tests (un test depende que otro corra primero)
- ❌ Delays hardcodeados (Thread.Sleep, Task.Delay)
- ❌ APIs o frameworks de testing deprecados
- ❌ Tests que solo cubren happy path

### 2. Necesidades de Migración de Framework
- MSTest → xUnit (C#)
- NUnit → xUnit (C#)
- Protractor → Cypress/Playwright (Angular)
- Jasmine patterns legacy → Jasmine moderno
- Callbacks basados en eventos → async/await
- Assertions manuales → FluentAssertions

## 🔧 Modernización de C# Tests

### Patrón 1: Migración de MSTest/NUnit a xUnit

**Cambios de Atributos:**
- `[TestClass]` → Remover (no necesario en xUnit)
- `[TestMethod]` → `[Fact]`
- `[DataTestMethod]` + `[DataRow]` → `[Theory]` + `[InlineData]`
- `[TestInitialize]` → Constructor de clase
- `[TestCleanup]` → `Dispose()` implementando `IDisposable`
- `[ClassInitialize]` → `IClassFixture<T>`

**Cambios de Assertions:**
- Migrar de `Assert.*` a FluentAssertions
- `Assert.AreEqual(expected, actual)` → `actual.Should().Be(expected)`
- `Assert.IsTrue(condition)` → `condition.Should().BeTrue()`
- `Assert.IsNotNull(obj)` → `obj.Should().NotBeNull()`
- `Assert.ThrowsException<T>()` → `FluentActions.Invoking().Should().Throw<T>()`

### Patrón 2: Mejorar Nomenclatura de Tests

**Formato Recomendado**: `MethodName_Scenario_ExpectedBehavior`

**Ejemplos de transformación:**
- `Test1()` → `Login_ValidCredentials_ReturnsSuccess()`
- `TestLogin()` → `Login_InvalidPassword_ReturnsUnauthorized()`
- `CheckIfWorks()` → `CreateTicket_EmptyTitle_ThrowsValidationException()`

### Patrón 3: Agregar Estructura AAA

Transformar tests sin estructura a patrón AAA con comentarios:

**Estructura requerida:**
- Sección `// Arrange` - Setup de datos, mocks, sistema bajo test (SUT)
- Sección `// Act` - Ejecución del método bajo test
- Sección `// Assert` - Verificaciones y assertions

### Patrón 4: Reemplazar Assertions Débiles

**Transformaciones comunes:**
- Cambiar assertions simples `Assert.IsNotNull(result)` por verificaciones detalladas con FluentAssertions
- Agregar verificaciones de propiedades específicas
- Verificar estado completo del objeto retornado
- Añadir assertions de tipos complejos

### Patrón 5: Eliminar Delays Manuales

Reemplazar `Thread.Sleep()` o `Task.Delay()` con:
- Auto-waiting del framework de test
- Mocking de dependencias asíncronas
- Test doubles que no requieren delays
- Uso apropiado de `Task.CompletedTask` en mocks

### Patrón 6: Agregar Casos de Test Faltantes

Si solo existe happy path, agregar:
- **Tests de validación**: Inputs inválidos, null values, empty strings
- **Tests de error handling**: Excepciones esperadas, edge cases
- **Tests de edge cases**: Límites, caracteres especiales, concurrencia
- **Tests de autorización**: Permisos, roles, usuarios no autenticados

## 🎭 Modernización de TypeScript/Angular Tests

### Patrón 1: Migración de Protractor a Cypress/Playwright

**Transformaciones comunes:**
- `browser.get()` → `cy.visit()` (Cypress) o `page.goto()` (Playwright)
- `element(by.css())` → `cy.get()` o `page.locator()`
- `element.click()` → `cy.click()` o `locator.click()`
- Eliminar `browser.wait()` innecesarios (auto-waiting)

### Patrón 2: Modernizar Jasmine Tests

**Mejoras a aplicar:**
- Usar `async/await` en lugar de `done()` callback
- Actualizar sintaxis de `beforeEach` con TestBed.configureTestingModule
- Usar `flush()` y `tick()` apropiadamente con `fakeAsync`
- Reemplazar `spy` legacy por `jasmine.createSpy()`

### Patrón 3: Componentes Angular Testing

**Patrones modernos:**
- Usar Standalone Components testing (Angular 17+)
- Signals testing con `TestBed`
- Simplificar mocks con `jasmine.createSpyObj`
- Testing de `@Input` y `@Output` apropiadamente

## 📋 Checklist de Modernización

Para cada archivo de tests modernizado, verificar:

- [ ] Nombres descriptivos: `Method_Scenario_Result`
- [ ] Estructura AAA clara con comentarios
- [ ] FluentAssertions en lugar de assertions básicas
- [ ] Mocks configurados apropiadamente
- [ ] Sin magic numbers o strings (usar constantes)
- [ ] Sin Thread.Sleep o delays manuales
- [ ] Cobertura de casos: happy path + errores + edge cases
- [ ] Tests independientes (no orden dependiente)
- [ ] Async/await en lugar de callbacks
- [ ] Framework moderno (xUnit para C#, Jasmine/Cypress para Angular)
- [ ] Todos los tests pasan

## 🎯 Formato de Prompt para Copilot

```
Moderniza los siguientes tests legacy a estándares actuales:

**Archivo de tests**: [nombre del archivo]

**Problemas identificados:**
- [Nombres genéricos de tests]
- [Sin estructura AAA]
- [Assertions débiles]
- [Framework deprecado]
- [Solo happy path coverage]

**Modernizaciones requeridas:**
- Migrar a [xUnit / Jasmine moderno / Cypress]
- Aplicar naming convention: Method_Scenario_Result
- Agregar estructura AAA con comentarios
- Usar FluentAssertions (C#) o expect detallado (TS)
- Eliminar delays manuales
- Agregar tests de error cases y edge cases
- Verificar interacciones con mocks
- Usar async/await consistentemente

**Framework target:**
- C#: xUnit + FluentAssertions + NSubstitute/Moq
- TypeScript: Jasmine + Karma o Cypress
- E2E: Cypress o Playwright

**Salida esperada:**
- Tests modernizados con mejores nombres
- Estructura clara y legible
- Cobertura mejorada
- Explicación de cambios realizados

Tests a modernizar: [#file o #selection]
```

## 📝 Consideraciones Especiales

### Preservar Intención Original
- No cambiar lógica de validación existente
- Mantener casos de test cubiertos
- Solo mejorar estructura y legibilidad

### Testing de Código Legacy
- Tests pueden revelar bugs en código original
- Documentar comportamiento inesperado encontrado
- No "arreglar" tests que fallan (arreglar el código)

### Balance Refactorización vs Reescritura
- Refactorizar tests simples
- Reescribir tests muy complejos desde cero
- Mantener cobertura durante el proceso

### Tests de Integración vs Unitarios
- Separar tests de integración de unitarios
- Tests unitarios deben ser rápidos (< 100ms)
- Tests de integración pueden ser más lentos

## 🚫 Anti-Patterns a Evitar

- **NO eliminar tests** sin entender qué validan
- **NO cambiar lógica** de validación sin justificación
- **NO sobre-mockear**: Mock solo lo necesario
- **NO tests frágiles**: Evitar dependencia de datos específicos
- **NO ignorar tests que fallan**: Investigar y corregir
- **NO tests interdependientes**: Cada test debe ser independiente


