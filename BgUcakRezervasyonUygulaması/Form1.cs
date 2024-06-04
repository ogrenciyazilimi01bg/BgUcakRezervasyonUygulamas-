using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;
using BgUcakRezervasyonUygulaması.Models;

namespace BgUcakRezervasyonUygulaması
{
    public partial class Form1 : Form
    {
        private static string connectionString = "Data Source=..\\..\\Data\\BgUcakRezervasyonUygulamasıDb.db;Version=3;";
        public Form1()
        {
            InitializeComponent();
            LoadUcusData();
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            Buguntarih.Text = DateTime.Now.ToString("dd.MM.yy");
            comboBoxSehir.SelectedIndexChanged += ComboBoxSehir_SelectedIndexChanged;
            comboBoxSaat.SelectedIndexChanged += ComboBoxSaat_SelectedIndexChanged;
            comboBox3.Items.Add("Kadın");
            comboBox3.Items.Add("Erkek");
            checkBox2.Enabled = false;

            InitializeButtons();
            if (comboBoxSaat.Items.Count > 0)
            {
                comboBoxSaat.SelectedIndex = 0;
            }
            if (comboBoxSehir.Items.Count > 0)
            {
                comboBoxSehir.SelectedIndex = 0;
            }
        }
        string Lok_id;
        string ucusId;
        bool isFirst = true;
        private void ComboBoxSehir_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUcakModel();
        }

