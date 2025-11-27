---
description: 'Migra código JavaScript a TypeScript con tipos estrictos, interfaces y mejores prácticas'
---

# Migración de JavaScript a TypeScript

## 🎯 Propósito
Convertir código JavaScript legacy a TypeScript moderno con tipos estrictos, interfaces bien definidas y aprovechando características de TypeScript 5.x, sin incluir código completo de implementación.

## 🔍 Análisis de Código JavaScript

Al analizar un archivo .js, identificar y reportar:

### 1. Tipado Implícito
- Variables sin tipo explícito
- Parámetros de función sin tipos
- Retornos de función sin tipo anotado
- Uso de `any` implícito que debe ser tipado

### 2. Objetos Sin Estructura
- Objetos literales que necesitan interfaces
- Propiedades dinámicas que requieren index signatures
- Callbacks sin firma de función definida
- Arrays sin tipo específico

### 3. Patrones Deprecados
- Uso de `var` que debe migrarse a `const`/`let`
- Callbacks que deben convertirse a Promises/async-await
- Clases sin modificadores de acceso (public, private, protected)
- Uso de `==` que debe cambiarse a `===`
- Funciones tradicionales que pueden ser arrow functions

### 4. Librerías Sin Tipos
- Imports de librerías que necesitan `@types/`
- Código que requiere definiciones de tipos personalizadas

## 🔧 Reglas de Migración

### 1. Configuración TypeScript Estricta

Configurar `tsconfig.json` con:
- `strict: true` - Habilitar todas las verificaciones estrictas
- `noImplicitAny: true` - No permitir any implícito
- `strictNullChecks: true` - Verificación estricta de null/undefined
- `strictFunctionTypes: true` - Tipos de funciones estrictos
- `strictPropertyInitialization: true` - Inicialización de propiedades obligatoria
- `noImplicitReturns: true` - Todas las rutas deben retornar valor
- `noFallthroughCasesInSwitch: true` - No permitir fallthrough en switch
- Target: ES2022 o superior
- Module: ESNext o CommonJS según necesidad

### 2. Transformación de Variables

Reglas de conversión:
- **var → const/let**: Analizar si la variable cambia o no
  - Variables que no cambian → `const`
  - Variables que cambian → `let`
  - Agregar tipo explícito si no es inferible obviomente

- **Tipos para variables**: 
  - Primitivos: string, number, boolean, null, undefined
  - Arrays: `Type[]` o `Array<Type>`
  - Objetos: Crear interface cuando tenga estructura definida

### 3. Funciones con Tipos Explícitos

Transformaciones requeridas:
- Agregar tipos a todos los parámetros
- Especificar tipo de retorno explícitamente
- Convertir callbacks a arrow functions tipadas
- Para funciones asíncronas, retornar `Promise<Type>`
- Usar parámetros opcionales (`param?: Type`) cuando sea apropiado
- Usar parámetros por defecto cuando tenga sentido

### 4. Interfaces y Types

Crear interfaces para:
- Objetos con estructura definida (DTOs, modelos)
- Props de componentes
- Respuestas de API
- Configuraciones de objetos

Usar `type` para:
- Union types (`type Status = 'open' | 'closed'`)
- Intersection types (`type A & B`)
- Tipos primitivos con alias
- Tipos de funciones complejos

### 5. Clases con Modificadores

Agregar a clases:
- Modificadores de acceso: `public`, `private`, `protected`
- Readonly para propiedades inmutables
- Tipos en todas las propiedades
- Tipos en el constructor
- Tipos en métodos
- Abstract classes cuando sea apropiado

### 6. Enums y Constantes

Convertir:
- Objetos con constantes → `enum` o `const enum`
- Strings mágicos repetidos → `enum` de strings
- Números mágicos → constantes tipadas

Considerar:
- Union types como alternativa a enums pequeños
- `as const` para objetos inmutables

### 7. Manejo de Null y Undefined

Aplicar:
- Usar tipos nullable explícitos: `Type | null` o `Type | undefined`
- Optional chaining: `obj?.property`
- Nullish coalescing: `value ?? defaultValue`
- Non-null assertion operator `!` solo cuando estés 100% seguro
- Type guards para verificar null/undefined antes de usar

