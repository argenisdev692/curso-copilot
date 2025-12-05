---
description: 'Analiza la tarea y selecciona automáticamente el framework de prompt óptimo'
---

# 🧭 Router de Frameworks de Prompts

Analiza la tarea del usuario y **selecciona automáticamente** el framework de prompt más adecuado, luego genera el prompt optimizado.

## 📥 Entrada del Usuario

- **Tarea**: {{tarea}}
- **Contexto Adicional** (opcional): {{contexto}}

---

## 🔍 Proceso de Análisis (Ejecutar Siempre)

### Paso 1: Clasificación de Tipo de Tarea

```
┌─────────────────────────────────────────────────────────────────────┐
│                    CLASIFICACIÓN INICIAL                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ¿Qué tipo de tarea es?                                            │
│                                                                     │
│  🔨 IMPLEMENTAR CÓDIGO                                              │
│     └── Continuar a Paso 2A                                        │
│                                                                     │
│  🔬 INVESTIGAR / DECIDIR                                           │
│     └── Framework: CLEAR                                           │
│                                                                     │
│  🐛 DIAGNOSTICAR ERROR                                              │
│     └── Continuar a Paso 2B                                        │
│                                                                     │
│  🔄 CÓDIGO REPETITIVO / MIGRACIÓN                                  │
│     └── Framework: Few-Shot                                        │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Paso 2A: Análisis de Complejidad (Para Implementación)

| # | Criterio | Pregunta Clave | Peso |
|---|----------|----------------|------|
| 1 | **Archivos** | ¿Cuántos archivos generará? | +1 por archivo >1 |
| 2 | **Primera vez** | ¿Es primera implementación de esto en el proyecto? | +2 si es nueva |
| 3 | **Integración** | ¿Conecta con sistema externo (API, DB, Queue, etc.)? | +2 si conecta |
| 4 | **Resiliencia** | ¿Requiere retry, circuit breaker, DLQ? | +2 si requiere |
| 5 | **Producción** | ¿Es código crítico (pagos, auth, datos sensibles)? | +2 si es crítico |
| 6 | **Edge cases** | ¿Hay múltiples escenarios de error a manejar? | +1 por cada >2 |

**Scoring**:
- **0-2 puntos** → CARE
- **3-5 puntos** → C.O.R.E.
- **6+ puntos** → C.R.E.A.T.E.

### Paso 2B: Análisis de Diagnóstico (Para Errores)

| Criterio | Sí → | No → |
|----------|------|------|
| ¿Tengo stack trace? | ReAcT | Chain-of-Thought |
| ¿Es error de lógica/algoritmo? | Chain-of-Thought | ReAcT |
| ¿Necesito verificar datos externos? | ReAcT | Chain-of-Thought |

---

## 📋 Frameworks Disponibles

### 🟢 CARE - Tareas Rápidas (~30-50 tokens)

```
C - Context: [contexto mínimo]
A - Action: [qué hacer]
R - Result: [qué esperar]
E - Example: [ejemplo simple]
```

**Usar cuando**:
- ✅ Tarea se completa en <30 min
- ✅ Un solo archivo
- ✅ Sin edge cases complejos
- ✅ Patrón conocido/repetido

**Ejemplos**: DTO, validador simple, utility function, fix pequeño.

---

### 🟡 C.O.R.E. - Balance General (~50-80 tokens)

```
C - Context: [stack + archivos + patrones]
O - Objective: [qué generar específicamente]
R - Restrictions: [límites, requisitos, evitar]
E - Example: [formato de salida esperado]
```

**Usar cuando**:
- ✅ 1-3 archivos relacionados
- ✅ Hay reglas de negocio
- ✅ Necesito especificar restricciones
- ✅ Quiero ejemplo de output

**Ejemplos**: Endpoint API, componente Angular, servicio con lógica, repository.

---

### 🔴 C.R.E.A.T.E. - Tareas Complejas (~80-150 tokens)

```
C - Context: [proyecto + stack completo + patrones actuales]
R - Request: [funcionalidad detallada + componentes]
E - Examples: [JSON entrada/salida + config]
A - Adjustments: [patrones resiliencia + configuración]
T - Type of Output: [lista archivos + estructura carpetas]
E - Extras: [edge cases + consideraciones producción]
```

**Usar cuando**:
- ✅ Primera vez implementando
- ✅ Múltiples archivos (>3)
- ✅ Integración externa
- ✅ Patrones de resiliencia
- ✅ Código para producción

**Ejemplos**: RabbitMQ, JWT Auth, Gateway API, Microservicio, Event Sourcing.

---

### 🔵 CLEAR - Decisiones Técnicas (~60-100 tokens)

```
C - Challenge: [problema o decisión a tomar]
L - Limits: [restricciones del proyecto]
E - Evaluate: [opciones a comparar]
A - Analyze: [criterios de evaluación]
R - Recommend: [formato de recomendación esperado]
```

**Usar cuando**:
- ✅ Comparar tecnologías/patrones
- ✅ Documentar el "por qué"
- ✅ Crear ADR (Architecture Decision Record)
- ✅ Evaluar trade-offs

**Ejemplos**: ¿SignalR vs WebSockets?, ¿SQL vs NoSQL?, ¿Monolito vs Microservicios?

---

### 🟣 Chain-of-Thought - Razonamiento (~40-70 tokens)

```
Problema: [descripción del problema]
Piensa paso a paso:
1. [primer aspecto a analizar]
2. [segundo aspecto]
3. [conclusión esperada]
```

**Usar cuando**:
- ✅ Problema de lógica compleja
- ✅ Optimización de algoritmo
- ✅ Debugging sin stack trace
- ✅ Necesito entender el "cómo"

**Ejemplos**: Query lenta, refactoring complejo, diseño de algoritmo.

---

### 🟠 ReAcT - Diagnóstico Iterativo (~50-90 tokens)

```
Observation: [qué observo - error, log, comportamiento]
Thought: [hipótesis inicial]
Action: [qué verificar/probar]
Expected: [qué debería pasar si la hipótesis es correcta]
```

**Usar cuando**:
- ✅ Error en producción con stack trace
- ✅ Necesito trazabilidad del análisis
- ✅ Verificación factual necesaria

**Ejemplos**: Exception en producción, comportamiento inesperado, análisis de logs.

---

### ⚪ Few-Shot - Patrones Repetitivos (~40-80 tokens)

```
Patrón a seguir:
[Ejemplo 1 Input] → [Ejemplo 1 Output]
[Ejemplo 2 Input] → [Ejemplo 2 Output]

