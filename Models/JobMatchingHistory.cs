namespace CareerPilot_AI.Models
{
    public class JobMatchingHistory
    {
        public int JobMatchingHistoryId { get; set; }

        public int UserId { get; set; }

        public int ResumeId { get; set; }

        public string JobTitle { get; set; }

        public string? Company { get; set; }

        public int MatchScore { get; set; }

        public string MatchedSkills { get; set; }

        public string MissingSkills { get; set; }

        public string MatchedKeywords { get; set; }

        public string MissingKeywords { get; set; }

        public string ExperienceMatch { get; set; }

        public string Suggestions { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}