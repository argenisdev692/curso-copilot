Feature: Gestión de Usuarios
  As a administrador del sistema
  I want to gestionar usuarios y sus permisos
  So that pueda controlar el acceso al sistema

  Background:
    Given el administrador está autenticado como "Admin"
    And el sistema tiene configuración de roles

  Scenario: Crear nuevo usuario exitosamente
    Given el administrador está en la gestión de usuarios
    When el administrador hace click en "Nuevo Usuario"
    And el administrador ingresa nombre "Ana López"
    And el administrador ingresa email "ana.lopez@test.com"
    And el administrador selecciona rol "Agente"
    And el administrador hace click en "Crear Usuario"
    Then el usuario "Ana López" es creado exitosamente
    And el usuario recibe email de bienvenida
    And el usuario aparece en la lista de usuarios activos

  Scenario: Actualizar información de usuario
    Given existe usuario "Pedro García" con rol "Usuario"
    When el administrador edita el usuario "Pedro García"
    And el administrador cambia el rol a "Agente"
    And el administrador guarda los cambios
    Then el usuario "Pedro García" tiene rol "Agente"
    And los cambios son registrados en el historial

  Scenario: Desactivar usuario
    Given existe usuario activo "Carlos Ruiz"
    When el administrador desactiva al usuario "Carlos Ruiz"
    Then el usuario "Carlos Ruiz" tiene estado "Inactivo"
    And el usuario no puede iniciar sesión
    And los tickets asignados al usuario son reasignados

  Scenario Outline: Validación de formato de email al crear usuario
    Given el administrador está creando un nuevo usuario
    When el administrador ingresa email "<email>"
    And el administrador completa los demás campos correctamente
    And el administrador hace click en "Crear Usuario"
    Then el administrador ve mensaje de error "<mensaje>"

    Examples:
      | email           | mensaje                          |
      | usuario         | Email debe tener formato válido  |
      | usuario@        | Email debe tener formato válido  |
      | usuario.com     | Email debe tener formato válido  |
      |                 | Email es requerido               |

  Scenario: Crear usuario con email duplicado
    Given existe usuario con email "duplicado@test.com"
    When el administrador intenta crear usuario con email "duplicado@test.com"
    Then el administrador ve mensaje de error "Email ya está registrado"
    And el usuario no es creado

  Scenario: Administrador visualiza lista de usuarios
    Given existen usuarios en diferentes roles
    When el administrador accede a la lista de usuarios
    Then el administrador ve todos los usuarios con información:
      | Campo      | Visible |
      | Nombre     | Sí      |
      | Email      | Sí      |
      | Rol        | Sí      |
      | Estado     | Sí      |
      | Fecha registro | Sí  |
    And los usuarios están ordenados alfabéticamente por nombre

  Scenario: Filtrar usuarios por rol
    Given existen usuarios con diferentes roles
    When el administrador filtra usuarios por rol "Agente"
    Then solo se muestran usuarios con rol "Agente"
    And el contador muestra la cantidad correcta

  Scenario: Usuario no administrador intenta gestionar usuarios
    Given el usuario está autenticado como "Agente"
    When el usuario intenta acceder a gestión de usuarios
    Then el usuario recibe error "Acceso denegado"
    And el usuario es redirigido al dashboard

  Scenario: Reset de password por administrador
    Given existe usuario "Lucía Fernández"
    When el administrador resetea password del usuario "Lucía Fernández"
    Then el usuario "Lucía Fernández" recibe email con nueva password temporal
    And el usuario debe cambiar password en próximo login