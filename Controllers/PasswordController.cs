using CareerPilot_AI.DTOs;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly PasswordService _passwordService;

        public PasswordController(PasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string result = _passwordService.ChangePassword(
                userId,
                dto);

            if (result == "User not found")
            {
                return NotFound(result);
            }

            if (result != "Password changed successfully")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordDTO dto)
        {
            string result = _passwordService.ForgotPassword(dto);

            return Ok(result);
        }

        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp(VerifyOtpDTO dto)
        {
            string result = _passwordService.VerifyOtp(dto);

            if (result == "OTP verified successfully")
            {
                return Ok(result);
            }

            if (result == "User not found" ||
                result == "No OTP found for this user" ||
                result == "OTP has expired" ||
                result == "Invalid OTP")
            {
                return BadRequest(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            string result = _passwordService.ResetPassword(dto);

            if (result == "Password reset successfully")
            {
                return Ok(result);
            }

            if (result == "User not found" ||
                result == "No OTP found for this user" ||
                result == "OTP has expired" ||
                result == "Invalid OTP" ||
                result == "New password and confirm password do not match")
            {
                return BadRequest(result);
            }

            return BadRequest(result);
        }
    }
}