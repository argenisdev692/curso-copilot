# 🔍 Guía de Configuración: Tavily MCP Server

> **Versión**: Noviembre 2025  
> **Requisitos**: VS Code, GitHub Copilot, Node.js 18+

---

## 📋 ¿Qué es Tavily MCP?

Tavily es un motor de búsqueda optimizado para IA. El MCP Server permite a Copilot buscar información actualizada en la web:

| Capacidad | Descripción |
|-----------|-------------|
| **Web Search** | Búsqueda web con resultados estructurados |
| **Extract** | Extraer contenido de URLs específicas |
| **News** | Búsqueda de noticias recientes |
| **Research** | Investigación profunda con múltiples fuentes |
| **Crawl** | Rastrear sitios web |
| **Map** | Mapear estructura de sitios |

---

## 🔧 Paso 1: Obtener API Key de Tavily

### 1.1 Crear Cuenta

1. Ir a [Tavily.com](https://tavily.com)
2. Click en **Get Started** o **Sign Up**
3. Registrarse con email o GitHub

### 1.2 Obtener API Key

1. Ir al [Dashboard](https://app.tavily.com)
2. En la sección **API Keys**, copiar tu key
3. Formato: `tvly-xxxxxxxxxxxxxxxxxxxxxxxx`

### 1.3 Plan Gratuito

Tavily ofrece un plan gratuito con:
- **1,000 búsquedas/mes** gratis
- Suficiente para desarrollo y pruebas

---

## ⚙️ Paso 2: Configurar en VS Code

### 2.1 Configuración Básica

Abrir `settings.json`:
```
Ctrl+Shift+P → "Preferences: Open User Settings (JSON)"
```

Agregar:
```json
{
  "mcp": {
    "servers": {
      "tavily": {
        "command": "npx",
        "args": ["-y", "tavily-mcp@latest"],
        "env": {
          "TAVILY_API_KEY": "tvly-xxxxxxxxxxxxxxxxxxxx"
        }
      }
    }
  }
}
```

### 2.2 Configuración Segura (Recomendado)

**Windows (PowerShell como Admin):**
```powershell
[System.Environment]::SetEnvironmentVariable("TAVILY_API_KEY", "tvly-xxx", "User")
```

**Luego en settings.json:**
```json
{
  "mcp": {
    "servers": {
      "tavily": {
        "command": "npx",
        "args": ["-y", "tavily-mcp@latest"],
        "env": {
          "TAVILY_API_KEY": "${env:TAVILY_API_KEY}"
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

Debes ver `tavily` en estado **Running**.

### 3.3 Probar en Copilot Chat

```
@tavily busca las últimas novedades de .NET 9
```

---

## 💬 Ejemplos de Uso

### Búsqueda Web General
```
@tavily busca "mejores prácticas Entity Framework Core 2025"
```

```
@tavily ¿qué es nuevo en Angular 19?
```

### Búsqueda de Noticias
```
@tavily noticias recientes sobre GitHub Copilot
```

```
@tavily últimas actualizaciones de seguridad en .NET
```

### Investigación Técnica
```
@tavily investiga las diferencias entre RabbitMQ y Azure Service Bus
```

```
@tavily compara JWT vs OAuth2 vs OpenID Connect para autenticación API
```

### Extraer Contenido de URL
```
@tavily extrae el contenido principal de https://docs.microsoft.com/en-us/aspnet/core/
```

### Documentación Actualizada
```
@tavily busca la documentación oficial de FluentValidation para .NET 8
```

```
@tavily ¿cómo configurar CORS en .NET 9?
```

### Resolver Errores
```
@tavily busca solución para "System.InvalidOperationException: Unable to resolve service"
```

```
@tavily cómo resolver error CORS en Angular llamando a API .NET
```

### Ejemplos para TicketManagementSystem

Basado en tu proyecto de gestión de tickets, aquí van búsquedas útiles con Tavily:

#### Mejores Prácticas .NET y EF Core
```
@tavily busca mejores prácticas para implementar CQRS en .NET 8 con MediatR
```

```
@tavily investiga patrones de soft delete en Entity Framework Core 2025
```

```
@tavily compara diferentes estrategias de logging en ASP.NET Core (Serilog vs Microsoft.Extensions.Logging)
```

#### Autenticación y Seguridad
```
@tavily busca últimas mejores prácticas para JWT refresh tokens en APIs .NET
```

```
@tavily investiga cómo implementar rate limiting en ASP.NET Core 8
```

```
@tavily compara OAuth2 vs JWT para autenticación en aplicaciones web modernas
```

#### Frontend Angular
```
@tavily busca mejores prácticas para formularios reactivos en Angular 18
```

```
@tavily investiga cómo optimizar performance en aplicaciones Angular con lazy loading
```

```
@tavily compara Tailwind CSS vs Angular Material para UI components
```

#### Testing y Calidad
```
@tavily busca estrategias de testing para APIs REST en .NET usando xUnit
```

```
@tavily investiga herramientas para integration testing en Angular
```

#### Base de Datos y ORM
```
@tavily busca mejores prácticas para migraciones en Entity Framework Core
```

```
@tavily investiga optimizaciones de queries en EF Core para aplicaciones de alta carga
```

#### Despliegue y DevOps
```
@tavily busca guías para desplegar aplicaciones .NET + Angular en Azure
```

```
@tavily investiga configuraciones de CI/CD para proyectos .NET con GitHub Actions
```

---

## 🎯 Herramientas Disponibles

El MCP de Tavily expone estas herramientas:

### tavily-search
Búsqueda web general.
```
@tavily busca [tu query]
```

### tavily-extract  
Extrae contenido de URLs específicas.
```
@tavily extrae contenido de [URL]
```

### tavily-crawl
Rastrea un sitio web.
```
@tavily rastrea [URL base] buscando [patrón]
```

### tavily-map
Mapea la estructura de un sitio.
```
@tavily mapea la estructura de [URL]
```

---

## ⚙️ Opciones Avanzadas

### Configurar Profundidad de Búsqueda

```json
{
  "mcp": {
    "servers": {
      "tavily": {
        "command": "npx",
        "args": ["-y", "tavily-mcp@latest"],
        "env": {
          "TAVILY_API_KEY": "${env:TAVILY_API_KEY}"
        }
      }
    }
  }
}
```

### Filtrar Dominios

En los prompts puedes especificar:
```
@tavily busca "CQRS pattern" solo en sitios: microsoft.com, medium.com
```

### Búsqueda con Fecha

```
@tavily noticias de GitHub Copilot de la última semana
```

---

## ⚠️ Troubleshooting

### Error: "Invalid API Key"

1. Verificar que copiaste la key completa
2. La key debe empezar con `tvly-`
3. Verificar en el dashboard que la key esté activa

### Error: "Rate limit exceeded"

1. Plan gratuito: 1,000 búsquedas/mes
2. Esperar al siguiente mes o upgrade de plan
3. Verificar uso en el dashboard

### Resultados vacíos

1. Query muy específica, ampliar términos
2. Probar con términos en inglés
3. Verificar que el MCP está running

### MCP no responde

```powershell
# Verificar que el paquete se instala correctamente
npx tavily-mcp@latest --version
```

Si falla, instalar globalmente:
```powershell
npm install -g tavily-mcp
```

Y cambiar configuración:
```json
{
  "mcp": {
    "servers": {
      "tavily": {
        "command": "tavily-mcp",
        "args": [],
        "env": {
          "TAVILY_API_KEY": "${env:TAVILY_API_KEY}"
        }
      }
    }
  }
}
```

---

## 🔗 Combinando con Otros MCPs

### Tavily + GitHub
```
@tavily busca cómo implementar GitHub Actions para .NET 9
Luego: @github crea un workflow básico basándote en esa información
```

### Tavily + Supabase
```
@tavily busca mejores prácticas para índices en PostgreSQL
Luego: @supabase crea índices en mi tabla tickets según esas recomendaciones
```

---

## 💰 Planes de Tavily

| Plan | Búsquedas/mes | Precio |
|------|---------------|--------|
| Free | 1,000 | $0 |
| Basic | 10,000 | $20/mes |
| Pro | 100,000 | $100/mes |
| Enterprise | Ilimitado | Contactar |

Para desarrollo y curso, el plan **Free** es suficiente.

---

## 🚀 Cómo Usar en Copilot Chat

Para usar el MCP de Tavily en el panel de chat de Copilot en VS Code, no tienes que escribir un comando tipo "use tavily". Funciona de forma diferente a los comandos de barra (/).

### Pasos para Usar:

1. **Asegúrate de estar en "Modo Agente"**: En la ventana del chat de Copilot, fíjate en la parte de abajo o en el menú desplegable (a veces dice "Copilot" o "Agent"). Para que las herramientas MCP funcionen, debes estar usando el modo que permite herramientas (generalmente el Agent).

2. **Activa la herramienta**: En la caja de chat, busca un icono de herramientas o un clip ("Attach context"). Si haces clic ahí, deberías ver una lista de "MCP Servers". Asegúrate de que Tavily está activado o seleccionado.

3. **Habla normal**: No escribas "use tavily". Simplemente pídele lo que quieres hacer. Copilot detectará que necesita usar la herramienta de Tavily y te pedirá permiso.

### Ejemplos:
- "Busca las últimas novedades de .NET 9."
- "¿Qué es nuevo en Angular 19?"
- "Busca solución para 'System.InvalidOperationException: Unable to resolve service'."

### Truco del #:
A veces, si quieres forzar que use una herramienta, puedes escribir # en el chat. Al escribir # te saldrá una lista de contextos y herramientas disponibles. Ahí podrías ver algo relacionado con Tavily si está bien conectado.

## 📚 Recursos

- [Tavily Website](https://tavily.com)
- [Tavily API Docs](https://docs.tavily.com)
- [Tavily MCP Package](https://www.npmjs.com/package/tavily-mcp)
- [MCP en VS Code](https://code.visualstudio.com/docs/copilot/customization/mcp-servers)
