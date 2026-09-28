using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace CareerPilot_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly ResumeAnalyzerService _resumeAnalyzerService;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(
            CareerPilotAIDbContext context,
            ResumeAnalyzerService resumeAnalyzerService,
            ILogger<ResumeController> logger)
        {
            _context = context;
            _resumeAnalyzerService = resumeAnalyzerService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("UploadResume")]
        public async Task<IActionResult> UploadResume(
            [FromForm] ResumeUploadDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (dto.ResumeFile == null ||
                dto.ResumeFile.Length == 0)
            {
                return BadRequest("Please upload a resume.");
            }

            if (dto.ResumeFile.Length > 5 * 1024 * 1024)
            {
                return BadRequest(
                    "Resume file size cannot exceed 5 MB.");
            }

            if (Path.GetExtension(dto.ResumeFile.FileName)
                .ToLower() != ".pdf")
            {
                return BadRequest("Only PDF files are allowed.");
            }

            string uniqueFileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(dto.ResumeFile.FileName);

            string uploadsFolder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads");

            string filePath =
                Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

            using (var stream =
                new FileStream(filePath, FileMode.Create))
            {
                dto.ResumeFile.CopyTo(stream);
            }

            Resume resume = new Resume();

            resume.UserId = userId;
            resume.FileName = uniqueFileName;
            resume.FileLocation = filePath;
            resume.UploadedAt = DateTime.Now;

            _context.Resumes.Add(resume);
            _context.SaveChanges();

            StringBuilder resumeText = new StringBuilder();

            using (PdfDocument document =
                   PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    resumeText.AppendLine(
                        ContentOrderTextExtractor.GetText(page));
                }
            }

            ResumeAnalysisResult? result =
                await _resumeAnalyzerService
                    .AnalyzeResume(resumeText.ToString());

            if (result == null)
            {
                return BadRequest("AI analysis failed.");
            }

            ResumeAnalysis analysis =
                new ResumeAnalysis();

            analysis.ResumeId = resume.ResumeId;
            analysis.ATSScore = result.ATSScore;
            analysis.Summary = result.Summary;
            analysis.Skills =
                string.Join(", ", result.Skills);
            analysis.MissingSkills =
                string.Join(", ", result.MissingSkills);
            analysis.Suggestions =
                string.Join(", ", result.Suggestions);
            analysis.AnalyzedAt = DateTime.Now;

            try
            {
                _context.ResumeAnalyses.Add(analysis);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while saving resume analysis.");

                return StatusCode(
                    500,
                    "Something went wrong while processing your request.");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("ResumeHistory")]
        public IActionResult ResumeHistory()
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<Resume> resumes =
                _context.Resumes
                .Where(r => r.UserId == userId)
                .ToList();

            List<ResumeHistoryDTO> history =
                new List<ResumeHistoryDTO>();

            foreach (Resume resume in resumes)
            {
                ResumeAnalysis analysis =
                    _context.ResumeAnalyses
                    .FirstOrDefault(
                        a => a.ResumeId == resume.ResumeId);

                if (analysis == null)
                {
                    continue;
                }

                ResumeHistoryDTO dto =
                    new ResumeHistoryDTO
                    {
                        ResumeId = resume.ResumeId,
                        FileName = resume.FileName,
                        ATSScore = analysis.ATSScore,
                        AnalyzedAt = analysis.AnalyzedAt
                    };

                history.Add(dto);
            }

            if (history.Count == 0)
            {
                return NotFound(
                    "No resume history found.");
            }

            return Ok(history);
        }

        [Authorize]
        [HttpGet("GetResumeAnalysis/{resumeId}")]
        public IActionResult GetResumeAnalysis(int resumeId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ResumeAnalysis analysis =
                _context.ResumeAnalyses
                .FirstOrDefault(
                    a =>
                        a.ResumeId == resumeId &&
                        a.Resume.UserId == userId);

            if (analysis == null)
            {
                return NotFound(
                    "No analysis found for this resume.");
            }

            ResumeAnalysisDTO dto =
                new ResumeAnalysisDTO
                {
                    ResumeId = analysis.ResumeId,
                    ATSScore = analysis.ATSScore,
                    Summary = analysis.Summary,
                    Skills = analysis.Skills,
                    MissingSkills = analysis.MissingSkills,
                    Suggestions = analysis.Suggestions,
                    AnalyzedAt = analysis.AnalyzedAt
                };

            return Ok(dto);
        }

        [Authorize]
        [HttpDelete("DeleteResume/{resumeId}")]
        public IActionResult DeleteResume(int resumeId)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            Resume resume =
                _context.Resumes
                .FirstOrDefault(
                    r =>
                        r.ResumeId == resumeId &&
                        r.UserId == userId);

            if (resume == null)
            {
                return NotFound("Resume not found.");
            }

            ResumeAnalysis analysis =
                _context.ResumeAnalyses
                .FirstOrDefault(
                    a => a.ResumeId == resumeId);

            if (analysis != null)
            {
                _context.ResumeAnalyses.Remove(analysis);
            }

            _context.Resumes.Remove(resume);
            _context.SaveChanges();

            if (System.IO.File.Exists(resume.FileLocation))
            {
                System.IO.File.Delete(
                    resume.FileLocation);
            }

            return Ok(
                "Resume and its analysis deleted successfully.");
        }
    }
}

