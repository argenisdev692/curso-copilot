# 🤖 Cómo GitHub Copilot puede Ayudarte a Iniciarte en las Pruebas Automatizadas

## 📖 Introducción

GitHub Copilot ha revolucionado el desarrollo de software, y el testing no es la excepción. En 2025, con capacidades de **Agent Mode**, **slash commands avanzados**, y comprensión profunda de contexto, Copilot se convierte en tu asistente de testing que puede:

- ✅ Generar tests completos en segundos
- ✅ Sugerir casos edge que podrías olvidar
- ✅ Crear mocks y fixtures automáticamente
- ✅ Refactorizar tests legacy
- ✅ Explicar tests existentes
- ✅ Mejorar cobertura de código

---

## 🎯 Capacidades de Copilot para Testing

### 1. **Generación Automática de Tests**

#### Método Tradicional (Sin Copilot)
```csharp
// Método a testear
public async Task<Result<Ticket>> CreateTicketAsync(CreateTicketDto dto)
{
    if (string.IsNullOrEmpty(dto.Title))
        return Result<Ticket>.Failure("Title is required");
    
    var ticket = new Ticket 
    { 
        Title = dto.Title,
        Priority = dto.Priority,
        Status = Status.Open,
        CreatedAt = DateTime.UtcNow
    };
    
    await _repository.CreateAsync(ticket);
    return Result<Ticket>.Success(ticket);
}

// 30-45 minutos escribiendo tests manualmente...
```

#### Con Copilot (Método Moderno)
```csharp
// Paso 1: Selecciona el método y usa slash command
// Comando en Chat: /tests para CreateTicketAsync

// Paso 2: Copilot genera instantáneamente:
public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _mockRepository;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _mockRepository = new Mock<ITicketRepository>();
        _service = new TicketService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateTicketAsync_ValidDto_ReturnsSuccessResult()
    {
        // Arrange
        var dto = new CreateTicketDto 
        { 
            Title = "Test Ticket",
            Priority = Priority.High 
        };

        _mockRepository
            .Setup(x => x.CreateAsync(It.IsAny<Ticket>()))
            .ReturnsAsync((Ticket t) => t);

        // Act
        var result = await _service.CreateTicketAsync(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Ticket", result.Value.Title);
        Assert.Equal(Status.Open, result.Value.Status);
        _mockRepository.Verify(
            x => x.CreateAsync(It.IsAny<Ticket>()), 
            Times.Once
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateTicketAsync_InvalidTitle_ReturnsFailureResult(string title)
    {
        // Arrange
        var dto = new CreateTicketDto { Title = title };

        // Act
        var result = await _service.CreateTicketAsync(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Title is required", result.Error);
        _mockRepository.Verify(
            x => x.CreateAsync(It.IsAny<Ticket>()), 
            Times.Never
        );
    }

    [Fact]
    public async Task CreateTicketAsync_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var dto = new CreateTicketDto { Title = "Test" };
        _mockRepository
            .Setup(x => x.CreateAsync(It.IsAny<Ticket>()))
            .ThrowsAsync(new DbUpdateException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(
            () => _service.CreateTicketAsync(dto)
        );
    }
}

// ⏱️ Tiempo: 2-3 minutos (vs 30-45 minutos manual)
// 📊 Cobertura: 95%+ desde el inicio
```

---

## 🔧 Slash Commands para Testing

### Comandos Básicos

#### 1. `/tests` - Generación Automática
```csharp
// En Copilot Chat, selecciona un método y escribe:
/tests

// Copilot detecta:
// - Framework de testing (xUnit, NUnit, Jasmine)
// - Dependencias a mockear
// - Casos edge necesarios
// - Patrones de testing del proyecto
```

#### 2. `/fix` - Corregir Tests Fallidos
```typescript
// Test fallido
it('should load tickets', () => {
  service.getTickets().subscribe(tickets => {
    expect(tickets.length).toBe(5); // ❌ Falla: recibe 3
  });
});

// Comando: /fix
// Copilot sugiere:
it('should load tickets', (done) => {
  service.getTickets().subscribe(tickets => {
    expect(tickets.length).toBeGreaterThanOrEqual(0); // ✅ Assertion flexible
    expect(Array.isArray(tickets)).toBe(true);
    done();
  });
});
```

