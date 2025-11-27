# 🤖 Ejemplos de Prompts para TicketManagementSystem (Sesión 2)

## 📌 Tema 5: Integración Frontend-Backend con Copilot

### Subtema: Creación de endpoints en backend con .NET

**Objetivo:** Generar endpoints REST robustos y bien documentados.

#### 💬 Prompt para Endpoint de Login:
 Crea el endpoint `POST /api/auth/login` en `AuthController` para el `TicketManagementSystem`.

 **Requisitos:**
 * Recibe `LoginDto` con Email y Password
 * Valida credenciales contra base de datos usando BCrypt
 * Genera JWT token con claims (sub, email, role, exp: 1 hora)
 * Retorna `AuthResponse` con accessToken, refreshToken, expiresIn, user
 * Maneja errores: 401 Unauthorized para credenciales inválidas
 * Logging estructurado con ILogger
 * Documentación Swagger con [ProducesResponseType]

#### 💬 Prompt para Endpoint de Tickets con Paginación:
 Implementa `GET /api/tickets` con paginación y filtros avanzados.

 **Parámetros de Query:**
 * page (int, default 1)
 * pageSize (int, default 10, max 50)
 * status (string, opcional: Open, InProgress, Resolved, Closed)
 * priority (string, opcional: Low, Medium, High, Critical)
 * search (string, opcional: buscar en title y description)
 * assignedTo (int, opcional: id del usuario asignado)

 **Respuesta:**
 * PagedResponse<TicketDto con data, totalCount, page, pageSize
 * Include Creator y AssignedUser
 * AsNoTracking para performance
 * Logging del tiempo de ejecución

### Subtema: Generación de servicios de consumo en frontend

**Objetivo:** Crear servicios Angular tipados para consumir APIs REST.

#### 💬 Prompt para AuthService:
 Crea `AuthService` en Angular para manejar autenticación JWT.

 **Funcionalidades:**
 * login(credentials: LoginDto): Observable<AuthResponse
 * logout(): void (limpiar localStorage)
 * refreshToken(): Observable<AuthResponse
 * isAuthenticated(): boolean
 * getCurrentUser(): User | null
 * BehaviorSubject para estado de autenticación
 * Guardar tokens en localStorage
 * Auto-refresh cuando token expira

#### 💬 Prompt para TicketService con Interceptors:
 Implementa `TicketService` con manejo automático de JWT y errores.

 **Métodos:**
 * getTickets(params): Observable<PagedResponse<Ticket
 * getTicket(id): Observable<Ticket
 * createTicket(dto): Observable<Ticket
 * updateTicket(id, dto): Observable<Ticket
 * deleteTicket(id): Observable<void

 **Características:**
 * HttpClient con baseUrl de environment
 * Interceptor automático para Authorization header
 * Manejo de errores centralizado (401 → redirect login)
 * Retry automático para requests fallidas
 * Loading states con BehaviorSubject

### Subtema: Autenticación y autorización entre frontend y backend

**Objetivo:** Implementar sistema completo de auth con JWT.

#### 💬 Prompt para JWT Authentication Middleware:
 Crea `JwtAuthenticationMiddleware` para validar tokens JWT en .NET.

 **Funcionalidad:**
 * Extraer token del header Authorization
 * Validar token con JwtSecurityTokenHandler
 * Setear HttpContext.User con claims
 * Manejar tokens expirados (401)
 * Logging de requests autenticados
 * Configuración desde IOptions<JwtSettings

#### 💬 Prompt para Role-Based Authorization:
 Implementa autorización basada en roles para el sistema de tickets.

 **Requisitos:**
 * [Authorize(Roles = "Admin")] para endpoints administrativos
 * [Authorize(Roles = "Agent,Admin")] para gestión de tickets
 * Custom AuthorizationHandler para permisos granulares
 * Validación de ownership (solo autor puede editar su ticket)
 * 403 Forbidden para accesos no autorizados
 * Logging de intentos de acceso no autorizado

### Subtema: Uso de JWT con ayuda de Copilot

**Objetivo:** Generar configuración y manejo de JWT de forma asistida.

#### 💬 Prompt para JWT Configuration:
 Configura JWT authentication en `Program.cs` para .NET 8.

 **Configuración requerida:**
 * AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 * Configurar TokenValidationParameters
 * Validar Issuer, Audience, Lifetime
 * Mapear claims a identity
 * Eventos para logging de tokens inválidos
 * Configuración desde appsettings.json

