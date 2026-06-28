using System.Collections.Generic;

namespace ApiProjects.Models
{
    public class FuelViewModel
    {
        public bool success { get; set; }
        public List<FuelCountry> result { get; set; }
    }

    public class FuelCountry
    {
        public string country { get; set; }  // Ülke Adı (Turkey, Germany vb.)
        public string gasoline { get; set; } // Benzin Litre Fiyatı
        public string diesel { get; set; }   // Motorin Litre Fiyatı
        public string lpg { get; set; }      // LPG Litre Fiyatı
        public string currency { get; set; } // Para birimi (euro)
    }
}