# 🚀 Prompts para GitHub Copilot 2025

## ✨ Novedades en 2025

### Agent Mode - Automatización Multi-Paso
Agent Mode permite automatización completa de tareas complejas con razonamiento multi-paso, cambios en múltiples archivos y ejecución de comandos.

### MCP Integration - Capacidades Extendidas
Model Context Protocol permite integrar herramientas externas y servidores MCP para funcionalidades avanzadas.

### Slash Commands - Comandos Rápidos
Comandos como `/optimize`, `/tools`, `/clear`, `/help` para acciones específicas.

### Custom Instructions - Instrucciones Personalizadas
Configura instrucciones específicas por repositorio o tipo de archivo.

## 🎯 Prompts Básicos

### Generación de Funciones
```csharp
// Crear una función que valide email en C# con expresiones regulares
public bool IsValidEmail(string email)
{
    // Implementar validación usando Regex con patrón moderno
}
```

### Creación de Clases
```csharp
// Crear una clase User con propiedades básicas y validaciones
public class User
{
    // Propiedades: Id (GUID), Name (requerido), Email (único), CreatedAt
    // Incluir Data Annotations para validación
}
```

### Generación de Tests
```csharp
// Crear test unitario completo para método Login con Arrange-Act-Assert
[Fact]
public async Task Login_ValidCredentials_ReturnsToken()
{
    // Arrange: Setup mock services y datos de prueba
    // Act: Ejecutar método bajo prueba
    // Assert: Verificar resultado esperado
}
```

## 🔧 Prompts Avanzados

### Arquitectura y Patrones con Agent Mode
```
/optimize
Implementar patrón Repository completo para entidad Product con:
- Interfaz genérica IRepository<T>
- Implementación con Entity Framework Core
- Unit of Work pattern
- Inyección de dependencias
- Manejo de transacciones
```

### API REST Moderna
```
/optimize
Crear controlador REST completo para gestión de orders con:
- Endpoints CRUD asíncronos
- Validación con FluentValidation
- Respuestas HTTP apropiadas
- Documentación Swagger/OpenAPI
- Manejo de errores global
- Rate limiting básico
```

### Frontend Angular con Signals
```
/optimize
Crear componente de lista de productos moderno con Angular 17+:
- Signals para estado reactivo
- Control flow syntax (@if, @for)
- Standalone components
- Servicios con HttpClient
- Formularios reactivos tipados
- Material Design opcional
```

## ⚡ Agent Mode - Automatización Completa

### Desarrollo Fullstack
```
Crear una aplicación completa de gestión de tareas con:
1. Backend .NET API con Entity Framework
2. Frontend Angular con componentes modernos
3. Base de datos MongoDB
4. Autenticación JWT
5. Tests unitarios e integración
6. Documentación automática
7. Dockerización completa
```

### Refactorización Multi-Archivo
```
Refactorizar toda la aplicación para usar:
- Arquitectura limpia (Clean Architecture)
- CQRS pattern con MediatR
- Result pattern para respuestas API
- FluentValidation para validaciones
- AutoMapper para mapeos
- Logging estructurado con Serilog
```

### Integración con MCP
```
Crear un sistema de notificaciones usando MCP server que:
- Conecte con servicios externos (SendGrid, Twilio)
- Implemente templates de email/SMS
- Maneje colas de mensajes
- Proporcione métricas de entrega
- Incluya reintentos y circuit breaker
```

## 🎨 Prompts con Slash Commands

### Optimización de Código
```
/optimize
Refactorizar este método para usar LINQ moderno y mejorar performance
```

### Análisis de Herramientas
```
/tools
Mostrar todas las herramientas MCP disponibles para análisis de código
```

### Contexto Específico
```
/context package.json
Analizar dependencias del proyecto y sugerir actualizaciones de seguridad
```

### Enfoque en Directorio
```
/focus src/components
Crear un nuevo componente de formulario con validación completa
```

## 📋 Custom Instructions - Instrucciones Personalizadas

### Archivo `.github/copilot-instructions.md`
```markdown
# Instrucciones para Copilot en este proyecto

## Estándares de Código
- Usar PascalCase para clases e interfaces
- camelCase para métodos y variables
- Interfaces prefijan con 'I'
- Async/await para operaciones I/O

## Arquitectura
- Clean Architecture con capas separadas
- Dependency Injection obligatorio
- Repository pattern para acceso a datos
- CQRS para operaciones complejas

## Testing
- Cobertura mínima 80%
- Tests unitarios con xUnit
- Tests de integración con TestServer
- Mocks con Moq

## Documentación
- XML comments en métodos públicos
- README actualizado en cambios
- API documentada con Swagger
```

### Instrucciones por Tipo de Archivo
```markdown
---
applyTo: "**/*.cs"
---

# Reglas específicas para archivos C#
- Usar records para DTOs inmutables
- Nullability annotations activadas
- Pattern matching en switch expressions
- Source generators para boilerplate
```

## 🔍 Issue Management con Copilot

### Crear Issues desde Imágenes
```
Analizar esta captura de pantalla de error y crear un issue detallado con:
- Descripción del problema
- Pasos para reproducir
- Información del entorno
- Severidad y prioridad
- Labels apropiadas
```

