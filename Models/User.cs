namespace CareerPilot_AI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        // public ICollection<PasswordResetOTP> PasswordResetOTPs { get; set; }

        public ICollection<Resume> Resumes { get; set; } // it means one user can have many resumes
    }
}
