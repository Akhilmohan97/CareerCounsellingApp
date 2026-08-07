using System.Text.Json.Serialization;

namespace CareerCounsellingApp.Services.AI.Models
{
    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; } = new();

        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; } = string.Empty;

        [JsonPropertyName("index")]
        public int Index { get; set; }
    }
}
