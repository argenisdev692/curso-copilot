---
description: 'Genera tests BDD usando sintaxis Gherkin para escenarios Given-When-Then'
---

# Generador de Tests BDD - Gherkin (Given-When-Then)

## 🎯 Propósito
Generar tests Behavior-Driven Development (BDD) usando sintaxis Gherkin que describan comportamiento del sistema en lenguaje natural legible por stakeholders técnicos y no técnicos.

## 📚 Fundamentos de Gherkin

### Estructura Básica

Un archivo Gherkin (.feature) contiene:
- **Feature**: Descripción high-level de la funcionalidad
- **User Story Format**: As a [rol] / I want [feature] / So that [beneficio]
- **Background**: Precondiciones comunes para todos los escenarios
- **Scenario**: Caso de test específico con Given-When-Then
- **Scenario Outline**: Template con múltiples ejemplos de datos
- **Examples**: Tabla de datos para Scenario Outline

### Keywords de Gherkin

| **Keyword** | **Propósito** | **Uso** |
|-------------|---------------|---------|
| `Feature` | Descripción de funcionalidad | Título y contexto de la feature |
| `Scenario` | Caso de test específico | Un escenario concreto a testear |
| `Scenario Outline` | Template con datos variables | Múltiples casos con misma estructura |
| `Background` | Setup común | Precondiciones para todos los escenarios |
| `Given` | Precondición/estado inicial | Estado del sistema antes de la acción |
| `When` | Acción/evento | Lo que el usuario hace |
| `Then` | Resultado esperado | Verificación del resultado |
| `And` | Continuar keyword anterior | Añadir más pasos del mismo tipo |
| `But` | Resultado negativo | Lo que NO debe pasar |
| `Examples` | Tabla de datos | Datos para Scenario Outline |

## ✍️ Reglas de Escritura Gherkin

### Estilo y Formato
- **Imperativo presente**: "el usuario ingresa", no "el usuario ingresó"
- **Lenguaje de negocio**: Evitar detalles técnicos (IDs de base de datos, clases CSS, selectores)
- **Una acción por paso**: No combinar múltiples acciones en un When
- **Observable outcomes**: Then debe verificar algo visible o medible
- **Perspectiva de usuario**: Describir desde punto de vista del usuario final

### Buenas Prácticas

**✅ Correcto:**
- `Given el usuario está en la página de login`
- `When el usuario ingresa credenciales válidas`
- `Then el usuario ve su perfil`

**❌ Incorrecto:**
- `Given navego a /auth/login` (demasiado técnico)
- `When hago click, escribo, hago click otra vez` (múltiples acciones)
- `Then el div con class='profile' está visible` (detalle de implementación)

### Escenarios a Generar

Para cada Feature, crear escenarios para:

1. **Happy Path** - Flujo exitoso principal
2. **Alternative Paths** - Variaciones válidas del flujo
3. **Error Cases** - Validaciones fallidas, permisos denegados
4. **Edge Cases** - Límites, caracteres especiales, concurrencia

## 🎨 Patterns Comunes de BDD

### Feature: Autenticación de Usuario

Estructura típica:
- User story (As a / I want / So that)
- Background con precondiciones comunes
- Scenario para login exitoso
- Scenario para credenciales inválidas
- Scenario para usuario bloqueado
- Scenario Outline para múltiples intentos fallidos

### Feature: Gestión de Tickets

Estructura típica:
- Scenario para crear ticket exitosamente
- Scenario para crear con datos inválidos
- Scenario para editar ticket propio
- Scenario para editar ticket sin permisos
- Scenario Outline para diferentes estados de tickets

### Feature: Búsqueda y Filtros

Estructura típica:
- Scenario para búsqueda básica
- Scenario para búsqueda sin resultados
- Scenario para aplicar múltiples filtros
- Scenario Outline para diferentes combinaciones de filtros

## 📐 Estructura de Scenarios

### Scenario Simple

Componentes:
- Título descriptivo del escenario
- Given: Estado inicial del sistema
- When: Acción del usuario
- Then: Resultado verificable

### Scenario Outline con Examples

Componentes:
- Título con indicación de que usa múltiples ejemplos
- Given/When/Then con placeholders entre `<>`
- Examples: Tabla con headers y filas de datos

## 📋 Checklist para Gherkin de Calidad

Para cada Feature file, verificar:

- [ ] Feature tiene título claro y user story
- [ ] Background contiene solo precondiciones comunes
- [ ] Cada Scenario tiene título descriptivo
- [ ] Given describe estado inicial claramente
- [ ] When describe UNA acción del usuario
- [ ] Then describe resultado observable
- [ ] No hay detalles de implementación (selectores CSS, IDs)
- [ ] Lenguaje de negocio, no técnico
- [ ] Cobertura: happy path + errores + edge cases
- [ ] Scenario Outline usado para datos repetitivos
- [ ] Examples con casos representativos

## 🎯 Formato de Prompt para Copilot

```
Genera tests BDD en sintaxis Gherkin para la siguiente funcionalidad:

**Feature**: [nombre de la funcionalidad]

**User Story:**
- As a [rol]
- I want [feature]
- So that [beneficio]

**Escenarios requeridos:**
- Happy path: [descripción del flujo exitoso]
- Error cases: [validaciones, permisos, etc.]
- Edge cases: [límites, caracteres especiales]

**Formato:**
- Usar sintaxis Gherkin estándar
- Lenguaje de negocio (no técnico)
- Given-When-Then claros
- Scenario Outline para datos múltiples
- Background para setup común

**Contexto del sistema:**
- [Descripción breve del módulo/sistema]
- [Roles de usuario relevantes]
- [Estados o condiciones importantes]

**Salida esperada:**
- Archivo .feature completo
- Feature con user story
- Background si es necesario
- Múltiples Scenarios cubriendo casos
- Scenario Outline con Examples si aplica

Funcionalidad a documentar: [descripción]
```

## 📝 Consideraciones Especiales

### Granularidad de Scenarios
- **No muy atómicos**: Combinar steps relacionados
- **No muy amplios**: Un scenario = un comportamiento
- **Balance**: Legibilidad vs cobertura

### Mantenibilidad
- Rehusar steps cuando sea posible
- Mantener steps simples y claros
- Evitar steps muy específicos que cambien frecuentemente
- Usar Background para reducir repetición

### Datos de Test
- Usar datos representativos en Examples
- Incluir casos límite en Examples
- No usar datos reales (PII, contraseñas)
- Considerar data generation para E2E

### Colaboración
- Escribir Gherkin con Product Owners
- Validar que stakeholders entienden scenarios
- Usar como documentación viva
- Actualizar cuando comportamiento cambia

## 🚫 Anti-Patterns a Evitar

- **NO detalles de implementación**: Selectores CSS, IDs técnicos
- **NO steps procedurales**: "Click aquí, luego aquí, luego..."
- **NO assertions técnicas**: "El status code es 200"
- **NO scenarios interdependientes**: Cada uno debe ser independiente
- **NO data específica innecesaria**: "Usuario con ID 12345"
- **NO omitir Given o Then**: Estructura completa siempre
