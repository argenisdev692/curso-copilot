# 🤖 Ejemplos de Prompts para TicketManagementSystem (Sesión 1)

Este documento contiene ejemplos prácticos de prompts diseñados para aplicar los conceptos de la **Sesión 1** directamente en el desarrollo del proyecto `TicketManagementSystem`. Estos prompts siguen las reglas maestras de arquitectura y buenas prácticas definidas para el curso.

---

## 📌 Tema 1: Introducción a GitHub Copilot

### Subtema: Diferencia entre Copilot Chat y autocompletado tradicional

**Objetivo:** Entender cuándo usar Chat vs. autocompletado inline para máxima productividad.

#### 💬 Prompt para Chat:
> Explica la diferencia entre usar Copilot Chat y el autocompletado inline en el contexto del desarrollo del `TicketManagementSystem`.
>
> Proporciona:
> 1. **3 escenarios donde el autocompletado inline es más eficiente** (ejemplo: completar propiedades de un DTO conocido)
> 2. **3 escenarios donde Copilot Chat es superior** (ejemplo: diseñar la arquitectura de un nuevo módulo)
> 3. Un flujo de trabajo híbrido para implementar el endpoint `GET /api/tickets/{id}` usando ambas herramientas de forma complementaria

#### 💬 Prompt para Práctica:
> **Ejercicio comparativo:**
>
> 1. **Con autocompletado inline:** Comienza a escribir el constructor de `TicketRepository` y deja que Copilot autocomplete los métodos CRUD básicos (GetById, GetAll, Add, Update, Delete).
>
> 2. **Con Copilot Chat:** Pide que revise el código generado, sugiera optimizaciones de rendimiento y añada un método complejo: `GetTicketsWithPaginationAndFiltersAsync`.
>
> 3. **Reflexiona:** ¿Cuándo fue más eficiente cada herramienta? ¿En qué momento cambiaste de una a otra?

---

### Subtema: Limitaciones y riesgos de Copilot en proyectos grandes

**Objetivo:** Identificar situaciones donde Copilot puede generar código subóptimo o inseguro.

#### 💬 Prompt:
> Actúa como un revisor de código senior. Analiza los siguientes riesgos al usar Copilot en el `TicketManagementSystem`:
>
> 1. **Seguridad:** ¿Qué problemas podría introducir Copilot al generar código de autenticación JWT? Proporciona 3 ejemplos de vulnerabilidades comunes (hardcoded secrets, tokens sin expiración, validación incorrecta).
> 2. **Rendimiento:** Identifica patrones anti-performance que Copilot podría generar en consultas EF Core (N+1, falta de `AsNoTracking`, exceso de datos cargados).
> 3. **Arquitectura:** ¿Cómo puede Copilot violar la separación de capas si no se le guía correctamente? Describe un ejemplo donde genera lógica de negocio directamente en el Controller.
>
> Para cada riesgo, proporciona:
> - Descripción del código problemático
> - Explicación de por qué es problemático
> - Estrategia de prompt mejorada que evite ese error

---

### Subtema: Copilot vs ChatGPT: diferencias en el flujo de desarrollo

**Objetivo:** Comprender las ventajas únicas de Copilot integrado en el IDE.

#### 💬 Prompt:
> Compara Copilot y ChatGPT en el desarrollo del `TicketManagementSystem`:
>
> **Escenario:** Necesitas implementar un sistema de notificaciones por email cuando un ticket cambia de estado.
>
> 1. **Con ChatGPT (externo):**
>    - ¿Qué información debes copiar y pegar del proyecto?
>    - ¿Cuántos pasos requiere integrar el código generado?
>    - ¿Qué contexto pierde ChatGPT entre mensajes?
>
> 2. **Con Copilot (en VS Code/VS):**
>    - ¿Qué contexto tiene automáticamente del proyecto?
>    - ¿Cómo usa los archivos abiertos y el workspace?
>    - Demuestra usando `#file` y `#codebase` para generar el servicio `EmailNotificationService`
>
> 3. Crea un flujo de trabajo híbrido donde ChatGPT ayuda en diseño arquitectónico y Copilot en implementación concreta.

---

### Subtema: Estrategias de adopción en equipos de desarrollo

**Objetivo:** Planificar la introducción gradual de Copilot en un equipo real.

#### 💬 Prompt:
> Diseña un plan de adopción de Copilot para un equipo de 5 desarrolladores trabajando en `TicketManagementSystem`.
>
> **Contexto del equipo:**
> - 2 desarrolladores senior (.NET), 2 mid-level (Angular), 1 junior (fullstack)
> - Proyecto en producción, sprints de 2 semanas
> - Preocupaciones: calidad del código, dependencia de IA, curva de aprendizaje
>
> **Entrega un plan que incluya:**
> 1. **Fase 1 (Sprint 1-2):** Tareas específicas donde introducir Copilot (ej: generación de tests, documentación)
> 2. **Fase 2 (Sprint 3-4):** Expansión a desarrollo de features (con checkpoints de code review)
> 3. **Fase 3 (Sprint 5+):** Adopción completa con best practices
> 4. Métricas a medir en cada fase (velocidad, bugs, calidad)
> 5. Ejercicios prácticos para cada nivel de seniority aplicados al proyecto actual

---

### Subtema: KPIs de éxito al usar Copilot en proyectos

**Objetivo:** Definir métricas concretas para evaluar el impacto de Copilot.

#### 💬 Prompt:
> Define un dashboard de KPIs para medir el ROI de Copilot en el desarrollo del `TicketManagementSystem`.
>
> **Métricas a trackear durante 3 meses:**
>
> 1. **Productividad:**
>    - Tiempo promedio para implementar un CRUD completo (con/sin Copilot)
>    - Líneas de código generadas vs. editadas manualmente
>    - Velocidad de resolución de bugs
>
> 2. **Calidad:**
>    - Cobertura de tests antes/después
>    - Número de code smells detectados en SonarQube
>    - Bugs introducidos en código generado vs. manual
>
> 3. **Adopción:**
>    - % de código escrito con asistencia de Copilot
>    - Satisfacción del equipo (encuesta mensual)
>    - Tiempo de onboarding de nuevos desarrolladores
>
> **Entrega:**
> - Template de Excel/Google Sheets para tracking
> - 3 user stories del backlog de `TicketManagementSystem` para medir como benchmark (ej: "Implementar filtros avanzados de búsqueda")
> - Criterios de éxito cuantitativos (ej: "Reducir tiempo de implementación de CRUD en 40%")

---

### Subtema: Casos de uso reales en desarrollo web fullstack

**Objetivo:** Diseñar una funcionalidad completa (Backend + Frontend) asegurando coherencia arquitectónica.

#### 💬 Prompt:
> Actúa como un Arquitecto de Software. Diseña el flujo completo para la funcionalidad de "Asignación Automática de Tickets" en el `TicketManagementSystem`.
>
> 1.  **Backend (.NET 8):** Define la firma del método en `ITicketService` y su implementación en `TicketService` que asigne un ticket a un agente basado en la carga de trabajo actual.
> 2.  **Frontend (Angular):** Genera el método en `TicketService` (TypeScript) para consumir este endpoint.
>
> **Requisitos:**
> *   Usa `Result<T>` o `ApiResponse<T>` para la respuesta.
> *   Asegura manejo de excepciones y logging estructurado con `ILogger`.
> *   El frontend debe manejar estados de carga y errores con `RxJS`.

---

## 📌 Tema 2: Prompt Engineering para Desarrolladores

### Subtema: Tipos de prompts - Descripción, Contexto, Tarea y Ejemplo

**Objetivo:** Dominar los 4 tipos de prompts y cuándo usar cada uno.

#### 💬 Prompt Tipo 1: DESCRIPCIÓN (para autocompletado inline)
> Escribe un comentario descriptivo antes de la firma del método:
>
> *"Método que valida si un ticket puede ser cerrado: debe tener todos los comentarios respondidos, no puede tener subtareas pendientes, y el usuario debe ser el asignado o un admin"*
>
> Luego escribe solo la firma del método `CanCloseTicketAsync` y observa cómo Copilot autocompleta la implementación basándose en la descripción.

#### 💬 Prompt Tipo 2: CONTEXTO (para generar código coherente con el proyecto)
> Genera un repositorio `CommentRepository` para el `TicketManagementSystem`.
>
> **Contexto del proyecto:**
> - Usamos el patrón Repository con Entity Framework Core
> - Todos los repositorios implementan `IRepository<T>` genérico
> - Tenemos auditoría automática con `ISoftDelete` y `IAuditable`
> - El `DbContext` se llama `AppDbContext`
> - Estamos en .NET 8 con C# 12
>
> El repositorio debe incluir métodos específicos para:
> - Obtener comentarios de un ticket con paginación
> - Marcar comentarios como leídos por un usuario

#### 💬 Prompt Tipo 3: TAREA (para workflows complejos)
> **Tarea:** Implementar autenticación JWT en el `TicketManagementSystem`
>
> **Pasos a seguir:**
> 1. Crea la entidad `RefreshToken` con relación a `User`
> 2. Implementa `ITokenService` con métodos para generar, validar y refrescar tokens
> 3. Crea el endpoint `POST /api/auth/refresh` en `AuthController`
> 4. Configura JWT en `Program.cs` (issuer, audience, key desde appsettings)
> 5. Añade el middleware de autenticación en el pipeline
>
> Explica el enfoque arquitectónico antes de generar código. Trabaja paso a paso, esperando confirmación antes de continuar.

