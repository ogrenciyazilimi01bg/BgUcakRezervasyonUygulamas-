# BgUcakRezervasyonUygulaması
Ucak Bileti Rezervasyon Uygulaması
Bu, C# ve Visual Studio kullanılarak geliştirilmiş bir windows form uygulamasıdır. Bu uygulama, kullanıcının bir uçak bileti  rezervasyon işlemini gerçekleştirmesini  sağlar.
## Genel bakış
Bu projede, Ucak Bilet rezervasyon işlemleri için geliştirilen Ucak bilet rezervasyonu ve seçilen uçuşa ait  biletlerin gösterildiği kodların kaynağı yer almaktadır. Bu proje, C# dilinde yazılmış bir uçak bilet rezervasyon sistemi uygulamasını içermektedir.Proje temelde Ucak bileti rezervasyon projesi Windows Form üzerinden yapılmaktadır. Proje kullanıcının istekleri doğrultusunda bilet rezervasyonu ve bilet bilgileri göstermeyi amaçlamaktadır. Bu yönergeleri, projeyi kendi makinenizde çalıştırmak ve geliştirmek için kullanabilirsiniz.Bu uygulama, bir havayolları markasının  bilet rezervasyon  işlemlerini yönetmek için tasarlanmıştır. Kullanıcılar, mevcut lokasyonlar arasından seçim yapabilir, Ucus saatlerini görüntüleyebilir ve boş koltuklara bilet rezerve yapabilir .Windows Form da tek bir form üzerinden tablar oluşturularak yapıldı.
## Proje Yapısı
Uygulama, aşağıdaki bileşenlerden oluşur: BgUcakRezervasyonUygulaması: Ana proje dosyası. BgUcakRezervasyonUygulaması.Data: Veritabanı işlemleri için gereken sınıfları içerir. BgUcakRezervasyonUygulaması.Models: Uygulama veri modelini tanımlayan sınıfları içerir. BgUcakRezervasyonUygulaması.Servisler: Uygulama iş mantığını içeren servis sınıflarını içerir. Form1.cs: Ana uygulama formu ve kullanıcı arayüzü işlemlerini içerir.
## Kullanılan Teknolojiler
C#: Uygulama, C# programlama dili kullanılarak geliştirilmiştir. SQLite: Hafif ve yerel bir veritabanı kullanılmıştır. Windows Forms (WinForms): Kullanıcı arayüzü oluşturmak için Windows Forms kullanılmıştır.
Kurulum
Depoyu bilgisayarınıza klonlayın veya ZIP dosyası olarak indirin. Visual Studio'da projeyi açın. Proje bağımlılıklarını yüklemek için NuGet Paket Yöneticisi'ni kullanın. Projeyi derleyin ve çalıştırın.

## Dosya Yapısı
BgUcakRezervasyonUygulaması: Ana uygulama dosyası.
Data: Veritabanı dosyalarını içeren klasör.
Models: Uygulama veri modellerini içeren klasör.
Form1.cs: Ana formun kodları.
Form1.Designer.cs: Ana formun tasarım kodları.
BgUcakRezervasyonUygulamasıDb.db: SQLite veritabanı dosyası.
## Sınıflar
Bu proje aşağıdaki sınıflardan oluşmaktadır: Lokasyon.cs:Lokasyonları temsil eden sınıfı içerir. Ucak.cs:Ucak bilgilerini temsil eden sınıfı içerir. Ucus.cs:Ucuş verilerini temsil eden sınıfı içerir. Rezervasyon.cs:Bilet rezervasyonlarını temsil eden sınıfı içerir.
Kullanımı
## Uygulama şu şekilde çalışır:
Program Çlıştırıldığında Windows Form açılır. Ve ilk Tab olan Lokasyonda işlemler başlar. Burda Bugüne ait uçuşlar ve onlara ait veriler gösterilir.Yeşil renkli olan uçuşlar aktif ve kırmızı renkli olan pasif uçuşlardır. ComboBoxTarihten ne zaman uçmak istediğine dair veri seçilir.ComboBoxUlke de gidilmek istene ülke seçilir .Ulke verisine göre ComboboxSehirden şehir verisi seçilir nereden nereye olduğu Bu seçilen şehir verisine göre havaalanı verisi otomatik yüklenir.Burdan ucus saati seçilir.İşleme devam et butonuna basılarak bir sonraki tab kısmına yani Ucak Bilgileri tabına geçilir burda seçilen verilere göre uçak bilgileri gösterilir.İşleme devam et butonuna basılır.
Rezervasyon tabına geçilir burda seçilen uçuş verilerine göre Koltuk durumları gösterilir ve o an seçilmiş uçak verilerine göre o anki uçuşa ait rezervasyon bilgileri gösterilir.Burda kişi Yeşil renkli koltuklardan birini rezervasyon yapmak için seçer.Kırmızı renkli koltuklar dolu koltuklar.Ve ad,yaş ,engel durumu, cinsiyet verilerini girerek.İşlemi bitir butonuna basar .Burda yaşlı verisi yaş kısmında girilen yaş  70 ve 70 yaştan büyükse Checkbox otomatik dolar.Engelli durumu varsa orda check box işaretlenmeli.Veri İşlemi bitir butonuna basılınca kaydedilir ve yeni bir rezervasyon daha arka arkaya yapılabilir.Bu sayfadaki Ucuşlar kısmında uçuş saati değişirse koltuk verileride o uçuşa göre değişecektir. Örneğin :Bu programı bugün ayın kaçıysa onu seçip ardından Türkiye ,İzmir -İstanbul 11:00 verileri ile deniyebilirsiniz.
