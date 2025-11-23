using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models
{
    public class Practice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public PracticeCategory Category { get; set; }

        public int DurationMinutes { get; set; }

        public string? Instructions { get; set; }

        public bool IsActive { get; set; }
    }

    public enum PracticeCategory
    {
        Breathing = 1,
        Visualization = 2,
        CBT = 3,
        Gestalt = 4,
        Meditation = 5,
        Mindfulness = 6,
        Relaxation = 7
    }
}
