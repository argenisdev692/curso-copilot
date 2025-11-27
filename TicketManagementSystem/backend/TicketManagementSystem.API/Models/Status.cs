// Enum para los estados del ticket en el workflow del sistema 
// Estados en orden de progresión:  
// - Open (nuevo ticket creado) 
// - InProgress (asignado y en trabajo) 
// - Resolved (solucionado, pendiente verificación) 
// - Closed (cerrado definitivamente) 
// Valores numéricos: Open=0, InProgress=1, Resolved=2, Closed=3 
// Transiciones válidas: Open→InProgress→Resolved→Closed 
namespace TicketManagementSystem.API.Models
{
    public enum Status
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }
}
