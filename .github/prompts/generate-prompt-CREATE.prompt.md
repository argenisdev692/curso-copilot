---
description: 'Genera 3 versiones de prompts C.R.E.A.T.E para tareas complejas'
---

# 🚀 Generador de Prompts C.R.E.A.T.E (Context-Request-Examples-Adjustments-Type-Extras)

Crear prompts ultra-detallados para GitHub Copilot usando el framework **C.R.E.A.T.E** para **tareas complejas**.

## 📥 Entrada del Usuario

- **Tema**: {{tema}}
- **Contexto Técnico**: {{contexto}}
- **Request/Objetivo**: {{objetivo}}
- **Ejemplos Deseados**: {{ejemplos}}
- **Ajustes/Patrones**: {{ajustes}}
- **Tipo de Output**: {{tipoOutput}}
- **Extras/Edge Cases**: {{extras}}

## 📋 Framework C.R.E.A.T.E Explicado

```
┌─────────────────────────────────────────────────────────────────────┐
│                      C.R.E.A.T.E. FRAMEWORK                         │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  C - CONTEXT (Contexto Técnico)                                    │
│  ├── Proyecto, stack tecnológico                                   │
│  ├── Patrones existentes (Repository, CQRS, etc.)                  │
│  └── Archivos relevantes (usar sintaxis #TuArchivo.cs)             │
│                                                                     │
│  R - REQUEST (Solicitud Específica)                                │
│  ├── Qué funcionalidad/componente crear                            │
│  ├── Propósito y caso de uso                                       │
│  └── Componentes específicos requeridos                            │
│                                                                     │
│  E - EXAMPLES (Ejemplos Concretos)                                 │
│  ├── JSON de entrada/salida esperados                              │
│  ├── Flujos de datos                                               │
│  └── Configuraciones en appsettings/env                            │
│                                                                     │
│  A - ADJUSTMENTS (Ajustes y Patrones)                              │
│  ├── Patrones de resiliencia (retry, circuit breaker)              │
│  ├── Configuración con IOptions                                    │
│  └── Correlation IDs, logging estructurado                         │
│                                                                     │
│  T - TYPE OF OUTPUT (Formato de Salida)                            │
│  ├── Lista de archivos a generar                                   │
│  ├── Estructura de carpetas                                        │
│  └── Convenciones de código (async, XML docs)                      │
│                                                                     │
│  E - EXTRAS (Consideraciones Adicionales)                          │
│  ├── Edge cases a manejar                                          │
│  ├── Manejo de errores específicos                                 │
│  └── Consideraciones de producción                                 │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## 🔧 Reglas de Optimización para C.R.E.A.T.E

1. **Tokens: 80-150** (tareas complejas requieren más detalle)
2. **Siempre incluir**:
   - Patrones de resiliencia
   - Edge cases
   - Estructura de archivos
3. **Abreviaturas**: TS, API, DTO, DLQ, CB (Circuit Breaker), Repo, Svc
4. **Referencias**: usar `#NombreArchivo.cs` para archivos, `@workspace` para contexto
5. **Versiones**: Detectar del proyecto o usar "latest" (no hardcodear versiones)

## 📤 Formato de Salida Requerido

