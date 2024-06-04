using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BgUcakRezervasyonUygulaması.Data
{
   
    public static class DbInitializer
    {
        private static string connectionString = "Data Source=..\\..\\Data\\BgUcakRezervasyonUygulamasıDb.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists("..\\..\\Data\\BgUcakRezervasyonUygulamasıDb.db"))
            {
                SQLiteConnection.CreateFile("..\\..\\Data\\BgUcakRezervasyonUygulamasıDb.db;");
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string createUcakTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Ucak (
                            id INTEGER PRIMARY KEY NOT NULL,
                            marka TEXT NOT NULL,
                            model TEXT NOT NULL,
                            seriNo TEXT NOT NULL,
                            koltukKapasitesi INTEGER NOT NULL
                        );";

                    // Ucak tablosuna veri ekle
                    string insertUcakQuery = @"
                        INSERT INTO Ucak (marka, model, seriNo, koltukKapasitesi) VALUES 
                        ('Boeing', '707', 'ABC554', 42),
                        ('Airbus', 'A318', 'BEG01', 42),
                        ('Bombardier', 'A220', 'GEC221', 42),
                        ('Tupolev', 'Tu-204', 'BUR345', 42),
                        ('Airbus', 'A818', 'EYG54', 42),
                        ('Bombardier', 'A420', 'EMR10', 42),
                        ('Tupolev', 'Tu044', 'OKN57', 42);";

                    // Lokasyon tablosunu oluştur
                    string createLokasyonTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Lokasyon (
                            id INTEGER PRIMARY KEY NOT NULL,
                            ulke TEXT NOT NULL,
                            sehir TEXT NOT NULL,
                            havaalani TEXT NOT NULL,
                            AktifPasif INTEGER NOT NULL
                        );";

                    // Lokasyon tablosuna veri ekle
                    string insertLokasyonQuery = @"
                        INSERT INTO Lokasyon (ulke, sehir, havaalani, AktifPasif) VALUES
                        ('Türkiye', 'İstanbul - İzmir', 'İstanbul Sabiha Gökçen Havalimanı - İzmir Adnan Menderes Havalimanı', 1),
                        ('Türkiye', 'İzmir - İstanbul', 'İzmir Adnan Menderes Havalimanı - İstanbul Sabiha Gökçen Havalimanı', 1),
                        ('Karadağ - Türkiye', 'Podgorica - İzmir', 'Podgorica Havalimanı - İzmir Adnan Menderes Havalimanı', 1),
                        ('Türkiye - Karadağ', 'İzmir - Podgorica', 'İzmir Adnan Menderes Havalimanı - Podgorica Havalimanı', 1),
                        ('Türkiye - Almanya', 'İstanbul - Berlin', 'İstanbul Sabiha Gökçen Havalimanı - Berlin Brandenburg Havalimanı', 1),
                        ('Almanya - Türkiye', 'Berlin - İstanbul', 'Berlin Brandenburg Havalimanı - İstanbul Sabiha Gökçen Havalimanı', 1),
                        ('Hollanda - Türkiye', 'Amsterdam - İstanbul', 'Amsterdam Schiphol Havalimanı - İstanbul Sabiha Gökçen Havalimanı', 0);";

                    // Ucus tablosunu oluştur
                    string createUcusTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Ucus (
                            id INTEGER PRIMARY KEY NOT NULL,
                            saat TEXT NOT NULL,
                            ucakId INTEGER NOT NULL,
                            LokasyonId INTEGER NOT NULL,
                            FOREIGN KEY (LokasyonId) REFERENCES Lokasyon(id),
                            FOREIGN KEY (ucakId) REFERENCES Ucak(id)
                        );";

                    // Ucus tablosuna veri ekle
                    string insertUcusQuery = @"
                        INSERT INTO Ucus (saat, ucakId, LokasyonId) VALUES 
                        ('11:00', 1, 1),
                        ('11:00', 2, 2),
                        ('11:00', 3, 3),
                        ('11:00', 4, 4),
                        ('11:00', 5, 5),
                        ('11:00', 6, 6),
                        ('11:00', 7, 7),
                        ('15:40', 1, 1),
                        ('15:40', 2, 2),
                        ('15:40', 3, 3),
                        ('15:40', 4, 4),
                        ('15:40', 5, 5),
                        ('15:40', 6, 6),
                        ('15:40', 7, 7),
                        ('21:00', 1, 1),
                        ('21:00', 2, 2),
                        ('21:00', 3, 3),
                        ('21:00', 4, 4),
                        ('21:00', 5, 5),
                        ('21:00', 6, 6),
                        ('21:00', 7, 7);";

                    // BiletBilgi tablosunu oluştur
                    string createRezervasyonTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Rezervasyon (
                            id INTEGER PRIMARY KEY NOT NULL,
                            musteriAd TEXT NOT NULL,
                            koltukNumarasi TEXT NOT NULL,
                            tarih TEXT NOT NULL,
                            lokasyonId INTEGER NOT NULL,
                            ucakId INTEGER NOT NULL,
                            ucusId INTEGER NOT NULL,
                            engelli INTEGER NOT NULL,
                            yasli INTEGER NOT NULL,
                            yas INTEGER NOT NULL ,
                            cinsiyet TEXT NOT NULL,
                            FOREIGN KEY (lokasyonId) REFERENCES Lokasyon(id),
                            FOREIGN KEY (ucakId) REFERENCES Ucak(id),
                            FOREIGN KEY (ucusId) REFERENCES Ucus(id)
                        );";

                    // BiletBilgi tablosuna veri ekle
                    DateTime bugununTarihi = DateTime.Now;
                    string bugununTarihiFormatli = bugununTarihi.ToString("dd.MM.yy");
                    string[] days = new string[4];
                    for (int i = 1; i <= 4; i++)
                    {
                        DateTime gelecektekiTarih = bugununTarihi.AddDays(i);
                        days[i - 1] = gelecektekiTarih.ToString("dd.MM.yy");
                    }

                    string insertRezervasyonQuery = @"
                        INSERT INTO Rezervasyon (musteriAd, koltukNumarasi, tarih, lokasyonId, ucakId, ucusId, engelli, yasli, yas, cinsiyet) VALUES 
                        ('Nilay Resmi', 'A1', '" + days[2] + @"', 1, 1, 1,0 ,0 ,29 , 'Kadın'),
                        ('Begüm Geçer', 'B1', '" + bugununTarihiFormatli + @"', 1,1,8,0,0,22,'Kadın'),
                        ('Begüm Geçer', 'B1', '" + days[0] + @"', 4,4,4,0,0,22,'Kadın'),
                        ('Merve Sezgi', 'D4', '" + bugununTarihiFormatli + @"', 4, 4, 4,0,0,22,'Kadın'),
                        ('Kerim Durmaz', 'D1', '" + bugununTarihiFormatli + @"', 4, 4, 11,0,0,25,'Erkek'),
                        ('Ali Kiraz', 'D2', '" + bugununTarihiFormatli + @"', 4, 4, 11,0,0,24,'Erkek'),
                        ('Melike Tez', 'F6', '" + bugununTarihiFormatli + @"', 4, 4, 11,0,0,25,'Kadın'),
                        ('Emin Dehmen', 'G6', '" + bugununTarihiFormatli + @"', 4, 4, 18,0,0,25,'Erkek'),
                        ('Deniz Can ', 'F1', '" + bugununTarihiFormatli + @"', 4, 4, 18,1,0,25,'Erkek'),
                        ('Elif Mavi', 'B1', '" + bugununTarihiFormatli + @"', 4, 4, 18,0,0,22,'Kadın'),
                        ('Alp Mavi ', 'B2', '" + bugununTarihiFormatli + @"', 4, 4, 18,0,0,22,'Erkek'),
                        ('Begüm Geçer', 'B1', '" + days[3] + @"', 3,3,3,0,0,22,'Kadın'),
                        ('Begüm Geçer', 'B1', '" + days[3] + @"', 2,2,16,0,0,22,'Kadın'),
                        ('Ege Garanti', 'A1', '" + bugununTarihiFormatli + @"', 5, 5, 19,0,0,27,'Erkek'),
                        ('Mustafa Aksoy ', 'A2', '" + bugununTarihiFormatli + @"', 5, 5, 19,1,0,27,'Erkek'),
                        ('Ayşe Aydın', 'A3', '" + bugununTarihiFormatli + @"', 5, 5, 19,0,0,26,'Kadın'),
                        ('Efe Öztürk ', 'A4', '" + bugununTarihiFormatli + @"', 5, 5, 19,1,0,26,'Erkek'),
                        ('Bilge Türk', 'A6', '" + bugununTarihiFormatli + @"', 5, 5, 5,1,1,80,'Kadın'),
                        ('Ensar Türk ', 'A5', '" + bugununTarihiFormatli + @"', 5, 5, 5,1,1,81,'Erkek'),
                        ('Nur Haktan', 'C3', '" + bugununTarihiFormatli + @"', 5, 5, 12,0,0,30,'Kadın'),
                        ('Hakan Sevinç ', 'F6', '" + bugununTarihiFormatli + @"', 5, 5, 12,0,0,40,'Erkek'),
                        ('Bora Kaya', 'G6', '" + bugununTarihiFormatli + @"', 6, 6, 6,1,0,50,'Erkek'),
                        ('Deniz Kaptan ', 'F1', '" + bugununTarihiFormatli + @"', 6, 6, 13,1,1,70,'Erkek'),
                        ('Ege Kavaklı', 'B1', '" + bugununTarihiFormatli + @"', 6, 6, 20,1,0,22,'Erkek'),
                        ('Ahmet Öz ', 'B2', '" + bugununTarihiFormatli + @"', 6, 6,6 ,0,0,32,'Erkek'),
                        ('Sude Çağrı', 'B5', '" + days[3] + @"', 1, 1, 1, 0,0,27 ,'Kadın'),
                        ('Zehra Gurur', 'C5', '" + days[1] + @"', 2, 2, 9,1 ,0,26 ,'Kadın'),
                        ('Gizem Güven', 'D4', '" + days[0] + @"', 2, 2, 9,0,0,30,'Kadın'),
                        ('Zehra Gurur', 'A1', '" + days[2] + @"', 2, 2, 9,0,1,80,'Kadın'),
                        ('Ece Düz', 'E4', '" + days[3] + @"', 2, 2, 9,1,1,75,'Kadın'),
                        ('Ece Güler', 'E1', '" + days[0] + @"', 2, 2, 9, 0,0,20,'Kadın'),
                        ('Okan Candan', 'G2', '" + days[1] + @"', 2, 2, 9,0,1,85,'Erkek'),
                        ('Serap Durmaz', 'C6','" +bugununTarihiFormatli + @"', 3, 3, 10, 0, 1,91,'Kadın'),
                        ('Burcu Deniz', 'C6','" + bugununTarihiFormatli + @"', 3, 3, 3, 0, 0,30,'Kadın'),
                        ('Toprak Burcu', 'C4', '" + bugununTarihiFormatli + @"', 2, 2, 9,1,0,37,'Erkek'),
                        ('Ece Düz', 'E1', '" + bugununTarihiFormatli + @"', 4, 4, 4,0,1,80,'Kadın'),
                        ('Buğra Doğan','C3','" +bugununTarihiFormatli + @"', 3, 3, 17, 0, 0,30,'Erkek'),
                        ('Emir Bulut', 'G1', '" + bugununTarihiFormatli + @"', 2, 2, 2,1,1,95,'Erkek'),
                        ('Kerem Toprak', 'E5', '" + bugununTarihiFormatli + @"', 2, 2, 2,0,0,26,'Erkek'),
                        ('Ece Düz', 'B4', '" + bugununTarihiFormatli + @"', 2, 2, 16,0,1,90,'Kadın'),
                        ('Kazım Gümüş','C5','" +bugununTarihiFormatli + @"', 1, 1, 8, 0, 0,30,'Erkek'),
                        ('Nilsu Çiçek','C2','" +bugununTarihiFormatli + @"', 1, 1, 8, 0, 0,24,'Kadın'),
                        ('Çilek Demir','C2','" +bugununTarihiFormatli + @"', 1, 1, 15, 0, 0,24,'Kadın'),
                        ('Burak Can', 'G4', '" + bugununTarihiFormatli + @"', 2, 2, 16,1,0,42,'Erkek'),
                        ('Sezen Umut', 'G6', '" + bugununTarihiFormatli + @"', 1, 1, 1,0,1,85,'Kadın');";




                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createUcakTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = insertUcakQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createLokasyonTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = insertLokasyonQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createUcusTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = insertUcusQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createRezervasyonTableQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = insertRezervasyonQuery;
                        command.ExecuteNonQuery();
                    }

                }
            }

        }


    }
}
