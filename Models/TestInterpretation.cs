using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sofia.Web.Models
{
    public class TestInterpretation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TestId { get; set; }

        // Percent based thresholds (0-100)
        [Range(0, 100)]
        public double MinPercent { get; set; }

        [Range(0, 100)]
        public double MaxPercent { get; set; }

        [Required]
        [StringLength(200)]
        public string Level { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? InterpretationText { get; set; }

        [ForeignKey("TestId")]
        public Test? Test { get; set; }
    }
}