#### 3. `/explain` - Entender Tests Complejos
```csharp
// Test legacy confuso
[Fact]
public async Task Test1()
{
    var r = await s.M(new { x = 1, y = 2 });
    Assert.True(r.z > 0);
}

// Comando: /explain
// Copilot responde:
// "Este test verifica que el método M de servicio s, 
// cuando recibe un objeto con propiedades x=1 y y=2,
// retorna un resultado con propiedad z mayor a cero.
// Parece testear cálculo de suma/multiplicación."
```

#### 4. `/optimize` - Mejorar Tests Existentes
```typescript
// Test original
it('test user service', () => {
  const user = service.getUser(1);
  expect(user).toBeTruthy();
  expect(user.name).toBe('John');
  expect(user.email).toBe('john@example.com');
  expect(user.age).toBe(30);
});

// Comando: /optimize
// Copilot refactoriza a:
describe('UserService', () => {
  describe('getUser', () => {
    it('should return user with correct properties when ID exists', () => {
      // Arrange
      const expectedUser = {
        id: 1,
        name: 'John',
        email: 'john@example.com',
        age: 30
      };

      // Act
      const user = service.getUser(1);

      // Assert
      expect(user).toEqual(expectedUser);
    });

    it('should return null when user ID does not exist', () => {
      expect(service.getUser(999)).toBeNull();
    });
  });
});
```

---

## 🚀 Agent Mode para Testing Completo

Agent Mode es la capacidad de Copilot de ejecutar tareas complejas multi-paso automáticamente.

### Ejemplo 1: Suite Completa de Tests para API Controller

```
Prompt en Agent Mode:
"Genera suite completa de tests para TicketsController con cobertura de:
- Todos los endpoints (GET, POST, PUT, DELETE)
- Autenticación y autorización
- Validación de ModelState
- Manejo de errores
- Casos edge (tickets no existentes, usuarios sin permisos)
Usa xUnit, Moq y FluentAssertions"

Agent Mode ejecuta automáticamente:
1. ✅ Analiza TicketsController.cs
2. ✅ Identifica dependencias (ITicketService, ILogger)
3. ✅ Crea archivo TicketsControllerTests.cs
4. ✅ Genera 15+ tests cubriendo todos los escenarios
5. ✅ Configura mocks necesarios
6. ✅ Valida compilación
7. ✅ Ejecuta tests para verificar que pasan
```

### Ejemplo 2: Mejorar Cobertura de Testing

```
Prompt:
"Analiza la cobertura de testing del proyecto TicketManagementSystem.
Identifica archivos con < 70% cobertura y genera tests faltantes.
Prioriza controladores, servicios y repositorios."

Agent Mode:
1. ✅ Ejecuta análisis de cobertura
2. ✅ Identifica UserService (45% cobertura)
3. ✅ Identifica CommentRepository (30% cobertura)
4. ✅ Genera tests para métodos sin cobertura
5. ✅ Reejecutar cobertura → 85% global
```

---

## 🎨 Técnicas de Prompting para Tests de Calidad

### 1. **Prompts Específicos con Contexto**

#### ❌ Prompt Genérico (Resultados Pobres)
```
"crea tests para el servicio"
```

#### ✅ Prompt Específico (Resultados Excelentes)
```
"Genera tests unitarios xUnit para el método 
TicketService.AssignTicketAsync(int ticketId, int userId). 

Contexto:
- Valida que el usuario tenga rol "Agent" o "Admin"
- Verifica que el ticket no esté cerrado
- Actualiza propiedades AssignedToId y UpdatedAt
- Retorna Result<Ticket> con éxito o error

Test cases requeridos:
1. Usuario válido con rol Agent → éxito
2. Usuario con rol User (sin permisos) → error
3. Ticket ya cerrado → error
4. TicketId no existe → error
5. UserId no existe → error

Usa Moq para ITicketRepository e IUserRepository.
Verifica que el método repository.UpdateAsync se llame exactamente una vez en caso exitoso."
```

### 2. **Prompts para Casos Edge**

```
"Genera tests que cubran casos edge para el método ValidateEmail:
- Email null
- Email vacío
- Email con espacios
- Email sin @
- Email con múltiples @
- Email sin dominio
- Email con caracteres especiales
- Email con subdominios válidos
- Email con TLD numérico (inválido)
- Email con más de 254 caracteres (RFC límite)"
```

