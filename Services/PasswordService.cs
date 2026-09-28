using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace CareerPilot_AI.Services
{
    public class PasswordService
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly EmailService _emailService;

        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public PasswordService(
            CareerPilotAIDbContext context,
            EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public string ChangePassword(
            int userId,
            ChangePasswordDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return "User not found";
            }

            var passwordVerificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    dto.CurrentPassword);

            if (passwordVerificationResult ==
                PasswordVerificationResult.Failed)
            {
                return "Old password is incorrect";
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return "New password and confirm password do not match";
            }

            if (dto.NewPassword == dto.CurrentPassword)
            {
                return "New password must be different from current password";
            }

            user.Password = _passwordHasher.HashPassword(
                user,
                dto.NewPassword);

            _context.SaveChanges();

            return "Password changed successfully";
        }

        public string ForgotPassword(ForgotPasswordDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return "If the email is registered, an OTP has been sent.";
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            int generatedOTP =
         RandomNumberGenerator.GetInt32(100000, 1000000);

            if (passwordReset != null)
            {
                passwordReset.FailedAttempts = 0;
                passwordReset.OTP = generatedOTP.ToString();
                passwordReset.CreatedAt = DateTime.Now;
                passwordReset.ExpiresAt =
                    DateTime.Now.AddMinutes(5);
            }
            else
            {
                PasswordResetOTP newPasswordReset =
                    new PasswordResetOTP();

                newPasswordReset.UserId = user.UserId;
                newPasswordReset.OTP = generatedOTP.ToString();
                newPasswordReset.CreatedAt = DateTime.Now;
                newPasswordReset.ExpiresAt =
                    DateTime.Now.AddMinutes(5);

                _context.PasswordResetOTPs.Add(newPasswordReset);
            }

            _context.SaveChanges();

            _emailService.SendEmail(
                user.Email,
                "CareerPilot AI - Password Reset OTP",
                $"Your OTP is: {generatedOTP}");

            return "If the email is registered, an OTP has been sent.";
        }

        public string VerifyOtp(VerifyOtpDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return "User not found";
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            if (passwordReset == null)
            {
                return "No OTP found for this user";
            }
            if (passwordReset.FailedAttempts >= 5)
            {
                return "OTP verification failed";
            }
            if (DateTime.Now > passwordReset.ExpiresAt)
            {
                return "OTP has expired";
            }

            if (passwordReset.FailedAttempts >= 5)
            {
                return "OTP verification failed";
            }

            if (dto.OTP == passwordReset.OTP)
            {
                return "OTP verified successfully";
            }

            passwordReset.FailedAttempts++;

            _context.SaveChanges();

            return "Invalid OTP";
        }

        public string ResetPassword(ResetPasswordDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return "User not found";
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            if (passwordReset == null)
            {
                return "No OTP found for this user";
            }

            if (DateTime.Now > passwordReset.ExpiresAt)
            {
                return "OTP has expired";
            }

            if (dto.OTP != passwordReset.OTP)
            {
                return "Invalid OTP";
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return "New password and confirm password do not match";
            }

            user.Password = _passwordHasher.HashPassword(
                user,
                dto.NewPassword);

            _context.PasswordResetOTPs.Remove(passwordReset);

            _context.SaveChanges();

            return "Password reset successfully";
        }
    }
}