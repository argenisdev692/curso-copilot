# PROMPT ENGINEERING - BUENAS PRÁCTICAS PROFESIONALES

## ❌ ERRORES COMUNES

### 1. Prompts Vagos
**Mal:**
// crear servicio de usuario

**Bien:**
// Crear UserService con inyección de IUserRepository
// Método RegisterAsync: valida email único, hashea password
// Retorna Result<UserDto> con manejo de errores

### 2. Sin Contexto de Tecnologías
**Mal:**
// hacer login

**Bien:**
// Implementar login con JWT en .NET Core
// Usar Identity para autenticación
// Retornar token con claims (UserId, Email, Roles)

### 3. No Especificar Patrones
**Mal:**
// controlador de productos

**Bien:**
// Controller con patrón CQRS
// Commands: CreateProduct, UpdateProduct
// Queries: GetProducts, GetProductById
// Usar MediatR para dispatch

### 4. Olvidar Testing
**Mal:**
// Solo genera código de producción

**Bien:**
// Incluye en el prompt: "Genera también tests unitarios con xUnit"

### 5. No Iterar
**Mal:**
// Acepta primera sugerencia

**Bien:**
// Refina: "Agregar validaciones", "Mejorar nombres", "Agregar logging"

---

## ✅ BUENAS PRÁCTICAS

### 1. Especifica el Stack Completo
.NET Core 8, EF Core, FluentValidation, AutoMapper, xUnit

### 2. Menciona Patrones de Diseño
Repository, UnitOfWork, CQRS, Mediator, Factory

### 3. Define Tipos Explícitamente
Task<Result<UserDto>>, IActionResult, IEnumerable<T>

### 4. Incluye Requisitos No Funcionales
- Performance: caching, async/await
- Seguridad: validación, sanitización
- Observabilidad: logging, metrics

### 5. Usa Ejemplos Input/Output
// Input: { email: "test@test.com", password: "Pass123!" }
// Output: { token: "eyJ...", expiresIn: 3600 }

### 6. Iteración Progresiva
1. Estructura básica
2. Agregar validaciones
3. Agregar logging
4. Agregar tests
5. Optimizar performance

---

## 🎯 PLANTILLA DE PROMPT PROFESIONAL

// [ACCIÓN] + [COMPONENTE] + [TECNOLOGÍAS]
// [REQUISITOS FUNCIONALES]
// [PATRONES A APLICAR]
// [TIPOS Y FIRMAS]
// [REQUISITOS NO FUNCIONALES]
// [EJEMPLO INPUT/OUTPUT]

Ejemplo:
// Crear endpoint API REST para búsqueda de productos con .NET Core 8
// Funcional: paginación, filtros (categoría, precio), ordenamiento
// Patrones: Repository, Result pattern, CQRS
// Firma: Task<ActionResult<PagedResponse<ProductDto>>>
// No funcional: caching 5min, rate limiting, logging
// Input: { page: 1, pageSize: 20, category: "electronics", sortBy: "price" }
// Output: { items: [...], totalCount: 150, page: 1, pageSize: 20 }