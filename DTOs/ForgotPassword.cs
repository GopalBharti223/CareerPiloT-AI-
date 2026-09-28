using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.DTOs
{
    public class ForgotPasswordDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }


    }
}
