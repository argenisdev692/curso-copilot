# 📚 Guía de Configuración: Context7 MCP Server

> **Versión**: Noviembre 2025  
> **Requisitos**: VS Code 1.105.1+, GitHub Copilot, Node.js 18+  
> **Paquete NPM**: `@upstash/context7-mcp`

---

## 📋 ¿Qué es Context7 MCP?

Context7 MCP es un servidor de Model Context Protocol desarrollado por **Upstash** que proporciona documentación actualizada y ejemplos de código directamente en tus prompts.

### ❌ Sin Context7 (El Problema)

Los LLM se basan en información obsoleta o genérica sobre las bibliotecas que usas:

| Problema | Descripción |
|----------|-------------|
| **Datos Obsoletos** | Ejemplos de código basados en datos de entrenamiento de hace 1+ año |
| **APIs Falsas** | Alucinaciones de métodos y funciones que no existen |
| **Versiones Antiguas** | Respuestas genéricas para versiones desactualizadas |
| **Sintaxis Incorrecta** | Patrones deprecados o eliminados en versiones recientes |

### ✅ Con Context7 (La Solución)

Context7 MCP extrae documentación y ejemplos de código **actualizados** y **específicos de cada versión** directamente desde la fuente:

| Beneficio | Descripción |
|-----------|-------------|
| **Docs Actualizados** | Documentación oficial en tiempo real |
| **Versión Específica** | Ejemplos para la versión exacta que usas |
| **Código Real** | Snippets funcionales desde repos oficiales |
| **Sin Configuración Extra** | Solo agrega `use context7` al prompt |

---

## 🔧 Paso 1: Requisitos Previos

### 1.1 Verificar Versión de VS Code

```
Help → About
```

**Requerido**: VS Code **1.105.1** o superior.

### 1.2 Verificar Node.js

```powershell
node --version
```

**Requerido**: Node.js **v18.0.0** o superior.

---

## ⚙️ Paso 2: Instalación en VS Code

### Opción A: Instalación desde Marketplace (Recomendado)

1. Abrir VS Code
2. Abrir Extensions (`Ctrl+Shift+X`)
3. Buscar `@mcp` o ejecutar comando:
   ```
   Ctrl+Shift+P → "MCP: Browse Servers"
   ```
4. Buscar **"Context7"** en la lista
5. Click en **Install**

### Opción B: Instalación Manual

Abrir `settings.json`:
```
Ctrl+Shift+P → "Preferences: Open User Settings (JSON)"
```

Agregar la configuración:

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "type": "stdio",
        "command": "npx",
        "args": ["-y", "@upstash/context7-mcp@latest"]
      }
    }
  }
}
```

### Opción C: Instalación con Docker

Si prefieres usar Docker:

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "command": "docker",
        "args": ["run", "-i", "--rm", "context7-mcp"],
        "transportType": "stdio"
      }
    }
  }
}
```

**Dockerfile** necesario:
```dockerfile
FROM node:18-alpine
WORKDIR /app
RUN npm install -g @upstash/context7-mcp
CMD ["context7-mcp"]
```

Construir imagen:
```powershell
docker build -t context7-mcp .
```

---

## ✅ Paso 3: Verificar Instalación

### 3.1 Reiniciar VS Code

Cerrar y abrir VS Code completamente.

### 3.2 Verificar MCP Activo

```
Ctrl+Shift+P → "MCP: List Servers"
```

Debes ver `context7` en estado **Running**.

### 3.3 Probar con MCP Inspector (Opcional)

```powershell
npx -y @modelcontextprotocol/inspector npx @upstash/context7-mcp@latest
```

---

## 💬 Cómo Usar Context7

### Uso Básico: Agregar `use context7`

Simplemente agrega **`use context7`** al final de tu prompt:

```
Crea un middleware Next.js que verifique si hay un JWT válido 
en las cookies y redirija a los usuarios no autenticados a `/login`. use context7
```

```
Configura un script de Cloudflare Worker para almacenar en caché 
las respuestas de la API JSON durante cinco minutos. use context7
```

### Ejemplos por Tecnología

#### 🔷 Next.js / React
```
Crea un componente Server de Next.js 14 que use Server Actions 
para enviar un formulario. use context7
```

```
Implementa el patrón de Parallel Routes en Next.js 14. use context7
```

#### 🟢 Node.js / Express
```
Crea un middleware de rate limiting con express-rate-limit v7. use context7
```

```
Implementa autenticación con Passport.js y estrategia JWT. use context7
```

#### 🔵 TypeScript / Zod
```
Valida un objeto de usuario con Zod incluyendo email, 
password y confirmPassword con refinement. use context7
```