        private void LoadRezervasyonData()
        {
            List<string> reservedSeats = new List<string>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT Rezervasyon.id AS RezervasyonID, musteriAd AS MusteriAd, koltukNumarasi AS KoltukNumarasi, 
                   tarih AS Tarih, lokasyonId AS LokasyonID, ucakId AS UcakID, 
                   ucusId AS UcusID, 
                   engelli AS Engelli, 
                   yasli AS Yasli, 
                   yas AS Yas, 
                   cinsiyet AS Cinsiyet
            FROM Rezervasyon Where lokasyonId=@lok_id AND tarih=@tarih AND ucusId=@ucusId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@lok_id", Lok_id);
                    command.Parameters.AddWithValue("@tarih", comboBoxTarih.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@ucusId", ucusId);

                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet, "Rezervasyon");
                        dataGridView2.DataSource = dataSet.Tables["Rezervasyon"];
                    }
                }

                // Engelli için Checkbox kolonu oluşturur.
                DataGridViewCheckBoxColumn engelliColumn = new DataGridViewCheckBoxColumn();
                engelliColumn.Name = "Engelli";
                engelliColumn.HeaderText = "Engelli";
                engelliColumn.DataPropertyName = "Engelli"; // Bu veritabanı kolonuna bağlanır.

                // Mevcut kolonu kaldırır ve yerine checkbox kolonu ekler.
                if (dataGridView2.Columns["Engelli"] != null)
                {
                    dataGridView2.Columns.Remove("Engelli");
                }
                dataGridView2.Columns.Add(engelliColumn);

                // Yaslı için Checkbox kolonu oluşturur.
                DataGridViewCheckBoxColumn yasliColumn = new DataGridViewCheckBoxColumn();
                yasliColumn.Name = "Yasli";
                yasliColumn.HeaderText = "Yaşlı";
                yasliColumn.DataPropertyName = "Yasli"; // Bu veritabanı kolonuna bağlanır.

                // Mevcut kolonu kaldırır ve yerine checkbox kolonu ekler.
                if (dataGridView2.Columns["Yasli"] != null)
                {
                    dataGridView2.Columns.Remove("Yasli");
                }
                dataGridView2.Columns.Add(yasliColumn);

                // Her satır için checkbox değerini ayarla ve reservedSeats listesine koltuk numaralarını ekler.
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["Engelli"].Value != null && row.Cells["Engelli"].Value.ToString() == "1")
                    {
                        row.Cells["Engelli"].Value = true;
                    }
                    else
                    {
                        row.Cells["Engelli"].Value = false;
                    }

                    if (row.Cells["Yasli"].Value != null && row.Cells["Yasli"].Value.ToString() == "1")
                    {
                        row.Cells["Yasli"].Value = true;
                    }
                    else
                    {
                        row.Cells["Yasli"].Value = false;
                    }

                    if (row.Cells["KoltukNumarasi"].Value != null)
                    {
                        reservedSeats.Add(row.Cells["KoltukNumarasi"].Value.ToString());
                    }
                }
            }

            // Rezerve edilmiş koltukları vurgular.
            HighlightReservedSeats(reservedSeats);
        }

        private void ucusIdBul(ComboBox obj)
        {
            if (obj.Name != "comboBoxSaat")
            {
                dataGridView2.DataSource = null;

                // Tüm butonların rengini yeşil yapar.
                foreach (var button in seatButtonMap.Values)
                {
                    button.BackColor = Color.Green;
                    button.Enabled = true;
                }

                labelKoltukNo.Text = string.Empty;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id FROM Ucus WHERE saat = @saat AND LokasyonId = @lok_id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@saat", obj.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@lok_id", Lok_id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ucusId = reader["id"].ToString();
                        }
                    }
                }
            }
            LoadRezervasyonData();
        }

        private void ComboBoxSaat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucusIdBul(comboBoxSaat);
            string selectedSaat = comboBoxSaat.SelectedItem.ToString();

            // comboBox1 içine yazdırır.
            comboBox1.Items.Clear(); // Önceki verileri temizler.

            // Diğer saat verilerini Ucus tablosundan alır ve comboBox1 içine ekler.
            LoadSaatIntoComboBox(selectedSaat);
            LoadUcakModel();
        }

        private void LoadSaatIntoComboBox(string selectedSaat)
        {
            List<string> uniqueSaats = new List<string>(); // Benzersiz saatleri saklamak için bir liste oluşturur.

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT saat FROM Ucus";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string saat = reader["saat"].ToString();
                            if (!uniqueSaats.Contains(saat)) // Eğer saat listede yoksa ekler.
                            {
                                comboBox1.Items.Add(saat);
                                uniqueSaats.Add(saat); // Listeye ekler.
                            }
                        }
                    }
                }
            }

            // Seçilen saati ComboBox'ta seçili olarak belirtir.
            comboBox1.SelectedItem = selectedSaat;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime bugununTarihi = DateTime.Now;
            for (int i = 0; i <= 5; i++)
            {
                DateTime gelecektekiTarih = bugununTarihi.AddDays(i);
                string gelecektekiTarihFormatli = gelecektekiTarih.ToString("dd.MM.yy");
                comboBoxTarih.Items.Add(gelecektekiTarihFormatli);


            }
            LoadCountriesIntoComboBox();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            LoadSaatIntoComboBox();
            InitializeButtons();


        }
        private Dictionary<string, Button> seatButtonMap = new Dictionary<string, Button>();
        private void InitializeButtons()
        {
            int buttonIndexOffset = 3; // button3'ten başlamak için offset.
            int totalButtons = 45;

            // Butonları formun Controls koleksiyonunda arayacak.
            for (int i = 0; i < totalButtons - buttonIndexOffset; i++)
            {
                Button button = this.Controls.Find($"button{i + buttonIndexOffset}", true).FirstOrDefault() as Button;
                if (button != null)
                {
                    int group = i / 6;
                    int index = i % 6 + 1;
                    char letter = (char)('A' + group);
                    string seatNumber = $"{letter}{index}";
                    button.Text = seatNumber;

                    // Butonun arka plan rengini yeşil yapar.
                    button.BackColor = Color.Green;

                    button.Enabled = true;

                    // Sözlüğe butonu eklenir.
                    seatButtonMap[seatNumber] = button;

                    // Click event handler'ı eklenir
                    button.Click += SeatButton_Click;
                    

                }
            }
        }

        private Button previouslySelectedButton = null;

        private void SeatButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Eğer buton zaten yeşilse 
                if (button.BackColor == Color.Green || (previouslySelectedButton != null && previouslySelectedButton == button))
                {
                    // Seçilen koltuğun numarasını etikete yazar.
                    labelKoltukNo.Text = button.Text;

                    // Önceki seçilen koltuğun rengini yeşil yapar.
                    if (previouslySelectedButton != null)
                    {
                        previouslySelectedButton.BackColor = Color.Green;
                    }

                    // Şu an seçilen koltuğun rengini turuncu yapar.
                    button.BackColor = Color.Orange;

                    // Bu koltuğu önceki seçilen koltuk olarak ayarlar.
                    previouslySelectedButton = button;
                }
            }
        }
        private void HighlightReservedSeats(List<string> reservedSeats)
        {
            foreach (string seatNumber in reservedSeats)
            {
                if (seatButtonMap.TryGetValue(seatNumber, out Button button))
                {
                    // Buton üzerinde işlem yapar, örneğin arka plan rengini değiştirir.
                    button.BackColor = Color.Red;
                    button.Enabled = false;
                }
            }
        }

        private void comboBoxTarih_SelectedIndexChanged(object sender, EventArgs e)
        {
            TarihS.Text = comboBoxTarih.SelectedItem.ToString();
        }


        private void LoadUcakModel()
        {
            string selectedSehir = comboBoxSehir.SelectedItem?.ToString();
            string selectedSaat = comboBoxSaat.SelectedItem?.ToString();


            if (string.IsNullOrEmpty(selectedSehir) || string.IsNullOrEmpty(selectedSaat))
                return;

            int? ucakId = GetUcakIdFromUcus(selectedSehir, selectedSaat);

            if (ucakId.HasValue)
            {
                string model = GetModelFromUcak(ucakId.Value);
                Modellbl.Text = model;
            }
            else
            {
                Modellbl.Text = "Uygun uçak bulunamadı.";
            }
            if (ucakId.HasValue)
            {
                (string marka, string seriNo, int koltukKapasitesi) = GetUcakDetailsFromUcak(ucakId.Value);
                UcakMarkasiA.Text = marka;
                SeriNoA.Text = seriNo;
                KoltukKapasitesi.Text = koltukKapasitesi.ToString();
            }
            else
            {
                UcakMarkasiA.Text = "Marka bilgisi bulunamadı.";
                SeriNoA.Text = "Seri No bilgisi bulunamadı.";
                KoltukKapasitesi.Text = "Koltuk kapasitesi bilgisi bulunamadı.";
            }
        }
        private (string, string, int) GetUcakDetailsFromUcak(int ucakId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT marka, seriNo, koltukKapasitesi FROM Ucak WHERE id = @ucakId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ucakId", ucakId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string marka = reader["marka"].ToString();
                            string seriNo = reader["seriNo"].ToString();
                            int koltukKapasitesi = Convert.ToInt32(reader["koltukKapasitesi"]);

                            return (marka, seriNo, koltukKapasitesi);
                        }
                    }
                }
            }

            return ("", "", 0); // Bulunamadı durumu için varsayılan değerler döndürülür.
        }


        private int? GetUcakIdFromUcus(string sehir, string saat)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT Ucus.ucakId
                    FROM Ucus
                    INNER JOIN Lokasyon ON Ucus.LokasyonId = Lokasyon.id
                    WHERE Lokasyon.sehir = @sehir AND Ucus.saat = @saat";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sehir", sehir);
                    command.Parameters.AddWithValue("@saat", saat);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            return null;
        }

        private string GetModelFromUcak(int ucakId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT model FROM Ucak WHERE id = @ucakId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ucakId", ucakId);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                }
            }

            return "Model bilgisi bulunamadı.";
        }
        private void comboBoxUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxSehir.Items.Clear(); // Önceki şehirleri temizler.
            string selectedCountry = comboBoxUlke.SelectedItem?.ToString(); // Seçilen ülkeyi alır.

            // Seçilen ülkeye göre ilgili şehirleri veritabanından alır.
            if (!string.IsNullOrEmpty(selectedCountry))
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT sehir FROM Lokasyon WHERE AktifPasif = 1 AND ulke = @country";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@country", selectedCountry);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxSehir.Items.Add(reader["sehir"].ToString());
                            }
                        }
                    }
                }
            }

        }



        private void LoadUcusData()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT Ucus.id AS UcusID,Lokasyon.AktifPasif AS AktifPasif, Ucus.saat AS Saat, 
                   Lokasyon.ulke AS Ulke, Lokasyon.sehir AS Sehir
                   
            FROM Ucus
            INNER JOIN Lokasyon ON Ucus.LokasyonId = Lokasyon.id";

                using (var adapter = new SQLiteDataAdapter(query, connection))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet, "Ucus");

                    dataGridView1.DataSource = dataSet.Tables["Ucus"];

                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Geçerli bir hücre olduğundan emin olunur.
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // AktifPasif sütununda veri varsa metni belirtir.
                if (cell.OwningColumn.Name == "AktifPasif")
                {
                    if (cell.Value != null && cell.Value != DBNull.Value)
                    {
                        int value = Convert.ToInt32(cell.Value);
                        if (value == 0)
                        {
                            e.Value = "Pasif";
                            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red; // AktifPasif durumu 0 ise kırmızı arka plan rengi.
                        }
                        else if (value == 1)
                        {
                            e.Value = "Aktif";
                            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green; // AktifPasif durumu 1 ise yeşil arka plan rengi.
                        }
                    }
                }
            }
        }
        private void LoadCountriesIntoComboBox()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT ulke FROM Lokasyon WHERE AktifPasif = 1";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxUlke.Items.Add(reader["ulke"].ToString());
                        }
                    }
                }
            }
        }

        private void comboBoxSehir_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCity = comboBoxSehir.SelectedItem?.ToString(); // Seçilen şehri alır.


            // Seçilen şehre göre ilgili havaalanlarını veritabanından alır.
            if (!string.IsNullOrEmpty(selectedCity))
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT havaalani FROM Lokasyon WHERE AktifPasif = 1 AND sehir = @city";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@city", selectedCity);
                        var havaalaniList = new List<string>();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                havaalaniList.Add(reader["havaalani"].ToString());
                            }
                        }

                        if (havaalaniList.Count > 0)
                        {
                            comboBoxHavaalani.DataSource = havaalaniList; // ComboBox'a havaalanı bilgisini veri kaynağı olarak atar.
                        }
                        else
                        {
                            comboBoxHavaalani.Items.Clear();
                            comboBoxHavaalani.Items.Add("Havaalanı bulunamadı"); // Bulunamazsa uygun bir mesaj ekler.
                            comboBoxHavaalani.SelectedIndex = 0; // İlk mesajı seçili hale getirir.
                        }
                    }
                }
            }
            else
            {
                // Seçilen şehir boş veya null ise, ComboBox'a uygun bir mesaj ekler.
                comboBoxHavaalani.Items.Clear();
                comboBoxHavaalani.Items.Add("Lütfen bir şehir seçin");
                comboBoxHavaalani.SelectedIndex = 0; // İlk mesajı seçili hale getirir.
            }
            LokasyonVeriA.Text = comboBoxSehir.SelectedItem.ToString();
            Lokasyons.Text = comboBoxSehir.SelectedItem.ToString();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id FROM Lokasyon WHERE sehir = @sehir";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sehir", comboBoxSehir.SelectedItem.ToString());
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lok_id = reader["id"].ToString();


                        }
                    }
                }
            }
        }




        private void LoadSaatIntoComboBox()
        {
            List<string> uniqueSaats = new List<string>(); // Benzersiz saatleri saklamak için bir liste oluşturur.

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT saat FROM Ucus";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string saat = reader["saat"].ToString();
                            if (!uniqueSaats.Contains(saat)) // Eğer saat listede yoksa eklenir.
                            {
                                comboBoxSaat.Items.Add(saat);
                                uniqueSaats.Add(saat); // Listeye eklenir.
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Combobox'ların seçili olup olmadığını kontrol edilir.
            if (comboBoxTarih.SelectedItem != null &&
                comboBoxUlke.SelectedItem != null &&
                comboBoxSehir.SelectedItem != null)
            {
                // Eğer hepsi seçiliyse, tabUcak tabına geçilir.

                LoadRezervasyonData();
                tabControl1.SelectTab(tabUcak);
            }
            else
            {
                // Eğer herhangi biri seçili değilse, kullanıcıya uyarı verir.
                MessageBox.Show("Lütfen tüm bilgileri seçiniz.");
            }

        }


        private void buttonDevam_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabRezervasyon);
        }


        private void comboBoxTarih_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            TarihS.Text = comboBoxTarih.SelectedItem.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isFirst)
            {
                ucusIdBul(comboBox1);
            }
            else
            {
                isFirst = false;
            }

        }
       

        

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // numericUpDown1'in değeri 70'ten büyükse checkBox2'yi işaretler.
            if (numericUpDown1.Value >= 70)
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }
        }

        private void Bitir_Click(object sender, EventArgs e)
        {
           
            string selectedSehir = comboBoxSehir.SelectedItem?.ToString();
            string selectedSaat = comboBoxSaat.SelectedItem?.ToString();


            if (string.IsNullOrEmpty(selectedSehir) || string.IsNullOrEmpty(selectedSaat))
                return;

            int? ucakId = GetUcakIdFromUcus(selectedSehir, selectedSaat);
            int IsBlocked = checkBox1.Checked ? 1 : 0;
            int IsOld = Convert.ToInt32(numericUpDown1.Value) <= 70 ? 0 : 1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    string insertRezervasyonQuery = @"
            INSERT INTO Rezervasyon (musteriAd, koltukNumarasi, tarih, lokasyonId, ucakId, ucusId, engelli, yasli, yas, cinsiyet) VALUES 
            ('" + textAd.Text + "', '" + labelKoltukNo.Text + "', '" + comboBoxTarih.SelectedItem.ToString() + "', " + Lok_id + ", " + ucakId + ", " + ucusId + "," + IsBlocked + " ," + IsOld + " ," + Convert.ToInt32(numericUpDown1.Value) + " , '" + comboBox3.SelectedItem + "')";
                    command.CommandText = insertRezervasyonQuery;
                    command.ExecuteNonQuery();
                }
            }
            LoadRezervasyonData();
            textAd.Text = "";
            numericUpDown1.Value = 1;
            labelKoltukNo.Text = "";
            checkBox1.Checked = false;
            comboBox3.Text = "";

            MessageBox.Show("Bilet satışı gerçekleşmiştir.");
        }
    }
}