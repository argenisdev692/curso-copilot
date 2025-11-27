# 🎯 Análisis Completo: Frameworks de Prompts 2025

> **Contexto**: Guía práctica con ejemplos aplicados al proyecto **TicketManagementSystem**

---

## 📋 Tabla de Contenidos

1. [Resumen Ejecutivo](#1-resumen-ejecutivo)
2. [Framework C.R.E.A.T.E.](#2-framework-create)
3. [Framework CARE](#3-framework-care)
4. [Framework C.O.R.E.](#4-framework-core)
5. [Framework CLEAR](#5-framework-clear)
6. [Técnica Chain-of-Thought (CoT)](#6-técnica-chain-of-thought-cot)
7. [Técnica ReAcT](#7-técnica-react)
8. [Técnica Few-Shot Prompting](#8-técnica-few-shot-prompting)
9. [Comparativa de Frameworks](#9-comparativa-de-frameworks)
10. [Templates Reutilizables](#10-templates-reutilizables)
11. [Checklist de Selección](#11-checklist-de-selección)

---

## 1. Resumen Ejecutivo

### Principios Fundamentales en 2025

Según la investigación actual, los **3 factores más predictivos** para resultados de alta calidad son:

| Factor | Descripción | Impacto |
|--------|-------------|---------|
| **Claridad** | Instrucciones sin ambigüedades | ⭐⭐⭐⭐⭐ |
| **Contexto** | Información relevante del entorno | ⭐⭐⭐⭐⭐ |
| **Especificidad** | Requisitos explícitos y detallados | ⭐⭐⭐⭐⭐ |

### Mapa de Frameworks

```
┌─────────────────────────────────────────────────────────────────────┐
│                    FRAMEWORKS DE PROMPTS 2025                       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ESTRUCTURADOS          TÉCNICAS AVANZADAS      ESPECIALIZADOS     │
│  ├── C.R.E.A.T.E.      ├── Chain-of-Thought    ├── CLEAR           │
│  ├── CARE              ├── ReAcT               │   (Académico)     │
│  └── C.O.R.E.          └── Few-Shot            │                   │
│                                                                     │
│  ════════════════════════════════════════════════════════════════  │
│                                                                     │
│  CASO DE USO:                                                       │
│  • C.R.E.A.T.E. → Tareas complejas, múltiples requisitos           │
│  • CARE → Tareas rápidas, resultados concretos                     │
│  • C.O.R.E. → Balance entre detalle y velocidad                    │
│  • CoT → Razonamiento paso a paso, decisiones arquitectónicas      │
│  • ReAcT → Precisión factual, investigación                        │
│  • Few-Shot → Consistencia de formato, patrones repetitivos        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 2. Framework C.R.E.A.T.E.

### Estructura

| Componente | Significado | Descripción |
|------------|-------------|-------------|
| **C** | Context | Situación actual, tecnologías, proyecto |
| **R** | Request | La acción específica que necesitas |
| **E** | Examples | Ejemplos de entrada/salida esperados |
| **A** | Adjustments | Modificaciones o personalizaciones |
| **T** | Type of output | Formato del resultado esperado |
| **E** | Extras | Información adicional, edge cases |

### Ventajas ✅

- **Más completo**: Cubre todos los aspectos de una solicitud
- **Reduce iteraciones**: Menos ida y vuelta con el LLM
- **Ideal para tareas complejas**: Arquitectura, refactoring masivo
- **Resultados profesionales**: Código listo para producción

### Limitaciones ⚠️

- **Requiere más tiempo**: Elaborar el prompt toma más esfuerzo inicial
- **Overhead para tareas simples**: Excesivo para cambios pequeños
- **Curva de aprendizaje**: Requiere práctica para usarlo eficientemente

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
**C - Context:**
Proyecto TicketManagementSystem, API REST en .NET 8 con EF Core.
Archivo actual: TicketService.cs que implementa ITicketService.
Usamos patrón Repository, Result pattern, y AutoMapper para DTOs.

**R - Request:**
Crear un método para reasignar tickets en lote (bulk reassignment).
Debe permitir reasignar múltiples tickets a un nuevo usuario.

**E - Examples:**
Input: { ticketIds: [1, 2, 3], newAssigneeId: 5 }
Output exitoso: { success: true, reassignedCount: 3, failedIds: [] }
Output con errores: { success: false, reassignedCount: 1, failedIds: [2, 3] }

**A - Adjustments:**
- Solo tickets en estado Open o InProgress pueden reasignarse
- Validar que el nuevo asignado tenga rol "Technician" o "Admin"
- Máximo 50 tickets por operación

**T - Type of output:**
- Método async en TicketService
- DTO BulkReassignmentDto para request
- DTO BulkReassignmentResultDto para response
- Unit tests con xUnit

**E - Extras:**
- Usar transacción para atomicidad
- Log de auditoría por cada ticket reasignado
- Si falla uno, continuar con los demás (partial success)
```

### Resultado Esperado

```csharp
/// <summary>
/// Reasigna múltiples tickets a un nuevo usuario en una operación atómica parcial.
/// </summary>
/// <param name="dto">DTO con IDs de tickets y nuevo asignado</param>
/// <param name="ct">Token de cancelación</param>
/// <returns>Resultado con conteo de éxitos y fallos</returns>
public async Task<Result<BulkReassignmentResultDto>> BulkReassignAsync(
    BulkReassignmentDto dto, 
    CancellationToken ct)
{
    // Validación de límite
    if (dto.TicketIds.Count > 50)
        return Result<BulkReassignmentResultDto>.Failure(
            "Maximum 50 tickets per operation", "ValidationError");
    
    // Validar rol del nuevo asignado
    var assignee = await _userRepository.GetByIdAsync(dto.NewAssigneeId, ct);
    if (assignee?.Role is not ("Technician" or "Admin"))
        return Result<BulkReassignmentResultDto>.Failure(
            "Assignee must be Technician or Admin", "ValidationError");
    
    var reassigned = new List<int>();
    var failed = new List<int>();
    
    foreach (var ticketId in dto.TicketIds)
    {
        try
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId, ct);
            if (ticket?.Status is Status.Open or Status.InProgress)
            {
                ticket.AssignedToId = dto.NewAssigneeId;
                ticket.UpdatedAt = DateTime.UtcNow;
                await _ticketRepository.UpdateAsync(ticket, ct);
                reassigned.Add(ticketId);
                
                _logger.LogInformation(
                    "Ticket {TicketId} reassigned to user {UserId}", 
                    ticketId, dto.NewAssigneeId);
            }
            else
            {
                failed.Add(ticketId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to reassign ticket {TicketId}", ticketId);
            failed.Add(ticketId);
        }
    }
    
    await _unitOfWork.SaveChangesAsync(ct);
    
    return Result<BulkReassignmentResultDto>.Success(new BulkReassignmentResultDto
    {
        Success = failed.Count == 0,
        ReassignedCount = reassigned.Count,
        FailedIds = failed
    });
}
```

---

## 3. Framework CARE

### Estructura

| Componente | Significado | Descripción |
|------------|-------------|-------------|
| **C** | Context | Qué estás haciendo, dónde |
| **A** | Action | Qué acción necesitas |
| **R** | Result | Qué resultado esperas |
| **E** | Example | Un ejemplo concreto (opcional) |

### Ventajas ✅

- **Rápido de elaborar**: 4 componentes simples
- **Orientado a resultados**: Enfoca en el output esperado
- **Ideal para tareas medianas**: CRUD, componentes, servicios simples
- **Fácil de recordar**: Acrónimo intuitivo

### Limitaciones ⚠️

- **Menos detallado**: Puede omitir restricciones importantes
- **No ideal para arquitectura**: Falta espacio para decisiones complejas
- **Asume conocimiento previo**: El LLM debe inferir algunos requisitos

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
**C - Context:**
Frontend Angular 17, módulo de tickets, necesito validar formularios.

**A - Action:**
Crear validador personalizado para verificar que la fecha de vencimiento 
de un ticket no sea anterior a hoy.

**R - Result:**
Directiva de validación reutilizable que muestre mensaje de error.

**E - Example:**
Input: fecha "2024-01-15" (pasado) → Error: "La fecha debe ser futura"
Input: fecha "2025-12-25" (futuro) → Válido
```

### Resultado Esperado

```typescript
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Validador personalizado para fechas futuras
 * @param minDaysAhead Días mínimos en el futuro (default: 0 = hoy)
 * @returns ValidatorFn que valida fecha futura
 */
export function futureDateValidator(minDaysAhead: number = 0): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) return null;
    
    const inputDate = new Date(control.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    today.setDate(today.getDate() + minDaysAhead);
    
    return inputDate >= today 
      ? null 
      : { futureDate: { message: 'La fecha debe ser futura' } };
  };
}

// Uso en FormGroup:
// dueDate: ['', [Validators.required, futureDateValidator(1)]]
```

---

## 4. Framework C.O.R.E.

### Estructura

| Componente | Significado | Pregunta Clave |
|------------|-------------|----------------|
| **C** | Contexto | ¿Qué somos? ¿Qué tenemos? |
| **O** | Objetivo | ¿Qué queremos lograr? |
| **R** | Restricciones | ¿Qué NO queremos? ¿Qué evitar? |
| **E** | Ejemplo | ¿Cómo debe verse el resultado? |

### Ventajas ✅

- **Equilibrado**: Balance entre detalle y brevedad
- **Enfocado en restricciones**: Evita errores comunes proactivamente
- **Versátil**: Funciona para backend, frontend, tests
- **Pragmático**: Orientado a desarrolladores

### Limitaciones ⚠️

- **Sin formato explícito**: No especifica cómo entregar el resultado
- **Ejemplo opcional**: A veces se omite y reduce calidad
- **Menos estructura que C.R.E.A.T.E.**: Para tareas muy complejas puede quedarse corto

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
**C - Contexto:** 
API TicketManagementSystem en .NET 8. Controlador TicketsController.cs 
que usa ITicketService, IMapper, y MediatR para CQRS.

**O - Objetivo:** 
Endpoint para obtener estadísticas de tickets por estado y prioridad 
del último mes.

**R - Restricciones:** 
- No exponer datos sensibles de usuarios
- No consultas N+1 (usar proyección en query)
- Respuesta cacheada 5 minutos
- Solo usuarios autenticados

**E - Ejemplo de salida:**
{
  "period": "2025-10-26 to 2025-11-26",
  "byStatus": { "Open": 45, "InProgress": 23, "Resolved": 67, "Closed": 120 },
  "byPriority": { "Low": 50, "Medium": 100, "High": 80, "Critical": 25 },
  "totalTickets": 255,
  "avgResolutionTimeHours": 48.5
}
```

### Resultado Esperado

```csharp
/// <summary>
/// Get ticket statistics for the last month
/// </summary>
/// <returns>Aggregated ticket statistics</returns>
/// <response code="200">Returns ticket statistics</response>
/// <response code="401">Unauthorized</response>
[HttpGet("statistics")]
[Authorize]
[ResponseCache(Duration = 300)] // 5 minutos
[ProducesResponseType(typeof(TicketStatisticsDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<ActionResult<TicketStatisticsDto>> GetStatistics()
{
    var endDate = DateTime.UtcNow;
    var startDate = endDate.AddMonths(-1);
    
    var stats = await _context.Tickets
        .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
        .GroupBy(_ => 1) // Single group for aggregation
        .Select(g => new TicketStatisticsDto
        {
            Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
            ByStatus = g.GroupBy(t => t.Status)
                        .ToDictionary(s => s.Key.ToString(), s => s.Count()),
            ByPriority = g.GroupBy(t => t.Priority)
                          .ToDictionary(p => p.Key.ToString(), p => p.Count()),
            TotalTickets = g.Count(),
            AvgResolutionTimeHours = g
                .Where(t => t.Status == Status.Resolved && t.UpdatedAt != null)
                .Average(t => EF.Functions.DateDiffHour(t.CreatedAt, t.UpdatedAt))
        })
        .AsNoTracking()
        .FirstOrDefaultAsync();
    
    return Ok(stats ?? new TicketStatisticsDto { Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}" });
}
```

---

## 5. Framework CLEAR

### Estructura (Orientado a Investigación)

| Componente | Significado | Aplicación |
|------------|-------------|------------|
| **C** | Clarify | Clarificar el problema o pregunta |
| **L** | Literature | Contexto teórico o referencias |
| **E** | Evaluate | Criterios de evaluación |
| **A** | Analyze | Análisis esperado |
| **R** | Results | Formato de resultados |

### Ventajas ✅

- **Excelente para investigación**: Decisiones arquitectónicas, ADRs
- **Documenta el "por qué"**: No solo el "qué"
- **Ideal para evaluaciones**: Comparar opciones, trade-offs
- **Produce documentación**: Genera artefactos útiles

### Limitaciones ⚠️

- **No es para código directo**: Más para análisis que implementación
- **Requiere tiempo**: Elaborar el prompt es más lento
- **Específico para decisiones**: No para tareas de desarrollo diario

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
**C - Clarify (Clarificar):**
Necesito decidir si implementar notificaciones en tiempo real para 
TicketManagementSystem cuando se crea/actualiza un ticket.

**L - Literature (Referencias):**
Opciones a considerar:
1. SignalR (WebSockets nativos en .NET)
2. Server-Sent Events (SSE)
3. Polling tradicional
4. Azure SignalR Service (managed)

**E - Evaluate (Criterios):**
- Escalabilidad (1000+ usuarios concurrentes)
- Costo de infraestructura
- Complejidad de implementación
- Soporte en Angular 17
- Latencia aceptable (<500ms)

**A - Analyze (Análisis esperado):**
Tabla comparativa con pros/cons de cada opción.
Recomendación basada en el contexto del proyecto.

**R - Results (Formato):**
ADR (Architecture Decision Record) con:
- Contexto, Decisión, Consecuencias
```

### Resultado Esperado: ADR

```markdown
# ADR-002: Notificaciones en Tiempo Real

## Estado
Propuesto

## Contexto
TicketManagementSystem necesita notificar a usuarios cuando:
- Se les asigna un ticket
- Un ticket que crearon cambia de estado
- Se agrega un comentario a sus tickets

Usuarios esperados: 500-1000 concurrentes.
Stack actual: .NET 8 + Angular 17.

## Opciones Evaluadas

| Criterio | SignalR | SSE | Polling | Azure SignalR |
|----------|---------|-----|---------|---------------|
| Escalabilidad | ⭐⭐⭐ | ⭐⭐⭐ | ⭐ | ⭐⭐⭐⭐⭐ |
| Costo | Bajo | Bajo | Bajo | Medio |
| Complejidad | Media | Baja | Baja | Baja |
| Angular Support | ✅ | ✅ | ✅ | ✅ |
| Latencia | <100ms | <200ms | 1-5s | <100ms |
| Bidireccional | ✅ | ❌ | ❌ | ✅ |

## Decisión
**Usar SignalR con fallback a SSE**.

Razones:
1. Integración nativa con .NET 8
2. Librería oficial @microsoft/signalr para Angular
3. Escalable hasta 1000 usuarios sin managed service
4. Bidireccional para futuras features (chat, colaboración)

## Consecuencias
### Positivas
- Notificaciones instantáneas
- Mejor UX para usuarios
- Infraestructura preparada para features futuras

### Negativas
- Aumenta complejidad del backend
- Requiere manejo de reconexiones en frontend
- Consumo de memoria por conexión activa

## Implementación
1. Hub: `NotificationHub.cs` con grupos por UserId
2. Frontend: `NotificationService` con auto-reconnect
3. Eventos: `TicketAssigned`, `TicketStatusChanged`, `CommentAdded`
```

---

## 6. Técnica Chain-of-Thought (CoT)

### ¿Qué es?
Solicitar al LLM que **explique su razonamiento paso a paso** antes de generar código.

### Cuándo Usar

- Decisiones arquitectónicas complejas
- Debugging de lógica de negocio
- Optimización de algoritmos
- Refactoring con múltiples opciones

### Ventajas ✅

- **Detecta errores lógicos temprano**: Ves el razonamiento antes del código
- **Educativo**: Aprendes el "por qué" de las decisiones
- **Mejora calidad**: El LLM "piensa" mejor cuando explica

### Limitaciones ⚠️

- **Consume más tokens**: Las explicaciones añaden overhead
- **Más lento**: Requiere procesamiento adicional
- **No siempre necesario**: Overkill para tareas simples

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
Antes de generar código, analiza paso a paso:

**Problema:** 
El método GetTicketsAsync en TicketService.cs está lento cuando hay 
+10,000 tickets. Actualmente carga todos y filtra en memoria.

**Tu análisis debe cubrir:**
1. ¿Cuál es el problema de rendimiento actual?
2. ¿Qué patrones de optimización aplican aquí?
3. ¿Cuáles son los trade-offs de cada enfoque?
4. ¿Cuál recomiendas y por qué?

**Después del análisis**, genera el código optimizado.
```

### Resultado Esperado

```markdown
## Análisis Paso a Paso

### 1. Problema de Rendimiento Actual
El código actual hace:
```csharp
var allTickets = await _context.Tickets.ToListAsync();
return allTickets.Where(t => t.Status == status).Take(pageSize);
```
**Problema**: Carga TODOS los tickets a memoria antes de filtrar.
Con 10,000 tickets, esto significa:
- ~10MB de memoria por request
- Tiempo de query: O(n) donde n = total tickets
- No aprovecha índices de la BD

### 2. Patrones de Optimización Aplicables

| Patrón | Descripción | Aplicabilidad |
|--------|-------------|---------------|
| **Server-side pagination** | OFFSET/FETCH en SQL | ✅ Directamente aplicable |
| **Cursor-based pagination** | WHERE id > lastId | ✅ Para scroll infinito |
| **Specification Pattern** | Filtros componibles | ✅ Ya existe en el proyecto |
| **Compiled Queries** | Queries pre-compiladas | ⚠️ Solo si es hot path |

### 3. Trade-offs

**Server-side pagination (OFFSET/FETCH):**
- ✅ Fácil de implementar
- ✅ Funciona con cualquier ordenamiento
- ⚠️ Performance degrada en páginas muy altas (OFFSET grande)

**Cursor-based pagination:**
- ✅ Performance constante O(1)
- ✅ Ideal para feeds/scroll infinito
- ⚠️ Más complejo de implementar
- ⚠️ No permite saltar a página específica

### 4. Recomendación

**Usar Server-side pagination con Specification Pattern** porque:
1. El proyecto ya tiene `ISpecification<T>` implementado
2. Los usuarios navegan por páginas, no scroll infinito
3. Raramente acceden a páginas >100

## Código Optimizado
```csharp
public async Task<Result<PagedResponse<TicketDto>>> GetTicketsAsync(
    GetTicketsQueryParameters parameters)
{
    var spec = new TicketFilterSpecification(parameters);
    
    var query = _context.Tickets
        .AsNoTracking()
        .Where(spec.Criteria)
        .OrderByDescending(t => t.CreatedAt);
    
    var totalItems = await query.CountAsync();
    
    var tickets = await query
        .Skip((parameters.Page - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .Select(t => new TicketDto
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt
        })
        .ToListAsync();
    
    return Result<PagedResponse<TicketDto>>.Success(new PagedResponse<TicketDto>
    {
        Items = tickets,
        TotalItems = totalItems,
        Page = parameters.Page,
        PageSize = parameters.PageSize
    });
}
```

---

## 7. Técnica ReAcT

### ¿Qué es?
**Re**asoning + **Act**ion: Combina razonamiento explícito con acciones verificables.

### Estructura
```
Thought: [Razonamiento sobre qué hacer]
Action: [Acción concreta a tomar]
Observation: [Resultado de la acción]
... (repetir hasta resolver)
Final Answer: [Respuesta definitiva]
```

### Cuándo Usar

- Debugging complejo
- Investigación de errores en producción
- Tareas que requieren verificación factual
- Análisis de logs o trazas

### Ventajas ✅

- **Alta precisión**: Cada paso es verificable
- **Trazabilidad**: Puedes seguir el razonamiento
- **Reduce alucinaciones**: Fuerza verificación

### Limitaciones ⚠️

- **Muy verboso**: Genera mucho texto
- **Lento**: Múltiples ciclos de razonamiento
- **Específico**: No para generación de código directa

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
Usa el patrón ReAcT para diagnosticar este error:

**Error en producción:**
"System.InvalidOperationException: Sequence contains no elements"
Stack trace apunta a TicketService.GetByIdAsync()

**Código actual:**
```csharp
public async Task<TicketDto> GetByIdAsync(int id)
{
    var ticket = await _context.Tickets.SingleAsync(t => t.Id == id);
    return _mapper.Map<TicketDto>(ticket);
}
```

**Aplica el patrón Thought → Action → Observation hasta encontrar la causa raíz.**
```

### Resultado Esperado

```markdown
**Thought 1:** 
El error "Sequence contains no elements" ocurre en SingleAsync cuando 
no hay elementos que coincidan. Esto sugiere que se está buscando un 
ticket con ID que no existe.

**Action 1:** 
Verificar si el método valida la existencia del ticket antes de acceder.

**Observation 1:** 
El código usa `SingleAsync` directamente sin validación previa. Si el ID 
no existe, lanza excepción en lugar de retornar null.

---

**Thought 2:** 
El problema es usar `SingleAsync` vs `SingleOrDefaultAsync`. Pero también 
debería haber validación en el controlador. Revisar el flujo completo.

**Action 2:** 
Verificar cómo llega el ID al servicio.

**Observation 2:**
```csharp
// TicketsController.cs
[HttpGet("{id}")]
public async Task<ActionResult<TicketDto>> GetById(int id)
{
    var ticket = await _ticketService.GetByIdAsync(id);
    return Ok(ticket); // No valida si es null
}
```
El controlador tampoco valida. Un ID inválido propaga el error.

---

**Thought 3:** 
La solución debe:
1. Usar `SingleOrDefaultAsync` en el servicio
2. Validar nulidad y retornar Result pattern
3. El controlador debe manejar NotFound

**Action 3:** 
Generar código corregido.

**Observation 3:** 
Código corregido implementado.

---

**Final Answer:**

**Causa raíz:** `SingleAsync` lanza excepción cuando no encuentra elementos.

**Solución:**
```csharp
// TicketService.cs
public async Task<Result<TicketDto>> GetByIdAsync(int id)
{
    var ticket = await _context.Tickets
        .AsNoTracking()
        .SingleOrDefaultAsync(t => t.Id == id);
    
    if (ticket is null)
        return Result<TicketDto>.Failure($"Ticket {id} not found", "NotFound");
    
    return Result<TicketDto>.Success(_mapper.Map<TicketDto>(ticket));
}

// TicketsController.cs
[HttpGet("{id}")]
[ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<TicketDto>> GetById(int id)
{
    var result = await _ticketService.GetByIdAsync(id);
    
    if (!result.IsSuccess)
        return NotFound(new ProblemDetails { Title = result.ErrorMessage });
    
    return Ok(result.Value);
}
```
```

---

## 8. Técnica Few-Shot Prompting

### ¿Qué es?
Proporcionar **ejemplos de entrada/salida** para que el LLM aprenda el patrón.

### Cuándo Usar

- Generación de código repetitivo
- Migración de código legacy
- Aplicar patrones consistentes
- Documentación automática

### Ventajas ✅

- **Consistencia garantizada**: El output sigue el formato del ejemplo
- **Reduce ambigüedad**: El LLM "entiende" exactamente qué quieres
- **Ideal para automatización**: Scripts de generación de código

### Limitaciones ⚠️

- **Requiere buenos ejemplos**: Basura entra, basura sale
- **Consume tokens**: Los ejemplos ocupan espacio
- **Puede sobre-ajustar**: Si los ejemplos son muy específicos

### 📝 Ejemplo Aplicado: TicketManagementSystem

```markdown
Genera DTOs siguiendo EXACTAMENTE este patrón:

**Ejemplo 1 - Input (Entity):**
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**Ejemplo 1 - Output (DTOs):**
```csharp
/// <summary>
/// DTO for User responses
/// </summary>
public record UserDto(
    int Id,
    string Username,
    string Email,
    DateTime CreatedAt
);

/// <summary>
/// DTO for creating a new User
/// </summary>
public record CreateUserDto(
    [Required][MaxLength(50)] string Username,
    [Required][EmailAddress] string Email
);

/// <summary>
/// DTO for updating an existing User
/// </summary>
public record UpdateUserDto(
    [MaxLength(50)] string? Username,
    [EmailAddress] string? Email
);
```

---

**Ahora genera DTOs para esta Entity:**
```csharp
public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int TicketId { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```
```

### Resultado Esperado

```csharp
/// <summary>
/// DTO for Comment responses
/// </summary>
public record CommentDto(
    int Id,
    string Content,
    int TicketId,
    int AuthorId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

/// <summary>
/// DTO for creating a new Comment
/// </summary>
public record CreateCommentDto(
    [Required][MaxLength(1000)] string Content,
    [Required] int TicketId
);

/// <summary>
/// DTO for updating an existing Comment
/// </summary>
public record UpdateCommentDto(
    [MaxLength(1000)] string? Content
);
```

---

## 9. Comparativa de Frameworks

### Matriz de Decisión

| Framework | Complejidad Tarea | Tokens | Velocidad | Mejor Para |
|-----------|-------------------|--------|-----------|------------|
| **CARE** | Baja-Media | 🟢 Bajo | 🟢 Rápido | Tareas diarias |
| **C.O.R.E.** | Media | 🟡 Medio | 🟡 Medio | Balance general |
| **C.R.E.A.T.E.** | Alta | 🔴 Alto | 🔴 Lento | Arquitectura |
| **CLEAR** | Investigación | 🔴 Alto | 🔴 Lento | ADRs, decisiones |
| **CoT** | Razonamiento | 🔴 Alto | 🔴 Lento | Debugging |
| **ReAcT** | Verificación | 🔴 Muy Alto | 🔴 Muy Lento | Diagnóstico |
| **Few-Shot** | Repetitivo | 🟡 Medio | 🟢 Rápido | Generación masiva |

### Diagrama de Flujo: ¿Cuál Usar?

```
┌──────────────────────────────────────────────────────────────────┐
│                    ¿QUÉ FRAMEWORK USAR?                          │
└──────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌─────────────────┐
                    │ ¿Es una tarea   │
                    │ de investigación│
                    │ o decisión?     │
                    └────────┬────────┘
                             │
              ┌──────────────┴──────────────┐
              │ SÍ                          │ NO
              ▼                             ▼
        ┌──────────┐               ┌─────────────────┐
        │  CLEAR   │               │ ¿Necesitas      │
        │  o CoT   │               │ generar código  │
        └──────────┘               │ repetitivo?     │
                                   └────────┬────────┘
                                            │
                         ┌──────────────────┴──────────────┐
                         │ SÍ                              │ NO
                         ▼                                 ▼
                   ┌──────────┐               ┌─────────────────┐
                   │ Few-Shot │               │ ¿Es una tarea   │
                   │          │               │ compleja con    │
                   └──────────┘               │ muchos          │
                                              │ requisitos?     │
                                              └────────┬────────┘
                                                       │
                                    ┌──────────────────┴──────────┐
                                    │ SÍ                          │ NO
                                    ▼                             ▼
                              ┌───────────┐              ┌─────────────┐
                              │ C.R.E.A.T.E│              │ ¿Necesitas  │
                              │ o C.O.R.E. │              │ restricciones│
                              └───────────┘              │ explícitas? │
                                                         └──────┬──────┘
                                                                │
                                              ┌─────────────────┴─────────┐
                                              │ SÍ                        │ NO
                                              ▼                           ▼
                                        ┌──────────┐               ┌──────────┐
                                        │ C.O.R.E. │               │   CARE   │
                                        └──────────┘               └──────────┘
```

---

## 10. Templates Reutilizables

### 🔷 Template Rápido (CARE) - Para Tareas Diarias

```markdown
**C:** [Proyecto], [archivo actual], [tecnología versión]
**A:** [Crear/Modificar/Refactorizar] [qué cosa]
**R:** [Descripción del resultado esperado]
**E:** [Input → Output esperado]
```

### 🔷 Template Estándar (C.O.R.E.) - Para Desarrollo

```markdown
**Contexto:** 
- Proyecto: [nombre]
- Tecnología: [stack con versiones]
- Archivo: [ruta del archivo]

**Objetivo:** 
[Descripción clara de qué crear/modificar]

**Restricciones:**
- [Lo que NO debe hacer]
- [Patrones a seguir]
- [Validaciones requeridas]

**Ejemplo de salida:**
[Formato o estructura esperada]
```

### 🔷 Template Completo (C.R.E.A.T.E.) - Para Arquitectura

```markdown
## C - Context
- **Proyecto:** [nombre y descripción]
- **Stack:** [tecnologías con versiones]
- **Módulo:** [dónde se implementará]
- **Dependencias existentes:** [servicios, repos, etc.]

## R - Request
[Descripción detallada de la tarea]

## E - Examples
**Input:**
```
[ejemplo de entrada]
```
**Output esperado:**
```
[ejemplo de salida]
```

## A - Adjustments
- [Personalización 1]
- [Personalización 2]
- [Caso especial a manejar]

## T - Type of Output
- [Formato: archivo .cs, .ts, test, documentación]
- [Estructura: clase, interfaz, record]
- [Incluir: comentarios XML, tests, etc.]

## E - Extras
- [Edge cases a considerar]
- [Performance requirements]
- [Consideraciones de seguridad]
```

### 🔷 Template de Diagnóstico (ReAcT)

```markdown
**Problema:** 
[Descripción del error o comportamiento inesperado]

**Error/Síntoma:**
```
[Stack trace o mensaje de error]
```

**Código involucrado:**
```
[Código relevante]
```

**Instrucciones:**
Usa el patrón Thought → Action → Observation para:
1. Identificar la causa raíz
2. Proponer solución
3. Generar código corregido

**Final Answer debe incluir:**
- Causa raíz identificada
- Código corregido
- Prevención futura
```

---

## 11. Checklist de Selección

### ¿Cuándo usar cada framework?

#### CARE ✓
- [ ] Tarea simple o mediana
- [ ] Tiempo limitado
- [ ] No hay requisitos complejos
- [ ] El contexto es obvio

#### C.O.R.E. ✓
- [ ] Tarea estándar de desarrollo
- [ ] Necesito especificar qué NO hacer
- [ ] Quiero un ejemplo de salida
- [ ] Balance entre velocidad y detalle

#### C.R.E.A.T.E. ✓
- [ ] Tarea arquitectónica compleja
- [ ] Múltiples requisitos técnicos
- [ ] Primera vez implementando esta feature
- [ ] Necesito código listo para producción

#### CLEAR ✓
- [ ] Decisión arquitectónica
- [ ] Comparar opciones/tecnologías
- [ ] Crear documentación ADR
- [ ] Investigación técnica

#### Chain-of-Thought ✓
- [ ] Problema de razonamiento
- [ ] Debugging complejo
- [ ] Optimización de algoritmo
- [ ] Quiero entender el "por qué"

#### ReAcT ✓
- [ ] Error en producción
- [ ] Diagnóstico paso a paso
- [ ] Verificación factual necesaria
- [ ] Trazabilidad del análisis

#### Few-Shot ✓
- [ ] Código repetitivo
- [ ] Migración de patrones
- [ ] Generación masiva
- [ ] Formato muy específico

---

## 📚 Recursos Adicionales

| Recurso | Descripción |
|---------|-------------|
| [estrategias-construccion-prompts.md](../recursos/prompts/estrategias-construccion-prompts.md) | Guía detallada de construcción |
| [optimizacion-tokens-copilot.md](../recursos/prompts/optimizacion-tokens-copilot.md) | Reducir consumo de tokens |
| [copilot-prompts-2025.md](../recursos/prompts/copilot-prompts-2025.md) | Prompts específicos actualizados |

---

> **💡 Tip Final:** 
> La maestría en prompts no viene de memorizar frameworks, sino de **practicar** y **adaptar**. 
> Comienza con CARE para tareas simples, evoluciona a C.O.R.E., y reserva C.R.E.A.T.E. para arquitectura. 
> Con el tiempo, desarrollarás intuición sobre cuál usar.
