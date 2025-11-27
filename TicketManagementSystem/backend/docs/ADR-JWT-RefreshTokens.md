# ADR: Implementación de Autenticación con JWT y Refresh Tokens

## Estado
Aceptado

## Fecha
25 de noviembre de 2025

## Contexto
En el sistema de gestión de tickets (TicketManagementSystem), se requiere implementar un mecanismo de autenticación seguro, escalable y eficiente para proteger las APIs REST. La autenticación debe manejar sesiones de usuario de manera stateless, permitiendo la escalabilidad horizontal sin depender de almacenamiento de sesiones en el servidor. Además, debe considerar la seguridad contra ataques comunes como robo de tokens y la necesidad de revocación de accesos.

## Opciones Consideradas
1. **Sesiones basadas en servidor (cookies)**: Almacenar sesiones en memoria o base de datos. Fácil revocación, pero no escalable y requiere afinidad de sesiones.
2. **JWT solo**: Tokens stateless con expiración corta. Simple, pero revocación compleja y riesgo de tokens largos.
3. **JWT + Refresh Tokens**: Combinación de access tokens cortos con refresh tokens para renovación. Equilibra stateless con capacidad de revocación controlada.
4. **OAuth 2.0 con servidor de autorización**: Completo, pero aumenta complejidad y dependencias.

## Decisión
Implementar autenticación basada en JWT (JSON Web Tokens) combinado con Refresh Tokens. Los access tokens tendrán una expiración corta (ej. 15 minutos) para minimizar riesgos, mientras que los refresh tokens permitirán renovar el acceso sin re-autenticación completa.

## Consecuencias

### Pros
- **Stateless**: Los tokens no requieren almacenamiento en el servidor, permitiendo escalabilidad horizontal sin afinidad de sesiones. Los servidores pueden validar tokens de forma independiente.
- **Escalables**: Facilita el despliegue en múltiples instancias o contenedores sin necesidad de compartir estado de sesiones.
- **Eficiencia**: Reduce carga en base de datos para validaciones de autenticación, mejorando el rendimiento general del sistema.
- **Compatibilidad**: Estándar ampliamente soportado en ecosistemas .NET y frontend (Angular), facilitando la integración.

### Contras
- **Revocación compleja**: Dado que los tokens son stateless, revocar un token específico requiere implementar listas de revocación (blacklist) o expiración forzada, lo que introduce complejidad adicional y potencial impacto en performance.
- **Riesgo de robo**: Si un access token es comprometido, puede ser usado hasta su expiración. Requiere HTTPS obligatorio y manejo cuidadoso de tokens en cliente.
- **Gestión de refresh tokens**: Necesidad de almacenar refresh tokens de forma segura (ej. en base de datos con hashing), lo que introduce estado parcial y requiere limpieza periódica de tokens expirados.
- **Complejidad de implementación**: Requiere configuración de middleware para validación, renovación y revocación, aumentando el código boilerplate.

### Mitigaciones
- Implementar lista de revocación en Redis/cache para tokens comprometidos.
- Usar claims adicionales en JWT para control de acceso granular (RBAC).
- Configurar expiraciones apropiadas y rotación automática de refresh tokens.
- Integrar logging estructurado en middleware para auditoría de autenticación.

## Implementación Técnica
- Usar `Microsoft.AspNetCore.Authentication.JwtBearer` para validación.
- Middleware personalizado para renovación de tokens.
- Almacenamiento de refresh tokens en base de datos con `IAuditable` para tracking.
- Validación con FluentValidation para requests de autenticación.

## Referencias
- RFC 7519: JSON Web Token (JWT)
- OWASP Authentication Cheat Sheet
- Microsoft Identity Platform documentation