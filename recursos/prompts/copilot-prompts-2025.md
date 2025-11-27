# Prompts de GitHub Copilot para C# (.NET) y Angular - 2025

Este documento contiene 140 prompts optimizados para C# (.NET), 140 para Angular, y 30 para VBA, organizados por categorías basadas en el curso de GitHub Copilot para desarrolladores web. Los prompts están en español y están diseñados para ser utilizados con GitHub Copilot para mejorar la productividad en el desarrollo fullstack.

## Prompts para C# (.NET)

### 1. Autocompletado Avanzado en C#

1. "Crea un método público y estático en C# llamado CalculateFactorial que tome un parámetro entero (int) y devuelva el factorial usando recursión. Incluye validación para números negativos lanzando ArgumentException, maneja el caso base para 0 y 1 devolviendo 1, y usa tipo long para el retorno para manejar números grandes sin overflow."
2. "Crea una clase pública en C# llamada Point con dos propiedades automáticas públicas: X de tipo double e Y de tipo double. Incluye un constructor que inicialice ambas propiedades, métodos para calcular la distancia al origen (double DistanceToOrigin()) y la distancia a otro punto (double DistanceTo(Point other)), y un método ToString() sobrescrito."
3. "Implementa un método público y estático en C# llamado ReverseString que tome una cadena (string) como parámetro y devuelva la cadena invertida sin usar métodos incorporados como Reverse() o StringBuilder. Usa un bucle for para construir la cadena invertida carácter por carácter, manejando cadenas nulas."
4. "Crea un método público y estático en C# llamado SumEvenNumbers que tome una lista de enteros (List<int>) como parámetro, itere sobre ella usando foreach, y sume únicamente los números pares. Devuelve la suma como int. Incluye validación para lista nula lanzando ArgumentNullException."
5. "Crea una estructura (struct) en C# llamada Employee con propiedades automáticas: Name (string), Age (int), Salary (decimal). Incluye un constructor para inicializar todas las propiedades con validación (edad > 0, salario >= 0), y un método ToString() sobrescrito para mostrar la información formateada como 'Name: {Name}, Age: {Age}, Salary: {Salary:C}'."
6. "Implementa un método público y estático en C# llamado IsPalindrome que tome una cadena (string) y devuelva bool indicando si es un palíndromo. Ignora mayúsculas/minúsculas y espacios. Usa un bucle para comparar caracteres desde ambos extremos, manejando cadenas nulas o vacías."
7. "Crea un método público y estático en C# llamado PerformOperation que tome dos doubles (num1, num2) y un string (operation) para la operación (+, -, *, /). Usa switch statement para manejar cada caso, incluyendo validación para división por cero lanzando DivideByZeroException. Devuelve double."
8. "Crea una clase Product en C# con propiedades automáticas: Id (int), Name (string), Price (decimal), Stock (int). Incluye getters y setters públicos con validación en setters (Price >= 0, Stock >= 0), un constructor para inicializar todas las propiedades, y un método ToString() para mostrar detalles del producto."
9. "Implementa un método público y estático en C# llamado SortStringsAlphabetically que tome una lista de strings (List<string>) y la ordene alfabéticamente sin usar Sort(). Usa un algoritmo de ordenamiento simple como bubble sort, ignorando mayúsculas para comparación case-insensitive, y maneja listas nulas."
10. "Crea una clase Vehicle en C# con propiedades automáticas: Make (string), Model (string), Year (int), Color (string). Incluye un constructor que tome parámetros para inicializar todas las propiedades con validación (Year entre 1900 y DateTime.Now.Year), otro constructor sin parámetros que use valores por defecto, y un método GetVehicleInfo() que devuelva una cadena con la información formateada."

### 2. Generación de Snippets y Patrones de Código Repetitivos

11. "Implementa el patrón Singleton en C# para una clase DatabaseConfiguration que maneje la configuración de conexión a base de datos. Incluye una propiedad Instance estática y thread-safe usando Lazy<T>, constructor privado, y métodos para obtener y actualizar la configuración con validación de parámetros."
12. "Crea un patrón Factory Method en C# con una interfaz IVehicleFactory y clases concretas CarFactory y TruckFactory. Incluye una clase abstracta Vehicle con propiedades comunes (Make, Model, Year) y métodos virtuales para cada tipo de vehículo, con implementaciones específicas en subclases."
13. "Implementa el patrón Observer en C# con una interfaz IObserver con método Update(), una clase Subject que mantenga una lista de observers, y métodos Attach(), Detach() y Notify(). Crea un ejemplo con un objeto WeatherStation que notifique temperatura a displays usando eventos."
14. "Genera el patrón Strategy en C# con una interfaz ISortingStrategy con método Sort(List<int>), clases concretas BubbleSortStrategy y QuickSortStrategy. Incluye un contexto SortingContext que inyecte la estrategia vía constructor y un método para cambiar estrategia en runtime."
15. "Crea el patrón Decorator en C# con una interfaz IComponent con método Operation(), una clase base ConcreteComponent, y decoradores concretos LoggingDecorator y CachingDecorator que envuelvan el componente y agreguen funcionalidades como logging y caching sin modificar la clase base."
16. "Implementa el patrón Command en C# con una interfaz ICommand con métodos Execute() y Undo(), clases concretas SaveCommand y DeleteCommand que encapsulen operaciones, y un invoker CommandManager con pila para ejecutar y deshacer comandos en orden."
17. "Genera el patrón Adapter en C# con una interfaz ITarget con método Request(), una clase Adaptee con método SpecificRequest(), y una clase Adapter que herede de Target e implemente Request() llamando a SpecificRequest() del Adaptee para compatibilidad."
18. "Crea el patrón Template Method en C# con una clase abstracta DataProcessor con método template ProcessData() que llame a métodos abstractos LoadData(), ProcessDataCore() y SaveData(). Implementa subclases CsvDataProcessor y XmlDataProcessor con lógica específica."
19. "Implementa el patrón State en C# con una interfaz IState con métodos Handle() y ChangeState(), estados concretos DraftState, ReviewState y PublishedState, y una clase Document que delegue comportamiento al estado actual y permita transiciones."
20. "Genera el patrón Builder en C# con una interfaz IProductBuilder con métodos fluentes para propiedades opcionales, una clase Product con constructor privado, y una clase Director que use el builder para construir productos complejos con validaciones en cada paso."

### 3. Refactorización Asistida por IA en Clases y Métodos

