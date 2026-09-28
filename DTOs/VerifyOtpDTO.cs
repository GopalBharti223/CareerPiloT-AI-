using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class VerifyOtpDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }
    }
}