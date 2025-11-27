# ⏱️ Rate Limiting Configuration Guide

## 📊 Configuración Actual

### **Development Environment** (`appsettings.Development.json`)

Límites **muy permisivos** para facilitar desarrollo y pruebas:

| Endpoint | Límite | Período | Uso |
|----------|--------|---------|-----|
| **General** | 1000 requests | 1 minuto | Todos los endpoints no especificados |
| **POST /api/auth/login** | 100 requests | 1 minuto | Autenticación |
| **POST /api/auth/register** | 50 requests | 1 minuto | Registro de usuarios |
| **POST /api/auth/logout** | 100 requests | 1 minuto | Cierre de sesión |

### **Production Environment** (`appsettings.json`)

Límites **moderados** para equilibrar usabilidad y seguridad:

| Endpoint | Límite | Período | Uso |
|----------|--------|---------|-----|
| **General** | 200 requests | 1 minuto | Todos los endpoints no especificados |
| **POST /api/auth/login** | 20 requests | 1 minuto | Protección contra brute force |
| **POST /api/auth/register** | 10 requests | 1 minuto | Prevenir spam de registros |
| **POST /api/auth/logout** | 30 requests | 1 minuto | Cierre de sesión |
| **GET /api/tickets*** | 100 requests | 1 minuto | Consultas de tickets |
| **POST /api/tickets*** | 30 requests | 1 minuto | Creación de tickets |

---

## 🔒 Recomendaciones para Producción Estricta

Para entornos de producción con alta seguridad, considera estos límites más restrictivos:

```json
{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      },
      {
        "Endpoint": "POST:/api/auth/login",
        "Period": "5m",
        "Limit": 5,
        "Comment": "Solo 5 intentos de login cada 5 minutos (previene brute force)"
      },
      {
        "Endpoint": "POST:/api/auth/register",
        "Period": "1h",
        "Limit": 3,
        "Comment": "Máximo 3 registros por hora por IP (previene spam)"
      },
      {
        "Endpoint": "POST:/api/auth/refresh",
        "Period": "1m",
        "Limit": 10
      },
      {
        "Endpoint": "GET:/api/tickets*",
        "Period": "1m",
        "Limit": 30
      },
      {
        "Endpoint": "POST:/api/tickets*",
        "Period": "1m",
        "Limit": 10
      },
      {
        "Endpoint": "PUT:/api/tickets*",
        "Period": "1m",
        "Limit": 15
      },
      {
        "Endpoint": "DELETE:/api/tickets*",
        "Period": "1m",
        "Limit": 5
      }
    ]
  }
}
```

---

## 📝 Configuración por Casos de Uso

### **1. Aplicación Pública (SaaS)**
```json
{
  "POST:/api/auth/login": { "Period": "5m", "Limit": 5 },
  "POST:/api/auth/register": { "Period": "1h", "Limit": 3 },
  "GET:*": { "Period": "1m", "Limit": 30 },
  "POST:*": { "Period": "1m", "Limit": 10 }
}
```

### **2. Aplicación Interna (Corporativa)**
```json
{
  "POST:/api/auth/login": { "Period": "1m", "Limit": 20 },
  "POST:/api/auth/register": { "Period": "1d", "Limit": 50 },
  "GET:*": { "Period": "1m", "Limit": 100 },
  "POST:*": { "Period": "1m", "Limit": 50 }
}
```

### **3. API Pública con Plan Gratuito**
```json
{
  "GeneralRules": [
    { "Endpoint": "*", "Period": "1h", "Limit": 100 },
    { "Endpoint": "*", "Period": "1d", "Limit": 1000 }
  ],
  "ClientRules": {
    "premium-client": [
      { "Endpoint": "*", "Period": "1h", "Limit": 10000 }
    ]
  }
}
```

---

## 🛠️ Configuración Avanzada

### **Rate Limiting por Cliente (Client-Based)**

```json
{
  "ClientRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "ClientIdHeader": "X-ClientId",
    "ClientRules": {
      "client-free": [
        { "Endpoint": "*", "Period": "1h", "Limit": 100 }
      ],
      "client-premium": [
        { "Endpoint": "*", "Period": "1h", "Limit": 10000 }
      ],
      "client-enterprise": [
        { "Endpoint": "*", "Period": "1h", "Limit": 100000 }
      ]
    }
  }
}
```

### **Whitelist de IPs**

```json
{
  "IpRateLimiting": {
    "IpWhitelist": [
      "127.0.0.1",
      "::1",
      "192.168.*"
    ]
  }
}
```

### **Rate Limiting por Endpoint Regex**

