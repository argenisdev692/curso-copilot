---
description: 'Router avanzado de frameworks de prompts con técnicas 2025 - Meta-Prompting, Self-Consistency, Negative Prompting'
---

# 🧭 Router de Frameworks de Prompts v2.0 (2025)

> **Actualizado con técnicas de Prompt Engineering 2025**: Meta-Prompting, Self-Consistency, Negative Prompting, Tree of Thoughts, Context Engineering

Analiza la tarea del usuario y **selecciona automáticamente** el framework de prompt más adecuado, luego genera el prompt optimizado con técnicas avanzadas.

## 📥 Entrada del Usuario

- **Tarea**: {{tarea}}
- **Contexto Adicional** (opcional): {{contexto}}
- **Criticidad** (opcional): {{criticidad}} → Alta = activar Self-Consistency
- **Iteración** (opcional): {{iteracion}} → Si es retry, activar Meta-Prompting

---

## 🆕 Técnicas Avanzadas 2025 (Aplicar según contexto)

### 🧬 Meta-Prompting
> **Cuándo**: El prompt inicial no dio buenos resultados, necesitas optimizar automáticamente

El LLM mejora el prompt antes de ejecutarlo:
```
Antes de responder, analiza este prompt y mejóralo:
- ¿Es claro y específico?
- ¿Faltan restricciones importantes?
- ¿El formato de salida está definido?
Genera el prompt mejorado y luego responde.
```

### 🎯 Self-Consistency (Para código crítico)
> **Cuándo**: Pagos, Auth, datos sensibles, decisiones arquitectónicas

Genera múltiples soluciones y elige la más consistente:
```
Genera 3 soluciones diferentes para este problema.
Para cada una: implementación + pros/cons + score confianza (1-10).
Selecciona la más consistente y explica por qué.
```

### ❌ Negative Prompting
> **Cuándo**: Siempre que haya anti-patterns conocidos

Añadir al final de cada prompt:
```
❌ NO uses: [patrones obsoletos]
❌ NO generes: [anti-patterns del stack]
❌ EVITA: [errores comunes de la tecnología]
```

### 🌳 Tree of Thoughts (ToT)
> **Cuándo**: Problemas con múltiples soluciones válidas

Explora múltiples rutas de razonamiento:
```
Explora 3 enfoques diferentes para resolver esto:
1. [Enfoque A] → Evalúa viabilidad
2. [Enfoque B] → Evalúa viabilidad  
3. [Enfoque C] → Evalúa viabilidad
Elige el mejor y justifica.
```

### 📐 Least-to-Most
> **Cuándo**: Tareas muy complejas que abruman al modelo

Descomponer en sub-problemas:
```
Divide esta tarea en pasos más pequeños:
1. Primero resuelve: [sub-problema 1]
2. Luego: [sub-problema 2]
3. Finalmente: [sub-problema 3]
Ejecuta cada paso antes de continuar.
```

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
│  🔮 OPTIMIZAR PROMPT EXISTENTE                                      │
│     └── Framework: Meta-Prompting                                  │
│                                                                     │
│  🎯 TAREA CRÍTICA (pagos, auth, seguridad)                         │
│     └── Framework: Self-Consistency + C.R.E.A.T.E.                 │
│                                                                     │
│  🌳 MÚLTIPLES SOLUCIONES VÁLIDAS                                   │
│     └── Framework: Tree of Thoughts                                │
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
| 7 | **🆕 Iteración** | ¿Es un retry de prompt fallido? | +3 → activar Meta-Prompting |

**Scoring**:
- **0-2 puntos** → CARE
- **3-5 puntos** → C.O.R.E.
- **6-8 puntos** → C.R.E.A.T.E.
- **9+ puntos** → C.R.E.A.T.E. + Self-Consistency

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

## 🆕 Frameworks Avanzados 2025

### 🔮 Meta-Prompting - Auto-Mejora (~50-100 tokens)

```
Analiza y mejora este prompt antes de ejecutarlo:
[prompt original]

Criterios de mejora:
1. ¿Es específico sobre QUÉ generar?
2. ¿Define restricciones claras?
3. ¿El formato de salida está definido?
4. ¿Cubre edge cases?

Genera:
- Prompt mejorado
- Justificación de cambios
- Luego ejecuta el prompt mejorado
```