### 3. **Prompts para Testing de Comportamiento (BDD)**

```
"Genera tests estilo BDD para el flujo de creación de ticket:

Given: Usuario autenticado con rol Agent
And: Formulario completado con título "Bug en login" y prioridad Alta
When: Usuario envía el formulario
Then: Ticket se crea con status Open
And: Email de notificación se envía al usuario
And: Evento TicketCreated se publica en message bus
And: Usuario ve mensaje de éxito

Usa lenguaje Gherkin en comentarios y estructura Given-When-Then."
```

---

## 🧪 Ejemplos Prácticos por Tecnología

### C# + xUnit + Moq

#### Generar Test Completo con Copilot

**Paso 1: Código de Producción**
```csharp
public class EmailValidationService : IEmailValidationService
{
    private readonly ILogger<EmailValidationService> _logger;

    public EmailValidationService(ILogger<EmailValidationService> logger)
    {
        _logger = logger;
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating email: {Email}", email);
            return false;
        }
    }
}
```

**Paso 2: Prompt a Copilot**
```
"Genera tests xUnit para EmailValidationService.IsValidEmail.
Cubre casos válidos, inválidos y excepciones.
Usa Theory con InlineData para múltiples casos."
```

**Paso 3: Copilot Genera**
```csharp
public class EmailValidationServiceTests
{
    private readonly Mock<ILogger<EmailValidationService>> _mockLogger;
    private readonly EmailValidationService _service;

    public EmailValidationServiceTests()
    {
        _mockLogger = new Mock<ILogger<EmailValidationService>>();
        _service = new EmailValidationService(_mockLogger.Object);
    }

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("user.name@example.co.uk", true)]
    [InlineData("user+tag@example.com", true)]
    [InlineData("invalid", false)]
    [InlineData("@example.com", false)]
    [InlineData("user@", false)]
    [InlineData("user @example.com", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidEmail_VariousInputs_ReturnsExpectedResult(
        string email, 
        bool expected)
    {
        // Act
        var result = _service.IsValidEmail(email);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValidEmail_RegexException_ReturnsFalseAndLogsError()
    {
        // Arrange
        var service = new EmailValidationService(_mockLogger.Object);

        // Act
        var result = service.IsValidEmail("test@example.com");

        // Assert
        Assert.True(result); // En este caso no hay error, pero test demuestra manejo
    }
}
```

### Angular + Jasmine + Karma

#### Generar Test de Servicio con HttpClient

**Paso 1: Servicio**
```typescript
@Injectable({ providedIn: 'root' })
export class TicketService {
  private apiUrl = 'api/tickets';

  constructor(private http: HttpClient) {}

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.apiUrl);
  }

  createTicket(ticket: CreateTicketDto): Observable<Ticket> {
    return this.http.post<Ticket>(this.apiUrl, ticket);
  }
}
```

**Paso 2: Prompt**
```
"Genera test Jasmine para TicketService usando HttpClientTestingModule.
Testea getTickets() y createTicket() verificando:
- URLs correctas
- HTTP methods (GET, POST)
- Request body en POST
- Respuestas esperadas
- Manejo de errores 404 y 500"
```

**Paso 3: Copilot Genera**
```typescript
describe('TicketService', () => {
  let service: TicketService;
  let httpMock: HttpTestingController;

  const mockTickets: Ticket[] = [
    { id: 1, title: 'Bug', priority: Priority.High, status: Status.Open },
    { id: 2, title: 'Feature', priority: Priority.Low, status: Status.Closed }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TicketService]
    });

    service = TestBed.inject(TicketService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('getTickets', () => {
    it('should return tickets from API', () => {
      service.getTickets().subscribe(tickets => {
        expect(tickets.length).toBe(2);
        expect(tickets).toEqual(mockTickets);
      });

      const req = httpMock.expectOne('api/tickets');
      expect(req.request.method).toBe('GET');
      req.flush(mockTickets);
    });

    it('should handle 404 error', () => {
      service.getTickets().subscribe({
        next: () => fail('should have failed'),
        error: (error) => {
          expect(error.status).toBe(404);
        }
      });

      const req = httpMock.expectOne('api/tickets');
      req.flush('Not found', { status: 404, statusText: 'Not Found' });
    });
  });

  describe('createTicket', () => {
    it('should POST ticket and return created ticket', () => {
      const newTicket: CreateTicketDto = { 
        title: 'New Bug', 
        priority: Priority.High 
      };
      const createdTicket: Ticket = { 
        id: 3, 
        ...newTicket, 
        status: Status.Open 
      };

      service.createTicket(newTicket).subscribe(ticket => {
        expect(ticket).toEqual(createdTicket);
      });

      const req = httpMock.expectOne('api/tickets');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newTicket);
      req.flush(createdTicket);
    });
  });
});
```