Aplicar a:
[Lista de items a transformar]
```

**Usar cuando**:
- ✅ Tengo ejemplos de referencia
- ✅ Código repetitivo
- ✅ Migración de formato
- ✅ Consistencia de estilo

**Ejemplos**: Generar DTOs desde Entities, migrar JS a TS, crear tests similares.

---

## 📤 Formato de Salida del Router

```markdown
## 🧭 Análisis de Tarea

### 📥 Tarea Recibida
> [tarea del usuario]

### 🔍 Clasificación

| Aspecto | Evaluación |
|---------|------------|
| Tipo de Tarea | 🔨 Implementar / 🔬 Investigar / 🐛 Diagnosticar / 🔄 Migrar |
| Archivos Estimados | [1 / 2-3 / 4+] |
| Complejidad | [Baja / Media / Alta] |
| Score de Complejidad | [X/10 puntos] |

### ✅ Framework Seleccionado

| Framework | Razón de Selección |
|-----------|-------------------|
| **[FRAMEWORK]** | [justificación basada en criterios] |

### ❌ Frameworks Descartados

| Framework | Por qué NO |
|-----------|-----------|
| [Framework 1] | [razón] |
| [Framework 2] | [razón] |

---

## 📝 Prompt Generado

### 🎯 [Framework Seleccionado] - Versión Optimizada

```
[Prompt generado siguiendo el formato del framework]
```

| Aspecto | Valor |
|---------|-------|
| Tokens Estimados | ~XX |
| Cobertura | ⭐⭐⭐⭐ |
| Iteraciones Esperadas | [1 / 2-3] |

---