21. "Refactoriza un método largo en C# llamado ProcessLargeDataSet que contenga más de 50 líneas mezclando validación, procesamiento y logging. Divídelo en métodos privados más pequeños como ValidateInputData(), ProcessRecordsInBatch(), GenerateProcessingReport(), cada uno con una sola responsabilidad, parámetros claros y nombres descriptivos siguiendo SOLID principles."
22. "Extrae una interfaz en C# llamada IService de una clase concreta BusinessService que implemente métodos como Execute(), Validate(), Log(). La interfaz debe definir contratos limpios para inyección de dependencias, testing unitario y mocking, permitiendo múltiples implementaciones."
23. "Renombra variables en C# en un método CalculateInterest que use abreviaturas como 'p', 'r', 't', 'amt' a nombres descriptivos como 'principalAmount', 'interestRate', 'timePeriod', 'totalAmount', siguiendo las convenciones camelCase para locales y PascalCase para parámetros."
24. "Convierte un método estático en C# llamado GenerateReport que dependa de configuración global y servicios externos a un método de instancia en una clase ReportGenerator que reciba dependencias vía constructor usando inyección de dependencias, permitiendo testing y flexibilidad."
25. "Elimina código duplicado en C# identificando métodos similares ValidateCustomer() y ValidateEmployee() que compartan validaciones de email y edad. Crea un método base ValidatePerson() con parámetros genéricos y extrae lógica común a métodos privados reutilizables."
26. "Simplifica expresiones condicionales complejas en C# en un método con múltiples if-else anidados para determinar tipo de usuario. Convierte a operadores ternarios cuando apropiado, extrae condiciones a variables booleanas como 'isPremiumUser', 'hasValidSubscription' y considera switch expressions para C# 8+."
27. "Extrae constantes en C# de un método que use números mágicos como 86400 (segundos en día), 1000 (máximo items), 'production' (ambiente) a constantes públicas o privadas con nombres descriptivos como SECONDS_PER_DAY, MAX_ITEMS_PER_PAGE, PRODUCTION_ENVIRONMENT para mejorar mantenibilidad y evitar bugs."
28. "Convierte bucles tradicionales en C# como foreach con listas acumuladoras a expresiones LINQ más concisas usando métodos como Where() para filtrado, Select() para transformación, Sum() para agregación, GroupBy() para agrupamiento, mejorando legibilidad y performance en colecciones grandes."
29. "Separa responsabilidades en C# en una clase monolítica UserManager que maneje autenticación, perfil, emails y logging. Crea clases separadas AuthenticationService, ProfileService, EmailService, Logger con interfaces respectivas, aplicando principio de responsabilidad única y facilitando testing."
30. "Optimiza el rendimiento en C# en un bucle foreach que realice llamadas a base de datos, concatenaciones de strings y cálculos innecesarios. Mueve operaciones fuera del bucle, usa StringBuilder para strings, considera parallel processing con PLINQ, y implementa caching para datos frecuentemente accedidos."

### 4. Generación de Controladores y Servicios en ASP.NET

31. "Genera un controlador ASP.NET Core API llamado ProductsController con rutas RESTful para operaciones CRUD completas (GET /api/products, POST /api/products, PUT /api/products/{id}, DELETE /api/products/{id}). Incluye inyección de IProductService, validación con ModelState, manejo de excepciones con filtros, y respuestas IActionResult apropiadas con códigos HTTP correctos."
32. "Crea un servicio en ASP.NET Core llamado JwtAuthenticationService que implemente IAuthenticationService con métodos AuthenticateUser(), GenerateToken(), ValidateToken() usando Microsoft.IdentityModel.Tokens. Incluye configuración de issuer, audience, expiración, y claims para roles y permisos en el token."
33. "Implementa un controlador API en ASP.NET Core llamado PaginatedDataController que acepte parámetros query [FromQuery] int pageNumber, int pageSize, string sortBy. Devuelva PagedResponse<T> con metadata (totalItems, totalPages, hasNext), validación de límites, y soporte para ordenamiento ascendente/descendente."
34. "Genera un servicio de logging en ASP.NET Core usando Serilog llamado StructuredLoggingService con configuración programática para sinks (File, Console, Seq). Incluya métodos LogInformation(), LogError() con structured logging usando LogContext, enrichers para propiedades como UserId, RequestId, y configuración desde appsettings.json."
35. "Crea un controlador en ASP.NET Core llamado FileUploadController con endpoints [HttpPost] UploadFile(IFormFile file) para subida múltiple y [HttpGet] DownloadFile(string fileId) para descarga. Incluya validación de Content-Type, límites de tamaño con configuración, almacenamiento seguro en wwwroot/uploads, y headers apropiados para descarga."
36. "Implementa un servicio de caché distribuido en ASP.NET Core llamado RedisCacheService implementando IDistributedCache. Incluya métodos GetAsync<T>(), SetAsync<T>() con serialización JSON, configuración de connection string, manejo de TimeSpan para expiración, y soporte para tags o keys pattern para invalidación masiva."
37. "Genera un controlador de autenticación en ASP.NET Core llamado ExternalAuthController con endpoints Challenge(string provider) para iniciar OAuth flow y Callback() para manejar respuesta. Configure AuthenticationBuilder con AddGoogle(), AddFacebook(), incluya creación de ApplicationUser local, y generación de JWT tras autenticación exitosa."
38. "Crea un servicio de notificaciones por email en ASP.NET Core llamado SmtpEmailService usando MailKit o FluentEmail. Incluya métodos SendWelcomeEmail(), SendPasswordReset() con templates Razor, configuración SMTP desde appsettings, envío asíncrono con BackgroundService, y logging de envíos exitosos/fallidos."
39. "Implementa un controlador de reportes en ASP.NET Core llamado PdfReportsController con [HttpGet] GenerateReport([FromQuery] ReportParameters params). Use DinkToPdf o iTextSharp para generar PDF desde HTML templates, incluya filtros por fecha/rango, validación de permisos con [Authorize], y retorno como FileResult con content-disposition attachment."
40. "Genera un servicio de validación personalizado en ASP.NET Core llamado BusinessValidationService implementando IValidator<T> con FluentValidation. Incluya reglas complejas como validación cruzada de campos, dependencias de base de datos, mensajes de error localizados con IStringLocalizer, y integración con ModelState en controladores."

### 5. Uso en Proyectos de Microservicios con .NET

