# Authentication Issues - Resolution Log

## Créditos Iniciales
**Sistema de Autenticación Inicial**: Implementado por **Grok AI**
- Login component y funcionalidad básica
- AuthState management con signals
- JWT token handling
- Refresh token mechanism

---

## Problema 1: Error 500 - "User Already Exists" No Detectado

**Fecha**: 19 Noviembre 2025  
**Componente**: Backend - UserRepository/CompiledQueries  
**Severidad**: Alta  

### Síntomas
```
POST /api/auth/register → 500 Internal Server Error
SQLite Error 19: 'UNIQUE constraint failed: Users.Email'
Frontend: "An error occurred while processing the request"
```

### Causa Raíz
El compiled query `GetUserByEmailAsync` NO incluía usuarios soft-deleted en la búsqueda, pero la constraint UNIQUE de la base de datos SÍ los incluye:

**Flujo del Error**:
1. Usuario `test@example.com` se registra exitosamente
2. Usuario es eliminado (soft-delete: `IsDeleted = true`)
3. Usuario intenta registrarse nuevamente con el mismo email
4. `GetByEmailAsync()` devuelve `null` porque filtra soft-deleted ❌
5. Backend intenta `INSERT` en la DB
6. SQLite rechaza por UNIQUE constraint → 500 Error

### Código Problemático
```csharp
// ❌ CompiledQueries.cs - NO detecta soft-deleted users
public static readonly Func<ApplicationDbContext, string, Task<User?>> GetUserByEmailAsync =
    EF.CompileAsyncQuery((ApplicationDbContext context, string email) =>
        context.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));
            // Falta: .IgnoreQueryFilters()
```

### Solución Implementada

**Paso 1**: Agregar `IgnoreQueryFilters()` en CompiledQueries.cs
```csharp
// ✅ CORRECTO - Incluye soft-deleted users para validación única
public static readonly Func<ApplicationDbContext, string, Task<User?>> GetUserByEmailAsync =
    EF.CompileAsyncQuery((ApplicationDbContext context, string email) =>
        context.Users
            .AsNoTracking()
            .IgnoreQueryFilters() // Incluye usuarios con IsDeleted = true
            .FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));
```

**Paso 2**: Mejorar logging en AuthController.cs
```csharp
catch (Exception ex)
{
    _logger.LogError(ex, 
        "Unexpected error during registration for user {Email}: {ExceptionType} - {Message} - {StackTrace}", 
        dto.Email, ex.GetType().Name, ex.Message, ex.StackTrace);
    // Ahora vemos el tipo exacto de excepción y stack trace completo
}
```

**Paso 3**: Fix AuthController.cs - CreatedAtAction error
```csharp
// ❌ ANTES - Causaba error al generar URL
return CreatedAtAction(nameof(Login), result);

// ✅ DESPUÉS - Devuelve 201 directamente
return StatusCode(StatusCodes.Status201Created, result);
```

**Paso 4**: Mejorar manejo de errores en frontend
```typescript
// register.component.ts - Extract error from ProblemDetails
error: (error) => {
  let errorMsg = 'Registration failed';
  if (error?.error?.detail) {
    errorMsg = error.error.detail; // ProblemDetails.Detail
  } else if (error?.message) {
    errorMsg = error.message;
  }
  this.errorMessage.set(errorMsg);
}

// auth.service.ts
private handleRegisterError(error: HttpErrorResponse): Observable<never> {
  let errorMessage = 'Error de registro';
  
  if (error.error?.detail) {
    errorMessage = error.error.detail; // Prioridad 1: ProblemDetails
  } else if (error.error?.title) {
    errorMessage = error.error.title;
  } else if (error.error?.message) {
    errorMessage = error.error.message;
  }
  
  return throwError(() => new Error(errorMessage));
}
```

### Archivos Modificados
- ✅ `Repositories/CompiledQueries.cs`
- ✅ `Controllers/AuthController.cs`
- ✅ `Services/AuthService.cs`
- ✅ `frontend/auth/components/register/register.component.ts`
- ✅ `frontend/core/authentication/services/auth.service.ts`

### Resultado
- ✅ Email duplicado ahora devuelve **400 Bad Request** en lugar de 500
- ✅ Mensaje de error claro: "User with this email already exists"
- ✅ Frontend muestra el error correctamente al usuario
- ✅ Funciona para usuarios activos y soft-deleted

---

## Problema 2: Register No Redirige a Login

**Fecha**: 19 Noviembre 2025  
**Componente**: Frontend - RegisterComponent  
**Severidad**: Media

### Síntomas
- Usuario completa registro exitosamente (201 Created)
- Frontend no navega a `/auth/login`
- Usuario se queda en la página de registro sin feedback

### Solución Implementada

**Paso 1**: Mejorar navegación en register.component.ts
```typescript
next: (response) => {
  console.log('✅ Registration successful:', response);
  this.loading.set(false);
  
  // Navigate to login with query params
  this.router.navigate(['/auth/login'], { 
    queryParams: { 
      registered: 'true', 
      email: userData.email 
    } 
  }).then(
    success => console.log('✅ Navigation successful:', success),
    error => console.error('❌ Navigation failed:', error)
  );
}
```

