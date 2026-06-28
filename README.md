# ApiProjects - Modern Dashboard Application

Bu proje, günlük ihtiyaç duyulabilecek çeşitli canlı verileri (hava durumu, döviz kurları, kripto para piyasası, akaryakıt fiyatları, futbol puan durumu, trend filmler, popüler müzikler, güncel haberler, yemek tarifleri ve günün sözü) tek bir şık arayüzde birleştiren modern bir **ASP.NET Core MVC** kontrol paneli (dashboard) uygulamasıdır.

## 🚀 Özellikler

Uygulama, tamamen responsive (mobil uyumlu) ve modern bir kullanıcı arayüzüne sahip olup aşağıdaki 10 farklı widget bileşenini içerir:

*   **🌤 Hava Durumu:** Open-Meteo API kullanılarak İstanbul için 3 günlük canlı hava tahmini.
*   **💱 Döviz Kurları:** Open Exchange Rates API üzerinden USD/TRY, EUR/TRY ve GBP/TRY gibi güncel döviz çiftlerinin takibi.
*   **🪙 Kripto Para:** CoinLore API entegrasyonu ile en popüler 20 kripto para biriminin anlık fiyatları ve 24 saatlik değişim oranları.
*   **⛽ Akaryakıt Fiyatları:** Gas Price API üzerinden Türkiye ve Avrupa ülkelerine ait güncel benzin, motorin ve LPG fiyatları.
*   **⚽ Süper Lig Zirvesi:** Canlı puan durumu verisiyle Süper Lig'in zirvesindeki takımlar ve istatistikleri.
*   **🎬 Günün Filmi:** TMDB (The Movie Database) API kullanılarak günün en popüler filmi, özeti ve puanı.
*   **🎵 Günün Şarkısı:** Deezer API aracılığıyla küresel müzik listelerinin zirvesindeki şarkılar ve albüm detayları.
*   **📰 Güncel Haberler:** Hürriyet, Sabah ve NTV gibi güvenilir ulusal kaynaklardan çekilen canlı RSS haber akışı.
*   **🍳 Günün Tarifi:** TheMealDB API entegrasyonu ile her gün için farklı bir yemek önerisi ve tarifi.
*   **✍️ Günün Sözü:** ZenQuotes API aracılığıyla günün motivasyonel sözü ve yazarı.

---

## 🛠 Kullanılan Teknolojiler

*   **Backend:** .NET Core (ASP.NET Core 10.0 MVC)
*   **Frontend:** HTML5, Vanilla CSS, Javascript
*   **Veri Yönetimi:** `IMemoryCache` ile verimli veri önbelleğe alma (Caching)
*   **İletişim:** Harici servislerle veri alışverişi için `IHttpClientFactory` ve `HttpClient`
*   **Tasarım:** Modern karanlık tema, özel kart tasarımları ve mikro animasyonlar

---

## ⚙️ Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/kullanici_adiniz/ApiProjects.git
cd ApiProjects
```

### 2. API Anahtarlarını Yapılandırın
Projede kullanılan bazı servisler API anahtarı gerektirmektedir. Güvenlik amacıyla bu anahtarlar GitHub'a yüklenmez. 

1. Projenin ana dizininde bulunan `appsettings.Example.json` dosyasının adını `appsettings.json` olarak değiştirin.
2. Dosya içerisindeki ilgili alanlara kendi API anahtarlarınızı yazın:

```json
{
  "ApiKeys": {
    "RapidApiKey": "SİZİN_RAPID_API_ANAHTARINIZ",
    "TmdbBearerToken": "SİZİN_TMDB_BEARER_TOKENİNİZ"
  }
}
```

### 3. Projeyi Derleyin ve Çalıştırın
```bash
dotnet restore
dotnet build
dotnet run --project ApiProjects/ApiProjects.csproj
```

Uygulama varsayılan olarak `http://localhost:5119` adresi üzerinden çalışmaya başlayacaktır.

---

## 📝 Lisans
Bu proje MIT Lisansı ile lisanslanmıştır.