using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ApiProjects.Controllers
{
    public class FootballMatchController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FootballMatchController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
                RequestUri = new Uri("https://super-lig-standings.p.rapidapi.com/"),
                Headers =
                {
                    { "x-rapidapi-key", _configuration["ApiKeys:RapidApiKey"] ?? "" },
                    { "x-rapidapi-host", "super-lig-standings.p.rapidapi.com" },
                },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var footballData = JsonSerializer.Deserialize<List<FootballMatchViewModel>>(body, options);

                    return View(footballData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Futbol detay API hatası: " + ex.Message);
                return View(new List<FootballMatchViewModel>());
            }
        }
    }
}