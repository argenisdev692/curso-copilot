using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Helpers
{
    public static class TicketHelper
    {
        public static TicketStatusViewModel TransformTicketStatus(Status status, Priority priority)
        {
            var statusText = status switch
            {
                Status.Open => "Abierto",
                Status.InProgress => "En Progreso",
                Status.Resolved => "Resuelto",
                Status.Closed => "Cerrado",
                _ => "Desconocido"
            };

            var badgeColor = status switch
            {
                Status.Open => "#4CAF50",
                Status.InProgress => "#2196F3",
                Status.Resolved => "#FF9800",
                Status.Closed => "#9E9E9E",
                _ => "#000000"
            };

            var priorityText = priority switch
            {
                Priority.Low => "Baja",
                Priority.Medium => "Media",
                Priority.High => "Alta",
                _ => "Desconocida"
            };

            return new TicketStatusViewModel
            {
                StatusText = statusText,
                BadgeColor = badgeColor,
                Priority = priorityText
            };
        }

        public static DaysSinceCreationResult CalculateDaysSinceCreation(DateTime createdAt, DateTime? currentDate = null)
        {
            var now = currentDate ?? DateTime.UtcNow;
            var daysElapsed = (int)(now - createdAt).TotalDays;

            var isOverdue = daysElapsed > 7;

            var category = daysElapsed switch
            {
                <= 2 => "Nuevo",
                <= 7 => "Reciente",
                <= 14 => "Antiguo",
                _ => "Muy Antiguo"
            };

            return new DaysSinceCreationResult
            {
                DaysElapsed = daysElapsed,
                IsOverdue = isOverdue,
                Category = category
            };
        }
    }
}