```markdown
## 🎯 Tema: [tema]

### 📊 Análisis de Complejidad

| Criterio | Evaluación | Justificación |
|----------|------------|---------------|
| Primera implementación | ✅/❌ | [razón] |
| Múltiples archivos (>3) | ✅/❌ | [razón] |
| Integración externa | ✅/❌ | [razón] |
| Patrones de resiliencia | ✅/❌ | [razón] |
| Código producción | ✅/❌ | [razón] |

**Veredicto**: [✅ C.R.E.A.T.E es el framework correcto / ⚠️ Considerar C.O.R.E]

---

## 📝 Versiones Generadas

### 🔷 Versión 1 - C.R.E.A.T.E Completo (Máximo Detalle)

```
C - CONTEXT:
[Proyecto] + [Stack completo] + [Patrones actuales]
Archivos: (referenciar con #NombreReal.cs)

R - REQUEST:
[Funcionalidad detallada]
Componentes: [lista de componentes]

E - EXAMPLES:
Input JSON: { ... }
Output JSON: { ... }
Config: { ... }

A - ADJUSTMENTS:
- [Patrón 1]: [detalle]
- [Patrón 2]: [detalle]
- [Configuración]: [detalle]

T - TYPE OF OUTPUT:
Archivos:
├── folder/File1.cs
├── folder/File2.cs
└── folder/File3.cs
Convenciones: [async, XML docs, etc.]

E - EXTRAS:
⚠️ Edge cases:
- [caso 1]
- [caso 2]
🔒 Producción:
- [consideración 1]
```

| Aspecto | Evaluación |
|---------|------------|
| Tokens | ~XXX |
| Cobertura | ⭐⭐⭐⭐⭐ |
| Flexibilidad | ⭐⭐ |
| Fortaleza | Detalle exhaustivo, menos iteraciones |
| Debilidad | Puede ser restrictivo |
| Uso ideal | Primera implementación, código crítico |

---

### 🔶 Versión 2 - C.R.E.A.T.E Balanceado

```
C: [Stack] + [Patrones] | (referenciar archivo relevante)
R: [Objetivo principal] + [Componentes clave]
E: Input/Output esperado, config ejemplo
A: [Patrón resiliencia] + [IOptions] + [CorrelationId]
T: [N archivos]: File1, File2, File3 | async + XML docs
E: Edge: [caso crítico] | Prod: [consideración principal]
```

| Aspecto | Evaluación |
|---------|------------|
| Tokens | ~XXX |
| Cobertura | ⭐⭐⭐⭐ |
| Flexibilidad | ⭐⭐⭐ |
| Fortaleza | Balance entre detalle y brevedad |
| Debilidad | Puede requerir 1-2 follow-ups |
| Uso ideal | Desarrollador con experiencia en el stack |

---

### 🔷 Versión 3 - C.R.E.A.T.E Condensado

```
C: [Stack mínimo] (archivo principal)
R: [Objetivo directo]
E: JSON ejemplo clave
A: [Patrón principal], [Config]
T: [Archivos]: estructura simple
E: Edge: [1 caso crítico]
```

| Aspecto | Evaluación |
|---------|------------|
| Tokens | ~XXX |
| Cobertura | ⭐⭐⭐ |
| Flexibilidad | ⭐⭐⭐⭐ |
| Fortaleza | Rápido, permite más creatividad de Copilot |
| Debilidad | Puede omitir edge cases importantes |
| Uso ideal | Prototipo rápido, luego refinar |

---

## 🏆 Análisis Comparativo

| Versión | Tokens | Detalle | Edge Cases | Producción | Score |
|---------|--------|---------|------------|------------|-------|
| V1 Completo | ~XXX | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | X/10 |
| V2 Balanceado | ~XXX | ⭐⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐⭐ | X/10 |
| V3 Condensado | ~XXX | ⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐ | X/10 |

---

## ✅ Recomendación Final

### 🥇 Mejor para Código de Producción: Versión X

**Justificación**:
- [Razón 1]
- [Razón 2]
- [Razón 3]

### 🎯 Selección por Escenario

| Escenario | Versión Recomendada | Por qué |
|-----------|---------------------|---------|
| Primera vez implementando | V1 Completo | Minimiza errores y re-trabajo |
| Dev senior en el stack | V2 Balanceado | Conoce patrones, necesita guía |
| Prototipo/POC | V3 Condensado | Velocidad > perfección |
| Código crítico/pagos | V1 Completo | No hay margen de error |
| Refactoring existente | V2 Balanceado | Ya hay contexto en el código |

### 💡 Estrategia de Iteración

1. **Usa V2 Balanceado** como punto de partida
2. Si el output es incompleto → Refina con elementos de V1
3. Si funciona bien → Guarda el prompt para reutilizar
```

---

## 🎯 Cuándo usar C.R.E.A.T.E

| ✅ Usar C.R.E.A.T.E | ❌ NO usar C.R.E.A.T.E |
|--------------------|----------------------|
| Integraciones (RabbitMQ, Stripe, etc.) | CRUD simple (usar CARE) |
| Autenticación/Autorización JWT | Componente UI básico (usar C.O.R.E) |
| Microservicios nuevos | Endpoint simple (usar C.O.R.E) |
| Event Sourcing / CQRS | Fix pequeño (usar CARE) |
| Arquitectura desde cero | Decisiones técnicas (usar CLEAR) |
| Código crítico (pagos, seguridad) | Prototipo rápido (usar C.O.R.E) |

---

## 📚 Ejemplo de Uso

**Input**:
- Tema: Integrar RabbitMQ
- Contexto: .NET 8 + MediatR + TicketManagementSystem
- Objetivo: Publisher + Consumer para notificaciones
- Ejemplos: TicketCreatedEvent JSON
- Ajustes: Retry exponencial, DLQ, CorrelationId
- Tipo Output: 7 archivos en /Messaging
- Extras: RabbitMQ caído, mensajes duplicados

---

## 🔗 MCPs Recomendados (Incluir en Respuesta)

**IMPORTANTE**: Al final de cada respuesta, incluir sugerencias de MCPs relevantes según el tema del prompt generado.

### Formato de Sugerencia

```markdown
---

## 🚀 Siguiente Paso: Usa MCPs para Implementar

Después de elegir tu versión de prompt, usa estos MCPs en un **nuevo chat**:

### 📖 Documentación Oficial
```
@context7 /[librería] [pega aquí el prompt elegido]
```

### 🌐 Mejores Prácticas Web
```
@tavily [tema] best practices [año actual]
```

### 📂 Contexto del Proyecto
```
@workspace [pega aquí el prompt elegido]
```
```

### Tabla de MCPs por Tecnología

| Tema del Prompt | @context7 | @tavily |
|-----------------|-----------|----------|
| Angular Frontend | `/angular` | "Angular [feature] best practices" |
| .NET/C# Backend | `/dotnet`, `/aspnetcore` | ".NET [feature] production patterns" |
| EF Core/SQL Server | `/efcore` | "EF Core [pattern] performance" |
| RabbitMQ/Mensajería | `/rabbitmq` | "RabbitMQ .NET resilience patterns" |
| Docker/.NET | `/docker` | "Docker .NET container optimization" |
| JWT/Auth | `/aspnetcore` | "JWT authentication .NET Angular" |
| xUnit/Testing | `/xunit` | ".NET testing best practices" |
| SignalR | `/signalr` | "SignalR .NET real-time patterns" |

### Ejemplo de Sugerencia Generada

Si el tema es "Componentes Angular Login", incluir al final:

```markdown
---

## 🚀 Siguiente Paso: Implementa con MCPs

📖 **Documentación Angular**:
```
@context7 /angular [pega V2 Balanceado aquí]
```

🌐 **Mejores prácticas de Auth**:
```
@tavily Angular JWT authentication best practices
```

📂 **Contexto de tu proyecto**:
```
@workspace Implementa usando V2: C: Angular + .NET JWT...
```

💡 **Tip**: Usa @context7 primero para documentación oficial, luego @tavily para patrones avanzados.
```
