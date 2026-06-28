using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("RecipeDetailCache", out RecipeViewModel? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();

            try
            {
                using var response = await client.GetAsync("https://www.themealdb.com/api/json/v1/1/search.php?s=");
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<RecipeViewModel>(body, options);

                    if (data != null)
                    {
                        _memoryCache.Set("RecipeDetailCache", data, TimeSpan.FromHours(2));
                    }

                    return View(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Tarif hatası: " + ex.Message);
            }

            return View(new RecipeViewModel());
        }
    }
}