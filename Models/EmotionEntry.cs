using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofia.Web.Models
{
    public class EmotionEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public EmotionType Emotion { get; set; }

        public string? Note { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
