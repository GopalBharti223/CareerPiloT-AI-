using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class ChangePasswordDTO
    {

        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

    }
}
