---
description: 'Reescribe código legacy a estándares modernos: .NET 8, Angular 17+, async/await, standalone components'
---

# Reescritura de Código Legacy

## 🎯 Propósito
Modernizar código legacy a tecnologías y patrones actuales, mejorando mantenibilidad, performance y developer experience, sin incluir código completo de implementación.

## 💬 Enfoque de Modernización

### Backend (.NET Framework → .NET 8)
**Transformaciones principales:**
- .NET Framework 4.x → .NET 8 con ASP.NET Core Web API
- Entity Framework 6 → Entity Framework Core 8
- Código síncrono → async/await en TODOS los métodos I/O
- System.Web.Http → Microsoft.AspNetCore.Mvc
- Inyección de dependencias con constructor injection
- Logging estructurado con ILogger en lugar de Log4Net/NLog directo
- Result<T> pattern para respuestas consistentes
- XML Comments obligatorios para documentación

### Frontend (AngularJS → Angular 17+)
**Transformaciones principales:**
- AngularJS 1.x → Angular 17+ con Standalone Components
- Callbacks/Promises → async/await y Observables
- $scope → Signals para estado reactivo
- Controllers → Component classes con lifecycle hooks
- NgModules → Standalone components con imports directos
- Control flow syntax (@if, @for, @switch)
- ChangeDetectionStrategy.OnPush
- Lazy loading de componentes

### JavaScript/TypeScript
**Transformaciones principales:**
- var → const/let apropiadamente
- Callbacks → Promises → async/await
- function() → Arrow functions donde aplique
- == → === (strict equality)
- any → Tipos específicos con interfaces
- Error handling consistente con try/catch
- Null safety con optional chaining

## 📋 Identificación de Código Legacy

### Backend Legacy (.NET)
**Indicadores:**
- Uso de `System.Web.Http` o `System.Web.Mvc`
- Referencias a `HttpContext.Current`
- Entity Framework 6 con `DbContext` legacy
- Métodos síncronos para I/O (sin async/await)
- Dependencias manejadas manualmente (sin DI)
- `ConfigurationManager` para configuración
- Logging con `Debug.WriteLine` o `Console.WriteLine`

### Frontend Legacy (Angular/JavaScript)
**Indicadores:**
- AngularJS patterns: `$scope`, `$http`, controllers
- Callbacks anidados (callback hell)
- NgModules complejos en lugar de standalone
- Uso de `var` en lugar de const/let
- Promises sin async/await
- `==` en lugar de `===`
- Tipos `any` en TypeScript

## 🔧 Proceso de Modernización

### Fase 1: Análisis del Código Legacy
1. **Inventario**: Listar archivos/componentes a modernizar
2. **Dependencias**: Identificar dependencias externas y versiones
3. **Complejidad**: Evaluar complejidad de cada componente
4. **Priorización**: Ordenar por impacto y riesgo
5. **Testing**: Verificar cobertura de tests existente

### Fase 2: Plan de Migración
1. **Estrategia**: Incremental (feature by feature) vs Big Bang
2. **Orden**: Definir orden de migración (dependencies first)
3. **Compatibilidad**: Plan para mantener compatibilidad temporal
4. **Rollback**: Estrategia de rollback si algo falla
5. **Testing**: Plan de testing durante migración

### Fase 3: Modernización
1. **Setup**: Nuevo proyecto con tecnologías modernas
2. **Configuración**: Setup de DI, logging, configuración
3. **Migración**: Convertir código feature por feature
4. **Testing**: Tests unitarios y de integración
5. **Validación**: Verificar funcionalidad equivalente

### Fase 4: Validación
1. **Tests**: Ejecutar suite completa de tests
2. **Performance**: Comparar performance antes/después
3. **Functional**: Validación funcional completa
4. **Code Review**: Revisión de código modernizado
5. **Documentation**: Actualizar documentación

## 📐 Reglas de Transformación

### Backend: .NET Framework → .NET 8

**Sistema de Configuración:**
- `ConfigurationManager.AppSettings` → `IConfiguration` inyectado
- web.config → appsettings.json
- Connection strings en DI y IOptions pattern

**Controllers:**
- Herencia de `ApiController` → `ControllerBase`
- Attributes: `[Route]`, `[HttpGet]`, etc. con routing moderno
- Return types: `IActionResult` → `ActionResult<T>`
- Dependency Injection vía constructor

