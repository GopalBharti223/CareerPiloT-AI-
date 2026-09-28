using System.ComponentModel.DataAnnotations;
namespace CareerPilot_AI.DTOs

{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(10)]
        public string MobileNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
