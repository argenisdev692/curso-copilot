# 🚀 MCP Servers para GitHub Copilot - Configuración 2025

## Investigación - Noviembre 2025

Esta guía documenta la configuración de MCP (Model Context Protocol) servers para integrar bases de datos y frameworks con GitHub Copilot, específicamente para el curso de desarrollo web con .NET y Angular.

---

## 📊 Supabase MCP Server

### ✅ **Disponibilidad**: SÍ - MCP Server Oficial Disponible

**Repositorio**: [supabase-community/supabase-mcp](https://github.com/supabase-community/supabase-mcp)

### Características Principales:
- **Database Schema Information**: Proporciona información del esquema de base de datos
- **Query Assistance**: Ayuda con consultas y operaciones de datos
- **Security Features**: Configuración de permisos por proyecto
- **Multiple Features**: docs, database, debugging, development, functions, branching

### Configuración para VS Code:

```json
{
  "mcpServers": {
    "supabase": {
      "command": "npx",
      "args": ["-y", "@supabase-community/supabase-mcp"],
      "env": {
        "SUPABASE_ACCESS_TOKEN": "your-access-token",
        "SUPABASE_PROJECT_REF": "your-project-ref"
      }
    }
  }
}
```

### Uso con GitHub Copilot:
- **Agent Mode**: Interactúa con Supabase usando lenguaje natural
- **Database Operations**: CRUD operations, schema queries
- **Development Support**: Ayuda con queries y debugging

---

## 🗄️ MongoDB Atlas MCP Server

### ✅ **Disponibilidad**: SÍ - MCP Server Oficial Disponible

**Repositorio**: [mongodb-developer/mcp-mongodb-atlas](https://github.com/mongodb-developer/mcp-mongodb-atlas)

### Características Principales:
- **Cluster Management**: Crear y gestionar clusters MongoDB Atlas
- **User Management**: Administración de usuarios de base de datos
- **Network Access**: Configuración de acceso de red
- **Database Operations**: Queries, aggregations, indexing

### Configuración para VS Code:

```json
{
  "mcpServers": {
    "mongodb-atlas": {
      "command": "npx",
      "args": ["mcp-mongodb-atlas"],
      "env": {
        "ATLAS_PUBLIC_KEY": "your-public-key",
        "ATLAS_PRIVATE_KEY": "your-private-key"
      }
    }
  }
}
```

### Uso con GitHub Copilot:
- **Agent Mode**: Gestión completa de MongoDB Atlas
- **Schema Introspection**: Inspección de esquemas de datos
- **Query Generation**: Generación de queries MongoDB
- **Administrative Tasks**: Gestión de clusters y usuarios

---

## 🔧 Configuración General de MCP Servers en VS Code

### 1. Instalar MCP Servers desde Marketplace:
```
Ctrl+Shift+P → MCP: Add Server
```

### 2. Configuración en settings.json:
```json
{
  "mcp": {
    "servers": {
      "supabase": {
        "command": "npx",
        "args": ["-y", "@supabase-community/supabase-mcp"],
        "env": {
          "SUPABASE_ACCESS_TOKEN": "${SUPABASE_ACCESS_TOKEN}",
          "SUPABASE_PROJECT_REF": "${SUPABASE_PROJECT_REF}"
        }
      },
      "mongodb-atlas": {
        "command": "npx",
        "args": ["mcp-mongodb-atlas"],
        "env": {
          "ATLAS_PUBLIC_KEY": "${ATLAS_PUBLIC_KEY}",
          "ATLAS_PRIVATE_KEY": "${ATLAS_PRIVATE_KEY}"
        }
      }
    }
  }
}
```

### 3. Variables de Entorno:
- Configurar variables sensibles en VS Code
- Usar `${VARIABLE_NAME}` para referencias seguras

---

## 🎯 Uso en el Curso - Sesión 1

### Para Supabase:
```bash
# Instalar y configurar
npm install -g @supabase-community/supabase-mcp

# Configurar en VS Code settings
# Luego usar en Copilot Chat:
/connect supabase
/show schema
/generate query for users table
```

### Para MongoDB Atlas:
```bash
# Instalar
npm install -g mcp-mongodb-atlas

# Configurar API keys en Atlas
# Usar en Copilot:
/create cluster
/manage users
/query collection
```

### Para Entity Framework:
```bash
# Sin MCP específico - usar prompts
# En Copilot Chat:
/create ef core model for Product
/generate dbcontext with relationships
/create migration script
```

---

## 🔒 Consideraciones de Seguridad

### Supabase MCP:
- **Project Scoping**: Limitar acceso a proyectos específicos
- **Token Management**: Usar tokens con permisos mínimos
- **Audit Logging**: Revisar logs de operaciones

### MongoDB Atlas MCP:
- **API Keys**: Usar keys con permisos específicos
- **Network Security**: Configurar IP whitelisting
- **Access Control**: Roles granulares por usuario

### Mejores Prácticas:
- ✅ Usar variables de entorno para credenciales
- ✅ Implementar rotación de tokens/keys
- ✅ Monitorear uso de MCP servers
- ✅ Limitar alcance de operaciones

---

## 📈 Beneficios para el Desarrollo

### Productividad:
- **Queries Automáticas**: Generación de consultas optimizadas
- **Schema Awareness**: Contexto completo de base de datos
- **Debugging Asistido**: Ayuda con problemas de datos

### Calidad:
- **Validación**: Verificación de queries y operaciones
- **Best Practices**: Sugerencias basadas en estándares
- **Consistency**: Operaciones uniformes

### Integración:
- **Fullstack Development**: Backend + Frontend + Database
- **CI/CD**: Automatización de operaciones de BD
- **Team Collaboration**: Conocimiento compartido

---

## 🚀 Próximos Pasos

1. **Configurar Supabase MCP** en entorno de desarrollo
2. **Instalar MongoDB Atlas MCP** para gestión de clusters
3. **Documentar Workflows** específicos del curso
4. **Crear Templates** de configuración para estudiantes
5. **Evaluar Feedback** en sesiones prácticas

---

**Fuentes de Investigación**:
- GitHub MCP Registry
- Documentación Oficial Supabase
- MongoDB Developer Center
- Microsoft MCP Documentation
- VS Code MCP Integration

**Última actualización**: Noviembre 2025