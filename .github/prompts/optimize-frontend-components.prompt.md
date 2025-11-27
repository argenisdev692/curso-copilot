---
description: 'Optimiza componentes Angular aplicando OnPush, trackBy, signals, lazy loading y mejores prácticas de performance'
---

# Optimización de Componentes Frontend Angular

## 🎯 Propósito
Optimizar componentes Angular para mejorar performance, reducir change detection cycles y aplicar mejores prácticas de Angular 17+, sin incluir código de implementación completo.

## 🔍 Análisis de Problemas Comunes

Al analizar un componente, identificar:

### 1. Change Detection Ineficiente
- Uso de `ChangeDetectionStrategy.Default` (verifica TODO el árbol)
- `*ngFor` sin `trackBy` function (re-renderiza elementos innecesariamente)
- Funciones llamadas directamente en templates (se ejecutan en cada cycle)
- Subscriptions que no aprovechan OnPush strategy

### 2. Estado No Optimizado
- Uso de `BehaviorSubject` en lugar de Signals de Angular 17+
- Estado mutable modificado directamente sin inmutabilidad
- Sin `computed` values para valores derivados
- Falta de `effect()` para side effects

### 3. Carga No Optimizada
- Módulos cargados eagerly en lugar de lazy loading
- Componentes grandes sin code splitting
- Imágenes sin lazy loading
- Bundle size grande sin tree shaking

### 4. Memory Leaks
- Subscriptions sin `unsubscribe` o `takeUntil`
- Event listeners sin cleanup en `ngOnDestroy`
- Timers (`setInterval`, `setTimeout`) sin `clearInterval`/`clearTimeout`
- Componentes que no implementan `OnDestroy`

## 🔧 Reglas de Optimización

### 1. Change Detection Strategy: OnPush

Aplicar cuando:
- Componente es presentacional (solo recibe @Input)
- @Input properties son inmutables o usan Observables/Signals
- No hay mutación directa de objetos
- Estado se maneja de forma predecible

Requerimientos:
- Todos los @Input deben ser inmutables o Observables
- Usar `markForCheck()` cuando actualices estado internamente
- Eventos (`@Output`) funcionan normalmente
- AsyncPipe funciona automáticamente

### 2. TrackBy en *ngFor

Implementar trackBy para:
- Listas que cambian frecuentemente
- Items con identificador único (ID)
- Prevenir re-renderizado innecesario

Función trackBy debe:
- Retornar un identificador único (number o string)
- Ser pura (no depender de estado externo)
- Ser consistente entre renders

### 3. Signals en lugar de RxJS (Angular 17+)

Migrar a Signals cuando:
- Estado local del componente
- Valores derivados simples
- No requieres operadores RxJS complejos
- Quieres sintaxis más simple

Usar Signals para:
- `signal()` - Estado mutable reactivo
- `computed()` - Valores derivados automáticamente
- `effect()` - Side effects reactivos
- En templates sin AsyncPipe

Mantener RxJS para:
- HTTP requests
- WebSocket streams
- Operadores complejos (debounce, switchMap, etc.)
- Lógica asíncrona compleja

### 4. Lazy Loading de Módulos/Componentes

Implementar lazy loading para:
- Rutas que no se usan inmediatamente
- Features administrativas
- Componentes grandes (dashboards, reports)
- Módulos de terceros pesados

Estrategias:
- Lazy load routes con `loadChildren`
- Lazy load components con `loadComponent` (standalone)
- Preloading strategy personalizada para rutas críticas
- Route Guards para proteger rutas lazy

### 5. Virtual Scrolling

Usar CDK Virtual Scrolling cuando:
- Listas con más de 100 items
- Scroll infinito
- Performance de renderizado es crítica

Consideraciones:
- Solo renderiza items visibles en viewport
- Soporta items de altura variable
- Integrar con paginación server-side
- Mantener scroll position al navegar

### 6. Optimización de Queries HTTP

Aplicar:
- **Debounce** en búsquedas (evitar queries en cada tecla)
- **shareReplay()** para compartir responses entre suscriptores
- **Cancelación** de requests en progreso con `takeUntil` o AbortController
- **Prefetch** de datos en route resolvers
- **Cache** para datos que cambian poco

### 7. OnPush + Immutability

