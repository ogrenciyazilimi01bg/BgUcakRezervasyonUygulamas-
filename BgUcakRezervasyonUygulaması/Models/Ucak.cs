using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgUcakRezervasyonUygulaması.Models
{
    public class Ucak
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Marka { get; set; }
        public string SeriNo { get; set; }
        public int KoltukKapasitesi { get; set; }

    }
}
