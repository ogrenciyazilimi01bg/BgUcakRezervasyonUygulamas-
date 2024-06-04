using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgUcakRezervasyonUygulaması.Models
{
    public class Lokasyon
    {
        public int Id { get; set; }
        public string Sehir { get; set; }
        public string Ulke { get; set; }
        public string Havaalani { get; set; }
        public bool AktifPasif { get; set; }
        
    }
}