#### 💬 Prompt Tipo 4: EJEMPLO (para casos muy específicos)
> Necesito un método para calcular estadísticas de tickets. Aquí hay un ejemplo de entrada/salida:
>
> **Entrada:** Lista de tickets con propiedades Id, Status (Open/Closed), Priority (High/Medium/Low), CreatedAt y ClosedAt.
>
> **Salida esperada:** Un objeto con:
> - TotalTickets: 2
> - OpenTickets: 1
> - ClosedTickets: 1
> - AverageResolutionTimeInDays: 10
> - HighPriorityPercentage: 50.0
>
> Genera el método `CalculateTicketStatistics` que produzca este resultado. Usa LINQ y retorna un record `TicketStatistics` con estas propiedades.

---

### Subtema: Uso de comentarios y docstrings para guiar la generación de código

**Objetivo:** Aprovechar comentarios estratégicos para generar código preciso.

#### 💬 Prompt:
> Crea el servicio `TicketAssignmentService` usando comentarios estratégicos para guiar la generación:
>
> **Estructura de comentarios a incluir:**
> - Comentario XML de documentación con resumen, parámetros, retorno y excepciones
> - Comentarios inline describiendo cada paso de la lógica:
>   1. Validar que el ticket existe y no está ya asignado
>   2. Obtener todos los agentes activos con rol "Agent"
>   3. Calcular carga de trabajo (contar tickets abiertos asignados)
>   4. Seleccionar el agente con menos tickets
>   5. Asignar el ticket y guardar en base de datos
>   6. Loguear la asignación con ILogger
>   7. Retornar el usuario asignado
>
> Observa cómo Copilot genera la implementación basándose en estos comentarios.

#### 💬 Prompt para Comparación:
> Genera el mismo servicio `TicketAssignmentService` pero ahora usando Copilot Chat con un prompt detallado en lenguaje natural (sin comentarios en el código).
>
> Compara:
> - Tiempo de generación
> - Precisión del código
> - Necesidad de ajustes manuales
> - ¿Cuál método preferirías en tu flujo de trabajo diario?

---

### Subtema: Prompts para generación de controladores, componentes o servicios

**Objetivo:** Dominar la generación de componentes arquitectónicos completos.

#### 💬 Prompt para Controlador (.NET):
> Genera el controlador `TicketsController` para la API del `TicketManagementSystem`.
>
> **Especificaciones:**
> - Debe heredar de `ControllerBase` y usar attribute routing
> - Implementar todos los endpoints CRUD (GET lista, GET por ID, POST, PUT, DELETE)
> - Incluir endpoint adicional: `PATCH /api/tickets/{id}/assign` para asignar tickets
> - Cada endpoint debe documentarse con `[ProducesResponseType]` para Swagger
> - Usar inyección de dependencias para `ITicketService` y `ILogger`
> - Seguir el estándar RFC 7807 para respuestas de error
> - NO incluir lógica de negocio, solo coordinar entre la request y el servicio

#### 💬 Prompt para Servicio (Angular):
> Crea el servicio `TicketService` en Angular para el módulo de tickets.
>
> **Requisitos:**
> - Usar `HttpClient` para comunicación con la API
> - Implementar métodos: getTickets (con paginación), getTicketById, createTicket, updateTicket, deleteTicket, assignTicket
> - Todos los métodos deben retornar `Observable` con tipos específicos
> - Incluir manejo de errores centralizado con transformación de mensajes del backend
> - Usar interfaces TypeScript para todos los DTOs (TicketResponse, CreateTicketRequest, etc.)
> - Incluir headers de autenticación JWT en todas las peticiones
> - Implementar retry logic para fallos de red

#### 💬 Prompt para Componente (Angular):
> Diseña el componente `TicketListComponent` con arquitectura standalone de Angular.
>
> **Funcionalidad:**
> - Mostrar tabla de tickets con columnas: ID, Título, Estado, Prioridad, Fecha Creación, Acciones
> - Incluir filtros por Estado y Prioridad
> - Implementar paginación server-side
> - Botones de acción: Ver detalle, Editar, Eliminar (con confirmación)
>
> **Requisitos Técnicos:**
> - Usar Signals para el estado del componente
> - Implementar debounce en los filtros de búsqueda
> - Manejar estados de loading, error y sin datos
> - Usar OnPush change detection strategy
> - Implementar navegación a detalle y creación de tickets

---

### Subtema: Uso de ejemplos de entrada y salida para afinar resultados

**Objetivo:** Mejorar la precisión de Copilot mediante ejemplos concretos.

#### 💬 Prompt con Ejemplo de Transformación:
> Necesito un método que transforme una lista de tickets de la base de datos a DTOs para la API.
>
> **Entrada:** Entidad `Ticket` con propiedades: Id, Title, Description, Status (enum), Priority (enum), CreatedAt, UpdatedAt, AssignedUser (navegación), Category (navegación).
>
> **Salida esperada:** `TicketResponseDto` con propiedades: Id, Title, Status (string), Priority (string), CreatedAt, AssignedUserName (string o null), CategoryName (string).
>
> **Transformaciones específicas:**
> - Enum Status y Priority deben convertirse a strings legibles
> - Si no hay usuario asignado, AssignedUserName debe ser null (no "N/A" ni cadena vacía)
> - CategoryName viene de la navegación Category.Name
>
> Crea el método de mapeo manual (sin AutoMapper) que realice esta transformación de forma óptima con LINQ.

#### 💬 Prompt con Ejemplo de Validación:
> Implementa un validador personalizado para el DTO `CreateTicketDto`.
>
> **Ejemplo de entrada válida:**
> - Title: "Error en módulo de pagos"
> - Description: "Al intentar procesar un pago con tarjeta Visa, el sistema retorna error 500"
> - CategoryId: 3
> - Priority: "High"
>
> **Ejemplo de entrada inválida:**
> - Title: "ayuda" (muy genérico y corto)
> - Description: "no funciona" (muy corto e inespecífico)
> - CategoryId: 0 (inválido)
> - Priority: "Urgent" (no existe en el enum)
>
> Crea un validador con FluentValidation que rechace descripciones genéricas menores a 20 caracteres y títulos con palabras como "ayuda", "error", "no funciona" si no tienen más contexto.

---

### Subtema: Ajuste del estilo de código según convenciones del equipo

**Objetivo:** Configurar Copilot para respetar los estándares del proyecto.

#### 💬 Prompt para Configuración de Estilo:
> Configura tu workspace para que Copilot respete las siguientes convenciones del `TicketManagementSystem`:
>
> **Backend (.NET):**
> - Interfaces siempre con prefijo `I`
> - Servicios con sufijo `Service`
> - Métodos async siempre con sufijo `Async`
> - Usar `var` solo cuando el tipo es obvio
> - Llaves siempre en nueva línea (Allman style)
> - Comentarios XML obligatorios en todos los métodos públicos
> - Usar `_camelCase` para campos privados
>
> **Frontend (Angular):**
> - Archivos con nomenclatura kebab-case
> - Clases con PascalCase
> - Métodos y propiedades con camelCase
> - Usar Signals en lugar de BehaviorSubject cuando sea posible
> - Preferir arrow functions en callbacks
> - Destructuring en parámetros cuando tenga sentido
>
> Genera un archivo `.editorconfig` y configuración de Copilot que refleje estas reglas.

#### 💬 Prompt para Revisión de Estilo:
> Revisa el siguiente archivo y ajústalo a las convenciones del equipo sin cambiar la lógica:
>
> Usa las reglas maestras del proyecto definidas en `.github/instructions/rules.instructions.md`. Identifica:
> 1. Variables que no siguen naming conventions
> 2. Métodos async sin sufijo `Async`
> 3. Falta de comentarios XML
> 4. Validaciones que deberían estar en FluentValidation
> 5. Logging incorrecto (Console.WriteLine en lugar de ILogger)

---

### Subtema: Mejora incremental del código mediante prompts iterativos

**Objetivo:** Refinar código existente en múltiples iteraciones.

#### 💬 Prompt Iteración 1 - Funcionalidad Básica:
> Crea un método básico `SearchTickets` que reciba un término de búsqueda y retorne tickets que contengan ese término en el título o descripción.

#### 💬 Prompt Iteración 2 - Optimización:
> Mejora el método anterior añadiendo:
> - Búsqueda case-insensitive
> - Uso de `.AsNoTracking()` para mejorar rendimiento
> - Paginación (pageNumber y pageSize como parámetros)

#### 💬 Prompt Iteración 3 - Filtros Avanzados:
> Extiende el método para incluir filtros opcionales:
> - Filtro por estado (puede ser múltiple)
> - Filtro por prioridad
> - Rango de fechas (desde/hasta)
> - Usuario asignado

#### 💬 Prompt Iteración 4 - Performance y Logging:
> Optimiza el método final:
> - Usa proyección directa a DTO para evitar cargar entidades completas
> - Añade logging estructurado con tiempo de ejecución
> - Implementa cache para búsquedas repetidas frecuentemente
> - Añade métricas de uso (número de filtros aplicados, resultados retornados)

