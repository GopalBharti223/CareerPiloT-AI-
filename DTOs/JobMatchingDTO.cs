using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class JobMatchingDTO
    {
        [Required]
        public int ResumeId { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string? Company { get; set; }

        [Required]
        [MinLength(20)]
        public string JobDescription { get; set; }
    }
}