#### 💬 Prompt para Refresh Token Implementation:
 Implementa sistema de refresh tokens para mantener sesiones activas.

 **Funcionalidad:**
 * Generar refresh token único por usuario
 * Almacenar hash en base de datos
 * Endpoint POST /api/auth/refresh
 * Validar refresh token y generar nuevo access token
 * Expiración de refresh tokens (7 días)
 * Invalidar refresh tokens en logout

### Subtema: Ejemplo práctico - login completo con Copilot

**Objetivo:** Crear flujo completo de login frontend-backend.

#### 💬 Prompt para Login Component:
 Crea `LoginComponent` en Angular con formulario reactivo.

 **Características:**
 * FormGroup con email y password
 * Validaciones: email pattern, password required
 * Loading state durante login
 * Error handling con mensajes específicos
 * Redirect a dashboard después de login exitoso
 * Persistencia de returnUrl para deep linking
 * Diseño responsive con Bootstrap

#### 💬 Prompt para Login Backend:
 Implementa lógica completa de login en `AuthService` (.NET).

 **Pasos:**
 * Buscar usuario por email
 * Verificar password con BCrypt
 * Generar access token y refresh token
 * Actualizar last login date
 * Logging de login exitoso/fallido
 * Retornar AuthResponse con tokens y user info

### Subtema: Generación de interceptores para APIs en frontend

**Objetivo:** Crear interceptores HTTP para cross-cutting concerns.

#### 💬 Prompt para Auth Interceptor:
 Crea `AuthInterceptor` para añadir automáticamente JWT a requests.

 **Funcionalidad:**
 * Implementar HttpInterceptor
 * Añadir Authorization header si hay token
 * Manejar 401 responses con auto-refresh
 * Skip interceptor para login/refresh endpoints
 * Logging de requests interceptados

#### 💬 Prompt para Error Interceptor:
 Implementa `ErrorInterceptor` para manejo global de errores HTTP.

 **Manejo por status:**
 * 400: Mostrar errores de validación
 * 401: Redirect a login, limpiar auth state
 * 403: Mostrar mensaje de permisos insuficientes
 * 404: Mostrar página 404 o mensaje
 * 500: Mostrar error genérico, logging detallado
 * Network errors: Retry automático

### Subtema: Testing de endpoints generados con Copilot

**Objetivo:** Generar tests para validar endpoints REST.

#### 💬 Prompt para Integration Tests:
 Crea tests de integración para `TicketsController` usando WebApplicationFactory.

 **Test Cases:**
 * GET /api/tickets returns paginated results
 * POST /api/tickets creates ticket successfully
 * PUT /api/tickets/{id} updates existing ticket
 * DELETE /api/tickets/{id} soft deletes ticket
 * GET /api/tickets/{id} returns 404 for non-existent ticket
 * Authentication required for protected endpoints

#### 💬 Prompt para Unit Tests con Mocks:
 Genera unit tests para `TicketService` con xUnit y Moq.

 **Tests a crear:**
 * CreateAsync_ValidData_ReturnsCreatedTicket
 * CreateAsync_InvalidUser_ThrowsNotFoundException
 * UpdateAsync_ValidTransition_UpdatesSuccessfully
 * UpdateAsync_InvalidTransition_ThrowsBusinessException
 * AssignToUser_UserNotAgent_ThrowsUnauthorizedException

### Subtema: Documentación automática de APIs consumidas

**Objetivo:** Generar documentación completa de APIs.

#### 💬 Prompt para Swagger Configuration:
 Configura Swagger/OpenAPI completo para el proyecto .NET.

 **Configuración:**
 * AddSwaggerGen con título, versión, descripción
 * Incluir XML comments de controladores
 * Configurar autenticación JWT en Swagger UI
 * Añadir ejemplos de request/response
 * Agrupar endpoints por tags
 * Descripciones detalladas con markdown

#### 💬 Prompt para API Documentation:
 Genera documentación completa de la API de tickets.

 **Secciones:**
 * Autenticación (login, refresh, logout)
 * Gestión de tickets (CRUD completo)
 * Comentarios y historial
 * Paginación y filtros
 * Códigos de error y ProblemDetails
 * Ejemplos de uso con curl

### Subtema: Refactorización de integraciones legacy

**Objetivo:** Modernizar código de integración existente.

