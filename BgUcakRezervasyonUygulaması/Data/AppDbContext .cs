using Microsoft.EntityFrameworkCore;
using BgUcakRezervasyonUygulaması.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgUcakRezervasyonUygulaması.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= ..\\..\\Data\\BgUcakRezervasyonUygulamasıDb.db");
        }

        public DbSet<Ucak> Ucak { get; set; }
        public DbSet<Lokasyon> Lokasyon { get; set; }
        public DbSet<Ucus> Ucus { get; set; }
        public DbSet<Rezervasyon> Rezervasyon { get; set; }

    }
}
