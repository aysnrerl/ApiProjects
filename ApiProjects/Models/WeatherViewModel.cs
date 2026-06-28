using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiProjects.Models
{
    public class WeatherRoot
    {
        [JsonPropertyName("current")]
        public OpenMeteoCurrentWeather current { get; set; }

        [JsonPropertyName("daily")]
        public OpenMeteoDailyWeather daily { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string timezone_abbreviation { get; set; }
    }

    public class OpenMeteoCurrentWeather
    {
        [JsonPropertyName("temperature_2m")]
        public double temperature_2m { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public double apparent_temperature { get; set; }

        [JsonPropertyName("weathercode")]
        public int weathercode { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public double windspeed_10m { get; set; }

        [JsonPropertyName("relativehumidity_2m")]
        public int relativehumidity_2m { get; set; }
    }

    public class OpenMeteoDailyWeather
    {
        [JsonPropertyName("time")]
        public List<string> time { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double> temperature_2m_max { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double> temperature_2m_min { get; set; }

        [JsonPropertyName("weathercode")]
        public List<int> weathercode { get; set; }
    }
}