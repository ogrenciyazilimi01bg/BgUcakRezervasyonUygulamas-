using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgUcakRezervasyonUygulaması.Models
{
    public class Rezervasyon
    {
        public int Id { get; set; }
        public int LokasyonId { get; set; }
        public int UcusId { get; set; }
        public int UcakId { get; set; }
        public string MusteriAd { get; set; }
        public string KoltukNumarasi { get; set; }
        public string Tarih { get; set; }
        public bool Engelli { get; set; }
        public bool Yasli { get; set; } 
        public int Yas { get; set; }
        public string Cinsiyet { get; set;}
    }
}
