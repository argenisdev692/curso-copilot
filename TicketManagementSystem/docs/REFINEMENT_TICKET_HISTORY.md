# 🔍 Scrum Refinamiento: Ver Historial de Cambios de Tickets

## 📋 Información de la Sesión

| Campo | Valor |
|-------|-------|
| **Fecha** | 2025-11-25 |
| **Facilitador** | Scrum Master |
| **Duración** | 45 minutos |
| **Participantes** | PO, Tech Lead, Dev Team |
| **Feature** | Visualización de Historial de Cambios |

---

## 🎯 Objetivo del Refinamiento

Detallar y estimar la funcionalidad para que los usuarios puedan **visualizar el historial completo de cambios** realizados en un ticket, incluyendo cambios de estado, prioridad, asignación y descripciones.

---

## 📊 Análisis del Estado Actual

### ✅ Ya Implementado (Backend)

| Componente | Estado | Ubicación |
|------------|--------|-----------|
| Modelo `TicketHistory` | ✅ Completo | `Models/TicketHistory.cs` |
| DbSet en Context | ✅ Completo | `ApplicationDbContext.cs` |
| Endpoint `GET /api/tickets/{id}/history` | ✅ Completo | `TicketsController.cs` |
| Query CQRS `GetTicketHistoryQuery` | ✅ Completo | `CQRS/Queries/` |
| Service Method | ✅ Completo | `TicketService.cs` |
| Tests Unitarios | ✅ Parcial | `Tests/Unit/` |
| Tests Integración | ✅ Parcial | `Tests/Integration/` |

### ❌ Pendiente (Frontend)

| Componente | Estado | Descripción |
|------------|--------|-------------|
| Modelo TypeScript | ❌ Falta | `models/ticket-history.model.ts` |
| Service Method | ❌ Falta | `ticket.service.ts` - `getHistory()` |
| Componente Timeline | ❌ Falta | UI para mostrar historial |
| Integración en Detalle | ❌ Falta | Tab/Sección en vista de ticket |

---

## 📝 User Story Refinada

### US-HIST-001: Ver historial de cambios de un ticket

**Como** usuario del sistema (Admin, Agent o propietario del ticket)  
**Quiero** ver el historial completo de cambios realizados en un ticket  
**Para** entender la evolución del ticket y tener trazabilidad de las acciones realizadas

---

## 🔐 Reglas de Negocio

| # | Regla | Validación |
|---|-------|------------|
| RN-01 | Solo usuarios autorizados pueden ver el historial | Backend ya implementado |
| RN-02 | El historial se muestra en orden cronológico descendente | Más reciente primero |
| RN-03 | Cada entrada debe mostrar: fecha, usuario, campo cambiado, valor anterior, valor nuevo | Completitud de datos |
| RN-04 | Los cambios de estado y prioridad deben usar badges con colores | UX |
| RN-05 | El nombre del usuario debe ser clickeable (navegar a perfil) | Opcional - Sprint futuro |

---

## 📐 Diseño de UI/UX

### Mockup - Vista Timeline

```
┌─────────────────────────────────────────────────────────────────┐
│  📜 Historial de Cambios                              [🔄 Refresh]│
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ● 25 Nov 2025, 14:32 - Juan García                            │
│  │  Estado: [🟡 En Progreso] → [🟢 Resuelto]                   │
│  │  "Ticket completado después de aplicar hotfix"              │
│  │                                                              │
│  ● 24 Nov 2025, 10:15 - María López                            │
│  │  Asignado: Sin asignar → Juan García                        │
│  │  Prioridad: [🟡 Media] → [🔴 Alta]                          │
│  │  "Escalado por urgencia del cliente"                        │
│  │                                                              │
│  ● 23 Nov 2025, 09:00 - Carlos Ruiz                            │
│  │  ✨ Ticket creado                                            │
│  │  Estado: [🔵 Abierto]                                        │
│  │  Prioridad: [🟡 Media]                                       │
│  │                                                              │
│  └─ [Ver más antiguos...]                                       │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Componentes UI Requeridos

1. **Timeline Container** - Contenedor principal con scroll
2. **Timeline Item** - Cada entrada del historial
3. **Change Badge** - Badges para estado/prioridad
4. **User Avatar/Link** - Información del usuario que hizo el cambio
5. **Timestamp** - Fecha formateada (relativa y absoluta)

---

## 📦 Modelo de Datos (Frontend)

### TypeScript Interface

```typescript
// models/ticket-history.model.ts
export interface TicketHistory {
  id: number;
  ticketId: number;
  changedById: number;
  changedByName: string;       // Nombre del usuario (JOIN)
  changedAt: Date;
  oldStatus?: TicketStatus;
  newStatus: TicketStatus;
  oldPriority?: TicketPriority;
  newPriority: TicketPriority;
  oldAssignedToId?: number;
  newAssignedToId?: number;
  oldAssignedToName?: string;  // Nombre (JOIN)
  newAssignedToName?: string;  // Nombre (JOIN)
  changeDescription?: string;
}

