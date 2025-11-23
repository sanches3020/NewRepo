using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Message { get; set; }

        public NotificationType Type { get; set; }

        public NotificationPriority Priority { get; set; }

        public bool IsRead { get; set; }

        public bool IsActive { get; set; }

        // Убрали DateTime.Now (ломает миграции)
        public DateTime CreatedAt { get; set; }

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

        public bool DailyReminder { get; set; }

        public TimeSpan DailyReminderTime { get; set; }

        public bool GoalReminder { get; set; }

        public bool MoodCheckReminder { get; set; }

        public TimeSpan MoodCheckTime { get; set; }

        public bool WeeklyReport { get; set; }

        public DayOfWeek WeeklyReportDay { get; set; }

        public bool PracticeReminder { get; set; }

        public bool PsychologistReminder { get; set; }

        public bool EmailNotifications { get; set; }

        public bool PushNotifications { get; set; }

        public DateTime UpdatedAt { get; set; }
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
}
