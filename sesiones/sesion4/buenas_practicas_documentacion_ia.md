# Buenas Prácticas de Documentación Asistida por IA

> **Guía teórica para documentar código y proyectos con ayuda de GitHub Copilot y herramientas de IA**

---

## 📚 ¿Por qué Documentar con IA?

### El Problema Tradicional

| Desafío | Impacto |
|---------|---------|
| Documentación desactualizada | Desarrolladores desconfían de los docs |
| Tiempo invertido en escribir | Menos tiempo para codificar |
| Inconsistencia de estilos | Difícil de navegar |
| Falta de ejemplos | Curva de aprendizaje alta |

### La Solución con IA

La IA puede **acelerar** la creación de documentación y **mantener consistencia**, pero **requiere supervisión humana** para garantizar precisión.

```
┌─────────────────────────────────────────────────────────────┐
│  FLUJO DE DOCUMENTACIÓN ASISTIDA POR IA                     │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. GENERAR    → IA crea borrador inicial                   │
│  2. REVISAR    → Humano valida exactitud técnica            │
│  3. REFINAR    → IA mejora basándose en feedback            │
│  4. PUBLICAR   → Integrar en flujo de CI/CD                 │
│  5. MANTENER   → IA detecta docs desactualizados            │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 🎯 Principios Fundamentales

### 1. La IA Genera, el Humano Valida

```
❌ INCORRECTO: Copiar documentación generada sin revisar
✅ CORRECTO: Usar IA como primer borrador y verificar detalles
```

**Riesgos de no validar:**
- Documentación incorrecta (alucinaciones de la IA)
- Ejemplos que no compilan
- Referencias a código inexistente

### 2. Contexto es Rey

La calidad de la documentación generada depende directamente del **contexto proporcionado**.

| Sin Contexto | Con Contexto |
|--------------|--------------|
| "Documenta esta función" | "Documenta `CalculateDiscount` para devs que integran nuestra API de pagos" |
| Resultado genérico | Resultado específico y útil |

### 3. Documentación como Código

Tratar la documentación como parte del código:
- **Versionada** en Git junto al código
- **Revisada** en Pull Requests
- **Testeada** (links rotos, ejemplos que compilan)
- **Automatizada** en CI/CD

---

## 📝 Tipos de Documentación y Cómo Generarla

### 1. Documentación de API (OpenAPI/Swagger)

**Cuándo usar IA:**
- Generar schemas OpenAPI desde código
- Crear descripciones de endpoints
- Generar ejemplos de request/response

**Prompt efectivo:**
```
[C] API REST .NET 9 para gestión de tickets
[O] Documentación OpenAPI 3.0 para endpoint POST /api/tickets

Incluir:
- Descripción del endpoint
- Schema del request body (CreateTicketDto)
- Responses: 201, 400, 401, 500
- Ejemplos realistas

[R] NO inventar campos que no existen en el DTO
```

**Validación humana requerida:**
- [ ] Schemas coinciden con DTOs reales
- [ ] Ejemplos son válidos y realistas
- [ ] Códigos de error documentados existen

---

### 2. README.md de Proyecto

**Estructura recomendada:**

```markdown
# Nombre del Proyecto

Descripción breve (1-2 oraciones)

## 🚀 Quick Start
Comandos mínimos para ejecutar

## 📋 Requisitos
Software necesario

## 🔧 Instalación
Pasos detallados

## 📖 Uso
Ejemplos básicos

## 🏗️ Arquitectura
Diagrama y explicación

## 🧪 Testing
Cómo ejecutar tests

## 📚 Documentación Adicional
Links a docs detallados
```

**Prompt efectivo:**
```
[C] Proyecto TicketManagementSystem: .NET 9 backend + Angular 19 frontend
[O] README.md profesional

Secciones: badges, descripción, quick start, instalación, arquitectura (Mermaid)
Comandos específicos para backend/ y frontend/

[R] NO inventar requisitos no mencionados
```

---

### 3. Documentación de Código (XML Comments / JSDoc)

**Cuándo usar IA:**
- Generar comentarios para métodos existentes
- Documentar parámetros y retornos
- Agregar ejemplos de uso

**Ejemplo C# (XML Comments):**
```csharp
/// <summary>
/// Calcula el descuento aplicable a un ticket basado en el tipo de cliente.
/// </summary>
/// <param name="amount">Monto original del ticket. Debe ser mayor a 0.</param>
/// <param name="customerType">Tipo de cliente: "VIP", "Regular", "New".</param>
/// <returns>Monto del descuento a aplicar. Retorna 0 si el tipo no es válido.</returns>
/// <exception cref="ArgumentOutOfRangeException">Si amount es negativo.</exception>
/// <example>
/// <code>
/// var discount = CalculateDiscount(100.0m, "VIP"); // Retorna 20.0m
/// </code>
/// </example>
public decimal CalculateDiscount(decimal amount, string customerType)
```

**Validación humana requerida:**
- [ ] Descripción coincide con comportamiento real
- [ ] Ejemplo compila y es correcto
- [ ] Excepciones documentadas son las que realmente se lanzan

---

### 4. ADRs (Architecture Decision Records)

**Formato estándar:**

```markdown
# ADR-001: Uso de JWT para Autenticación

