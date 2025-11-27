---
description: 'Refactoriza controladores ASP.NET Core complejos aplicando SRP, extrayendo métodos y separando responsabilidades'
---

# Refactorización de Controladores Complejos

## 🎯 Propósito
Refactorizar controladores ASP.NET Core que violan Single Responsibility Principle, tienen métodos muy largos o mezclan lógica de negocio con lógica de presentación.

## 🔍 Análisis de Problemas Comunes

Al analizar un controlador, identificar:

### 1. Violaciones de SRP (Single Responsibility Principle)
- Lógica de negocio dentro del controlador (debe estar en Services)
- Validaciones complejas en el controlador (deben estar en Validators)
- Llamadas directas a DbContext (debe usar Repositories)
- Construcción manual de DTOs (debe usar AutoMapper o mappers)
- Envío de emails/notificaciones desde el controlador

### 2. Métodos Largos
- Métodos con más de 30-40 líneas de código
- Múltiples niveles de anidación (if dentro de foreach dentro de if)
- Lógica repetida entre diferentes endpoints
- Responsabilidades mezcladas en un solo método

### 3. Acoplamiento Fuerte
- Dependencias de clases concretas en lugar de interfaces
- Instanciación directa de objetos con `new` 
- Acceso directo a HttpContext fuera de lo necesario
- Dependencias innecesarias inyectadas

### 4. Manejo de Respuestas Inconsistente
- Mezcla de tipos de retorno (IActionResult, ObjectResult, ActionResult<T>)
- Códigos de estado HTTP inconsistentes
- Respuestas de error sin estructura estándar
- Falta de uso de ProblemDetails

## 🔧 Reglas de Refactorización

### Para Controllers: Responsabilidades Permitidas

**✅ Permitido:**
- Recibir requests HTTP y validar ModelState
- Llamar a métodos de Services pasando DTOs
- Mapear resultados de Services a respuestas HTTP apropiadas
- Retornar códigos de estado HTTP (200, 201, 204, 400, 404, 500)
- Coordinar entre múltiples servicios si es necesario (pero sin lógica)
- Manejar autenticación/autorización con attributes

**❌ Prohibido:**
- Lógica de negocio (cálculos, validaciones complejas, reglas de dominio)
- Acceso directo a base de datos (DbContext, queries directas)
- Validaciones más allá de ModelState básico
- Construcción de emails, notificaciones, reportes
- Transformaciones complejas de datos
- Manejo de transacciones
- Logging de lógica de negocio (solo logging de requests/responses)

### Patrón de Refactorización

#### Problemas Comunes y Soluciones:

**Problema 1: Lógica de Negocio en Controller**
- **Solución**: Extraer a Service
- Crear interface IService
- Implementar Service con lógica
- Inyectar IService en controller

**Problema 2: Validaciones Complejas**
- **Solución**: Usar FluentValidation
- Crear validators específicos para cada DTO
- Registrar validators en DI
- Controller solo valida ModelState

**Problema 3: Acceso Directo a DbContext**
- **Solución**: Implementar Repository pattern
- Crear interface IRepository<T>
- Implementar repository específico
- Service usa repository, no DbContext

**Problema 4: Métodos Muy Largos**
- **Solución**: Extraer métodos privados o Services
- Dividir en pasos lógicos
- Cada método hace una cosa
- Nombre descriptivo de métodos

**Problema 5: Construcción Manual de DTOs**
- **Solución**: Usar AutoMapper o extension methods
- Configurar mapeo Entity ↔ DTO
- Controller solo llama a mapper
- Mantener mapeos en un solo lugar

### Estructura Ideal de un Controller

Un controller bien diseñado debe tener:

1. **Dependencias Mínimas**: Solo servicios realmente necesarios
2. **Constructor con DI**: Inyección de interfaces, no clases concretas
3. **Métodos Cortos**: 10-20 líneas máximo por endpoint
4. **Validación Simple**: Solo ModelState, validaciones complejas en validators
5. **Manejo de Errores**: Try-catch solo si necesario, middleware global para excepciones
6. **Respuestas Consistentes**: Usar ActionResult<T> y códigos HTTP estándar
7. **Documentación**: Atributos para Swagger (ProducesResponseType, SwaggerOperation)
8. **Autorización**: Attributes [Authorize] donde corresponda

## 📋 Checklist de Refactorización

Para cada controller refactorizado, verificar:

- [ ] No hay lógica de negocio en el controller
- [ ] No hay acceso directo a DbContext
- [ ] Validaciones complejas están en FluentValidation
- [ ] Métodos tienen menos de 30 líneas
- [ ] Solo inyecta servicios que realmente usa
- [ ] Usa interfaces (IService) no clases concretas
- [ ] Retorna ActionResult<T> consistentemente
- [ ] Códigos HTTP apropiados (200, 201, 204, 400, 404, 500)
- [ ] Documentado con atributos de Swagger
- [ ] Manejo de errores delegado a middleware global
- [ ] Usa DTOs, no expone entidades directamente
- [ ] Logging solo de requests/responses, no de lógica
- [ ] Nombres de métodos descriptivos y verbos HTTP correctos

## 🎯 Formato de Prompt para Copilot

```
Refactoriza el siguiente controller ASP.NET Core aplicando SRP y mejores prácticas:

**Controller**: [nombre del controller]

**Problemas identificados:**
- [Lógica de negocio en controller]
- [Acceso directo a DbContext]
- [Método muy largo (> 50 líneas)]
- [Validaciones complejas]
- [Construcción manual de emails/notificaciones]

**Refactorizaciones requeridas:**
- Extraer lógica de negocio a IService
- Crear Repository para acceso a datos
- Dividir métodos largos en pasos lógicos
- Mover validaciones a FluentValidation
- Extraer construcción de emails a IEmailService
- Implementar mapeo automático con AutoMapper
- Agregar documentación Swagger
- Usar ActionResult<T> consistentemente

**Servicios a crear (si no existen):**
- ITicketService / TicketService
- ITicketRepository / TicketRepository
- IEmailService / EmailService
- Validators con FluentValidation

**Salida esperada:**
- Controller refactorizado (solo coordinación)
- Interfaces de servicios necesarios
- Explicación de cambios realizados
- Beneficios de la refactorización
- Checklist de testing post-refactorización

Controller a refactorizar: [#file o #selection]
```

## 📝 Consideraciones Especiales

### Cuándo NO Refactorizar
- Controller ya es simple y cumple SRP
- Overhead de crear Services no justificado (endpoints muy simples)
- Tiempo/recursos limitados y controller funciona

### Refactorización Incremental
- Refactorizar endpoint por endpoint
- Empezar por los más complejos
- Mantener tests pasando en cada paso
- Commit frecuente durante refactorización

### Testing Durante Refactorización
- Ejecutar tests existentes después de cada cambio
- Agregar tests si no existen
- Verificar que comportamiento no cambia
- Tests de integración son valiosos aquí

### Backwards Compatibility
- Mantener contratos de API si es público
- No cambiar URLs de endpoints
- No cambiar estructura de DTOs sin versionado
- Deprecar antes de eliminar endpoints

## 🚫 Anti-Patterns a Evitar

- **NO crear "God Services"**: Services con demasiadas responsabilidades
- **NO sobre-abstraer**: No crear capas innecesarias
- **NO mezclar async/sync**: Mantener consistencia
- **NO ignorar validación**: Siempre validar inputs
- **NO exponer entidades**: Usar DTOs siempre
- **NO hardcodear**: Configuración en appsettings.json
- **NO loguear datos sensibles**: Passwords, tokens, PII


