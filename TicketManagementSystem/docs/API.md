# API Documentation

## Endpoints Disponibles

### Autenticación

#### POST /api/auth/login
Inicia sesión de usuario.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response 200:**
```json
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "refresh_token_here",
  "user": {
    "id": 1,
    "email": "user@example.com",
    "name": "John Doe"
  }
}
```

**Códigos de Estado:**
- 200: Login exitoso
- 400: Datos inválidos
- 401: Credenciales incorrectas

### Tickets

#### GET /api/tickets
Obtiene lista paginada de tickets.

**Query Parameters:**
- pageNumber (int, opcional): Número de página (default: 1)
- pageSize (int, opcional): Tamaño de página (default: 10, max: 100)

**Response 200:**
```json
{
  "items": [
    {
      "id": 1,
      "title": "Bug en login",
      "description": "El login falla en móvil",
      "status": "Open",
      "priority": "High",
      "createdAt": "2025-11-20T10:00:00Z",
      "createdBy": {
        "id": 1,
        "name": "John Doe"
      }
    }
  ],
  "totalCount": 25,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 3
}
```

#### POST /api/tickets
Crea un nuevo ticket.

**Request Body:**
```json
{
  "title": "Nuevo ticket",
  "description": "Descripción del ticket",
  "priority": "Medium",
  "categoryId": 1
}
```

**Response 201:**
```json
{
  "id": 26,
  "title": "Nuevo ticket",
  "description": "Descripción del ticket",
  "status": "Open",
  "priority": "Medium",
  "createdAt": "2025-11-20T12:00:00Z"
}
```

#### PUT /api/tickets/{id}
Actualiza un ticket existente.

#### DELETE /api/tickets/{id}
Elimina un ticket.

## Esquemas de Datos

### Ticket
```typescript
interface Ticket {
  id: number;
  title: string;
  description?: string;
  status: TicketStatus;
  priority: Priority;
  categoryId: number;
  assignedToId?: number;
  createdAt: string;
  updatedAt: string;
  createdBy: User;
  assignedTo?: User;
}
```

### User
```typescript
interface User {
  id: number;
  email: string;
  name: string;
  role: string;
}
```

## Autenticación
Todos los endpoints requieren un Bearer token en el header Authorization.

```
Authorization: Bearer eyJhbGc...
```