## Estado
Aceptado

## Contexto
Necesitamos autenticación stateless para nuestra API REST.

## Decisión
Usaremos JWT con refresh tokens.

## Consecuencias

### Positivas
- Stateless, escala horizontalmente
- Estándar ampliamente soportado

### Negativas
- Revocación de tokens compleja
- Tokens pueden ser grandes
```

**Prompt efectivo:**
```
[C] TicketManagementSystem, decisión de autenticación
[O] ADR para elección de JWT vs Sessions vs OAuth

Incluir: contexto, opciones evaluadas, decisión, consecuencias (pros/cons)

[R] Ser objetivo, no solo defender JWT
```

---

## ⚠️ Errores Comunes y Cómo Evitarlos

### 1. Confiar Ciegamente en la IA

| Error | Consecuencia | Solución |
|-------|--------------|----------|
| Copiar sin revisar | Docs incorrectos | Code review de docs |
| No verificar ejemplos | Código que no compila | Ejecutar ejemplos |
| Aceptar versiones inventadas | Incompatibilidades | Validar versiones reales |

### 2. Sobre-documentar

```
❌ EXCESO:
/// <summary>
/// Este método suma dos números. Toma el primer número y lo suma
/// con el segundo número para producir un resultado que es la suma
/// de ambos números.
/// </summary>

✅ CONCISO:
/// <summary>
/// Suma dos números.
/// </summary>
```

**Regla:** Si el código es claro, la documentación debe ser breve.

### 3. Documentación Desactualizada

**Estrategia de mantenimiento:**

```yaml
# En CI/CD: verificar que docs se actualicen con código
- name: Check docs updated
  run: |
    # Si cambió Controllers/, verificar que OpenAPI se actualizó
    if git diff --name-only | grep -q "Controllers/"; then
      if ! git diff --name-only | grep -q "openapi.yaml"; then
        echo "⚠️ Actualizar documentación de API"
        exit 1
      fi
    fi
```

---

## 🔄 Flujo de Trabajo Recomendado

### Al Desarrollar Nueva Funcionalidad

```
1. Escribir código
2. Pedir a Copilot: "Documenta esta función con XML comments"
3. REVISAR y corregir inexactitudes
4. Commit código + documentación juntos
```

### Al Hacer Code Review

```
Checklist de documentación:
[ ] Métodos públicos tienen XML comments
[ ] README actualizado si hay cambios de setup
[ ] OpenAPI actualizado si hay cambios en API
[ ] ADR creado si hay decisión arquitectónica
```

### Mantenimiento Periódico

```
Mensualmente:
1. "Revisa README de TicketManagementSystem, ¿está actualizado?"
2. Verificar links rotos en docs
3. Ejecutar ejemplos de código en docs
4. Actualizar versiones mencionadas
```

---

## 📊 Métricas de Calidad de Documentación

| Métrica | Cómo Medir | Objetivo |
|---------|------------|----------|
| Cobertura | % de métodos públicos documentados | > 90% |
| Freshness | Días desde última actualización | < 30 días |
| Ejemplos válidos | % de ejemplos que compilan | 100% |
| Links funcionales | % de links que responden | 100% |

### Herramientas de Validación

```bash
# Verificar XML comments en .NET
dotnet build /warnaserror:CS1591

# Verificar links en Markdown
npm install -g markdown-link-check
markdown-link-check README.md

# Validar OpenAPI
npm install -g @stoplight/spectral
spectral lint openapi.yaml
```

---

## 🎓 Checklist Final

Antes de considerar documentación "completa":

### Documentación de Proyecto
- [ ] README.md con quick start funcional
- [ ] CONTRIBUTING.md para nuevos desarrolladores
- [ ] CHANGELOG.md actualizado
- [ ] LICENSE file presente

### Documentación de API
- [ ] OpenAPI/Swagger actualizado
- [ ] Ejemplos de request/response
- [ ] Códigos de error documentados
- [ ] Autenticación explicada

### Documentación de Código
- [ ] Métodos públicos con XML comments
- [ ] Clases complejas con resumen
- [ ] Patrones de diseño explicados
- [ ] Configuración documentada

### Calidad
- [ ] Revisada por humano
- [ ] Ejemplos ejecutados
- [ ] Links verificados
- [ ] Versionada en Git

---

## 📚 Recursos Adicionales

- [Microsoft - XML Documentation Comments](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/)
- [OpenAPI Specification](https://swagger.io/specification/)
- [ADR GitHub Template](https://github.com/joelparkerhenderson/architecture-decision-record)
- [Write the Docs - Documentation Guide](https://www.writethedocs.org/guide/)

---

> **Recuerda:** La mejor documentación es la que se **mantiene actualizada** y es **útil para quien la lee**. La IA acelera la creación, pero la calidad depende de la supervisión humana.
