using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
namespace TicketManagementSystem.API.Models; 
/// <summary> 
/// Entidad User para el sistema de tickets 
/// </summary> 
public class User 
{ 
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; } 
    [Required] 
    [MaxLength(256)] 
    [EmailAddress] 
    public string Email { get; set; } = string.Empty; 
    [Required] 
    public string PasswordHash { get; set; } = string.Empty; 
    [Required] 
    [MaxLength(100)] 
    public string FullName { get; set; } = string.Empty; 
    [Required] 
    [MaxLength(20)] 
    public string Role { get; set; } = "User"; // Admin, Agent, User 
    public bool IsActive { get; set; } = true; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 
    public bool IsDeleted { get; set; } = false;
    // Relaciones de navegación 
    /// <summary>
    /// Tickets creados por este usuario
    /// </summary>
    [InverseProperty("CreatedBy")]
    public virtual ICollection<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
    /// <summary>
    /// Tickets asignados a este usuario
    /// </summary>
    [InverseProperty("AssignedTo")]
    public virtual ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    /// <summary> 
    /// Comentarios creados por este usuario 
    /// </summary> 
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>(); 
}