using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly EmailService _emailService;

        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public PasswordController(
            CareerPilotAIDbContext context,
            EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var passwordVerificationResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    dto.CurrentPassword);

            if (passwordVerificationResult ==
                PasswordVerificationResult.Failed)
            {
                return BadRequest("Old password is incorrect");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest(
                    "New password and confirm password do not match");
            }

            if (dto.NewPassword == dto.CurrentPassword)
            {
                return BadRequest(
                    "New password must be different from current password");
            }

            user.Password = _passwordHasher.HashPassword(
                user,
                dto.NewPassword);

            _context.SaveChanges();

            return Ok("Password changed successfully");
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return Ok(
                    "If the email is registered, an OTP has been sent.");
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            Random random = new Random();
            int generatedOTP = random.Next(100000, 1000000);

            if (passwordReset != null)
            {
                passwordReset.OTP = generatedOTP.ToString();
                passwordReset.CreatedAt = DateTime.Now;
                passwordReset.ExpiresAt =
                    DateTime.Now.AddMinutes(5);

                _context.SaveChanges();

                _emailService.SendEmail(
                    user.Email,
                    "CareerPilot AI - Password Reset OTP",
                    $"Your OTP is: {generatedOTP}");
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
                _context.SaveChanges();

                _emailService.SendEmail(
                    user.Email,
                    "CareerPilot AI - Password Reset OTP",
                    $"Your OTP is: {generatedOTP}");
            }

            return Ok(
                "If the email is registered, an OTP has been sent.");
        }

        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp(VerifyOtpDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            if (passwordReset == null)
            {
                return BadRequest("No OTP found for this user");
            }

            if (DateTime.Now > passwordReset.ExpiresAt)
            {
                return BadRequest("OTP has expired");
            }

            if (dto.OTP == passwordReset.OTP)
            {
                return Ok("OTP verified successfully");
            }

            return BadRequest("Invalid OTP");
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            User user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            PasswordResetOTP passwordReset =
                _context.PasswordResetOTPs
                .FirstOrDefault(p => p.UserId == user.UserId);

            if (passwordReset == null)
            {
                return BadRequest("No OTP found for this user");
            }

            if (DateTime.Now > passwordReset.ExpiresAt)
            {
                return BadRequest("OTP has expired");
            }

            if (dto.OTP != passwordReset.OTP)
            {
                return BadRequest("Invalid OTP");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest(
                    "New password and confirm password do not match");
            }

            user.Password = _passwordHasher.HashPassword(
                user,
                dto.NewPassword);

            _context.PasswordResetOTPs.Remove(passwordReset);
            _context.SaveChanges();

            return Ok("Password reset successfully");
        }
    }
}

