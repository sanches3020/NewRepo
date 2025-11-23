namespace Sofia.Web.Models
{
    public class CreateNoteRequest
    {
        public string Content { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public string Emotion { get; set; } = string.Empty;
        public string? Activity { get; set; }
        public string? Date { get; set; }
        public bool IsPinned { get; set; } = false;
        public bool ShareWithPsychologist { get; set; } = false;
    }
}

