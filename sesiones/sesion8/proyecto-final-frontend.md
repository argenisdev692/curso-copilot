# 🎯 Proyecto Final Frontend: Sistema de Gestión de Reservas de Salas de Reuniones

## 📋 Descripción del Proyecto
Una SPA Angular completa para gestión de reservas de salas de reuniones que incluye:

- Autenticación JWT con guards e interceptors
- CRUD de salas y reservas con calendario interactivo
- Dashboard con estadísticas y gráficos
- Formularios reactivos con validación avanzada
- Manejo de estado con signals/services
- Testing completo (unitario y e2e)
- Responsive design con Angular Material + Tailwind CSS (utilities)
- Lazy loading y optimización de rendimiento

## 🎯 Aplicación de Sub-temas por Sesión

> **Formatos de Prompt utilizados:**
> - **C.R.E.A.T.E**: Estructurado para tareas complejas
> - **C.O.R.E**: Natural/compacto para Copilot Chat

---

## Sesión 1: Introducción y Prompt Engineering

### Tema 1 - Scaffolding del Proyecto
- Tarea: Crear proyecto Angular CLI con standalone components y SCSS, estructura modular (core, shared, features, layouts), instalar Angular Material (componentes) + Tailwind CSS (utilities), Jest para unit testing, Cypress para e2e, configurar environments y proxy, ESLint + Prettier.

### Tema 2 - Modelos y DTOs
- Tarea: Crear interfaces TypeScript para DTOs del backend: User, Room, Booking, AuthResponse, PaginatedResponse genérico. Enums string, barrel exports.

### Tema 3 - Servicio HTTP Base
- Tarea: Implementar BaseApiService genérico con HttpClient, métodos CRUD, retry exponencial, catchError centralizado, takeUntilDestroyed.

---

## Sesión 2: Desarrollo e Integración

### Tema 4 - Servicios de Dominio
- Tarea: Crear RoomService y BookingService extendiendo BaseApiService, con métodos específicos del dominio, cache simple para rooms, signals para estado reactivo.

### Tema 5 - Autenticación JWT
- Tarea: Implementar AuthService con signals, JwtInterceptor para Bearer token y refresh automático, AuthGuard y RoleGuard, manejo de tokens en localStorage.

### Tema 6 - Componentes de Autenticación
- Tarea: Crear LoginComponent y RegisterComponent standalone con Reactive Forms, validación, PasswordInputComponent shared, accessibility.

---

## Sesión 3: Testing y Refactorización

### Tema 7 - Testing Unitario
- Tarea: Crear tests unitarios para AuthService y LoginComponent con Jasmine, HttpClientTestingModule, mocks, fakeAsync, coverage mayor a 80%.

### Tema 8 - Componentes de Salas
- Tarea: Implementar RoomListComponent con filtros y paginación, RoomFormComponent para create/edit, RoomCardComponent, ConfirmDialogComponent shared.

---

## Sesión 4: Documentación y Seguridad

### Tema 9 - Componentes de Reservas
- Tarea: Crear BookingCalendarComponent con FullCalendar, BookingFormComponent con validación de disponibilidad, MyBookingsComponent, TimeRangePickerComponent shared.

### Tema 10 - Dashboard y Estadísticas
- Tarea: Implementar DashboardComponent con widgets y gráficos (ngx-charts), StatCardComponent reutilizable, StatsService, auto-refresh.

---

## Sesión 5: CI/CD y Casos Avanzados

### Tema 11 - Layout y Navegación
- Tarea: Crear MainLayoutComponent con Angular Material sidenav responsive, menú dinámico por rol, HeaderComponent, BreadcrumbComponent, AuthLayoutComponent.

### Tema 12 - Notificaciones en Tiempo Real
- Tarea: Implementar NotificationService con SignalR client, NotificationBellComponent con badge y dropdown, toast para nuevas notificaciones, browser notifications.

### Tema 12b - PWA y Offline
- Tarea: Configurar Angular PWA con Service Worker, ngsw-config para cache de API, OfflineService con sync queue, manifest e install prompt.

---

## Sesión 6: VBA y Proyecto Final

### Tema 13 - Reportes y Exportación
- Tarea: Crear ReportsComponent con filtros, ReportTableComponent sortable, exportación Excel (backend) y PDF (jsPDF frontend), gráfico resumen.

### Tema 14 - Proyecto Final
- Tarea: Integrar todos los componentes del proyecto en una SPA completa y funcional conectada al backend .NET.
