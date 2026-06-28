using System.Collections.Generic;

namespace ApiProjects.Models
{
    public class NewsItemViewModel
    {
        public string title { get; set; }   // Haber başlığı
        public string url { get; set; }     // Haberin kaynak linki
        public string source { get; set; }  // Haberin geldiği site
        public string pubDate { get; set; } // Yayın tarihi (RSS'ten)
    }
}