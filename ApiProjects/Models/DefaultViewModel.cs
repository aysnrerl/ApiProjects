namespace ApiProjects.Models
{
        public class DefaultViewModel
        {
        // 1. Güncel Hava Durumu Verisi
        public WeatherRoot WeatherInfo { get; set; }

            // 2. Güncel Döviz Kurları (Hangi döviz modelini yazdıysan o gelecek, şimdilik taslak)
            public CurrencyViewModel CurrencyInfo { get; set; }

            // 3. Güncel Kripto Para Fiyatları
            public CryptoViewModel CryptoInfo { get; set; }

            // 4. Güncel Akaryakıt Fiyatları
            public FuelViewModel FuelInfo { get; set; }

            // 5. Güncel Bir Futbol Maçı Sonucu
            public List<FootballMatchViewModel> FootballInfo { get; set; }

            // 6. Günün En Popüler Filmi
            public MovieViewModel MovieInfo { get; set; }

            // 7. Günün En Çok Dinlenen Şarkısı
            public MusicViewModel MusicInfo { get; set; }

        // 8. 3 Adet Güncel Haber İçeriği (Liste olarak tutuyoruz)
            public List<NewsItemViewModel>? NewsInfo { get; set; }

        // 9. Yemek Tarifleri API üzerinden Günün Yemek Önerisi
            public RecipeViewModel RecipeInfo { get; set; }

            // 10. Günün Motivasyon Sözü
            public List<QuoteViewModel> QuoteInfo { get; set; }
        
    }
    
}
