using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class EmotionEntry
{
    public int Id { get; set; }
    
    public DateTime Date { get; set; }
    
    public EmotionType Emotion { get; set; }
    
    [StringLength(500)]
    public string? Note { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Связь с пользователем
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
}

public class DayEmotions
{
    public DateTime Date { get; set; }
    public List<EmotionEntry> Emotions { get; set; } = new List<EmotionEntry>();
    public List<Note> Notes { get; set; } = new List<Note>();
    public List<Goal> Goals { get; set; } = new List<Goal>();
}

