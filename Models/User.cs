using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = "user"; // "user" или "psychologist"
    
    [StringLength(100)]
    public string? FullName { get; set; }
    
    [StringLength(500)]
    public string? Bio { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Для психологов - связь с профилем психолога
    public int? PsychologistProfileId { get; set; }
    public virtual Psychologist? PsychologistProfile { get; set; }
    
    // Navigation properties
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
    public virtual ICollection<PsychologistAppointment> Appointments { get; set; } = new List<PsychologistAppointment>();
}

public enum UserRole
{
    User,
    Psychologist
}
