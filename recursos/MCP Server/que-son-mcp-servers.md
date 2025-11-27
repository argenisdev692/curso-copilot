# 🔌 ¿Qué son los MCP Servers?

> **MCP** = Model Context Protocol

---

## Concepto

Los **MCP Servers** son servidores que extienden las capacidades de GitHub Copilot permitiéndole conectarse con herramientas y servicios externos.

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│  GitHub Copilot │ ←→  │   MCP Server    │ ←→  │ Servicio Externo│
│    (Cliente)    │     │   (Puente)      │     │ (Azure, GitHub) │
└─────────────────┘     └─────────────────┘     └─────────────────┘
```

---

## ¿Para qué sirven?

| Función | Ejemplo |
|---------|---------|
| **Acceso a datos** | Consultar bases de datos, APIs |
| **Búsqueda web** | Tavily para búsquedas en tiempo real |
| **Control de versiones** | Operaciones Git avanzadas |
| **Cloud** | Gestión de recursos Azure |
| **Contexto externo** | Documentación, archivos remotos |

---

## Arquitectura Básica

```
VS Code + Copilot
       ↓
   MCP Client (integrado)
       ↓
   MCP Server (local o remoto)
       ↓
   Herramienta/API externa
```

---

## Configuración en VS Code

Los MCP servers se configuran en `settings.json`:

```json
{
  "mcp": {
    "servers": {
      "nombre-servidor": {
        "type": "stdio",
        "command": "npx",
        "args": ["-y", "@paquete/mcp-server"]
      }
    }
  }
}
```

---

## MCP Servers Populares

| Servidor | Uso |
|----------|-----|
| `@anthropic/mcp-server-github` | Operaciones GitHub |
| `tavily-mcp` | Búsquedas web en tiempo real |
| `@anthropic/mcp-server-filesystem` | Acceso a sistema de archivos |
| Azure MCP | Gestión recursos Azure |

---

## Beneficios

- ✅ **Contexto extendido**: Copilot accede a información fuera del workspace
- ✅ **Automatización**: Ejecuta acciones en servicios externos
- ✅ **Personalización**: Crea tus propios MCP servers
- ✅ **Seguridad**: Control granular de permisos

---

## Flujo de Trabajo

1. **Usuario** hace pregunta a Copilot
2. **Copilot** detecta que necesita información externa
3. **MCP Client** envía solicitud al MCP Server
4. **MCP Server** consulta servicio externo
5. **Respuesta** regresa a Copilot con contexto enriquecido

---

> 💡 **Tip**: Los MCP servers transforman a Copilot de un asistente de código a un agente capaz de interactuar con tu infraestructura completa.
