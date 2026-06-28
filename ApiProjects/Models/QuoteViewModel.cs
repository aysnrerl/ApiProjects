using System.Text.Json.Serialization;

namespace ApiProjects.Models
{
    public class QuoteViewModel
    {
        [JsonPropertyName("q")]
        public string text { get; set; }

        [JsonPropertyName("a")]
        public string author { get; set; }
    }
}