### 8. Generics

Implementar generics en:
- Funciones que trabajan con múltiples tipos
- Interfaces de repositorios (`IRepository<T>`)
- Respuestas de API (`ApiResponse<T>`)
- Utilidades de transformación de datos

### 9. Async/Await

Migrar callbacks a:
- Promises donde tenga sentido
- async/await para código más limpio
- Tipar correctamente: `Promise<Type>`
- Manejar errores con try/catch
- Evitar callback hell

### 10. Type Guards y Type Narrowing

Implementar:
- Type guards personalizados: `function isType(obj: unknown): obj is Type`
- `typeof` checks para primitivos
- `instanceof` checks para clases
- Discriminated unions con `type` property

## 📋 Checklist de Migración

Para cada archivo migrado, verificar:

- [ ] Extensión cambiada de .js a .ts
- [ ] Todas las variables tienen tipos (explícitos o inferidos)
- [ ] Todos los parámetros de función tienen tipos
- [ ] Todos los retornos de función tienen tipos explícitos
- [ ] Objetos estructurados tienen interfaces
- [ ] Uso de `const`/`let` apropiadamente (no `var`)
- [ ] Igualdad estricta (`===`) en lugar de `==`
- [ ] Clases con modificadores de acceso
- [ ] Propiedades de clase tipadas
- [ ] Enums para constantes relacionadas
- [ ] Manejo de null/undefined con tipos nullable
- [ ] Callbacks convertidos a Promises/async-await cuando apropiado
- [ ] Generics implementados donde beneficie
- [ ] Type guards para validaciones runtime
- [ ] Sin `any` explícito (solo si es absolutamente necesario)
- [ ] Compilación exitosa con strict mode

## 🎯 Formato de Prompt para Copilot

```
Migra el siguiente código JavaScript a TypeScript con tipos estrictos:

**Archivo a migrar**: [nombre del archivo]

**Requisitos de migración:**
- TypeScript 5.x con strict mode
- Interfaces para todas las estructuras de objetos
- Tipos explícitos en funciones (parámetros y retorno)
- Enums para constantes relacionadas
- Convertir callbacks a async/await donde sea apropiado
- Modificadores de acceso en clases
- Manejo de null/undefined con tipos nullable
- Generics donde mejore la reutilización

**Transformaciones específicas:**
- var → const/let según mutabilidad
- == → === (igualdad estricta)
- Callbacks → Promises/async-await
- Objetos sin estructura → Interfaces
- Strings/números mágicos → Enums o constantes tipadas

**Salida esperada:**
- Código TypeScript completo y compilable
- Lista de interfaces/types creados
- Explicación de cambios significativos
- Advertencias sobre conversiones que requieren revisión manual

Código JavaScript: [#selection o pegar código]
```

## 📝 Consideraciones Especiales

### Librerías de Terceros
- Instalar `@types/` packages cuando estén disponibles
- Crear archivos `.d.ts` para librerías sin tipos
- Usar `declare module` para módulos sin definiciones

### Código Legacy Complejo
- Migrar incrementalmente (archivo por archivo)
- Permitir `any` temporalmente si es necesario, con TODO
- Agregar comentarios `// @ts-ignore` solo como último recurso
- Priorizar tipado de APIs públicas sobre código interno

### Testing
- Migrar tests después de código de producción
- Usar tipos de testing framework (Jest, Mocha, etc.)
- Mantener la misma cobertura de tests

### Performance
- `const enum` para enums que se pueden inline
- Tipos bien definidos ayudan a Tree Shaking
- Evitar tipos muy complejos que aumenten tiempo de compilación

## 🚫 Anti-Patterns a Evitar

- **NO usar `any`** a menos que sea absolutamente necesario
- **NO usar `@ts-ignore`** como solución permanente
- **NO crear interfaces demasiado genéricas** (todo opcional)
- **NO ignorar null checks** porque "nunca va a ser null"
- **NO usar type assertions** (`as Type`) sin validación previa
- **NO mezclar callbacks y Promises** en el mismo código
