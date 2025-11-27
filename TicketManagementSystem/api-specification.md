# 📊 Ticket Management System - API Specification

## 🗄️ Database Tables

### Users
- **Id** (int, PK, Identity)
- **Email** (nvarchar(256), unique)
- **PasswordHash** (nvarchar(max))
- **FullName** (nvarchar(100))
- **Role** (nvarchar(20)) - Admin/Agent/User
- **IsActive** (bit)
- **CreatedAt** (datetime2)
- **UpdatedAt** (datetime2)

### Tickets
- **Id** (int, PK, Identity)
- **Title** (nvarchar(100))
- **Description** (nvarchar(1000))
- **Status** (nvarchar(20)) - Open/InProgress/Resolved/Closed
- **Priority** (nvarchar(20)) - Low/Medium/High/Critical
- **CreatedBy** (int, FK → Users.Id)
- **AssignedTo** (int, FK → Users.Id, nullable)
- **CreatedAt** (datetime2)
- **UpdatedAt** (datetime2)

### Comments
- **Id** (int, PK, Identity)
- **TicketId** (int, FK → Tickets.Id)
- **UserId** (int, FK → Users.Id)
- **Content** (nvarchar(500))
- **CreatedAt** (datetime2)
- **UpdatedAt** (datetime2)

### TicketHistory
- **Id** (int, PK, Identity)
- **TicketId** (int, FK → Tickets.Id)
- **ChangedBy** (int, FK → Users.Id)
- **FieldName** (nvarchar(50))
- **OldValue** (nvarchar(500))
- **NewValue** (nvarchar(500))
- **ChangedAt** (datetime2)

## 🔗 API Endpoints

### Authentication
#### POST /api/auth/login
**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "user": {
    "id": 1,
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "User"
  }
}
```

#### POST /api/auth/register
**Request:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "fullName": "John Doe",
  "role": "User"
}
```

**Response (201 Created):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "user": {
    "id": 2,
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "User",
    "isActive": true,
    "createdAt": "2025-01-01T10:00:00Z"
  }
}
```

#### POST /api/auth/refresh
**Request:**
```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

### Users
#### GET /api/users?role=Agent&isActive=true&search=john
**Response (200 OK):**
```json
[
  {
    "id": 1,
    "email": "user@example.com",
    "fullName": "John Doe",
    "role": "User",
    "isActive": true,
    "createdAt": "2025-01-01T10:00:00Z",
    "updatedAt": "2025-01-01T10:00:00Z"
  }
]
```

#### GET /api/users/{id}
**Response (200 OK):**
```json
{
  "id": 1,
  "email": "user@example.com",
  "fullName": "John Doe",
  "role": "User",
  "isActive": true,
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T10:00:00Z"
}
```

#### PUT /api/users/{id}
**Request:**
```json
{
  "fullName": "John Smith",
  "role": "Agent",
  "isActive": true
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "email": "user@example.com",
  "fullName": "John Smith",
  "role": "Agent",
  "isActive": true,
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T12:30:00Z"
}
```

#### DELETE /api/users/{id}
**Response (204 No Content)** - Soft delete (sets IsActive = false)

### Tickets
#### GET /api/tickets?page=1&pageSize=10&status=Open&priority=High
```json
{
  "data": [
    {
      "id": 1,
      "title": "System Bug",
      "description": "Application crashes on login",
      "status": "Open",
      "priority": "High",
      "createdBy": {
        "id": 1,
        "fullName": "John Doe"
      },
      "assignedTo": null,
      "createdAt": "2025-01-01T10:00:00Z",
      "updatedAt": "2025-01-01T10:00:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10
}
```

#### GET /api/tickets/{id}
```json
{
  "id": 1,
  "title": "System Bug",
  "description": "Application crashes on login",
  "status": "Open",
  "priority": "High",
  "createdBy": {
    "id": 1,
    "fullName": "John Doe"
  },
  "assignedTo": null,
  "comments": [],
  "history": [],
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T10:00:00Z"
}
```

#### POST /api/tickets
**Request:**
```json
{
  "title": "New Feature Request",
  "description": "Add dark mode to the application",
  "priority": "Medium"
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "title": "New Feature Request",
  "description": "Add dark mode to the application",
  "status": "Open",
  "priority": "Medium",
  "createdBy": {
    "id": 1,
    "fullName": "John Doe"
  },
  "assignedTo": null,
  "createdAt": "2025-01-01T14:00:00Z",
  "updatedAt": "2025-01-01T14:00:00Z"
}
```

#### PUT /api/tickets/{id}
**Request:**
```json
{
  "title": "Updated Bug Report",
  "description": "Application crashes on login - updated details",
  "status": "InProgress",
  "priority": "Critical",
  "assignedTo": 2
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Updated Bug Report",
  "description": "Application crashes on login - updated details",
  "status": "InProgress",
  "priority": "Critical",
  "createdBy": {
    "id": 1,
    "fullName": "John Doe"
  },
  "assignedTo": {
    "id": 2,
    "fullName": "Jane Smith"
  },
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-01T15:30:00Z"
}
```

#### DELETE /api/tickets/{id}
**Response (204 No Content)** - Soft delete

#### GET /api/tickets/my-tickets?status=Open
**Response (200 OK):** - Get tickets created by or assigned to current user
```json
{
  "data": [
    {
      "id": 1,
      "title": "System Bug",
      "description": "Application crashes on login",
      "status": "Open",
      "priority": "High",
      "createdBy": {
        "id": 1,
        "fullName": "John Doe"
      },
      "assignedTo": null,
      "createdAt": "2025-01-01T10:00:00Z",
      "updatedAt": "2025-01-01T10:00:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 10
}
```

