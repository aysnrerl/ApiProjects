using Microsoft.AspNetCore.Mvc;
using ApiProjects.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace ApiProjects.Controllers
{
    public class NewsItemController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        // Türkçe haber kaynakları — RSS, tamamen ücretsiz, API key gerektirmez
        private static readonly List<(string Url, string Name)> RssSources = new()
        {
            ("https://www.hurriyet.com.tr/rss/anasayfa", "Hürriyet"),
            ("https://www.sabah.com.tr/rss/anasayfa.xml", "Sabah"),
            ("https://www.ntv.com.tr/gundem.rss", "NTV"),
        };

        public NewsItemController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            if (_memoryCache.TryGetValue("NewsDetailCache", out List<NewsItemViewModel>? cached))
            {
                return View(cached);
            }

            var news = await FetchRssNewsAsync(20);
            _memoryCache.Set("NewsDetailCache", news, TimeSpan.FromMinutes(20));
            return View(news);
        }

        // RSS'i parse edip haber listesi döner — her yerden çağrılabilir
        public async Task<List<NewsItemViewModel>> FetchRssNewsAsync(int limit = 10)
        {
            var result = new List<NewsItemViewModel>();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(8);

            foreach (var (feedUrl, sourceName) in RssSources)
            {
                if (result.Count >= limit) break;

                try
                {
                    var response = await client.GetAsync(feedUrl);
                    if (!response.IsSuccessStatusCode) continue;

                    var xml = await response.Content.ReadAsStringAsync();
                    var doc = XDocument.Parse(xml);

                    var items = doc.Descendants("item")
                        .Take(limit / RssSources.Count + 2)
                        .Select(item =>
                        {
                            // <link> RSS'te bazen metin, bazen CDATA içinde gelir
                            var linkEl = item.Element("link");
                            var link = linkEl?.Value?.Trim();
                            if (string.IsNullOrEmpty(link))
                            {
                                // link elementi CDATA ise sonraki metin node'unu dene
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

                    result.AddRange(items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RSS hata ({sourceName}): {ex.Message}");
                }
            }

            return result.Take(limit).ToList();
        }
    }
}