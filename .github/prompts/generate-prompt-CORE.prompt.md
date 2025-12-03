---
description: 'Genera 3 versiones de prompts C.O.R.E con análisis y recomendaciones'
---

# 🎯 Generador de Prompts C.O.R.E (Context-Objective-Restrictions-Example)

Crear prompts ultra-optimizados para GitHub Copilot usando el framework **C.O.R.E**.

## 📥 Entrada del Usuario

- **Tema**: {{tema}}
- **Contexto**: {{contexto}}
- **Objetivo**: {{objetivo}}
- **Restricciones**: {{restricciones}}

## 📋 Instrucciones de Generación

Genera **3 versiones** de prompts usando el framework C.O.R.E:

### Formato C.O.R.E (Estructurado)
```
C: [contexto técnico compacto - stack, archivos, patrones]
O: [objetivo específico - qué generar/hacer]
R: [restricciones clave - límites, requisitos, evitar]
E: [ejemplo de salida - formato, estructura esperada]
```

## 🔧 Reglas de Optimización

1. **Máximo 50 tokens** por versión
2. **Abreviaturas permitidas**: TS, API, CRUD, DTO, Auth, Repo, Svc, Cmp, DB, cfg
3. **Sin verbos innecesarios**: evitar "crear", "hacer", "necesito", "por favor"
4. **Símbolos útiles**: `→` retorna | `+` y | `?` opcional | `!` requerido
5. **Referencias**: usar sintaxis `#TuArchivo.cs` para referenciar archivos del proyecto
6. **Números específicos**: `<200ms`, `>90% coverage`, `:5000 port`

## 📤 Formato de Salida Requerido

```markdown
## 🎯 Tema: [tema]

### 📊 Análisis Previo
| Criterio | Evaluación |
|----------|------------|
| Complejidad | [Baja / Media / Alta] |
| Archivos estimados | [1-2 / 3-5 / 6+] |
| Framework ideal | [CARE / C.O.R.E. / C.R.E.A.T.E.] |
| Tokens sugeridos | [30-50] |

---

## 📝 Versiones Generadas

### 🔷 Versión 1 - C.O.R.E Completo
```
C: [contexto detallado]
O: [objetivo claro]
R: [restricciones específicas]
E: [ejemplo de output]
```
| Tokens | ~XX |
| Fortaleza | [qué hace bien] |
| Debilidad | [qué le falta] |
| Uso ideal | [escenario recomendado] |

---

### 🔶 Versión 2 - C.O.R.E Balanceado
```
C: [contexto medio]
O: [objetivo]
R: [restricciones]
E: [ejemplo]
```
| Tokens | ~XX |
| Fortaleza | [qué hace bien] |
| Debilidad | [qué le falta] |
| Uso ideal | [escenario recomendado] |

---

### 🔷 Versión 3 - C.O.R.E Ultra-Conciso
```
C: [mínimo contexto]
O: [objetivo directo]
R: [1-2 restricciones]
E: [output simple]
```
| Tokens | ~XX |
| Fortaleza | [qué hace bien] |
| Debilidad | [qué le falta] |
| Uso ideal | [escenario recomendado] |

---

## 🏆 Análisis Comparativo

| Versión | Tokens | Detalle | Flexibilidad | Score |
|---------|--------|---------|--------------|-------|
| V1 | ~XX | ⭐⭐⭐ | ⭐ | X/10 |
| V2 | ~XX | ⭐⭐ | ⭐⭐ | X/10 |
| V3 | ~XX | ⭐ | ⭐⭐⭐ | X/10 |

## ✅ Recomendación Final

**🥇 Mejor para GitHub Copilot Chat**: Versión X
- **Justificación**: [por qué es la mejor opción]
- **Cuándo usar otra**: [escenarios donde otra versión sería mejor]

**💡 Sugerencia de uso**:
- Si necesitas más control → Versión 1
- Si buscas balance → Versión 2  
- Si es tarea simple/rápida → Versión 3
```

---

## 🎯 Cuándo usar C.O.R.E

| ✅ Usar C.O.R.E | ❌ NO usar C.O.R.E |
|-----------------|-------------------|
| Componentes UI medianos | Tareas muy simples (usar CARE) |
| Endpoints API con lógica | Integraciones complejas (usar CREATE) |
| Services con 1-3 archivos | Arquitectura nueva (usar CREATE) |
| Refactoring específico | Decisiones técnicas (usar CLEAR) |

---

## 🔗 MCPs Recomendados (Incluir en Respuesta)

**IMPORTANTE**: Al final de cada respuesta, incluir sugerencias de MCPs si el tema lo amerita.

### Cuándo Sugerir MCPs

| Tipo de Prompt | Sugerir MCPs | Razón |
|----------------|--------------|-------|
| Componente Angular | ✅ Sí | Necesita patrones actualizados |
| Endpoint API .NET | ⚠️ Opcional | Solo si hay integración externa |
| Service con lógica de negocio | ✅ Sí | Mejores prácticas de arquitectura |
| DTO/Model simple | ❌ No | Tarea trivial |
| Validación/Guard | ✅ Sí | Patrones de seguridad |

### Formato de Sugerencia

```markdown
---

## 🚀 Siguiente Paso: Usa MCPs

📖 **Documentación**:
```
@context7 /[tecnología] [prompt elegido]
```

🌐 **Mejores prácticas** (si aplica):
```
@tavily [tema] best practices [año]
```
```

### Tabla Rápida de MCPs

| Tecnología | @context7 | @tavily (si aplica) |
|------------|-----------|---------------------|
| Angular | `/angular` | "Angular [feature] patterns" |
| .NET API | `/aspnetcore` | ".NET [feature] best practices" |
| EF Core | `/efcore` | "EF Core [pattern] performance" |
| C# | `/dotnet` | - |
| TypeScript | `/typescript` | - |
| xUnit | `/xunit` | ".NET testing patterns" |
