# ✅ Checklist de Implementación - Seguridad JWT

> **Proyecto**: BookingSystemAPI  
> **Última actualización**: Diciembre 2025  
> **Responsable**: [Nombre del desarrollador]

---

## 🔴 Fase 1: Crítico (Implementar Inmediatamente)

### Backend .NET 8

- [ ] **JWT-B001**: Migrar almacenamiento de tokens a HttpOnly Cookies
  - [ ] Actualizar `AuthController.cs` para setear cookies en login
  - [ ] Configurar `CookieOptions` con `HttpOnly=true`, `Secure=true`, `SameSite=Strict`
  - [ ] Actualizar endpoint de refresh para leer cookie
  - [ ] Actualizar endpoint de logout para limpiar cookies

- [ ] **JWT-B002**: Implementar lectura de token desde cookie en JWT Bearer
  - [ ] Agregar evento `OnMessageReceived` en configuración JWT
  - [ ] Verificar que el token se extraiga de la cookie `access_token`

- [ ] **JWT-B003**: Configurar protección CSRF
  - [ ] Registrar `AntiForgeryMiddleware` en pipeline
  - [ ] Validar header `X-XSRF-TOKEN` en requests mutables
  - [ ] Generar token CSRF en responses GET

- [ ] **JWT-B004**: Agregar Security Headers Middleware
  - [ ] Verificar middleware `SecurityHeadersMiddleware` está activo
  - [ ] Confirmar headers: `X-Content-Type-Options`, `X-Frame-Options`, `CSP`

### Frontend Angular 18

- [ ] **JWT-F001**: Eliminar uso de localStorage para tokens
  - [ ] Remover `localStorage.setItem` para tokens
  - [ ] Remover `localStorage.getItem` para tokens
  - [ ] Actualizar `environment.ts` para remover keys de tokens

- [ ] **JWT-F002**: Actualizar AuthService para cookies
  - [ ] Agregar `withCredentials: true` a todas las requests HTTP
  - [ ] Remover lógica de almacenamiento de tokens
  - [ ] Implementar verificación de estado de auth via endpoint `/auth/me`

- [ ] **JWT-F003**: Actualizar Interceptor
  - [ ] Agregar envío de token CSRF en header `X-XSRF-TOKEN`
  - [ ] Configurar `withCredentials: true` globalmente
  - [ ] Manejar refresh con cookies

---

## 🟡 Fase 2: Alta Prioridad (Próximo Sprint)

### Backend

- [ ] **JWT-B005**: Migrar algoritmo de HS256 a RS256
  - [ ] Generar par de claves RSA (2048 bits mínimo)
  - [ ] Actualizar `GenerateJwtToken()` para usar RSA
  - [ ] Actualizar validación de tokens para clave pública
  - [ ] Configurar Key Vault para almacenar claves

- [ ] **JWT-B006**: Implementar JTI Blacklist
  - [ ] Agregar campo `Jti` al token
  - [ ] Crear tabla/cache para JTIs revocados
  - [ ] Validar JTI en middleware de autenticación

- [ ] **JWT-B007**: Configurar Azure Key Vault
  - [ ] Crear Key Vault en Azure
  - [ ] Subir claves RSA como secretos
  - [ ] Actualizar Program.cs para leer de Key Vault
  - [ ] Configurar Managed Identity para App Service

- [ ] **JWT-B008**: Implementar Policy-Based Authorization
  - [ ] Definir políticas (ej: "CanManageBookings", "CanViewReports")
  - [ ] Crear handlers para cada política
  - [ ] Aplicar `[Authorize(Policy = "...")]` en controllers

### Frontend

- [ ] **JWT-F004**: Implementar refresh silencioso
  - [ ] Agregar timer para refresh antes de expiración
  - [ ] Manejar cola de requests durante refresh
  - [ ] Implementar retry logic en interceptor

- [ ] **JWT-F005**: Guards con validación de roles
  - [ ] Actualizar AuthGuard para verificar claims
  - [ ] Implementar RoleGuard para rutas específicas
  - [ ] Agregar CanActivate basado en permisos

---

## 🟢 Fase 3: Mejoras (Roadmap)

### Backend

- [ ] **JWT-B009**: Integrar Azure AD B2C (opcional)
  - [ ] Configurar tenant B2C
  - [ ] Agregar `AddMicrosoftIdentityWebApi`
  - [ ] Actualizar policies de usuario

- [ ] **JWT-B010**: Implementar Audit Logging
  - [ ] Crear tabla `AuditLog`
  - [ ] Log eventos de auth (login, logout, refresh, revoke)
  - [ ] Incluir IP, User-Agent, Timestamp

- [ ] **JWT-B011**: Agregar CAPTCHA/reCAPTCHA
  - [ ] Integrar reCAPTCHA v3 en endpoints sensibles
  - [ ] Validar score en servidor
  - [ ] Aplicar a register y login después de N intentos

- [ ] **JWT-B012**: Implementar MFA
  - [ ] Agregar soporte para TOTP
  - [ ] Crear endpoints para setup y verificación
  - [ ] Integrar con authenticator apps

### Frontend

- [ ] **JWT-F006**: Implementar timeout de sesión
  - [ ] Detectar inactividad del usuario
  - [ ] Mostrar warning antes de expiración
  - [ ] Logout automático por inactividad

- [ ] **JWT-F007**: Agregar indicador de sesión
  - [ ] Mostrar tiempo restante de sesión
  - [ ] Opción de extender sesión
  - [ ] Notificación de sesión expirada

---

## 📊 Tests de Seguridad

### Automatizados

- [ ] **TEST-001**: Tests de headers de seguridad
- [ ] **TEST-002**: Tests de rate limiting
- [ ] **TEST-003**: Tests de account lockout
- [ ] **TEST-004**: Tests de token manipulation
- [ ] **TEST-005**: Tests de SQL injection
- [ ] **TEST-006**: Tests de XSS
- [ ] **TEST-007**: Tests de CSRF

### Manuales/Pen Testing

- [ ] **PENTEST-001**: Ejecutar OWASP ZAP scan
- [ ] **PENTEST-002**: Verificar con Burp Suite
- [ ] **PENTEST-003**: Validar con security headers analyzer

---

## 📝 Notas de Implementación

### Orden de Despliegue

1. **Backend primero**: Actualizar para soportar tanto header como cookie
2. **Frontend segundo**: Migrar gradualmente a cookies
3. **Cleanup**: Remover soporte de header Authorization una vez completada migración

### Compatibilidad hacia atrás

Durante la migración, el backend debe aceptar tokens desde:
1. Cookie `access_token` (nuevo)
2. Header `Authorization: Bearer ...` (legacy, deprecar después)

### Variables de entorno requeridas

```bash
# Producción
JWT_PRIVATE_KEY=<Base64 encoded RSA private key>
JWT_PUBLIC_KEY=<Base64 encoded RSA public key>
AZURE_KEY_VAULT_URI=https://your-keyvault.vault.azure.net

# Development
JWT_KEY=<min 32 chars secret for HS256>
```

---

## ✅ Criterios de Aceptación

Cada item se considera completo cuando:

1. ✅ Código implementado y revisado
2. ✅ Tests unitarios pasan
3. ✅ Tests de integración pasan
4. ✅ Documentación actualizada
5. ✅ Revisión de seguridad completada

---

> **Próxima revisión**: [Fecha]  
> **Aprobado por**: [Nombre del Tech Lead/Security Officer]
