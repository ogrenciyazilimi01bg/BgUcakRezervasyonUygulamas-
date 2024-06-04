using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgUcakRezervasyonUygulaması.Models
{
    public class Ucus
    {
        public int Id { get; set; }
        public int LokasyonId { get; set; }
        public int UcakId { get; set; }
        public string Saat { get; set; }
        //public DateTime Tarih { get; set; }
    }
}
