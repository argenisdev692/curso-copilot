Feature: Dashboard del Sistema
  As a usuario autenticado
  I want to ver métricas y resumen de actividad
  So that pueda tener una visión general del estado del sistema

  Background:
    Given el usuario está autenticado
    And existen datos en el sistema

  Scenario: Visualizar métricas principales en dashboard
    Given el usuario accede al dashboard
    Then el usuario ve las siguientes métricas:
      | Métrica              | Visible |
      | Total de tickets     | Sí      |
      | Tickets abiertos     | Sí      |
      | Tickets en progreso  | Sí      |
      | Tickets resueltos    | Sí      |
      | Tickets por prioridad| Sí      |
      | Tiempo promedio resolución | Sí |

  Scenario: Ver tickets recientes
    Given existen tickets creados recientemente
    When el usuario está en el dashboard
    Then el usuario ve lista de "Últimos 5 tickets" con información:
      | Campo      | Visible |
      | ID         | Sí      |
      | Título     | Sí      |
      | Estado     | Sí      |
      | Prioridad  | Sí      |
      | Fecha      | Sí      |

  Scenario: Acceder a ticket desde dashboard
    Given el usuario ve ticket "123" en tickets recientes
    When el usuario hace click en el ticket "123"
    Then el usuario es redirigido a la página de detalle del ticket "123"

  Scenario: Dashboard muestra actividad por período
    Given el usuario selecciona período "Últimos 7 días"
    When el dashboard se actualiza
    Then las métricas reflejan solo datos del período seleccionado
    And los gráficos muestran evolución semanal

  Scenario: Usuario sin datos ve dashboard vacío
    Given el usuario es nuevo y no tiene tickets asociados
    When el usuario accede al dashboard
    Then el usuario ve mensaje "No hay datos para mostrar"
    And las métricas muestran valores en cero
    And se sugiere crear primer ticket

  Scenario: Dashboard con filtros por rol
    Given el usuario está autenticado como "Agente"
    When el agente accede al dashboard
    Then el dashboard muestra métricas filtradas por tickets asignados al agente
    And el agente ve solo sus tickets en "Mis tickets"

  Scenario: Actualización automática de métricas
    Given el dashboard está abierto
    And se crea un nuevo ticket en otra sesión
    When pasan 30 segundos
    Then las métricas se actualizan automáticamente
    And el contador de tickets total aumenta

  Scenario: Exportar reporte desde dashboard
    Given el usuario está en el dashboard con datos
    When el usuario hace click en "Exportar Reporte"
    And el usuario selecciona formato "PDF"
    Then se genera archivo PDF con todas las métricas
    And el archivo incluye gráficos y estadísticas detalladas

  Scenario Outline: Navegación rápida desde dashboard
    Given el usuario está en el dashboard
    When el usuario hace click en "<elemento>"
    Then el usuario es redirigido a "<página>"

    Examples:
      | elemento          | página              |
      | Ver todos tickets | Lista de tickets    |
      | Crear ticket      | Formulario nuevo ticket |
      | Gestionar usuarios| Lista de usuarios   |
      | Mi perfil         | Página de perfil    |