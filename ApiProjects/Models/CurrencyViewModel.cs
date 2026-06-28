using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiProjects.Models
{
    public class CurrencyViewModel
    {
        [JsonPropertyName("base_code")]
        public string base_code { get; set; }

        [JsonPropertyName("time_last_update_utc")]
        public string time_last_update_utc { get; set; }

        [JsonPropertyName("rates")]
        public Dictionary<string, double> rates { get; set; }
    }
}