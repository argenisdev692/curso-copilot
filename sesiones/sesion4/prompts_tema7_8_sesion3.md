# 1. Components Angular 1

Para una aplicación Angular con autenticación, gestión de APIs REST y múltiples módulos CRUD, ¿qué arquitectura y patrones de diseño recomiendas?

Considera:
- Autenticación y autorización
- Consumo de APIs REST
- Gestión de estado
- Interceptores HTTP
- Guards de rutas
- Escalabilidad y mantenibilidad

# 2. Components Angular 2

Genera los componentes de autenticación siguiendo la arquitectura y patrones que sugeriste.

Sigue las convenciones de estructura que definiste anteriormente, guiate del archive markdown.

Opcional:

Basándote en los archivos [archivo1.md] y [archivo2.md], genera los componentes de autenticación divididos en 3 partes y confirmame cuando inicies la siguiente parte @workspace

---

# 🚀 Prompts Avanzados para Copilot en Angular - TicketManagementSystem Frontend

Este documento contiene prompts avanzados del Tema 4 para funcionalidades especializadas en `ticket-system-app`.

# 🚀 Prompts Avanzados para Copilot en Angular - TicketManagementSystem Frontend

Este documento contiene prompts avanzados del Tema 4 para funcionalidades especializadas en `ticket-system-app`.

---


## 🔄 Refactorización y Simplificación de Componentes Complejos

"Refactoriza el componente TicketListComponent existente para mejorar su mantenibilidad y performance. El componente actual tiene lógica compleja que necesito simplificar:

1. **Separación de responsabilidades:**
   - Extrae lógica de filtros a un servicio separado
   - Crea un servicio para manejo de paginación
   - Mueve lógica de carga de datos a un custom hook o servicio

2. **Componentes más pequeños:**
   - `TicketFiltersComponent` para la sección de filtros
   - `TicketTableComponent` para la tabla de tickets
   - `PaginationComponent` reutilizable
   - `LoadingSpinnerComponent` genérico

3. **Mejora de performance:**
   - Implementa OnPush change detection
   - Usa trackBy functions en *ngFor
   - Lazy loading de datos con Intersection Observer
   - Memoización de cálculos con computed signals

4. **Mejor manejo de estado:**
   - Centraliza estado en un servicio de estado (facade pattern)
   - Usa signals para estado local reactivo
   - Implementa patrón Command para acciones

5. **Mejor UX:**
   - Skeleton loaders durante carga inicial
   - Infinite scroll opcional
   - Mejor manejo de errores con retry
   - Estados de empty state mejorados

Refactoriza manteniendo la funcionalidad existente pero mejorando la arquitectura. Usa las mejores prácticas de Angular 19 con standalone components y signals."

---

## 📚 Generación Automática de Documentación de Componentes y Servicios

"Genera documentación técnica completa para TicketManagementSystem frontend usando herramientas de documentación automática. Necesito:

1. **Documentación de componentes:**
   - JSDoc comments para cada componente con descripción, inputs, outputs, ejemplos de uso
   - Documentación de @Input y @Output properties con tipos y propósito
   - Ejemplos de uso de componentes (ticket-list, ticket-form, etc.)
   - Descripción de signals y computed properties con su propósito

2. **Documentación de servicios:**
   - Documentación completa de ticket.service.ts y user.service.ts
   - Descripción de cada método público con parámetros y retorno
   - Ejemplos de uso de cada servicio desde componentes
   - Documentación de manejo de estado con signals
   - Patrones RxJS utilizados y su propósito

3. **Configuración Compodoc:**
   - Setup completo de Compodoc para Angular
   - Scripts en package.json para generar y servir documentación
   - Configuración de tsconfig.doc.json optimizada
   - Temas personalizados para la documentación

4. **README del proyecto:**
   - README.md completo del frontend con arquitectura de la aplicación
   - Diagramas de estructura de carpetas y flujo de datos
   - Guía de inicio rápido para desarrolladores
   - Scripts disponibles y su uso
   - Convenciones de código y patrones utilizados

5. **Storybook para componentes visuales:**
   - Setup de Storybook para documentación de componentes UI
   - Stories para componentes standalone (TicketList, TicketForm, Pagination)
   - Variantes y estados de cada componente
   - Controles interactivos para testing visual