**Usar cuando**:
- ✅ El primer intento no dio buenos resultados
- ✅ Prompt complejo que necesitas validar
- ✅ Quieres optimizar un prompt para reutilizar
- ✅ Necesitas escalar prompts para producción

**Ejemplos**: Refinar prompt fallido, optimizar template, validar antes de ejecutar.

---

### 🎯 Self-Consistency - Alta Confianza (~80-120 tokens)

```
Para esta tarea CRÍTICA, genera 3 soluciones diferentes:

Tarea: [descripción]

Para cada solución proporciona:
1. Implementación completa
2. Pros y contras
3. Score de confianza (1-10)
4. Riesgos identificados

Análisis final:
- Selecciona la solución más consistente
- Explica por qué es la más segura
- Lista verificaciones de seguridad aplicadas
```

**Usar cuando**:
- ✅ Código de pagos, transacciones financieras
- ✅ Autenticación y autorización
- ✅ Datos sensibles (PII, GDPR)
- ✅ Decisiones arquitectónicas irreversibles
- ✅ Código que afecta múltiples sistemas

**Ejemplos**: Integración Stripe, JWT implementation, encriptación, workflows críticos.

---

### 🌳 Tree of Thoughts (ToT) - Exploración (~60-100 tokens)

```
Problema: [descripción]

Explora 3 enfoques diferentes:

🌿 Rama A: [Enfoque 1]
├── Implementación
├── Viabilidad: [1-10]
└── Trade-offs

🌿 Rama B: [Enfoque 2]  
├── Implementación
├── Viabilidad: [1-10]
└── Trade-offs

🌿 Rama C: [Enfoque 3]
├── Implementación
├── Viabilidad: [1-10]
└── Trade-offs

🏆 Decisión: [Mejor rama + justificación]
```

**Usar cuando**:
- ✅ Múltiples patrones válidos para resolver
- ✅ Diseño de arquitectura sin restricciones claras
- ✅ Refactoring con varias opciones
- ✅ Optimización de performance

**Ejemplos**: ¿CQRS o Repository?, ¿Monolito modular o microservicios?, ¿SQL o NoSQL?

---

### 📐 Least-to-Most - Descomposición (~40-80 tokens)

```
Tarea compleja: [descripción]

Descomponer en pasos manejables:

Paso 1 (Fundación): [sub-tarea más simple]
→ Resuelve esto primero

Paso 2 (Construcción): [siguiente capa]
→ Usa resultado de Paso 1

Paso 3 (Integración): [conectar todo]
→ Usa resultados anteriores

Paso 4 (Refinamiento): [optimizar]
→ Aplicar mejoras finales

Ejecutar secuencialmente, validar cada paso.
```

**Usar cuando**:
- ✅ Tarea que abruma al modelo
- ✅ Proyecto desde cero muy grande
- ✅ Migración compleja
- ✅ Refactoring masivo

**Ejemplos**: Setup microservicio completo, migración de monolito, implementación completa de feature.

---

## 📤 Formato de Salida del Router

```markdown
## 🧭 Análisis de Tarea

### 📥 Tarea Recibida
> [tarea del usuario]

### 🔍 Clasificación

| Aspecto | Evaluación |
|---------|------------|
| Tipo de Tarea | 🔨 Implementar / 🔬 Investigar / 🐛 Diagnosticar / 🔄 Migrar / 🎯 Crítico |
| Archivos Estimados | [1 / 2-3 / 4+] |
| Complejidad | [Baja / Media / Alta / Crítica] |
| Score de Complejidad | [X/12 puntos] |
| 🆕 Técnicas 2025 | [Meta-Prompting / Self-Consistency / ToT / Ninguna] |

### ✅ Framework Seleccionado

| Framework | Razón de Selección |
|-----------|-------------------|
| **[FRAMEWORK]** | [justificación basada en criterios] |

### 🆕 Técnicas 2025 Aplicadas

| Técnica | ¿Aplicar? | Razón |
|---------|-----------|-------|
| Meta-Prompting | ✅/❌ | [si es retry o prompt complejo] |
| Self-Consistency | ✅/❌ | [si es código crítico] |
| Negative Prompting | ✅/❌ | [si hay anti-patterns conocidos] |
| Tree of Thoughts | ✅/❌ | [si hay múltiples soluciones] |
| Least-to-Most | ✅/❌ | [si es muy complejo] |

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

❌ NEGATIVE PROMPTING (si aplica):
- NO uses: [anti-patterns específicos del stack]
- NO generes: [código que viola reglas del proyecto]
- EVITA: [errores comunes de la tecnología]
```