41. "Genera un microservicio en .NET 6+ llamado InventoryMicroservice con API REST minimal. Incluya endpoints para CRUD de productos con Swagger documentation, Entity Framework Core in-memory para desarrollo, migraciones para SQL Server, validación con DataAnnotations, y configuración de CORS para comunicación con frontend."
42. "Crea comunicación eficiente entre microservicios en .NET usando gRPC con proto files. Defina servicio OrderService con métodos PlaceOrder(), CheckInventory() que llame a InventoryService. Incluya configuración de Kestrel para HTTP/2, cliente con gRPC channels, y interceptores para logging y autenticación."
43. "Implementa un API Gateway en .NET usando YARP (Yet Another Reverse Proxy) como alternativa a Ocelot. Configure rutas para downstream services con load balancing, transformación de requests/responses, agregación de respuestas de múltiples servicios, y middleware para autenticación JWT global."
44. "Genera un servicio de configuración centralizada en .NET usando Azure App Configuration o AWS Systems Manager. Implemente cliente que consuma configuración remota con refresh automático via IConfigurationRefresher, soporte para feature flags, secrets management, y configuración por ambientes con key-vault integration."
45. "Crea un microservicio de autenticación en .NET usando Duende IdentityServer como reemplazo de IdentityServer4. Configure grants para authorization code flow, clients para SPA y mobile apps, persistencia en PostgreSQL con Entity Framework, y endpoints para introspection y revocation de tokens."
46. "Implementa circuit breaker pattern en .NET usando Polly con HttpClientFactory. Configure políticas con Bulkhead isolation, Timeout, Retry, CircuitBreaker (con métricas), y Fallback. Integre con dependency injection como HttpMessageHandler, y monitoreo con Application Insights."
47. "Genera un servicio de logging distribuido en .NET con ELK stack usando Serilog para structured logging, Filebeat para shipping logs, Elasticsearch para indexing, Logstash para enrichment, Kibana para dashboards. Configure correlation IDs con ActivitySource, envío via GELF, y alerting con Watcher."
48. "Crea un microservicio de notificaciones en .NET usando NServiceBus con RabbitMQ como transport. Defina mensajes para EmailNotification, SmsNotification con sagas para reliability, configure pub/sub patterns, outbox pattern para consistency, y monitoring con ServiceControl."
49. "Implementa health checks comprehensivos en .NET Core con AspNetCore.Diagnostics.HealthChecks. Agregue checks para SQL Server, Redis, external APIs, system resources (CPU, memory). Configure /health endpoint con UI, métricas Prometheus, y alertas basadas en health status."
50. "Genera un servicio de service discovery en .NET usando Consul con Steeltoe Discovery. Configure registro automático de servicios con health checks, cliente que resuelva servicios por nombre, load balancing con Spring Cloud LoadBalancer, y integración con Docker containers para desarrollo local."

### 6. Copilot para Consultas LINQ

51. "Genera una consulta LINQ en C# usando method syntax para filtrar una colección List<Product> donde Price > 100. Incluye Where() para filtrado, Select() para proyección a anónimos con Name y Price, OrderBy() ascendente por precio, y ToList() para materialización."
52. "Crea una consulta LINQ que agrupe empleados por Department usando GroupBy(e => e.Department), calcule AverageSalary con Select(g => g.Average(e => e.Salary)), y proyecte a anónimos con DepartmentName y AverageSalary formateado a 2 decimales."
53. "Implementa una consulta LINQ para join entre orders y customers usando Join() por CustomerId, proyectando OrderId, OrderDate, CustomerName. Maneje casos donde customer sea null con DefaultIfEmpty() para left join, y use query syntax para legibilidad."
54. "Genera una consulta LINQ que ordene List<DateTime> en descendente con OrderByDescending(d => d), tome primeros 10 con Take(10), convierta a strings con Select(d => d.ToString('yyyy-MM-dd')), y retorne como array con ToArray()."
55. "Crea una consulta LINQ para seleccionar propiedades específicas de List<User> usando Select(u => new { u.Id, FullName = u.FirstName + ' ' + u.LastName, u.Email }), ignorando propiedades como Password y CreatedDate para DTOs ligeros."
56. "Implementa una consulta LINQ con proyección anónima para transformar List<Product> a objetos con DiscountedPrice = p.Price * 0.9, IsExpensive = p.Price > 500, CategoryUpper = p.Category.ToUpper(), usando Select() con expresiones complejas."
57. "Genera una consulta LINQ que cuente productos con Stock < 10 usando Count(p => p.Stock < 10), también calcule Sum(p => p.Stock) para productos filtrados, y retorne tupla (LowStockCount, TotalLowStock) con named tuples de C# 7+."
58. "Crea una consulta LINQ para encontrar producto más caro usando MaxBy(p => p.Price) de .NET 6+, o Max() con selector. Incluya también FirstOrDefault con OrderByDescending para el más caro, manejando colecciones vacías."
59. "Implementa una consulta LINQ que pagine eficientemente List<T> usando Skip((pageNumber - 1) * pageSize).Take(pageSize), con OrderBy() previo para consistencia. Incluya totalCount = Count() para metadata, y retorne PagedResult<T>."
60. "Genera una consulta LINQ para detectar duplicados en List<string> emails usando GroupBy(email => email).Where(g => g.Count() > 1).Select(g => g.Key), proyectando emails duplicados. Use también Distinct() para obtener únicos, y ToHashSet() para performance."

### 7. Mejora de la Legibilidad en Código Backend

61. "Mejora la legibilidad de este método en C# agregando comentarios descriptivos"
62. "Refactoriza nombres de variables en C# para que sean más expresivos"
63. "Simplifica expresiones booleanas complejas en C# usando variables intermedias"
64. "Agrega validaciones de parámetros en C# con mensajes de error claros"
65. "Convierte números mágicos en C# a constantes con nombres descriptivos"
66. "Mejora el flujo de control en C# usando early returns"
67. "Agrega manejo de excepciones específico en C# en lugar de catch genérico"
68. "Simplifica anidación excesiva en C# extrayendo métodos"
69. "Agrega tipos explícitos en C# para mejorar la autocompletación"
70. "Mejora la documentación XML en C# con ejemplos de uso"

### 8. Implementación de Patrones de Diseño con IA

