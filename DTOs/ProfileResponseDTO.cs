using Microsoft.AspNetCore.Http.HttpResults;

namespace CareerPilot_AI.DTOs
{
    public class ProfileResponseDTO
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