Estrategias de inmutabilidad:
- No mutar arrays/objetos directamente
- Usar spread operator para copias
- Usar métodos inmutables: `.map()`, `.filter()`, `.slice()`
- Librerías como Immer para estados complejos

### 8. Smart/Dumb Components Pattern

Separar componentes en:
- **Smart (Container)**: Maneja lógica, servicios, estado
- **Dumb (Presentational)**: Solo renderiza, recibe @Input, emite @Output

Beneficios:
- Componentes presentacionales fáciles de testear
- Reutilización de componentes UI
- Separación de concerns clara

### 9. Optimización de Images

Aplicar:
- Lazy loading con `loading="lazy"`
- Responsive images con `srcset`
- Formatos modernos (WebP, AVIF)
- Optimización de tamaño (compresión)
- Usar Angular Image Directive de Angular 17+

### 10. Bundle Optimization

Reducir bundle size:
- Tree shaking de código no usado
- Lazy load third-party libraries
- Analizar bundle con `webpack-bundle-analyzer`
- Usar production build (`ng build --configuration=production`)
- Code splitting automático de Angular

## 📋 Checklist de Optimización

Para cada componente optimizado, verificar:

- [ ] `ChangeDetectionStrategy.OnPush` aplicado (si es apropiado)
- [ ] `trackBy` function en todos los `*ngFor`
- [ ] No hay funciones llamadas directamente en templates
- [ ] Signals usados para estado local (Angular 17+)
- [ ] `computed()` para valores derivados
- [ ] Subscriptions con `takeUntil` o `AsyncPipe`
- [ ] `ngOnDestroy` implementado si hay cleanup
- [ ] Lazy loading de routes aplicado
- [ ] Virtual scrolling en listas grandes
- [ ] HTTP queries optimizadas (debounce, cache, cancel)
- [ ] Immutability respetada en @Input objects
- [ ] Smart/Dumb pattern aplicado
- [ ] Images con lazy loading
- [ ] Bundle size analizado y optimizado

## 🎯 Formato de Prompt para Copilot

```
Optimiza el componente Angular para performance máxima:

**Componente**: [nombre del componente]

**Problemas identificados:**
- [Change detection ineficiente]
- [*ngFor sin trackBy]
- [BehaviorSubject en lugar de Signals]
- [Memory leaks potenciales]
- [HTTP queries sin debounce]

**Optimizaciones requeridas:**
- Aplicar ChangeDetectionStrategy.OnPush
- Implementar trackBy functions
- Migrar a Signals de Angular 17+
- Agregar cleanup en ngOnDestroy
- Optimizar queries HTTP con debounce/cache
- Implementar lazy loading si aplica
- Refactorizar a Smart/Dumb pattern si es grande

**Métricas a mejorar:**
- Tiempo de renderizado inicial
- Número de change detection cycles
- Bundle size del componente
- Memory footprint

**Salida esperada:**
- Componente optimizado explicando cambios
- Métricas de performance antes/después (estimadas)
- Advertencias sobre breaking changes
- Recomendaciones adicionales

Componente a optimizar: [#file o #selection]
```

## 📝 Consideraciones Especiales

### Cuándo NO usar OnPush
- Componentes que dependen de cambios fuera de Angular (DOM events externos)
- Componentes con lógica muy compleja de detección de cambios
- Cuando el overhead de gestión de inmutabilidad es mayor que el beneficio

### Cuándo NO usar Signals
- Código que requiere operadores RxJS complejos
- Lógica asíncrona con múltiples streams
- Interop con librerías que esperan Observables
- Código existente con mucho RxJS que funciona bien

### Performance vs Complejidad
- No optimizar prematuramente
- Medir primero con Angular DevTools
- Optimizar solo componentes con problemas reales
- Mantener balance entre performance y legibilidad

## 🚫 Anti-Patterns a Evitar

- **NO llamar funciones** en templates: `{{ calculateTotal() }}`
- **NO mutar @Input** objects directamente
- **NO usar OnPush** sin entender inmutabilidad
- **NO olvidar trackBy** en *ngFor dinámicos
- **NO mezclar** Signals y BehaviorSubjects para el mismo propósito
- **NO lazy load** todo (considerar costo de HTTP request)
- **NO sobre-optimizar** componentes simples