| Aspecto | Valor |
|---------|-------|
| Tokens Estimados | ~XX |
| Cobertura | ⭐⭐⭐⭐ |
| Iteraciones Esperadas | [1 / 2-3] |
| Técnicas 2025 Incluidas | [lista] |

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
| **🆕 Meta-Prompting** | ⚠️ Opcional | Solo si necesitas docs para mejorar el prompt |
| **🆕 Self-Consistency** | ✅ **Siempre** | Código crítico necesita mejores prácticas de seguridad |
| **🆕 Tree of Thoughts** | ✅ Sí | Comparar arquitecturas necesita info actualizada |
| **🆕 Least-to-Most** | ✅ Sí | Sub-tareas pueden necesitar docs específicos |

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

## 🆕 Ejemplos de Routing con Técnicas 2025

### Ejemplo 6: "Integrar Stripe para pagos" (Self-Consistency)
```
Clasificación: Implementar → Código CRÍTICO → Pagos
Score: 10/12
→ Framework: C.R.E.A.T.E. + Self-Consistency
→ MCPs: ✅ @context7 /stripe + @tavily Stripe security

Prompt generado:
🎯 TAREA CRÍTICA: Integración de pagos

Genera 3 soluciones diferentes para integrar Stripe:

C: .NET 8 API + EF Core | TicketManagementSystem
R: Checkout, webhooks, refunds | Stripe.net SDK
E: PaymentIntent JSON, webhook signature validation
A: Retry 3x, idempotency keys, PCI compliance
T: 8 archivos: Settings, Service, Webhooks, DTOs, DI
E: Edge: pago duplicado, webhook replay, timeout Stripe

Para CADA solución:
1. Implementación completa
2. Pros/Cons de seguridad
3. Score confianza (1-10)
4. Riesgos PCI identificados

❌ NEGATIVE PROMPTING:
- NO guardes números de tarjeta en BD
- NO uses secretos hardcodeados
- EVITA logs con datos de pago

Selecciona la más segura y justifica.
```

### Ejemplo 7: "Refactoring de monolito" (Tree of Thoughts)
```
Clasificación: Arquitectura → Múltiples soluciones válidas
→ Framework: Tree of Thoughts + CLEAR
→ MCPs: ✅ @tavily microservices patterns 2025

Prompt generado:
Problema: Refactorizar TicketManagementSystem monolítico

Explora 3 enfoques:

🌿 Rama A: Monolito Modular
├── Implementación: Vertical slices
├── Viabilidad: ?/10
└── Trade-offs: [riesgo, costo, tiempo]

🌿 Rama B: Microservicios por dominio
├── Implementación: Bounded contexts
├── Viabilidad: ?/10
└── Trade-offs: [riesgo, costo, tiempo]

🌿 Rama C: Strangler Fig gradual
├── Implementación: Migración incremental
├── Viabilidad: ?/10
└── Trade-offs: [riesgo, costo, tiempo]

Criterios de evaluación:
- Riesgo de producción
- Costo de infraestructura
- Tiempo de implementación
- Experiencia del equipo

🏆 Decisión: [Mejor enfoque + plan de acción]
```

### Ejemplo 8: "El prompt anterior no funcionó" (Meta-Prompting)
```
Clasificación: Retry de prompt fallido
→ Framework: Meta-Prompting
→ MCPs: ❌ Depende del prompt mejorado

Prompt generado:
🔮 META-PROMPTING: Mejora este prompt antes de ejecutar

Prompt original fallido:
"Crea un servicio de notificaciones"

Analiza:
1. ¿Es específico? ❌ No dice qué tipo de notificaciones
2. ¿Define restricciones? ❌ No hay stack, patrones
3. ¿Formato de salida? ❌ No define archivos
4. ¿Edge cases? ❌ No considera fallos

Prompt mejorado generado:
C: .NET 8 + SignalR | TicketManagementSystem
O: NotificationService para push real-time a usuarios
R: INotificationService, async, <100ms latency
E: SendAsync(userId, message) → bool
A: Circuit breaker, fallback a email si SignalR falla
T: 4 archivos: Interface, Impl, Hub, DI

Ahora ejecuta el prompt mejorado.
```

