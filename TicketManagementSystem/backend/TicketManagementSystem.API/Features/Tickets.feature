Feature: Gestión de Tickets
  As a agente de soporte
  I want to crear, actualizar y gestionar tickets
  So that pueda resolver problemas de los usuarios eficientemente

  Background:
    Given el agente está autenticado como "Agente"
    And el sistema tiene usuarios registrados

  Scenario: Crear ticket nuevo exitosamente
    Given el agente está en la página de tickets
    When el agente hace click en "Nuevo Ticket"
    And el agente ingresa título "Problema con login"
    And el agente selecciona prioridad "Alta"
    And el agente ingresa descripción "Usuario no puede acceder al sistema"
    And el agente selecciona usuario asignado
    And el agente hace click en "Crear"
    Then el ticket es creado exitosamente
    And el ticket tiene ID único generado
    And el ticket tiene estado "Abierto"
    And el ticket tiene fecha de creación actual
    And el agente ve mensaje de confirmación

  Scenario: Actualizar estado de ticket
    Given existe un ticket con ID "123" en estado "Abierto"
    And el agente está viendo el ticket "123"
    When el agente cambia el estado a "En Progreso"
    And el agente hace click en "Actualizar"
    Then el ticket "123" tiene estado "En Progreso"
    And la fecha de actualización es modificada
    And se registra el cambio en el historial

  Scenario: Asignar ticket a agente
    Given existe un ticket sin asignar con ID "456"
    And existe agente "María González" disponible
    When el agente asigna el ticket "456" a "María González"
    Then el ticket "456" está asignado a "María González"
    And el agente recibe notificación de asignación

  Scenario Outline: Validación de campos requeridos al crear ticket
    Given el agente está creando un nuevo ticket
    When el agente deja campo "<campo>" vacío
    And el agente completa los demás campos
    And el agente hace click en "Crear"
    Then el agente ve mensaje de error "<mensaje>"
    And el ticket no es creado

    Examples:
      | campo      | mensaje                  |
      | título     | Título es requerido     |
      | descripción| Descripción es requerida|
      | prioridad  | Prioridad es requerida  |

  Scenario: Cerrar ticket resuelto
    Given existe ticket "789" en estado "En Progreso"
    And el ticket tiene una resolución documentada
    When el agente marca el ticket como "Resuelto"
    And el agente ingresa comentario de resolución
    Then el ticket "789" tiene estado "Resuelto"
    And la fecha de resolución es registrada
    And el usuario original recibe notificación

  Scenario: Buscar tickets por criterios
    Given existen múltiples tickets en el sistema
    When el agente busca tickets con filtro "estado = Abierto"
    Then solo se muestran tickets con estado "Abierto"
    And los resultados están ordenados por fecha de creación descendente

  Scenario: Agente sin permisos intenta modificar ticket
    Given el usuario está autenticado como "Usuario Regular"
    And existe ticket "999" asignado a otro agente
    When el usuario intenta actualizar el ticket "999"
    Then el usuario recibe error "No tiene permisos para modificar este ticket"
    And el ticket "999" no es modificado

  Scenario: Visualizar detalles completos de ticket
    Given existe ticket "111" con toda la información
    When el agente selecciona el ticket "111" para ver detalles
    Then el agente ve toda la información del ticket:
      | Campo          | Visible |
      | ID             | Sí      |
      | Título         | Sí      |
      | Descripción    | Sí      |
      | Estado         | Sí      |
      | Prioridad      | Sí      |
      | Usuario creador| Sí      |
      | Agente asignado| Sí      |
      | Fechas         | Sí      |
      | Historial      | Sí      |