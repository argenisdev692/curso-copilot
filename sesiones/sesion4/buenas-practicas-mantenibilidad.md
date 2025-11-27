# Buenas Prácticas en Mantenibilidad de Proyectos

## 🎯 Objetivos de Aprendizaje

Al finalizar esta guía, serás capaz de:
- Aplicar principios SOLID en código .NET y Angular
- Escribir código limpio y mantenible (Clean Code)
- Implementar patrones de diseño apropiados
- Estructurar proyectos escalables
- Usar GitHub Copilot para mejorar calidad de código

---

## 🏛️ Principios SOLID

### S - Single Responsibility Principle (SRP)

**"Una clase debe tener una sola razón para cambiar"**

#### ❌ Violación de SRP
```csharp
// ⚠️ Clase con múltiples responsabilidades
public class TicketService
{
    // Responsabilidad 1: Lógica de negocio
    public async Task<Ticket> CreateTicketAsync(CreateTicketDto dto) { }
    
    // Responsabilidad 2: Envío de emails
    public async Task SendNotificationEmail(Ticket ticket) 
    {
        var smtp = new SmtpClient();
        // ...lógica de email
    }
    
    // Responsabilidad 3: Logging
    public void LogTicketCreated(Ticket ticket)
    {
        File.AppendAllText("log.txt", $"Ticket {ticket.Id} created");
    }
    
    // Responsabilidad 4: Validación
    public bool ValidateTicket(Ticket ticket)
    {
        return !string.IsNullOrEmpty(ticket.Title) && ticket.Priority != null;
    }
}
```

#### ✅ Aplicando SRP
```csharp
// ✅ Cada clase tiene una responsabilidad única

// 1. Lógica de negocio
public class TicketService
{
    private readonly ITicketRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ILogger<TicketService> _logger;
    private readonly ITicketValidator _validator;
    
    public async Task<Result<Ticket>> CreateTicketAsync(CreateTicketDto dto)
    {
        var validationResult = _validator.Validate(dto);
        if (!validationResult.IsValid)
            return Result<Ticket>.Failure(validationResult.Errors);
        
        var ticket = await _repository.CreateAsync(dto);
        
        await _emailService.SendTicketCreatedNotificationAsync(ticket);
        _logger.LogInformation("Ticket {TicketId} created successfully", ticket.Id);
        
        return Result<Ticket>.Success(ticket);
    }
}

// 2. Servicio de email
public class EmailService : IEmailService
{
    public async Task SendTicketCreatedNotificationAsync(Ticket ticket)
    {
        // Solo lógica de email
    }
}

// 3. Validador
public class TicketValidator : ITicketValidator
{
    public ValidationResult Validate(CreateTicketDto dto)
    {
        // Solo lógica de validación
    }
}
```

---

### O - Open/Closed Principle (OCP)

**"Abierto para extensión, cerrado para modificación"**

#### ❌ Violación de OCP
```csharp
public class NotificationService
{
    public async Task SendNotification(Ticket ticket, string type)
    {
        if (type == "email")
        {
            // Enviar email
        }
        else if (type == "sms")
        {
            // Enviar SMS
        }
        else if (type == "push")
        {
            // Enviar push notification
        }
        // ⚠️ Cada nuevo canal requiere modificar esta clase
    }
}
```

#### ✅ Aplicando OCP
```csharp
// Interface para extensión
public interface INotificationChannel
{
    Task SendAsync(Ticket ticket);
}

// Implementaciones específicas
public class EmailNotificationChannel : INotificationChannel
{
    public async Task SendAsync(Ticket ticket)
    {
        // Lógica específica de email
    }
}

public class SmsNotificationChannel : INotificationChannel
{
    public async Task SendAsync(Ticket ticket)
    {
        // Lógica específica de SMS
    }
}

public class PushNotificationChannel : INotificationChannel
{
    public async Task SendAsync(Ticket ticket)
    {
        // Lógica específica de push
    }
}

// Servicio que usa los canales
public class NotificationService
{
    private readonly IEnumerable<INotificationChannel> _channels;
    
    public NotificationService(IEnumerable<INotificationChannel> channels)
    {
        _channels = channels;
    }
    
    public async Task SendAllNotificationsAsync(Ticket ticket)
    {
        foreach (var channel in _channels)
        {
            await channel.SendAsync(ticket);
        }
    }
}

// ✅ Agregar nuevo canal NO requiere modificar código existente
public class SlackNotificationChannel : INotificationChannel
{
    public async Task SendAsync(Ticket ticket) { /* ... */ }
}
```

---

### L - Liskov Substitution Principle (LSP)

**"Los objetos derivados deben ser sustituibles por sus tipos base"**

