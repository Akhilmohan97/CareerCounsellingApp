using CareerCounsellingApp.DTO;
using CareerCounsellingApp.Services.AI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services.AI
{
    public class GeminiAIService : IAIInterpretationService
    {
        private readonly GeminiPromptBuilder _promptBuilder;
        private readonly GeminiSettings _settings;
        private readonly HttpClient _httpClient;
        public GeminiAIService(GeminiPromptBuilder promptBuilder,
        GeminiSettings settings)
        {
            _promptBuilder = promptBuilder;
            _settings = settings;
            _httpClient = new HttpClient();
        }
        public async Task<AIInterpretationDto> GenerateAsync(AssessmentReportDto report)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
            var prompt = _promptBuilder.Build(report);
            var request = new
            {
                contents = new[]
                                {
                                    new
                                    {
                                        parts = new[]
                                        {
                                            new
                                            {
                                                text = prompt
                                            }
                                        }
                                    }
                                }
            };
            var response = await _httpClient.PostAsJsonAsync(endpoint,request);
            var responseJson = await response.Content.ReadAsStringAsync();

            var geminiResponse =
                JsonSerializer.Deserialize<GeminiResponse>(responseJson);
            var aiJson =
    geminiResponse!
        .Candidates[0]
        .Content
        .Parts[0]
        .Text;
            var interpretation =
    JsonSerializer.Deserialize<AIInterpretationDto>(aiJson);
            return interpretation;
        }
    }
}
