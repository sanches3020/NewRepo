namespace Sofia.Web.Models
{
    public class SaveEmotionRequest
    {
        public string Date { get; set; } = string.Empty;
        public string Emotion { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}