# 🐙 Guía de Configuración: GitHub MCP Server

> **Versión**: Noviembre 2025  
> **Requisitos**: VS Code, GitHub Copilot, Node.js 18+, Cuenta GitHub

---

## 📋 ¿Qué es GitHub MCP?

GitHub MCP Server permite a Copilot interactuar directamente con repositorios, issues, PRs y más:

| Capacidad | Descripción |
|-----------|-------------|
| **Repositories** | Listar, buscar, crear repos |
| **Issues** | Crear, buscar, comentar issues |
| **Pull Requests** | Ver PRs, reviews, comentarios |
| **Code Search** | Buscar código en repos |
| **Actions** | Ver estado de workflows |
| **Files** | Leer archivos de repos remotos |

---

## 🔧 Paso 1: Crear Personal Access Token (PAT)

### 1.1 Ir a GitHub Settings

1. Ir a [GitHub.com](https://github.com)
2. Click en tu avatar → **Settings**
3. Scroll hasta **Developer settings** (al final del menú izquierdo)
4. Click en **Personal access tokens** → **Tokens (classic)**

### 1.2 Generar Nuevo Token

1. Click **Generate new token** → **Generate new token (classic)**
2. **Note**: `copilot-mcp-server`
3. **Expiration**: 90 días (o según tu preferencia)
4. **Scopes** (permisos necesarios):

```
✅ repo (Full control of private repositories)
  ✅ repo:status
  ✅ repo_deployment
  ✅ public_repo
  ✅ repo:invite
  
✅ read:org (Read org membership)

✅ read:user (Read user profile)

✅ read:project (Read projects)
```

5. Click **Generate token**
6. **⚠️ COPIAR INMEDIATAMENTE** (solo se muestra una vez)

---

## ⚙️ Paso 2: Configurar en VS Code

### Opción A: Instalación desde Marketplace (Recomendado)

1. Abrir VS Code
2. Abrir Extensions (`Ctrl+Shift+X`)
3. Buscar `@mcp` o ejecutar comando:
   ```
   Ctrl+Shift+P → "MCP: Browse Servers"
   ```
4. Buscar **"GitHub"** en la lista
5. Click en **Install**
6. Configurar el token cuando se solicite (ver Paso 1)

### Opción B: Configuración Manual

Abrir `settings.json`:
```
Ctrl+Shift+P → "Preferences: Open User Settings (JSON)"
```

Agregar:
```json
{
  "mcp": {
    "servers": {
      "github": {
        "command": "npx",
        "args": ["-y", "@modelcontextprotocol/server-github"],
        "env": {
          "GITHUB_PERSONAL_ACCESS_TOKEN": "ghp_xxxxxxxxxxxxxxxxxxxx"
        }
      }
    }
  }
}
```

### Opción C: Configuración Segura con Variables de Entorno (Recomendado)

**Windows (PowerShell como Admin):**
```powershell
[System.Environment]::SetEnvironmentVariable("GITHUB_PERSONAL_ACCESS_TOKEN", "ghp_xxx", "User")
```

**Luego en settings.json:**
```json
{
  "mcp": {
    "servers": {
      "github": {
        "command": "npx",
        "args": ["-y", "@modelcontextprotocol/server-github"],
        "env": {
          "GITHUB_PERSONAL_ACCESS_TOKEN": "${env:GITHUB_PERSONAL_ACCESS_TOKEN}"
        }
      }
    }
  }
}
```

### Opción D: Usar Docker

Si prefieres Docker en lugar de npx:

```json
{
  "mcp": {
    "servers": {
      "github": {
        "command": "docker",
        "args": [
          "run", "-i", "--rm",
          "-e", "GITHUB_PERSONAL_ACCESS_TOKEN",
          "ghcr.io/github/github-mcp-server"
        ],
        "env": {
          "GITHUB_PERSONAL_ACCESS_TOKEN": "${env:GITHUB_PERSONAL_ACCESS_TOKEN}"
        }
      }
    }
  }
}
```

---

## ✅ Paso 3: Verificar Instalación

### 3.1 Reiniciar VS Code

Cerrar y abrir VS Code completamente.

### 3.2 Verificar MCP Activo

```
Ctrl+Shift+P → "MCP: List Servers"
```

Debes ver `github` en estado **Running**.

### 3.3 Probar en Copilot Chat

```
@github lista mis repositorios recientes
```

---

## 💬 Ejemplos de Uso

### Repositorios
```
@github lista mis 5 repositorios más recientes
```

```
@github busca repositorios sobre "ticket management system" en mi cuenta
```

### Issues
```
@github lista issues abiertos en mi-usuario/mi-repo
```

```
@github crea un issue en mi-usuario/mi-repo con título "Bug: login no funciona"
```

```
@github busca issues con label "bug" en mi-usuario/mi-repo
```

### Pull Requests
```
@github lista PRs abiertos en mi-usuario/mi-repo
```

```
@github muestra los cambios del PR #42 en mi-usuario/mi-repo
```

```
@github ¿qué PRs tengo pendientes de review?
```

### Código
```
@github busca "CreateTicketCommand" en mi-usuario/mi-repo
```

```
@github muestra el contenido de src/Services/TicketService.cs en mi-usuario/mi-repo
```

### Actions
```
@github muestra el estado del último workflow en mi-usuario/mi-repo
```

```
@github ¿por qué falló el último CI en mi-usuario/mi-repo?
```

---

## 🔐 Permisos por Caso de Uso

| Uso | Permisos Mínimos |
|-----|------------------|
| Solo lectura repos | `public_repo`, `read:user` |
| Issues y PRs | `repo` |
| Organizaciones | `read:org` |
| GitHub Actions | `workflow`, `actions:read` |
| Crear repos | `repo`, `delete_repo` |

---

## ⚠️ Troubleshooting

### Error: "Bad credentials"

1. Verificar que el token no expiró
2. Verificar que copiaste el token completo (empieza con `ghp_`)
3. Regenerar token si es necesario

### Error: "Resource not accessible"

1. El token no tiene los permisos necesarios
2. Regenerar token con scopes correctos
3. Verificar acceso al repositorio específico

### MCP lento o timeout

```json
{
  "mcp": {
    "servers": {
      "github": {
        "command": "npx",
        "args": ["-y", "@modelcontextprotocol/server-github"],
        "env": {
          "GITHUB_PERSONAL_ACCESS_TOKEN": "${env:GITHUB_PERSONAL_ACCESS_TOKEN}"
        },
        "timeout": 60000
      }
    }
  }
}
```

### No encuentra repos privados

Verificar que el token tiene scope `repo` completo (no solo `public_repo`).

---

## 🚀 Cómo Usar en Copilot Chat

Para usar el MCP de GitHub en el panel de chat de Copilot en VS Code, no tienes que escribir un comando tipo "use github". Funciona de forma diferente a los comandos de barra (/).

### Pasos para Usar:

1. **Asegúrate de estar en "Modo Agente"**: En la ventana del chat de Copilot, fíjate en la parte de abajo o en el menú desplegable (a veces dice "Copilot" o "Agent"). Para que las herramientas MCP funcionen, debes estar usando el modo que permite herramientas (generalmente el Agent).

2. **Activa la herramienta**: En la caja de chat, busca un icono de herramientas o un clip ("Attach context"). Si haces clic ahí, deberías ver una lista de "MCP Servers". Asegúrate de que GitHub está activado o seleccionado.

3. **Habla normal**: No escribas "use github". Simplemente pídele lo que quieres hacer. Copilot detectará que necesita usar la herramienta de GitHub y te pedirá permiso.

### Ejemplos:
- "Lista mis últimos issues asignados."
- "Busca el repositorio 'nombre-repo' y resume el README."

### Truco del #:
A veces, si quieres forzar que use una herramienta, puedes escribir # en el chat. Al escribir # te saldrá una lista de contextos y herramientas disponibles. Ahí podrías ver algo relacionado con GitHub si está bien conectado.

## 📚 Recursos

- [GitHub MCP Server Oficial](https://github.com/github/github-mcp-server)
- [Documentación PAT](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens)
- [MCP en VS Code](https://code.visualstudio.com/docs/copilot/customization/mcp-servers)
