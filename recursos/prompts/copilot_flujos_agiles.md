# Tema 12: Copilot en Flujos Ágiles y Gestión de Proyectos

> **Proyecto de referencia**: TicketManagementSystem (.NET 9 + Angular 19 + SQL Server)

---

## 🎯 ¿GitHub Copilot se integra con GitHub Projects?

### Respuesta Corta: **SÍ, indirectamente**

GitHub Copilot no tiene un panel dentro de GitHub Projects, pero se integra así:

| Integración | Cómo Funciona |
|-------------|---------------|
| **Copilot Coding Agent** | Asignas un Issue a `@copilot` y automáticamente crea un PR |
| **Issue Generation** | Desde Copilot Chat puedes crear Issues estructurados |
| **PR desde Issues** | `@github Open a PR to fix issue #123` |
| **Epics/Sub-issues** | Copilot puede generar árbol de issues desde una descripción |

### Flujo Real
```
GitHub Projects (Kanban) 
    ↓
Issue asignado a @copilot
    ↓
Copilot crea branch + código + PR
    ↓
PR review (humano)
    ↓
Merge → Issue se mueve a "Done"
```

---

## 🏃 Sprint Completo Asistido por IA

### Ejemplo: Sprint de 2 semanas para TicketManagementSystem

#### 📋 Día 1: Planning con Copilot

**Prompt para generar backlog desde épica:**
```
Genera Issues para épica: "Sistema de notificaciones para TicketManagementSystem"

Sub-features:
- Notificaciones por email cuando ticket cambia de estado
- Notificaciones in-app en tiempo real
- Preferencias de usuario para notificaciones
- Historial de notificaciones

Formato: título, descripción, criterios de aceptación, estimación (S/M/L)
Etiquetas: backend, frontend, database
```

**Resultado esperado:**
```markdown
## Issue 1: [Backend] Servicio de notificaciones por email
**Estimación:** M (5 puntos)
**Etiquetas:** backend, feature

### Descripción
Implementar servicio que envíe emails cuando un ticket cambie de estado.

### Criterios de Aceptación
- [ ] Enviar email al creador cuando ticket pase a "En Progreso"
- [ ] Enviar email al asignado cuando se le asigne ticket
- [ ] Template HTML configurable
- [ ] Cola de emails con retry

### Notas técnicas
- Usar SendGrid/SMTP configurado en appsettings
- Implementar INotificationService
```

---

#### 📋 Día 2-3: Desarrollo con Copilot

**Issue #1: Crear NotificationService**

```
Crea INotificationService para TicketManagementSystem

Métodos:
- SendTicketStatusChangedAsync(ticketId, oldStatus, newStatus)
- SendTicketAssignedAsync(ticketId, assigneeId)
- SendCommentAddedAsync(ticketId, commentId)

Inyectar: IEmailService, IUserRepository, ITicketRepository
Patrón: async, Result<T>, logging
```

**Issue #2: Crear endpoint de preferencias**
```
Endpoint CRUD para UserNotificationPreferences en TicketManagementSystem

DTO: userId, emailOnStatusChange, emailOnAssignment, emailOnComment
Ruta: /api/users/{id}/notification-preferences
Auth: solo el propio usuario o Admin
```

---

#### 📋 Día 4-5: Code Review con Copilot

**Prompt para review:**
```
Revisa #selection por:
- Seguridad (auth, injection)
- Performance (N+1, async)
- Patrones del proyecto (Result<T>, DTOs)

Formato: ✅ OK / ⚠️ Sugerencia / ❌ Problema
```

---

#### 📋 Día 6-8: Testing con Copilot

```
Tests unitarios para NotificationService

Escenarios:
- SendTicketStatusChangedAsync: éxito, usuario no encontrado, email falla
- Verificar que respeta preferencias del usuario

Framework: xUnit + NSubstitute
Patrón: Arrange/Act/Assert
```

---

