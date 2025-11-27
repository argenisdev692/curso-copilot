# 🗃️ Guía de Configuración: Supabase MCP Server

> **Versión**: Noviembre 2025  
> **Requisitos**: VS Code, GitHub Copilot, Node.js 18+

---

## 📋 ¿Qué es Supabase MCP?

Supabase MCP Server permite a GitHub Copilot interactuar directamente con tu base de datos Supabase:

| Capacidad | Descripción |
|-----------|-------------|
| **Schema Info** | Consultar estructura de tablas |
| **Query Assistance** | Generar y ejecutar queries SQL |
| **Database Operations** | CRUD desde Copilot |
| **Functions** | Interactuar con Edge Functions |
| **Debugging** | Ayuda con errores de DB |

---

## 🔧 Paso 1: Obtener Credenciales de Supabase

### 1.1 Access Token

1. Ir a [Supabase Dashboard](https://supabase.com/dashboard)
2. Click en tu avatar → **Account Settings**
3. Sección **Access Tokens**
4. Click **Generate New Token**
5. Nombre: `copilot-mcp`
6. **Copiar y guardar** el token (solo se muestra una vez)

### 1.2 Project Reference

1. En el Dashboard, selecciona tu proyecto
2. Ir a **Settings** → **General**
3. Buscar **Reference ID** (formato: `abcdefghijklmnop`)
4. Copiar el valor

---

## ⚙️ Paso 2: Configurar en VS Code

### Opción A: Instalación desde Marketplace (Recomendado)

1. Abrir VS Code
2. Abrir Extensions (`Ctrl+Shift+X`)
3. Buscar `@mcp` o ejecutar comando:
   ```
   Ctrl+Shift+P → "MCP: Browse Servers"
   ```
4. Buscar **"Supabase"** en la lista
5. Click en **Install**
6. Configurar credenciales cuando se solicite (ver Paso 1)

### Opción B: Configuración Manual

Abrir `settings.json`:
```
Ctrl+Shift+P → "Preferences: Open User Settings (JSON)"
```

#### Modo Read-Only (Por defecto - Solo lectura)
```json
{
  "mcp": {
    "servers": {
      "supabase": {
        "command": "npx",
        "args": ["-y", "@supabase-community/supabase-mcp"],
        "env": {
          "SUPABASE_ACCESS_TOKEN": "sbp_xxxxxxxxxxxxxxxxxxxxxxxxxx",
          "SUPABASE_PROJECT_REF": "abcdefghijklmnop"
        }
      }
    }
  }
}
```

#### ⚡ Modo Full Access (Lectura + Escritura - CRUD completo)

> **⚠️ Precaución**: Este modo permite INSERT, UPDATE y DELETE directamente desde Copilot.

```json
{
  "mcp": {
    "servers": {
      "supabase": {
        "command": "npx",
        "args": ["-y", "@supabase-community/supabase-mcp", "--read-only=false"],
        "env": {
          "SUPABASE_ACCESS_TOKEN": "sbp_xxxxxxxxxxxxxxxxxxxxxxxxxx",
          "SUPABASE_PROJECT_REF": "abcdefghijklmnop"
        }
      }
    }
  }
}
```

| Modo | Operaciones Permitidas |
|------|----------------------|
| `--read-only=true` (default) | SELECT únicamente |
| `--read-only=false` | SELECT, INSERT, UPDATE, DELETE |

### Opción C: Configuración Segura con Variables de Entorno (Recomendado)

En lugar de poner tokens en settings.json, usa variables de entorno:

**Windows (PowerShell como Admin):**
```powershell
[System.Environment]::SetEnvironmentVariable("SUPABASE_ACCESS_TOKEN", "sbp_xxx", "User")
[System.Environment]::SetEnvironmentVariable("SUPABASE_PROJECT_REF", "abcdefghijklmnop", "User")
```

**Luego en settings.json (con full access):**
```json
{
  "mcp": {
    "servers": {
      "supabase": {
        "command": "npx",
        "args": ["-y", "@supabase-community/supabase-mcp", "--read-only=false"],
        "env": {
          "SUPABASE_ACCESS_TOKEN": "${env:SUPABASE_ACCESS_TOKEN}",
          "SUPABASE_PROJECT_REF": "${env:SUPABASE_PROJECT_REF}"
        }
      }
    }
  }
}
```

---

## ✅ Paso 3: Verificar Instalación

### 3.1 Reiniciar VS Code

Cerrar y abrir VS Code para que cargue las variables de entorno.

### 3.2 Verificar MCP Activo

```
Ctrl+Shift+P → "MCP: List Servers"
```

Debes ver `supabase` en estado **Running**.

### 3.3 Probar en Copilot Chat

Abre Copilot Chat y escribe:

```
@supabase lista las tablas de mi proyecto
```

---

## 💬 Ejemplos de Uso

### Consultar Schema
```
@supabase ¿qué tablas tengo en mi base de datos?
```

### Generar Query
```
@supabase genera un query para obtener todos los usuarios creados este mes
```

### Crear Tabla
```
@supabase crea una tabla "tickets" con id, title, description, status, created_at
```

### Debugging
```
@supabase este query da error: SELECT * FROM usres WHERE id = 1
```

### Políticas RLS
```
@supabase crea una política RLS para que los usuarios solo vean sus propios tickets
```

---

## ⚠️ Troubleshooting

### Error: "Cannot find module"

```powershell
# Limpiar cache de npx
npx clear-npx-cache

# Instalar manualmente
npm install -g @supabase-community/supabase-mcp
```

### Error: "Invalid access token"

1. Verificar que el token no expiró
2. Generar nuevo token en Supabase Dashboard
3. Actualizar en variables de entorno

### MCP no aparece en lista

1. Verificar sintaxis JSON (sin comas extra)
2. Reiniciar VS Code completamente
3. Verificar que Node.js está en PATH

---

## 🚀 Cómo Usar en Copilot Chat

Para usar el MCP de Supabase en el panel de chat de Copilot en VS Code, no tienes que escribir un comando tipo "use supabase". Funciona de forma diferente a los comandos de barra (/).

### Pasos para Usar:

1. **Asegúrate de estar en "Modo Agente"**: En la ventana del chat de Copilot, fíjate en la parte de abajo o en el menú desplegable (a veces dice "Copilot" o "Agent"). Para que las herramientas MCP funcionen, debes estar usando el modo que permite herramientas (generalmente el Agent).

2. **Activa la herramienta**: En la caja de chat, busca un icono de herramientas o un clip ("Attach context"). Si haces clic ahí, deberías ver una lista de "MCP Servers". Asegúrate de que Supabase está activado o seleccionado.

3. **Habla normal**: No escribas "use supabase". Simplemente pídele lo que quieres hacer. Copilot detectará que necesita usar la herramienta de Supabase y te pedirá permiso.

### Ejemplos:
- "Lista las tablas de mi proyecto Supabase."
- "Genera un query para obtener todos los usuarios creados este mes."
- "Crea una tabla 'tickets' con id, title, description, status, created_at."

### Truco del #:
A veces, si quieres forzar que use una herramienta, puedes escribir # en el chat. Al escribir # te saldrá una lista de contextos y herramientas disponibles. Ahí podrías ver algo relacionado con Supabase si está bien conectado.

## 📚 Recursos

- [Repositorio Oficial](https://github.com/supabase-community/supabase-mcp)
- [Documentación Supabase](https://supabase.com/docs)
- [MCP en VS Code](https://code.visualstudio.com/docs/copilot/customization/mcp-servers)
