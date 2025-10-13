using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class Note
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Текст заметки обязателен")]
    [StringLength(2000, ErrorMessage = "Максимум 2000 символов")]
    public string Content { get; set; } = string.Empty;
    
    public string? Tags { get; set; }
    
    public EmotionType Emotion { get; set; }
    
    public string? Activity { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsPinned { get; set; }
    
    public bool ShareWithPsychologist { get; set; }
}

public enum EmotionType
{
    VerySad = 1,
    Sad = 2,
    Neutral = 3,
    Happy = 4,
    VeryHappy = 5,
    Anxious = 6,
    Calm = 7,
    Excited = 8,
    Frustrated = 9,
    Grateful = 10
}