```
Usa Zod para inferir tipos de un schema de API. use context7
```

#### ☁️ Cloudflare Workers
```
Crea un Worker que actúe como proxy inverso con caché. use context7
```

```
Implementa un endpoint de Cloudflare Worker con KV storage. use context7
```

#### 🗄️ Prisma / Drizzle
```
Configura Drizzle ORM con PostgreSQL y migraciones. use context7
```

```
Crea relaciones many-to-many con Prisma. use context7
```

#### 🎨 Tailwind CSS
```
Crea un componente de card responsive con Tailwind CSS v4. use context7
```

#### 🧪 Testing
```
Escribe tests para un componente React con Vitest y Testing Library. use context7
```

### Ejemplos para TicketManagementSystem

Basado en tu proyecto de gestión de tickets (backend .NET con EF Core, CQRS, MediatR; frontend Angular), aquí van ejemplos específicos:

#### Backend .NET / Entity Framework Core
```
Implementa soft delete en mi ApplicationDbContext usando Global Query Filters en EF Core 8. use context7
```

```
Crea un Command Handler para actualizar el estado de un ticket usando MediatR en .NET 8, incluyendo validación con FluentValidation. use context7
```

```
Configura relaciones many-to-many entre Tickets y Tags usando EF Core 8 con Fluent API. use context7
```

#### Autenticación JWT en ASP.NET Core
```
Implementa refresh tokens en ASP.NET Core 8 con JWT Bearer, incluyendo el endpoint en AuthController. use context7
```

```
Crea un middleware de rate limiting usando AspNetCoreRateLimit en .NET 8. use context7
```

#### CQRS y MediatR
```
Implementa un Query para obtener tickets paginados con filtros usando MediatR y EF Core. use context7
```

```
Configura un pipeline behavior para logging automático en MediatR. use context7
```

#### Frontend Angular
```
Crea un componente de tabla responsive para listar tickets usando Angular 18 y Tailwind CSS. use context7
```

```
Implementa un formulario reactivo para crear tickets con validación usando Angular Reactive Forms. use context7
```

#### Testing en .NET
```
Escribe tests unitarios para un servicio de tickets usando xUnit, FluentAssertions y Moq. use context7
```

```
Configura integration tests para controladores API usando WebApplicationFactory en .NET 8. use context7
```

---

## 🛠️ Herramientas Disponibles

Context7 MCP expone las siguientes herramientas que los LLMs pueden usar:

| Herramienta | Descripción |
|-------------|-------------|
| `resolve-library-id` | Resuelve un nombre de librería genérico a un ID compatible con Context7 |
| `get-library-docs` | Obtiene documentación y ejemplos de código para una librería específica |

---

## ⚙️ Configuración Avanzada

### Ajustar Tokens Mínimos

Si necesitas más contexto en las respuestas:

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "type": "stdio",
        "command": "npx",
        "args": ["-y", "@upstash/context7-mcp@latest"],
        "env": {
          "DEFAULT_MINIMUM_TOKENS": "6000"
        }
      }
    }
  }
}
```

### Con API Key (Opcional)

Si tienes una API key de Context7, puedes aumentarla para obtener mejores límites de rate y acceso prioritario.

#### Obtener API Key de Context7

1. Ve al sitio web oficial: [https://context7.com/](https://context7.com/)
2. Regístrate con tu email o cuenta de GitHub
3. Una vez registrado, ve al dashboard: [https://context7.com/dashboard](https://context7.com/dashboard)
4. En la sección de API Keys, genera una nueva key
5. Copia la key (formato: `ctx7-xxxxxxxxxxxxxxxxxxxxxxxx`)

#### Insertar API Key en VS Code

Agrega la key a tu configuración de VS Code:

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "type": "stdio",
        "command": "npx",
        "args": [
          "-y", 
          "@upstash/context7-mcp@latest",
          "--api-key",
          "ctx7-xxxxxxxxxxxxxxxxxxxx"
        ]
      }
    }
  }
}
```

**Nota**: La API key es opcional para uso básico, pero recomendada para proyectos intensivos.

### Timeout Extendido

Para documentaciones grandes:

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "type": "stdio",
        "command": "npx",
        "args": ["-y", "@upstash/context7-mcp@latest"],
        "timeout": 60000
      }
    }
  }
}
```

---

## 🔄 Runtimes Alternativos

### Usando Bun (más rápido)

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "command": "bunx",
        "args": ["-y", "@upstash/context7-mcp@latest"]
      }
    }
  }
}
```

