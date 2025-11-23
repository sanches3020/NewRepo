using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class UserStatistics
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public DateTime Date { get; set; }

    // Статистика по заметкам
    public int NotesCount { get; set; } = 0;
    public int PinnedNotesCount { get; set; } = 0;
    public int SharedNotesCount { get; set; } = 0;

    // Статистика по целям
    public int GoalsCount { get; set; } = 0;
    public int CompletedGoalsCount { get; set; } = 0;
    public int ActiveGoalsCount { get; set; } = 0;
    public double AverageGoalProgress { get; set; } = 0;

    // Статистика по эмоциям
    public int EmotionsCount { get; set; } = 0;
    public EmotionType? DominantEmotion { get; set; }
    public double AverageEmotionScore { get; set; } = 0;

    // Статистика по практикам
    public int PracticesCount { get; set; } = 0;
    public int TotalPracticeMinutes { get; set; } = 0;

    // Статистика по сессиям с психологом
    public int AppointmentsCount { get; set; } = 0;
    public int CompletedAppointmentsCount { get; set; } = 0;

    // Общая статистика
    public int TotalActivityDays { get; set; } = 0;
    public int CurrentStreak { get; set; } = 0; // Дней подряд активности
    public int LongestStreak { get; set; } = 0;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
