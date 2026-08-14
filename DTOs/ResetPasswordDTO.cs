using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}
