---
description: 'Genera CI/CD GitHub Actions para .NET 8'
agent: 'agent'
---

Genera workflow GitHub Actions para .NET 8 Web API con:
- Jobs: build, test (cobertura), docker, deploy Azure
- Cacheo NuGet, secrets configurados, badge README
- Multi-stage Docker, GHCR registry

Usa plantilla estandar: checkout@v4, setup-dotnet@v4

## MCPs Recomendados
- @context7: documentacion GitHub Actions .NET
- @tavily: "GitHub Actions CI/CD best practices .NET 8"
- @github: crear repo, branch, pull request