#### 💬 Prompt para Legacy API Client:
 Refactoriza un cliente API legacy para usar HttpClient moderno.

 **Mejoras:**
 * Reemplazar WebClient/HttpWebRequest con HttpClient
 * Añadir CancellationToken a todos los métodos
 * Implementar Polly para retry y circuit breaker
 * Añadir logging estructurado
 * Manejo de errores consistente
 * Configuración desde IOptions

#### 💬 Prompt para Legacy Authentication:
 Moderniza sistema de autenticación legacy a JWT.

 **Migración:**
 * Reemplazar cookies/sessions con JWT
 * Implementar refresh tokens
 * Añadir claims para roles y permisos
 * Mantener compatibilidad durante transición
 * Logging de cambios de autenticación

### Subtema: Buenas prácticas en proyectos fullstack

**Objetivo:** Establecer estándares para desarrollo fullstack.

#### 💬 Prompt para API Design Standards:
 Define estándares de diseño de API para el proyecto.

 **Convenciones:**
 * RESTful resource naming
 * HTTP status codes apropiados
 * Consistent error response format (ProblemDetails)
 * Versioning strategy (URL versioning)
 * Pagination standards
 * Filtering and sorting conventions

#### 💬 Prompt para Fullstack Architecture:
 Establece arquitectura recomendada para aplicaciones fullstack.

 **Capas:**
 * Frontend: Componentes, Servicios, Guards, Interceptors
 * Backend: Controllers, Services, Repositories, DTOs
 * Comunicación: REST API con OpenAPI spec
 * Autenticación: JWT con refresh tokens
 * Testing: Unit, Integration, E2E
 * CI/CD: Build, test, deploy automatizado

---

## 📌 Tema 6: Testing Automatizado con GitHub Copilot

### Subtema: Introducción al testing y su importancia en el desarrollo moderno

**Objetivo:** Comprender fundamentos del testing automatizado.

#### 💬 Prompt para Testing Fundamentals:
 Explica la importancia del testing automatizado en desarrollo moderno.

 **Aspectos a cubrir:**
 * Beneficios: calidad, confianza, velocidad de desarrollo
 * Tipos de tests: unitarios, integración, E2E
 * Pirámide de testing
 * TDD vs BDD
 * Métricas de calidad: cobertura, defectos encontrados
 * ROI del testing automatizado

#### 💬 Prompt para Testing Strategy:
 Define estrategia de testing para `TicketManagementSystem`.

 **Plan de testing:**
 * Unit tests: lógica de negocio, validaciones
 * Integration tests: APIs, base de datos
 * E2E tests: flujos completos de usuario
 * Coverage mínimo: 80%
 * Tests en CI/CD pipeline
 * Testing de performance y seguridad

### Subtema: Cómo Copilot puede ayudarte a iniciarte en las pruebas automatizadas

**Objetivo:** Usar Copilot para aprender y aplicar testing.

#### 💬 Prompt para First Unit Test:
 Crea tu primer unit test para el método `TicketService.CreateAsync`.

 **Guía paso a paso:**
 * Instalar xUnit y FluentAssertions
 * Crear TestFixture con mocks (Moq)
 * Arrange: setup datos de prueba
 * Act: ejecutar método
 * Assert: verificar resultado esperado
 * Ejecutar test y verificar que pasa

#### 💬 Prompt para Test Discovery:
 Usa Copilot para identificar qué métodos necesitan tests prioritariamente.

 **Análisis:**
 * Métodos con lógica compleja
 * Funciones críticas para el negocio
 * Código con alto riesgo de bugs
 * Métodos que han tenido bugs anteriormente
 * Nueva funcionalidad sin tests

### Subtema: Generación de pruebas unitarias en C# con xUnit

**Objetivo:** Crear tests unitarios completos para backend.

#### 💬 Prompt para Service Unit Tests:
 Genera suite completa de unit tests para `TicketService`.

 **Tests requeridos:**
 * Constructor_ValidDependencies_CreatesInstance
 * CreateAsync_ValidData_CreatesTicketSuccessfully
 * CreateAsync_UserNotFound_ThrowsNotFoundException
 * CreateAsync_InvalidPriority_ThrowsValidationException
 * UpdateAsync_ValidUpdate_UpdatesTicket
 * UpdateAsync_InvalidTransition_ThrowsBusinessException
 * AssignToUser_UserNotAgent_ThrowsUnauthorizedException