### Issues por Lotes
```
Crear issues separados para estas funcionalidades:
1. Sistema de autenticación con OAuth2
2. Dashboard de métricas en tiempo real
3. API de exportación de datos
4. Sistema de notificaciones push
```

## 🧪 Mejores Prácticas de Prompts 2025

### 1. **Sé Específico y Contextual**
✅ Bueno: "Crear un método que calcule el factorial usando recursión con memoización"
❌ Malo: "Crear función matemática"

### 2. **Proporciona Ejemplos Concretos**
```
Crear una función de validación que:
- Acepte: "user@example.com" → válido
- Rechace: "invalid-email" → inválido
- Maneje casos edge: null, string vacío, emails con caracteres especiales
```

### 3. **Especifica Restricciones y Requisitos**
```
Implementar API REST que:
- Use .NET 8 con minimal APIs
- Incluya validación automática
- Maneje errores con ProblemDetails
- Sea versionable (v1, v2)
- Incluya rate limiting
```

### 4. **Itera y Refina**
- Empieza con versión básica
- Pide mejoras incrementales
- Usa `/clear` para contexto fresco
- Combina con `/optimize` para mejoras

### 5. **Aprovecha Agent Mode**
```
No solo "crear componente", sino:
"Crear componente de login que integre con API de autenticación,
incluya manejo de errores, validación de formulario,
y navegación condicional basada en roles de usuario"
```

## 💻 Ejemplos por Tecnología 2025

### .NET C# Moderno
```csharp
// Records para DTOs
public record CreateUserRequest(string Name, string Email);

// Pattern Matching avanzado
public string ProcessOrder(Order order) => order.Status switch
{
    OrderStatus.Pending => "Procesando pago",
    OrderStatus.Paid => "Preparando envío",
    OrderStatus.Shipped => "En tránsito",
    _ => "Estado desconocido"
};

// Source Generators para boilerplate
[GenerateRepository(typeof(Product))]
public partial class ProductRepository { }
```

### Angular/TypeScript con Signals
```typescript
// Signals para estado reactivo
@Component({...})
export class TaskListComponent {
  private tasksService = inject(TasksService);
  tasks = signal<Task[]>([]);
  filter = signal<'all' | 'pending' | 'completed'>('all');

  filteredTasks = computed(() =>
    this.tasks().filter(task =>
      this.filter() === 'all' || task.status === this.filter()
    )
  );

  async ngOnInit() {
    this.tasks.set(await this.tasksService.getTasks());
  }
}
```

### Testing Moderno
```csharp
// xUnit con teoría y datos
[Theory]
[InlineData("user@example.com", true)]
[InlineData("invalid-email", false)]
[InlineData("", false)]
[InlineData(null, false)]
public void EmailValidation_WorksCorrectly(string email, bool expected)
{
    // Arrange & Act & Assert
    email.IsValidEmail().Should().Be(expected);
}

// Integration tests con TestServer
public class UsersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CreateUser_ReturnsCreated_WhenValidData()
    {
        // Test completo de API
    }
}
```

### DevOps con MCP
```
Configurar pipeline CI/CD que:
- Use GitHub Actions con MCP server
- Ejecute tests en paralelo
- Genere reportes de cobertura
- Despliegue automáticamente a staging
- Incluya validaciones de seguridad
- Notifique a Slack/Teams sobre estado
```

## 🎯 Casos de Uso Avanzados 2025

### 1. **Desarrollo Fullstack con Agent Mode**
```
Crear aplicación e-commerce completa:
- Backend: .NET API con CQRS y Event Sourcing
- Frontend: Angular con NgRx para state management
- Base de datos: PostgreSQL con EF Core
- Autenticación: JWT con refresh tokens
- Testing: Cobertura completa con Playwright E2E
- DevOps: Docker + GitHub Actions + Azure deployment
```

### 2. **Migración de Legacy Code**
```
Migrar aplicación ASP.NET MVC a .NET 8 con:
- Minimal APIs en lugar de controllers
- Entity Framework moderno
- Autenticación moderna (Microsoft Identity)
- Frontend Angular actualizado
- Tests migrados a xUnit
- Dockerización completa
```

### 3. **Integración con IA Externa via MCP**
```
Crear asistente de código que:
- Analice commits para generar changelogs
- Revise PRs automáticamente
- Sugiera mejoras de arquitectura
- Detecte vulnerabilidades de seguridad
- Optimice queries de base de datos
- Genere documentación técnica
```

---

## 📚 Recursos Adicionales 2025

- [GitHub Copilot Agent Mode Docs](https://docs.github.com/en/copilot/using-github-copilot/coding-agent)
- [MCP Registry](https://registry.modelcontextprotocol.io/)
- [Custom Instructions Guide](https://docs.github.com/en/copilot/customizing-copilot/adding-repository-custom-instructions-for-github-copilot)
- [Slash Commands Reference](https://docs.github.com/en/copilot/using-github-copilot/using-slash-commands-in-github-copilot)
- [Prompt Engineering Best Practices](https://docs.github.com/en/copilot/using-github-copilot/prompt-engineering-for-copilot-chat)