71. "Implementa el patrón Repository en C# para acceso a datos"
72. "Crea el patrón Unit of Work en C# para transacciones de base de datos"
73. "Genera el patrón CQRS en C# separando comandos de queries"
74. "Implementa el patrón Mediator en C# para comunicación entre objetos"
75. "Crea el patrón Specification en C# para consultas complejas"
76. "Genera el patrón Visitor en C# para operaciones en estructuras de objetos"
77. "Implementa el patrón Chain of Responsibility en C# para manejo de solicitudes"
78. "Crea el patrón Interpreter en C# para lenguajes específicos de dominio"
79. "Genera el patrón Memento en C# para guardar y restaurar estados"
80. "Implementa el patrón Flyweight en C# para optimizar uso de memoria"

### 9. Generación de Documentación XML de Métodos

81. "Genera documentación XML completa para este método en C# incluyendo parámetros y retorno"
82. "Crea documentación XML para una clase en C# con descripción y ejemplos"
83. "Implementa documentación XML para propiedades en C# con validaciones"
84. "Genera documentación XML para constructores en C# con parámetros opcionales"
85. "Crea documentación XML para eventos en C# explicando cuándo se disparan"
86. "Implementa documentación XML para métodos asíncronos en C#"
87. "Genera documentación XML para métodos genéricos en C#"
88. "Crea documentación XML para operadores sobrecargados en C#"
89. "Implementa documentación XML para métodos de extensión en C#"
90. "Genera documentación XML para interfaces en C# con contratos"

### 10. Casos Prácticos en Aplicaciones Empresariales .NET

91. "Genera una API REST en ASP.NET Core para gestión de empleados con roles"
92. "Crea un sistema de logging empresarial en .NET con múltiples sinks"
93. "Implementa autenticación multi-factor en ASP.NET Identity"
94. "Genera un servicio de mensajería en .NET usando SignalR para tiempo real"
95. "Crea un pipeline de CI/CD en .NET con Azure DevOps"
96. "Implementa un sistema de caché distribuido en .NET con Redis"
97. "Genera un servicio de notificaciones push en .NET para aplicaciones móviles"
98. "Crea un dashboard administrativo en ASP.NET MVC con gráficos"
99. "Implementa integración con servicios externos en .NET usando WebHooks"
100. "Genera un sistema de reportes en .NET exportando a múltiples formatos"

### 11. Integración con Bases de Datos en .NET

101. "Genera un contexto de Entity Framework Core en C# para una base de datos de productos con entidades Product, Category, Supplier. Incluya configuraciones de relaciones uno-a-muchos, muchos-a-muchos, claves foráneas, índices y constraints. Use fluent API para configuraciones complejas y data annotations para validaciones simples."
102. "Crea un repositorio genérico en C# implementando IRepository<T> con métodos CRUD asíncronos (GetByIdAsync, GetAllAsync, AddAsync, UpdateAsync, DeleteAsync). Incluya soporte para especificaciones (ISpecification<T>) para consultas complejas, paginación y ordenamiento, usando Entity Framework Core como ORM."
103. "Implementa el patrón Unit of Work en C# con una interfaz IUnitOfWork que agrupe múltiples repositorios (IProductRepository, IOrderRepository) y maneje transacciones con SaveChangesAsync(). Incluya inyección de dependencias en Program.cs de ASP.NET Core y manejo de transacciones anidadas."
104. "Genera migraciones de Entity Framework Core en C# para crear tablas con relaciones. Incluya Up() y Down() métodos con CreateTable(), AddColumn(), AddForeignKey(), CreateIndex(). Maneje datos iniciales con InsertData() para roles y categorías por defecto."
105. "Crea un servicio de base de datos en C# usando Dapper para operaciones de alto rendimiento. Incluya métodos para ejecutar stored procedures, queries parametrizadas, bulk inserts con SqlBulkCopy, y mapeo manual de resultados a objetos POCO con DynamicParameters."
106. "Implementa consultas LINQ to Entities en C# optimizadas con Include() para eager loading, ThenInclude() para navegación profunda, AsNoTracking() para consultas de solo lectura, y Select() para proyección. Incluya joins explícitos, group by con agregaciones, y subqueries."
107. "Genera un modelo de dominio en C# con entidades ricas usando Domain-Driven Design. Incluya agregados (ProductAggregate con Product, ProductVariant), value objects (Money, Address), domain events (ProductCreatedEvent), y validaciones de negocio con métodos de entidad."
108. "Crea un servicio de caché de segundo nivel en C# usando Redis con IDistributedCache. Incluya métodos para cachear queries complejas, invalidación automática con tags, serialización JSON con System.Text.Json, y configuración de expiración sliding/absolute."
109. "Implementa un patrón CQRS en C# con MediatR para separar comandos y queries. Incluya command handlers (CreateProductCommandHandler), query handlers (GetProductsQueryHandler), validators con FluentValidation, y pipeline behaviors para logging y validación global."
110. "Genera un contexto de base de datos en C# con soporte para múltiples proveedores (SQL Server, PostgreSQL, SQLite). Incluya configuración condicional en appsettings.json, factory pattern para DbContext, y migraciones específicas por proveedor con --context y --provider flags."
### 12. Seguridad y Autenticación en .NET

111. "Implementa autenticación JWT en ASP.NET Core con un servicio ITokenService que genere tokens usando SymmetricSecurityKey, claims para roles y permisos, expiración configurable, y validación en middleware. Incluya refresh tokens con almacenamiento seguro en base de datos."
112. "Crea un sistema de autorización basado en roles en C# usando ASP.NET Identity con UserManager<ApplicationUser> y RoleManager<IdentityRole>. Incluya políticas de autorización personalizadas con IAuthorizationRequirement, handlers para permisos granulares, y [Authorize] attributes en controladores."
113. "Genera un servicio de hashing de contraseñas en C# usando BCrypt o Argon2 con PasswordHasher<TUser>. Incluya métodos para hash, verify, y configuración de work factor. Implemente validaciones de fortaleza de contraseña con regex y políticas de expiración."
114. "Implementa autenticación de dos factores (2FA) en ASP.NET Core usando Identity con TOTP (Time-based One-Time Password). Incluya generación de códigos QR con QrCode.Net, validación de códigos, backup codes, y recuperación de cuenta segura."
115. "Crea un middleware de autenticación personalizado en C# que valide tokens JWT de múltiples issuers (Google, Facebook, local). Incluya caching de claves públicas, validación de audience y issuer, rate limiting por IP, y logging de intentos fallidos."
116. "Genera un sistema de OAuth 2.0 en ASP.NET Core usando Duende IdentityServer como authorization server. Configure clients para SPA y mobile apps, grants implícito y code flow, scopes personalizados, y persistencia en SQL Server con Entity Framework."
117. "Implementa encriptación de datos sensibles en C# usando AES-256 con claves derivadas de PBKDF2. Incluya métodos para encriptar/desencriptar strings y streams, gestión segura de claves con Azure Key Vault, y rotación automática de claves."
118. "Crea un servicio de auditoría de seguridad en C# que registre eventos críticos (login/logout, cambios de permisos, accesos a datos sensibles) usando Serilog con structured logging. Incluya correlation IDs, masking de datos sensibles, y exportación a SIEM systems."
119. "Genera validaciones de entrada seguras en C# usando DataAnnotations y FluentValidation con sanitización de XSS. Incluya validaciones de longitud, formato, y listas blancas para inputs, protección contra SQL injection con parameterized queries, y rate limiting."
120. "Implementa un sistema de gestión de sesiones seguro en ASP.NET Core con protección contra fixation attacks, idle timeout, concurrent session control, y almacenamiento distribuido en Redis. Incluya sliding expiration y invalidación automática al logout."
### 13. Optimización de Rendimiento y Escalabilidad en .NET

