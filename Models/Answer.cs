using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class Answer
{
    public int Id { get; set; }

    public int QuestionId { get; set; }
    public virtual Question? Question { get; set; }

    [Required]
    [StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    // Numeric value used in scoring
    public int Value { get; set; } = 0;

    public int Order { get; set; } = 0;
}