#### ❌ Violación de LSP
```csharp
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    
    public int GetArea() => Width * Height;
}

public class Square : Rectangle
{
    // ⚠️ Rompe LSP: cambiar Width también cambia Height
    public override int Width
    {
        get => base.Width;
        set
        {
            base.Width = value;
            base.Height = value; // Side effect inesperado
        }
    }
    
    public override int Height
    {
        get => base.Height;
        set
        {
            base.Width = value;
            base.Height = value;
        }
    }
}

// Uso que falla
Rectangle rect = new Square();
rect.Width = 5;
rect.Height = 10;
Console.WriteLine(rect.GetArea()); // Esperado: 50, Actual: 100 ⚠️
```

#### ✅ Aplicando LSP
```csharp
// Interface común
public interface IShape
{
    int GetArea();
}

// Implementaciones independientes
public class Rectangle : IShape
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public int GetArea() => Width * Height;
}

public class Square : IShape
{
    public int Side { get; set; }
    
    public int GetArea() => Side * Side;
}

// ✅ Uso predecible
IShape shape1 = new Rectangle { Width = 5, Height = 10 };
IShape shape2 = new Square { Side = 5 };

Console.WriteLine(shape1.GetArea()); // 50 ✅
Console.WriteLine(shape2.GetArea()); // 25 ✅
```

---

### I - Interface Segregation Principle (ISP)

**"Los clientes no deben depender de interfaces que no usan"**

#### ❌ Violación de ISP
```csharp
// ⚠️ Interface "gorda" con muchas responsabilidades
public interface ITicketOperations
{
    Task CreateAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(int id);
    Task AssignAsync(int ticketId, int userId);
    Task CloseAsync(int ticketId);
    Task ReopenAsync(int ticketId);
    Task AddCommentAsync(int ticketId, Comment comment);
    Task AttachFileAsync(int ticketId, FileAttachment file);
    Task SendEmailAsync(int ticketId);
    Task GenerateReportAsync(int ticketId);
}

// Cliente que solo necesita lectura forzado a implementar TODO
public class TicketReportService : ITicketOperations
{
    public Task GenerateReportAsync(int ticketId) { /* Implementado */ }
    
    // ⚠️ Métodos innecesarios
    public Task CreateAsync(Ticket ticket) => throw new NotImplementedException();
    public Task UpdateAsync(Ticket ticket) => throw new NotImplementedException();
    public Task DeleteAsync(int id) => throw new NotImplementedException();
    // ...etc
}
```

#### ✅ Aplicando ISP
```csharp
// Interfaces segregadas por responsabilidad
public interface ITicketWriter
{
    Task CreateAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(int id);
}

public interface ITicketAssignment
{
    Task AssignAsync(int ticketId, int userId);
}

public interface ITicketStatusManager
{
    Task CloseAsync(int ticketId);
    Task ReopenAsync(int ticketId);
}

public interface ITicketComments
{
    Task AddCommentAsync(int ticketId, Comment comment);
}

public interface ITicketReporting
{
    Task GenerateReportAsync(int ticketId);
}

// ✅ Clientes implementan solo lo que necesitan
public class TicketReportService : ITicketReporting
{
    public Task GenerateReportAsync(int ticketId)
    {
        // Solo implementa lo necesario
    }
}

public class TicketService : ITicketWriter, ITicketAssignment, ITicketStatusManager
{
    // Implementa múltiples interfaces relacionadas
}
```

---

### D - Dependency Inversion Principle (DIP)

**"Depender de abstracciones, no de concreciones"**

#### ❌ Violación de DIP
```csharp
// ⚠️ Dependencia directa de implementación concreta
public class TicketService
{
    private readonly SqlServerTicketRepository _repository; // Acoplamiento fuerte
    
    public TicketService()
    {
        _repository = new SqlServerTicketRepository(); // Instanciación directa
    }
    
    public async Task<Ticket> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

// ⚠️ Cambiar a MongoDB requiere modificar TicketService
```

#### ✅ Aplicando DIP
```csharp
// Abstracción
public interface ITicketRepository
{
    Task<Ticket> GetByIdAsync(int id);
    Task<List<Ticket>> GetAllAsync();
    Task<Ticket> CreateAsync(Ticket ticket);
}

// Implementaciones concretas
public class SqlServerTicketRepository : ITicketRepository
{
    public async Task<Ticket> GetByIdAsync(int id) { /* SQL Server logic */ }
    public async Task<List<Ticket>> GetAllAsync() { /* ... */ }
    public async Task<Ticket> CreateAsync(Ticket ticket) { /* ... */ }
}

public class MongoDbTicketRepository : ITicketRepository
{
    public async Task<Ticket> GetByIdAsync(int id) { /* MongoDB logic */ }
    public async Task<List<Ticket>> GetAllAsync() { /* ... */ }
    public async Task<Ticket> CreateAsync(Ticket ticket) { /* ... */ }
}

// ✅ Service depende de abstracción
public class TicketService
{
    private readonly ITicketRepository _repository; // Interfaz, no implementación
    
    public TicketService(ITicketRepository repository) // Inyección de dependencia
    {
        _repository = repository;
    }
    
    public async Task<Ticket> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

// Configuración en Program.cs
services.AddScoped<ITicketRepository, SqlServerTicketRepository>();
// ✅ Cambiar a MongoDB solo requiere cambiar esta línea
// services.AddScoped<ITicketRepository, MongoDbTicketRepository>();
```

