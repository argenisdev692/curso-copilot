# Guía de Características de GitHub Copilot

## 📋 Conceptos Principales

### 1. Chat Instructions (Instrucciones de Chat)

**Definición:** Directrices que definen **CÓMO** debe realizarse el trabajo. Se aplican automáticamente a todas las conversaciones del workspace.

**Características:**
- ✅ Aplicación automática e invisible
- ✅ Alcance: Todo el workspace o subcarpetas específicas
- ✅ Propósito: Establecer reglas y estándares de desarrollo
- ✅ Permanente: No necesitas invocarlas manualmente

**Cuándo usar:**
- Estándares de código del equipo
- Convenciones de nomenclatura
- Patrones arquitectónicos a seguir
- Reglas de documentación

---

### 2. Prompt Files (Archivos de Prompt)

**Definición:** Plantillas reutilizables que definen **QUÉ** debe hacerse para tareas específicas. Se invocan bajo demanda.

**Características:**
- ✅ Activación manual mediante comandos `/nombre`
- ✅ Alcance: Tarea específica
- ✅ Propósito: Automatizar tareas repetitivas
- ✅ Reutilizable: Compartible entre proyectos

**Cuándo usar:**
- Tareas repetitivas específicas
- Generación de código con plantilla
- Revisiones de código especializadas
- Comandos personalizados del equipo

---

## 📁 Estructura de Directorios

```
mi-proyecto/
├── .github/
│   ├── copilot-instructions.md          # Instrucciones globales del proyecto
│   ├── instructions/                     # Instrucciones por área/tecnología
│   │   ├── backend-dotnet.instructions.md
│   │   ├── frontend-angular.instructions.md
│   │   └── database.instructions.md
│   └── prompts/                          # Archivos de prompts reutilizables
│       ├── review-api.prompt.md
│       ├── generate-service.prompt.md
│       └── create-test.prompt.md
├── src/
└── README.md
```

---

## 💡 Ejemplos Prácticos

### Ejemplo 1: Chat Instructions - Archivo Único

**Ubicación:** `.github/copilot-instructions.md`

**Contenido:**
```markdown
# Instrucciones de Desarrollo del Proyecto

## Backend (.NET)

**Arquitectura:**
- Aplicar principios SOLID
- Separación de capas: Controllers → Services → Repositories
- Usar inyección de dependencias para todo

**Código:**
- DTOs para todas las transferencias de datos
- Validaciones con FluentValidation
- Comentarios XML en miembros públicos
- async/await para operaciones I/O

**Logging y Errores:**
- ILogger con contexto estructurado
- ProblemDetails para respuestas de error
- Nunca exponer stack traces en producción

**Testing:**
- Código testeable con interfaces
- Configuración externalizada (appsettings.json)

## Frontend (Angular)

- Componentes standalone por defecto
- TypeScript estricto (strict mode)
- Signals para gestión de estado reactivo
- Estructura modular con feature modules
- RxJS para operaciones asíncronas complejas
- Nomenclatura: PascalCase para clases, camelCase para métodos y propiedades
- Usar servicios para lógica de negocio
- Lazy loading para módulos de funcionalidades

## Base de Datos

- Migraciones versionadas con nombre descriptivo
- Índices en columnas de búsqueda frecuente
- Constraints y relaciones siempre explícitas
- Nunca usar SELECT * en queries
```

---

### Ejemplo 2: Chat Instructions - Múltiples Archivos

**Ubicación:** `.github/instructions/frontend-angular.instructions.md`

**Contenido:**
```markdown
# Instrucciones Frontend Angular

## Estructura de Proyecto

Estructura basada en características (feature-based):
- app/core/ (Servicios singleton, guards, interceptors)
- app/shared/ (Componentes reutilizables, pipes, directivas)
- app/features/ (Módulos de funcionalidades)
- app/models/ (Interfaces y tipos TypeScript)

## Reglas de Código

1. **Componentes:**
   - Preferir standalone components
   - OnPush change detection strategy
   - Smart components (containers) vs Dumb components (presentational)
   - Usar signals para estado reactivo
   - Inyección de dependencias en constructor

2. **Servicios:**
   - Providencia en 'root' por defecto
   - Métodos que retornan Observables
   - Manejo de errores con catchError
   - HttpClient para llamadas API
   - Estado compartido con BehaviorSubject o signals

3. **Directivas y Pipes:**
   - Crear pipes puros cuando sea posible
   - Directivas standalone
   - Nomenclatura descriptiva

4. **Formularios:**
   - Reactive Forms sobre Template-driven
   - Validaciones síncronas y asíncronas
   - FormBuilder para construcción
   - Separar lógica de validación en validators

5. **Routing:**
   - Lazy loading para feature modules
   - Guards para protección de rutas
   - Resolvers para pre-carga de datos
   - Rutas tipadas cuando sea posible

6. **RxJS:**
   - Unsubscribe con async pipe o takeUntil
   - Operators: map, filter, switchMap, debounceTime
   - Evitar nested subscriptions
   - shareReplay para datos compartidos

## Convenciones de Nomenclatura

- Componentes: `feature-name.component.ts`
- Servicios: `feature-name.service.ts`
- Guards: `feature-name.guard.ts`
- Pipes: `feature-name.pipe.ts`
- Módulos: `feature-name.module.ts`

## Buenas Prácticas

- trackBy en *ngFor para listas dinámicas
- Lazy loading de imágenes y módulos
- Uso de Angular Material o biblioteca UI consistente
- Accessibility (ARIA labels, roles)
- Internacionalización preparada (i18n)
- Error boundary para manejo de errores global
```

---

**Ubicación:** `.github/instructions/database.instructions.md`

**Contenido:**
```markdown
# Instrucciones Backend .NET

## Estructura de Proyecto

Todos los proyectos backend deben seguir esta estructura:
- API/ (Controllers, Middleware)
- Application/ (Services, DTOs, Validators)
- Domain/ (Entities, Interfaces)
- Infrastructure/ (Repositories, DbContext)

## Reglas de Código

1. **Controllers:**
   - Solo orquestación, sin lógica de negocio
   - Retornar ActionResult con tipo específico
   - Atributos de ruta explícitos
   - Documentación XML completa

2. **Services:**
   - Interfaces en Application, implementación en Infrastructure
   - Métodos async para operaciones I/O
   - Retornar Result pattern para manejo de errores
   - Logging estructurado en operaciones críticas

3. **Repositories:**
   - Patrón Repository genérico más específicos
   - IQueryable para queries complejas
   - AsNoTracking para operaciones de solo lectura
   - Especificaciones para queries reutilizables

4. **DTOs y Validación:**
   - Crear DTOs para Request y Response
   - FluentValidation para todas las validaciones
   - Mapeos automáticos con AutoMapper o Mapster
   - Validación en pipeline de MediatR si se usa

5. **Manejo de Errores:**
   - ProblemDetails para respuestas consistentes
   - Middleware de excepciones global
   - Logging de errores con contexto
   - Nunca exponer información sensible

## Configuración

- appsettings.json para configuración por ambiente
- User Secrets para desarrollo local
- Variables de entorno en producción
- Opciones fuertemente tipadas con IOptions
```

---

**Ubicación:** `.github/instructions/frontend-angular.instructions.md`