121. "Implementa caching multinivel en ASP.NET Core con IMemoryCache para datos locales, IDistributedCache con Redis para distribuido, y output caching con [ResponseCache]. Incluya estrategias de invalidación, compresión gzip, y monitoreo de hit rates con Application Insights."
122. "Crea un servicio de pool de conexiones de base de datos en C# optimizado con SqlConnection pooling, timeouts configurables, y health checks. Incluya retry logic con Polly, circuit breaker para protección, y métricas de performance con Prometheus."
123. "Genera procesamiento asíncrono en C# usando BackgroundService para tareas de larga duración, Channel<T> para producer-consumer patterns, y IHostedService para limpieza automática. Incluya cancellation tokens, exception handling, y logging estructurado."
124. "Implementa lazy loading y virtualización en C# para colecciones grandes usando IEnumerable con yield return, PLINQ para paralelización, y Memory<T> para manejo eficiente de memoria. Incluya benchmarking con BenchmarkDotNet y profiling con dotTrace."
125. "Crea un sistema de mensajería asíncrona en .NET usando Azure Service Bus o RabbitMQ con MassTransit. Incluya sagas para transacciones distribuidas, dead letter queues, y monitoreo con health checks y métricas de throughput."
126. "Genera optimización de consultas LINQ en C# con compiled queries para EF Core, query interception para logging, y batching automático. Incluya análisis de execution plans, índices apropiados, y uso de AsNoTracking para read-only operations."
127. "Implementa compresión y minificación en ASP.NET Core con middleware personalizado para responses JSON/XML, Brotli compression, y caching de assets estáticos. Incluya CDN integration y headers apropiados para browser caching."
128. "Crea un sistema de rate limiting distribuido en C# usando AspNetCoreRateLimit con Redis store, políticas por endpoint/usuario/IP, y headers informativos. Incluya configuración desde appsettings y logging de violaciones."
129. "Genera profiling y diagnóstico en .NET con MiniProfiler para queries, Application Insights para telemetría, y EventSource para logging de alto rendimiento. Incluya custom metrics, alerts basados en thresholds, y dashboards en Azure Monitor."
130. "Implementa escalabilidad horizontal en .NET con Kubernetes deployments, health probes, y auto-scaling basado en CPU/memory. Incluya configuración de Docker multi-stage builds, secrets management con Key Vault, y service mesh con Istio para routing inteligente."

### 14. Integración de Copilot en CI/CD y DevOps

131. "Genera un pipeline completo de GitHub Actions para aplicación ASP.NET Core con build, tests unitarios, análisis SonarQube, y despliegue a Azure App Service. Incluya stages separados, caching de dependencias, y notificaciones."
132. "Crea un Dockerfile multi-stage optimizado para .NET 8 API con capas separadas para build y runtime, escaneo de vulnerabilidades, y configuración de health checks. Incluya COPY --from para artefactos y USER no-root."
133. "Implementa configuración de Azure DevOps YAML pipeline con templates reutilizables para CI/CD de aplicación .NET. Incluya variables de entorno, approvals para producción, y integración con Azure Key Vault."
134. "Genera workflow de GitHub Actions para despliegue a AWS ECS con Terraform para infraestructura. Incluya steps para build de imagen Docker, push a ECR, y actualización de service con zero-downtime."
135. "Crea pipeline de Jenkins con Declarative Pipeline para aplicación .NET usando Copilot. Incluya stages para checkout, restore, build, test, publish, y deploy con Blue-Green deployment strategy."
136. "Implementa integración con GitVersion en pipeline CI/CD para versionado semántico automático. Incluya configuración de branching strategy, tags, y actualización de assemblies en .NET."
137. "Genera scripts de PowerShell para automatización de despliegues en IIS con Copilot. Incluya backup de aplicación actual, stop/start de servicios, y rollback automático en caso de falla."
138. "Crea configuración de Terraform para infraestructura completa de aplicación .NET en Azure. Incluya App Service, SQL Database, Key Vault, y networking con VNet integration."
139. "Implementa monitoreo y alerting en pipeline CI/CD con Application Insights y Azure Monitor. Incluya métricas de performance, logs estructurados, y alertas basadas en SLOs."
140. "Genera documentación automática de APIs desplegadas con OpenAPI/Swagger en CI/CD pipeline. Incluya generación de cliente SDK, actualización de documentación en GitHub Pages, y notificaciones a stakeholders."



## Prompts para Angular

### 1. Autocompletado Avanzado en Templates y Componentes Standalone

1. "Genera un componente standalone en Angular que muestre una lista de elementos usando signals y @for"
2. "Crea un template en Angular para un formulario reactivo con validación usando ReactiveFormsModule"
3. "Implementa un componente standalone en Angular que maneje estado local con signals"
4. "Genera un template que renderice una tabla de datos con encabezados usando @for"
5. "Crea un componente standalone en Angular para mostrar un modal con Angular Material"
6. "Implementa un template para una barra de navegación responsive usando Angular CDK"
7. "Genera un componente standalone que use HttpClient para cargar datos en ngOnInit"
8. "Crea un template para mostrar tarjetas de productos usando @if y @for"
9. "Implementa un componente standalone con inputs tipadas usando input() signals"
10. "Genera un template para un carrusel de imágenes usando Angular animations"

