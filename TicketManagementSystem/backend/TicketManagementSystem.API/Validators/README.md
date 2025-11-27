# Guía de Validación de DTOs - TicketManagementSystem

## 📋 Descripción General

Esta guía documenta el uso del sistema de validación de DTOs basado en **FluentValidation** para el proyecto TicketManagementSystem. El snippet `BaseTicketValidator.cs` proporciona una base reutilizable para validar DTOs relacionados con tickets, siguiendo las mejores prácticas de .NET 8 y ASP.NET Core.

## 🚀 Instalación y Configuración

### 1. Dependencias Requeridas
Asegúrate de tener instalados los siguientes paquetes NuGet:
```bash
dotnet add package FluentValidation.AspNetCore
dotnet add package Microsoft.Extensions.Localization
```

### 2. Configuración en Program.cs
Agrega las siguientes líneas en tu `Program.cs` para registrar validadores y localización:

```csharp
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

// ... otros servicios ...

builder.Services.AddValidatorsFromAssemblyContaining<BaseTicketValidator<CreateTicketDto>>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configurar cultura por defecto
var supportedCultures = new[]
{
    new CultureInfo("es-ES"), // Español
    new CultureInfo("en-US")  // Inglés
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
```

### 3. Estructura de Archivos
Crea la siguiente estructura en tu proyecto:
```
Validators/
├── BaseTicketValidator.cs          # Clase base con validaciones comunes
├── CreateTicketDtoValidator.cs     # Validador específico (si no existe)
├── UpdateTicketDtoValidator.cs     # Validador específico (si no existe)
└── README.md                       # Esta documentación
Resources/
├── Validators.es-ES.resx           # Recursos en español
└── Validators.en-US.resx           # Recursos en inglés (opcional)
```

## 📖 Uso Básico

### Crear un Nuevo Validador
1. Crea una clase que herede de `BaseTicketValidator<T>`:
```csharp
using FluentValidation;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Validators
{
    public class CreateTicketDtoValidator : BaseTicketValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator(IStringLocalizer<CreateTicketDtoValidator> localizer)
            : base(localizer)
        {
            RuleFor(x => x.Title)
                .ValidateTitle();

            RuleFor(x => x.Description)
                .ValidateDescription();

            RuleFor(x => x.PriorityId)
                .ValidatePriorityId();
        }
    }
}
```

2. Inyecta el validador en tu controlador:
```csharp
private readonly IValidator<CreateTicketDto> _validator;

public TicketsController(IValidator<CreateTicketDto> validator, /* otros parámetros */)
{
    _validator = validator;
}
```

3. Usa el validador en tus acciones:
```csharp
[HttpPost]
public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto dto)
{
    var validationResult = await _validator.ValidateAsync(dto);
    if (!validationResult.IsValid)
    {
        return BadRequest(new
        {
            Errors = validationResult.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            })
        });
    }

    // Procede con la lógica de negocio...
}
```

## 🔧 Validaciones Comunes Disponibles

La clase `BaseTicketValidator<T>` proporciona métodos reutilizables:

### Validaciones de Texto
- `ValidateTitle()`: Título requerido, longitud 5-200, caracteres válidos
- `ValidateDescription()`: Descripción requerida, máximo 2000 caracteres

### Validaciones de Datos
- `ValidateEmail()`: Email requerido, formato válido, máximo 254 caracteres
- `ValidatePriorityId()`: ID de prioridad requerido, entre 1-4
- `ValidateUserId(fieldName)`: ID de usuario válido (> 0)

### Reglas de Negocio
- `ValidateAssignedUserDifferentFromCreator()`: Valida que el asignado no sea el creador
- `ValidateStatusTransition(currentStatus)`: Valida transiciones de estado válidas

## 🎯 Extensión para Nuevos DTOs

### Ejemplo Completo
```csharp
public class AssignTicketDto
{
    public int TicketId { get; set; }
    public int AssignedToUserId { get; set; }
    public string Notes { get; set; }
}

public class AssignTicketDtoValidator : BaseTicketValidator<AssignTicketDto>
{
    public AssignTicketDtoValidator(IStringLocalizer<AssignTicketDtoValidator> localizer)
        : base(localizer)
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage(_localizer["TicketIdInvalid"]);

        RuleFor(x => x.AssignedToUserId)
            .ValidateUserId("AssignedTo");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage(_localizer["NotesMaxLength", 500])
            .When(x => !string.IsNullOrEmpty(x.Notes));

        // Regla de negocio personalizada
        RuleFor(x => x)
            .Must(dto => dto.AssignedToUserId != GetCurrentUserId())
            .WithMessage(_localizer["CannotAssignToSelf"]);
    }
}
```

