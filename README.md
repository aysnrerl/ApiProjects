# 💻 ApiProjects - Modern Dashboard Application

Bu proje, günlük ihtiyaç duyulabilecek çeşitli canlı verileri (hava durumu, döviz kurları, kripto para piyasası, akaryakıt fiyatları, futbol puan durumu, trend filmler, popüler müzikler, güncel haberler, yemek tarifleri ve günün sözü) tek bir şık arayüzde birleştiren modern bir **ASP.NET Core MVC** kontrol paneli (dashboard) uygulamasıdır.

---

## 📸 Ekran Görüntüleri

### 1. Ana Sayfa (Dashboard)
Tüm widget'ların bir arada bulunduğu, dinamik olarak açılıp kapatılabilen modern karanlık temalı ana panel.
![Ana Sayfa](ApiProjects/wwwroot/screenshots/dashboard.png)

### 2. Hava Durumu Detay
Detaylı 3 günlük hava tahminleri, nem oranları, rüzgar hızları ve durum analizleri.
![Hava Durumu](ApiProjects/wwwroot/screenshots/weather.png)

### 3. Döviz Kurları
USD bazlı olarak çekilen ve Türk Lirası dahil 12 farklı ülkenin canlı kur takibini sağlayan detay sayfası.
![Döviz Kurları](ApiProjects/wwwroot/screenshots/currency.png)

### 4. Kripto Para Piyasası
En popüler 20 kripto para biriminin piyasa sıralaması, anlık fiyatları ve 24 saatlik değişim endeksleri.
![Kripto Para](ApiProjects/wwwroot/screenshots/crypto.png)

### 5. Avrupa Akaryakıt Fiyat Endeksi
Türkiye ve Avrupa ülkelerine ait güncel benzin, motorin ve LPG litre fiyatlarının listelendiği detay sayfası.
![Akaryakıt Fiyatları](ApiProjects/wwwroot/screenshots/fuel.png)

### 6. Süper Lig Puan Durumu
Süper Lig güncel puan durumu tablosu, galibiyet, beraberlik, mağlubiyet ve gol averajı detayları.
![Süper Lig](ApiProjects/wwwroot/screenshots/football.png)

### 7. Trend Filmler
TMDB API altyapısı kullanılarak vizyondaki ve trend listelerindeki popüler sinema filmleri.
![Trend Filmler](ApiProjects/wwwroot/screenshots/movies.png)

### 8. Popüler Müzikler
Deezer küresel listelerinin en popüler albümleri, şarkıları ve sanatçı bilgileri.
![Popüler Müzikler](ApiProjects/wwwroot/screenshots/music.png)

### 9. Son Dakika Haberler
Ulusal haber kaynaklarından çekilen canlı ve anlık RSS haber akışı listesi.
![Güncel Haberler](ApiProjects/wwwroot/screenshots/news.png)

### 10. Yemek Önerisi ve Tarifi
Dünya mutfağından rastgele seçilen günün yemeği, malzemeleri ve adım adım hazırlanış tarifi.
![Yemek Önerisi](ApiProjects/wwwroot/screenshots/recipe.png)

### 11. Günün Motivasyon Sözü
Zihni tazeleyen ve ilham veren günlük motivasyon sözleri ve yazarları.
![Motivasyon Sözü](ApiProjects/wwwroot/screenshots/quote.png)

---

## 🚀 Modüller ve Sayfa Detayları

Uygulama, her biri kendine ait denetleyiciye (Controller) ve arayüze (View) sahip 10 bağımsız modülden oluşmaktadır:

### 1. 🌤 Hava Durumu (`/Weather`)
*   **Kullanılan Servis:** Open-Meteo API (Açık kaynak, API anahtarı gerektirmez).
*   **Açıklama:** İstanbul konumu için (`latitude=41.0082`, `longitude=28.9784`) 3 günlük hava durumu tahminlerini getirir. 
*   **Teknik Detay:** Sıcaklık (`temperature_2m`), hissedilen sıcaklık (`apparent_temperature`), bağıl nem ve rüzgar hızı verilerini işler. Gelen sayısal hava durumu kodlarını (`weathercode`) Türkçe açıklamalara ve dinamik emojilere dönüştüren yardımcı metotlar barındırır.

### 2. 💱 Döviz Kurları (`/Currency`)
*   **Kullanılan Servis:** Open Exchange Rates API.
*   **Açıklama:** Döviz bilgilerini USD tabanlı baz kod üzerinden çeker ve listeler.
*   **Teknik Detay:** API'den dönen kurlar (`rates`) sözlüğünden (Dictionary) Türk Lirası (TRY) başta olmak üzere EUR, GBP, JPY, CHF, CAD, AUD, CNY, SAR, AED, NOK ve SEK değerleri ayıklanarak ekrana yansıtılır. Son güncelleme zamanı UTC olarak gösterilir.

### 3. 🪙 Kripto Para (`/Crypto`)
*   **Kullanılan Servis:** CoinLore API (Açık kaynak, API anahtarı gerektirmez).
*   **Açıklama:** Piyasa değerine göre en popüler 20 kripto parayı listeler.
*   **Teknik Detay:** CoinLore `/api/tickers/` uç noktasından çekilen veriler `CoinGeckoCoin` veri modeline eşlenir. Fiyat değişim yüzdeleri (`percent_change_24h`) analiz edilerek pozitif değişimler yeşil yukarı yönlü okla (▲), negatif değişimler kırmızı aşağı yönlü okla (▼) gösterilir. Logolar dinamik olarak sunucudan yüklenir.

