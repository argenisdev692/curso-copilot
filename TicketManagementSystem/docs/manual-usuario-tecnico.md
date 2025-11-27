# Manual Técnico para Usuarios - Ticket Management System

## 📋 Introducción

Este manual técnico proporciona una guía completa para usuarios del Sistema de Gestión de Tickets (Ticket Management System). El sistema permite gestionar tickets de soporte técnico de manera eficiente, con funcionalidades de registro, autenticación, creación y seguimiento de tickets.

### 🏗️ Arquitectura del Sistema

El sistema está compuesto por:
- **Backend**: API RESTful desarrollada en ASP.NET Core con autenticación JWT
- **Frontend**: Aplicación web en Angular con interfaz responsive
- **Base de Datos**: SQL Server con Entity Framework Core

### 🔐 Flujo de Usuario

El flujo principal del usuario incluye:
1. **Registro** de cuenta
2. **Inicio de sesión** (Login)
3. **Creación de tickets**
4. **Consulta del historial** de tickets

---

## 1. 📝 Registro de Usuario

### Descripción
El registro permite crear una nueva cuenta en el sistema. Los usuarios pueden registrarse con diferentes roles (User, Agent, Admin).

### Endpoint API
```
POST /api/auth/register
```

### Datos Requeridos
```json
{
  "email": "usuario@ejemplo.com",
  "password": "contraseña_segura",
  "fullName": "Nombre Completo",
  "role": "User"
}
```

### Pasos en la Interfaz Web
1. Acceder a la página de registro
2. Completar el formulario con:
   - Correo electrónico (único en el sistema)
   - Contraseña (mínimo 8 caracteres)
   - Nombre completo
   - Rol (por defecto "User")
3. Hacer clic en "Registrarse"
4. Recibir confirmación y token de acceso

### Respuesta Exitosa
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "email": "usuario@ejemplo.com",
    "fullName": "Nombre Completo",
    "role": "User",
    "isActive": true,
    "createdAt": "2025-11-25T10:00:00Z"
  }
}
```

### Consideraciones Técnicas
- El email debe ser único en el sistema
- La contraseña se almacena hasheada con algoritmos seguros
- El registro automático activa la cuenta (isActive: true)

---

## 2. 🔑 Inicio de Sesión (Login)

### Descripción
El login autentica al usuario y proporciona tokens JWT para acceder a recursos protegidos.

### Endpoint API
```
POST /api/auth/login
```

### Datos Requeridos
```json
{
  "email": "usuario@ejemplo.com",
  "password": "contraseña_segura"
}
```

### Pasos en la Interfaz Web
1. Acceder a la página de login
2. Ingresar correo electrónico y contraseña
3. Hacer clic en "Iniciar Sesión"
4. Recibir tokens de acceso y ser redirigido al dashboard

### Respuesta Exitosa
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "email": "usuario@ejemplo.com",
    "fullName": "Nombre Completo",
    "role": "User"
  }
}
```

### Manejo de Tokens
- **Access Token**: Token de corta duración (1 hora) para acceder a APIs
- **Refresh Token**: Token de larga duración para renovar el access token
- Los tokens se almacenan automáticamente en localStorage del navegador

### Endpoint de Refresh
```
POST /api/auth/refresh
```
```json
{
  "refreshToken": "token_de_refresh"
}
```

---

## 3. 🎫 Creación de Tickets

### Descripción
Los usuarios pueden crear tickets para reportar problemas, solicitar funcionalidades o hacer consultas.

### Endpoint API
```
POST /api/tickets
```

### Datos Requeridos
```json
{
  "title": "Título del ticket",
  "description": "Descripción detallada del problema",
  "priority": "Low|Medium|High|Critical"
}
```

### Pasos en la Interfaz Web
1. Desde el dashboard, hacer clic en "Crear Nuevo Ticket"
2. Completar el formulario:
   - **Título**: Breve descripción del problema (máx. 100 caracteres)
   - **Descripción**: Detalles completos (máx. 1000 caracteres)
   - **Prioridad**: Low, Medium, High, Critical
3. Hacer clic en "Crear Ticket"

### Respuesta Exitosa
```json
{
  "id": 1,
  "title": "Título del ticket",
  "description": "Descripción detallada",
  "status": "Open",
  "priority": "Medium",
  "createdBy": {
    "id": 1,
    "fullName": "Nombre Completo"
  },
  "assignedTo": null,
  "createdAt": "2025-11-25T10:00:00Z",
  "updatedAt": "2025-11-25T10:00:00Z"
}
```