Usa TypeDoc/Compodoc para documentación automática, genera comentarios JSDoc detallados siguiendo estándares de Angular. Integra la generación de docs en el pipeline de desarrollo."

---

## 🏛️ Aplicación de Patrones de Diseño y Estructura de Carpetas Sugeridos por Copilot

"Mejora la arquitectura de TicketManagementSystem frontend aplicando patrones de diseño profesionales. Necesito:

1. **Patrón Repository:**
   - Implementa el patrón Repository para abstraer acceso a datos
   - Crea TicketRepository y UserRepository que encapsulen llamadas HTTP
   - Implementa cache strategy en repositories
   - Manejo centralizado de errores en repositories

2. **Patrón Facade:**
   - Crea TicketFacade que simplifique interacción con múltiples servicios
   - Centraliza lógica de negocio compleja (filtros + paginación + estado)
   - Expone API simplificada para componentes
   - Manejo de estado global de tickets

3. **Patrón State Management:**
   - Implementa state management con signals de Angular
   - Store pattern para estado global (tickets, users, auth)
   - Actions y reducers usando signals
   - Devtools integration para debugging

4. **Patrón Smart/Dumb Components:**
   - Refactoriza componentes en Smart (containers) y Dumb (presentational)
   - Smart: ticket-list-container, ticket-form-container
   - Dumb: ticket-table, ticket-card, ticket-filters UI-only components
   - Comunicación mediante @Input/@Output claramente definidos

5. **Estructura de carpetas enterprise:**
   - Reorganiza proyecto con estructura feature-based:
   ```
   src/app/
   ├── core/              # Servicios singleton, guards, interceptors
   ├── shared/            # Componentes, pipes, directivas compartidas
   ├── features/
   │   ├── tickets/       # Feature completo de tickets
   │   │   ├── api/       # Repositories, DTOs
   │   │   ├── state/     # State management
   │   │   ├── components/# Smart & Dumb components
   │   │   └── services/  # Business logic
   │   ├── auth/          # Feature de autenticación
   │   └── users/         # Feature de usuarios
   └── layout/            # Layout components
   ```

6. **Patrón Dependency Injection avanzado:**
   - InjectionTokens para configuración
   - Factory providers para servicios complejos
   - Multi-providers para estrategias
   - Tree-shakeable providers

Reestructura el proyecto completo manteniendo funcionalidad existente. Genera archivos índice (barrel exports) para cada módulo. Documenta la arquitectura resultante."

---
## 🔄 Consejos de Migración Asistida por IA entre Versiones de Angular y TypeScript

"Ayúdame a preparar y ejecutar la migración de TicketManagementSystem frontend de Angular 19 a Angular 20 (cuando esté disponible). Necesito:

1. **Análisis pre-migración:**
   - Audit completo de dependencias actuales
   - Identificación de código deprecated en Angular 19
   - Verificación de compatibilidad de librerías de terceros
   - Backup y plan de rollback
   - Tests baseline para comparación post-migración

2. **Preparación del código:**
   - Actualiza código a las últimas features de Angular 19
   - Elimina uso de APIs deprecated
   - Convierte todo a standalone components si no lo está
   - Moderniza uso de signals en lugar de observables donde aplique
   - Actualiza control flow syntax (@if, @for, @switch)

3. **Plan de migración Angular 19 → 20:**
   - Análisis de breaking changes anunciados
   - Estrategia de migración incremental
   - Uso de Angular CLI update schematics
   - Verificación de nuevas features disponibles
   - Testing exhaustivo post-cada paso

4. **Actualización TypeScript:**
   - Migración a la última versión de TypeScript compatible con Angular 20
   - Aprovecha nuevas features de TypeScript 5.x
   - Actualiza tsconfig.json con opciones modernas
   - Mejora type safety con nuevas capacidades

5. **Modernización de RxJS:**
   - Actualiza a RxJS 8.x o superior
   - Migración completa a operadores pipeable
   - Conversión a signals donde sea más apropiado
   - Mejor manejo de subscriptions con takeUntilDestroyed
   - Eliminación de operadores deprecated

6. **Nuevas features de Angular 20:**
   - Implementa nuevas APIs y mejoras
   - Aprovecha optimizaciones de rendimiento
   - Actualiza a nuevos patterns recomendados por el equipo Angular
   - Mejoras en hydration, SSR, y performance