export interface TicketHistoryChange {
  field: 'status' | 'priority' | 'assignee' | 'created';
  oldValue: string | null;
  newValue: string;
  oldDisplayValue?: string;    // Para mostrar nombres amigables
  newDisplayValue?: string;
}
```

---

## 🔧 Tareas Técnicas Detalladas

### Backend (Mejoras Opcionales)

| # | Tarea | SP | Prioridad |
|---|-------|----|-----------| 
| BE-01 | Crear DTO `TicketHistoryDto` con nombres de usuarios | 2 | Alta |
| BE-02 | Agregar paginación al endpoint de historial | 2 | Media |
| BE-03 | Agregar filtros por rango de fechas | 1 | Baja |
| BE-04 | Mejorar tests de integración | 1 | Media |

### Frontend (Implementación Principal)

| # | Tarea | SP | Prioridad |
|---|-------|----|-----------| 
| FE-01 | Crear modelo `TicketHistory` TypeScript | 1 | Alta |
| FE-02 | Agregar método `getTicketHistory()` en `TicketService` | 1 | Alta |
| FE-03 | Crear componente `TicketHistoryTimeline` | 3 | Alta |
| FE-04 | Crear componente `TicketHistoryItem` | 2 | Alta |
| FE-05 | Integrar en vista de detalle del ticket | 2 | Alta |
| FE-06 | Implementar loading skeleton | 1 | Media |
| FE-07 | Implementar "Ver más" / paginación | 2 | Media |
| FE-08 | Tests de componentes | 2 | Alta |

---

## 📊 Estimación Final

### Story Points por Área

| Área | Story Points | Complejidad |
|------|--------------|-------------|
| Backend (mejoras) | 6 | Baja |
| Frontend (implementación) | 14 | Media |
| **TOTAL** | **20** | **Media** |

### Desglose de Complejidad

```
Complejidad = (Esfuerzo × Riesgo × Incertidumbre)

Esfuerzo:       Media (Frontend nuevo, Backend existente)
Riesgo:         Bajo (API ya probada, no hay lógica compleja)
Incertidumbre:  Baja (Requerimientos claros, UI definida)

