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
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var fuelData = JsonSerializer.Deserialize<FuelViewModel>(body, options);

                    return View(fuelData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Akaryakıt detay API hatası: " + ex.Message);
                return View(new FuelViewModel());
            }
        }
    }
}