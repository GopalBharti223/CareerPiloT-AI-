using CareerPilot_AI.DTOs;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize]
        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userDto = _profileService.GetProfile(userId);

            if (userDto == null)
            {
                return NotFound("User not found");
            }

            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(UpdateProfileDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string result = _profileService.UpdateProfile(
                userId,
                dto);

            if (result == "User not found")
            {
                return NotFound(result);
            }

            if (result == "Email already registered")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}