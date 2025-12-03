# 🔐 API de Autenticación - Ejemplos JSON para Swagger

## Descripción General

Esta API implementa autenticación JWT con soporte para **Refresh Tokens**, **Rate Limiting** y configuración segura de CORS.

---

## 🚀 Endpoints de Autenticación

### 1. Registrar Usuario

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
    "email": "nuevo.usuario@ejemplo.com",
    "password": "MiContraseña123!",
    "confirmPassword": "MiContraseña123!",
    "firstName": "Juan",
    "lastName": "Pérez"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJudWV2by51c3VhcmlvQGVqZW1wbG8uY29tIiwiZ2l2ZW5fbmFtZSI6Ikp1YW4iLCJmYW1pbHlfbmFtZSI6IlDDqXJleiIsImp0aSI6ImE1YjZjN2Q4LWU5ZjAtMTIzNC01Njc4LTlhYmNkZWYwMTIzNCIsImlhdCI6MTczMzE1MDAwMCwiZXhwIjoxNzMzMTUzNjAwfQ.xyz123",
        "refreshToken": "dGhpcyBpcyBhIHNlY3VyZSByZWZyZXNoIHRva2VuIGdlbmVyYXRlZCB3aXRoIGNyeXB0b2dyYXBoaWMgcmFuZG9tbmVzcw==",
        "expiresAt": "2025-12-02T15:30:00Z",
        "user": {
            "id": 1,
            "email": "nuevo.usuario@ejemplo.com",
            "firstName": "Juan",
            "lastName": "Pérez"
        }
    }
}
```

**Validaciones de Contraseña:**
- ✅ Mínimo 8 caracteres
- ✅ Al menos una letra mayúscula
- ✅ Al menos una letra minúscula
- ✅ Al menos un número
- ✅ Al menos un carácter especial

---

### 2. Login

**Endpoint:** `POST /api/auth/login`

**Request Body:**
```json
{
    "email": "usuario@ejemplo.com",
    "password": "MiContraseña123!"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        "refreshToken": "dGhpcyBpcyBhIHNlY3VyZSByZWZyZXNoIHRva2VuLi4u",
        "expiresAt": "2025-12-02T15:30:00Z",
        "user": {
            "id": 1,
            "email": "usuario@ejemplo.com",
            "firstName": "Juan",
            "lastName": "Pérez"
        }
    }
}
```

**Error de Credenciales (400 Bad Request):**
```json
{
    "success": false,
    "message": "Credenciales inválidas.",
    "data": null
}
```

---

### 3. Refresh Token

**Endpoint:** `POST /api/auth/refresh`

**Request Body:**
```json
{
    "refreshToken": "dGhpcyBpcyBhIHNlY3VyZSByZWZyZXNoIHRva2VuLi4u"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...NUEVO_TOKEN",
        "refreshToken": "bnVldm8gcmVmcmVzaCB0b2tlbiBnZW5lcmFkbw==",
        "expiresAt": "2025-12-02T16:30:00Z",
        "user": {
            "id": 1,
            "email": "usuario@ejemplo.com",
            "firstName": "Juan",
            "lastName": "Pérez"
        }
    }
}
```

> ⚠️ **Nota:** El refresh token anterior es automáticamente revocado (rotación de tokens).

---

### 4. Revocar Token (Logout)

**Endpoint:** `POST /api/auth/revoke`

**Request Body:**
```json
{
    "refreshToken": "dGhpcyBpcyBhIHNlY3VyZSByZWZyZXNoIHRva2VuLi4u"
}
```

**Respuesta Exitosa (200 OK):**
```json
{
    "success": true,
    "message": "Token revocado exitosamente.",
    "data": true
}
```

---

## 🔒 Uso del Token en Swagger

1. Ejecuta el endpoint de **Login** o **Register**
2. Copia el valor del campo `token` de la respuesta
3. Haz clic en el botón **"Authorize"** (🔓) en la parte superior de Swagger
4. Ingresa: `Bearer <tu_token_aquí>`
5. Haz clic en **"Authorize"**

**Ejemplo:**
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIi...
```

---

## ⚡ Rate Limiting

Los endpoints de autenticación tienen protección contra ataques de fuerza bruta:

| Entorno     | Límite | Ventana | Cola |
|-------------|--------|---------|------|
| Desarrollo  | 10 req | 60 seg  | 5    |
| Producción  | 5 req  | 60 seg  | 2    |

**Error (429 Too Many Requests):**
```json
{
    "type": "https://tools.ietf.org/html/rfc6585#section-4",
    "title": "Too Many Requests",
    "status": 429
}
```

---

## 🔧 Configuración de Producción

### Variables de Entorno Recomendadas

```bash
# JWT Key segura (mínimo 32 caracteres)
Jwt__Key=TuClaveSecretaSuperSeguraDeAlMenos32CaracteresParaProduccion!

# Connection String SQL Server
ConnectionStrings__DefaultConnection=Server=tu-servidor.database.windows.net;Database=BookingSystemDb;User Id=tu-usuario;Password=tu-password;TrustServerCertificate=True;

# CORS Origins permitidos
Cors__AllowedOrigins__0=https://tudominio.com
Cors__AllowedOrigins__1=https://app.tudominio.com
```

### User Secrets (Desarrollo)

```bash
dotnet user-secrets set "Jwt:Key" "TuClaveSecretaDeDesarrolloDeAlMenos32Caracteres!"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=BookingSystemDb;Trusted_Connection=True;"
```

---

## 📊 Claims del JWT Token

El token JWT contiene los siguientes claims:

| Claim | Descripción | Ejemplo |
|-------|-------------|---------|
| `sub` | ID del usuario | `"1"` |
| `email` | Correo electrónico | `"usuario@ejemplo.com"` |
| `given_name` | Nombre | `"Juan"` |
| `family_name` | Apellido | `"Pérez"` |
| `jti` | ID único del token | `"a5b6c7d8-..."` |
| `iat` | Fecha de emisión (Unix timestamp) | `1733150000` |
| `exp` | Fecha de expiración (Unix timestamp) | `1733153600` |

---

## 🛡️ Seguridad Implementada

- ✅ **BCrypt** para hash de contraseñas
- ✅ **JWT con HS256** para tokens de acceso
- ✅ **Refresh Token Rotation** - tokens de refresco de un solo uso
- ✅ **Rate Limiting** en endpoints de autenticación
- ✅ **CORS restrictivo** en producción
- ✅ **Validación robusta** con FluentValidation
- ✅ **Logging estructurado** con Serilog
- ✅ **ClockSkew = 0** - Sin tolerancia en expiración de tokens
