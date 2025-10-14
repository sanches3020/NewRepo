using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class Goal
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public GoalType Type { get; set; }
    
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? TargetDate { get; set; }
    
    public int Progress { get; set; } = 0;
    
    public bool IsFromPsychologist { get; set; }
    
    // Связь с пользователем
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
}

public enum GoalType
{
    Personal = 1,
    Therapy = 2,
    Wellness = 3,
    Social = 4,
    Professional = 5
}

public enum GoalStatus
{
    Active = 1,
    Completed = 2,
    Paused = 3,
    Cancelled = 4
}
