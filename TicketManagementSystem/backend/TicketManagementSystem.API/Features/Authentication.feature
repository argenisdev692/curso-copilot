Feature: Autenticación de Usuario
  As a usuario del sistema
  I want to poder iniciar sesión y registrarme
  So that pueda acceder a mis recursos y funcionalidades

  Background:
    Given el sistema de tickets está disponible

  Scenario: Login exitoso con credenciales válidas
    Given el usuario está registrado con email "usuario@test.com" y password "Password123!"
    And el usuario está en la página de login
    When el usuario ingresa email "usuario@test.com"
    And el usuario ingresa password "Password123!"
    And el usuario hace click en "Iniciar Sesión"
    Then el usuario recibe un token de autenticación válido
    And el usuario ve el dashboard
    And el mensaje de bienvenida muestra "Bienvenido"

  Scenario: Login fallido con credenciales inválidas
    Given el usuario está en la página de login
    When el usuario ingresa email "usuario@test.com"
    And el usuario ingresa password "wrongpassword"
    And el usuario hace click en "Iniciar Sesión"
    Then el usuario ve mensaje de error "Credenciales inválidas"
    And el usuario permanece en la página de login
    And no se genera token de autenticación

  Scenario: Registro exitoso de nuevo usuario
    Given el usuario está en la página de registro
    And no existe usuario con email "nuevo@test.com"
    When el usuario ingresa nombre "Juan Pérez"
    And el usuario ingresa email "nuevo@test.com"
    And el usuario ingresa password "SecurePass123!"
    And el usuario confirma password "SecurePass123!"
    And el usuario hace click en "Registrarse"
    Then el usuario es creado exitosamente
    And el usuario recibe mensaje "Registro completado"
    And el usuario es redirigido a la página de login

  Scenario Outline: Validación de campos requeridos en registro
    Given el usuario está en la página de registro
    When el usuario deja campo "<campo>" vacío
    And el usuario completa los demás campos correctamente
    And el usuario hace click en "Registrarse"
    Then el usuario ve mensaje de error "<mensaje>"
    And el formulario no es enviado

    Examples:
      | campo     | mensaje                    |
      | nombre    | Nombre es requerido       |
      | email     | Email es requerido        |
      | password  | Password es requerido     |

  Scenario: Registro con email ya existente
    Given existe usuario registrado con email "existente@test.com"
    And el usuario está en la página de registro
    When el usuario ingresa nombre "Otro Usuario"
    And el usuario ingresa email "existente@test.com"
    And el usuario ingresa password "Password123!"
    And el usuario confirma password "Password123!"
    And el usuario hace click en "Registrarse"
    Then el usuario ve mensaje de error "Email ya está registrado"
    And el formulario no es enviado

  Scenario: Logout exitoso
    Given el usuario está autenticado
    And el usuario está en el dashboard
    When el usuario hace click en "Cerrar Sesión"
    Then el usuario es desautenticado
    And el token de autenticación es invalidado
    And el usuario es redirigido a la página de login

  Scenario: Acceso a recurso protegido sin autenticación
    Given el usuario no está autenticado
    When el usuario intenta acceder a "/api/tickets"
    Then el usuario recibe respuesta HTTP 401 Unauthorized
    And el mensaje indica "Token requerido"