---

## 🧹 Principios de Clean Code

### 1. Nombres Significativos

#### ❌ Nombres Ambiguos
```csharp
public class Mgr
{
    public List<T> GetData(int id) // ¿Qué datos?
    {
        var d = DateTime.Now; // ¿d de qué?
        var temp = Process(id); // ¿temp de qué?
        return temp;
    }
}
```

#### ✅ Nombres Descriptivos
```csharp
public class TicketManager
{
    public async Task<List<Ticket>> GetTicketsByUserIdAsync(int userId)
    {
        var currentDateTime = DateTime.UtcNow;
        var userTickets = await FetchTicketsFromDatabaseAsync(userId);
        return userTickets;
    }
}
```

---

### 2. Funciones Pequeñas

#### ❌ Función Larga (>50 líneas)
```csharp
public async Task<Result> ProcessTicketAsync(int ticketId)
{
    // 1. Validación (10 líneas)
    var ticket = await _repository.GetByIdAsync(ticketId);
    if (ticket == null) return Result.NotFound();
    if (ticket.Status == Status.Closed) return Result.BadRequest("Closed");
    // ...
    
    // 2. Lógica de negocio (20 líneas)
    if (ticket.Priority == Priority.High) { /* ... */ }
    // ...
    
    // 3. Notificaciones (15 líneas)
    await SendEmailAsync(ticket);
    await SendSmsAsync(ticket);
    // ...
    
    // 4. Logging (10 líneas)
    _logger.LogInformation("Ticket processed");
    // ...
}
```

#### ✅ Funciones Pequeñas y Cohesivas
```csharp
public async Task<Result> ProcessTicketAsync(int ticketId)
{
    var ticket = await GetValidatedTicketAsync(ticketId);
    if (ticket == null) return Result.NotFound();
    
    await ApplyBusinessRulesAsync(ticket);
    await SendNotificationsAsync(ticket);
    LogTicketProcessed(ticket);
    
    return Result.Success();
}

private async Task<Ticket?> GetValidatedTicketAsync(int ticketId)
{
    var ticket = await _repository.GetByIdAsync(ticketId);
    return ticket?.Status != Status.Closed ? ticket : null;
}

private async Task ApplyBusinessRulesAsync(Ticket ticket)
{
    if (ticket.Priority == Priority.High)
        await EscalateTicketAsync(ticket);
}

private async Task SendNotificationsAsync(Ticket ticket)
{
    await _emailService.SendAsync(ticket);
    await _smsService.SendAsync(ticket);
}

private void LogTicketProcessed(Ticket ticket)
{
    _logger.LogInformation("Ticket {TicketId} processed", ticket.Id);
}
```

**Regla**: Funciones deben tener < 20 líneas idealmente

---

### 3. No Más de 3 Parámetros

#### ❌ Muchos Parámetros
```csharp
public Task CreateTicketAsync(
    string title,
    string description,
    int priority,
    int categoryId,
    int createdById,
    int? assignedToId,
    DateTime? dueDate,
    List<string> tags,
    bool isUrgent)
{
    // ...
}
```

#### ✅ Usar DTOs
```csharp
public record CreateTicketDto(
    string Title,
    string Description,
    int Priority,
    int CategoryId,
    int CreatedById,
    int? AssignedToId = null,
    DateTime? DueDate = null,
    List<string>? Tags = null,
    bool IsUrgent = false
);

public Task<Ticket> CreateTicketAsync(CreateTicketDto dto)
{
    // ✅ Un solo parámetro, fácil de extender
}
```

---

### 4. DRY (Don't Repeat Yourself)

#### ❌ Código Duplicado
```csharp
public async Task CloseTicketAsync(int ticketId)
{
    var ticket = await _repository.GetByIdAsync(ticketId);
    if (ticket == null) throw new NotFoundException();
    if (ticket.Status == Status.Closed) throw new InvalidOperationException();
    
    ticket.Status = Status.Closed;
    await _repository.UpdateAsync(ticket);
}

public async Task ReopenTicketAsync(int ticketId)
{
    var ticket = await _repository.GetByIdAsync(ticketId);
    if (ticket == null) throw new NotFoundException();
    if (ticket.Status == Status.Closed) throw new InvalidOperationException();
    
    ticket.Status = Status.Open;
    await _repository.UpdateAsync(ticket);
}
```

