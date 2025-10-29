using System;
using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models
{
    public class EmotionEntry
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public EmotionType Emotion { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}