using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CareerCounsellingApp.DTO
{
    public class AIInterpretationDto
    {
        [JsonPropertyName("executiveSummary")]
        public string ExecutiveSummary { get; set; } = string.Empty;

        [JsonPropertyName("strengths")]
        public List<string> Strengths { get; set; } = new();

        [JsonPropertyName("developmentAreas")]
        public List<string> DevelopmentAreas { get; set; } = new();

        [JsonPropertyName("discussionPoints")]
        public List<string> DiscussionPoints { get; set; } = new();
    }
}
