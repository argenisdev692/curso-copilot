## 🏗️ Generación de Componentes, Módulos y Rutas de Navegación con IA

"Genera un sistema completo de rutas de navegación para la aplicación TicketManagementSystem en Angular. Considera que ya existe un componente ticket-list y necesito:

1. **Rutas principales:**
   - Dashboard principal (`/`)
   - Lista de tickets (`/tickets`)
   - Detalle de ticket (`/tickets/:id`)
   - Crear ticket (`/tickets/new`)
   - Editar ticket (`/tickets/:id/edit`)
   - Gestión de usuarios (`/users`)
   - Perfil de usuario (`/profile`)

2. **Rutas protegidas:**
   - Implementa guards para autenticación
   - Guards por roles (Admin, Agent, User)
   - Redirección automática al login

3. **Lazy loading:**
   - Módulos separados por features (tickets, users, auth)
   - Configuración de rutas hijas
   - Preloading strategy inteligente

4. **Estructura de componentes:**
   - Componentes standalone donde aplique
   - Layout components (header, sidebar, footer)
   - Page components para cada ruta

5. **Navegación programática:**
   - Servicio de navegación reutilizable
   - Manejo de breadcrumbs
   - Query params y fragmentos

Genera archivos de rutas, guards, servicios de navegación, y componentes de layout. Usa las mejores prácticas de Angular 19 con signals y standalone components. Incluye configuración de router con tracing para desarrollo."

---

## 🌐 Creación de Servicios para Consumo de APIs REST y Manejo de Datos

"Crea un servicio de autenticación completo para TicketManagementSystem que consuma la API REST del backend. El servicio debe incluir:

1. **Métodos de autenticación:**
   - Login con email/password
   - Registro de nuevos usuarios
   - Refresh token automático
   - Logout con limpieza de estado

2. **Manejo de tokens:**
   - Almacenamiento seguro en localStorage/sessionStorage
   - Interceptor para agregar Authorization header
   - Refresh automático antes de expirar
   - Validación de tokens

3. **Estado de autenticación:**
   - Signals para estado reactivo (isAuthenticated, currentUser, userRole)
   - Guards que consuman este estado
   - Auto-login al iniciar la app

4. **Manejo de errores:**
   - Errores específicos de autenticación (401, 403)
   - Retry logic para refresh tokens
   - Mensajes de error user-friendly

5. **Integración con backend:**
   - DTOs que coincidan con los del backend C#
   - Mapeo de respuestas a interfaces TypeScript
   - Validación de respuestas

Usa HttpClient, signals de Angular, y patrones de RxJS. Implementa interceptores para manejo automático de tokens. Incluye métodos para verificar permisos por roles."

---

## 📝 Formularios Reactivos y Validaciones Personalizadas

"Genera un componente completo para crear tickets en TicketManagementSystem usando formularios reactivos de Angular. El componente debe incluir:

1. **Formulario reactivo:**
   - Campos: title, description, priority, assignedTo
   - FormGroup con FormControls tipados
   - Validators integrados y personalizados

2. **Validaciones personalizadas:**
   - Título: requerido, min 5 chars, max 100 chars
   - Descripción: requerida, min 10 chars, max 1000 chars
   - Prioridad: requerida, valores del enum TicketPriority
   - Asignado a: opcional, debe existir en lista de usuarios

3. **Estado del formulario:**
   - Signals para loading, errors, success
   - Mensajes de error específicos por campo
   - Estados de validación visual (valid/invalid/touched)

4. **Interacción con APIs:**
   - Cargar lista de usuarios para asignación
   - Submit con manejo de respuestas
   - Navegación automática al crear exitosamente

5. **UX/UI considerations:**
   - Loading states durante submit
   - Disable form mientras carga
   - Mensajes de éxito/error con snackbar
   - Reset form después de éxito

Usa ReactiveFormsModule, signals, y servicios existentes. Implementa patrones de validación robustos y manejo de errores comprehensivo."

---

## 🔧 Generación de Pipes y Directivas Dinámicas según el Contexto

"Crea pipes y directivas personalizadas para el sistema de tickets TicketManagementSystem. Necesito:

1. **Pipes personalizados:**
   - `priorityColor`: Transforma prioridad a clase CSS (Low->green, Medium->yellow, High->orange, Critical->red)
   - `statusIcon`: Convierte status a icono Unicode apropiado
   - `relativeTime`: Muestra tiempo relativo ('hace 2 horas', 'ayer', etc.)
   - `truncateText`: Trunca texto largo con ellipsis y tooltip

2. **Directivas estructurales:**
   - `*appHasRole`: Muestra contenido solo si usuario tiene rol específico
   - `*appCanEditTicket`: Verifica permisos para editar ticket
   - `*appLoading`: Muestra skeleton loader mientras carga

3. **Directivas de atributo:**
   - `appHighlightSearch`: Resalta texto que coincide con búsqueda
   - `appConfirmClick`: Muestra confirmación antes de ejecutar acción
   - `appAutoFocus`: Auto-focus en input cuando se muestra

4. **Pipes async inteligentes:**
   - `userName`: Resuelve ID de usuario a nombre usando cache
   - `ticketStats`: Calcula estadísticas de tickets en tiempo real

Implementa con standalone APIs de Angular, usa signals donde aplique, y optimiza para performance. Incluye tests unitarios básicos para cada pipe/directiva."

---

## 📋 Creación de Interfaces y Tipos TypeScript con Sugerencias 

"Genera un sistema completo de tipos TypeScript para TicketManagementSystem que mejore las interfaces existentes. Necesito:

1. **Tipos avanzados para API responses:**
   - Generic `ApiResponse<T>` con metadata (success, message, data)
   - `PagedResponse<T>` con información de paginación
   - `ValidationError` con campo específico y mensaje

2. **Unions y discriminated unions:**
   - `TicketStatus` como union type en lugar de enum
   - `UserRole` como 'Admin' | 'Agent' | 'User'
   - `NotificationType` discriminated union

3. **Utility types:**
   - `CreateTicketDto` = Omit<Ticket, 'id' | 'createdAt' | 'updatedAt'>
   - `UpdateTicketDto` = Partial<CreateTicketDto>
   - `TicketFilters` con todos los campos de filtro opcionales

4. **Mapped types:**
   - `TicketFormValue` que mapea campos del formulario
   - `UserPermissions` basado en rol del usuario
   - `ComponentState` genérico para estado de componentes

5. **Conditional types:**
   - `IsAuthenticated<T>` que cambia tipo basado en auth state
   - `CanEdit<T>` que verifica permisos por tipo de entidad

6. **Template literal types:**
   - Rutas de API como template literals
   - Clases CSS dinámicas
   - Event names tipados

Mejora las interfaces existentes en `models/` y crea nuevos archivos de tipos. Usa advanced TypeScript features como const assertions, satisfies operator, y exact types. Incluye JSDoc comments detallados."

---

## ⚡ Optimización del Rendimiento y Carga de Módulos

"Optimiza el performance de la aplicación TicketManagementSystem implementando técnicas avanzadas de Angular. Necesito:

1. **Lazy loading inteligente:**
   - Configura preloading strategies personalizadas
   - Implementa lazy loading basado en roles de usuario
   - Pre-carga módulos críticos en background

2. **Bundle splitting avanzado:**
   - Separa bundles por features (auth, tickets, users)
   - Lazy loading de librerías pesadas (moment.js, chart.js)
   - Dynamic imports para componentes opcionales

3. **Change detection optimization:**
   - Implementa OnPush en todos los componentes
   - Usa detach/reattach estratégicamente
   - Memoización con computed signals

4. **Caching y estado:**
   - Implementa caching HTTP inteligente
   - Estado persistente con IndexedDB
   - Service worker para offline capability

5. **Virtual scrolling:**
   - Para listas largas de tickets
   - Implementa cdk-virtual-scroll
   - Infinite scroll con intersection observer

6. **Tree shaking y dead code elimination:**
   - Configura build optimizer
   - Elimina código no usado
   - Lazy loading de locales i18n

7. **Monitoring y analytics:**
   - Performance monitoring con Angular DevTools
   - Core Web Vitals tracking
   - Error tracking con Sentry

Genera configuración de build, servicios de optimización, y componentes optimizados. Incluye métricas para medir mejoras de performance."