### Usando Deno

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "command": "deno",
        "args": ["run", "--allow-net", "npm:@upstash/context7-mcp"]
      }
    }
  }
}
```

---

## ⚠️ Troubleshooting

### Error: `ERR_MODULE_NOT_FOUND`

**Solución 1**: Cambiar de `npx` a `bunx`:
```json
{
  "command": "bunx",
  "args": ["-y", "@upstash/context7-mcp@latest"]
}
```

**Solución 2**: Remover `@latest`:
```json
{
  "args": ["-y", "@upstash/context7-mcp"]
}
```

### Error: MCP no responde

1. Verificar que Node.js 18+ está instalado
2. Verificar conexión a internet (Context7 necesita acceso a docs remotas)
3. Reiniciar VS Code completamente
4. Verificar con `MCP: List Servers`

### Error: Timeout en documentación

Algunas librerías grandes pueden tardar. Aumentar timeout:
```json
{
  "timeout": 120000
}
```

### Librería no encontrada

Si una librería no está disponible en Context7:
1. Verificar el nombre exacto de la librería
2. Intentar con nombre alternativo (ej: `react-query` vs `@tanstack/react-query`)
3. Submeter la librería en [GitHub de Context7](https://github.com/upstash/context7)

---

## 🆚 Comparación: Sin Context7 vs Con Context7

### Ejemplo: Next.js Middleware

**❌ Sin Context7 (Respuesta típica de LLM):**
```typescript
// Puede usar APIs obsoletas o incorrectas
import { NextResponse } from 'next/server'

export function middleware(request) {
  // Sintaxis de Next.js 12 (obsoleta)
  const token = request.cookies.get('token')
  // ...
}
```

**✅ Con Context7 (Respuesta actualizada):**
```typescript
// Usa la API actual de Next.js 14
import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'

export function middleware(request: NextRequest) {
  // API correcta de Next.js 14
  const token = request.cookies.get('auth-token')?.value
  
  if (!token) {
    return NextResponse.redirect(new URL('/login', request.url))
  }
  
  return NextResponse.next()
}

export const config = {
  matcher: ['/dashboard/:path*', '/api/protected/:path*']
}
```

---

## 🎯 Mejores Prácticas

### 1. Especificar Versión Cuando Sea Crítico
```
Crea un formulario con React Hook Form v7 y Zod. use context7
```

### 2. Combinar con Contexto Local
```
Analiza mi componente en #file:UserForm.tsx y mejóralo 
usando las mejores prácticas de React Hook Form. use context7
```

### 3. Pedir Comparaciones
```
Muéstrame cómo migrar de getServerSideProps a Server Components 
en Next.js 14. use context7
```

### 4. Solicitar Ejemplos Completos
```
Dame un ejemplo completo de autenticación con NextAuth.js v5 
incluyendo providers y middleware. use context7
```

---

## 📚 Fuentes de Documentación Soportadas

Context7 obtiene documentación de:

| Fuente | Ejemplos |
|--------|----------|
| **Documentación Oficial** | Next.js, React, Vue, Angular, Svelte |
| **NPM/Package Repos** | Cualquier paquete con README detallado |
| **GitHub Repos** | Documentación de proyectos open source |
| **Cloud Services** | AWS, Google Cloud, Azure, Cloudflare |
| **Frameworks** | Express, Fastify, Hono, Nest.js |
| **ORMs** | Prisma, Drizzle, TypeORM, Sequelize |
| **Testing** | Jest, Vitest, Playwright, Cypress |

---

## 📚 Recursos

- [Context7 GitHub (Upstash)](https://github.com/upstash/context7)
- [NPM Package](https://www.npmjs.com/package/@upstash/context7-mcp)
- [VS Code Marketplace Extension](https://marketplace.visualstudio.com/items?itemName=Upstash.context7-mcp)
- [Blog Oficial Upstash](https://upstash.com/blog/context7-mcp)
- [MCP en VS Code Docs](https://code.visualstudio.com/docs/copilot/customization/mcp-servers)
- [MCP Inspector](https://github.com/modelcontextprotocol/inspector)

---

## 🔗 Configuración Completa de Ejemplo

```json
{
  "mcp": {
    "servers": {
      "context7": {
        "type": "stdio",
        "command": "npx",
        "args": ["-y", "@upstash/context7-mcp@latest"],
        "env": {
          "DEFAULT_MINIMUM_TOKENS": "6000"
        },
        "timeout": 60000
      }
    }
  }
}
```

---

> **💡 Tip**: Context7 es **gratuito** para uso personal. Mantenido por el equipo de Upstash.
