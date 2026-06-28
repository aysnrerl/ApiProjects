using ApiProjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApiProjects.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DefaultViewModel();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = null
            };

            if (_memoryCache.TryGetValue("WeatherCache", out WeatherRoot? weather))
            {
                dashboardData.WeatherInfo = weather!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var url = "https://api.open-meteo.com/v1/forecast" +
                              "?latitude=41.0082&longitude=28.9784" +
                              "&current=temperature_2m,apparent_temperature,weathercode,windspeed_10m,relativehumidity_2m" +
                              "&daily=temperature_2m_max,temperature_2m_min,weathercode" +
                              "&timezone=Europe/Istanbul&forecast_days=3";

                    using var weatherResponse = await client.GetAsync(url);
                    if (weatherResponse.IsSuccessStatusCode)
                    {
                        var body = await weatherResponse.Content.ReadAsStringAsync();
                        var weatherData = JsonSerializer.Deserialize<WeatherRoot>(body, jsonOptions);
                        if (weatherData != null)
                        {
                            dashboardData.WeatherInfo = weatherData;
                            _memoryCache.Set("WeatherCache", weatherData, TimeSpan.FromMinutes(20));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hava durumu hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("CurrencyCache", out CurrencyViewModel? currency))
            {
                dashboardData.CurrencyInfo = currency!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    using var currencyResponse = await client.GetAsync("https://open.er-api.com/v6/latest/USD");
                    if (currencyResponse.IsSuccessStatusCode)
                    {
                        var body = await currencyResponse.Content.ReadAsStringAsync();
                        var currencyData = JsonSerializer.Deserialize<CurrencyViewModel>(body, jsonOptions);
                        if (currencyData != null)
                        {
                            dashboardData.CurrencyInfo = currencyData;
                            _memoryCache.Set("CurrencyCache", currencyData, TimeSpan.FromMinutes(30));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Döviz hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("CryptoCache", out CryptoViewModel? crypto))
            {
                dashboardData.CryptoInfo = crypto!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("User-Agent", "ApiProjects-CryptoApp/1.0");
                    using var cryptoResponse = await client.GetAsync("https://api.coinlore.net/api/tickers/?start=0&limit=20");
                    if (cryptoResponse.IsSuccessStatusCode)
                    {
                        var body = await cryptoResponse.Content.ReadAsStringAsync();
                        var coinLoreData = JsonSerializer.Deserialize<CoinLoreResponse>(body, jsonOptions);

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

                        var cryptoData = new CryptoViewModel { coins = coins };
                        dashboardData.CryptoInfo = cryptoData;
                        _memoryCache.Set("CryptoCache", cryptoData, TimeSpan.FromMinutes(10));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Kripto hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("FuelCache", out FuelViewModel? fuel))
            {
                dashboardData.FuelInfo = fuel!;
            }
            else
            {
                try
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

                    using var fuelResponse = await client.SendAsync(request);
                    if (fuelResponse.IsSuccessStatusCode)
                    {
                        var body = await fuelResponse.Content.ReadAsStringAsync();
                        var fuelData = JsonSerializer.Deserialize<FuelViewModel>(body, jsonOptions);
                        if (fuelData != null)
                        {
                            dashboardData.FuelInfo = fuelData;
                            _memoryCache.Set("FuelCache", fuelData, TimeSpan.FromMinutes(30));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Akaryakıt hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("FootballCache", out List<FootballMatchViewModel>? footballmatch))
            {
                dashboardData.FootballInfo = footballmatch!;
            }
            else
            {
                try
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

                    using var footballResponse = await client.SendAsync(request);
                    if (footballResponse.IsSuccessStatusCode)
                    {
                        var body = await footballResponse.Content.ReadAsStringAsync();
                        var footballData = JsonSerializer.Deserialize<List<FootballMatchViewModel>>(body, jsonOptions);
                        if (footballData != null)
                        {
                            dashboardData.FootballInfo = footballData;
                            _memoryCache.Set("FootballCache", footballData, TimeSpan.FromMinutes(30));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Futbol hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("MusicDetailCache", out MusicViewModel? music))
            {
                dashboardData.MusicInfo = music!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    using var musicResponse = await client.GetAsync("https://api.deezer.com/chart/0/tracks?limit=10");
                    if (musicResponse.IsSuccessStatusCode)
                    {
                        var body = await musicResponse.Content.ReadAsStringAsync();
                        var musicData = JsonSerializer.Deserialize<MusicViewModel>(body, jsonOptions);
                        if (musicData != null)
                        {
                            dashboardData.MusicInfo = musicData;
                            _memoryCache.Set("MusicDetailCache", musicData, TimeSpan.FromMinutes(30));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Müzik hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("MovieCache", out MovieViewModel? movie))
            {
                dashboardData.MovieInfo = movie!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri("https://api.themoviedb.org/3/trending/movie/day?language=tr-TR"),
                    };
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        _configuration["ApiKeys:TmdbBearerToken"] ?? "");

                    using var movieResponse = await client.SendAsync(request);
                    if (movieResponse.IsSuccessStatusCode)
                    {
                        var body = await movieResponse.Content.ReadAsStringAsync();
                        var movieData = JsonSerializer.Deserialize<MovieViewModel>(body, jsonOptions);
                        if (movieData != null)
                        {
                            dashboardData.MovieInfo = movieData;
                            _memoryCache.Set("MovieCache", movieData, TimeSpan.FromMinutes(30));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Film hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("NewsDetailCache", out List<NewsItemViewModel>? news))
            {
                dashboardData.NewsInfo = news!;
            }
            else
            {
                try
                {
                    var rssSources = new List<(string Url, string Name)>
                    {
                        ("https://www.hurriyet.com.tr/rss/anasayfa", "Hürriyet"),
                        ("https://www.sabah.com.tr/rss/anasayfa.xml", "Sabah"),
                        ("https://www.ntv.com.tr/gundem.rss", "NTV"),
                    };

                    var newsList = new List<NewsItemViewModel>();
                    var newsClient = _httpClientFactory.CreateClient();
                    newsClient.Timeout = TimeSpan.FromSeconds(8);

                    foreach (var (feedUrl, sourceName) in rssSources)
                    {
                        if (newsList.Count >= 6) break;
                        try
                        {
                            var rssResponse = await newsClient.GetAsync(feedUrl);
                            if (!rssResponse.IsSuccessStatusCode) continue;

                            var xml = await rssResponse.Content.ReadAsStringAsync();
                            var doc = XDocument.Parse(xml);

                            var items = doc.Descendants("item")
                                .Take(3)
                                .Select(item =>
                                {
                                    var linkEl = item.Element("link");
                                    var link = linkEl?.Value?.Trim();
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        link = (string?)item.Nodes()
                                            .OfType<XText>()
                                            .FirstOrDefault(n => n.Value.StartsWith("http"))?.Value?.Trim();
                                    }
                                    return new NewsItemViewModel
                                    {
                                        title = item.Element("title")?.Value?.Trim() ?? "",
                                        url = link ?? feedUrl,
                                        source = sourceName,
                                        pubDate = item.Element("pubDate")?.Value?.Trim() ?? ""
                                    };
                                })
                                .Where(n => !string.IsNullOrEmpty(n.title))
                                .ToList();

                            newsList.AddRange(items);
                        }
                        catch (Exception rssEx)
                        {
                            Console.WriteLine($"RSS hatası ({sourceName}): {rssEx.Message}");
                        }
                    }

                    if (newsList.Count > 0)
                    {
                        dashboardData.NewsInfo = newsList;
                        _memoryCache.Set("NewsDetailCache", newsList, TimeSpan.FromMinutes(20));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Haberler hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("RecipeCache", out RecipeViewModel? recipe))
            {
                dashboardData.RecipeInfo = recipe!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    using var recipeResponse = await client.GetAsync("https://www.themealdb.com/api/json/v1/1/search.php?s=");
                    if (recipeResponse.IsSuccessStatusCode)
                    {
                        var body = await recipeResponse.Content.ReadAsStringAsync();
                        var recipeData = JsonSerializer.Deserialize<RecipeViewModel>(body, jsonOptions);
                        if (recipeData != null)
                        {
                            dashboardData.RecipeInfo = recipeData;
                            _memoryCache.Set("RecipeCache", recipeData, TimeSpan.FromHours(2));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Tarif hatası: " + ex.Message);
                }
            }

            if (_memoryCache.TryGetValue("QuoteDetailCache", out List<QuoteViewModel>? quotes))
            {
                dashboardData.QuoteInfo = quotes!;
            }
            else
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    using var quoteResponse = await client.GetAsync("https://zenquotes.io/api/quotes");
                    if (quoteResponse.IsSuccessStatusCode)
                    {
                        var body = await quoteResponse.Content.ReadAsStringAsync();
                        var quoteData = JsonSerializer.Deserialize<List<QuoteViewModel>>(body, jsonOptions);
                        if (quoteData != null)
                        {
                            dashboardData.QuoteInfo = quoteData;
                            _memoryCache.Set("QuoteDetailCache", quoteData, TimeSpan.FromHours(6));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Söz hatası: " + ex.Message);
                }
            }

            return View(dashboardData);
        }
    }
}