### Agregar Nuevas Validaciones a la Base
Si necesitas validaciones comunes adicionales, extiende `BaseTicketValidator<T>`:
```csharp
protected IRuleBuilderOptions<T, DateTime> ValidateFutureDate(IRuleBuilder<T, DateTime> ruleBuilder)
{
    return ruleBuilder
        .GreaterThan(DateTime.UtcNow).WithMessage(_localizer["DateMustBeFuture"]);
}
```

## 🌐 Localización de Mensajes

### Crear Archivos de Recursos
1. Agrega un nuevo archivo `Validators.resx` en la carpeta `Resources/`.
2. Agrega entradas como:
   - **Nombre**: `TitleRequired`
   - **Valor**: `El título es obligatorio`
   - **Comentario**: `Mensaje para título requerido`

### Entradas Recomendadas
```
TitleRequired: El título es obligatorio
TitleLength: El título debe tener entre {0} y {1} caracteres
DescriptionRequired: La descripción es obligatoria
EmailInvalid: El formato del email no es válido
PriorityRequired: La prioridad es obligatoria
UserIdInvalid: El ID de usuario '{0}' no es válido
CannotAssignToSelf: No puedes asignarte un ticket a ti mismo
```

### Uso en Código
```csharp
RuleFor(x => x.Title)
    .NotEmpty().WithMessage(_localizer["TitleRequired"]);
```

## 🧪 Testing de Validadores

### Ejemplo de Test Unitario
```csharp
using FluentValidation.TestHelper;
using Xunit;

public class CreateTicketDtoValidatorTests
{
    private readonly CreateTicketDtoValidator _validator;

    public CreateTicketDtoValidatorTests()
    {
        var localizer = new Mock<IStringLocalizer<CreateTicketDtoValidator>>();
        localizer.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString("key", "message"));
        _validator = new CreateTicketDtoValidator(localizer.Object);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new CreateTicketDto { Title = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Valid()
    {
        var dto = new CreateTicketDto { Title = "Valid Title" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }
}
```

## 🔍 Troubleshooting

### Problemas Comunes

1. **"No se puede resolver IStringLocalizer"**
   - Asegúrate de tener `Microsoft.Extensions.Localization` instalado
   - Verifica la configuración de localización en `Program.cs`

2. **Mensajes no se localizan**
   - Confirma que los archivos `.resx` estén en `Resources/`
   - Asegúrate de que el nombre del archivo coincida con el namespace

3. **Validaciones no se ejecutan**
   - Verifica que el validador esté registrado en DI
   - Confirma que el DTO se valide antes de procesar

4. **Errores de compilación**
   - Asegúrate de que `FluentValidation` esté actualizado
   - Verifica que los DTOs existan y sean accesibles

### Logs y Debugging
Habilita logs detallados para validación:
```csharp
builder.Services.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<BaseTicketValidator<CreateTicketDto>>();
    fv.ValidatorOptions.DisplayNameResolver = (type, member, expression) =>
    {
        return member?.Name ?? "Unknown";
    };
});
```

## 📚 Mejores Prácticas

1. **Mantén validadores pequeños**: Un validador por DTO, enfocado en una responsabilidad.
2. **Reutiliza reglas base**: Usa los métodos de `BaseTicketValidator<T>` siempre que sea posible.
3. **Valida temprano**: Ejecuta validaciones lo antes posible en el pipeline.
4. **Mensajes descriptivos**: Usa localización para mensajes claros y consistentes.
5. **Tests exhaustivos**: Cubre casos positivos, negativos y edge cases.
6. **Performance**: Evita validaciones costosas en reglas comunes.

## 📞 Soporte

Para preguntas o issues relacionados con esta guía:
- Revisa los tests unitarios incluidos
- Consulta la documentación oficial de FluentValidation
- Contacta al equipo de desarrollo

---

**Última actualización**: Noviembre 2025
**Versión**: 1.0
**Compatible con**: .NET 8, ASP.NET Core 8