**Entity Framework:**
- `DbContext` legacy → EF Core `DbContext`
- Configuración con Fluent API en `OnModelCreating`
- Queries con `.AsNoTracking()` para lectura
- Migraciones con EF Core tools

**Async/Await:**
- Todos los métodos I/O deben ser async
- Retornar `Task<T>` o `ValueTask<T>`
- Usar `ConfigureAwait(false)` en librerías
- No mezclar sync y async code

### Frontend: AngularJS → Angular 17+

**Components:**
- Controladores → Standalone Components
- `$scope` → Component properties con Signals
- `ng-repeat` → `*ngFor` → `@for` (control flow)
- `ng-if` → `*ngIf` → `@if` (control flow)
- Lifecycle: `$onInit` → `ngOnInit`

**Services:**
- `$http` → Angular `HttpClient`
- Promises → Observables con RxJS
- `$q` → async/await con `firstValueFrom()`
- Dependency injection con tokens

**Estado:**
- `$scope.$watch` → Signals con `effect()`
- `$rootScope` → Services compartidos
- Two-way binding → Event emitters o Signals

**Routing:**
- `$routeProvider` → Angular Router
- Route params con params observable
- Guards para protección de rutas
- Lazy loading de módulos/components

## ✅ Checklist de Modernización

### Backend
- [ ] .NET 8 SDK instalado y configurado
- [ ] Proyecto migrado a SDK-style .csproj
- [ ] appsettings.json para configuración
- [ ] Dependency Injection configurado
- [ ] Todos los métodos I/O son async
- [ ] EF Core con migraciones
- [ ] Logging con ILogger
- [ ] Tests unitarios pasando
- [ ] Sin warnings de compilación

### Frontend
- [ ] Angular 17+ instalado
- [ ] Standalone components implementados
- [ ] Signals para estado reactivo
- [ ] Control flow syntax (@if, @for)
- [ ] HttpClient para HTTP
- [ ] Observables manejados correctamente
- [ ] TypeScript estricto sin any
- [ ] Tests unitarios pasando
- [ ] Build de producción exitoso

## 🎯 Formato de Prompt para Copilot

```
Reescribe el siguiente código legacy a estándares modernos:

**Tipo de código**: [Backend .NET / Frontend Angular / JavaScript]

**Tecnología origen**: [.NET Framework 4.8 / AngularJS 1.6 / ES5 JavaScript]
**Tecnología destino**: [.NET 8 / Angular 17+ / TypeScript 5]

**Transformaciones requeridas:**

Backend:
- Migrar a .NET 8 con ASP.NET Core
- Implementar async/await en I/O
- Configurar Dependency Injection
- Entity Framework 6 → EF Core 8
- Logging con ILogger

Frontend:
- Migrar a Angular 17+ standalone
- Implementar Signals para estado
- Control flow syntax moderno
- TypeScript estricto
- Observables en lugar de Promises

JavaScript/TypeScript:
- var → const/let
- Callbacks → async/await
- Tipos explícitos (no any)
- Strict equality (===)
- Optional chaining

**Salida esperada:**
- Descripción de código modernizado (sin implementación completa)
- Lista de cambios principales
- Beneficios obtenidos (performance, mantenibilidad)
- Consideraciones para migración
- Breaking changes potenciales

Código legacy: [#file o #selection]
```

## 📝 Consideraciones Especiales

### Migración Incremental vs Big Bang
**Incremental (Recomendado):**
- Menos riesgo
- Testing continuo
- Rollback más fácil
- Convivencia temporal de sistemas

**Big Bang:**
- Más rápido si el sistema es pequeño
- Mayor riesgo
- Requiere más testing
- Downtime potencial

### Backwards Compatibility
- Mantener contratos de API durante transición
- Versionado de APIs si es público
- Deprecation warnings antes de eliminar features
- Documentar breaking changes

### Performance
- Medir antes y después
- Async/await puede mejorar scalability
- Signals reducen change detection
- Bundle size puede aumentar (optimizar)

### Testing
- Mantener o mejorar cobertura
- Tests de regresión críticos
- Tests de integración end-to-end
- Performance testing

## 🚫 Anti-Patterns a Evitar

- **NO convertir todo a la vez**: Migración incremental
- **NO ignorar breaking changes**: Documentar y comunicar
- **NO eliminar tests**: Mantener o mejorar cobertura
- **NO asumir equivalencia**: Validar comportamiento
- **NO sobre-optimizar**: Funcionalidad primero, optimización después