### Estados de Ticket
- **Open**: Ticket recién creado, esperando asignación
- **InProgress**: En proceso de resolución
- **Resolved**: Problema resuelto
- **Closed**: Ticket cerrado definitivamente

### Prioridades
- **Low**: Problema menor, no urgente
- **Medium**: Problema moderado
- **High**: Problema importante que requiere atención
- **Critical**: Problema crítico que afecta operaciones

---

## 4. 📊 Historial de Tickets

### Descripción
El historial muestra todos los cambios realizados en un ticket, incluyendo modificaciones de estado, asignaciones y comentarios.

### Endpoint API
```
GET /api/tickets/{id}/history?page=1&pageSize=20
```

### Parámetros de Consulta
- `page`: Número de página (por defecto: 1)
- `pageSize`: Elementos por página (por defecto: 20)
- `startDate`: Fecha de inicio (formato: YYYY-MM-DD)
- `endDate`: Fecha de fin (formato: YYYY-MM-DD)
- `changedBy`: ID del usuario que realizó el cambio

### Pasos en la Interfaz Web
1. Desde la lista de tickets, seleccionar un ticket específico
2. Hacer clic en la pestaña "Historial" o "History"
3. Visualizar la línea de tiempo de cambios
4. Usar filtros para buscar cambios específicos

### Estructura del Historial
```json
{
  "data": [
    {
      "id": 1,
      "ticketId": 1,
      "changedBy": {
        "id": 2,
        "fullName": "Agente de Soporte"
      },
      "changes": [
        {
          "fieldName": "status",
          "oldValue": "Open",
          "newValue": "InProgress",
          "changedAt": "2025-11-25T11:00:00Z"
        },
        {
          "fieldName": "assignedTo",
          "oldValue": null,
          "newValue": "Agente de Soporte",
          "changedAt": "2025-11-25T11:00:00Z"
        }
      ],
      "changedAt": "2025-11-25T11:00:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 20
}
```

### Campos Rastreados en el Historial
- **Status**: Cambios de estado del ticket
- **Priority**: Modificaciones de prioridad
- **AssignedTo**: Asignaciones a agentes
- **Title**: Cambios en el título
- **Description**: Modificaciones en la descripción
- **Comments**: Nuevos comentarios agregados

---

## 🔧 Configuración y Requisitos Técnicos

### Requisitos del Sistema
- **Navegador**: Chrome 90+, Firefox 88+, Safari 14+, Edge 90+
- **Conexión**: HTTPS obligatorio para producción
- **JavaScript**: Habilitado (requerido para Angular)

### Variables de Entorno
```bash
# Backend
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection="Server=.;Database=TicketManagement;Trusted_Connection=True;"
JWT__Key="tu_clave_secreta_jwt"
JWT__Issuer="TicketManagementSystem"
JWT__Audience="TicketManagementUsers"

# Frontend
API_BASE_URL=https://api.ticketmanagement.com
```

### Configuración de CORS
El backend está configurado para aceptar solicitudes desde:
- `http://localhost:4200` (desarrollo)
- `https://ticketmanagement.com` (producción)

---

## 🛠️ Solución de Problemas

### Problemas Comunes

#### 1. Error de Autenticación
**Síntoma**: "Invalid credentials"
**Solución**:
- Verificar email y contraseña
- Asegurarse de que la cuenta esté activa
- Revisar mayúsculas/minúsculas

#### 2. Token Expirado
**Síntoma**: "Unauthorized" en requests
**Solución**:
- El sistema automáticamente renueva tokens usando refresh token
- Si persiste, hacer logout y login nuevamente

#### 3. Error al Crear Ticket
**Síntoma**: "Bad Request"
**Solución**:
- Verificar que título y descripción no estén vacíos
- Asegurarse de que la prioridad sea válida
- Comprobar límites de caracteres

#### 4. Historial No Carga
**Síntoma**: Lista vacía en historial
**Solución**:
- Verificar permisos de acceso al ticket
- Comprobar conectividad con la API
- Revisar logs del servidor

---

## 📞 Soporte Técnico

Para soporte técnico adicional:
- Consultar la documentación completa en `/docs`
- Revisar logs de aplicación en el servidor
- Contactar al equipo de desarrollo

---

## 🔄 Actualizaciones y Versiones

### Última Versión: v2.1.0
- Historial de tickets mejorado
- Paginación en APIs
- Filtros avanzados
- Interfaz responsive optimizada

### Próximas Funcionalidades
- Notificaciones en tiempo real
- Adjuntos en tickets
- API de integración externa
- Dashboard analítico

---

*Este manual se actualiza con cada nueva versión del sistema. Para la versión más reciente, consulte el repositorio oficial.*