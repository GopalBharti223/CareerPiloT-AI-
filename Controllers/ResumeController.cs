using CareerPilot_AI.Data;
using System.Threading.Tasks;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using CareerPilot_AI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

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

        public ResumeController(CareerPilotAIDbContext context, ResumeAnalyzerService resumeAnalyzerService, ILogger<ResumeController> logger)
        {
            _context = context;
            _resumeAnalyzerService = resumeAnalyzerService;
            _logger = logger;
        }


        [Authorize]
        [HttpPost("UploadResume")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (dto.ResumeFile == null || dto.ResumeFile.Length == 0)
            {
                return BadRequest("Please upload a resume.");
            }

            if (dto.ResumeFile.Length > 5 * 1024 * 1024)
            {
                return BadRequest("Resume file size cannot exceed 5 MB.");
            }


            if (Path.GetExtension(dto.ResumeFile.FileName).ToLower() != ".pdf")
            {
                return BadRequest("Only PDF files are allowed.");
            }


            string uniqueFileName = Guid.NewGuid().ToString() +
                        Path.GetExtension(dto.ResumeFile.FileName);

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
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

            // Extract text from the uploaded PDF
            // Read PDF and extract text
            StringBuilder resumeText = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(filePath))
            {
                foreach (Page page in document.GetPages())
                {
                    resumeText.AppendLine(ContentOrderTextExtractor.GetText(page));
                }
            }

            ResumeAnalysisResult? result =
            await _resumeAnalyzerService.AnalyzeResume(resumeText.ToString());

            if (result == null)
            {
                return BadRequest("AI analysis failed.");
            }
            
            
            ResumeAnalysis analysis = new ResumeAnalysis();
            analysis.ResumeId = resume.ResumeId;
            analysis.ATSScore= result.ATSScore;
            analysis.Summary = result.Summary;
            analysis.Skills = string.Join(", ", result.Skills);
            analysis.MissingSkills = string.Join(", ", result.MissingSkills);
            analysis.Suggestions = string.Join(", ", result.Suggestions);
            analysis.AnalyzedAt = DateTime.Now;
          //  analysis.AnalysisId = 0; // Ensure the primary key is set to 0 for auto-increment


            try
            {

                _context.ResumeAnalyses.Add(analysis);
                _context.SaveChanges();

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving resume analysis.");
                return StatusCode(500,
                    "Something went wrong while processing your request.");
            }




            return Ok(result);
        }

        [Authorize]
        [HttpGet("ResumeHistory")]
        public IActionResult ResumeHistory()
        {
            //To fetch the userId from the JWT token
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);


            // Fetch all resumes for the user whose userId is fetched from the JWT token 
            //Store in the list of Resume objects
            // Suppose UserId = 1, having 3 resumes,resume objected store all threee resumes
            //List is used to store multiple resumes for the user
            //ToList() is used to convert the result of the query to a list of Resume objects
            //Resume is List name
            //resumes is object of List<Resume> which stores all the resumes for the user
            List<Resume> resumes = _context.Resumes
                .Where(r => r.UserId == userId)
                .ToList();


            //It is empty list of ResumeHistoryDTO objects to store the history of resumes for the user
            List<ResumeHistoryDTO> history = new List<ResumeHistoryDTO>();



            foreach (Resume resume in resumes)
            {

                // Fetch the analysis for each resume
                //a=>r.ResumeId == resume.ResumeId is a lambda expression that checks if the ResumeId of the analysis matches the ResumeId of the resume
                //analysis is an object of ResumeAnalysis that stores the analysis for the resume
                //ResumeAnalysis is a class that represents the analysis of a resume
                //FirstOrDefault() is used to fetch the first analysis that matches the condition or null if no analysis is found
                
                ResumeAnalysis analysis = _context.ResumeAnalyses.FirstOrDefault(a => a.ResumeId == resume.ResumeId);

                if (analysis == null)
                {
                    continue;
                }

                ResumeHistoryDTO dto = new ResumeHistoryDTO
                {
                    ResumeId = resume.ResumeId,
                    FileName = resume.FileName,
                    ATSScore = analysis.ATSScore,
                    AnalyzedAt = analysis.AnalyzedAt
                };

                // Add the DTO to the history list
                //Add() method 
                history.Add(dto);
            }

            if (history.Count == 0)
            {
                return NotFound("No resume history found.");
            }

            return Ok(history);
        }

        [Authorize]
        [HttpGet("GetResumeAnalysis/{resumeId}")]

        public IActionResult GetResumeAnalysis(int resumeId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            ResumeAnalysis analysis = _context.ResumeAnalyses
           .FirstOrDefault(a =>
               a.ResumeId == resumeId &&
               a.Resume.UserId == userId);

            

            if (analysis == null)
            {
                return NotFound("No analysis found for this resume.");
            }

            ResumeAnalysisDTO dto = new ResumeAnalysisDTO
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
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Resume resume = _context.Resumes.FirstOrDefault(r => r.ResumeId == resumeId && r.UserId == userId);
            if (resume == null)
            {
                return NotFound("Resume not found.");
            }
            // Delete the associated analysis first
            ResumeAnalysis analysis = _context.ResumeAnalyses.FirstOrDefault(a => a.ResumeId == resumeId);
            if (analysis != null)
            {
                _context.ResumeAnalyses.Remove(analysis);
            }
            // Delete the resume
            _context.Resumes.Remove(resume);
            _context.SaveChanges();
            // Optionally, delete the file from the server
            if (System.IO.File.Exists(resume.FileLocation))
            {
                System.IO.File.Delete(resume.FileLocation);
            }
            return Ok("Resume and its analysis deleted successfully.");
        }



    }
}
