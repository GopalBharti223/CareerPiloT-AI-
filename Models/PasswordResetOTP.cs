using System.ComponentModel.DataAnnotations;

namespace CareerPilot_AI.Models
{
    public class PasswordResetOTP
    {
        [Key]
        public int OtpId { get; set; }

        public int UserId { get; set; }

        public string OTP { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int FailedAttempts { get; set; }

        public User User { get; set; }
    }
}