---

### Subtema: Generación de código reusable, snippets y utilidades internas

**Objetivo:** Crear librerías internas y utilidades reutilizables.

#### 💬 Prompt para Utilidad Genérica:
> Crea una clase estática `PaginationHelper` con métodos de utilidad para paginación que pueda usarse en todo el proyecto.
>
> **Métodos requeridos:**
> 1. `Paginate<T>(IQueryable<T> query, int pageNumber, int pageSize)` - Retorna resultado paginado
> 2. `CalculateTotalPages(int totalItems, int pageSize)` - Calcula total de páginas
> 3. `GetSkipCount(int pageNumber, int pageSize)` - Calcula cuántos registros saltar
> 4. `IsValidPageNumber(int pageNumber, int totalPages)` - Valida si el número de página es válido
>
> Debe retornar un objeto `PagedResult<T>` con: Items (lista), TotalItems, CurrentPage, TotalPages, HasPreviousPage, HasNextPage.

#### 💬 Prompt para Extension Methods:
> Crea extension methods para `IQueryable<Ticket>` que encapsulen filtros comunes:
>
> **Extension methods a crear:**
> - `WhereStatus(TicketStatus status)` - Filtra por estado
> - `WherePriority(Priority priority)` - Filtra por prioridad
> - `WhereCreatedBetween(DateTime from, DateTime to)` - Filtra por rango de fechas
> - `WhereAssignedTo(int userId)` - Filtra por usuario asignado
> - `WhereSearchTerm(string searchTerm)` - Búsqueda en título y descripción
> - `OrderByCreatedDate(bool descending = true)` - Ordenamiento por fecha
>
> Deben ser chainables (poder combinar múltiples filtros).

#### 💬 Prompt para Snippet Personalizado:
> Crea snippets de VS Code para acelerar el desarrollo en el proyecto:
>
> **Snippets para Backend:**
> - `service-interface` - Genera interface IService básica
> - `service-impl` - Genera implementación de servicio con inyección de dependencias
> - `controller-crud` - Genera controlador con endpoints CRUD
> - `dto-record` - Genera record DTO con validaciones
>
> **Snippets para Frontend:**
> - `ng-component-signals` - Componente standalone con Signals
> - `ng-service-http` - Servicio con HttpClient y manejo de errores
> - `ng-form-reactive` - Formulario reactivo con validaciones
>
> Genera el JSON de configuración de snippets para ambos lenguajes.

---

### Subtema: Aplicación de prompts en testing, documentación y CI/CD

**Objetivo:** Usar Copilot para tareas más allá del código productivo.

#### 💬 Prompt para Tests Unitarios:
> Genera tests unitarios completos para el servicio `TicketService` usando xUnit y NSubstitute.
>
> **Cobertura requerida:**
> - Test para cada método público (GetById, GetAll, Create, Update, Delete)
> - Casos happy path y casos de error
> - Tests para validaciones de negocio
> - Tests para excepciones específicas (TicketNotFoundException, ValidationException)
> - Uso de Arrange-Act-Assert pattern
> - Nombres descriptivos: `MetodoTesteado_Escenario_ResultadoEsperado`
>
> Crea mocks para ITicketRepository, ILogger y cualquier otra dependencia.

#### 💬 Prompt para Documentación:
> Genera documentación completa para el módulo de Tickets del `TicketManagementSystem`:
>
> **Documentos a crear:**
> 1. **README.md del módulo** - Descripción, arquitectura, endpoints disponibles
> 2. **API.md** - Documentación detallada de cada endpoint con ejemplos de request/response
> 3. **ARCHITECTURE.md** - Diagrama de flujo, patrones utilizados, decisiones de diseño
> 4. **TESTING.md** - Guía de cómo ejecutar tests, convenciones de testing
>
> Incluye ejemplos prácticos de uso y troubleshooting común.

#### 💬 Prompt para CI/CD:
> Crea un pipeline de CI/CD para el `TicketManagementSystem` usando GitHub Actions.
>
> **Pipeline de Backend (.NET):**
> - Trigger en push a main y PRs
> - Pasos: Restore, Build, Test (con reporte de cobertura), Análisis de código con SonarQube
> - Build de imagen Docker
> - Deploy a Azure App Service (solo en main)
>
> **Pipeline de Frontend (Angular):**
> - Instalación de dependencias con npm
> - Linting con ESLint
> - Tests con Karma/Jest
> - Build de producción
> - Deploy a Azure Static Web Apps
>
> Incluye manejo de secretos, notificaciones de fallo y badges de estado.

---

### Subtema: Errores comunes al escribir prompts y cómo evitarlos

**Objetivo:** Identificar y corregir anti-patterns en prompts.

#### 💬 Prompt de Análisis:
> Analiza los siguientes prompts problemáticos y explica por qué son ineficaces:
>
> **Prompt Problemático 1:** "Crea un servicio"
> - **Problema:** Muy vago, sin contexto
> - **Corrección:** Especificar qué servicio, para qué funcionalidad, con qué dependencias, siguiendo qué patrones
>
> **Prompt Problemático 2:** "Haz que este código funcione" (sin mostrar el código)
> - **Problema:** Falta de contexto, Copilot no puede ver el código
> - **Corrección:** Usar `#selection` o `#file` para dar contexto del código a revisar
>
> **Prompt Problemático 3:** "Genera toda la aplicación de gestión de tickets"
> - **Problema:** Demasiado amplio, resultado será genérico
> - **Corrección:** Dividir en tareas pequeñas, ir paso a paso con prompts específicos
>
> **Prompt Problemático 4:** "Crea un controller con todos los métodos necesarios y bien documentado"
> - **Problema:** "Necesarios" es subjetivo, "bien documentado" es ambiguo
> - **Corrección:** Especificar exactamente qué endpoints, qué documentación (XML comments, Swagger attributes)
>
> Reescribe cada prompt problemático siguiendo las mejores prácticas.

#### 💬 Prompt de Práctica:
> **Ejercicio:** Identifica los errores en tu último prompt usado hoy y reescríbelo.
>
> **Checklist de validación:**
> - [ ] ¿Es específico sobre QUÉ generar?
> - [ ] ¿Proporciona contexto suficiente del proyecto?
> - [ ] ¿Especifica tecnologías y versiones?
> - [ ] ¿Define los requisitos técnicos claramente?
> - [ ] ¿Indica patrones o estándares a seguir?
> - [ ] ¿Es una tarea manejable (no demasiado amplia)?
> - [ ] ¿Menciona archivos o tipos existentes del proyecto usando #file?

---

### Subtema: Creación de funciones y clases a partir de descripciones detalladas

**Objetivo:** Generar lógica de dominio compleja con especificaciones claras.

#### 💬 Prompt:
> Genera una clase de servicio de dominio llamada `SlaCalculatorService` para el proyecto `TicketManagementSystem`.
>
> **Responsabilidad:** Calcular la fecha de vencimiento de un ticket basándose en su prioridad y el horario laboral (9:00 - 18:00, Lunes a Viernes).
>
> **Especificaciones Técnicas:**
> *   Implementa una interfaz `ISlaCalculatorService`.
> *   Usa C# 12 y .NET 8.
> *   Maneja días festivos (pásalos como una lista de `DateTime` inyectada o configurada).
> *   Incluye validaciones de entrada (no fechas pasadas).
> *   Documenta todos los métodos públicos con comentarios XML.
> *   No uses `Console.WriteLine`, usa `ILogger`.

### Subtema: Uso de Copilot para refactorizar y documentar funciones existentes

**Objetivo:** Modernizar código legacy o mejorar código existente.

#### 💬 Prompt:
> Refactoriza el siguiente método legacy `ProcessTicketUpdate` (asume que te paso el código) para cumplir con los estándares de Clean Code y SOLID del proyecto:
>
> 1.  Extrae la lógica de validación a una clase `FluentValidation`.
> 2.  Usa el patrón **Early Return** para reducir la anidación.
> 3.  Reemplaza los `throw new Exception` genéricos por excepciones de dominio personalizadas (`TicketNotFoundException`, `ValidationException`).
> 4.  Asegura que las llamadas a base de datos sean asíncronas (`await`, `CancellationToken`).
> 5.  Añade comentarios XML explicando el "Por qué" de la lógica compleja.

---

## 📌 Tema 3: Copilot en Backend con .NET

### Subtema: Configuración inicial y uso de Copilot en proyectos .NET

**Objetivo:** Optimizar el entorno de desarrollo para aprovechar Copilot al máximo.

#### 💬 Prompt para Configuración:
> Configura un proyecto .NET 8 nuevo para el `TicketManagementSystem` con la estructura óptima para trabajar con Copilot.
>
> **Estructura del proyecto:**
> - TicketManagement.API (Web API)
> - TicketManagement.Core (Entidades, Interfaces, DTOs)
> - TicketManagement.Infrastructure (EF Core, Repositorios)
> - TicketManagement.Application (Servicios, Lógica de negocio)
> - TicketManagement.Tests (xUnit, tests unitarios)
>
> **Configuración adicional:**
> - Habilitar Nullable Reference Types en todos los proyectos
> - Configurar EditorConfig con reglas de estilo del equipo
> - Añadir archivo `.copilotignore` para excluir archivos generados
> - Crear archivo `architecture-rules.md` que Copilot pueda referenciar con #file
> - Configurar Swagger con documentación XML automática

