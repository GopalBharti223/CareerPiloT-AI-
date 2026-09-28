using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly CareerPilotAIDbContext _context;

        public ProfileController(CareerPilotAIDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            ProfileResponseDTO userDto = new ProfileResponseDTO();

            userDto.UserName = user.UserName;
            userDto.Email = user.Email;
            userDto.MobileNumber = user.MobileNumber;
            userDto.CreatedAt = user.CreatedAt;

            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(UpdateProfileDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User user = _context.Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var existingUser = _context.Users.FirstOrDefault(u =>
                u.Email == dto.Email &&
                u.UserId != userId);

            if (existingUser != null)
            {
                return BadRequest("Email already registered");
            }

            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.MobileNumber = dto.MobileNumber;

            _context.SaveChanges();

            return Ok("Profile updated successfully");
        }
    }
}

