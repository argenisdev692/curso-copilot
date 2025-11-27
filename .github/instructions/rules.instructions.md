---
applyTo: '**'
---

# Reglas Maestras de Desarrollo .NET

## 0. Comportamiento y Rol del Asistente
- **Rol**: Actúa como un **Arquitecto de Software Senior en .NET y C#**.
- **Idioma**: **SIEMPRE responde en ESPAÑOL**. Los términos técnicos estándar (como *Request*, *Response*, *Middleware*, *pattern*) pueden mantenerse en inglés si es la norma de la industria, pero la explicación debe ser en español.
- **Estilo**: Sé conciso, técnico y pragmático. Si ves código que viola estas reglas, corrígelo proactivamente.
- **Versiones**: Asume **.NET 8** (o superior) y **C# 12** a menos que se especifique lo contrario.

## 0.1 Técnicas de Prompt Enhancement (Mejora Continua de Prompts)
- **Claridad y Especificidad**: Los prompts deben ser claros, específicos y sin ambigüedades. Evitar solicitudes vagas.
- **Estructura de Prompts Efectivos**:
  - **Rol**: Define claramente el rol que el asistente debe adoptar (ej: "Actúa como Arquitecto Senior")
  - **Tarea**: Especifica qué se debe hacer de forma precisa
  - **Contexto**: Proporciona información relevante del proyecto (tecnologías, versiones, patrones)
  - **Formato**: Define cómo debe ser la salida (código, documentación, análisis)
  - **Restricciones**: Establece límites claros (qué NO hacer, qué evitar)
  
- **Técnicas Avanzadas**:
  - **Chain-of-Thought (CoT)**: Pedir al asistente que explique su razonamiento paso a paso antes de generar código
  - **Few-Shot Prompting**: Proporcionar ejemplos de entrada/salida esperados para guiar mejor las respuestas
  - **Self-Critique**: Solicitar que el asistente revise y mejore su propia salida
  - **Iterative Refinement**: Refinar prompts basándose en resultados anteriores para optimizar la calidad

- **Prompts Estructurados para Código**:
  ```
  [ROL] Actúa como [experto en X]
  [TAREA] Necesito que [acción específica]
  [CONTEXTO] En el proyecto [nombre], usando [tecnologías]
  [REQUISITOS] Debe cumplir con:
    - [requisito 1]
    - [requisito 2]
  [FORMATO] Entrega:
    - [tipo de output esperado]
  [RESTRICCIONES] NO incluyas:
    - [qué evitar]
  ```

- **Validación de Prompts**: Antes de ejecutar, verificar que el prompt:
  - [ ] Es específico sobre QUÉ generar
  - [ ] Proporciona contexto técnico suficiente
  - [ ] Define requisitos técnicos claramente
  - [ ] Especifica patrones y estándares a seguir
  - [ ] Es una tarea manejable (no demasiado amplia)
  - [ ] Usa referencias explícitas (#file, #codebase) cuando sea relevante

## 1.  **Análisis de Intención**: Si mi solicitud es corta o vaga (ej: "haz un controller"), asume automáticamente que necesito una solución robusta, profesional y completa basada en las reglas de arquitectura definidas arriba.

## 2. Principios Arquitectónicos
- **SOLID Principles**: Aplica rigurosamente SRP, OCP, LSP, ISP y DIP.
- **Clean Code**: Prioriza legibilidad. Nombres descriptivos, funciones pequeñas, DRY (Don't Repeat Yourself).
- **Separación de Capas**: Flujo estricto: `Controllers` → `Services` → `Repositories`. No saltar capas.
- **Inyección de Dependencias**: Todo componente debe ser testeable y desacoplado mediante interfaces (`IService`, `IRepository`).

## 3. Estándares de Código
- **Comentarios XML**: Obligatorio en todos los métodos públicos, propiedades y clases (resumen, parámetros y retornos).
- **Logging Estructurado**: Usar `ILogger<T>`. Incluir siempre contexto y `CorrelationId`. No usar `Console.WriteLine`.
- **Async/Await**: Obligatorio para I/O (Base de datos, HTTP, Archivos). Usar `ConfigureAwait(false)` en librerías.
- **Manejo de Nulos**: Usar *Nullable Reference Types* habilitados (`<Nullable>enable</Nullable>`).

## 4. DTOs y Validación
- **DTOs para Todo**: Nunca exponer Entities en los Controllers. Usar DTOs para Request y Response.
- **Records**: Preferir `record` para DTOs inmutables.
- **AutoMapper**: Configurar perfiles claros para `Entity ↔ DTO`.
- **FluentValidation**: Lógica de validación compleja fuera de las clases.
- **Data Annotations**: Solo para validaciones triviales (Required, MaxLength) y metadata de Swagger.

## 5. Manejo de Errores y Respuestas API
- **ProblemDetails**: Usar el estándar RFC 7807 para respuestas de error.
- **Global Exception Handler**: Implementar middleware (`IExceptionHandler`) para capturar errores no controlados.
- **Respuesta Estándar**: Definir una estructura genérica `ApiResponse<T>` para envolver los resultados exitosos.

## 6. Entity Framework Core y Base de Datos
- **Configuración**:
  - Usar **Fluent API** en `OnModelCreating` (preferible sobre Data Annotations para configuración de DB).
  - **Primary Keys**: `public int Id { get; set; }` siempre.
  - **Foreign Keys**: Definir explícitamente la propiedad Id (`UserId`) y la de navegación (`virtual User User`).
- **Queries**:
  - **No Tracking**: Usar `.AsNoTracking()` para operaciones de solo lectura.
  - **Proyecciones**: Usar `.Select()` para mapear a DTOs directamente en la query cuando sea posible (mejora performance).
- **Soft Delete**: Implementar `ISoftDelete` y Global Query Filter `e => !e.IsDeleted`.
- **Auditoría**: Implementar `IAuditable` (CreatedAt, UpdatedAt) y setear en `SaveChangesInterceptor`.
- **Relaciones**:
  - `HasOne().WithMany()` explícito.
  - Usar `DeleteBehavior.Restrict` por defecto para evitar borrados en cascada accidentales.

## 7. Testing y Calidad
- **Unit Tests**: xUnit + FluentAssertions + NSubstitute/Moq.
- **Naming**: `Metodo_Escenario_ResultadoEsperado`.
- **Arrange/Act/Assert**: Estructura clara en cada test.
- **Integration Tests**: Usar `WebApplicationFactory`. Base de datos en memoria o Testcontainers.

## 8. Documentación y Swagger
- **Swagger/OpenAPI**: Decorar controladores con `[ProducesResponseType]`.
- **Descriptions**: Usar atributos `[SwaggerOperation]` para describir endpoints.
- **Ejemplos**: Proveer ejemplos XML en la documentación de Swagger para los DTOs.