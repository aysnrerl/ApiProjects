using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiProjects.Models
{
    public class CryptoViewModel
    {
        public List<CoinGeckoCoin> coins { get; set; }
    }

    public class CoinGeckoCoin
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double current_price { get; set; }
        public double price_change_percentage_24h { get; set; }
        public int market_cap_rank { get; set; }
        public long market_cap { get; set; }
    }

    public class CoinLoreResponse
    {
        [JsonPropertyName("data")]
        public List<CoinLoreCoin> Data { get; set; }
    }

    public class CoinLoreCoin
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("price_usd")]
        public string PriceUsd { get; set; }

        [JsonPropertyName("percent_change_24h")]
        public string PercentChange24h { get; set; }

        [JsonPropertyName("market_cap_usd")]
        public string MarketCapUsd { get; set; }
    }
}