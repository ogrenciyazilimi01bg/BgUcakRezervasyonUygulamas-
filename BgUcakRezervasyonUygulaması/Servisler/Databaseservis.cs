using BgUcakRezervasyonUygulaması.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BgUcakRezervasyonUygulaması.Models;

namespace BgUcakRezervasyonUygulaması.Servisler
{

    internal class Databaseservis
    {


        AppDbContext _dbContext = new AppDbContext();
        public List<Ucak> Getucak()
        {
            return _dbContext.Ucak.ToList();
        }

        public List<Lokasyon> Getlokasyon()
        {
            return _dbContext.Lokasyon.ToList();
        }
        public List<Ucus> Getucus()
        {
            return _dbContext.Ucus.ToList();
        }
        public List<Rezervasyon> Getrezervasyon()
        {
            return _dbContext.Rezervasyon.ToList();
        }
        //public int ReturnLokasyonId(string Sehir)
        //{
        //    var lokasyon = _dbContext.Lokasyon.FirstOrDefault(f => f.Sehir == Sehir);
        //    return lokasyon != null ? lokasyon.Id : -1;
        //}
        //public int GetUcakIdBylokasyonAndUcus(int lokasyonId, string saat)
        //{
        //    var ucus = _dbContext.Ucus.FirstOrDefault(f => f.LokasyonId == lokasyonId && f.Saat == saat);
        //    if (ucus != null)
        //    {
        //        return ucus.UcakId;
        //    }
        //    return -1;
        //}



    }
}