### Ejemplo 9: "Crear sistema completo de auditoría" (Least-to-Most)
```
Clasificación: Implementar → Muy complejo → Descomponer
Score: 11/12
→ Framework: Least-to-Most + C.R.E.A.T.E.
→ MCPs: ✅ @context7 /efcore /aspnetcore

Prompt generado:
📐 LEAST-TO-MOST: Sistema de auditoría completo

Descomponer en 4 pasos manejables:

───────────────────────────────────
PASO 1 (Fundación): Modelo de datos
───────────────────────────────────
- AuditLog entity con campos estándar
- DbContext configuration con Fluent API
- ✅ Ejecutar y validar antes de continuar

───────────────────────────────────
PASO 2 (Interceptor): Captura automática
───────────────────────────────────
- SaveChangesInterceptor para EF Core
- Detectar Insert/Update/Delete
- Serializar cambios a JSON
- ✅ Ejecutar y validar

───────────────────────────────────
PASO 3 (API): Consulta de logs
───────────────────────────────────
- AuditController con filtros
- Paginación, búsqueda por entidad
- DTOs de respuesta
- ✅ Ejecutar y validar

───────────────────────────────────
PASO 4 (Refinamiento): Optimización
───────────────────────────────────
- Índices para consultas frecuentes
- Limpieza de logs antiguos
- Dashboard de métricas
- ✅ Validación final

Ejecutar secuencialmente, no continuar si paso anterior falla.
```

---

## 🔄 Flujo de Iteración Automática (2025)

```
┌─────────────────────────────────────────────────────────────────────┐
│              FLUJO DE RECUPERACIÓN DE PROMPTS FALLIDOS             │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Prompt Original → Resultado Insatisfactorio                       │
│         │                                                           │
│         ▼                                                           │
│  ┌──────────────────────────────────────────────────────────┐      │
│  │ ITERACIÓN 1: Agregar más contexto                        │      │
│  │ └── Añadir #archivos, @workspace, ejemplos específicos   │      │
│  └──────────────────────────────────────────────────────────┘      │
│         │                                                           │
│         ▼ ¿Sigue fallando?                                         │
│  ┌──────────────────────────────────────────────────────────┐      │
│  │ ITERACIÓN 2: Subir de framework                          │      │
│  │ └── CARE → C.O.R.E. → C.R.E.A.T.E.                       │      │
│  └──────────────────────────────────────────────────────────┘      │
│         │                                                           │
│         ▼ ¿Sigue fallando?                                         │
│  ┌──────────────────────────────────────────────────────────┐      │
│  │ ITERACIÓN 3: Activar Meta-Prompting                      │      │
│  │ └── Pedir al LLM que mejore el prompt antes de ejecutar  │      │
│  └──────────────────────────────────────────────────────────┘      │
│         │                                                           │
│         ▼ ¿Sigue fallando?                                         │
│  ┌──────────────────────────────────────────────────────────┐      │
│  │ ITERACIÓN 4: Descomponer (Least-to-Most)                 │      │
│  │ └── Dividir tarea en sub-problemas más pequeños          │      │
│  └──────────────────────────────────────────────────────────┘      │
│         │                                                           │
│         ▼ ¿Sigue fallando?                                         │
│  ┌──────────────────────────────────────────────────────────┐      │
│  │ ITERACIÓN 5: Consultar documentación externa             │      │
│  │ └── @context7, @tavily para patrones específicos         │      │
│  └──────────────────────────────────────────────────────────┘      │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
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

---

## 🆕 Tips Avanzados 2025

### 📊 Cuándo usar cada técnica nueva

| Situación | Técnica 2025 | Por qué |
|-----------|--------------|---------|
| Prompt no da resultado esperado | **Meta-Prompting** | El LLM mejora el prompt |
| Código de pagos/auth | **Self-Consistency** | 3 soluciones, elegir más segura |
| "¿Cómo debería diseñar...?" | **Tree of Thoughts** | Explorar múltiples arquitecturas |
| Tarea muy grande | **Least-to-Most** | Dividir para conquistar |
| Siempre | **Negative Prompting** | Evitar anti-patterns |

### 🔐 Negative Prompting por Stack

```
.NET/C#:
❌ NO uses: new HttpClient() sin IHttpClientFactory
❌ NO uses: async void (excepto event handlers)
❌ EVITA: .Result o .Wait() en async

