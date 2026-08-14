namespace CareerPilot_AI.Models
{
    public class ResumeAnalysisResult
    {
        public int ATSScore { get; set; }

        public string Summary { get; set; }

        public List<string> Skills { get; set; }

        public List<string> MissingSkills { get; set; }

        public List<string> Suggestions { get; set; }
    }
}