### 2. Generación de Servicios y Lógica Reutilizable con IA

11. "Genera un servicio inyectable en Angular para manejar formularios reactivos"
12. "Crea un servicio para persistir estado en localStorage usando BehaviorSubject"
13. "Implementa un operador RxJS personalizado en Angular para debounce en búsquedas"
14. "Genera un servicio HttpClient personalizado en Angular para llamadas a API"
15. "Crea un servicio de autenticación en Angular usando guards y interceptores"
16. "Implementa un servicio de paginación en Angular para listas grandes"
17. "Genera un servicio para lazy loading usando IntersectionObserver"
18. "Crea un servicio para responsive design usando matchMedia"
19. "Implementa un servicio para comparar valores previos usando signals"
20. "Genera un servicio para operaciones diferidas usando setTimeout con RxJS"

### 3. Sugerencias de Tipado en TypeScript para Componentes y Inputs

21. "Genera tipos TypeScript para inputs de un componente de lista"
22. "Crea interfaces TypeScript para un componente de formulario"
23. "Implementa tipos genéricos en TypeScript para componentes reutilizables"
24. "Genera tipos de utilidad en TypeScript para inputs opcionales"
25. "Crea tipos discriminados en TypeScript para estados de componente"
26. "Implementa tipos mapeados en TypeScript para variantes de componente"
27. "Genera tipos condicionales en TypeScript para inputs dinámicas"
28. "Crea tipos de template literal en TypeScript para clases CSS"
29. "Implementa tipos de función en TypeScript para event handlers"
30. "Genera tipos de módulo en TypeScript para imports dinámicos"

### 4. Creación Asistida de Servicios de Inyección de Dependencias en Angular

31. "Genera un servicio de autenticación en Angular con inyección de dependencias"
32. "Crea un servicio de tema en Angular para modo oscuro/claro usando signals"
33. "Implementa un servicio de carrito de compras en Angular"
34. "Genera un servicio de notificaciones en Angular con toast usando Angular Material"
35. "Crea un servicio de configuración global en Angular"
36. "Implementa un servicio de idioma en Angular para internacionalización"
37. "Genera un servicio de usuario en Angular con perfil"
38. "Crea un servicio de permisos en Angular para control de acceso"
39. "Implementa un servicio de caché en Angular para datos"
40. "Genera un servicio de navegación en Angular con historial"

### 5. Generación de Pruebas Unitarias con Jasmine y Angular Testing Utilities

41. "Genera pruebas unitarias en Jasmine para un componente de botón"
42. "Crea tests con Angular Testing Utilities para un formulario reactivo"
43. "Implementa pruebas de integración en Jasmine para servicios inyectables"
44. "Genera tests de snapshot en Jasmine para componentes UI"
45. "Crea pruebas de accesibilidad con Angular Testing Utilities"
46. "Implementa mocking en Jasmine para llamadas a HttpClient"
47. "Genera pruebas de error handling en Jasmine para componentes"
48. "Crea tests de performance con Angular Testing Utilities"
49. "Implementa pruebas de navegación con Angular Router Testing Library"
50. "Genera tests de servicios en Jasmine para providers"

### 6. Refactorización de Componentes con Decoradores a Standalone

51. "Convierte un componente con NgModule en Angular a standalone con imports"
52. "Refactoriza ngOnInit a constructor con inject() en Angular"
53. "Convierte propiedades @Input a input() signals en componentes standalone"
54. "Refactoriza métodos de ciclo de vida a hooks en Angular standalone"
55. "Convierte ngOnChanges a effect() con signals"
56. "Refactoriza @Input a inputs desestructurados en Angular"
57. "Convierte métodos de clase a funciones arrow en Angular"
58. "Refactoriza markForCheck a signals para re-renders"
59. "Convierte ViewChild a viewChild() en Angular"
60. "Refactoriza contextos a inyección de dependencias en standalone"

### 7. Implementación Guiada de Patrones como Servicios y Directivas

61. "Genera un servicio inyectable en Angular para logging"
62. "Crea un servicio en Angular para manejo de errores con interceptores"
63. "Implementa un patrón de directiva en Angular para compartir lógica"
64. "Genera un guard en Angular para autenticación de rutas"
65. "Crea un patrón de servicio para manejo de formularios"
66. "Implementa un servicio en Angular para lazy loading"
67. "Genera un patrón de directiva para animaciones"
68. "Crea un servicio en Angular para optimización de performance"
69. "Implementa un patrón de servicio para internacionalización"
70. "Genera un servicio en Angular para gestión de estado global"

### 8. Autocompletado en Integraciones con Librerías Angular

71. "Genera integración con NgRx en Angular para estado global"
72. "Crea configuración de HttpClient en Angular para llamadas a API"
73. "Implementa Angular Router para navegación con rutas protegidas"
74. "Genera integración con Akita para estado ligero"
75. "Crea configuración de Apollo Client para GraphQL en Angular"
76. "Implementa Angular Reactive Forms para validación de formularios"
77. "Genera integración con Angular Material para componentes"
78. "Crea configuración de Angular Animations para estilos"
79. "Implementa Angular Animations para animaciones"
80. "Genera integración con HttpClient para interceptores HTTP"

### 9. Generación Automática de Documentación de Inputs y Componentes

81. "Genera documentación JSDoc para inputs de componente Angular"
82. "Crea documentación TypeScript para interfaces de componente"
83. "Implementa documentación Storybook para componentes UI Angular"
84. "Genera README para librería de componentes Angular"
85. "Crea documentación de API para servicios inyectables"
86. "Implementa comentarios descriptivos para componentes complejos"
87. "Genera documentación de uso para servicios Angular"
88. "Crea guías de migración para actualizaciones de componentes"
89. "Implementa documentación de testing para componentes"
90. "Genera documentación de accesibilidad para componentes UI"

### 10. Casos Prácticos en Aplicaciones Empresariales con Angular y TypeScript

91. "Genera un dashboard administrativo en Angular con gráficos usando Angular Material"
92. "Crea un sistema de gestión de usuarios en Angular con roles usando guards"
93. "Implementa un editor de contenido rico en Angular con TypeScript usando Angular CDK"
94. "Genera una aplicación de e-commerce en Angular con carrito usando signals"
95. "Crea un sistema de reservas en Angular con calendario usando Angular Material"
96. "Implementa un portal de empleados en Angular con autenticación usando JWT"
97. "Genera un sistema de reportes en Angular con exportación usando Angular CDK"
98. "Crea una aplicación de gestión de proyectos en Angular"
99. "Implementa un sistema de notificaciones en tiempo real en Angular usando WebSockets"
100. "Genera una aplicación de análisis de datos en Angular con visualizaciones usando Angular Material"

