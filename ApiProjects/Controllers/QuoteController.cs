using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuoteController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("QuoteDetailCache", out List<QuoteViewModel>? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                using var response = await client.GetAsync("https://zenquotes.io/api/quotes");
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var quotes = JsonSerializer.Deserialize<List<QuoteViewModel>>(body, options);

                    if (quotes != null)
                    {
                        _memoryCache.Set("QuoteDetailCache", quotes, TimeSpan.FromHours(6));
                    }

                    return View(quotes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Söz hatası: " + ex.Message);
            }

            return View(new List<QuoteViewModel>());
        }
    }
}