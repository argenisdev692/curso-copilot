
## 🏛️ Aplicación de Patrones de Diseño y Estructura de Carpetas Sugeridos por Copilot
## Concepto e Importancia de la Aplicación de Patrones de Diseño y Estructura de Carpetas

La aplicación de patrones de diseño y una estructura de carpetas bien organizada es fundamental para el desarrollo de software escalable y mantenible. Los patrones de diseño proporcionan soluciones probadas a problemas comunes, mejorando la reutilización de código, la separación de responsabilidades y la facilidad de mantenimiento. Una estructura de carpetas enterprise facilita la navegación, el desarrollo en equipo y la evolución del proyecto a largo plazo.

## Principios SOLID

Los principios SOLID son un conjunto de cinco principios fundamentales de diseño orientado a objetos introducidos por Robert C. Martin. Estos principios promueven la creación de software más mantenible, flexible y escalable al fomentar buenas prácticas de separación de responsabilidades, extensibilidad y abstracción.

## 💡 Ejemplo Prompt:

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

7. **Principios SOLID:**
   - Aplica los cinco principios SOLID en toda la arquitectura
   - **SRP (Single Responsibility Principle)**: Cada clase, servicio o componente debe tener una sola responsabilidad y razón para cambiar
   - **OCP (Open-Closed Principle)**: Diseña entidades abiertas para extensión pero cerradas para modificación
   - **LSP (Liskov Substitution Principle)**: Los subtipos deben ser sustituibles por sus tipos base sin alterar el comportamiento
   - **ISP (Interface Segregation Principle)**: Crea interfaces específicas y pequeñas en lugar de interfaces generales grandes
   - **DIP (Dependency Inversion Principle)**: Los módulos de alto nivel no deben depender de módulos de bajo nivel; ambos deben depender de abstracciones

Reestructura el proyecto completo manteniendo funcionalidad existente. Genera archivos índice (barrel exports) para cada módulo. Documenta la arquitectura resultante."

---
