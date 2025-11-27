---
description: 'Genera tests unitarios completos para backend C# (Controllers, Services, Repositories)'
---

# Generador de Tests Backend - C# xUnit + FluentAssertions

## 🎯 Propósito
Generar tests unitarios completos y profesionales para código C# backend siguiendo principios SOLID y mejores prácticas de la industria, sin incluir código de implementación completo.

## 📋 Instrucciones de Análisis

Al analizar un archivo C#, identificar:
1. **Tipo de Archivo**: Controller, Service, Repository, Validator, Helper o Extension
2. **Dependencias**: Todas las interfaces y servicios inyectados que requieren mocking
3. **Métodos Públicos**: Todos los métodos que requieren tests
4. **Reglas de Negocio**: Validaciones, autorizaciones, cálculos complejos
5. **Casos Edge**: Entradas null, colecciones vacías, condiciones límite

## 🧪 Reglas de Generación de Tests

### Para Controllers (ASP.NET Core API)

Generar tests que cubran:

#### Endpoints HTTP
- Testear TODOS los métodos HTTP (GET, POST, PUT, DELETE, PATCH)
- Verificar códigos HTTP correctos para cada escenario:
  - 200 OK para queries exitosas
  - 201 Created para creaciones exitosas
  - 204 No Content para deletes exitosos
  - 400 Bad Request para validaciones fallidas
  - 404 Not Found para recursos no encontrados
  - 401 Unauthorized para usuarios no autenticados
  - 403 Forbidden para usuarios sin permisos
  - 500 Internal Server Error para errores no controlados

#### Validación de Modelo
- Testear ModelState inválido retorna BadRequest (400)
- Verificar ValidationProblemDetails en respuesta
- Probar cada regla de validación del DTO
- Verificar mensajes de error descriptivos

#### Autorización
- Testear usuario no autenticado retorna 401
- Testear usuario sin permisos retorna 403
- Verificar que atributos [Authorize] funcionan apropiadamente
- Testear roles y policies

#### Lógica de Negocio
- Testear operaciones exitosas (happy path)
- Testear recursos no encontrados (404)
- Testear conflictos de negocio (409 Conflict)
- Testear errores de validación (400)

### Para Services (Lógica de Negocio)

Generar tests que cubran:

#### Validación de Reglas de Negocio
- Testear cada regla de negocio específica del dominio
- Verificar mensajes de error descriptivos y útiles
- Probar validaciones cruzadas entre campos
- Testear condiciones de borde de reglas de negocio

#### Orquestación de Múltiples Dependencias
- Verificar orden correcto de llamadas a dependencias
- Testear escenarios de rollback de transacciones
- Testear lógica de compensación en caso de fallos
- Verificar que todas las dependencias son llamadas apropiadamente

#### Transformaciones de Datos
- Validación de entrada (nulls, vacíos, formatos)
- Mapeo correcto de DTO a Entity
- Mapeo correcto de Entity a DTO
- Cálculos complejos con casos límite

### Para Repositories (Acceso a Datos)

Usar **InMemory Database** de EF Core para tests:

Cobertura requerida:
- Operaciones CRUD completas (Create, Read, Update, Delete)
- Filtros de queries y ordenamiento
- Paginación con parámetros variables
- Carga anticipada (Include) de relaciones
- Comportamiento de soft delete
- Validación de constraints únicos
- Queries complejas con múltiples joins

## 📐 Estándares de Estructura de Tests

### Convención de Nombres
**Formato**: `NombreMetodo_Escenario_ResultadoEsperado`

**Ejemplos:**
- `CreateTicket_ValidData_ReturnsSuccess`
- `GetTicketById_NonExistentId_ReturnsNotFound`
- `UpdateTicket_ClosedStatus_ThrowsInvalidOperationException`
- `DeleteTicket_UserWithoutPermission_ThrowsUnauthorizedException`
- `AssignTicket_AlreadyAssigned_ReturnsConflict`

### Patrón AAA (Arrange-Act-Assert)

Estructura cada test en tres bloques claramente separados:

**// Arrange**: Configurar dependencias, crear mocks, preparar datos de prueba, setup del SUT
**// Act**: Ejecutar el método bajo prueba una sola vez
**// Assert**: Verificar el resultado esperado, verificar interacciones con mocks

Agregar comentarios `// Arrange`, `// Act`, `// Assert` explícitamente.

### Usar FluentAssertions

Preferir FluentAssertions sobre Assert.* nativo:
- Sintaxis más legible y expresiva
- Mensajes de error más descriptivos
- Assertions de objetos complejos más fáciles
- Mejor para colecciones y tipos anónimos

### Mocking con NSubstitute o Moq

Para cada dependencia:
- Crear mock de la interface
- Configurar comportamiento esperado (Returns, Throws)
- Verificar interacciones importantes (Received, DidNotReceive)

## 📋 Checklist de Tests Completos

Para cada clase testeada, verificar:

- [ ] Todos los métodos públicos tienen tests
- [ ] Happy path cubierto
- [ ] Error cases cubiertos (null, empty, invalid)
- [ ] Edge cases cubiertos (límites, concurrencia)
- [ ] Excepciones verificadas con tipo específico
- [ ] Interacciones con mocks verificadas
- [ ] Naming convention aplicada
- [ ] Estructura AAA clara
- [ ] FluentAssertions usadas
- [ ] Tests independientes (no orden dependiente)
- [ ] No hay Thread.Sleep o delays
- [ ] Todos los tests pasan

## 🎯 Formato de Prompt para Copilot

```
Genera tests unitarios completos para el siguiente código C#:

**Tipo**: [Controller / Service / Repository]
**Clase**: [nombre de la clase]

**Cobertura requerida:**
- Todos los métodos públicos
- Happy path + error cases + edge cases
- Validaciones de negocio
- Excepciones específicas del dominio
- Interacciones con dependencias

**Framework:**
- xUnit para tests
- FluentAssertions para assertions
- NSubstitute/Moq para mocking

**Estructura:**
- Naming: Method_Scenario_Result
- Patrón AAA con comentarios
- Un método de test por escenario
- Setup compartido en constructor si necesario

**Casos específicos a cubrir:**
[Listar casos específicos del dominio]

**Salida esperada:**
- Clase de test completa
- Setup de mocks explicado
- Todos los escenarios críticos cubiertos
- Tests compilables y ejecutables

Código a testear: [#file o #selection]
```

## 📝 Casos Comunes de Testing

### Controllers
- Valid input → 200/201 con DTO correcto
- Invalid ModelState → 400 con ValidationProblemDetails
- Service throws NotFoundException → 404
- Service throws ValidationException → 400
- User not authenticated → 401
- User without permission → 403

### Services
- Valid business logic → Success result
- Invalid input → ValidationException
- Entity not found → NotFoundException
- Business rule violation → BusinessRuleException
- Multiple service calls → Verify order
- Transaction rollback → Verify state

### Repositories
- Add entity → Entity saved with ID
- Get by ID exists → Returns entity
- Get by ID not exists → Returns null
- Update entity → Changes persisted
- Delete entity → Entity removed or soft deleted
- Query with filter → Returns filtered results

## 🚫 Anti-Patterns a Evitar

- **NO tests que dependen de orden de ejecución**
- **NO tests con lógica compleja** (el test debe ser simple)
- **NO tests que testean implementación** (testear comportamiento)
- **NO mocking excesivo** (solo lo necesario)
- **NO assertions débiles** (verificar estado completo)
- **NO tests lentos** (unitarios < 100ms)
- **NO datos hardcodeados compartidos** (cada test su data)