#### 💬 Prompt para Repository Unit Tests:
 Crea unit tests para `TicketRepository` con EF Core in-memory.

 **Tests:**
 * GetAllAsync_WithFilters_ReturnsFilteredResults
 * GetByIdAsync_ExistingId_ReturnsTicket
 * GetByIdAsync_NonExistingId_ReturnsNull
 * AddAsync_ValidTicket_AddsToDatabase
 * UpdateAsync_ValidTicket_UpdatesInDatabase
 * DeleteAsync_ValidId_SoftDeletesTicket

### Subtema: Creación de tests en Angular con Jasmine y Karma guiados por prompts

**Objetivo:** Testing del frontend con Angular.

#### 💬 Prompt para Component Unit Tests:
 Genera tests unitarios para `TicketListComponent`.

 **Tests:**
 * should create component
 * should load tickets on init
 * should filter tickets by search term
 * should change page when pagination clicked
 * should navigate to create ticket
 * should handle loading states
 * should handle error states

#### 💬 Prompt para Service Unit Tests:
 Crea unit tests para `TicketService` en Angular.

 **Tests:**
 * should get tickets with params
 * should get ticket by id
 * should create ticket successfully
 * should update ticket
 * should delete ticket
 * should handle http errors
 * should retry on network failure

### Subtema: Uso de Copilot para generar mocks y datos simulados

**Objetivo:** Crear datos de prueba realistas.

#### 💬 Prompt para Test Data Builder:
 Crea un builder para generar datos de prueba de tickets.

 **Funcionalidad:**
 * TicketBuilder con métodos fluentes
 * WithTitle(), WithDescription(), WithStatus(), etc.
 * Build() que retorna Ticket
 * Builders anidados para User, Comments
 * Datos realistas vs datos edge case

#### 💬 Prompt para Mock Data Generator:
 Genera datos simulados para testing con Faker/Bogus.

 **Implementación:**
 * Instalar Bogus NuGet package
 * Crear TicketFaker con reglas
 * Generar listas de tickets con variedad
 * Datos consistentes para tests repetibles
 * Edge cases: títulos largos, descripciones vacías, etc.

### Subtema: Testing de controladores, servicios y APIs REST

**Objetivo:** Tests de integración para APIs.

#### 💬 Prompt para Controller Integration Tests:
 Crea tests de integración para `TicketsController`.

 **Usando WebApplicationFactory:**
 * TestServer con base de datos in-memory
 * Autenticación con JWT válido
 * Tests de endpoints CRUD
 * Validación de responses HTTP
 * Tests de autorización (401, 403)
 * Performance assertions

#### 💬 Prompt para API Contract Tests:
 Implementa contract tests para validar API consistency.

 **Tests:**
 * Response schema validation
 * Required fields presence
 * Data types correctness
 * Error response format
 * Pagination structure
 * Hypermedia links

### Subtema: Ejemplo práctico - mejora de cobertura con Copilot

**Objetivo:** Aumentar cobertura de tests usando IA.

#### 💬 Prompt para Coverage Analysis:
 Analiza cobertura actual y identifica gaps.

 **Pasos:**
 * Ejecutar report de cobertura
 * Identificar métodos sin tests
 * Priorizar por complejidad/riesgo
 * Generar tests para uncovered code
 * Verificar mejora de cobertura

#### 💬 Prompt para Missing Test Generation:
 Genera tests para código no cubierto en `TicketValidationService`.

 **Métodos a testear:**
 * ValidateTicketCreation
 * ValidateStatusTransition
 * ValidateUserPermissions
 * ValidateBusinessRules
 * Edge cases y error conditions

### Subtema: Introducción a pruebas end-to-end (E2E) con IA

**Objetivo:** Automatizar tests de flujos completos.

#### 💬 Prompt para E2E Setup:
 Configura Playwright para tests E2E en el proyecto.

 **Configuración:**
 * Instalar Playwright
 * Configurar browsers (Chrome, Firefox)
 * Base URL y environment setup
 * Page objects pattern
 * Screenshots on failure
 * CI/CD integration

#### 💬 Prompt para First E2E Test:
 Crea test E2E para flujo completo de creación de ticket.

 **Escenario:**
 * Login como user
 * Navigate to tickets page
 * Click "New Ticket"
 * Fill form with valid data
 * Submit form
 * Verify ticket appears in list
 * Verify success message

