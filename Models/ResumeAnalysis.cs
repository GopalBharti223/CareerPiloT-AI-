using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.Models
{
    public class ResumeAnalysis
    {
        [Key]
        public int AnalysisId { get; set; }   // Primary Key

        public int ResumeId { get; set; }           // Foreign Key

        public int ATSScore { get; set; }

        public string MissingSkills { get; set; }

        public string Skills { get; set; }
        public string Summary { get; set; }




        //public string Strength { get; set; }

        //public string Weakness { get; set; }

        public string Suggestions { get; set; }

        public DateTime AnalyzedAt { get; set; }

        public Resume Resume { get; set; }  // Navigation property

    
    }
}
