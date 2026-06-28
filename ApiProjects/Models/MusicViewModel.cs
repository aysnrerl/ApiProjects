using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ApiProjects.Models
{
    // Deezer Chart API modeli — API Key gerektirmez
    public class MusicViewModel
    {
        [JsonPropertyName("data")]
        public List<DeezerTrack> data { get; set; }
    }

    public class DeezerTrack
    {
        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("rank")]
        public int rank { get; set; }

        [JsonPropertyName("preview")]
        public string preview { get; set; } // 30 saniyelik önizleme URL'si

        [JsonPropertyName("artist")]
        public DeezerArtist artist { get; set; }

        [JsonPropertyName("album")]
        public DeezerAlbum album { get; set; }
    }

    public class DeezerArtist
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("picture_medium")]
        public string picture_medium { get; set; }
    }

    public class DeezerAlbum
    {
        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("cover_medium")]
        public string cover_medium { get; set; }  // 250x250 kapak görseli
    }
}