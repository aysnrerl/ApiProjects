using System.Collections.Generic;

namespace ApiProjects.Models
{
    public class MovieViewModel
    {
        // TMDB API bize "results" adında bir film listesi döner
        public List<TmdbMovie> results { get; set; }
    }

    public class TmdbMovie
    {
        public string title { get; set; }          // Film adı
        public string overview { get; set; }       // Filmin kısa özeti
        public string poster_path { get; set; }     // Afiş resminin kodu (/kDp26... gibi)
        public double vote_average { get; set; }    // Filmin IMDb/TMDB puanı
        public string release_date { get; set; }    // Vizyon tarihi
    }
}