7. **Testing regression:**
   - Suite completa de tests e2e
   - Tests de integración críticos
   - Performance benchmarks (comparar con Angular 19)
   - Verificación de funcionalidad en diferentes navegadores
   - Mobile testing y responsive

8. **Monitoreo post-migración:**
   - Setup de monitoring de errores (Sentry/LogRocket)
   - Análisis de bundle size changes
   - Performance metrics (Core Web Vitals)
   - User feedback y bug reporting
   - Plan de hotfixes rápidos

9. **Estrategia de mantenimiento continuo:**
   - Script de verificación de dependencias obsoletas
   - Plan de actualización trimestral
   - Documentación de cambios por versión
   - Proceso para mantenerse actualizado con prereleases
   - Contribución a la comunidad con findings

10. **Checklist de migración:**
    - [ ] Backup completo del código
    - [ ] Tests passing al 100%
    - [ ] Dependencias auditadas
    - [ ] Plan de comunicación con equipo
    - [ ] Ambiente staging preparado
    - [ ] Rollback strategy documentado
    - [ ] Performance baseline establecido
    - [ ] Migración ejecutada en staging
    - [ ] Tests regression completos
    - [ ] Aprobación de stakeholders
    - [ ] Deploy gradual a producción
    - [ ] Monitoreo intensivo primeros días

Genera un plan de migración detallado específico para TicketManagementSystem. Incluye comandos exactos de Angular CLI, archivos a modificar, y scripts de verificación. Prioriza cambios por impacto y riesgo. Considera rollback en cada etapa."

---

## 🔍 Detección de code smells asistida por Copilot

**Objetivo:** Identificar y corregir patrones de código problemáticos en el proyecto.

#### 💬 Prompt:
> Analiza el código del proyecto `TicketManagementSystem` y detecta code smells comunes. Para cada smell identificado, proporciona el archivo, línea aproximada, descripción del problema y una sugerencia de refactorización aplicando principios SOLID y Clean Code. Prioriza smells que afecten mantenibilidad y performance.

---

## ⚡ Mejora de consultas y rendimiento en .NET y Angular

**Objetivo:** Optimizar consultas de base de datos y rendimiento frontend.

#### 💬 Prompt:
> Revisa las consultas EF Core en el proyecto `TicketManagementSystem` y identifica problemas de performance como N+1 queries, falta de índices, o proyecciones ineficientes. Propón refactorizaciones específicas para cada caso, incluyendo código antes/después y métricas de mejora esperadas.

---

## 🔄 Reescritura de código legacy con prompts específicos

**Objetivo:** Modernizar código antiguo siguiendo estándares actuales.

#### 💬 Prompt:
> Identifica código legacy en el proyecto `TicketManagementSystem` (ej. .NET Framework antiguo, JavaScript vanilla, o patrones obsoletos) y genera un plan de reescritura paso a paso. Incluye prompts específicos para cada refactorización, considerando compatibilidad backward y testing exhaustivo.

---

## 🎨 Creación de estilos consistentes en frontend

**Objetivo:** Establecer y aplicar convenciones de estilo uniformes.

#### 💬 Prompt:
> Define un sistema de diseño consistente para el frontend del `TicketManagementSystem` incluyendo paleta de colores, tipografía, espaciado, y componentes base. Genera reglas CSS/SCSS reutilizables y un guide de implementación para mantener consistencia en todos los componentes.

---

## 🤖 Automatización de refactorizaciones repetitivas

**Objetivo:** Crear herramientas para refactorizaciones comunes.

#### 💬 Prompt:
> Diseña scripts o herramientas automatizadas para refactorizaciones repetitivas en el `TicketManagementSystem`, como renombrado de propiedades, extracción de métodos comunes, o conversión de patrones. Incluye ejemplos de uso y validaciones para asegurar que no rompan funcionalidad.

---

## 🏗️ Buenas prácticas en mantenibilidad de proyectos

**Objetivo:** Establecer estándares para código mantenible a largo plazo.

#### 💬 Prompt:
> Crea una guía completa de mejores prácticas para mantener el `TicketManagementSystem` a largo plazo. Incluye convenciones de código, patrones de arquitectura, estrategias de testing, documentación, y procesos de code review. Proporciona checklists y métricas para medir mantenibilidad.

---

