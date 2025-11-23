using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofia.Web.Models
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Текст заметки обязателен")]
        [StringLength(2000, ErrorMessage = "Максимум 2000 символов")]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Tags { get; set; }

        [Required]
        public EmotionType Emotion { get; set; }

        [StringLength(500)]
        public string? Activity { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public bool IsPinned { get; set; } = false;

        public bool ShareWithPsychologist { get; set; } = false;

        // Связь с пользователем
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User? User { get; set; }
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
}
