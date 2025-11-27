
# Prompts para Copilot - Autenticación JWT

## 🎯 AUTENTICACIÓN JWT: Login Completo con Copilot (Paso a Paso)

**OBJETIVO:** Implementar flujo completo de autenticación desde cero usando Copilot

**BACKEND YA DISPONIBLE:**
- ✅ POST /api/auth/login (AuthController.cs líneas 53-100)
- ✅ LoginDto: { email, password }
- ✅ LoginResponseDto: { token, refreshToken, expiresAt, user }
- ✅ Validación FluentValidation con LoginDtoValidator
- ✅ ProblemDetails para errores

---

## PASO 1: Crear LoginComponent con diseño moderno (25 min)

### 📝 PROMPT PARA COPILOT:

```
Crea LoginComponent Angular standalone con diseño moderno Glassmorphism/Neumorphism:

FUNCIONALIDAD:
- Formulario reactivo: email (required, email format), password (required, min 6 chars)
- Signals para loading y errorMessage
- Integración con AuthService (inyectar service)
- Redirect a /tickets después login exitoso
- Validaciones inline con feedback visual
- Animaciones suaves de entrada

DISEÑO TAILWIND CSS:
- Glassmorphism card: backdrop-blur-xl, bg-white/10, border border-white/20
- Gradient background animado con colores modernos
- Inputs con efecto glass: bg-white/5, focus:bg-white/10, placeholder opacity
- Botón con gradiente y hover effects
- Iconos para email y password (heroicons o lucide)
- Loading spinner animado cuando loading=true
- Error toast con animación slide-in
- Responsive: móvil primero, desktop optimizado
- Typography moderna: font-inter o font-outfit

UX/UI:
- Transiciones suaves (transition-all duration-300)
- Focus states claros con ring-2 ring-primary
- Disabled state visual cuando loading
- Password toggle (mostrar/ocultar)
- Link a /register con hover effect
- "¿Olvidaste tu contraseña?" link
- Auto-focus en email input al montar

ACCESIBILIDAD:
- Labels asociados con inputs
- Aria-labels descriptivos
- Error messages con role="alert"
- Navegación por teclado fluida

Archivo: src/app/features/auth/login/login.component.ts
```

**🎯 Validar contra:**
- LoginDtoValidator.cs reglas
- ProblemDetails error responses

---

## PASO 2: Crear RegisterComponent con diseño moderno (25 min)

### 📝 PROMPT PARA COPILOT:

```
Crea RegisterComponent Angular standalone con diseño moderno matching LoginComponent:

FUNCIONALIDAD:
- Formulario: email, password, confirmPassword, fullName, role (dropdown)
- Validaciones: password min 8 chars + uppercase + number, passwords match
- Custom validator para password matching
- Integración con AuthService.register()
- Success message animado y redirect a /login
- Password strength indicator visual

DISEÑO TAILWIND CSS:
- Mismo estilo Glassmorphism que LoginComponent
- Card más grande para acomodar más campos
- Role selector con custom dropdown styled
- Password strength bar con colores: red→yellow→green
- Confirmación visual cuando passwords coinciden
- Animaciones de entrada escalonadas por campo
- Success modal/toast antes de redirect

UX/UI:
- Password requirements tooltip/popover
- Real-time validation feedback
- Confirmación antes de submit
- Link a /login si ya tiene cuenta
- Multi-step form opcional (3 pasos: datos → password → confirmación)

Archivo: src/app/features/auth/register/register.component.ts
```

**🎯 Validar contra:**
- RegisterDtoValidator.cs reglas
- Password requirements del backend

---

## PASO 3: Crear interfaces TypeScript (10 min)

### 📝 PROMPT PARA COPILOT:

```
Crea interfaces TypeScript para autenticación basadas en el backend .NET TicketManagementSystem:
- LoginRequest con email y password
- LoginResponse con token, refreshToken, expiresAt, user
- UserBasicDto con id, email, fullName, role
- ProblemDetails para errores del backend

Archivo: src/app/models/auth.interface.ts
```

**🎯 Validar contra backend:**
- Controllers/AuthController.cs
- DTOs/LoginDto.cs
- DTOs/LoginResponseDto.cs

---

## PASO 4: Crear AuthService (15 min)

### 📝 PROMPT PARA COPILOT:

```
Crea AuthService Angular que integre con backend .NET:
- Consume POST /api/auth/login, POST /api/auth/register, POST /api/auth/refresh
- Métodos: login(), register(), logout(), refreshToken(), getToken()
- Signals: isAuthenticated, currentUser
- Guardar tokens en localStorage
- Manejo de errores ProblemDetails

Archivo: src/app/core/auth/auth.service.ts
```

**🎯 Validar contra:**
- AuthController.cs endpoints
- LoginResponseDto structure
- Error handling con ProblemDetails

---

## PASO 5: Crear AuthInterceptor (15 min)

### 📝 PROMPT PARA COPILOT:

```
Crea AuthInterceptor funcional (HttpInterceptorFn) que:
- Agrega Authorization: Bearer {token} header a requests autenticadas
- Excluye endpoints públicos: /api/auth/login, /api/auth/register
- Si 401: logout automático y redirect a /login
- Usa nueva API funcional de Angular

Archivo: src/app/core/interceptors/auth.interceptor.ts
```

---

## PASO 6: Crear AuthGuard (10 min)

### 📝 PROMPT PARA COPILOT:

```
Crea AuthGuard funcional (CanActivateFn) que:
- Verifica AuthService.isAuthenticated signal
- Redirect a /login si no autenticado
- Guarda returnUrl para redirect post-login

Archivo: src/app/core/guards/auth.guard.ts
```


## PASO 7: Configurar rutas y providers (10 min)

### 📝 PROMPTS PARA COPILOT:

**1. Configurar interceptor:**
```
Agrega authInterceptor a app.config.ts usando provideHttpClient con withInterceptors
```

**2. Configurar rutas:**
```
Agrega rutas en app.routes.ts:
- /login → LoginComponent
- /register → RegisterComponent
- /tickets → TicketListComponent (protegida con authGuard)

Archivo: src/app/app.routes.ts
```

---


## 📊 RESUMEN

**TIEMPO TOTAL:** ~100 minutos

**ARCHIVOS GENERADOS:**
- LoginComponent: UI con Glassmorphism
- RegisterComponent: UI con validaciones
- auth.interface.ts: Types TypeScript
- auth.service.ts: Lógica de autenticación
- auth.interceptor.ts: Bearer token
- auth.guard.ts: Protección de rutas
- app.config.ts: Configuración de providers
- app.routes.ts: Rutas de la aplicación