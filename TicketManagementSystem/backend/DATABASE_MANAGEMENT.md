# Database Management - SQLite

Este documento contiene instrucciones para resetear, limpiar y gestionar la base de datos SQLite del sistema.

---

## 🗄️ Información de la Base de Datos

**Motor**: SQLite  
**Archivos**:
- `TicketManagementDb.db` - Archivo principal de la base de datos
- `TicketManagementDb.db-shm` - Shared Memory file (temporal)
- `TicketManagementDb.db-wal` - Write-Ahead Log (temporal)

**Ubicación**: `backend/TicketManagementSystem.API/`

---

## 🔄 Opción 1: Reset Completo (Recomendado)

Esta opción elimina toda la base de datos y la recrea con datos de seed.

### Pasos:

```powershell
# 1. Detener el backend (Ctrl+C en la terminal donde está corriendo)

# 2. Navegar al directorio del proyecto
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API

# 3. Eliminar archivos de base de datos
Remove-Item -Path "TicketManagementDb.db*" -Force

# 4. Verificar que se eliminaron
Get-ChildItem -Filter "TicketManagementDb.db*"
# Debería mostrar: "Cannot find path..."

# 5. Recrear base de datos aplicando migraciones
dotnet ef database update

# 6. Reiniciar el backend
dotnet run
```

### ✅ Resultado:
- Base de datos limpia con estructura actualizada
- Datos de seed cargados automáticamente:
  - 3 usuarios (Admin, Agent, User)
  - 3 tickets de ejemplo
  - Comentarios y historial de ejemplo

---

## 🧹 Opción 2: Limpiar Solo Datos de Usuario

Si solo quieres eliminar usuarios específicos sin resetear todo:

```powershell
# 1. Instalar SQLite CLI (si no lo tienes)
# Descargar de: https://www.sqlite.org/download.html
# O usar: winget install sqlite.sqlite

# 2. Conectar a la base de datos
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API
sqlite3 TicketManagementDb.db

# 3. Ver usuarios actuales
SELECT Id, Email, FullName, IsDeleted FROM Users;

# 4. Eliminar usuario específico (hard delete)
DELETE FROM Users WHERE Email = 'usuario@example.com';

# 5. O marcar como soft-deleted
UPDATE Users SET IsDeleted = 1, UpdatedAt = datetime('now') WHERE Email = 'usuario@example.com';

# 6. Salir
.exit
```

---

## 🔍 Opción 3: Consultar Base de Datos

Para inspeccionar datos sin modificar:

```powershell
# Abrir SQLite
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API
sqlite3 TicketManagementDb.db

# Ver todas las tablas
.tables

# Ver estructura de tabla
.schema Users

# Consultas útiles
SELECT COUNT(*) FROM Users WHERE IsDeleted = 0;
SELECT Email, FullName, Role FROM Users WHERE IsDeleted = 0;
SELECT * FROM Tickets ORDER BY CreatedAt DESC LIMIT 10;

# Salir
.exit
```

---

## 🛠️ Entity Framework Core Migrations

### Ver migraciones disponibles:
```powershell
dotnet ef migrations list
```

### Aplicar migraciones pendientes:
```powershell
dotnet ef database update
```

### Revertir a una migración específica:
```powershell
dotnet ef database update <MigrationName>
```

### Crear nueva migración:
```powershell
dotnet ef migrations add <MigrationName>
```

### Eliminar última migración (si no se ha aplicado):
```powershell
dotnet ef migrations remove
```

---

## 📊 Datos de Seed (OnModelCreating)

La base de datos incluye datos de prueba automáticos en `ApplicationDbContext.cs`:

### Usuarios de Seed:

| Email | Password | Role | ID |
|-------|----------|------|-----|
| admin@example.com | Admin@123 | Admin | 1 |
| agent@example.com | Agent@123 | Agent | 2 |
| user@example.com | User@123 | User | 3 |

### Tickets de Seed:
- **3 tickets de ejemplo** con diferentes estados y prioridades
- Asignados a diferentes usuarios
- Incluyen comentarios e historial

**Nota**: Los passwords son hasheados con BCrypt antes de almacenarse.

---

## 🚨 Problemas Comunes

### Error: "Database is locked"
```powershell
# Solución: Detener el backend y eliminar archivos temporales
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API
Remove-Item -Path "TicketManagementDb.db-shm", "TicketManagementDb.db-wal" -Force -ErrorAction SilentlyContinue
```

### Error: "UNIQUE constraint failed: Users.Email"
```powershell
# Solución: El email ya existe (incluso si está soft-deleted)
# Opción A: Usar otro email
# Opción B: Eliminar usuario existente (hard delete)
# Opción C: Reset completo (Opción 1)
```

### Error: "No migrations found"
```powershell
# Solución: Crear migración inicial
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🔒 Backup de Base de Datos

### Crear backup:
```powershell
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API

# Crear carpeta de backups
New-Item -ItemType Directory -Path "backups" -Force

# Copiar base de datos con timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
Copy-Item "TicketManagementDb.db" "backups/TicketManagementDb_$timestamp.db"
```

### Restaurar backup:
```powershell
cd c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API

# Detener backend primero (Ctrl+C)

# Restaurar desde backup
Copy-Item "backups/TicketManagementDb_20251119_153045.db" "TicketManagementDb.db" -Force

# Reiniciar backend
dotnet run
```

---

## 📝 Scripts de Utilidad

### Script PowerShell: Reset Completo
Guarda como `reset-database.ps1`:

```powershell
# Reset Database Script
$projectPath = "c:\Users\ARGENIS\Documents\copilot-curso-2025\TicketManagementSystem\backend\TicketManagementSystem.API"

Write-Host "🔄 Resetting database..." -ForegroundColor Yellow

# 1. Navigate to project
Set-Location $projectPath

# 2. Remove database files
Remove-Item -Path "TicketManagementDb.db*" -Force -ErrorAction SilentlyContinue
Write-Host "✅ Database files removed" -ForegroundColor Green

# 3. Apply migrations
Write-Host "📦 Applying migrations..." -ForegroundColor Yellow
dotnet ef database update

Write-Host "✅ Database reset complete!" -ForegroundColor Green
Write-Host "ℹ️  You can now run: dotnet run" -ForegroundColor Cyan
```

**Uso**:
```powershell
.\reset-database.ps1
```

---

## 🔐 Consideraciones de Seguridad

⚠️ **IMPORTANTE**:
- ❌ Nunca commitear archivos `*.db` a Git (ya incluido en `.gitignore`)
- ❌ Nunca usar datos de seed en producción
- ❌ Nunca exponer SQLite en producción (usar SQL Server/PostgreSQL)
- ✅ Cambiar passwords de seed antes de deployment
- ✅ Hacer backups regulares en producción

---

## 📚 Recursos Adicionales

- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [Entity Framework Core Migrations](https://docs.microsoft.com/ef/core/managing-schemas/migrations/)
- [SQLite Browser (GUI)](https://sqlitebrowser.org/)

---

**Última actualización**: 19 Noviembre 2025
