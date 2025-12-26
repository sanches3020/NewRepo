using System.ComponentModel.DataAnnotations;

namespace Sofia.Web.Models;

public enum TestType
{
    BuiltIn,
    Custom
}

public class Test
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public TestType Type { get; set; } = TestType.BuiltIn;

    // For psychologist-created tests
    public int? CreatedByPsychologistId { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
