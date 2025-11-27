# 🎯 Análisis de Selección de Frameworks de Prompts

> **Guía práctica para elegir el framework correcto según el tipo de tarea**

---

## 📋 Tabla de Contenidos

1. [Introducción](#1-introducción)
2. [Caso de Estudio: RabbitMQ](#2-caso-de-estudio-rabbitmq)
3. [Matriz de Decisión General](#3-matriz-de-decisión-general)
4. [Criterios de Selección por Framework](#4-criterios-de-selección-por-framework)
5. [Árbol de Decisión Visual](#5-árbol-de-decisión-visual)
6. [Ejemplos de Selección por Escenario](#6-ejemplos-de-selección-por-escenario)
7. [Prompts en Markdown: Análisis de Tokens](#7-prompts-en-markdown-análisis-de-tokens)

---

## 1. Introducción

### El Problema
Elegir el framework de prompts incorrecto resulta en:
- ❌ Código incompleto que requiere múltiples iteraciones
- ❌ Prompts demasiado elaborados para tareas simples (overhead)
- ❌ Falta de especificaciones críticas en tareas complejas
- ❌ Consumo innecesario de tokens

### La Solución
Aplicar un **análisis sistemático** basado en criterios objetivos para seleccionar el framework adecuado.

---

## 2. Caso de Estudio: RabbitMQ

### Contexto de la Tarea
Integrar **RabbitMQ** como sistema de mensajería en el proyecto **TicketManagementSystem** (.NET 8).

### 🎯 ¿Por qué C.R.E.A.T.E.?

| Criterio | Aplicabilidad a RabbitMQ | Justificación |
|----------|--------------------------|---------------|
| **Tarea compleja** | ✅ Aplica | Integración de mensajería requiere múltiples componentes |
| **Múltiples requisitos técnicos** | ✅ Aplica | Configuración, producers, consumers, retry policies |
| **Primera implementación** | ✅ Aplica | Probablemente nuevo en el proyecto |
| **Código producción** | ✅ Aplica | Necesitas código robusto desde el inicio |
| **Patrones específicos** | ✅ Aplica | Pub/Sub, Dead Letter Queue, Circuit Breaker |
| **Múltiples archivos** | ✅ Aplica | Settings, interfaces, implementaciones, eventos |

### Análisis de Alternativas Descartadas

| Framework | ¿Por qué NO para RabbitMQ? |
|-----------|---------------------------|
| **CARE** | ❌ Demasiado simple, no cubre retry policies ni edge cases |
| **C.O.R.E.** | ⚠️ Podría funcionar, pero falta espacio para ejemplos detallados |
| **CLEAR** | ❌ Es para investigación/decisiones, no implementación |
| **Chain-of-Thought** | ❌ Es para razonamiento, no para generar código estructurado |
| **Few-Shot** | ⚠️ Útil solo si tienes ejemplos previos de integración similar |

### Componentes de C.R.E.A.T.E. Aplicados

```
┌─────────────────────────────────────────────────────────────────────┐
│              C.R.E.A.T.E. PARA RABBITMQ                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  C - CONTEXT                                                        │
│  ├── Proyecto: TicketManagementSystem                              │
│  ├── Stack: .NET 8, EF Core 9, MediatR                             │
│  ├── Patrones: Repository, Result Pattern, CQRS                    │
│  └── Archivos relevantes para contexto                             │
│                                                                     │
│  R - REQUEST                                                        │
│  ├── Notificaciones asíncronas (tickets)                           │
│  ├── Desacoplamiento API ↔ Notificaciones                          │
│  └── Componentes: Connection, Publisher, Consumer, Events         │
│                                                                     │
│  E - EXAMPLES                                                       │
│  ├── JSON de mensaje esperado                                      │
│  ├── Flujo de creación de ticket                                   │
│  └── Configuración en appsettings.json                             │
│                                                                     │
│  A - ADJUSTMENTS                                                    │
│  ├── Retry exponencial                                             │
│  ├── Dead Letter Queue                                             │
│  ├── CorrelationId para trazabilidad                               │
│  └── IOptions pattern para configuración                           │
│                                                                     │
│  T - TYPE OF OUTPUT                                                 │
│  ├── 7+ archivos específicos                                       │
│  ├── Estructura de carpetas definida                               │
│  └── Comentarios XML, async/await                                  │
│                                                                     │
│  E - EXTRAS                                                         │
│  ├── Edge cases: RabbitMQ no disponible                            │
│  ├── Mensajes duplicados (idempotencia)                            │
│  └── Consideraciones de producción                                 │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 3. Matriz de Decisión General

### Selección por Tipo de Tarea

| Tipo de Tarea | Framework Recomendado | Tokens Estimados |
|---------------|----------------------|------------------|
| CRUD simple | CARE | 🟢 100-200 |
| Componente UI | C.O.R.E. | 🟡 200-400 |
| Servicio con lógica | C.O.R.E. | 🟡 300-500 |
| Integración externa | **C.R.E.A.T.E.** | 🔴 500-1000 |
| Arquitectura nueva | **C.R.E.A.T.E.** | 🔴 800-1500 |
| Decisión técnica | CLEAR | 🔴 400-800 |
| Debugging complejo | Chain-of-Thought | 🔴 500-1000 |
| Diagnóstico errores | ReAcT | 🔴 600-1200 |
| Código repetitivo | Few-Shot | 🟡 300-500 |
| Migración de código | Few-Shot | 🟡 400-700 |

### Selección por Complejidad

```
Complejidad Baja          Complejidad Media         Complejidad Alta
     │                          │                          │
     ▼                          ▼                          ▼
┌─────────┐              ┌─────────────┐           ┌─────────────┐
│  CARE   │              │   C.O.R.E.  │           │  C.R.E.A.T.E│
└─────────┘              └─────────────┘           └─────────────┘
     │                          │                          │
     ▼                          ▼                          ▼
• Validador simple       • Endpoint API            • RabbitMQ
• Utility function       • Service básico          • Auth JWT
• DTO mapping            • Componente Angular      • Microservicio
• Fix pequeño            • Repository pattern      • Event Sourcing
```

---

## 4. Criterios de Selección por Framework

### CARE - Para Tareas Rápidas

| Criterio | Sí/No |
|----------|-------|
| ¿Se completa en <30 min? | ✅ |
| ¿Un solo archivo? | ✅ |
| ¿Sin edge cases complejos? | ✅ |
| ¿Patrón conocido? | ✅ |

**Ejemplo**: Crear un DTO, agregar validación, utility function.

---

### C.O.R.E. - Balance General

| Criterio | Sí/No |
|----------|-------|
| ¿Necesito especificar restricciones? | ✅ |
| ¿1-3 archivos relacionados? | ✅ |
| ¿Hay reglas de negocio? | ✅ |
| ¿Necesito ejemplo de output? | ✅ |

**Ejemplo**: Endpoint con validación, componente con estados, servicio CRUD.

---

### C.R.E.A.T.E. - Tareas Complejas

| Criterio | Sí/No |
|----------|-------|
| ¿Primera vez implementando esto? | ✅ |
| ¿Múltiples archivos (>3)? | ✅ |
| ¿Integración externa? | ✅ |
| ¿Patrones de resiliencia? | ✅ |
| ¿Código para producción? | ✅ |

**Ejemplo**: RabbitMQ, Autenticación JWT, Gateway API, Event Sourcing.

---

### CLEAR - Decisiones Arquitectónicas

| Criterio | Sí/No |
|----------|-------|
| ¿Necesito comparar opciones? | ✅ |
| ¿Documentar el "por qué"? | ✅ |
| ¿Crear ADR? | ✅ |
| ¿Evaluar trade-offs? | ✅ |

**Ejemplo**: ¿SignalR vs WebSockets?, ¿SQL vs NoSQL?, ¿Monolito vs Microservicios?

---

### Chain-of-Thought - Razonamiento

| Criterio | Sí/No |
|----------|-------|
| ¿Problema de lógica compleja? | ✅ |
| ¿Necesito entender el "cómo"? | ✅ |
| ¿Optimización de algoritmo? | ✅ |
| ¿Debugging sin stack trace? | ✅ |

**Ejemplo**: Optimizar query lenta, refactoring complejo, diseño de algoritmo.

---

### ReAcT - Diagnóstico

| Criterio | Sí/No |
|----------|-------|
| ¿Error en producción? | ✅ |
| ¿Necesito trazabilidad del análisis? | ✅ |
| ¿Stack trace disponible? | ✅ |
| ¿Verificación factual necesaria? | ✅ |

**Ejemplo**: Exception en producción, comportamiento inesperado, análisis de logs.

---

### Few-Shot - Patrones Repetitivos

| Criterio | Sí/No |
|----------|-------|
| ¿Tengo ejemplos de referencia? | ✅ |
| ¿Código repetitivo? | ✅ |
| ¿Migración de formato? | ✅ |
| ¿Consistencia de estilo? | ✅ |

**Ejemplo**: Generar DTOs desde Entities, migrar de JS a TS, crear tests similares.

---

## 5. Árbol de Decisión Visual

```
                        ┌─────────────────────┐
                        │ ¿Qué tipo de tarea? │
                        └──────────┬──────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        ▼                          ▼                          ▼
 ┌──────────────┐          ┌──────────────┐          ┌──────────────┐
 │ Implementar  │          │  Investigar  │          │  Diagnosticar│
 │   código     │          │   decidir    │          │    error     │
 └──────┬───────┘          └──────┬───────┘          └──────┬───────┘
        │                         │                         │
        ▼                         ▼                         ▼
 ┌──────────────┐          ┌──────────────┐          ┌──────────────┐
 │¿Complejidad? │          │    CLEAR     │          │¿Stack trace? │
 └──────┬───────┘          │     o        │          └──────┬───────┘
        │                  │    CoT       │                 │
   ┌────┼────┐             └──────────────┘            ┌────┴────┐
   │    │    │                                         │         │
   ▼    ▼    ▼                                         ▼         ▼
 Baja Media Alta                                      Sí        No
   │    │    │                                         │         │
   ▼    ▼    ▼                                         ▼         ▼
 CARE CORE CREATE                                   ReAcT      CoT
```

---

## 6. Ejemplos de Selección por Escenario

### Escenario 1: Agregar campo a DTO
```
Análisis:
- ¿Tarea rápida? ✅ Sí
- ¿Un archivo? ✅ Sí
- ¿Sin lógica compleja? ✅ Sí
→ Framework: CARE
```

### Escenario 2: Nuevo endpoint de búsqueda
```
Análisis:
- ¿Necesito restricciones? ✅ Sí (paginación, filtros)
- ¿2-3 archivos? ✅ Sí (Controller, Service, DTO)
- ¿Reglas de negocio? ✅ Sí (validación de parámetros)
→ Framework: C.O.R.E.
```

### Escenario 3: Integrar Stripe para pagos
```
Análisis:
- ¿Primera implementación? ✅ Sí
- ¿Múltiples archivos? ✅ Sí (Settings, Service, DTOs, Webhooks)
- ¿Integración externa? ✅ Sí
- ¿Patrones de resiliencia? ✅ Sí (retry, idempotencia)
→ Framework: C.R.E.A.T.E.
```

### Escenario 4: ¿Redis o Memcached para caché?
```
Análisis:
- ¿Comparar opciones? ✅ Sí
- ¿Documentar decisión? ✅ Sí
- ¿Evaluar trade-offs? ✅ Sí
→ Framework: CLEAR
```

### Escenario 5: Query lenta en producción
```
Análisis:
- ¿Problema de lógica? ✅ Sí
- ¿Necesito entender cómo optimizar? ✅ Sí
→ Framework: Chain-of-Thought
```

### Escenario 6: NullReferenceException en endpoint
```
Análisis:
- ¿Error en producción? ✅ Sí
- ¿Tengo stack trace? ✅ Sí
- ¿Necesito trazabilidad? ✅ Sí
→ Framework: ReAcT
```

### Escenario 7: Convertir 20 entities a DTOs
```
Análisis:
- ¿Tengo ejemplo? ✅ Sí (un DTO ya existe)
- ¿Código repetitivo? ✅ Sí
- ¿Consistencia importante? ✅ Sí
→ Framework: Few-Shot
```

---

## 7. Prompts en Markdown: Análisis de Tokens

### ¿Es correcto usar archivos .md para prompts?

| Aspecto | Evaluación |
|---------|------------|
| **Reutilización** | ✅ El mismo prompt sirve múltiples veces |
| **Versionado** | ✅ Se guarda en Git con historial |
| **Colaboración** | ✅ El equipo comparte prompts probados |
| **Documentación** | ✅ Sirve como referencia técnica |
| **Consistencia** | ✅ Todos usan el mismo formato |

### ¿Se ahorran tokens?

```
┌─────────────────────────────────────────────────────────────────────┐
│                    COMPARACIÓN DE TOKENS                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ESCENARIO 1: Escribir todo en chat (3 iteraciones)                │
│  ─────────────────────────────────────────────────                 │
│  Prompt 1: "Crea RabbitMQSettings con..." → 500 tokens             │
│  Prompt 2: "Ahora el publisher..." → 500 + contexto previo         │
│  Prompt 3: "Falta el consumer..." → 500 + contexto acumulado       │
│  TOTAL: ~1500 tokens de entrada (acumulativo)                      │
│  ⚠️ Problema: contexto se contamina con errores anteriores         │
│                                                                     │
│  ESCENARIO 2: Usar archivo .md con #file (chats nuevos)            │
│  ─────────────────────────────────────────────────────             │
│  Chat 1: "#file:rabbitmq.md crear Settings" → ~600 tokens          │
│  Chat 2: "#file:rabbitmq.md crear publisher" → ~600 tokens         │
│  Chat 3: "#file:rabbitmq.md crear consumer" → ~600 tokens          │
│  TOTAL: ~1800 tokens (sin acumulación)                             │
│  ✅ Ventaja: cada chat empieza limpio                              │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### Estrategia Óptima

| Paso | Acción |
|------|--------|
| 1 | Crear archivo .md con prompt completo (C.R.E.A.T.E.) |
| 2 | En NUEVO chat: `@workspace Implementa paso 1 de #file:prompt.md` |
| 3 | Validar código generado |
| 4 | NUEVO chat para siguiente paso |
| 5 | Repetir con chats frescos |

### Beneficios Reales

| Beneficio | Impacto |
|-----------|---------|
| **Sin contexto acumulado** | Cada chat empieza limpio |
| **Prompt consistente** | No olvidas requisitos |
| **Menos errores** | El prompt está validado |
| **Reutilizable** | Otros devs lo pueden usar |
| **Auditable** | Queda en Git |
| **Documentación viva** | El prompt ES la especificación |

---

## 📚 Referencias

| Archivo | Descripción |
|---------|-------------|
| [frameworks-prompts-analisis-completo.md](./frameworks-prompts-analisis-completo.md) | Detalles de cada framework |
| [estrategias-construccion-prompts.md](./estrategias-construccion-prompts.md) | Cómo construir prompts |
| [optimizacion-tokens-copilot.md](./optimizacion-tokens-copilot.md) | Reducir consumo de tokens |

---

## 🎯 Resumen Ejecutivo

| Pregunta | Respuesta |
|----------|-----------|
| **¿Cuál para RabbitMQ?** | **C.R.E.A.T.E.** - Tarea compleja, múltiples componentes |
| **¿Cuál para CRUD?** | **CARE** - Simple y rápido |
| **¿Cuál para decisiones?** | **CLEAR** - Documenta el "por qué" |
| **¿Cuál para debugging?** | **ReAcT** o **CoT** según contexto |
| **¿Prompts en .md?** | ✅ **SÍ** - Best practice |
| **¿Ahorra tokens?** | ⚠️ **No directamente**, pero mantiene contexto limpio |

---

> **💡 Regla de Oro:**  
> *Usa el framework más simple que cubra tus requisitos.*  
> Si CARE es suficiente, no uses C.R.E.A.T.E.  
> Si necesitas robustez, no escatimes en C.R.E.A.T.E.
