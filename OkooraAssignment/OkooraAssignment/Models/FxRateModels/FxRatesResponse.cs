using System.Text.Json.Serialization;

namespace OkooraAssignment.Models.FxRateModels
{
    public class FxRatesResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("terms")]
        public string? Terms { get; set; }

        [JsonPropertyName("privacy")]
        public string? Privacy { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("base")]
        public string? BaseCurrency { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }

        [JsonPropertyName("erroe")]
        public ErrorDetails? Error { get; set; } // Add error object for handling API errors

        public class ErrorDetails
        {
            public string? Code { get; set; }
            public int Status { get; set; }
            public string? Message { get; set; }
            public string? Description { get; set; }
            public string? Link { get; set; }
            public string? DocsLink { get; set; }
            public string? SupportEmail { get; set; }
        }
    }
}