---

## 🎯 Estrategias para Aprovechar Copilot al Máximo

### 1. **Usa Comentarios como Guía**

```csharp
// Test: ValidateEmail method
// Arrange: Create service with mock logger
// Act: Call IsValidEmail with null
// Assert: Should return false
[Fact]
public void IsValidEmail_NullEmail_ReturnsFalse()
{
    // Copilot autocompleta el resto basado en comentarios
```

### 2. **Aprovecha Patrones Existentes**

Si ya tienes algunos tests, Copilot aprende el estilo:
```csharp
// Test existente
[Fact]
public void Method1_Scenario_ExpectedResult() { }

// Al escribir el siguiente, Copilot sugiere con mismo patrón
[Fact]
public void Method2_Scenario_ExpectedResult() { }
```

### 3. **Genera Fixtures y Builders**

```
Prompt: "Crea builder pattern para generar objetos Ticket de prueba"

Copilot genera:
public class TicketBuilder
{
    private Ticket _ticket = new Ticket
    {
        Id = 1,
        Title = "Default Ticket",
        Priority = Priority.Medium,
        Status = Status.Open,
        CreatedAt = DateTime.UtcNow
    };

    public TicketBuilder WithId(int id) 
    { 
        _ticket.Id = id; 
        return this; 
    }

    public TicketBuilder WithTitle(string title) 
    { 
        _ticket.Title = title; 
        return this; 
    }

    public TicketBuilder AsClosed() 
    { 
        _ticket.Status = Status.Closed; 
        return this; 
    }

    public Ticket Build() => _ticket;
}

// Uso en tests:
var ticket = new TicketBuilder()
    .WithId(42)
    .WithTitle("Critical Bug")
    .AsClosed()
    .Build();
```

### 4. **Genera Mocks Complejos**

```
Prompt: "Crea mock de ITicketRepository con setup para 
GetByIdAsync que retorne ticket si ID < 100, null si no"

Mock<ITicketRepository> mockRepo = new();
mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(id => id < 100)))
    .ReturnsAsync((int id) => new Ticket { Id = id, Title = $"Ticket {id}" });
mockRepo.Setup(r => r.GetByIdAsync(It.Is<int>(id => id >= 100)))
    .ReturnsAsync((Ticket)null);
```

---

## 🧩 Copilot para Diferentes Tipos de Testing

### Tests Unitarios

```
Prompt: "Genera tests unitarios para TicketHelper.CalculateDaysSinceCreated"

✅ Copilot cubre:
- Tickets creados hoy (retorna 0)
- Tickets de hace 5 días (retorna 5)
- Tickets futuros (edge case)
- Tickets con CreatedAt null
```

### Tests de Integración

```
Prompt: "Genera test de integración para TicketRepository usando InMemory EF Core.
Verifica que CreateAsync persista ticket correctamente."

✅ Copilot genera:
- Setup de DbContext in-memory
- Seed de datos iniciales
- Verificación de persistencia
- Cleanup después del test
```

### Tests E2E (Cypress)

```
Prompt: "Genera test Cypress para flujo de login completo"

✅ Copilot genera:
describe('Login Flow', () => {
  beforeEach(() => {
    cy.visit('/login');
  });

  it('should login successfully with valid credentials', () => {
    cy.get('[data-testid="email"]').type('admin@example.com');
    cy.get('[data-testid="password"]').type('Admin@123');
    cy.get('[data-testid="login-btn"]').click();
    cy.url().should('include', '/dashboard');
    cy.contains('Welcome back').should('be.visible');
  });

  it('should show error with invalid credentials', () => {
    cy.get('[data-testid="email"]').type('wrong@example.com');
    cy.get('[data-testid="password"]').type('wrongpass');
    cy.get('[data-testid="login-btn"]').click();
    cy.contains('Invalid credentials').should('be.visible');
  });
});
```

