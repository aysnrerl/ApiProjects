using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class FuelController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FuelController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://gas-price.p.rapidapi.com/europeanCountries"),
                Headers =
                {
                    { "x-rapidapi-key", _configuration["ApiKeys:RapidApiKey"] ?? "" },
                    { "x-rapidapi-host", "gas-price.p.rapidapi.com" },
                },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var fuelData = JsonSerializer.Deserialize<FuelViewModel>(body, options);
                        if (fuelData != null && fuelData.result != null && fuelData.result.Count > 0)
                        {
                            return View(fuelData);
                        }
                    }
                    throw new Exception("API request was not successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Akaryakıt detay API hatası: " + ex.Message);
                var fallbackFuel = new FuelViewModel
                {
                    success = true,
                    result = new List<FuelCountry>
                    {
                        new FuelCountry { country = "Turkey", gasoline = "1.24", diesel = "1.22", lpg = "0.72", currency = "EUR" },
                        new FuelCountry { country = "Germany", gasoline = "1.82", diesel = "1.69", lpg = "0.98", currency = "EUR" },
                        new FuelCountry { country = "France", gasoline = "1.89", diesel = "1.74", lpg = "1.02", currency = "EUR" },
                        new FuelCountry { country = "Italy", gasoline = "1.91", diesel = "1.78", lpg = "1.05", currency = "EUR" }
                    }
                };
                return View(fallbackFuel);
            }
        }
    }
}