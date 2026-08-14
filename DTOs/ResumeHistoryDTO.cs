namespace CareerPilot_AI.DTOs
{
    public class ResumeHistoryDTO
    {
        public int ResumeId { get; set; }
        public string FileName { get; set; }
        public double ATSScore { get; set; } 

        public DateTime AnalyzedAt { get; set; }

    }
}
