using CareerPilot_AI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CareerPilot_AI.Services
{
    public class ResumeAnalyzerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ResumeAnalyzerService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ResumeAnalysisResult?> AnalyzeResume(string resumeText)
        {
            try
            {
                string apiKey = _configuration["Groq:ApiKey"];
                string model = _configuration["Groq:Model"];

                string url = "https://api.groq.com/openai/v1/chat/completions";

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = $@"
                            You are an expert ATS Resume Analyzer.

                            Analyze the following resume.

                            Return ONLY valid JSON.

                            Do NOT write explanations.
                            Do NOT use markdown.
                            Do NOT wrap JSON inside ```.

                            Return EXACTLY this format:

                            {{
                              ""ATSScore"": 0,
                              ""Summary"": """",
                              ""Skills"": [],
                              ""MissingSkills"": [],
                              ""Suggestions"": []
                            }}

                            Resume:

                            {resumeText}
                            "
                        }
                    },
                    temperature = 0.2
                };

                string json = JsonSerializer.Serialize(requestBody);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                _httpClient.DefaultRequestHeaders.Clear();

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                HttpResponseMessage response =
                    await _httpClient.PostAsync(url, content);

                string responseBody =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(responseBody);
                }

                using JsonDocument document = JsonDocument.Parse(responseBody);

                string aiJson =
                    document.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                ResumeAnalysisResult? result =
                    JsonSerializer.Deserialize<ResumeAnalysisResult>(aiJson, options);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}