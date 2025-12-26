using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public enum AnswerType
{
    SingleChoice,
    MultipleChoice,
    Numeric,
    Text
}

public class Question
{
    public int Id { get; set; }

    public int TestId { get; set; }
    public virtual Test? Test { get; set; }

    [Required]
    [StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    public AnswerType Type { get; set; } = AnswerType.SingleChoice;

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
