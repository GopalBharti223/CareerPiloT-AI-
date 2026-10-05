using CareerPilot_AI.Data;
using CareerPilot_AI.DTOs;
using CareerPilot_AI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace CareerPilot_AI.Services
{
    public class ResumeService
    {
        private readonly CareerPilotAIDbContext _context;
        private readonly ResumeAnalyzerService _resumeAnalyzerService;
        private readonly ILogger<ResumeService> _logger;

        public ResumeService(
            CareerPilotAIDbContext context,
            ResumeAnalyzerService resumeAnalyzerService,
            ILogger<ResumeService> logger)
        {
            _context = context;
            _resumeAnalyzerService = resumeAnalyzerService;
            _logger = logger;
        }

        public async Task<ResumeAnalysisResult?> UploadResume(
    int userId,
    ResumeUploadDTO dto)
        {
            if (dto.ResumeFile == null ||
                dto.ResumeFile.Length == 0)
            {
                return null;
            }

            if (dto.ResumeFile.Length > 5 * 1024 * 1024)
            {
                return null;
            }

            if (Path.GetExtension(dto.ResumeFile.FileName)
                .ToLower() != ".pdf")
            {
                return null;
            }

            using (var stream = dto.ResumeFile.OpenReadStream())
            {
                byte[] header = new byte[5];

                await stream.ReadAsync(header, 0, 5);

                string fileSignature =
                    Encoding.ASCII.GetString(header);

                if (fileSignature != "%PDF-")
                {
                    return null;
                }
            }

            string uniqueFileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(dto.ResumeFile.FileName);

            string uploadsFolder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath =
                Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

            try
            {
                using (var stream =
                    new FileStream(filePath, FileMode.Create))
                {
                    await dto.ResumeFile.CopyToAsync(stream);
                }

                Resume resume = new Resume();

                resume.UserId = userId;
                resume.FileName = dto.ResumeFile.FileName;
                resume.FileLocation = filePath;
                resume.UploadedAt = DateTime.UtcNow;

                _context.Resumes.Add(resume);

                await _context.SaveChangesAsync();

                StringBuilder resumeText =
                    new StringBuilder();

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
                        .AnalyzeResume(
                            resumeText.ToString());

                if (result == null)
                {
                    return null;
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
                analysis.AnalyzedAt = DateTime.UtcNow;

                _context.ResumeAnalyses.Add(analysis);

                await _context.SaveChangesAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while uploading or analyzing resume.");

                return null;
            }
        }

        public List<ResumeHistoryDTO> ResumeHistory(
            int userId)
        {
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

            return history;
        }

        public ResumeAnalysisDTO GetResumeAnalysis(int resumeId, int userId)
        {
            ResumeAnalysis analysis =
                _context.ResumeAnalyses
                .FirstOrDefault(
                    a =>
                        a.ResumeId == resumeId &&
                        a.Resume.UserId == userId);

            if (analysis == null)
            {
                return null;
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

            return dto;
        }

        public string DeleteResume(int resumeId,int userId)
        {
            Resume resume =
                _context.Resumes
                .FirstOrDefault(
                    r =>
                        r.ResumeId == resumeId &&
                        r.UserId == userId);

            if (resume == null)
            {
                return "Resume not found.";
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

            if (System.IO.File.Exists(
                resume.FileLocation))
            {
                System.IO.File.Delete(
                    resume.FileLocation);
            }

            return "Resume and its analysis deleted successfully.";
        }
    }
}