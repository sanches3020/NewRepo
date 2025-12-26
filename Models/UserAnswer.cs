using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class UserAnswer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuestionId { get; set; }
    public virtual Question? Question { get; set; }

    // Selected answer (for single-choice) or one of selected options (for multiple we store one record per selection)
    public int? AnswerId { get; set; }
    public virtual Answer? Answer { get; set; }

    // Free text answer (for Text type)
    public string? TextAnswer { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