### 11. Integración con APIs y Estado Global Avanzado

101. "Genera integración avanzada con HttpClient en TypeScript para gestión de estado servidor con interceptores, retry logic, y error handling. Incluya configuración de timeout, cache, y transformación de responses."
102. "Crea un sistema de estado global en Angular usando NgRx con stores tipados, effects, y selectors. Incluya slices para diferentes dominios (auth, cart, notifications) con acciones asíncronas y memoización."
103. "Implementa integración con GraphQL en Angular usando Apollo Client con queries, mutations, subscriptions en tiempo real, y caching inteligente. Incluya configuración de client con error handling, authentication headers, y optimistic UI updates."
104. "Genera un servicio de API REST en Angular con HttpClient interceptores para autenticación automática, retry logic con exponential backoff, y transformación de responses. Incluya tipos TypeScript para requests/responses y manejo de errores global."
105. "Crea un patrón de composición de servicios en Angular para combinar múltiples servicios inyectables (auth, permissions, profile) en un servicio compuesto. Incluya memoización con signals y optimización de re-renders."
106. "Implementa sincronización de estado offline en Angular usando Service Workers y IndexedDB para cache local, sync queue para mutations pendientes, y conflict resolution. Incluya indicadores de conectividad y retry automático al reconectar."
107. "Genera integración con WebSockets en Angular para comunicación bidireccional usando native WebSockets. Incluya reconexión automática, rooms/channels, y actualización de estado en tiempo real con signals."
108. "Crea un sistema de gestión de formularios avanzado en Angular con ReactiveFormsModule y custom validators. Incluya validaciones asíncronas, arrays dinámicos, valueChanges subscriptions, y integración con UI libraries como Angular Material."
109. "Implementa lazy loading inteligente en Angular con loadChildren, dynamic imports, y preload strategies. Incluya code splitting por rutas, componentes críticos, y loading states personalizados con skeletons."
110. "Genera un sistema de notificaciones push en Angular usando Service Workers y Notification API. Incluya suscripción automática, permisos de usuario, y manejo de clicks para navegación interna con Angular Router."

### 12. Optimización de Rendimiento en Angular

111. "Implementa memoización avanzada en Angular usando OnPush change detection, trackBy functions, y pure pipes. Incluya profiling con Angular DevTools, bundle analysis con webpack-bundle-analyzer, y lazy loading de módulos."
112. "Crea virtualización de listas en Angular con Angular CDK virtual scrolling para renderizar miles de elementos eficientemente. Incluya scroll infinito, búsqueda, y actualización de datos en tiempo real sin re-renders completos."
113. "Genera optimización de re-renders en Angular identificando componentes innecesarios con lifecycle hooks personalizados. Incluya shallow comparison, stable references, y context optimization con skipSelf."
114. "Implementa code splitting estratégico en Angular con dynamic imports, route-based splitting, y preload hints. Incluya análisis de bundles, tree shaking, y loading states con ng-template."
115. "Crea un sistema de caching inteligente en Angular con HttpClient interceptors para datos frecuentemente accedidos. Incluya background refetch, deduplication, y optimistic updates para mejorar UX."
116. "Genera profiling de performance en Angular con Angular Profiler, measurement de render times, y custom pipes para tracking. Incluya reporting a analytics y alerts para regressions de performance."
117. "Implementa web workers en Angular para cálculos pesados usando Angular's built-in web worker support. Incluya comunicación bidireccional, error handling, y actualización de UI desde workers sin blocking."
118. "Crea optimización de imágenes en Angular con lazy loading, responsive images, y WebP format. Incluya blur placeholders, progressive loading, y CDN integration con automatic optimization."
119. "Genera reducción de bundle size en Angular con tree shaking, dynamic imports, y análisis de dependencias. Incluya removal de unused code, minification avanzada, y gzip compression."
120. "Implementa debouncing y throttling en Angular para eventos de usuario (scroll, resize, input) usando custom operators RxJS. Incluya cleanup automático, configuración dinámica, y testing de performance."

### 13. Seguridad y Mejores Prácticas en Angular

121. "Implementa Content Security Policy (CSP) en Angular con headers seguros, nonce generation, y inline styles/scripts seguros. Incluya configuración de webpack para nonces y reporting de violations."
122. "Crea sanitización de inputs en Angular usando DomSanitizer contra XSS. Incluya validación de tipos, encoding automático, y custom sanitizers para diferentes contextos."
123. "Genera autenticación segura en Angular con JWT tokens, refresh automático, y secure storage (httpOnly cookies). Incluya route protection con guards, token expiration handling, y logout automático."
124. "Implementa protección contra CSRF en Angular con tokens anti-forgery, SameSite cookies, y validation en API. Incluya configuración de CORS segura y headers apropiados."
125. "Crea auditoría de seguridad en Angular con ESLint security plugins, dependency scanning con npm audit, y SAST tools. Incluya reporting automático y remediation guidelines."
126. "Genera encriptación de datos sensibles en Angular usando Web Crypto API para client-side encryption. Incluya key management seguro, PBKDF2 para key derivation, y zero-knowledge patterns."
127. "Implementa rate limiting en Angular para API calls con custom interceptors y exponential backoff. Incluya circuit breaker pattern, retry logic, y user feedback para límites excedidos."
128. "Crea validación de tipos estricta en Angular con TypeScript strict mode, noImplicitAny, y custom type guards. Incluya branded types para IDs, discriminated unions, y exhaustive checks."
129. "Genera manejo seguro de errores en Angular con error boundaries, global error handlers, y logging estructurado. Incluya user-friendly messages, recovery mechanisms, y error reporting a servicios externos."
130. "Implementa mejores prácticas de accesibilidad en Angular con Angular CDK a11y, ARIA attributes automáticos, y keyboard navigation. Incluya focus management, screen reader support, y WCAG compliance testing."

### 14. Casos Avanzados y Proyecto Final con Copilot