## 🚀 Siguiente Paso

📋 **Copia el prompt y úsalo en un nuevo chat**:
```
[prompt listo para copiar]
```

### 💡 MCPs Recomendados (si aplica)

| MCP | Uso | Cuándo Sugerir |
|-----|-----|----------------|
| @context7 /[lib] | Documentación oficial | Siempre para código nuevo |
| @tavily | Mejores prácticas web | Integraciones, patrones avanzados |
| @workspace | Contexto del proyecto | Siempre |

### 📊 Tabla de MCPs por Tecnología

| Tema Detectado | @context7 | @tavily (si aplica) |
|----------------|-----------|---------------------|
| Angular Frontend | `/angular` | "Angular [feature] best practices" |
| .NET/C# Backend | `/dotnet`, `/aspnetcore` | ".NET [feature] production patterns" |
| EF Core/SQL Server | `/efcore` | "EF Core [pattern] performance" |
| RabbitMQ/Mensajería | `/rabbitmq` | "RabbitMQ .NET resilience patterns" |
| Docker/.NET | `/docker` | "Docker .NET container optimization" |
| JWT/Auth | `/aspnetcore` | "JWT authentication .NET Angular" |
| xUnit/Testing | `/xunit` | ".NET testing best practices" |
| SignalR | `/signalr` | "SignalR .NET real-time patterns" |
| Redis/Cache | `/redis` | "Redis .NET distributed cache" |
| gRPC | `/grpc` | "gRPC .NET microservices" |

### 🚀 Formato de Sugerencia (Incluir Siempre)

```
---

## 🚀 Siguiente Paso: Implementa con MCPs

📖 **Documentación Oficial**:
@context7 /[tecnología detectada] [prompt generado]

🌐 **Mejores Prácticas** (si es integración/patrón avanzado):
@tavily [tema] best practices 2025

📂 **Contexto del Proyecto**:
@workspace [prompt generado]

💡 **Tip**: Usa @context7 primero para docs oficiales, luego @tavily para patrones de producción.
```
```

---

## 🔗 Reglas para Sugerir MCPs

### Cuándo Incluir MCPs en la Respuesta

| Framework Seleccionado | Sugerir MCPs | Razón |
|------------------------|--------------|-------|
| **CARE** | ⚠️ Opcional | Solo si es componente UI o integración simple |
| **C.O.R.E.** | ✅ Sí | Componentes y endpoints necesitan patrones actualizados |
| **C.R.E.A.T.E.** | ✅ **Siempre** | Tareas complejas requieren documentación oficial |
| **CLEAR** | ✅ Sí | Decisiones necesitan información actualizada |
| **Chain-of-Thought** | ❌ No | Es razonamiento, no implementación |
| **ReAcT** | ❌ No | Es diagnóstico, no código nuevo |
| **Few-Shot** | ⚠️ Opcional | Solo si la migración involucra nueva tecnología |

### Detección Automática de Tecnología

```
Palabras clave en tarea → MCP sugerido:
- "Angular", "component", "frontend" → @context7 /angular
- ".NET", "API", "Controller", "C#" → @context7 /aspnetcore
- "EF Core", "DbContext", "migration" → @context7 /efcore
- "RabbitMQ", "queue", "message" → @context7 /rabbitmq
- "JWT", "auth", "token" → @context7 /aspnetcore + @tavily auth
- "test", "xUnit", "mock" → @context7 /xunit
- "SignalR", "realtime", "hub" → @context7 /signalr
- "Docker", "container" → @context7 /docker
- "Redis", "cache" → @context7 /redis
```

---

## 🎯 Ejemplos de Routing (Con MCPs)

### Ejemplo 1: "Agregar campo email a UserDTO"
```
Clasificación: Implementar → 1 archivo → Sin edge cases
Score: 1/10
→ Framework: CARE
→ MCPs: ❌ No necesario (tarea trivial)

