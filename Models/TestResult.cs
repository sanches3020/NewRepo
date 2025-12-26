using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public class TestResult
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TestId { get; set; }
    public virtual Test? Test { get; set; }

    public DateTime TakenAt { get; set; } = DateTime.Now;

    // Total numeric score
    public int Score { get; set; }

    // e.g. "Low", "Medium", "High"
    [StringLength(50)]
    public string? Level { get; set; }

    [StringLength(2000)]
    public string? Interpretation { get; set; }
}
