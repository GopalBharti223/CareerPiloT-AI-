namespace CareerPilot_AI.Models
{
    public class Resume
    {
        public int ResumeId { get; set; }

        public int UserId { get; set; }

        public string FileName { get; set; }

        public string FileLocation { get; set; }

        public DateTime UploadedAt { get; set; }

        public User User { get; set; }

    }
}
