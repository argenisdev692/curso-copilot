# 🔐 Estándares de Seguridad JWT para BookingSystemAPI

> **Versión**: 1.0.0  
> **Fecha**: Diciembre 2025  
> **Stack**: .NET 8 API + Angular 18 SPA  
> **Cumplimiento**: OWASP API Security Top 10 (2023)

---

## 📑 Índice

1. [ADR - Decisiones de Arquitectura](#1-adr---decisiones-de-arquitectura)
2. [Análisis Comparativo](#2-análisis-comparativo)
3. [OWASP API Security Top 10 - Cobertura](#3-owasp-api-security-top-10---cobertura)
4. [Checklist Backend .NET 8](#4-checklist-backend-net-8)
5. [Checklist Frontend Angular 18](#5-checklist-frontend-angular-18)
6. [Implementación de Código](#6-implementación-de-código)
7. [Tests de Seguridad](#7-tests-de-seguridad)
8. [Configuración Azure](#8-configuración-azure)

---

## 1. ADR - Decisiones de Arquitectura

### ADR-001: Almacenamiento de Tokens

| Aspecto | Decisión |
|---------|----------|
| **Estado** | ✅ Aprobado |
| **Contexto** | El frontend SPA necesita almacenar tokens JWT de forma segura |
| **Decisión** | Usar **HttpOnly Cookies** para Access Token + Refresh Token |
| **Alternativas Rechazadas** | localStorage (vulnerable XSS), sessionStorage (pérdida en pestañas) |
| **Consecuencias** | Requiere configurar CORS y SameSite correctamente |

### ADR-002: Algoritmo de Firma JWT

| Aspecto | Decisión |
|---------|----------|
| **Estado** | ✅ Aprobado |
| **Contexto** | Necesitamos seleccionar algoritmo criptográfico para firmar tokens |
| **Decisión** | **RS256** (RSA con SHA-256) para producción |
| **Alternativas** | HS256 (simétrico, menos seguro), PS256 (más complejo) |
| **Consecuencias** | Requiere gestión de par de claves RSA, pero permite verificación sin compartir secreto |

### ADR-003: Estrategia de Refresh Token

| Aspecto | Decisión |
|---------|----------|
| **Estado** | ✅ Aprobado |
| **Contexto** | Necesitamos mantener sesiones de usuario sin comprometer seguridad |
| **Decisión** | **Refresh Token Rotation** con detección de reutilización |
| **Alternativas** | Silent refresh (problemas con ITP), Token estático (inseguro) |
| **Consecuencias** | Cada refresh genera nuevo par de tokens, el anterior se invalida |

### ADR-004: Duración de Tokens

| Aspecto | Decisión |
|---------|----------|
| **Estado** | ✅ Aprobado |
| **Decisión** | Access Token: **15 minutos**, Refresh Token: **7 días** (sliding) |
| **Justificación** | Minimiza ventana de ataque, balance UX/Seguridad |

---

## 2. Análisis Comparativo

### 2.1 Almacenamiento de Tokens

| Criterio | localStorage | sessionStorage | HttpOnly Cookie |
|----------|:------------:|:--------------:|:---------------:|
| **Protección XSS** | ❌ Vulnerable | ❌ Vulnerable | ✅ Inmune |
| **Protección CSRF** | ✅ Inmune | ✅ Inmune | ⚠️ Requiere mitigación |
| **Persistencia** | ✅ Permanente | ❌ Por sesión | ✅ Configurable |
| **Multi-pestaña** | ✅ Sí | ❌ No | ✅ Sí |
| **SSR Compatible** | ❌ No | ❌ No | ✅ Sí |
| **RECOMENDACIÓN** | ❌ | ❌ | ✅ **ELEGIDO** |

**Puntuación de Criterios (⭐ = peso)**:
- Protección XSS/CSRF: ⭐⭐⭐⭐⭐ (5/5)
- Implementación: ⭐⭐⭐ (3/5)
- Performance: ⭐⭐ (2/5)
- Compatibilidad Azure: ⭐⭐⭐⭐ (4/5)

### 2.2 Algoritmos de Firma

| Criterio | HS256 | RS256 | PS256 |
|----------|:-----:|:-----:|:-----:|
| **Tipo** | Simétrico | Asimétrico | Asimétrico |
| **Seguridad** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Performance** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |
| **Key Distribution** | ❌ Compartir secreto | ✅ Solo clave pública | ✅ Solo clave pública |
| **Azure AD B2C** | ⚠️ Limitado | ✅ Compatible | ✅ Compatible |
| **JWKS Support** | ❌ No | ✅ Sí | ✅ Sí |
| **RECOMENDACIÓN** | Dev only | ✅ **ELEGIDO** | Casos especiales |

### 2.3 Estrategias de Refresh

| Criterio | Silent Refresh | Refresh Token Rotation |
|----------|:--------------:|:---------------------:|
| **Seguridad** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Detección Compromiso** | ❌ No | ✅ Sí (reuse detection) |
| **UX** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **Compatibilidad ITP** | ❌ Problemas | ✅ Funciona |
| **Implementación** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| **RECOMENDACIÓN** | ❌ | ✅ **ELEGIDO** |

---

## 3. OWASP API Security Top 10 - Cobertura

### Estado Actual vs Recomendado

| # | Vulnerabilidad | Estado Actual | Mejora Requerida |
|---|---------------|:-------------:|------------------|
| **API1:2023** | Broken Object Level Authorization | ⚠️ Parcial | Implementar policy-based authorization |
| **API2:2023** | Broken Authentication | ✅ Implementado | Migrar a HttpOnly cookies |
| **API3:2023** | Broken Object Property Level Authorization | ⚠️ Parcial | DTOs estrictos, validación entrada |
| **API4:2023** | Unrestricted Resource Consumption | ✅ Implementado | Rate limiting configurado |
| **API5:2023** | Broken Function Level Authorization | ⚠️ Parcial | Roles y claims granulares |
| **API6:2023** | Unrestricted Access to Sensitive Business Flows | ⚠️ Parcial | Captcha, límites de operación |
| **API7:2023** | Server Side Request Forgery | ✅ N/A | No aplica directamente |
| **API8:2023** | Security Misconfiguration | ⚠️ Parcial | Headers de seguridad, CORS estricto |
| **API9:2023** | Improper Inventory Management | ⚠️ Parcial | Versionado API, documentación |
| **API10:2023** | Unsafe Consumption of APIs | ✅ N/A | Validar integraciones externas |

### Implementaciones Existentes ✅

```
✅ Password hashing con BCrypt (workFactor: 12)
✅ Timing attack prevention (random delays)
✅ Account lockout service
✅ IP-based rate limiting
✅ Refresh token rotation
✅ Token revocation
✅ Sanitización de inputs
✅ Logging seguro (email hashing)
```

---

## 4. Checklist Backend .NET 8

### 4.1 Configuración JWT

- [ ] **Migrar de HS256 a RS256** para producción
- [ ] Almacenar claves en **Azure Key Vault**
- [ ] Configurar **ClockSkew = TimeSpan.Zero**
- [ ] Validar **Issuer y Audience** estrictamente
- [ ] Implementar **JTI (JWT ID) blacklist** para revocación
- [ ] Configurar expiración corta: **15 minutos**

### 4.2 Cookies HttpOnly

- [ ] Configurar `HttpOnly = true`
- [ ] Configurar `Secure = true` (HTTPS only)
- [ ] Configurar `SameSite = Strict` o `Lax`
- [ ] Implementar **anti-CSRF tokens**
- [ ] Definir `Path` y `Domain` correctamente

### 4.3 Protección de Endpoints

- [ ] Aplicar `[Authorize]` por defecto
- [ ] Usar `[AllowAnonymous]` explícitamente
- [ ] Implementar **Policy-based authorization**
- [ ] Validar **Claims** en cada endpoint sensible
- [ ] Implementar **Resource-based authorization**

### 4.4 Headers de Seguridad

- [ ] `X-Content-Type-Options: nosniff`
- [ ] `X-Frame-Options: DENY`
- [ ] `X-XSS-Protection: 1; mode=block`
- [ ] `Strict-Transport-Security` (HSTS)
- [ ] `Content-Security-Policy`
- [ ] `Referrer-Policy: strict-origin-when-cross-origin`

### 4.5 Rate Limiting & Throttling

- [ ] Rate limit en `/auth/*` (5 requests/min)
- [ ] Rate limit global (100 requests/min)
- [ ] Exponential backoff en intentos fallidos
- [ ] IP blacklisting temporal

### 4.6 Logging & Auditoría

- [ ] Log intentos de autenticación (éxito/fallo)
- [ ] Log revocación de tokens
- [ ] Log cambios de password
- [ ] **NO loguear** tokens o passwords
- [ ] Incluir CorrelationId en todos los logs

---

## 5. Checklist Frontend Angular 18

### 5.1 Manejo de Tokens

- [ ] **NO almacenar en localStorage/sessionStorage**
- [ ] Recibir tokens via **HttpOnly cookies**
- [ ] Usar `withCredentials: true` en HttpClient
- [ ] Implementar **silent refresh** antes de expiración
- [ ] Limpiar estado en logout

### 5.2 Interceptors

- [ ] Auth interceptor para agregar credentials
- [ ] Error interceptor para manejar 401/403
- [ ] Retry logic con refresh token
- [ ] Queue de requests durante refresh

### 5.3 Protección de Rutas

- [ ] Guards para rutas autenticadas
- [ ] Redirect a login si no autenticado
- [ ] Validar roles/permisos en guards
- [ ] Lazy loading de módulos protegidos

### 5.4 Seguridad General

- [ ] Sanitizar inputs de usuario
- [ ] Usar `DomSanitizer` cuando sea necesario
- [ ] CSP meta tags
- [ ] No exponer información sensible en templates

---

## 6. Implementación de Código

### 6.1 Backend - Configuración JWT con HttpOnly Cookies

```csharp
// Program.cs - Configuración mejorada
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = GetSigningKey(builder.Configuration),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };

        // Leer token desde cookie en lugar de header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("access_token", out var token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

private static SecurityKey GetSigningKey(IConfiguration config)
{
    // Para producción: usar RSA desde Azure Key Vault
    var rsaKey = RSA.Create();
    rsaKey.ImportFromPem(config["Jwt:PrivateKey"]);
    return new RsaSecurityKey(rsaKey);
}
```

### 6.2 Backend - Cookie Configuration

```csharp
// AuthController.cs - Login con cookies
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
{
    var result = await _authService.LoginAsync(loginDto, GetClientIpAddress());
    
    if (!result.IsSuccess)
        return BadRequest(ApiResponse<object>.Fail(result.Error?.Message, CorrelationId));

    // Configurar cookies HttpOnly
    SetTokenCookies(result.Value!.Token, result.Value.RefreshToken);
    
    // Devolver solo información del usuario (sin tokens en body)
    return Ok(ApiResponse<UserDto>.Ok(result.Value.User, "Login exitoso", CorrelationId));
}

private void SetTokenCookies(string accessToken, string refreshToken)
{
    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,
        Secure = true, // Solo HTTPS
        SameSite = SameSiteMode.Strict,
        Path = "/",
        Expires = DateTimeOffset.UtcNow.AddMinutes(15)
    };
    
    Response.Cookies.Append("access_token", accessToken, cookieOptions);
    
    var refreshCookieOptions = new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/api/auth", // Solo para refresh endpoint
        Expires = DateTimeOffset.UtcNow.AddDays(7)
    };
    
    Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
}
```

### 6.3 Backend - Anti-CSRF Token

```csharp
// Middleware/AntiForgeryMiddleware.cs
public class AntiForgeryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAntiforgery _antiforgery;

    public AntiForgeryMiddleware(RequestDelegate next, IAntiforgery antiforgery)
    {
        _next = next;
        _antiforgery = antiforgery;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Generar token CSRF para requests GET
        if (HttpMethods.IsGet(context.Request.Method))
        {
            var tokens = _antiforgery.GetAndStoreTokens(context);
            context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
                new CookieOptions
                {
                    HttpOnly = false, // JavaScript debe poder leerlo
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
        }

        await _next(context);
    }
}
```

### 6.4 Frontend - Auth Service con Cookies

```typescript
// auth.service.ts - Versión segura con cookies
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  private readonly _currentUser = signal<User | null>(null);
  readonly currentUser = this._currentUser.asReadonly();
  readonly isAuthenticated = computed(() => !!this._currentUser());

  /**
   * Login - No maneja tokens directamente, el servidor los envía como cookies
   */
  login(credentials: LoginRequest): Observable<ApiResponse<UserDto>> {
    return this.http.post<ApiResponse<UserDto>>(
      `${this.apiUrl}/login`,
      credentials,
      { withCredentials: true } // ⚠️ Importante: enviar/recibir cookies
    ).pipe(
      tap(response => {
        if (response.success) {
          this._currentUser.set(response.data);
        }
      })
    );
  }

  /**
   * Refresh token - El servidor lee el refresh_token desde la cookie
   */
  refreshToken(): Observable<ApiResponse<void>> {
    return this.http.post<ApiResponse<void>>(
      `${this.apiUrl}/refresh`,
      {},
      { withCredentials: true }
    );
  }

  /**
   * Logout - El servidor invalida los tokens y limpia cookies
   */
  logout(): void {
    this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true })
      .subscribe({
        complete: () => {
          this._currentUser.set(null);
          this.router.navigate(['/auth/login']);
        }
      });
  }
}
```

### 6.5 Frontend - Interceptor con CSRF

```typescript
// auth.interceptor.ts - Con soporte CSRF
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Agregar CSRF token para requests mutables
  if (['POST', 'PUT', 'DELETE', 'PATCH'].includes(req.method)) {
    const csrfToken = getCookie('XSRF-TOKEN');
    if (csrfToken) {
      req = req.clone({
        setHeaders: { 'X-XSRF-TOKEN': csrfToken }
      });
    }
  }

  // Agregar withCredentials para todas las requests a la API
  if (req.url.includes(environment.apiUrl)) {
    req = req.clone({ withCredentials: true });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('/auth/refresh')) {
        return handleTokenRefresh(req, next, authService);
      }
      return throwError(() => error);
    })
  );
};

function getCookie(name: string): string | null {
  const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  return match ? match[2] : null;
}

function handleTokenRefresh(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn,
  authService: AuthService
): Observable<any> {
  return authService.refreshToken().pipe(
    switchMap(() => next(req)),
    catchError((refreshError) => {
      authService.logout();
      return throwError(() => refreshError);
    })
  );
}
```

### 6.6 Backend - Security Headers Middleware

```csharp
// Middleware/SecurityHeadersMiddleware.cs
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Headers de seguridad
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
        
        // HSTS para producción
        if (!context.Request.Host.Host.Contains("localhost"))
        {
            context.Response.Headers.Append(
                "Strict-Transport-Security", 
                "max-age=31536000; includeSubDomains; preload");
        }

        // CSP
        context.Response.Headers.Append(
            "Content-Security-Policy",
            "default-src 'self'; script-src 'self'; style-src 'self' 'unsafe-inline'; img-src 'self' data: https:; font-src 'self'; connect-src 'self' https://api.yourdomain.com");

        await _next(context);
    }
}
```

---

## 7. Tests de Seguridad

### 7.1 Tests Unitarios - AuthService

```csharp
// Tests/Security/AuthServiceSecurityTests.cs
[TestClass]
public class AuthServiceSecurityTests
{
    [TestMethod]
    public async Task Login_WithInvalidCredentials_ShouldNotRevealUserExistence()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        var service = CreateAuthService(mockRepo.Object);

        // Act
        var result1 = await service.LoginAsync(new LoginDto { Email = "exists@test.com", Password = "wrong" }, null);
        var result2 = await service.LoginAsync(new LoginDto { Email = "notexists@test.com", Password = "wrong" }, null);

        // Assert - Mismo mensaje para ambos casos (no revelar existencia)
        result1.Error?.Message.Should().Be(result2.Error?.Message);
    }

    [TestMethod]
    public async Task Login_WithMultipleFailedAttempts_ShouldLockAccount()
    {
        // Arrange
        var service = CreateAuthServiceWithRealLockout();
        var loginDto = new LoginDto { Email = "test@test.com", Password = "wrong" };

        // Act - 5 intentos fallidos
        for (int i = 0; i < 5; i++)
        {
            await service.LoginAsync(loginDto, "127.0.0.1");
        }
        var result = await service.LoginAsync(loginDto, "127.0.0.1");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.Code.Should().Be("AccountLocked");
    }

    [TestMethod]
    public void GenerateJwtToken_ShouldNotIncludeSensitiveData()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@test.com", PasswordHash = "hash123" };
        var service = CreateAuthService();

        // Act
        var token = service.GenerateJwtToken(user);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // Assert - No debe contener password hash
        jwt.Claims.Should().NotContain(c => c.Value.Contains("hash"));
        jwt.Claims.Should().NotContain(c => c.Type.Contains("password", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public async Task RefreshToken_WhenReused_ShouldInvalidateAllTokens()
    {
        // Arrange
        var service = CreateAuthServiceWithDb();
        var loginResult = await service.LoginAsync(new LoginDto { Email = "test@test.com", Password = "correct" }, null);
        var refreshToken = loginResult.Value!.RefreshToken;

        // Act - Usar refresh token
        await service.RefreshTokenAsync(refreshToken, null);
        
        // Intentar reusar el mismo refresh token
        var reuseResult = await service.RefreshTokenAsync(refreshToken, null);

        // Assert
        reuseResult.IsSuccess.Should().BeFalse();
        reuseResult.Error?.Code.Should().Be("TokenExpired");
    }
}
```

### 7.2 Tests de Integración

```csharp
// Tests/Integration/AuthSecurityIntegrationTests.cs
[TestClass]
public class AuthSecurityIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthSecurityIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [TestMethod]
    public async Task Login_ShouldSetHttpOnlyCookies()
    {
        // Arrange
        var loginDto = new { Email = "test@test.com", Password = "Test123!" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        // Assert
        response.Headers.TryGetValues("Set-Cookie", out var cookies);
        cookies.Should().Contain(c => c.Contains("HttpOnly"));
        cookies.Should().Contain(c => c.Contains("Secure"));
        cookies.Should().Contain(c => c.Contains("SameSite=Strict"));
    }

    [TestMethod]
    public async Task Response_ShouldIncludeSecurityHeaders()
    {
        // Act
        var response = await _client.GetAsync("/api/health");

        // Assert
        response.Headers.Should().Contain(h => h.Key == "X-Content-Type-Options");
        response.Headers.Should().Contain(h => h.Key == "X-Frame-Options");
        response.Headers.Should().Contain(h => h.Key == "X-XSS-Protection");
    }

    [TestMethod]
    public async Task RateLimiting_ShouldBlockExcessiveRequests()
    {
        // Arrange
        var loginDto = new { Email = "test@test.com", Password = "wrong" };

        // Act - Enviar más de 5 requests en 60 segundos
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _client.PostAsJsonAsync("/api/auth/login", loginDto));
        var responses = await Task.WhenAll(tasks);

        // Assert - Al menos una debe ser 429
        responses.Should().Contain(r => r.StatusCode == HttpStatusCode.TooManyRequests);
    }

    [TestMethod]
    public async Task CSRF_ShouldBeValidated()
    {
        // Arrange
        var loginDto = new { Email = "test@test.com", Password = "Test123!" };
        
        // Act - Request sin CSRF token
        var requestWithoutCsrf = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login")
        {
            Content = JsonContent.Create(loginDto)
        };
        
        // No incluir el header X-XSRF-TOKEN
        var response = await _client.SendAsync(requestWithoutCsrf);

        // Assert - Debería fallar si CSRF está habilitado
        // (Nota: ajustar según implementación específica)
    }
}
```

### 7.3 Tests de Penetración Automatizados

```csharp
// Tests/Security/PenetrationTests.cs
[TestClass]
public class PenetrationTests
{
    [TestMethod]
    public async Task SqlInjection_ShouldBeBlocked()
    {
        // Arrange
        var maliciousInputs = new[]
        {
            "'; DROP TABLE Users; --",
            "1' OR '1'='1",
            "admin'--",
            "1; SELECT * FROM Users"
        };

        // Act & Assert
        foreach (var input in maliciousInputs)
        {
            var loginDto = new { Email = input, Password = input };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            
            // No debe causar error de servidor
            response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
        }
    }

    [TestMethod]
    public async Task XSS_InputShouldBeSanitized()
    {
        // Arrange
        var xssPayloads = new[]
        {
            "<script>alert('xss')</script>",
            "javascript:alert(1)",
            "<img src=x onerror=alert(1)>",
            "{{constructor.constructor('alert(1)')()}}"
        };

        // Act & Assert
        foreach (var payload in xssPayloads)
        {
            var registerDto = new 
            { 
                Email = "test@test.com", 
                Password = "Test123!",
                FirstName = payload,
                LastName = payload
            };
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            var content = await response.Content.ReadAsStringAsync();
            
            // El payload no debe estar sin sanitizar en la respuesta
            content.Should().NotContain("<script>");
        }
    }

    [TestMethod]
    public async Task JWT_TamperingShouldBeDetected()
    {
        // Arrange - Obtener token válido
        var loginResponse = await LoginAsTestUser();
        var validToken = GetTokenFromCookies(loginResponse);
        
        // Act - Modificar el token (cambiar un carácter)
        var tamperedToken = validToken.Substring(0, validToken.Length - 5) + "XXXXX";
        
        // Intentar usar el token modificado
        _client.DefaultRequestHeaders.Add("Cookie", $"access_token={tamperedToken}");
        var response = await _client.GetAsync("/api/bookings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
```

---

## 8. Configuración Azure

### 8.1 Azure App Service Settings

```json
{
  "Jwt": {
    "Issuer": "https://your-app.azurewebsites.net",
    "Audience": "https://your-app.azurewebsites.net",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "ApplicationInsights": {
    "ConnectionString": "@Microsoft.KeyVault(SecretUri=https://your-keyvault.vault.azure.net/secrets/AppInsightsConnectionString)"
  }
}
```

### 8.2 Azure Key Vault para Claves RSA

```bash
# Crear par de claves RSA
openssl genrsa -out private.pem 2048
openssl rsa -in private.pem -pubout -out public.pem

# Subir a Key Vault
az keyvault secret set --vault-name "your-keyvault" \
  --name "JwtPrivateKey" \
  --file private.pem

az keyvault secret set --vault-name "your-keyvault" \
  --name "JwtPublicKey" \
  --file public.pem
```

### 8.3 Azure AD B2C (Opcional)

```csharp
// Program.cs - Configuración Azure AD B2C
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

// appsettings.json
{
  "AzureAdB2C": {
    "Instance": "https://yourtenant.b2clogin.com/",
    "Domain": "yourtenant.onmicrosoft.com",
    "ClientId": "your-client-id",
    "SignUpSignInPolicyId": "B2C_1_SignUpSignIn"
  }
}
```

### 8.4 Configuración CORS para Azure

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
                "https://your-frontend.azurestaticapps.net",
                "https://your-custom-domain.com")
            .AllowCredentials() // ⚠️ Necesario para cookies
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Correlation-Id");
    });
});
```

---

## 📋 Resumen de Recomendaciones Prioritarias

### 🔴 Alta Prioridad (Implementar Inmediatamente)

1. **Migrar tokens de localStorage a HttpOnly Cookies**
2. **Implementar protección CSRF**
3. **Agregar headers de seguridad**
4. **Migrar de HS256 a RS256 para producción**

### 🟡 Media Prioridad (Próximo Sprint)

1. Implementar JTI blacklist para revocación inmediata
2. Configurar Azure Key Vault para secretos
3. Agregar tests de seguridad automatizados
4. Implementar policy-based authorization

### 🟢 Baja Prioridad (Roadmap)

1. Integrar Azure AD B2C
2. Implementar MFA
3. Agregar Audit logging a base de datos
4. Implementar CAPTCHA en endpoints sensibles

---

> **Nota**: Esta guía debe revisarse y actualizarse periódicamente conforme evolucionen las amenazas y mejores prácticas de seguridad.