131. "Genera arquitectura completa de aplicación fullstack con backend ASP.NET Core API y frontend Angular standalone. Incluya estructura de carpetas, separación de concerns, y configuración de desarrollo con hot reload."
132. "Crea sistema de autenticación integrado JWT entre .NET backend y Angular frontend. Incluya guards para rutas protegidas, interceptores para tokens, y manejo de refresh tokens automático."
133. "Implementa lazy loading avanzado en Angular con dynamic imports, preload strategies, y code splitting por features. Incluya análisis de bundles y optimización de carga inicial."
134. "Genera integración con GraphQL en Angular usando Apollo Client para consultas complejas. Incluya queries, mutations, subscriptions, y caching inteligente con error handling."
135. "Crea sistema de notificaciones en tiempo real con SignalR en .NET y Angular usando RxJS. Incluya conexión automática, reconexión, y actualización de UI en tiempo real."
136. "Implementa internacionalización completa en Angular con ngx-translate y backend support en .NET. Incluya lazy loading de traducciones, cambio dinámico de idioma, y pluralización."
137. "Genera dashboard administrativo en Angular con Angular Material, gráficos de Chart.js, y reportes exportables. Incluya responsive design, filtros avanzados, y PWA capabilities."
138. "Crea sistema de logging distribuido entre Angular frontend y .NET backend. Incluya correlación de requests, structured logging con Serilog, y dashboard de Kibana."
139. "Implementa testing end-to-end completo con Playwright para aplicación fullstack. Incluya configuración de tests, page objects, y CI/CD integration con reporting."
140. "Genera documentación técnica completa para proyecto final fullstack. Incluya README detallado, diagramas de arquitectura, API documentation con Swagger, y guías de despliegue."


## Prompts para VBA

### 1. Redacción y Análisis de Código VBA

1. "Genera una función VBA en Excel para validar y procesar datos de una hoja de cálculo con manejo de errores y logging. Incluya verificación de tipos de datos, rangos válidos, y mensajes de usuario claros."
2. "Crea un procedimiento VBA en Access para importar datos desde CSV a tabla con transformación de datos. Incluya mapeo de columnas, conversión de tipos, y validación de integridad referencial."
3. "Implementa una macro VBA en Word para automatizar la generación de documentos con plantillas. Incluya reemplazo de marcadores, inserción de imágenes, y formato condicional basado en datos."
4. "Genera código VBA para Outlook que procese emails entrantes y extraiga información estructurada. Incluya parsing de cuerpo de email, attachments handling, y almacenamiento en Excel."
5. "Crea una función VBA personalizada para cálculos financieros complejos en Excel. Incluya validación de parámetros, manejo de errores matemáticos, y documentación inline."
6. "Implementa un formulario VBA en Access con validación en tiempo real y navegación intuitiva. Incluya controles personalizados, eventos de usuario, y integración con queries."
7. "Genera código VBA para PowerPoint que automatice la creación de presentaciones desde datos Excel. Incluya inserción de gráficos, animaciones, y formato consistente."
8. "Crea un módulo VBA para análisis de datos en Excel con funciones estadísticas avanzadas. Incluya procesamiento de arrays grandes, optimización de performance, y exportación de resultados."
9. "Implementa sistema de logging VBA personalizado para debugging y monitoreo. Incluya niveles de log, timestamps, y escritura a archivo o base de datos."
10. "Genera código VBA para integración entre aplicaciones Office con automatización OLE. Incluya comunicación entre Excel, Word y Access con manejo de excepciones."

### 2. Limpieza y Refactorización de Código VBA

11. "Refactoriza código VBA spaghetti con múltiples bucles anidados y variables globales. Convierte a procedimientos modulares con parámetros claros, elimina código duplicado, y mejora legibilidad."
12. "Optimiza performance de macro VBA que procesa miles de filas en Excel. Implementa procesamiento por lotes, deshabilita screen updating, y usa arrays en lugar de range operations."
13. "Convierte código VBA con manejo de errores básico a error handling estructurado. Incluya On Error statements apropiados, logging de errores, y recovery mechanisms."
14. "Refactoriza funciones VBA con parámetros opcionales confusos. Establece parámetros por defecto claros, valida entradas, y documenta comportamiento esperado."
15. "Elimina código VBA obsoleto y dependencias no utilizadas. Identifica dead code, remueve referencias innecesarias, y optimiza tamaño del proyecto."
16. "Convierte constantes mágicas en VBA a constantes nombradas. Define enumeraciones apropiadas, mejora mantenibilidad, y reduce errores de tipeo."
17. "Refactoriza procedimientos VBA largos en funciones más pequeñas. Aplica principio de responsabilidad única, mejora testabilidad, y facilita debugging."
18. "Optimiza uso de memoria en VBA con liberación explícita de objetos. Implementa proper cleanup de COM objects, evita memory leaks, y mejora estabilidad."
19. "Convierte código VBA procedural a orientado a objetos básico. Crea clases personalizadas, encapsula datos, y mejora reusabilidad del código."
20. "Refactoriza consultas SQL embebidas en VBA a stored procedures. Mejora seguridad, performance, y mantenibilidad separando lógica de datos."

### 3. Migración de Código VBA a C# con Ayuda de Copilot

21. "Migra función VBA de Excel a método C# equivalente con LINQ. Convierte bucles For a expresiones lambda, mantiene lógica de negocio, y adapta a .NET framework."
22. "Convierte procedimiento VBA de Access a clase C# con Entity Framework. Mapea tablas a entidades, implementa CRUD operations, y maneja relaciones de datos."
23. "Migra macro VBA de Word a aplicación C# con OpenXML SDK. Convierte manipulación de documentos, mantiene formato, y mejora performance."
24. "Adapta código VBA de Outlook a C# con MailKit o similar. Convierte procesamiento de emails, mantiene lógica de parsing, y mejora seguridad."
25. "Convierte función financiera VBA a método C# con Math.NET. Mantiene precisión matemática, adapta algoritmos, y añade validación robusta."
26. "Migra formulario VBA de Access a aplicación WPF/WinForms en C#. Convierte UI controls, mantiene validación, y mejora UX con .NET controls."
27. "Adapta código VBA de PowerPoint a C# con Presentation Framework. Convierte creación de slides, mantiene animaciones, y añade export capabilities."
28. "Convierte módulo VBA de análisis estadístico a C# con Math.NET Numerics. Mantiene algoritmos, mejora performance, y añade testing unitario."
29. "Migra sistema de logging VBA personalizado a C# con Serilog. Convierte niveles de log, mantiene estructura, y añade sinks modernos."
30. "Adapta integración VBA entre Office apps a C# con Office Interop. Mantiene automatización, mejora error handling, y añade async operations."