using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("WeatherDetailCache", out WeatherRoot? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();
            var url = "https://api.open-meteo.com/v1/forecast" +
                      "?latitude=41.0082&longitude=28.9784" +
                      "&current=temperature_2m,apparent_temperature,weathercode,windspeed_10m,relativehumidity_2m" +
                      "&daily=temperature_2m_max,temperature_2m_min,weathercode" +
                      "&timezone=Europe/Istanbul&forecast_days=3";

            try
            {
                using var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<WeatherRoot>(body, options);

                    if (data != null)
                    {
                        _memoryCache.Set("WeatherDetailCache", data, TimeSpan.FromMinutes(30));
                    }

                    return View(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hava durumu hatası: " + ex.Message);
            }

            return View(new WeatherRoot());
        }

        public static string GetWeatherEmoji(int code)
        {
            if (code == 0) return "☀️";
            if (code <= 3) return "⛅";
            if (code <= 48) return "🌫️";
            if (code <= 67) return "🌧️";
            if (code <= 77) return "🌨️";
            if (code <= 82) return "🌦️";
            return "⛈️";
        }

        public static string GetWeatherDescription(int code)
        {
            if (code == 0) return "Açık";
            if (code == 1) return "Çoğunlukla açık";
            if (code == 2) return "Parçalı bulutlu";
            if (code == 3) return "Bulutlu";
            if (code <= 48) return "Sisli";
            if (code <= 55) return "Çiseleyen yağmur";
            if (code <= 67) return "Yağmurlu";
            if (code <= 77) return "Karlı";
            if (code <= 82) return "Sağanak";
            return "Fırtınalı";
        }
    }
}