#### 📋 Día 9-10: Deploy y Documentación

```
Actualiza README de TicketManagementSystem

Agregar sección: "Sistema de Notificaciones"
Incluir: configuración SMTP, variables de entorno, troubleshooting
```

---

## 🔄 Roles de Copilot en Flujos Ágiles

### Scrum

| Ceremonia | Uso de Copilot |
|-----------|----------------|
| **Planning** | Generar Issues desde épicas, estimar basado en código similar |
| **Daily** | "¿Qué issues tienen blockers técnicos?" basado en código |
| **Review** | Generar resumen de cambios del sprint |
| **Retro** | Analizar métricas de PRs (tiempo merge, comments) |

### Prompts por Ceremonia

**Sprint Planning:**
```
Descompone feature "[NOMBRE]" en tareas técnicas para TicketManagementSystem

Considerar: backend (.NET 9), frontend (Angular 19), DB (SQL Server)
Formato: Issue con descripción, AC, estimación
Dependencias entre tareas
```

**Sprint Review:**
```
Genera release notes para TicketManagementSystem desde commits de las últimas 2 semanas

Formato: Added, Changed, Fixed
Audiencia: stakeholders no técnicos
```

**Retrospectiva:**
```
Analiza PRs del sprint en #file:CHANGELOG.md

Identificar: PRs con muchos comentarios, tiempo promedio de merge, patterns repetidos
Sugerencias de mejora para próximo sprint
```

---

### Kanban

| Columna | Automatización con Copilot |
|---------|---------------------------|
| **Backlog** | `@copilot` genera Issues desde conversaciones |
| **Ready** | Copilot valida que Issue tenga AC completos |
| **In Progress** | Asignar Issue a `@copilot` para que code |
| **Review** | Copilot sugiere reviewers basado en CODEOWNERS |
| **Done** | Auto-genera changelog entry |

---

## 📦 Copilot en Monorepo (Tu Proyecto)

Tu proyecto TicketManagementSystem ya es un **monorepo**:
```
TicketManagementSystem/
├── backend/          # .NET 9 API
├── frontend/         # Angular 19
├── docs/             # Documentación
```

### Prompts Específicos para Monorepo

**Cambio cross-cutting (afecta backend + frontend):**
```
Implementa feature "filtro de tickets por fecha" en TicketManagementSystem

Backend: nuevo query param en GET /api/tickets?fromDate=&toDate=
Frontend: componente date-range-picker + actualizar servicio

Generar ambos cambios coordinados
```

**Contexto específico por carpeta:**
```
@workspace En backend/ implementa [FEATURE]
```

```
@workspace En frontend/ crea componente para [FEATURE]
```

**Validar consistencia:**
```
Verifica que DTOs en backend/TicketManagementSystem.API/DTOs/ 
coincidan con interfaces en frontend/src/app/models/

Listar discrepancias: propiedad, tipo, obligatoriedad
```

### Configuración Recomendada para Monorepo

Crear archivo `.github/copilot-instructions.md`:
```markdown
# Copilot Instructions para TicketManagementSystem

## Estructura
- backend/: .NET 9, EF Core, SQL Server
- frontend/: Angular 19, TypeScript strict

## Convenciones Backend
- Usar Result<T> para operaciones que pueden fallar
- DTOs en carpeta DTOs/, sufijo Dto
- Validación con FluentValidation
- Async en todo I/O

## Convenciones Frontend  
- Standalone components
- Signals para estado
- Servicios con inject()
- Interfaces en models/

## NO hacer
- Exponer entities en controllers
- Console.log en producción
- any en TypeScript
```

---

## 🔗 Integración con GitHub Projects

### Setup Recomendado

1. **Crear Project en GitHub** (tipo Board/Kanban)
2. **Columnas sugeridas:**
   - 📥 Backlog
   - 📋 Ready (con AC completos)
   - 🤖 Copilot Working (asignado a @copilot)
   - 👀 In Review
   - ✅ Done

