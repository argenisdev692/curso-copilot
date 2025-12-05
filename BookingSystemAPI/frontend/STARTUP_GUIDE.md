# 🚀 Guía de Inicio - BookingSystemAPI Frontend

## Descripción

Este documento explica las dos formas de ejecutar el frontend Angular según cómo esté corriendo el backend:
1. **Con Backend Local** (`dotnet run`) → `npm start`
2. **Con Backend Docker** → `npm run start:docker`

---

## 📋 Requisitos Previos

- Node.js 18+ 
- npm 9+
- Angular CLI (opcional, incluido en devDependencies)

### Instalación de dependencias
```powershell
cd BookingSystemAPI/frontend
npm install
```

---

## 🖥️ Opción 1: Con Backend Local (`npm start`)

### Cuándo usar
- El backend está corriendo con `dotnet run`
- Puerto del backend: **5146**

### Comando
```powershell
npm start
```

### Configuración del Proxy
El archivo `proxy.conf.json` redirige las llamadas `/api/*` al backend local:

```json
{
  "/api": {
    "target": "http://localhost:5146",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}
```

### URLs

| Servicio | URL |
|----------|-----|
| **Frontend** | `http://localhost:4200` |
| **Backend (proxy)** | `http://localhost:5146` |
| **Login** | `http://localhost:4200/auth/login` |
| **Register** | `http://localhost:4200/auth/register` |

### Ejemplo completo
```powershell
# Terminal 1 - Backend
cd BookingSystemAPI/backend/BookingSystemAPI.Api
dotnet run

# Terminal 2 - Frontend
cd BookingSystemAPI/frontend
npm start

# Abrir navegador en http://localhost:4200
```

---

## 🐳 Opción 2: Con Backend Docker (`npm run start:docker`)

### Cuándo usar
- El backend está corriendo con `docker compose up`
- Puerto del backend: **8080**

### Comando
```powershell
npm run start:docker
```

### Configuración del Proxy
El archivo `proxy.conf.docker.json` redirige las llamadas `/api/*` al backend en Docker:

```json
{
  "/api": {
    "target": "http://localhost:8080",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}
```

### URLs

| Servicio | URL |
|----------|-----|
| **Frontend** | `http://localhost:4200` |
| **Backend (Docker)** | `http://localhost:8080` |
| **Login** | `http://localhost:4200/auth/login` |
| **Register** | `http://localhost:4200/auth/register` |

### Ejemplo completo
```powershell
# Terminal 1 - Backend (Docker)
cd BookingSystemAPI/backend
docker compose up -d

# Terminal 2 - Frontend
cd BookingSystemAPI/frontend
npm run start:docker

# Abrir navegador en http://localhost:4200
```

---

## 📊 Comparación Rápida

| Característica | `npm start` | `npm run start:docker` |
|----------------|-------------|------------------------|
| **Puerto Backend** | 5146 | 8080 |
| **Archivo Proxy** | proxy.conf.json | proxy.conf.docker.json |
| **Backend requerido** | `dotnet run` | `docker compose up` |
| **Base de datos** | InMemory | SQL Server |
| **Persistencia datos** | ❌ No | ✅ Sí |

---

## 📝 Scripts Disponibles

| Comando | Descripción |
|---------|-------------|
| `npm start` | Inicia con proxy a puerto 5146 (dotnet run) |
| `npm run start:docker` | Inicia con proxy a puerto 8080 (Docker) |
| `npm run build` | Compila para desarrollo |
| `npm run build:prod` | Compila para producción |
| `npm test` | Ejecuta tests con Jest |
| `npm run test:watch` | Tests en modo watch |
| `npm run test:coverage` | Tests con reporte de cobertura |
| `npm run e2e` | Ejecuta tests E2E con Cypress |
| `npm run e2e:open` | Abre Cypress UI |
| `npm run lint` | Ejecuta ESLint |
| `npm run lint:fix` | Corrige errores de ESLint |
| `npm run format` | Formatea código con Prettier |

---

## 🔧 Solución de Problemas

### Error: `500 Internal Server Error` en `/api/auth/register`

**Causa:** Estás usando el comando incorrecto para el backend que está corriendo.

| Backend corriendo | Comando correcto |
|-------------------|------------------|
| `dotnet run` (puerto 5146) | `npm start` |
| `docker compose` (puerto 8080) | `npm run start:docker` |

### Error: `ECONNREFUSED` o `Proxy error`

**Causa:** El backend no está corriendo.

```powershell
# Verificar backend local
curl http://localhost:5146/health

# Verificar backend Docker
curl http://localhost:8080/health
docker compose ps
```

### Error: Puerto 4200 en uso

```powershell
# Encontrar proceso
netstat -ano | findstr :4200

# Usar puerto diferente
ng serve --port 4201
```

### Limpiar caché de Angular

```powershell
# Eliminar caché
Remove-Item -Recurse -Force .angular/cache

# Reinstalar dependencias
Remove-Item -Recurse -Force node_modules
npm install
```

---

## 🏗️ Estructura del Proyecto

```
frontend/
├── src/
│   ├── app/
│   │   ├── core/           # Guards, Interceptors, Services, Models
│   │   ├── features/       # Módulos de funcionalidades
│   │   │   ├── auth/       # Login, Register
│   │   │   ├── bookings/   # Gestión de reservas
│   │   │   ├── dashboard/  # Panel principal
│   │   │   └── rooms/      # Gestión de salas
│   │   ├── layouts/        # Layouts (Auth, Main)
│   │   └── shared/         # Componentes, Pipes, Directivas compartidas
│   └── environments/       # Configuración por ambiente
├── proxy.conf.json         # Proxy para backend local (5146)
├── proxy.conf.docker.json  # Proxy para backend Docker (8080)
└── package.json            # Scripts y dependencias
```

---

## ✅ Verificación de Funcionamiento

### 1. Verificar que el proxy funciona
Abre las DevTools del navegador (F12) → Network. Las llamadas a `/api/*` deben mostrar el proxy funcionando.

### 2. Probar registro de usuario
1. Navega a `http://localhost:4200/auth/register`
2. Completa el formulario:
   - Email: `usuario@ejemplo.com`
   - Password: `MiPassword123!`
   - Nombre: `Juan`
   - Apellido: `Pérez`
3. Verifica respuesta exitosa

### 3. Probar login
1. Navega a `http://localhost:4200/auth/login`
2. Usa las credenciales creadas
3. Verifica redirección al dashboard

---

## 🎯 Flujo de Trabajo Recomendado

### Desarrollo rápido (sin persistencia)
```powershell
# Backend (Terminal 1)
cd backend/BookingSystemAPI.Api
dotnet run

# Frontend (Terminal 2)
cd frontend
npm start
```

### Desarrollo con datos persistentes
```powershell
# Backend con Docker (Terminal 1)
cd backend
docker compose up -d

# Frontend (Terminal 2)
cd frontend
npm run start:docker
```
