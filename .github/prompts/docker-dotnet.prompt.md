---
description: 'Genera Dockerfile optimizado para .NET 8 Web API'
agent: 'agent'
---

Crea Dockerfile multi-stage .NET 8 Web API + docker-compose:
- Build: sdk:8.0, Runtime: aspnet:8.0
- Usuario non-root, health checks, EXPOSE 8080
- .dockerignore: bin/, obj/, .git, .vs/

Produccion-ready: restore, build, publish optimizado

## MCPs Recomendados
- @context7: documentacion Docker .NET 8
- @tavily: "Docker .NET 8 container optimization best practices"
- @github: crear archivo Dockerfile, pull request