3. **Automations del Project:**
   - Issue creado → Backlog
   - PR abierto → In Review
   - PR merged → Done

### Flujo con Copilot Coding Agent

```
1. Crear Issue en GitHub Projects
   "Agregar endpoint de estadísticas de tickets"

2. Agregar detalles con Copilot Chat:
   @github "Expande este issue con criterios de aceptación técnicos"

3. Asignar a @copilot:
   - Issue se mueve a "Copilot Working"
   - Copilot crea branch, escribe código, abre PR

4. Review humano:
   - PR tiene comentarios → @copilot responde/corrige
   - Aprobado → Merge

5. Auto-move a Done
```

### Prompts para GitHub Projects

**Crear Issue desde chat:**
```
@github Crea issue en TicketManagementSystem:

Título: Implementar paginación en lista de tickets
Labels: enhancement, frontend, backend
Project: Sprint 5
Milestone: v2.1.0

Descripción y AC automáticos basados en código existente
```

**Asignar trabajo a Copilot:**
```
@github Asigna issue #45 a Copilot para que implemente la solución
```

**Generar épica con sub-issues:**
```
@github Crea épica "Reportes y Dashboards" con sub-issues:
- Dashboard de tickets por estado
- Reporte de tickets por usuario
- Exportación a Excel
- Gráficos de tendencias

Asignar estimaciones y labels automáticamente
```

---

## ⚡ Reducción de Tiempo en PRs y Merges

### Antes vs Después con Copilot

| Tarea | Sin Copilot | Con Copilot | Ahorro |
|-------|-------------|-------------|--------|
| Escribir código | 2-4 hrs | 30-60 min | 70% |
| Escribir tests | 1-2 hrs | 15-30 min | 75% |
| Code review inicial | 30 min | 10 min | 66% |
| Resolver comments | 30 min | 10 min | 66% |
| Escribir PR description | 15 min | 2 min | 87% |

### Prompts para PRs Rápidos

**Generar PR description:**
```
Genera descripción de PR para los cambios en #selection

Formato:
## Cambios
## Testing
## Screenshots (si aplica)
## Checklist
```

**Review automático antes de submit:**
```
Pre-review de mi PR:

Verificar:
- [ ] Tests pasan
- [ ] Sin secrets hardcodeados
- [ ] DTOs actualizados
- [ ] Changelog entry

Listar issues encontrados
```

**Responder a review comments:**
```
El reviewer comentó: "[COMENTARIO]"
En el código: #selection

Sugiere fix o explica por qué está bien así
```

**Resolver conflictos:**
```
Resuelve conflicto de merge en #file

Mantener: cambios de mi branch para feature X
Integrar: cambios de main para bugfix Y
```

---

## 📋 Resumen de Prompts por Fase

| Fase | Prompt Clave |
|------|--------------|
| Planning | `Descompone épica "[X]" en Issues técnicos` |
| Development | `Implementa [feature] en [backend/frontend]` |
| Testing | `Tests para #selection, escenarios: [listar]` |
| Review | `Revisa #selection por seguridad y patterns` |
| PR | `Genera PR description para cambios actuales` |
| Merge | `Resuelve conflicto en #file, priorizar [X]` |
| Release | `Changelog desde commits de últimas 2 semanas` |

---

## 🎯 Ejercicio Práctico: Mini-Sprint

```
Simular sprint de 1 día para TicketManagementSystem:

1. Planning (5 min):
   "Genera 3 Issues para feature: filtros avanzados de tickets"

2. Development (15 min):
   "Implementa filtro por prioridad en backend"
   "Implementa dropdown de prioridad en frontend"

3. Testing (5 min):
   "Tests para el nuevo endpoint de filtros"

4. PR (5 min):
   "Genera PR description"
   "Pre-review de seguridad"

5. Merge + Release notes (5 min):
   "Changelog entry para filtros avanzados"
```
