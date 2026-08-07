using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CareerCounsellingApp.Services.AI.Models;

public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; } = new();

    [JsonPropertyName("usageMetadata")]
    public UsageMetadata? UsageMetadata { get; set; }

    [JsonPropertyName("modelVersion")]
    public string? ModelVersion { get; set; }

    [JsonPropertyName("responseId")]
    public string? ResponseId { get; set; }
}

public class UsageMetadata
{
    [JsonPropertyName("promptTokenCount")]
    public int PromptTokenCount { get; set; }

    [JsonPropertyName("candidatesTokenCount")]
    public int CandidatesTokenCount { get; set; }

    [JsonPropertyName("totalTokenCount")]
    public int TotalTokenCount { get; set; }
}
