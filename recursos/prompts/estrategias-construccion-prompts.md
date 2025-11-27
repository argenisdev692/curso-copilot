# 🎯 Guía Completa: Estrategias para Construir Prompts Efectivos

> **Objetivo**: Dominar la construcción de prompts para obtener resultados precisos y profesionales con GitHub Copilot.

---

## 📋 Tabla de Contenidos

1. [Fundamentos: ¿Por qué importa un buen prompt?](#1-fundamentos-por-qué-importa-un-buen-prompt)
2. [Anatomía de un Prompt Efectivo](#2-anatomía-de-un-prompt-efectivo)
3. [Frameworks y Fórmulas](#3-frameworks-y-fórmulas)
4. [Método de Construcción Progresiva](#4-método-de-construcción-progresiva)
5. [Técnicas Avanzadas](#5-técnicas-avanzadas)
6. [Plantillas Reutilizables](#6-plantillas-reutilizables)
7. [Comparativa: Prompts Buenos vs Malos](#7-comparativa-prompts-buenos-vs-malos)
8. [Checklist de Validación](#8-checklist-de-validación)

---

## 1. Fundamentos: ¿Por qué importa un buen prompt?

### El Problema
Un prompt vago genera código genérico que requiere múltiples iteraciones para corregir, **gastando tiempo y tokens**.

### La Solución
Un prompt bien estructurado genera código **específico, correcto y listo para usar** en el primer intento.

### Impacto Real

| Tipo de Prompt | Iteraciones Promedio | Tokens Consumidos | Calidad del Resultado |
|----------------|---------------------|-------------------|----------------------|
| Vago           | 4-6                 | Alto              | ⭐⭐                  |
| Básico         | 2-3                 | Medio             | ⭐⭐⭐                |
| Estructurado   | 1-2                 | Bajo              | ⭐⭐⭐⭐⭐             |

---

## 2. Anatomía de un Prompt Efectivo

### Los 5 Componentes Esenciales

```
┌─────────────────────────────────────────────────────────────┐
│  1. ROL        → ¿Quién debe ser el asistente?             │
│  2. TAREA      → ¿Qué acción específica debe realizar?     │
│  3. CONTEXTO   → ¿En qué proyecto/tecnología/situación?    │
│  4. REQUISITOS → ¿Qué características debe tener?          │
│  5. FORMATO    → ¿Cómo debe entregar el resultado?         │
│  (+ RESTRICCIONES → ¿Qué NO debe hacer?)                   │
└─────────────────────────────────────────────────────────────┘
```

### Estructura Visual

```
[ROL] Actúa como [tipo de experto]
[TAREA] Necesito que [acción específica]
[CONTEXTO] En el proyecto [nombre], usando [tecnologías v.X]
[REQUISITOS] Debe cumplir con:
  - [requisito técnico 1]
  - [requisito técnico 2]
  - [patrón o estándar]
[FORMATO] Entrega:
  - [tipo de output esperado]
[RESTRICCIONES] NO incluyas:
  - [qué evitar explícitamente]
```

---

## 3. Frameworks y Fórmulas

### 🔷 Fórmula C.O.R.E.

| Letra | Significado | Pregunta Clave |
|-------|-------------|----------------|
| **C** | Contexto    | ¿Qué somos? ¿Qué tenemos? (framework, archivos abiertos) |
| **O** | Objetivo    | ¿Qué queremos lograr exactamente? |
| **R** | Restricciones | ¿Qué NO queremos? ¿Qué librerías usar/evitar? |
| **E** | Ejemplo     | ¿Cómo debe verse el resultado? (opcional) |

### 🔷 Flujo Mental Universal

```
[CONTEXTO] → [OBJETIVO] → [ESPECIFICACIONES] → [RESTRICCIONES] → [FORMATO]
```

### 🔷 Ejemplo Aplicando C.O.R.E.

```markdown
**C - Contexto:** API REST en .NET 8, proyecto de gestión de tickets

**O - Objetivo:** Crear un servicio para gestionar el ciclo de vida de tickets

**R - Restricciones:** 
- Usar patrón Repository
- No exponer entities directamente
- Validar con FluentValidation

**E - Ejemplo de salida:** 
- ITicketService con métodos CRUD
- DTOs para Request/Response
- Manejo de errores con Result pattern
```

---

## 4. Método de Construcción Progresiva

> **Técnica pedagógica**: Mostrar cómo un prompt evoluciona de básico a profesional.

### Nivel 1: Prompt Básico (⭐)
```
Genera un componente Angular para mostrar una lista de productos
```
**Problema**: Demasiado vago. No especifica versión, estilo, tipado.

---

### Nivel 2: Con Contexto (⭐⭐)
```
Estoy trabajando en un e-commerce en Angular 17.
Necesito un componente para mostrar una lista de productos
```
**Mejora**: Añade contexto del proyecto y versión.

---

### Nivel 3: Con Especificaciones (⭐⭐⭐)
```
Estoy trabajando en un e-commerce en Angular 17.
Necesito un componente para mostrar una lista de productos.

Especificaciones:
- Usar standalone component
- Implementar OnInit
- Crear interfaz Product con: id, name, price, imageUrl
- Mostrar productos en cards con Bootstrap 5
```
**Mejora**: Define requisitos técnicos concretos.

---

### Nivel 4: Prompt Profesional (⭐⭐⭐⭐⭐)
```
Contexto: E-commerce Angular 17, módulo de catálogo

Objetivo: Componente standalone para listar productos

Requisitos técnicos:
- Interfaz Product: id(number), name(string), price(number), imageUrl(string)
- Implementar OnInit
- Array mock de 3 productos para testing
- UI: Bootstrap 5 cards en grid responsive (3 columnas desktop)

Restricciones:
- No usar servicios aún (datos hardcodeados)
- Código comentado en español

Formato: Archivo .ts completo listo para usar
```
**Resultado**: Código preciso, profesional, sin iteraciones adicionales.

---

## 5. Técnicas Avanzadas

### 🧠 Chain-of-Thought (CoT)
Pedir al asistente que explique su razonamiento **antes** de generar código.

```markdown
Antes de generar el código, explica:
1. Qué patrón de diseño aplicarás y por qué
2. Cómo estructurarás las capas
3. Qué validaciones consideras necesarias

Luego genera el código del servicio de autenticación.
```

**Beneficio**: Detectas errores de lógica antes de recibir código incorrecto.

---

### 📝 Few-Shot Prompting
Proporcionar ejemplos de entrada/salida para guiar el formato.

```markdown
Genera DTOs siguiendo este patrón:

Ejemplo de entrada (Entity):
public class User { public int Id; public string Name; }

Ejemplo de salida esperada (DTO):
public record UserDto(int Id, string Name);

Ahora genera DTOs para la entity Order con: Id, CustomerId, Total, Status, CreatedAt
```

**Beneficio**: Consistencia en el formato de salida.

---

### 🔄 Self-Critique
Solicitar que el asistente revise y mejore su propia respuesta.

```markdown
Genera un middleware de logging para .NET 8.

Después de generarlo:
1. Revisa si cumple con los principios SOLID
2. Identifica posibles mejoras de performance
3. Sugiere tests unitarios necesarios
```

**Beneficio**: Código más robusto en una sola interacción.

---

### 🎯 Iterative Refinement
Refinar basándose en resultados anteriores.

```markdown
El código anterior funciona pero:
- Falta manejo de nulos
- Los nombres de métodos no son descriptivos
- No tiene logging

Refactoriza manteniendo la lógica pero aplicando estas mejoras.
```

---

## 6. Plantillas Reutilizables

### 📦 Plantilla para C# / .NET Backend

```markdown
[CONTEXTO: {tipo de aplicación - API REST, Microservicio, etc.}]

Crear {controlador/servicio/repository} para {funcionalidad}

Requisitos:
- Input: {parámetros de entrada con tipos}
- Output: {tipo de retorno esperado}
- Validaciones: {reglas de negocio}
- Patrones: {Repository/CQRS/Mediator/etc.}

Incluir:
- Manejo de errores con {ProblemDetails/Result pattern}
- Comentarios XML completos
- {Async/await para I/O}
- {Unit tests con xUnit}

Tecnologías: .NET 8, C# 12, EF Core 8
```

---

### 🅰️ Plantilla para Angular Frontend

```markdown
[CONTEXTO: {módulo de la aplicación}]

Crear {componente/servicio/pipe/directiva} para {funcionalidad}

Especificaciones:
- Tipo: {standalone/module-based}
- Inputs: {lista con tipos}
- Outputs: {EventEmitters}
- State: {signals/observables/ngRx}
- UI: {Tailwind/Bootstrap/Material}

Incluir:
- Tipado TypeScript estricto (no any)
- Estados de: loading, error, empty, success
- {Reactive forms / Template forms}
- {Lazy loading / Routing}

Versión: Angular 17+, TypeScript 5.x
```

---

### 🧪 Plantilla para Tests

```markdown
Genera tests unitarios para {clase/método}

Escenarios a cubrir:
1. Caso exitoso: {descripción}
2. Caso de error: {descripción}
3. Edge case: {descripción}

Convenciones:
- Naming: Metodo_Escenario_ResultadoEsperado
- Estructura: Arrange/Act/Assert
- Framework: {xUnit/Jest/Jasmine}
- Mocking: {NSubstitute/Moq/jest.mock}
```

---

## 7. Comparativa: Prompts Buenos vs Malos

### Ejemplo 1: Crear un Servicio

#### ❌ Prompt Malo
```
Haz un servicio de usuarios
```
**Problemas**: No especifica tecnología, operaciones, validaciones, ni patrones.

#### ✅ Prompt Bueno
```
Servicio .NET 8 para gestión de usuarios

Operaciones:
- GetById(int id) → UserDto?
- GetAll() → IEnumerable<UserDto>
- Create(CreateUserDto) → Result<UserDto>
- Update(int id, UpdateUserDto) → Result<UserDto>
- Delete(int id) → Result<bool>

Requisitos:
- Inyectar IUserRepository, IMapper, ILogger<UserService>
- Validar con FluentValidation
- Retornar Result pattern (no excepciones para errores de negocio)
- Async/await en todas las operaciones

Incluir interfaz IUserService
```

---

### Ejemplo 2: Crear un Componente

#### ❌ Prompt Malo
```
Componente para mostrar datos
```

#### ✅ Prompt Bueno
```
Componente Angular 17 standalone: DataTableComponent

Props (Inputs):
- data: T[] (genérico)
- columns: ColumnDef[] con {key, header, sortable}
- loading: boolean

Eventos (Outputs):
- rowClick: EventEmitter<T>
- sortChange: EventEmitter<{column: string, direction: 'asc'|'desc'}>

Features:
- Ordenamiento por columnas
- Estado loading con skeleton
- Estado empty con mensaje personalizable
- Estilos con Tailwind CSS

No incluir paginación (se agregará después)
```

---

## 8. Checklist de Validación

Antes de enviar un prompt, verifica:

### ✅ Claridad
- [ ] ¿Es específico sobre QUÉ generar?
- [ ] ¿Evita ambigüedades?

### ✅ Contexto
- [ ] ¿Incluye tecnología y versión?
- [ ] ¿Menciona el módulo/proyecto donde se usará?

### ✅ Requisitos
- [ ] ¿Define tipos de entrada y salida?
- [ ] ¿Especifica patrones a seguir?
- [ ] ¿Lista validaciones necesarias?

### ✅ Restricciones
- [ ] ¿Indica qué NO hacer?
- [ ] ¿Define límites del scope?

### ✅ Formato
- [ ] ¿Especifica cómo entregar el resultado?
- [ ] ¿Pide comentarios o documentación?

### ✅ Eficiencia
- [ ] ¿Es una tarea manejable (no demasiado amplia)?
- [ ] ¿Usa referencias (#file, #codebase) cuando aplica?

---

## 📚 Recursos Adicionales

- [Optimización de Tokens en Copilot](./optimizacion-tokens-copilot.md)
- [Prompts para Copilot 2025](./copilot-prompts-2025.md)

---

## 9. Estrategias de Optimización (Para Enseñar en Clase)

### 🎓 Técnica 1: Prompts Atómicos

Divide tareas grandes en pasos pequeños y validables.

```
❌ Todo de golpe (~30 tokens prompt, ~3,000 tokens respuesta):
"Crea un controlador de usuarios con CRUD completo, validaciones, 
logging, manejo de errores, autenticación JWT, paginación y filtros"
```

```
✅ Atómico (~40 tokens totales, ~800 tokens respuesta):
Prompt 1: "UserController: inyectar IUserService"
Prompt 2: "GET /api/users endpoint con paginación"
Prompt 3: "POST /api/users con validación ModelState"
Prompt 4: "Agregar [Authorize] attribute a los endpoints"
```

**Beneficios**:
- Código validado paso a paso
- Fácil detectar y corregir errores
- Menos tokens de respuesta

---

### 🎓 Técnica 2: @workspace vs #file

```
❌ Consume mucho contexto:
#file:Models/User.cs
#file:Models/Product.cs  
#file:Models/Order.cs
"Crea DTOs para estos modelos"
→ Carga los 3 archivos COMPLETOS
```

```
✅ Más eficiente:
@workspace "Crea DTOs para User, Product, Order"
→ Copilot busca SOLO lo necesario
```

| Referencia | Usar cuando... |
|------------|----------------|
| `#file` | Necesitas contexto específico de 1 archivo |
| `#selection` | Solo necesitas las líneas seleccionadas |
| `@workspace` | Copilot debe buscar en el proyecto |

---

### 🎓 Técnica 3: Limpiar Contexto Estratégicamente

El contexto del chat **se acumula**. Inicia chats nuevos estratégicamente:

```
┌─────────────────────────────────────────────────────────────┐
│  FLUJO DE TRABAJO ÓPTIMO                                    │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  💬 Chat 1: Crear modelos                                   │
│     Contexto: [████░░░░░░░░░░░░░░░░] 20%                   │
│     ✅ Completado                                           │
│                                                             │
│  💬 Chat 1: Crear repositorios                              │
│     Contexto: [████████░░░░░░░░░░░░] 45%                   │
│     ✅ Completado                                           │
│                                                             │
│  💬 Chat 1: Crear controladores                             │
│     Contexto: [██████████████░░░░░░] 70%                   │
│     ✅ Completado                                           │
│                                                             │
│  🔄 ═══════ NUEVO CHAT (limpiar contexto) ═══════          │
│                                                             │
│  💬 Chat 2: Crear servicios de negocio                      │
│     Contexto: [███░░░░░░░░░░░░░░░░░] 15%                   │
│     ✅ Fresco y eficiente                                   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Regla**: Nuevo chat cada ~3-4 tareas complejas o cuando sientas respuestas lentas.

---

> **Recuerda**: Un prompt bien construido es una inversión que ahorra tiempo, tokens y frustración. Dedica 30 segundos extra a estructurarlo y ahorra 30 minutos de iteraciones.