### Subtema: Generación de scripts de usuario en Cypress, Playwright o e2

**Objetivo:** Tests que simulan comportamiento real de usuario.

#### 💬 Prompt para User Journey Test:
 Crea test E2E para journey completo de gestión de tickets.

 **Flujo:**
 * Login → Dashboard
 * Crear ticket → Ver en lista
 * Asignar ticket a agente
 * Agente login → Ver tickets asignados
 * Actualizar status → Ver historial
 * Añadir comentario → Ver en detalle
 * Resolver ticket → Ver métricas

#### 💬 Prompt para Critical Path Tests:
 Implementa tests para caminos críticos de la aplicación.

 **Tests:**
 * User registration and first login
 * Password reset flow
 * Ticket creation and assignment
 * SLA breach notification
 * Admin user management
 * System backup and restore

### Subtema: Uso de lenguaje Gherkin para describir escenarios Given-When-Then

**Objetivo:** Tests legibles por negocio usando BDD.

#### 💬 Prompt para Feature Files:
 Crea archivos .feature con escenarios Gherkin para tickets.

 **Ejemplo:**
 ```
 Feature: Ticket Management
   Scenario: Create new ticket
     Given user is logged in
     When user creates ticket with valid data
     Then ticket appears in the list
     And notification is sent to assigned user
 ```

 **Escenarios:**
 * Ticket creation and validation
 * Status transitions
 * Assignment workflow
 * SLA monitoring
 * Reporting and analytics

#### 💬 Prompt para Step Definitions:
 Implementa step definitions para escenarios Gherkin.

 **Bindings:**
 * Given user is logged in
 * When user performs action
 * Then verify expected result
 * Background steps for setup
 * Data tables for multiple test cases

### Subtema: Refactorización y modernización de tests legacy con ayuda de IA

**Objetivo:** Mejorar tests existentes.

#### 💬 Prompt para Legacy Test Modernization:
 Refactoriza tests legacy para usar mejores prácticas.

 **Mejoras:**
 * Reemplazar setup/teardown con xUnit fixtures
 * Usar FluentAssertions en lugar de Assert
 * Implementar builder pattern para test data
 * Añadir test categories y traits
 * Mejorar nombres descriptivos
 * Añadir parallel execution

#### 💬 Prompt para Test Code Quality:
 Mejora calidad del código de tests existente.

 **Refactorizaciones:**
 * Extraer métodos comunes a base classes
 * Implementar Page Object Model para UI tests
 * Usar TestContext para compartir state
 * Añadir logging y debugging helpers
 * Implementar retry mechanisms

### Subtema: Validación y revisión de tests generados automáticamente

**Objetivo:** Asegurar calidad de tests generados por IA.

#### 💬 Prompt para Test Review Checklist:
 Crea checklist para revisar tests generados por Copilot.

 **Validaciones:**
 * [ ] Test name describes behavior clearly
 * [ ] Arrange-Act-Assert structure followed
 * [ ] Appropriate assertions used
 * [ ] Edge cases covered
 * [ ] Mocks configured correctly
 * [ ] No flaky tests (non-deterministic)
 * [ ] Performance acceptable
 * [ ] Maintainable and readable

#### 💬 Prompt para Test Quality Metrics:
 Define métricas para evaluar calidad de test suite.

 **Métricas:**
 * Coverage: line, branch, method
 * Execution time
 * Flakiness rate
 * Maintenance effort
 * False positive/negative rates
 * ROI (bugs found vs development time)

### Subtema: Buenas prácticas de testing asistido por Copilot y recomendaciones finales

**Objetivo:** Establecer estándares para testing con IA.

#### 💬 Prompt para Testing Guidelines:
 Define guías para usar Copilot efectivamente en testing.

 **Recomendaciones:**
 * Usar Copilot para generar boilerplate
 * Revisar y entender lógica de tests generados
 * Combinar con conocimiento del dominio
 * Mantener tests legibles y mantenibles
 * Documentar casos edge case complejos
 * Usar Copilot para refactorizar tests existentes

#### 💬 Prompt para Testing Culture:
 Establece cultura de testing en el equipo.

 **Prácticas:**
 * Tests como documentación viva
 * Shift-left testing approach
 * Pair testing con Copilot
 * Continuous testing en CI/CD
 * Test reviews y feedback loops
 * Métricas y dashboards de calidad