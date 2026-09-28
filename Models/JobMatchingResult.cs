namespace CareerPilot_AI.Models
{
    public class JobMatchingResult
    {
        public int MatchScore { get; set; }

        public List<string> MatchedSkills { get; set; }

        public List<string> MissingSkills { get; set; }

        public List<string> MatchedKeywords { get; set; }

        public List<string> MissingKeywords { get; set; }

        public string ExperienceMatch { get; set; }

        public List<string> Suggestions { get; set; }
    }
}