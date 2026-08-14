namespace CareerPilot_AI.DTOs
{
    public class ResumeAnalysisDTO
    {
        public int ResumeId { get; set; }
        public double ATSScore { get; set; }

        public string Summary { get; set; }

        public string Skills { get; set; }

        public string MissingSkills { get; set; }

        public DateTime AnalyzedAt { get; set; }

        public string Suggestions { get; set; }
    }
}
