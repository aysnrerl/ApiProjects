using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class MusicController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public MusicController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("MusicDetailCache", out MusicViewModel? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.deezer.com/chart/0/tracks?limit=10"),
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var musicData = JsonSerializer.Deserialize<MusicViewModel>(body, options);

                    if (musicData != null)
                    {
                        _memoryCache.Set("MusicDetailCache", musicData, TimeSpan.FromMinutes(30));
                    }

                    return View(musicData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Müzik detay API hatası (Deezer): " + ex.Message);
                return View(new MusicViewModel());
            }
        }
    }
}