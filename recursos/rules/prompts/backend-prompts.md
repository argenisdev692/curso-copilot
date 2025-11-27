# Prompts Específicos para Copilot - Backend .NET

## CRUD Completo con Copilot en C#
```
Crear sistema CRUD completo para [Entidad] en TicketManagementSystem:

MODEL/ENTITY:
- Primary Key: public int Id { get; set; } con [Key] y DatabaseGenerated(DatabaseGeneratedOption.Identity)
- Foreign Keys: Propiedades explícitas (ej: CreatedById, AssignedToId)
- Navigation Properties: Propiedades virtuales para relaciones
- Data Annotations: [Required], [MaxLength], [ForeignKey], [InverseProperty]
- Audit Fields: CreatedAt, UpdatedAt, IsDeleted
- Índices: [Index(nameof(Property))] para búsquedas frecuentes

RELATIONSHIPS:
- Configurar relaciones en DbContext.OnModelCreating
- HasOne().WithMany() para uno-a-muchos
- HasForeignKey() explícito
- OnDelete() behavior apropiado (Restrict, Cascade, SetNull)
- InverseProperty para múltiples relaciones al mismo tipo

CONTROLLER:
- CRUD completo: GET (read/list), POST (create), PUT (update), DELETE (soft delete)
- Route: /api/[entidad]
- Autorización por roles con [Authorize]
- Validación automática con FluentValidation
- Respuestas ProblemDetails para errores
- Pagination y filtering en GET
- Soft delete con IsDeleted flag

SERVICE:
- Lógica de negocio separada del controller
- Validaciones adicionales
- Mapeo DTO ↔ Entity con AutoMapper
- Logging de operaciones con ILogger
- Manejo de transacciones
- Verificación de existencia de foreign keys

REPOSITORY:
- Acceso a datos con EF Core
- Queries optimizadas con AsNoTracking
- Includes para relaciones (ej: .Include(t => t.CreatedBy))
- Métodos async
- Pagination y filtering
- Métodos para verificar existencia de entidades relacionadas
```

## Creación de Documentación XML, Swagger y Comentarios
```
Generar documentación completa para TicketManagementSystem API:

XML COMMENTS:
- Summary para clases, métodos, propiedades
- Param para parámetros
- Returns para valores de retorno
- Example para casos de uso comunes
- Exception para excepciones lanzadas

SWAGGER CONFIGURATION:
- Security definitions para JWT
- Response examples
- Schema descriptions
- Tags para agrupación de endpoints
- Info completa de API (versión, contacto, licencia)

CODE COMMENTS:
- Comentarios explicativos en lógica compleja
- TODOs para mejoras futuras
- Warnings para código legacy
- References a requirements o tickets
```

## Aplicación de Principios SOLID y Patrones de Diseño
```
Refactorizar código legacy aplicando SOLID principles:

SINGLE RESPONSIBILITY:
- Dividir clases grandes en responsabilidades específicas
- Extraer métodos complejos
- Crear servicios especializados

OPEN/CLOSED:
- Interfaces para extension sin modificación
- Strategy pattern para algoritmos variables
- Factory pattern para object creation

DEPENDENCY INVERSION:
- Interfaces en lugar de implementaciones concretas
- Inyección de dependencias
- Programar contra abstracciones

LISKOV SUBSTITUTION:
- Interfaces consistentes
- Preconditions no más restrictivas
- Postconditions garantizadas

INTERFACE SEGREGATION:
- Interfaces pequeñas y específicas
- Evitar interfaces "fat"
- Client-specific interfaces
```

## Buenas Prácticas de Seguridad, Validación y Manejo de Excepciones
```
Implementar seguridad y validación robusta en TicketManagementSystem:

INPUT VALIDATION:
- FluentValidation rules completas
- Sanitización de inputs
- Validación de tipos y rangos
- Custom validators para lógica de negocio

AUTHENTICATION & AUTHORIZATION:
- JWT tokens con claims apropiados
- Role-based access control
- Object-level permissions
- Refresh token rotation

ERROR HANDLING:
- Global exception middleware
- ProblemDetails responses consistentes
- Logging de errores con contexto
- No exposure de información sensible

SECURITY BEST PRACTICES:
- HTTPS enforcement
- CORS configuration restrictiva
- Rate limiting implementation
- Security headers (HSTS, CSP, etc.)