using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class Psychologist
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    // Связь с пользователем
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
    
    [StringLength(500)]
    public string? Specialization { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(200)]
    public string? Education { get; set; }
    
    [StringLength(200)]
    public string? Experience { get; set; }
    
    [StringLength(100)]
    public string? Languages { get; set; }
    
    [StringLength(200)]
    public string? Methods { get; set; }
    
    public decimal? PricePerHour { get; set; }
    
    [StringLength(20)]
    public string? ContactPhone { get; set; }
    
    [StringLength(100)]
    public string? ContactEmail { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual ICollection<PsychologistReview> Reviews { get; set; } = new List<PsychologistReview>();
    public virtual ICollection<PsychologistAppointment> Appointments { get; set; } = new List<PsychologistAppointment>();
}

public class PsychologistReview
{
    public int Id { get; set; }
    
    public int PsychologistId { get; set; }
    
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [StringLength(1000)]
    public string? Comment { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual Psychologist Psychologist { get; set; } = null!;
}

public class PsychologistAppointment
{
    public int Id { get; set; }
    
    public int PsychologistId { get; set; }
    
    public int? UserId { get; set; }
    
    public DateTime AppointmentDate { get; set; }
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual Psychologist Psychologist { get; set; } = null!;
    public virtual User? User { get; set; }
}

public enum AppointmentStatus
{
    Scheduled,
    Confirmed,
    Completed,
    Cancelled,
    NoShow
}
