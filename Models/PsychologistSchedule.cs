using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class PsychologistSchedule
{
    public int Id { get; set; }
    
    public int PsychologistId { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual Psychologist Psychologist { get; set; } = null!;
}

public class PsychologistTimeSlot
{
    public int Id { get; set; }
    
    public int PsychologistId { get; set; }
    
    public DateTime Date { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public bool IsAvailable { get; set; } = true;
    
    public bool IsBooked { get; set; } = false;
    
    public int? BookedByUserId { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual Psychologist Psychologist { get; set; } = null!;
    public virtual User? BookedByUser { get; set; }
}