Angular:
❌ NO uses: any type (usar tipado estricto)
❌ NO uses: subscribe sin unsubscribe
❌ EVITA: lógica en templates

EF Core:
❌ NO uses: .ToList() antes de filtrar
❌ NO uses: lazy loading sin razón
❌ EVITA: N+1 queries

Seguridad:
❌ NO guardes: secrets en código
❌ NO expongas: stack traces en producción
❌ EVITA: SQL concatenado (usar parámetros)
```

### 📈 Context Engineering (Anthropic 2025)

> "No es solo prompt engineering, es **context engineering**"

| Elemento | Cómo Optimizar |
|----------|----------------|
| **System Prompt** | Definir rol experto + restricciones globales |
| **Archivos Adjuntos** | Solo los relevantes (no saturar contexto) |
| **Ejemplos** | Few-shot con 2-3 ejemplos de alta calidad |
| **Formato de Salida** | Especificar estructura exacta esperada |
| **Negative Constraints** | Qué NO hacer (más importante que qué hacer) |

### 🎯 Checklist Pre-Prompt (Validar antes de enviar)

```
✅ ¿Es específico sobre QUÉ generar?
✅ ¿Proporciona contexto técnico suficiente?
✅ ¿Define requisitos técnicos claramente?
✅ ¿Especifica patrones y estándares a seguir?
✅ ¿Es una tarea manejable (no demasiado amplia)?
✅ ¿Usa referencias explícitas (#file, @workspace)?
✅ ¿Incluye negative prompting si hay anti-patterns?
✅ ¿El formato de salida está definido?
```

---

## 📊 Matriz de Decisión Rápida 2025

```
┌─────────────────────────────────────────────────────────────────────┐
│              SELECCIÓN RÁPIDA DE FRAMEWORK + TÉCNICA               │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  Tarea Simple (1 archivo)                                          │
│  └── CARE + Negative Prompting (si aplica)                         │
│                                                                     │
│  Tarea Media (2-3 archivos, reglas negocio)                        │
│  └── C.O.R.E. + Negative Prompting                                 │
│                                                                     │
│  Tarea Compleja (>3 archivos, integración)                         │
│  └── C.R.E.A.T.E. + Negative Prompting                             │
│                                                                     │
│  Código CRÍTICO (pagos, auth, seguridad)                           │
│  └── C.R.E.A.T.E. + Self-Consistency + Negative Prompting          │
│                                                                     │
│  Prompt falló anteriormente                                         │
│  └── Meta-Prompting → subir framework → Least-to-Most              │
│                                                                     │
│  Múltiples soluciones válidas                                       │
│  └── Tree of Thoughts + CLEAR                                      │
│                                                                     │
│  Tarea muy grande para un prompt                                    │
│  └── Least-to-Most + C.R.E.A.T.E. por cada paso                    │
│                                                                     │
│  Decisión arquitectónica                                            │
│  └── CLEAR + Tree of Thoughts + @tavily                            │
│                                                                     │
│  Diagnóstico de error                                               │
│  └── ReAcT (con stack trace) / CoT (sin stack trace)              │
│                                                                     │
│  Código repetitivo / Migración                                      │
│  └── Few-Shot + ejemplos de alta calidad                           │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 📚 Referencias y Fuentes 2025

| Fuente | Técnica | URL/Paper |
|--------|---------|-----------|
| Anthropic | Context Engineering | claude.com/blog/best-practices |
| OpenAI | Meta-Prompting | OpenAI Cookbook |
| Databricks | GEPA (Genetic Prompt Optimization) | MLflow docs |
| Google | Chain-of-Thought | "CoT Prompting Elicits Reasoning" |
| Research | Tree of Thoughts | "ToT: Deliberate Problem Solving" |
| Industry | Self-Consistency | "Self-Consistency Improves CoT" |