Prompt generado:
C: UserDTO.cs en /DTOs
A: Agregar prop Email string + validación
R: DTO actualizado con [EmailAddress]
E: public string Email { get; set; }
```

### Ejemplo 2: "Crear endpoint de búsqueda de tickets con filtros"
```
Clasificación: Implementar → 3 archivos → Lógica de negocio
Score: 4/10
→ Framework: C.O.R.E.
→ MCPs: ✅ @context7 /aspnetcore, /efcore

Prompt generado:
C: .NET 8 API + EF Core | TicketsController.cs
O: GET /tickets/search con filtros (status, date, assignee) + paginación
R: Usar IQueryable, DTO response, <200ms, validar params
E: { tickets: [...], total: 100, page: 1, pageSize: 20 }

---
🚀 MCPs:
@context7 /aspnetcore [prompt arriba]
@context7 /efcore pagination IQueryable best practices
```

### Ejemplo 3: "Integrar RabbitMQ para notificaciones"
```
Clasificación: Implementar → 7+ archivos → Integración externa → Resiliencia
Score: 8/10
→ Framework: C.R.E.A.T.E.
→ MCPs: ✅ @context7 /rabbitmq + @tavily resilience

Prompt generado:
C: .NET 8 + MediatR + TicketManagementSystem | #TicketService.cs
R: Publisher + Consumer para eventos de tickets | RabbitMQ.Client
E: {"ticketId": 1, "event": "Created", "timestamp": "..."} | config en appsettings
A: Retry exponencial (3x), DLQ, CorrelationId, IOptions<RabbitMQSettings>
T: 7 archivos: Settings, Connection, Publisher, Consumer, Events (2), DI Extension
E: Edge: RabbitMQ caído → fallback log | Duplicados → idempotencia por messageId

---
🚀 MCPs:
@context7 /rabbitmq [prompt arriba]
@tavily RabbitMQ .NET resilience patterns dead letter queue 2025
```

### Ejemplo 4: "¿Debería usar Redis o Memcached para caché?"
```
Clasificación: Investigar/Decidir
→ Framework: CLEAR
→ MCPs: ✅ @tavily para comparación actualizada

Prompt generado:
C: Decidir sistema de caché distribuido para TicketManagementSystem
L: .NET 8, Azure, presupuesto medio, equipo sin experiencia Redis
E: Redis vs Memcached vs Azure Cache
A: Performance, features, costo Azure, curva aprendizaje, persistencia
R: ADR con recomendación + pros/cons + ejemplo config

---
🚀 MCPs:
@tavily Redis vs Memcached .NET Azure comparison 2025
@context7 /redis distributed cache .NET
```

### Ejemplo 5: "NullReferenceException en GetTicketById"
```
Clasificación: Diagnosticar → Con stack trace
→ Framework: ReAcT
→ MCPs: ❌ No necesario (diagnóstico, no código nuevo)

Prompt generado:
Observation: NullReferenceException en TicketService.GetTicketById línea 45
Thought: Posible ticket no encontrado o navegación nula
Action: Verificar si repo retorna null + revisar .Include()
Expected: Identificar punto exacto de null + sugerir null check o Result pattern
```

---

## 📚 Referencias Cruzadas

Para generar prompts con más detalle, usa estos comandos en Copilot Chat:

### 🔧 Generadores de Prompts (misma carpeta)

**Generar prompt C.O.R.E.:**
```
#file:generate-prompt-CORE.prompt.md
```

**Generar prompt C.R.E.A.T.E.:**
```
#file:generate-prompt-CREATE.prompt.md
```

### 📖 Documentación Adicional

Archivos de referencia en `recursos/prompts/`:
- `analisis-seleccion-frameworks.md` → Criterios de selección
- `frameworks-prompts-analisis-completo.md` → Todos los frameworks
- `optimizacion-tokens-copilot.md` → Optimizar tokens

---

## 💡 Tips de Uso

1. **Si no estás seguro**: El router te guía automáticamente
2. **Si el score está en límite (5-6)**: Usa C.O.R.E. primero, escala a C.R.E.A.T.E. si falta detalle
3. **Para código crítico**: Siempre usa C.R.E.A.T.E. sin importar el score
4. **Para prototipos**: Baja un nivel (C.R.E.A.T.E. → C.O.R.E., C.O.R.E. → CARE)
