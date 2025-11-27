# GitHub Copilot Rules Base - Backend (.NET)

## Reglas Fundamentales para Desarrollo con Copilot

### Principios Arquitectónicos
- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Clean Code**: Nombres descriptivos, funciones pequeñas, DRY (Don't Repeat Yourself)
- **Separación de Capas**: Controllers → Services → Repositories
- **DTOs para Todo**: Data Transfer Objects para todas las comunicaciones API

### Estándares de Código
- **Comentarios XML**: En todos los métodos públicos, propiedades y clases
- **Logging Estructurado**: Usar ILogger con contexto y correlation IDs
- **Async/Await**: Para todas las operaciones I/O (base de datos, HTTP, archivos)
- **Inyección de Dependencias**: Código testeable con interfaces y DI

### DTOs y Validación
- **DTOs para Todo**: Data Transfer Objects para todas las comunicaciones API
- **AutoMapper**: Para mapeo automático DTO ↔ Entity
- **FluentValidation**: Para todas las validaciones de entrada con reglas complejas
- **Data Annotations**: Para validaciones simples y metadata
- **Custom Validators**: Para lógica de negocio específica
- **Validation Attributes**: Para validaciones declarativas

### Manejo de Errores y Respuestas
- **ProblemDetails**: RFC 7807 para respuestas de error consistentes
- **Global Exception Handler**: Middleware para excepciones no manejadas
- **Validation Errors**: Detallados con campo específico y mensaje
- **Correlation IDs**: Para tracing de requests
- **Logging Estructurado**: Con contexto y niveles apropiados

### Patrones de Desarrollo
- **Repository Pattern**: Abstracción de acceso a datos
- **Service Layer**: Lógica de negocio separada
- **Unit of Work**: Para transacciones complejas
- **CQRS**: Separación de comandos y queries cuando aplique

### Testing y Calidad
- **Unit Tests**: xUnit con Moq para dependencias
- **Integration Tests**: WebApplicationFactory para APIs completas
- **Code Coverage**: Mínimo 80% en lógica crítica
- **TDD/BDD**: Desarrollo guiado por tests

### Documentación y API
- **Swagger/OpenAPI**: Documentación automática completa
- **Versionado de API**: Para cambios breaking
- **HATEOAS**: Links en responses cuando apropiado
- **Rate Limiting**: Protección contra abuso

### Entity Framework Core y Base de Datos
- **Models**: Entities con Data Annotations y Fluent API
- **Primary Keys**: `public int Id { get; set; }` con `[Key]` y `DatabaseGenerated(DatabaseGeneratedOption.Identity)`
- **Foreign Keys**: Propiedades explícitas con sufijo `Id` (ej: `CreatedById`, `AssignedToId`, `TicketId`)
- **Base Entity**: Campos comunes (Id, CreatedAt, UpdatedAt, IsDeleted)
- **Navigation Properties**: Propiedades virtuales para relaciones (ej: `public virtual User CreatedBy { get; set; }`)
- **Relationships**:
  - Configuración explícita con Fluent API en `OnModelCreating`
  - `HasOne().WithMany()` para uno-a-muchos
  - `HasMany().WithOne()` para inversa de uno-a-muchos
  - Foreign Keys con `HasForeignKey()` y `OnDelete()` behavior
  - `InverseProperty` attribute para múltiples relaciones al mismo tipo
- **Soft Delete**: Implementación con query filters globales `HasQueryFilter(e => !e.IsDeleted)`
- **Audit Fields**: CreatedAt, UpdatedAt automáticos con valores por defecto `DateTime.UtcNow`
- **Indexes**:
  - Índices simples con `[Index(nameof(Property))]`
  - Índices compuestos con `[Index(nameof(Prop1), nameof(Prop2))]`
  - Índices únicos con `.IsUnique()` en Fluent API
  - Índices descendentes con `IsDescending` parameter
- **Migrations**: Versionado de schema con nombres descriptivos
- **DbContext**: Configuración centralizada de EF Core
- **Seed Data**: Datos iniciales para desarrollo/testing en `OnModelCreating`

### Performance y Escalabilidad
- **Caching**: In-memory, Redis para escalabilidad
- **Pagination**: Para listas grandes
- **Async Operations**: Para operaciones costosas
- **Connection Pooling**: Configurado correctamente

## Estructura de Directorios para CRUD

```
TicketManagementSystem.API/
├── Controllers/
│   ├── [Entidad]Controller.cs
│   └── ...
├── DTOs/
│   ├── [Entidad]Dto.cs
│   ├── Create[Entidad]Dto.cs
│   ├── Update[Entidad]Dto.cs
│   └── ...
├── Services/
│   ├── Interfaces/
│   │   └── I[Entidad]Service.cs
│   └── [Entidad]Service.cs
├── Repositories/
│   ├── Interfaces/
│   │   └── I[Entidad]Repository.cs
│   └── [Entidad]Repository.cs
├── Validators/
│   ├── Create[Entidad]DtoValidator.cs
│   ├── Update[Entidad]DtoValidator.cs
│   └── ...
├── Mappings/
│   └── [Entidad]MappingProfile.cs
├── Models/
│   └── [Entidad].cs
└── Tests/
    ├── Unit/
    │   ├── Services/
    │   │   └── [Entidad]ServiceTests.cs
    │   └── Repositories/
    │       └── [Entidad]RepositoryTests.cs
    └── Integration/
        └── Controllers/
            └── [Entidad]ControllerTests.cs
```

## Checklist de Calidad para Code Reviews

### Arquitectura
- [ ] Principios SOLID aplicados correctamente
- [ ] Separación de capas mantenida
- [ ] DTOs usados consistentemente
- [ ] Inyección de dependencias implementada

### Código
- [ ] Comentarios XML completos
- [ ] Nombres descriptivos y consistentes
- [ ] Funciones pequeñas y enfocadas
- [ ] Código DRY (no repetitivo)

### Seguridad
- [ ] Validaciones de entrada robustas
- [ ] Autorización implementada
- [ ] Manejo seguro de datos sensibles
- [ ] Headers de seguridad configurados

### Performance
- [ ] Queries optimizadas
- [ ] Async/await usado apropiadamente
- [ ] Caching implementado donde necesario
- [ ] Memory leaks evitados

### Testing
- [ ] Unit tests para lógica crítica
- [ ] Integration tests para APIs
- [ ] Code coverage adecuada
- [ ] Tests automatizados en CI/CD

### Documentación
- [ ] Swagger documentation completa
- [ ] Comentarios de código útiles
- [ ] READMEs actualizados
- [ ] API versioning documentado