#### 💬 Prompt para Workspace Settings:
> Crea la configuración óptima de VS Code para desarrollo .NET con Copilot en el proyecto.
>
> **Settings a configurar:**
> - Extensiones recomendadas (C# Dev Kit, Copilot, etc.)
> - Tasks de build y run
> - Launch configurations para debugging
> - Snippets personalizados del proyecto
> - IntelliCode settings
> - Configuración de Copilot (sugerencias inline, filtros de lenguaje)

---

### Subtema: Uso de Copilot para crear y extender middlewares personalizados

**Objetivo:** Generar middlewares robustos para cross-cutting concerns.

#### 💬 Prompt para Middleware de Logging:
> Crea un middleware personalizado `RequestLoggingMiddleware` para el `TicketManagementSystem`.
>
> **Funcionalidad:**
> - Loguear cada request HTTP (método, path, query params, user id si está autenticado)
> - Capturar el tiempo de ejecución de la request
> - Loguear la response (status code, tiempo de respuesta)
> - Generar un CorrelationId único para cada request y añadirlo a los headers de response
> - Usar logging estructurado con ILogger
> - No loguear requests a endpoints de health check o Swagger
>
> Implementa el middleware y su extension method para registrarlo en el pipeline.

#### 💬 Prompt para Middleware de Manejo de Errores:
> Implementa un `GlobalExceptionHandlerMiddleware` usando la nueva interface `IExceptionHandler` de .NET 8.
>
> **Requisitos:**
> - Capturar excepciones no manejadas
> - Retornar respuestas en formato ProblemDetails (RFC 7807)
> - Diferenciar entre excepciones de dominio (404, 400) y errores del servidor (500)
> - Loguear stacktrace completo solo para errores 500
> - Incluir CorrelationId en la respuesta de error
> - En desarrollo, incluir detalles del error; en producción, mensajes genéricos
> - Manejar específicamente: NotFoundException, ValidationException, UnauthorizedException

#### 💬 Prompt para Middleware de Performance:
> Crea un middleware `PerformanceMonitoringMiddleware` para detectar requests lentas.
>
> **Funcionalidad:**
> - Medir tiempo de ejecución de cada request
> - Si supera un threshold configurable (ej: 3 segundos), loguear warning con detalles
> - Añadir header `X-Response-Time` con el tiempo en milisegundos
> - Permitir configurar endpoints a ignorar
> - Integrar con Application Insights para tracking de métricas

---

### Subtema: Ejemplo práctico - CRUD completo con Copilot en C#

**Objetivo:** Generar un módulo completo funcional paso a paso.

#### 💬 Prompt Paso 1 - Entidad:
> Crea la entidad `Comment` para el sistema de comentarios en tickets.
>
> **Propiedades:**
> - Id (int, primary key)
> - Content (string, requerido, max 2000 caracteres)
> - TicketId (int, foreign key)
> - AuthorId (int, foreign key a User)
> - CreatedAt, UpdatedAt (auditoría)
> - IsDeleted (soft delete)
> - Propiedades de navegación: Ticket, Author
>
> Implementa también la configuración de EF Core con Fluent API.

#### 💬 Prompt Paso 2 - DTOs:
> Genera los DTOs para el CRUD de comentarios:
> - `CommentResponseDto` (para retornar al cliente)
> - `CreateCommentDto` (para crear nuevos comentarios)
> - `UpdateCommentDto` (para actualizar comentarios existentes)
>
> Incluye validaciones con Data Annotations y crea validadores de FluentValidation para reglas complejas.

#### 💬 Prompt Paso 3 - Repository:
> Implementa `ICommentRepository` y `CommentRepository` con los siguientes métodos:
> - GetByIdAsync
> - GetCommentsByTicketIdAsync (con paginación)
> - AddAsync
> - UpdateAsync
> - DeleteAsync (soft delete)
> - GetCommentCountByTicketAsync
>
> Usa patrones de repository y Unit of Work.

#### 💬 Prompt Paso 4 - Service:
> Crea `ICommentService` y `CommentService` con lógica de negocio:
> - Validar que el ticket existe antes de añadir comentario
> - Solo el autor o un admin pueden editar/eliminar un comentario
> - Notificar al asignado del ticket cuando se añade un comentario
> - Retornar ApiResponse<T> en todos los métodos
> - Incluir logging estructurado y manejo de excepciones

#### 💬 Prompt Paso 5 - Controller:
> Implementa `CommentsController` con todos los endpoints CRUD.
> Documenta cada endpoint con atributos para Swagger y incluye autorización apropiada.

#### 💬 Prompt Paso 6 - Tests:
> Genera tests unitarios completos para `CommentService` cubriendo todos los casos de éxito y error.

---

### Subtema: Refactorización y optimización de lógica compleja en servicios

**Objetivo:** Mejorar código existente identificando oportunidades de optimización.

#### 💬 Prompt para Análisis:
> Analiza el servicio `TicketService` del proyecto y sugiere refactorizaciones para:
>
> **Code Smells a buscar:**
> - Métodos muy largos (>50 líneas)
> - Responsabilidades múltiples en un solo método
> - Código duplicado
> - Lógica condicional compleja (nested ifs)
> - Falta de manejo de errores
> - Queries N+1 en EF Core
> - Uso innecesario de cargas eager
>
> Para cada problema encontrado, propón la solución específica aplicando SOLID.

#### 💬 Prompt para Extracción de Métodos:
> Refactoriza el método `UpdateTicket` extrayendo responsabilidades a métodos privados:
>
> **Extracciones sugeridas:**
> - ValidateTicketUpdatePermissions (verificar que el usuario puede actualizar)
> - ValidateStatusTransition (validar que la transición de estado es válida)
> - NotifyStakeholders (notificar a usuarios relevantes del cambio)
> - LogTicketChange (auditoría del cambio)
>
> Mantén el método principal como un coordinador limpio de alto nivel.

#### 💬 Prompt para Patrón Strategy:
> La lógica de cálculo de SLA varía según el tipo de ticket (Incident, Request, Problem). Refactoriza usando el patrón Strategy.
>
> **Implementa:**
> - Interface `ISlaCalculator`
> - Implementaciones: `IncidentSlaCalculator`, `RequestSlaCalculator`, `ProblemSlaCalculator`
> - Factory o Strategy pattern para seleccionar el calculador apropiado
> - Inyección de dependencias para los calculadores

---

### Subtema: Generación de consultas LINQ y expresiones lambda optimizadas

**Objetivo:** Crear queries eficientes y legibles con EF Core.

#### 💬 Prompt para Query Compleja:
> Crea una consulta LINQ optimizada para el dashboard de tickets que retorne:
>
> **Datos requeridos:**
> - Total de tickets agrupados por estado
> - Tickets de alta prioridad sin asignar
> - Tickets próximos a vencer SLA (menos de 2 horas)
> - Top 5 categorías con más tickets abiertos
> - Tiempo promedio de resolución por categoría
>
> **Optimizaciones:**
> - Usar una sola query a base de datos si es posible
> - Proyectar directamente a DTOs
> - Usar AsNoTracking
> - Evitar cargas N+1

#### 💬 Prompt para Expression Builder:
> Crea un builder de expresiones dinámicas para filtrado avanzado de tickets.
>
> **Funcionalidad:**
> - Permitir combinar múltiples filtros (AND/OR)
> - Filtros disponibles: Status, Priority, Category, AssignedUser, DateRange, SearchTerm
> - Construir Expression<Func<Ticket, bool>> dinámicamente
> - Aplicar la expresión a IQueryable de forma performante
>
> Usa PredicateBuilder o similar para combinar expresiones.

#### 💬 Prompt para Proyección Eficiente:
> Mejora esta consulta que tiene problemas de performance:
>
> Consulta actual: Carga todas las entidades Ticket con includes de User, Category, Comments y luego mapea a DTOs en memoria.
>
> **Optimización requerida:**
> - Proyectar directamente a DTO en la query
> - Solo seleccionar columnas necesarias
> - Usar Select() en lugar de Include()
> - Comparar performance antes/después (añadir logging de tiempo de ejecución)

---

### Subtema: Creación de documentación XML, Swagger y comentarios de código

**Objetivo:** Generar documentación profesional automáticamente.

#### 💬 Prompt para Comentarios XML:
> Añade comentarios XML completos a todos los métodos públicos del servicio `TicketService`.
>
> **Requisitos para cada método:**
> - `<summary>`: Descripción clara de qué hace el método
> - `<param>`: Explicación de cada parámetro
> - `<returns>`: Qué retorna el método
> - `<exception>`: Qué excepciones puede lanzar
> - `<remarks>`: Notas adicionales sobre comportamiento especial o efectos secundarios
> - Ejemplos de uso cuando el método sea complejo

#### 💬 Prompt para Configuración Swagger:
> Configura Swagger en el proyecto con documentación profesional:
>
> **Configuración requerida:**
> - Incluir XML documentation en los endpoints
> - Añadir descripción del API, versión, contacto, licencia
> - Configurar autenticación JWT en Swagger UI
> - Añadir ejemplos de request/response para endpoints principales
> - Agrupar endpoints por tags (Tickets, Users, Comments, etc.)
> - Añadir descripciones ricas con markdown
> - Incluir códigos de respuesta HTTP con ejemplos de ProblemDetails

#### 💬 Prompt para README Técnico:
> Genera un README.md completo para el módulo de backend del `TicketManagementSystem`.
>
> **Secciones a incluir:**
> 1. Descripción general y propósito
> 2. Arquitectura y patrones utilizados
> 3. Estructura de carpetas explicada
> 4. Requisitos y dependencias
> 5. Configuración inicial (connection strings, secrets, etc.)
> 6. Cómo ejecutar el proyecto localmente
> 7. Cómo ejecutar los tests
> 8. Endpoints principales con ejemplos
> 9. Decisiones de diseño importantes
> 10. Troubleshooting común

---

### Subtema: Aplicación de principios SOLID y patrones de diseño sugeridos por IA

**Objetivo:** Mejorar la arquitectura aplicando principios y patrones.

#### 💬 Prompt para Análisis SOLID:
> Revisa el código del proyecto `TicketManagementSystem` e identifica violaciones de principios SOLID:
>
> **Analiza:**
> - **SRP:** ¿Hay clases con múltiples responsabilidades?
> - **OCP:** ¿Hay código que requiere modificación en lugar de extensión?
> - **LSP:** ¿Las herencias son correctas y sustituibles?
> - **ISP:** ¿Hay interfaces demasiado grandes que fuerzan implementaciones innecesarias?
> - **DIP:** ¿Hay dependencias de clases concretas en lugar de abstracciones?
>
> Para cada violación, propón la refactorización específica.

#### 💬 Prompt para Patrón Repository:
> El proyecto actualmente tiene acceso directo a DbContext desde los servicios. Implementa el patrón Repository + Unit of Work.
>
> **Implementación requerida:**
> - Interface genérica `IRepository<T>`
> - Implementación base `Repository<T>`
> - Repositorios específicos con métodos custom (ITicketRepository)
> - Interface `IUnitOfWork` para transacciones
> - Registro en inyección de dependencias

#### 💬 Prompt para Patrón CQRS:
> Evalúa si el patrón CQRS (Command Query Responsibility Segregation) beneficiaría al proyecto.
>
> **Análisis:**
> - Identifica comandos (Create, Update, Delete) y queries (Get, Search)
> - Propón estructura con MediatR: Commands, Queries, Handlers
> - Implementa un ejemplo completo: CreateTicketCommand con su Handler
> - Compara complejidad vs beneficios para el tamaño del proyecto

---

### Subtema: Buenas prácticas de seguridad, validación y manejo de excepciones

**Objetivo:** Asegurar el código contra vulnerabilidades comunes.

#### 💬 Prompt para Auditoría de Seguridad:
> Realiza una auditoría de seguridad del código del `TicketManagementSystem` y verifica:
>
> **Checklist de seguridad:**
> - [ ] No hay secrets hardcodeados (connection strings, JWT keys, API keys)
> - [ ] Todos los endpoints sensibles tienen autorización
> - [ ] Validación de entrada en todos los DTOs
> - [ ] Protección contra SQL Injection (uso correcto de EF Core)
> - [ ] Protección contra XSS en campos de texto
> - [ ] Rate limiting en endpoints públicos
> - [ ] CORS configurado correctamente
> - [ ] Passwords hasheadas con algoritmos seguros
> - [ ] JWT con expiración apropiada
> - [ ] Logging que no expone información sensible

#### 💬 Prompt para Sistema de Validación:
> Implementa un sistema robusto de validación multicapa:
>
> **Capas de validación:**
> 1. **Data Annotations** en DTOs (validaciones simples)
> 2. **FluentValidation** para reglas de negocio complejas
> 3. **Validaciones de dominio** en la capa de servicios
> 4. **Global validation filter** que captura errores de modelo y retorna ProblemDetails
>
> Implementa un ejemplo completo para CreateTicketDto.

#### 💬 Prompt para Excepciones Personalizadas:
> Crea un sistema de excepciones de dominio para el proyecto:
>
> **Excepciones a crear:**
> - `TicketManagementException` (base abstracta)
> - `NotFoundException` (para recursos no encontrados)
> - `ValidationException` (para errores de validación de negocio)
> - `UnauthorizedException` (para problemas de permisos)
> - `BusinessRuleException` (para violaciones de reglas de negocio)
>
> Cada excepción debe incluir: mensaje, código de error, metadatos adicionales.
> Configura el exception handler para mapear cada tipo a un HTTP status code apropiado.

---

### Subtema: Consejos de migración asistida por IA entre versiones de .NET y C#

**Objetivo:** Modernizar código legacy aprovechando nuevas características.

#### 💬 Prompt para Migración .NET 6 → .NET 8:
> Analiza el proyecto y sugiere mejoras al migrar de .NET 6 a .NET 8:
>
> **Áreas de modernización:**
> - Usar Minimal APIs donde tenga sentido
> - Aprovechar nuevas características de C# 12 (primary constructors, collection expressions)
> - Migrar a System.Text.Json si aún usa Newtonsoft
> - Usar el nuevo `IExceptionHandler` en lugar de middleware custom
> - Aprovechar mejoras de performance en EF Core 8
> - Actualizar paquetes NuGet a versiones compatibles
>
> Proporciona un plan de migración paso a paso con riesgos identificados.

#### 💬 Prompt para Modernización de C#:
> Refactoriza el código existente para usar características modernas de C# 12:
>
> **Modernizaciones a aplicar:**
> - Primary constructors en servicios y controladores
> - Collection expressions para inicialización de listas
> - Pattern matching avanzado en lugar de if/else
> - Inline arrays donde sea beneficioso
> - Alias de tipos para mejorar legibilidad
> - Init-only properties para inmutabilidad
>
> Muestra ejemplos antes/después de cada refactorización.

#### 💬 Prompt para Análisis de Compatibilidad:
> Antes de migrar a .NET 8, analiza:
>
> - Dependencias NuGet y su compatibilidad con .NET 8
> - Código deprecado que será removido
> - Breaking changes que afectarían al proyecto
> - Características nuevas que podrías aprovechar
> - Riesgos y plan de rollback si algo falla
>
> Genera un reporte de compatibilidad con prioridades (must-fix, should-fix, nice-to-have).

---

## 📌 Tema 3: Copilot en Backend con .NET
### Subtema: Generación de controladores y endpoints API REST en C#

**Objetivo:** Crear endpoints robustos y documentados.

#### 💬 Prompt:
> Crea el controlador `TicketsController` para `TicketManagementSystem.API`.
>
> **Requisitos:**
> *   Hereda de `ControllerBase`.
> *   Implementa un endpoint `POST` para crear tickets (`CreateTicket`).
> *   Recibe un `CreateTicketDto` y retorna `ApiResponse<TicketResponseDto>`.
> *   Usa `[ProducesResponseType]` para documentar códigos 201, 400 y 500 en Swagger.
> *   Inyecta `ITicketService` y `ILogger<TicketsController>`.
> *   Sigue el estándar RFC 7807 (ProblemDetails) para errores.
> *   No pongas lógica de negocio en el controlador, delega todo al servicio.

### Subtema: Creación de modelos, entidades y relaciones con Entity Framework Core

**Objetivo:** Modelado de datos eficiente con EF Core.

#### 💬 Prompt:
> Genera la entidad `Ticket` y su configuración de Entity Framework Core para `TicketManagementSystem`.
>
> **Detalles de la Entidad:**
> *   Propiedades: `Id` (int), `Title`, `Description`, `Status` (Enum), `Priority` (Enum), `CreatedAt`, `AssignedUserId` (nullable).
> *   Relaciones: Un Ticket pertenece a un `Category` y opcionalmente a un `User` (Agente).
>
> **Configuración (Fluent API):**
> *   Implementa `IEntityTypeConfiguration<Ticket>`.
> *   Configura `DeleteBehavior.Restrict` para las relaciones.
> *   Define índices en `Status` y `CreatedAt` para optimizar consultas.
> *   Implementa la interfaz `IAuditable` (si existe en el contexto) o añade propiedades de auditoría.

### Subtema: Generación de DTOs, validaciones de entrada y manejo de respuestas

**Objetivo:** Implementar validación robusta y DTOs inmutables.

#### 💬 Prompt:
> Crea un DTO inmutable (`record`) llamado `CreateTicketDto` y su validador correspondiente con `FluentValidation`.
>
> **Campos:**
> *   `Title`: Requerido, máx 100 caracteres.
> *   `Description`: Requerido, mín 20 caracteres.
> *   `CategoryId`: Requerido, debe ser mayor a 0.
> *   `Priority`: Requerido, debe ser un valor válido del Enum.
>
> **Reglas de Validación:**
> *   El título no debe contener palabras ofensivas (simula una validación `Must` con una lista negra).
> *   Usa mensajes de error localizables o códigos de error claros.

---

## 📌 Tema 4: Copilot en Frontend con Angular
### Subtema: Generación de componentes, módulos y rutas de navegación con IA

**Objetivo:** Crear componentes modernos y funcionales.

#### 💬 Prompt:
> Genera un componente Standalone de Angular llamado `TicketListComponent` para el proyecto `TicketManagementSystem`.
>
> **Funcionalidad:**
> *   Mostrar una tabla de tickets con columnas: ID, Título, Estado (con badge de color), Prioridad y Acciones.
> *   Incluir paginación server-side.
>
> **Requisitos Técnicos:**
> *   Usa Angular Material o HTML semántico con clases CSS limpias (BEM o Tailwind según proyecto).
> *   Usa `Signals` para manejar el estado de la lista de tickets.
> *   Implementa `OnDestroy` para limpiar suscripciones si no usas `AsyncPipe`.
> *   Incluye un botón "Nuevo Ticket" que navegue a `/tickets/create`.

### Subtema: Creación de servicios para consumo de APIs REST y manejo de datos

**Objetivo:** Comunicación tipada y segura con el backend.

#### 💬 Prompt:
> Crea un servicio `TicketService` en Angular para comunicar con el backend .NET.
>
> **Requisitos:**
> *   Usa `HttpClient`.
> *   Define métodos para: `getTickets(params: TicketQueryParams)`, `getTicketById(id)`, `createTicket(dto)`.
> *   Todos los métodos deben retornar `Observable<ApiResponse<T>>`.
> *   Implementa un manejo de errores genérico que transforme el error del backend en un mensaje amigable para el usuario (usando un servicio de notificaciones o `throwError`).
> *   Usa interfaces tipadas para todos los DTOs (`TicketResponse`, `CreateTicketRequest`).

### Subtema: Uso de Copilot para formularios reactivos y validaciones personalizadas

**Objetivo:** Formularios robustos y validación en tiempo real.

#### 💬 Prompt:
> Crea un componente `TicketCreateComponent` que use **Reactive Forms** de Angular.
>
> **Campos del Formulario:** Título, Descripción, Categoría (select), Prioridad (radio buttons).
>
> **Validaciones:**
> *   Todos requeridos.
> *   Descripción con validador personalizado: `noGenericDescriptionValidator` (evita textos como "ayuda", "error", "no funciona" si son muy cortos).
>
> **Comportamiento:**
> *   El botón de submit debe estar deshabilitado si el form es inválido o `isSubmitting` es true.
> *   Muestra mensajes de error debajo de cada input solo si el control está `touched` y `invalid`.

---

## 📌 Tema 4: Copilot en Frontend con Angular

### Subtema: Configuración inicial de Copilot en proyectos Angular y VS Code

**Objetivo:** Preparar el entorno Angular para trabajar eficientemente con Copilot.

#### 💬 Prompt para Configuración de Proyecto:
> Configura un proyecto Angular 17+ para el frontend del `TicketManagementSystem` optimizado para trabajar con Copilot.
>
> **Estructura del proyecto:**
> - Arquitectura standalone (sin NgModules)
> - Estructura de carpetas: features/ (módulos funcionales), core/ (servicios singleton), shared/ (componentes reutilizables)
> - Configuración de paths en tsconfig para imports limpios (@app, @core, @shared)
> - ESLint + Prettier configurados
> - Husky para pre-commit hooks
>
> **Archivos de contexto para Copilot:**
> - Crear `docs/architecture.md` con decisiones de diseño
> - Crear `docs/coding-standards.md` con convenciones del equipo
> - Configurar `.copilotignore` para excluir node_modules, dist, etc.

#### 💬 Prompt para VS Code Workspace:
> Crea la configuración óptima de VS Code para desarrollo Angular con Copilot:
>
> **Extensiones recomendadas:**
> - Angular Language Service
> - Copilot y Copilot Chat
> - ESLint, Prettier
> - Angular Snippets
>
> **Settings y Tasks:**
> - Tasks para ng serve, ng test, ng lint
> - Launch configurations para debugging
> - Snippets personalizados del proyecto
> - Configuración de Copilot específica para TypeScript y HTML

---

### Subtema: Generación de pipes y directivas dinámicas según el contexto

**Objetivo:** Crear pipes y directivas reutilizables para transformaciones comunes.

#### 💬 Prompt para Pipe de Transformación:
> Crea un pipe `ticketStatus` para el proyecto Angular que transforme el enum de estado de tickets en texto legible con color.
>
> **Funcionalidad:**
> - Input: valor del enum (Open, InProgress, Closed, OnHold)
> - Output: objeto con propiedades text (versión legible), class (clase CSS para badge), icon (nombre del icono)
> - text: Versión legible ("En Progreso", "Abierto", etc.)
> - class: Clase CSS para el badge (success, warning, danger, info)
> - icon: Nombre del icono de Material Icons o Font Awesome
>
> Debe ser un pure pipe para optimización de performance.

#### 💬 Prompt para Pipe de Fecha:
> Implementa un pipe `relativeTime` que muestre fechas en formato relativo (hace 2 horas, hace 3 días, etc.).
>
> **Requisitos:**
> - Si es menos de 1 minuto: "Justo ahora"
> - Si es menos de 1 hora: "Hace X minutos"
> - Si es menos de 24 horas: "Hace X horas"
> - Si es menos de 7 días: "Hace X días"
> - Si es más de 7 días: Mostrar fecha formateada
> - Debe actualizarse automáticamente (impure pipe o uso de observables)

#### 💬 Prompt para Directiva de Permisos:
> Crea una directiva estructural `*hasPermission` que muestre/oculte elementos según los permisos del usuario.
>
> **Funcionalidad:**
> - Uso básico: `*hasPermission="'tickets.edit'"`
> - Verificar contra el servicio de autenticación actual
> - Soportar múltiples permisos con AND/OR
> - Ejemplo avanzado: `*hasPermission="['tickets.edit', 'tickets.delete']; operator: 'or'"`
> - Opcionalmente renderizar un template alternativo si no tiene permiso

#### 💬 Prompt para Directiva de Validación:
> Implementa una directiva de atributo `noGenericText` que valide que el input no contenga texto genérico como "ayuda", "error", "no funciona".
>
> **Funcionalidad:**
> - Aplicable a inputs y textareas
> - Integrar con Angular Forms (reactive y template-driven)
> - Mostrar error de validación apropiado
> - Configurable: permitir pasar lista custom de palabras prohibidas
> - Ejemplo de uso: `<input noGenericText [forbiddenWords]="['test', 'prueba']">`

---

### Subtema: Creación de interfaces y tipos TypeScript con sugerencias de Copilot

**Objetivo:** Definir tipos robustos y reutilizables para el proyecto.

#### 💬 Prompt para Interfaces de DTOs:
> Crea todas las interfaces TypeScript necesarias para el módulo de tickets del frontend.
>
> **Interfaces requeridas:**
> - `Ticket` - Representa un ticket completo con todas sus propiedades
> - `TicketListItem` - Versión simplificada para listas (menos propiedades)
> - `CreateTicketRequest` - DTO para crear tickets
> - `UpdateTicketRequest` - DTO para actualizar tickets
> - `TicketFilters` - Objeto con todos los filtros posibles
> - `PaginatedTickets` - Respuesta paginada con meta información
>
> Usa tipos específicos (enums para Status/Priority, Date para fechas, tipos opcionales con ?)

#### 💬 Prompt para Type Guards:
> Implementa type guards para validación de tipos en runtime.
>
> **Type guards a crear:**
> - `isTicket(obj: unknown): obj is Ticket` - Valida que un objeto tiene la estructura de Ticket
> - `isTicketStatus(value: string): value is TicketStatus` - Valida que es un valor válido del enum
> - `isPaginatedResponse<T>(obj: unknown): obj is PaginatedResponse<T>` - Guard genérico para respuestas paginadas
>
> Útiles para validar datos de APIs externas o localStorage.

#### 💬 Prompt para Utility Types:
> Crea tipos de utilidad específicos del proyecto para simplificar definiciones comunes.
>
> **Utility types:**
> - `ApiResponse<T>` - Envuelve respuestas de la API con success, data, message
> - `FormValue<T>` - Extrae el tipo de valor de un FormGroup
> - `PartialBy<T, K>` - Hace opcionales solo ciertas propiedades
> - `RequiredBy<T, K>` - Hace requeridas solo ciertas propiedades
> - `DeepReadonly<T>` - Hace readonly recursivamente
>
> Documenta cada tipo con JSDoc explicando su uso.

---

### Subtema: Refactorización y simplificación de componentes complejos

**Objetivo:** Mejorar legibilidad y mantenibilidad de componentes.

#### 💬 Prompt para Análisis de Componente:
> Analiza el componente `TicketDetailsComponent` e identifica oportunidades de refactorización:
>
> **Problemas a buscar:**
> - Componente con más de 300 líneas
> - Lógica de negocio en el componente (debería estar en servicio)
> - Múltiples suscripciones sin manejo adecuado
> - Métodos muy largos
> - Estado inconsistente
> - Falta de separación de concerns
>
> Propón división en componentes más pequeños y extracción de lógica a servicios.

#### 💬 Prompt para Smart/Dumb Components:
> Refactoriza `TicketListComponent` aplicando el patrón Smart/Dumb (Container/Presentational):
>
> **Separación:**
> - **Smart Component** (`TicketListContainerComponent`):
>   - Maneja llamadas a servicios
>   - Maneja estado y lógica de negocio
>   - Pasa datos a componentes presentacionales
>
> - **Dumb Components**:
>   - `TicketTableComponent`: Solo renderiza tabla, recibe datos por @Input
>   - `TicketFiltersComponent`: Emite eventos de filtro, no conoce servicios
>   - `TicketActionsComponent`: Emite eventos de acciones
>
> Los componentes presentacionales deben ser puros y fáciles de testear.

#### 💬 Prompt para Uso de Signals:
> Migra un componente existente de RxJS BehaviorSubjects a Signals de Angular 17+.
>
> **Migración:**
> - Identificar estados manejados con BehaviorSubject
> - Convertir a signals con valores iniciales
> - Usar computed() para valores derivados
> - Usar effect() solo cuando sea necesario
> - Simplificar template eliminando AsyncPipe
> - Comparar complejidad y legibilidad antes/después

---

### Subtema: Optimización del rendimiento y carga de módulos con ayuda de Copilot

**Objetivo:** Aplicar técnicas de performance optimization.

#### 💬 Prompt para Lazy Loading:
> Configura lazy loading para todas las rutas del `TicketManagementSystem`.
>
> **Módulos a cargar lazy:**
> - Tickets (con sub-rutas: list, detail, create, edit)
> - Users (con gestión de usuarios)
> - Reports (con dashboards y reportes)
> - Settings (con configuración del sistema)
>
> Implementa preloading strategy personalizada: precargar módulos frecuentes, lazy load módulos administrativos.

#### 💬 Prompt para OnPush Strategy:
> Refactoriza componentes para usar `ChangeDetectionStrategy.OnPush`:
>
> **Requisitos:**
> - Identificar componentes candidatos (presentacionales, con @Inputs inmutables)
> - Asegurar que @Inputs sean inmutables o usen Observables/Signals
> - Usar markForCheck() cuando sea necesario
> - Verificar que no hay mutaciones directas de objetos
> - Medir impacto en performance con Angular DevTools

#### 💬 Prompt para Virtual Scrolling:
> Implementa virtual scrolling en la lista de tickets usando CDK.
>
> **Funcionalidad:**
> - Renderizar solo items visibles en viewport
> - Soportar items de altura variable
> - Mantener performance con listas de 10,000+ items
> - Integrar con búsqueda y filtros
> - Preservar scroll position al navegar back

#### 💬 Prompt para Optimización de Queries:
> Optimiza las peticiones HTTP del frontend:
>
> **Estrategias:**
> - Implementar debounce en búsquedas (evitar queries en cada tecla)
> - Usar shareReplay() para compartir respuestas entre suscriptores
> - Implementar pagination con infinite scroll
> - Prefetch de datos en resolvers de rutas
> - Cancelar requests en progreso al cambiar de página

---

### Subtema: Generación automática de documentación de componentes y servicios

**Objetivo:** Documentar código Angular de forma profesional.

#### 💬 Prompt para JSDoc en TypeScript:
> Añade comentarios JSDoc completos a todos los métodos públicos del servicio `TicketService`.
>
> **Documentación requerida:**
> - Descripción del método y su propósito
> - @param con tipo y descripción de cada parámetro
> - @returns con tipo de retorno y descripción
> - @throws para errores que puede lanzar
> - @example con ejemplo de uso real
> - @deprecated si aplica

#### 💬 Prompt para Compodoc:
> Configura Compodoc para generación automática de documentación del proyecto Angular.
>
> **Configuración:**
> - Instalar y configurar Compodoc
> - Generar documentación de componentes, servicios, pipes, directivas
> - Incluir gráficos de dependencias
> - Añadir páginas custom con markdown para arquitectura
> - Configurar script npm para generar y servir docs
> - Integrar generación de docs en CI/CD

#### 💬 Prompt para Storybook:
> Configura Storybook para documentar componentes de UI de forma interactiva.
>
> **Setup:**
> - Instalar Storybook para Angular
> - Crear stories para componentes principales (botones, inputs, cards)
> - Documentar todas las variantes y estados de cada componente
> - Añadir controles interactivos para props
> - Incluir ejemplos de uso y mejores prácticas
> - Configurar deployment de Storybook en hosting estático

---

### Subtema: Aplicación de patrones de diseño y estructura de carpetas sugeridos por Copilot

**Objetivo:** Establecer arquitectura escalable y mantenible.

#### 💬 Prompt para Estructura de Proyecto:
> Diseña la estructura de carpetas óptima para un proyecto Angular enterprise como `TicketManagementSystem`.
>
> **Estructura propuesta:**
> - app/core/ (servicios singleton, guards, interceptors)
> - app/shared/ (componentes, pipes, directivas reutilizables)
> - app/features/ (módulos funcionales independientes: tickets/, users/, reports/)
> - app/layout/ (header, sidebar, footer)
> - assets/
> - environments/
> - styles/
>
> Explica el propósito de cada carpeta y qué código va en cada una.

#### 💬 Prompt para Patrón Facade:
> Implementa el patrón Facade para simplificar la interacción con múltiples servicios.
>
> **Ejemplo:**
> Crear `TicketFacadeService` que encapsule:
> - TicketService (CRUD de tickets)
> - CommentService (comentarios de tickets)
> - AttachmentService (archivos adjuntos)
> - NotificationService (notificaciones)
>
> El facade expone métodos de alto nivel como `createTicketWithAttachments()` que coordina múltiples servicios internamente.

#### 💬 Prompt para State Management:
> Evalúa si necesitas una librería de state management (NgRx, Akita, Elf) para el proyecto.
>
> **Análisis:**
> - ¿Cuánto estado compartido hay entre componentes?
> - ¿La complejidad del estado justifica la curva de aprendizaje?
> - ¿Signals de Angular 17+ son suficientes?
>
> Si decides usar NgRx, implementa un ejemplo completo para el módulo de tickets (actions, reducers, effects, selectors).

---

### Subtema: Buenas prácticas y recomendaciones para proyectos Angular asistidos por IA

**Objetivo:** Establecer estándares de calidad para código generado por IA.

#### 💬 Prompt para Checklist de Calidad:
> Crea un checklist de revisión para código Angular generado por Copilot:
>
> **Checklist Frontend:**
> - [ ] Componente usa OnPush change detection cuando es posible
> - [ ] No hay lógica de negocio en el componente (está en servicios)
> - [ ] Observables se des-suscriben correctamente
> - [ ] Formularios tienen validaciones apropiadas
> - [ ] Manejo de estados de loading y error
> - [ ] Código es accesible (ARIA labels, navegación por teclado)
> - [ ] No hay hardcoded strings (usar i18n)
> - [ ] Tests unitarios incluidos
> - [ ] Documentación JSDoc en métodos públicos
> - [ ] No hay console.log en código de producción

#### 💬 Prompt para Testing:
> Genera tests unitarios completos para el componente `TicketListComponent` usando Jasmine y Karma.
>
> **Cobertura requerida:**
> - Test de creación del componente
> - Tests para cada método público
> - Tests de interacción con servicios (usando mocks)
> - Tests de renderizado de template
> - Tests de emisión de eventos
> - Tests de casos de error
> - Coverage mínimo: 80%

#### 💬 Prompt para Accesibilidad:
> Audita el proyecto para asegurar accesibilidad (WCAG 2.1 nivel AA):
>
> **Verificar:**
> - Todos los elementos interactivos son accesibles por teclado
> - Imágenes tienen texto alternativo
> - Contraste de colores adecuado
> - Labels asociados a inputs
> - Roles ARIA apropiados
> - Anuncios de screen reader para cambios dinámicos
> - Focus management en modales y navegación
>
> Propón correcciones para problemas encontrados.

---

### Subtema: Consejos de migración asistida por IA entre versiones de Angular y TypeScript

**Objetivo:** Modernizar proyectos Angular legacy.

#### 💬 Prompt para Migración Angular 15 → 17:
> Analiza el proyecto y sugiere un plan de migración de Angular 15 a Angular 17:
>
> **Pasos de migración:**
> 1. Actualizar dependencias con `ng update`
> 2. Migrar a arquitectura standalone (eliminar NgModules)
> 3. Adoptar Signals donde reemplacen RxJS apropiadamente
> 4. Actualizar a nueva control flow syntax (if, for, switch)
> 5. Aprovechar mejoras en SSR y hydration
> 6. Actualizar sintaxis de inyección de dependencias (inject() function)
>
> Identifica breaking changes y crea plan de rollback.

#### 💬 Prompt para Modernización TypeScript:
> Refactoriza el código TypeScript para usar características modernas (TypeScript 5.0+):
>
> **Modernizaciones:**
> - Usar const type parameters
> - Aprovechar satisfies operator para type checking
> - Usar decorators de TypeScript 5
> - Actualizar enums a const enums o union types según el caso
> - Mejorar tipos con template literal types
> - Usar utility types (Partial, Required, Pick, Omit) donde simplifiquen código

#### 💬 Prompt para Deprecations:
> Identifica APIs deprecadas de Angular en el proyecto y propón alternativas:
>
> **Buscar:**
> - Uso de ReactiveForms deprecados
> - Imports de @angular/platform-browser-dynamic innecesarios
> - ComponentFactoryResolver (reemplazar por ViewContainerRef.createComponent)
> - Métodos deprecados de Router
> - Uso de ModuleWithProviders sin generic type
>
> Para cada caso, proporciona la migración específica.

---

## 📌 Tema 1: Introducción a GitHub Copilot para Desarrolladores Web (Subtemas Faltantes)

### Subtema: Qué es Copilot y cómo funciona con LLMs

**Objetivo:** Comprender los fundamentos técnicos de Copilot.

#### 💬 Prompt:
> Explica qué es GitHub Copilot y cómo funciona internamente con Large Language Models (LLMs).
>
> **Proporciona:**
> 1. **Definición técnica:** ¿Qué es Copilot exactamente? (herramienta de IA generativa, basada en modelos de lenguaje, etc.)
> 2. **Funcionamiento interno:** Describe cómo procesa el código del usuario, el contexto del proyecto y genera sugerencias
> 3. **Modelos subyacentes:** ¿Qué LLMs usa Copilot? (basado en GPT, entrenado en código público de GitHub)
> 4. **Limitaciones técnicas:** ¿Por qué no siempre genera código perfecto? (contexto limitado, sesgos del entrenamiento, etc.)
> 5. **Diferencia con otros LLMs:** Comparación con ChatGPT, Claude, etc., en el contexto de desarrollo de software

### Subtema: Integración de Copilot en VS Code y Visual Studio

**Objetivo:** Configurar Copilot correctamente en el entorno de desarrollo.

#### 💬 Prompt:
> Guía paso a paso para integrar GitHub Copilot en VS Code y Visual Studio para el desarrollo del `TicketManagementSystem`.
>
> **Para VS Code:**
> 1. **Instalación:** Cómo instalar la extensión GitHub Copilot
> 2. **Autenticación:** Configuración de la cuenta GitHub con suscripción activa
> 3. **Configuración inicial:** Ajustes recomendados en settings.json
> 4. **Extensiones complementarias:** Copilot Chat, IntelliCode, etc.
>
> **Para Visual Studio:**
> 1. **Instalación:** Cómo instalar Copilot en Visual Studio 2022
> 2. **Configuración:** Opciones específicas para .NET y C#
> 3. **Integración con Resharper:** Si aplica
>
> **Configuración del proyecto:**
> - Crear archivos `.copilotignore` para excluir carpetas
> - Configurar workspace settings para optimizar sugerencias
> - Integrar con el flujo de trabajo del equipo

### Subtema: Limitaciones y riesgos de Copilot en proyectos grandes

**Objetivo:** Identificar situaciones donde Copilot puede generar código subóptimo o inseguro.

#### 💬 Prompt:
> Actúa como un revisor de código senior. Analiza los siguientes riesgos al usar Copilot en el `TicketManagementSystem`:
>
> 1. **Seguridad:** ¿Qué problemas podría introducir Copilot al generar código de autenticación JWT? Proporciona 3 ejemplos de vulnerabilidades comunes (hardcoded secrets, tokens sin expiración, validación incorrecta).
> 2. **Rendimiento:** Identifica patrones anti-performance que Copilot podría generar en consultas EF Core (N+1, falta de `AsNoTracking`, exceso de datos cargados).
> 3. **Arquitectura:** ¿Cómo puede Copilot violar la separación de capas si no se le guía correctamente? Describe un ejemplo donde genera lógica de negocio directamente en el Controller.
>
> Para cada riesgo, proporciona:
> - Descripción del código problemático
> - Explicación de por qué es problemático
> - Estrategia de prompt mejorada que evite ese error

### Subtema: Cómo interactuar con Copilot de forma efectiva

**Objetivo:** Aprender técnicas para obtener mejores sugerencias de Copilot.

#### 💬 Prompt:
> Describe las mejores prácticas para interactuar efectivamente con GitHub Copilot en el desarrollo del `TicketManagementSystem`.
>
> **Técnicas de interacción:**
> 1. **Comentarios estratégicos:** Cómo usar comentarios para guiar la generación de código
> 2. **Contexto del proyecto:** Importancia de tener archivos abiertos y workspace configurado
> 3. **Prompts específicos:** Diferencia entre prompts vagos y específicos
> 4. **Aceptación inteligente:** Cuándo aceptar sugerencias completas vs. modificarlas
> 5. **Iteración:** Cómo refinar sugerencias mediante prompts adicionales
>
> **Ejemplos prácticos:**
> - Cómo pedir a Copilot que genere un método completo vs. solo autocompletar
> - Uso de `#file` y `#codebase` para dar contexto
> - Manejo de sugerencias incorrectas o incompletas

### Subtema: Ejemplos de productividad en backend y frontend

**Objetivo:** Demostrar el impacto de Copilot en la velocidad de desarrollo.

#### 💬 Prompt:
> Proporciona ejemplos concretos de cómo GitHub Copilot acelera el desarrollo en el `TicketManagementSystem`.
>
> **Backend (.NET):**
> 1. **Generación de entidades EF Core:** Tiempo ahorrado al crear modelos con relaciones complejas
> 2. **Controladores CRUD:** Automatización de endpoints REST estándar
> 3. **Validaciones:** Creación rápida de reglas de negocio con FluentValidation
> 4. **Manejo de errores:** Implementación de middleware y exception handling
>
> **Frontend (Angular):**
> 1. **Componentes:** Creación de componentes con formularios reactivos
> 2. **Servicios:** Generación de llamadas HTTP con manejo de errores
> 3. **Interfaces TypeScript:** Definición automática de tipos para DTOs
> 4. **Pipes y directivas:** Utilidades comunes para transformación de datos
>
> **Métricas de productividad:**
> - Porcentaje de código generado vs. escrito manualmente
> - Reducción en tiempo de implementación de features estándar
> - Mejora en consistencia del código

### Subtema: Copilot vs ChatGPT: diferencias en el flujo de desarrollo

**Objetivo:** Comprender las ventajas únicas de Copilot integrado en el IDE.

#### 💬 Prompt:
> Compara Copilot y ChatGPT en el desarrollo del `TicketManagementSystem`:
>
> **Escenario:** Necesitas implementar un sistema de notificaciones por email cuando un ticket cambia de estado.
>
> 1. **Con ChatGPT (externo):**
>    - ¿Qué información debes copiar y pegar del proyecto?
>    - ¿Cuántos pasos requiere integrar el código generado?
>    - ¿Qué contexto pierde ChatGPT entre mensajes?
>
> 2. **Con Copilot (en VS Code/VS):**
>    - ¿Qué contexto tiene automáticamente del proyecto?
>    - ¿Cómo usa los archivos abiertos y el workspace?
>    - Demuestra usando `#file` y `#codebase` para generar el servicio `EmailNotificationService`
>
> 3. Crea un flujo de trabajo híbrido donde ChatGPT ayuda en diseño arquitectónico y Copilot en implementación concreta.

### Subtema: Estrategias de adopción en equipos de desarrollo

**Objetivo:** Planificar la introducción gradual de Copilot en un equipo real.

#### 💬 Prompt:
> Diseña un plan de adopción de Copilot para un equipo de 5 desarrolladores trabajando en `TicketManagementSystem`.
>
> **Contexto del equipo:**
> - 2 desarrolladores senior (.NET), 2 mid-level (Angular), 1 junior (fullstack)
> - Proyecto en producción, sprints de 2 semanas
> - Preocupaciones: calidad del código, dependencia de IA, curva de aprendizaje
>
> **Entrega un plan que incluya:**
> 1. **Fase 1 (Sprint 1-2):** Tareas específicas donde introducir Copilot (ej: generación de tests, documentación)
> 2. **Fase 2 (Sprint 3-4):** Expansión a desarrollo de features (con checkpoints de code review)
> 3. **Fase 3 (Sprint 5+):** Adopción completa con best practices
> 4. Métricas a medir en cada fase (velocidad, bugs, calidad)
> 5. Ejercicios prácticos para cada nivel de seniority aplicados al proyecto actual

### Subtema: KPIs de éxito al usar Copilot en proyectos

**Objetivo:** Definir métricas concretas para evaluar el impacto de Copilot.

#### 💬 Prompt:
> Define un dashboard de KPIs para medir el ROI de Copilot en el desarrollo del `TicketManagementSystem`.
>
> **Métricas a trackear durante 3 meses:**
>
> 1. **Productividad:**
>    - Tiempo promedio para implementar un CRUD completo (con/sin Copilot)
>    - Líneas de código generadas vs. editadas manualmente
>    - Velocidad de resolución de bugs
>
> 2. **Calidad:**
>    - Cobertura de tests antes/después
>    - Número de code smells detectados en SonarQube
>    - Bugs introducidos en código generado vs. manual
>
> 3. **Adopción:**
>    - % de código escrito con asistencia de Copilot
>    - Satisfacción del equipo (encuesta mensual)
>    - Tiempo de onboarding de nuevos desarrolladores
>
> **Entrega:**
> - Template de Excel/Google Sheets para tracking
> - 3 user stories del backlog de `TicketManagementSystem` para medir como benchmark (ej: "Implementar filtros avanzados de búsqueda")
> - Criterios de éxito cuantitativos (ej: "Reducir tiempo de implementación de CRUD en 40%")
