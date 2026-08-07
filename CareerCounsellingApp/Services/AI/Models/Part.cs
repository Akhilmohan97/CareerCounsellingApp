using System.Text.Json.Serialization;

namespace CareerCounsellingApp.Services.AI.Models
{
    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
