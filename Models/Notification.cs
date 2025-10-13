using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class Notification
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Message { get; set; }
    
    public NotificationType Type { get; set; }
    
    public NotificationPriority Priority { get; set; } = NotificationPriority.Medium;
    
    public bool IsRead { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? ScheduledAt { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    
    [StringLength(100)]
    public string? ActionUrl { get; set; }
    
    [StringLength(50)]
    public string? ActionText { get; set; }
}

public class NotificationSettings
{
    public int Id { get; set; }
    
    public bool DailyReminder { get; set; } = true;
    
    public TimeSpan DailyReminderTime { get; set; } = new TimeSpan(20, 0, 0); // 20:00
    
    public bool GoalReminder { get; set; } = true;
    
    public bool MoodCheckReminder { get; set; } = true;
    
    public TimeSpan MoodCheckTime { get; set; } = new TimeSpan(19, 0, 0); // 19:00
    
    public bool WeeklyReport { get; set; } = true;
    
    public DayOfWeek WeeklyReportDay { get; set; } = DayOfWeek.Sunday;
    
    public bool PracticeReminder { get; set; } = true;
    
    public bool PsychologistReminder { get; set; } = true;
    
    public bool EmailNotifications { get; set; } = false;
    
    public bool PushNotifications { get; set; } = true;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public enum NotificationType
{
    DailyReminder = 1,
    GoalReminder = 2,
    MoodCheck = 3,
    PracticeReminder = 4,
    WeeklyReport = 5,
    PsychologistReminder = 6,
    Achievement = 7,
    System = 8
}

public enum NotificationPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