#### GET /api/tickets/stats
**Response (200 OK):**
```json
{
  "totalTickets": 150,
  "openTickets": 45,
  "inProgressTickets": 30,
  "resolvedTickets": 50,
  "closedTickets": 25,
  "byPriority": {
    "low": 20,
    "medium": 60,
    "high": 50,
    "critical": 20
  },
  "averageResolutionTimeHours": 48.5
}
```

### Comments
#### GET /api/tickets/{ticketId}/comments
**Response (200 OK):**
```json
[
  {
    "id": 1,
    "content": "This is a comment on the ticket",
    "user": {
      "id": 1,
      "fullName": "John Doe"
    },
    "createdAt": "2025-01-01T11:00:00Z",
    "updatedAt": "2025-01-01T11:00:00Z"
  }
]
```

#### POST /api/tickets/{ticketId}/comments
**Request:**
```json
{
  "content": "This is a new comment"
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "content": "This is a new comment",
  "user": {
    "id": 1,
    "fullName": "John Doe"
  },
  "createdAt": "2025-01-01T12:00:00Z",
  "updatedAt": "2025-01-01T12:00:00Z"
}
```

#### PUT /api/comments/{id}
**Request:**
```json
{
  "content": "Updated comment content"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "content": "Updated comment content",
  "user": {
    "id": 1,
    "fullName": "John Doe"
  },
  "createdAt": "2025-01-01T11:00:00Z",
  "updatedAt": "2025-01-01T13:00:00Z"
}
```

#### DELETE /api/comments/{id}
**Response (204 No Content)**

### Ticket History
#### GET /api/tickets/{ticketId}/history
**Response (200 OK):**
```json
[
  {
    "id": 1,
    "ticketId": 1,
    "fieldName": "Status",
    "oldValue": "Open",
    "newValue": "InProgress",
    "changedBy": {
      "id": 2,
      "fullName": "Jane Smith"
    },
    "changedAt": "2025-01-01T15:30:00Z"
  },
  {
    "id": 2,
    "ticketId": 1,
    "fieldName": "AssignedTo",
    "oldValue": null,
    "newValue": "Jane Smith",
    "changedBy": {
      "id": 1,
      "fullName": "John Doe"
    },
    "changedAt": "2025-01-01T15:28:00Z"
  }
]
```

### Health Check
#### GET /health
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.1234567",
  "entries": {
    "sqlserver": {
      "data": {},
      "duration": "00:00:00.0987654",
      "status": "Healthy",
      "tags": []
    }
  }
}
```

---

## 🔐 Authentication & Authorization

### JWT Token Format
```json
{
  "sub": "1",
  "email": "user@example.com",
  "role": "User",
  "exp": 1735660800,
  "iss": "TicketManagementSystem",
  "aud": "TicketManagementSystem"
}
```

### Authorization Rules
- **Public endpoints:** `/api/auth/login`, `/api/auth/register`, `/health`
- **Authenticated endpoints:** All other endpoints require valid JWT token
- **Admin only:** `/api/users/*` (all user management endpoints)
- **Owner or Admin:** 
  - `PUT /api/comments/{id}` - only comment author can edit
  - `DELETE /api/comments/{id}` - only comment author or admin can delete
  - `DELETE /api/users/{id}` - cannot delete own account

### HTTP Headers
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

---

## 📡 HTTP Status Codes

### Success Codes
- **200 OK** - Request successful, resource returned
- **201 Created** - Resource created successfully
- **204 No Content** - Request successful, no content to return

### Client Error Codes
- **400 Bad Request** - Invalid request data or validation error
- **401 Unauthorized** - Missing or invalid authentication token
- **403 Forbidden** - Valid token but insufficient permissions
- **404 Not Found** - Resource does not exist
- **409 Conflict** - Resource conflict (e.g., duplicate email)

### Server Error Codes
- **500 Internal Server Error** - Unexpected server error

### Error Response Format
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["The Title field is required."],
    "Priority": ["Priority must be one of: Low, Medium, High, Critical"]
  },
  "traceId": "00-1234567890abcdef-1234567890abcdef-00"
}
```

---

## 🔍 Query Parameters

### Pagination
- `page` (int, default: 1) - Page number
- `pageSize` (int, default: 10, max: 100) - Items per page

### Filtering (Tickets)
- `status` (string) - Filter by status: Open, InProgress, Resolved, Closed
- `priority` (string) - Filter by priority: Low, Medium, High, Critical
- `assignedTo` (int) - Filter by assigned user ID
- `createdBy` (int) - Filter by creator user ID
- `search` (string) - Search in title and description

### Filtering (Users)
- `role` (string) - Filter by role: Admin, Agent, User
- `isActive` (bool) - Filter by active status
- `search` (string) - Search in email and full name

### Sorting
- `sortBy` (string, default: "createdAt") - Field to sort by
- `sortOrder` (string, default: "desc") - Sort order: asc, desc

---

## 📝 Notes

- All timestamps are in UTC ISO 8601 format
- Soft deletes set `IsDeleted = true` and preserve data
- Password minimum length: 8 characters
- JWT token expiration: 1 hour (3600 seconds)
- Refresh token expiration: 7 days
- Maximum file upload size: 10 MB (future feature)
- Rate limiting: 100 requests per minute per IP (future feature)