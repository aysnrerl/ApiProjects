using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("CurrencyCache", out CurrencyViewModel? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                using var response = await client.GetAsync("https://open.er-api.com/v6/latest/USD");
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<CurrencyViewModel>(body, options);

                    if (data != null)
                    {
                        _memoryCache.Set("CurrencyCache", data, TimeSpan.FromMinutes(30));
                    }

                    return View(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Döviz hatası: " + ex.Message);
            }

            return View(new CurrencyViewModel());
        }
    }
}