```json
{
  "GeneralRules": [
    {
      "Endpoint": "GET:/api/tickets/*/comments",
      "Period": "1m",
      "Limit": 50
    },
    {
      "Endpoint": "POST:/api/tickets/*/comments",
      "Period": "1m",
      "Limit": 20
    }
  ]
}
```

---

## 🎯 Períodos Disponibles

| Período | Descripción |
|---------|-------------|
| `1s` | 1 segundo |
| `10s` | 10 segundos |
| `1m` | 1 minuto |
| `5m` | 5 minutos |
| `15m` | 15 minutos |
| `30m` | 30 minutos |
| `1h` | 1 hora |
| `12h` | 12 horas |
| `1d` | 1 día |
| `7d` | 7 días |

---

## 🚨 Códigos de Error HTTP

### **429 Too Many Requests**

**Response Headers:**
```
X-Rate-Limit-Limit: 100
X-Rate-Limit-Remaining: 0
X-Rate-Limit-Reset: 2025-11-19T16:30:00Z
Retry-After: 30
```

**Response Body (ProblemDetails):**
```json
{
  "type": "https://httpstatuses.com/429",
  "title": "Too Many Requests",
  "status": 429,
  "detail": "Rate limit exceeded. Try again in 30 seconds.",
  "instance": "/api/auth/login",
  "extensions": {
    "traceId": "00-1234567890abcdef-1234567890abcdef-00"
  }
}
```

---

## 📊 Monitoring y Métricas

### **Logs a Monitorear**

```csharp
// En Program.cs o Middleware personalizado
app.Use(async (context, next) =>
{
    var endpoint = context.Request.Path;
    var method = context.Request.Method;
    var ipAddress = context.Connection.RemoteIpAddress?.ToString();
    
    _logger.LogInformation(
        "Request: {Method} {Endpoint} from {IP}",
        method, endpoint, ipAddress
    );
    
    await next();
    
    if (context.Response.StatusCode == 429)
    {
        _logger.LogWarning(
            "Rate limit exceeded: {Method} {Endpoint} from {IP}",
            method, endpoint, ipAddress
        );
    }
});
```

### **Métricas Recomendadas**

1. **Requests por minuto por IP**
2. **Porcentaje de requests bloqueados (429)**
3. **Endpoints más afectados por rate limiting**
4. **IPs que más exceden límites**
5. **Patrones de tráfico por hora del día**

---

## 🔧 Testing del Rate Limiting

### **Usando cURL**

```bash
# Test básico
for i in {1..10}; do
  curl -X POST http://localhost:5201/api/auth/login \
    -H "Content-Type: application/json" \
    -d '{"email":"test@example.com","password":"Test@123"}' \
    -w "\nStatus: %{http_code}\n" \
    -s -o /dev/null
done

# Con delay entre requests
for i in {1..100}; do
  curl -X GET http://localhost:5201/api/tickets \
    -H "Authorization: Bearer YOUR_TOKEN" \
    -w "\nRequest $i - Status: %{http_code}\n" \
    -s -o /dev/null
  sleep 0.5
done
```

### **Usando PowerShell**

```powershell
# Test de rate limiting
1..20 | ForEach-Object {
    $response = Invoke-WebRequest `
        -Uri "http://localhost:5201/api/auth/login" `
        -Method POST `
        -ContentType "application/json" `
        -Body '{"email":"test@example.com","password":"Test@123"}' `
        -ErrorAction SilentlyContinue
    
    Write-Host "Request $_ - Status: $($response.StatusCode)"
    Start-Sleep -Milliseconds 100
}
```

---

## 🎓 Best Practices

### ✅ **DO:**
1. Usar límites más permisivos en desarrollo
2. Monitorear métricas de rate limiting en producción
3. Incluir headers de rate limit en las responses
4. Documentar los límites en tu API docs
5. Implementar whitelist para IPs de confianza
6. Usar diferentes límites por tipo de usuario/plan
7. Cachear responses cuando sea posible para reducir carga

### ❌ **DON'T:**
1. Usar los mismos límites en dev y prod
2. Establecer límites demasiado bajos sin testing
3. Olvidar notificar a los clientes sobre límites
4. Ignorar métricas de requests bloqueados
5. Aplicar rate limiting a health checks
6. Bloquear IPs permanentemente sin revisión manual

---

## 📚 Referencias

- [AspNetCoreRateLimit Documentation](https://github.com/stefanprodan/AspNetCoreRateLimit)
- [RFC 6585 - HTTP Status Code 429](https://tools.ietf.org/html/rfc6585)
- [Best Practices for API Rate Limiting](https://cloud.google.com/architecture/rate-limiting-strategies-techniques)

---

**Última actualización:** 19 de Noviembre, 2025
