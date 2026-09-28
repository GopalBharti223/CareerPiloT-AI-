namespace CareerPilot_AI.DTOs
{
    public class JobMatchingHistoryDTO
    {
        public int JobMatchingHistoryId { get; set; }
        public int ResumeId { get; set; }
        public string JobTitle { get; set; }
        public string? Company { get; set; }
        public int MatchScore { get; set; }
        public List<string> MatchedSkills { get; set; }
        public List<string> MissingSkills { get; set; }
        public List<string> MatchedKeywords { get; set; }
        public List<string> MissingKeywords { get; set; }
        public string ExperienceMatch { get; set; }
        public List<string> Suggestions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}