**Paso 2**: Agregar mensaje de éxito en login.component.ts
```typescript
// Nuevo signal para mensaje de éxito
successMessage = signal<string | null>(null);

ngOnInit(): void {
  // Detectar si viene desde registro
  this.route.queryParams.subscribe(params => {
    if (params['registered'] === 'true') {
      this.successMessage.set('✅ Registration successful! Please sign in with your credentials.');
      
      // Pre-llenar email
      if (params['email']) {
        this.loginForm.patchValue({ email: params['email'] });
      }

      // Limpiar query params
      setTimeout(() => {
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: {},
          replaceUrl: true
        });
      }, 100);
    }
  });
}
```

**Paso 3**: Agregar UI para mensaje de éxito
```html
<!-- Success Message -->
<div *ngIf="successMessage()" 
     class="bg-green-500/20 border border-green-400 text-green-300 px-4 py-3 rounded-lg animate-fade-in">
  {{ successMessage() }}
</div>
```

### Archivos Modificados
- ✅ `frontend/auth/components/register/register.component.ts`
- ✅ `frontend/auth/components/login/login.component.ts`

### Resultado
- ✅ Usuario redirigido automáticamente a login después de registro
- ✅ Mensaje verde de confirmación visible
- ✅ Email pre-llenado en formulario de login
- ✅ UX mejorada significativamente

---

## Flujo Completo - Authentication

### Register → Login → Dashboard

```
┌─────────────────────────────────────────────────────────────┐
│ 1. REGISTER                                                 │
├─────────────────────────────────────────────────────────────┤
│ Usuario llena formulario                                    │
│ Frontend valida (password: 8 chars, upper, lower, digit,   │
│                  special char)                              │
│ POST /api/auth/register                                     │
│ Backend: GetByEmailAsync (IgnoreQueryFilters)              │
│   ├─ Email existe (incluye soft-deleted)? → 400 Bad Request│
│   └─ Email nuevo? → Crear usuario → 201 Created            │
│ Frontend: router.navigate(['/auth/login'], { queryParams })│
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ 2. LOGIN                                                    │
├─────────────────────────────────────────────────────────────┤
│ Muestra mensaje: "✅ Registration successful!"             │
│ Email pre-llenado desde query param                        │
│ Usuario ingresa password                                    │
│ POST /api/auth/login                                        │
│ Backend: Valida credenciales → Genera JWT                  │
│ Frontend: AuthState.setCurrentUser(user)                   │
│          AuthState.setAuthenticated(true)                  │
│          localStorage.setItem('token', jwt)                │
│ setTimeout(100ms) → router.navigate(['/dashboard'])        │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ 3. DASHBOARD                                                │
├─────────────────────────────────────────────────────────────┤
│ AuthGuard: canActivate()                                    │
│   └─ Verifica: authState.isAuthenticated() === true        │
│ Dashboard renderiza con datos del usuario                   │
│ Computed signals:                                           │
│   - currentUser()                                           │
│   - userName()                                              │
│   - userRole()                                              │
│   - isAdmin()                                               │
└─────────────────────────────────────────────────────────────┘
```

---

## Configuración Final

### appsettings.json (Consolidado)
```json
{
  "IpRateLimiting": {
    "GeneralRules": [
      { "Endpoint": "*", "Period": "1m", "Limit": 200 },
      { "Endpoint": "POST:/api/auth/login", "Period": "1m", "Limit": 20 },
      { "Endpoint": "POST:/api/auth/register", "Period": "1m", "Limit": 10 }
    ]
  }
}
```

**Nota**: `appsettings.Development.json` fue eliminado para consolidar configuración.

---

## Testing Checklist

### Escenario 1: Registro Nuevo Usuario
- [ ] Email único → 201 Created
- [ ] Redirige a `/auth/login?registered=true&email=...`
- [ ] Mensaje verde visible
- [ ] Email pre-llenado

### Escenario 2: Email Duplicado
- [ ] Email existente (activo) → 400 Bad Request
- [ ] Mensaje: "User with this email already exists"
- [ ] Email soft-deleted → 400 Bad Request (mismo mensaje)

### Escenario 3: Password Validation
- [ ] Falta minúscula → Error
- [ ] Falta mayúscula → Error
- [ ] Falta número → Error
- [ ] Falta carácter especial → Error
- [ ] Menos de 8 caracteres → Error
- [ ] Todo correcto → ✅ Registro exitoso

### Escenario 4: Login después de Registro
- [ ] Mensaje de éxito visible
- [ ] Email pre-llenado correctamente
- [ ] Login exitoso → Redirige a dashboard
- [ ] Dashboard muestra datos del usuario

---

## Créditos

**Grok AI**: 
- Implementación inicial del sistema de autenticación
- Login component y lógica básica
- AuthState management con Angular signals
- JWT token handling y refresh mechanism

**GitHub Copilot**:
- Debugging de problemas post-implementación
- Resolución de error 500 (soft-delete issue)
- Mejoras en manejo de errores (ProblemDetails)
- UX improvements (success message, pre-fill)
- Consolidación de configuración

---

**Última actualización**: 19 Noviembre 2025
