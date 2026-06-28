using ApiProjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiProjects.Controllers
{
    public class CryptoController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CryptoController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("CryptoCache", out CryptoViewModel? cached))
            {
                return View(cached);
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "ApiProjects-CryptoApp/1.0");

            try
            {
                using var response = await client.GetAsync("https://api.coinlore.net/api/tickers/?start=0&limit=20");
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var coinLoreData = JsonSerializer.Deserialize<CoinLoreResponse>(body, options);

                    var coins = new List<CoinGeckoCoin>();
                    if (coinLoreData?.Data != null)
                    {
                        foreach (var c in coinLoreData.Data)
                        {
                            double.TryParse(c.PriceUsd, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double price);
                            double.TryParse(c.PercentChange24h, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double change);
                            long.TryParse(c.MarketCapUsd, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out long cap);

                            coins.Add(new CoinGeckoCoin
                            {
                                id = c.Id,
                                symbol = c.Symbol,
                                name = c.Name,
                                market_cap_rank = c.Rank,
                                current_price = price,
                                price_change_percentage_24h = change,
                                market_cap = cap,
                                image = $"https://c1.coinlore.com/img/25x25/{c.Name.ToLower().Replace(" ", "-")}.png"
                            });
                        }
                    }

                    var model = new CryptoViewModel { coins = coins };
                    _memoryCache.Set("CryptoCache", model, TimeSpan.FromMinutes(10));
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kripto hatası: " + ex.Message);
            }

            return View(new CryptoViewModel { coins = new List<CoinGeckoCoin>() });
        }
    }
}