#### ✅ Extraer Lógica Común
```csharp
private async Task<Ticket> GetValidTicketAsync(int ticketId)
{
    var ticket = await _repository.GetByIdAsync(ticketId);
    if (ticket == null) throw new NotFoundException($"Ticket {ticketId} not found");
    if (ticket.Status == Status.Closed) throw new InvalidOperationException("Cannot modify closed ticket");
    return ticket;
}

public async Task CloseTicketAsync(int ticketId)
{
    var ticket = await GetValidTicketAsync(ticketId);
    ticket.Status = Status.Closed;
    await _repository.UpdateAsync(ticket);
}

public async Task ReopenTicketAsync(int ticketId)
{
    var ticket = await GetValidTicketAsync(ticketId);
    ticket.Status = Status.Open;
    await _repository.UpdateAsync(ticket);
}
```

---

## 📁 Estructura de Proyecto Escalable

### Backend (.NET)
```
TicketManagementSystem/
├── src/
│   ├── Core/                          # Dominio y lógica de negocio
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   ├── DTOs/
│   │   ├── Exceptions/
│   │   └── Services/
│   ├── Infrastructure/                # Implementaciones
│   │   ├── Data/
│   │   │   ├── Repositories/
│   │   │   └── DbContext/
│   │   ├── ExternalServices/
│   │   └── Logging/
│   ├── API/                           # Web API
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   └── Program.cs
│   └── Shared/                        # Código compartido
│       ├── Constants/
│       ├── Helpers/
│       └── Extensions/
└── tests/
    ├── UnitTests/
    ├── IntegrationTests/
    └── E2ETests/
```

### Frontend (Angular)
```
ticket-system-app/
├── src/
│   ├── app/
│   │   ├── core/                      # Singleton services
│   │   │   ├── services/
│   │   │   ├── guards/
│   │   │   ├── interceptors/
│   │   │   └── models/
│   │   ├── shared/                    # Componentes reutilizables
│   │   │   ├── components/
│   │   │   ├── directives/
│   │   │   ├── pipes/
│   │   │   └── utils/
│   │   ├── features/                  # Features por módulo
│   │   │   ├── tickets/
│   │   │   │   ├── components/
│   │   │   │   ├── services/
│   │   │   │   ├── models/
│   │   │   │   └── tickets.routes.ts
│   │   │   ├── auth/
│   │   │   └── admin/
│   │   └── app.routes.ts
│   ├── assets/
│   └── styles/
│       ├── _variables.scss
│       ├── _mixins.scss
│       └── global.scss
└── tests/
```

---

## 🤖 Uso de GitHub Copilot

### Prompts para Calidad de Código

#### 1. Detectar Violaciones SOLID
```
Analiza este código y detecta violaciones de principios SOLID.
Sugiere refactorización aplicando SRP, OCP, LSP, ISP y DIP.
```

#### 2. Aplicar Clean Code
```
Refactoriza este método siguiendo principios Clean Code:
- Nombres descriptivos
- Funciones pequeñas (< 20 líneas)
- Máximo 3 parámetros
- Extraer código duplicado
```

#### 3. Mejorar Mantenibilidad
```
Mejora la mantenibilidad de esta clase:
- Separar responsabilidades
- Inyección de dependencias
- Manejo de errores apropiado
- Logging estructurado
```

---

## ✅ Checklist de Mantenibilidad

### Principios SOLID
- [ ] Clases con responsabilidad única (SRP)
- [ ] Código extensible sin modificación (OCP)
- [ ] Sustitución segura de tipos (LSP)
- [ ] Interfaces segregadas (ISP)
- [ ] Dependencia de abstracciones (DIP)

### Clean Code
- [ ] Nombres descriptivos y sin ambigüedades
- [ ] Funciones pequeñas (< 20 líneas)
- [ ] Máximo 3 parámetros por función
- [ ] Sin código duplicado (DRY)
- [ ] Comentarios solo cuando necesario

### Arquitectura
- [ ] Separación clara de capas
- [ ] Inyección de dependencias configurada
- [ ] DTOs para transferencia de datos
- [ ] Manejo centralizado de errores
- [ ] Logging estructurado

### Testing
- [ ] Cobertura > 80% en lógica crítica
- [ ] Tests unitarios independientes
- [ ] Mocks para dependencias externas
- [ ] Tests de integración para flujos completos

---