## 🤝 Estrategias de trabajo colaborativo con Copilot

**Objetivo:** Optimizar el uso de Copilot en equipos de desarrollo.

#### 💬 Prompt:
> Diseña estrategias para que un equipo de 5 desarrolladores use Copilot de manera colaborativa en el `TicketManagementSystem`. Incluye cómo compartir prompts efectivos, dividir tareas entre humanos y IA, y mantener consistencia en el código generado por diferentes miembros.

---

## 👥 Integración de Copilot en pair programming

**Objetivo:** Incorporar Copilot en sesiones de pair programming.

#### 💬 Prompt:
> Describe cómo integrar Copilot en sesiones de pair programming para el `TicketManagementSystem`. Incluye roles específicos para el programador principal, el observador, y Copilot. Proporciona ejemplos de flujos de trabajo donde Copilot acelere la generación de código o testing.

---

## 🔍 Uso de Copilot en code reviews

**Objetivo:** Mejorar la calidad de code reviews con asistencia de IA.

#### 💬 Prompt:
> Crea un proceso de code review asistido por Copilot para el `TicketManagementSystem`. Incluye cómo usar Copilot para analizar pull requests, detectar problemas comunes, sugerir mejoras, y generar comentarios constructivos. Define criterios para cuándo confiar en las sugerencias de Copilot.

---

## 📝 Creación de snippets de equipo con Copilot

**Objetivo:** Desarrollar snippets personalizados para el equipo.

#### 💬 Prompt:
> Genera una colección de snippets personalizados para el equipo del `TicketManagementSystem` usando Copilot. Incluye snippets para patrones comunes como creación de servicios, validaciones, manejo de errores, y componentes Angular. Proporciona instrucciones para instalar y mantener estos snippets.

---

## 🏃 Ejemplo práctico: sprint completo asistido por IA

**Objetivo:** Demostrar un sprint completo usando Copilot.

#### 💬 Prompt:
> Planifica y ejecuta un sprint de 2 semanas para añadir una funcionalidad completa al `TicketManagementSystem` (ej. sistema de notificaciones) usando Copilot intensivamente. Incluye user stories, estimaciones, división de tareas entre humanos y IA, y métricas de productividad.

---

## 🔄 Roles de Copilot en flujos ágiles (Scrum/Kanban)

**Objetivo:** Integrar Copilot en metodologías ágiles.

#### 💬 Prompt:
> Define roles específicos para Copilot en flujos ágiles aplicados al `TicketManagementSystem`. Describe cómo Copilot puede ayudar en planning, daily standups, sprint reviews, y retrospectives. Incluye ejemplos de prompts para cada ceremonia ágil.

---

## 🏢 Uso de Copilot en repositorios monorepo

**Objetivo:** Gestionar proyectos monorepo con Copilot.

#### 💬 Prompt:
> Diseña estrategias para usar Copilot en un monorepo que incluya el `TicketManagementSystem` completo (backend, frontend, infraestructura). Incluye cómo manejar dependencias entre proyectos, generar código consistente, y mantener contexto del proyecto completo.

---

## 📊 Integración de Copilot con GitHub Projects

**Objetivo:** Conectar Copilot con herramientas de gestión de proyectos.

#### 💬 Prompt:
> Configura la integración de Copilot con GitHub Projects para el `TicketManagementSystem`. Incluye cómo usar Copilot para actualizar issues, crear branches automáticamente, generar descripciones de pull requests, y mantener sincronizados los proyectos con el código.

---

## 🔀 Reducción de tiempo en PRs y merges con Copilot

**Objetivo:** Acelerar el proceso de pull requests.

#### 💬 Prompt:
> Crea un workflow asistido por Copilot para reducir el tiempo de revisión y merge de pull requests en el `TicketManagementSystem`. Incluye prompts para generar descripciones automáticas, detectar conflictos, sugerir reviewers, y automatizar merges seguros.

---

## 👥 Buenas prácticas de colaboración en equipo

**Objetivo:** Establecer normas para trabajo en equipo con Copilot.

#### 💬 Prompt:
> Desarrolla un conjunto de mejores prácticas para que el equipo del `TicketManagementSystem` colabore efectivamente usando Copilot. Incluye políticas de uso, capacitación, resolución de conflictos entre código generado por diferentes miembros, y métricas de éxito del equipo.