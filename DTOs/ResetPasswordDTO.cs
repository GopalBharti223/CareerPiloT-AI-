using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string OTP { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}
