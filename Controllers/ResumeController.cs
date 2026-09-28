using CareerPilot_AI.DTOs;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly ResumeService _resumeService;

        public ResumeController(ResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [Authorize]
        [HttpPost("UploadResume")]
        public async Task<IActionResult> UploadResume(
            [FromForm] ResumeUploadDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _resumeService.UploadResume(
                userId,
                dto);

            if (result == null)
            {
                return BadRequest(
                    "Resume upload or AI analysis failed.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("ResumeHistory")]
        public IActionResult ResumeHistory()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var history =
                _resumeService.ResumeHistory(userId);

            if (history.Count == 0)
            {
                return NotFound(
                    "No resume history found.");
            }

            return Ok(history);
        }

        [Authorize]
        [HttpGet("GetResumeAnalysis/{resumeId}")]
        public IActionResult GetResumeAnalysis(
            int resumeId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var analysis =
                _resumeService.GetResumeAnalysis(
                    resumeId,
                    userId);

            if (analysis == null)
            {
                return NotFound(
                    "No analysis found for this resume.");
            }

            return Ok(analysis);
        }

        [Authorize]
        [HttpDelete("DeleteResume/{resumeId}")]
        public IActionResult DeleteResume(
            int resumeId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string result =
                _resumeService.DeleteResume(
                    resumeId,
                    userId);

            if (result == "Resume not found.")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}