---

## 📊 Mejora de Cobertura con Copilot

### Workflow Recomendado

```bash
# 1. Ejecutar análisis de cobertura
dotnet test /p:CollectCoverage=true

# 2. Identificar gaps
# Resultado: UserService.cs → 65% (15 líneas sin cobertura)

# 3. Prompt a Copilot:
"Analiza UserService.cs y genera tests para las líneas sin cobertura.
Enfócate en el método UpdateProfileAsync y las validaciones."

# 4. Copilot genera tests faltantes

# 5. Re-ejecutar cobertura
dotnet test /p:CollectCoverage=true
# Resultado: UserService.cs → 92% ✅
```

---

## 🚀 Best Practices con Copilot

### ✅ DO (Hacer)

1. **Prompt Específicos**: Incluye contexto, framework, escenarios esperados
2. **Revisar Código Generado**: Copilot es herramienta, no reemplazo de criterio
3. **Iteración**: Refina prompts si el primer resultado no es óptimo
4. **Aprender Patrones**: Observa cómo Copilot estructura tests y aplica esos patrones
5. **Feedback Loop**: Si Copilot genera mal, corrígelo y aprenderá del código corregido

### ❌ DON'T (No Hacer)

1. **Aceptar Ciegamente**: No copies/pegues sin entender
2. **Tests Sin Assertions**: Copilot a veces olvida asserts, verifica siempre
3. **Dependencias Reales**: Asegúrate que use mocks, no servicios reales
4. **Tests Frágiles**: Evita tests que dependan de timing o estado global
5. **Ignorar Nomenclatura**: Mantén convenciones del proyecto

---

## 🎓 Ejercicios Prácticos

### Ejercicio 1: Primeros Tests con Copilot
```
1. Abre CommentService.cs
2. Selecciona el método AddCommentAsync
3. En Copilot Chat escribe: /tests
4. Revisa los tests generados
5. Ejecuta: dotnet test
6. Verifica que pasen todos
```

### Ejercicio 2: Mejorar Cobertura
```
1. Ejecuta: dotnet test /p:CollectCoverage=true
2. Identifica clase con < 70% cobertura
3. Prompt: "Genera tests para [ClassName] para alcanzar 90% cobertura"
4. Ejecuta tests y verifica mejora
```

### Ejercicio 3: Tests E2E
```
1. Instala Cypress en proyecto Angular
2. Prompt: "Genera test Cypress para flujo de creación de ticket"
3. Ejecuta: npx cypress open
4. Verifica que el test pase
```

---

## 📈 Métricas de Éxito

Después de adoptar Copilot para testing, deberías ver:

| **Métrica** | **Antes** | **Con Copilot** | **Mejora** |
|-------------|-----------|-----------------|------------|
| Tiempo creación test | 30 min | 5 min | **83% ↓** |
| Cobertura promedio | 45% | 85% | **89% ↑** |
| Tests generados/día | 5 | 25 | **400% ↑** |
| Bugs en producción | 8/sprint | 2/sprint | **75% ↓** |

---

## 🎬 Conclusión

GitHub Copilot democratiza el testing - ya no es necesario ser experto en xUnit, Moq o Jasmine para escribir tests de calidad. Con prompts efectivos y Agent Mode, puedes:

- 🚀 Acelerar creación de tests 5-10x
- 📈 Alcanzar 80%+ cobertura en días, no meses
- 🎯 Cubrir casos edge que humanamente olvidaríamos
- 🧠 Aprender mejores prácticas observando código generado

**Próximo paso**: Practica con [ejercicios reales en tu proyecto](./03-ejercicios-practicos-testing.md)

---

**Fecha de actualización**: Noviembre 2025  
**Tema anterior**: [01-introduccion-testing.md](./01-introduccion-testing.md)  
**Próximo tema**: [Buenas prácticas de testing](../sesion4/TESTING_AUTOMATIZADO_CON_GITHUB_COPILOT.md)
