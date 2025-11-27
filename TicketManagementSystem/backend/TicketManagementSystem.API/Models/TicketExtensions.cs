using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Models
{
    /// <summary>
    /// Métodos de extensión para la entidad Ticket.
    /// </summary>
    public static class TicketExtensions
    {
        /// <summary>
        /// Verifica si el ticket está vencido (más de 7 días sin actualizar).
        /// </summary>
        /// <param name="ticket">El ticket a verificar.</param>
        /// <returns>True si el ticket tiene más de 7 días sin actualizar.</returns>
        public static bool IsOverdue(this Ticket ticket)
        {
            var daysSinceUpdate = (DateTime.UtcNow - ticket.UpdatedAt).TotalDays;
            return daysSinceUpdate > 7;
        }

        /// <summary>
        /// Obtiene el color CSS correspondiente a la prioridad del ticket.
        /// </summary>
        /// <param name="ticket">El ticket.</param>
        /// <returns>Color CSS en formato hexadecimal.</returns>
        public static string GetPriorityColor(this Ticket ticket)
        {
            return ticket.Priority switch
            {
                Priority.Low => "#4CAF50",      // Verde
                Priority.Medium => "#FFC107",   // Amarillo
                Priority.High => "#FF5722",     // Naranja
                Priority.Critical => "#F44336", // Rojo
                _ => "#9E9E9E"                   // Gris por defecto
            };
        }

        /// <summary>
        /// Verifica si el usuario puede ser asignado al ticket.
        /// </summary>
        /// <param name="ticket">El ticket.</param>
        /// <param name="user">El usuario a asignar.</param>
        /// <returns>True si el usuario puede ser asignado.</returns>
        public static bool CanBeAssignedTo(this Ticket ticket, User user)
        {
            return user.IsActive &&
                   (user.Role == "Agent" || user.Role == "Admin") &&
                   ticket.Status != Status.Closed;
        }

        /// <summary>
        /// Obtiene la clase CSS de Bootstrap para el badge del status del ticket.
        /// </summary>
        /// <param name="ticket">El ticket.</param>
        /// <returns>Clase CSS de Bootstrap para el badge.</returns>
        public static string GetStatusBadgeClass(this Ticket ticket)
        {
            return ticket.Status switch
            {
                Status.Open => "badge-success",
                Status.InProgress => "badge-info",
                Status.Resolved => "badge-warning",
                Status.Closed => "badge-secondary",
                _ => "badge-light"  // Por defecto
            };
        }
    }
}