Resultado: COMPLEJIDAD MEDIA - 20 SP
```

### Estimación por Técnica Planning Poker

| Tarea | Dev 1 | Dev 2 | Dev 3 | Consenso |
|-------|-------|-------|-------|----------|
| Modelo TS + Service | 2 | 1 | 2 | **2** |
| Timeline Component | 3 | 5 | 3 | **3** |
| History Item | 2 | 3 | 2 | **2** |
| Integración | 2 | 2 | 3 | **2** |
| Tests | 2 | 3 | 2 | **2** |
| Backend DTO | 2 | 2 | 1 | **2** |

---

## ✅ Criterios de Aceptación Detallados

### Funcionales

- [ ] **AC-01**: Al ver el detalle de un ticket, existe una sección/tab "Historial"
- [ ] **AC-02**: El historial muestra todos los cambios ordenados por fecha descendente
- [ ] **AC-03**: Cada cambio muestra: fecha, usuario, tipo de cambio, valores antes/después
- [ ] **AC-04**: Los cambios de estado muestran badges con colores correspondientes
- [ ] **AC-05**: Los cambios de prioridad muestran badges con colores correspondientes
- [ ] **AC-06**: Los cambios de asignación muestran el nombre del usuario
- [ ] **AC-07**: Si hay descripción del cambio, se muestra debajo de la entrada
- [ ] **AC-08**: El primer registro (creación) se muestra de forma especial

### No Funcionales

- [ ] **AC-09**: El historial carga en menos de 2 segundos para tickets con <100 entradas
- [ ] **AC-10**: Se muestra skeleton loading mientras carga
- [ ] **AC-11**: Manejo de errores con mensaje amigable si falla la carga
- [ ] **AC-12**: Responsive: funciona correctamente en móvil
- [ ] **AC-13**: Accesibilidad: navegable con teclado, lectores de pantalla

---

## 🔗 Dependencias

### Internas

| Dependencia | Tipo | Estado |
|-------------|------|--------|
| API `/api/tickets/{id}/history` | Backend | ✅ Implementado |
| Modelo `TicketHistory` (Backend) | Backend | ✅ Implementado |
| Vista de Detalle de Ticket | Frontend | ✅ Existente |
| `TicketService` | Frontend | ✅ Existente |

### Externas

| Dependencia | Tipo | Acción |
|-------------|------|--------|
| Ninguna | - | - |

---

## ⚠️ Riesgos Identificados

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| API no retorna nombres de usuarios | Alta | Medio | Crear DTO en backend con JOINs |
| Performance con muchos registros | Baja | Medio | Implementar paginación |
| Inconsistencia en datos históricos | Baja | Bajo | Validar datos existentes |

---

## 📋 Definition of Ready (DoR) ✅

- [x] User Story escrita con formato estándar
- [x] Criterios de aceptación definidos y verificables
- [x] Mockups de UI disponibles
- [x] Dependencias identificadas y resueltas
- [x] Estimación consensuada por el equipo
- [x] Tareas técnicas desglosadas
- [x] Sin blockers conocidos

---

## 📋 Definition of Done (DoD)

- [ ] Código completo y funcionando
- [ ] Code review aprobado
- [ ] Tests unitarios con >80% cobertura
- [ ] Tests E2E para flujo principal
- [ ] Sin errores de lint/build
- [ ] Documentación actualizada
- [ ] Desplegado en ambiente de QA
- [ ] Aprobación de QA
- [ ] Demo al PO completada

---

## 📝 Notas del Refinamiento

### Decisiones Tomadas

1. **Ubicación en UI**: Se implementará como tab "Historial" en la vista de detalle del ticket (no como modal)
2. **Paginación**: Inicial sin paginación, se agrega si el performance lo requiere
3. **Backend DTO**: Se creará nuevo DTO que incluya nombres de usuarios para evitar N+1 queries

### Preguntas para el PO

1. ¿Se requiere exportar el historial a PDF/Excel? → **Diferido a Sprint futuro**
2. ¿El historial debe incluir cambios en comentarios? → **No, solo cambios en el ticket**
3. ¿Usuarios regulares pueden ver historial de sus tickets? → **Sí, si son creadores o asignados**

### Deuda Técnica Identificada

- El endpoint actual retorna el modelo de Entity Framework directamente, debería usar un DTO
- Faltan índices optimizados para consultas de historial por rango de fechas

---

## 📅 Sprint Backlog Sugerido

### Sprint 8 (si se incluye esta feature)

| # | Item | SP | Asignado |
|---|------|----|---------| 
| 1 | BE-01: Crear TicketHistoryDto | 2 | Backend Dev |
| 2 | FE-01 + FE-02: Modelo y Service | 2 | Frontend Dev |
| 3 | FE-03 + FE-04: Componentes Timeline | 5 | Frontend Dev |
| 4 | FE-05: Integración en detalle | 2 | Frontend Dev |
| 5 | FE-06 + FE-07: Loading y paginación | 3 | Frontend Dev |
| 6 | FE-08: Tests | 2 | Frontend Dev |
| **TOTAL** | | **16** | |

---

## 📊 Diagrama de Secuencia

```
┌─────────┐          ┌─────────────┐         ┌───────────────┐         ┌────────────┐
│ Usuario │          │   Angular   │         │    API        │         │    DB      │
└────┬────┘          └──────┬──────┘         └───────┬───────┘         └─────┬──────┘
     │                      │                        │                       │
     │  Click "Historial"   │                        │                       │
     │─────────────────────>│                        │                       │
     │                      │                        │                       │
     │                      │  GET /tickets/1/history│                       │
     │                      │───────────────────────>│                       │
     │                      │                        │                       │
     │                      │                        │  SELECT * FROM        │
     │                      │                        │  TicketHistories      │
     │                      │                        │  JOIN Users           │
     │                      │                        │──────────────────────>│
     │                      │                        │                       │
     │                      │                        │<──────────────────────│
     │                      │                        │      Results          │
     │                      │<───────────────────────│                       │
     │                      │    TicketHistoryDto[]  │                       │
     │                      │                        │                       │
     │   Render Timeline    │                        │                       │
     │<─────────────────────│                        │                       │
     │                      │                        │                       │
```

---

## 🔖 Changelog del Refinamiento

| Versión | Fecha | Cambio |
|---------|-------|--------|
| 1.0 | 2025-11-25 | Refinamiento inicial creado |

---

*Documento generado durante sesión de refinamiento - Sprint Planning*