### 4. ⛽ Akaryakıt Fiyatları (`/Fuel`)
*   **Kullanılan Servis:** Gas Price API (RapidAPI).
*   **Açıklama:** Türkiye ve Avrupa ülkelerine ait güncel benzin, dizel ve LPG litre fiyatlarını listeler.
*   **Teknik Detay:** RapidAPI istek kotasının (Rate Limit) dolması veya servis kesintisi yaşanması durumunda uygulamanın aksamaması ve boş ekran göstermemesi için **Yedek Veri (Fallback)** mekanizması entegre edilmiştir. Hata durumunda bellekten son geçerli fiyatlar yüklenir.

### 5. ⚽ Süper Lig Zirvesi (`/FootballMatch`)
*   **Kullanılan Servis:** Super Lig Standings API (RapidAPI).
*   **Açıklama:** Süper Lig'in güncel puan durumunu, takımların galibiyet/mağlubiyet sayılarını ve puanlarını canlı olarak listeler.
*   **Teknik Detay:** `FootballMatchViewModel` içindeki takım logoları ve istatistik nesneleri (`stats`) parse edilir. Herhangi bir logo yükleme hatasına karşı yedek logo gösterimi (`onerror` fallback) arayüzde tanımlanmıştır.

### 6. 🎬 Günün Filmi (`/Movie`)
*   **Kullanılan Servis:** TMDB (The Movie Database) API (Bearer Token Yetkilendirmeli).
*   **Açıklama:** Vizyondaki en popüler trend filmleri afişleri, özetleri, çıkış tarihleri ve TMDB puanlarıyla birlikte listeler.
*   **Teknik Detay:** HTTP isteklerine `Authorization: Bearer <token>` başlığı eklenerek TMDB v3 API'sinden günlük trend listesi çekilir. Afiş resimleri TMDB görsel sunucusundan (`https://image.tmdb.org/t/p/w500`) dinamik olarak çözümlenir.

### 7. 🎵 Günün Şarkısı (`/Music`)
*   **Kullanılan Servis:** Deezer API (Açık kaynak, API anahtarı gerektirmez).
*   **Açıklama:** Küresel müzik listelerinin zirvesindeki şarkıları, albüm kapaklarını ve sanatçı detaylarını listeler.
*   **Teknik Detay:** Deezer `/chart/0/tracks` endpoint'inden en popüler 10 şarkı çekilir. Müzik çalar arayüzünde çalınabilmesi için şarkıların 30 saniyelik ses önizleme (`preview`) adresleri de veri modelinde saklanır.

### 8. 📰 Güncel Haberler (`/NewsItem`)
*   **Kullanılan Servis:** Hürriyet, Sabah ve NTV RSS Servisleri.
*   **Açıklama:** Güvenilir haber kaynaklarından anlık XML verilerini çekip parse ederek kullanıcılara reklamsız ve temiz bir haber listesi sunar.
*   **Teknik Detay:** `System.Xml.Linq` (LINQ to XML) kütüphanesi kullanılarak harici RSS kanallarından dönen XML verileri asenkron olarak taranır. `<item>` düğümleri içerisindeki başlık (`title`), bağlantı (`link`) ve yayın tarihi (`pubDate`) alanları ayıklanarak `NewsItemViewModel` listesine dönüştürülür.

### 9. 🍳 Günün Tarifi (`/Recipe`)
*   **Kullanılan Servis:** TheMealDB API (Açık kaynak, API anahtarı gerektirmez).
*   **Açıklama:** Her gün için farklı bir yemek, kategori, köken, görsel ve adım adım hazırlanış tarifi sunar.
*   **Teknik Detay:** `/api/json/v1/1/search.php` uç noktasından çekilen yemek verileri parse edilir. Tarif detay sayfasında yemeğin malzemeleri ve hazırlık aşamaları şık bir şekilde listelenir.

### 10. ✍️ Günün Sözü (`/Quote`)
*   **Kullanılan Servis:** ZenQuotes API (Açık kaynak, API anahtarı gerektirmez).
*   **Açıklama:** Motivasyonel ve ilham verici sözleri yazarları ile birlikte listeler.
*   **Teknik Detay:** ZenQuotes API'sinden gelen JSON dizisi `QuoteViewModel` listesine deserialize edilir. Önbellekte 6 saat boyunca saklanarak gereksiz dış isteklerin önüne geçilir.

---

## 🛠 Teknik Altyapı ve Mimarisi

*   **Verimli Veri İletişimi:** Harici API servisleriyle iletişim kurulurken .NET'in performanslı `IHttpClientFactory` yapısı kullanılmıştır. Bu sayede soket tükenmesi (socket exhaustion) sorunlarının önüne geçilmiştir.
*   **Önbellek Yönetimi (Caching):** API sağlayıcılarının istek sınırlarını (Rate Limit) aşmamak ve sayfa yüklenme hızını maksimuma çıkarmak amacıyla `IMemoryCache` kullanılarak veriler bellekte önbelleğe alınmıştır. (Örneğin; döviz verileri 30 dakika, kripto verileri 10 dakika, motivasyon sözleri ise 6 saat önbellekte tutulur).
*   **Güvenli Anahtar Yönetimi (Secrets):** RapidAPI ve TMDB gibi servislerin hassas API anahtarları kaynak kodların içerisinden tamamen arındırılmış, `appsettings.json` içerisine taşınmıştır. Bu dosyanın GitHub'a kaza ile yüklenmesini engellemek için `.gitignore` yapılandırması tamamlanmıştır.

---

## ⚙️ Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/aysnrerl/ApiProjects.git
cd ApiProjects
```

### 2. API Anahtarlarını Yapılandırın
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
Uygulama varsayılan olarak `http://localhost:5119` (veya `https://localhost:7075`) adresi üzerinden çalışmaya başlayacaktır.