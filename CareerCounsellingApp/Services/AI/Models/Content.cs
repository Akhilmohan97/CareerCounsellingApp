using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CareerCounsellingApp.Services.AI.Models
{
    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; } = new();

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
    }
}
