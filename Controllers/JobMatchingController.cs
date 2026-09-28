using CareerPilot_AI.DTOs;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobMatchingController : ControllerBase
    {
        private readonly JobMatchingService _jobMatchingService;

        public JobMatchingController(JobMatchingService jobMatchingService)
        {
            _jobMatchingService = jobMatchingService;
        }

        [HttpPost("MatchJob")]
        public async Task<IActionResult> MatchJob(JobMatchingDTO dto)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var result =
                await _jobMatchingService.MatchJob(userId, dto);

            if (result == null)
                return BadRequest("Unable to match resume with job description.");

            return Ok(result);
        }


        [HttpGet("History")]
        public async Task<IActionResult> GetHistory()
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var history =
                await _jobMatchingService.GetJobMatchingHistory(userId);

            return Ok(history);
        }

    }
}