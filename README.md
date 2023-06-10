# WPF ve SQL Server ile Oto Kiralama Acente Otomasyonu

Bu bir oto kiralama acentesi otomasyonudur. Sql Server ile güvenli bir şekilde tüm verileriniz korunur. Arabalara ait bir tabloda arabaların id(otomatik oluşturulur), plaka, marka, model, tip, renk, vites, yakıt, trip bilgisi, müsaitlik durumu, günlük ücret gibi bilgileri saklar. Arayüzde araç ekleme, çıkarma ve düzenleme butonları bulunmaktadır. Silme işleminde müsaitlik durumunu "ATIL DURUMDA" olarak işaretlenir ve böylelikle tamamen silme sonucu ortaya çıkabilecek veri kayıplarının önüne geçilir. Araç ve iş takibi yapmanız için gerekli butonlar bulunmaktadır ve tüm satışlar ve araçlar (atıl durumda olanlar da) listelenir. Ana sayfada atıl durumda olanların haricindeki tüm araçlar zaten listelenmektedir. İstediğiniz bir araca tıkladığınızda sağdaki panelde araç hakkında tüm bilgileri görebilirsiniz. Seçildikten sonra kiraya verme, teslim alma, boşa çıkarma ve bakıma alma butonlarını ihtiyacınız durumunda işlerinizi ve araçlarınızı takip edebilirsiniz. Tüm işlemleriniz beşeri hatalara karşı kontrol edilerek yapılmaktadır. Yani yanlışlıkla girilecek hatalı veri ve yanlış butona tıklama gibi durumlar önlenmektedir.

Kiraya verme butonunda bir form çıkmaktadır. Burada tc, ad, soyad ve kiraya verilecek gün sayısı istenmektedir. Yapılan girişler, kiralamalar adlı tabloda saklanacaktır. Arabalar tablosu ile ilişkili olan bu tabloda kimin ne zaman hangi aracı kiraladığını ve ne kadar ücret ödediği kaydedilmektedir. Geriye dönük her türlü veri depolanır ve istendiğinde uygulama içerisinde görüntülenebilir. Kirada olan bir araç yanlışlıkla yeniden kiraya verilemez, gerekli kontroller yapılmaktadır.

Teslim alma butonuna tıklandığında açılan formda aracın güncel trip bilgisi yazılmalıdır. Eskisinden büyük olması gerekir, yanlış girilmesine müsade edilmez. Giriş sonrası aracın trip bilgisi güncellenir ve araç "inceleme" durumuna alınır. Bu durumdayken isterseniz bakıma alabilirsiniz, isterseniz direkt boşa çıkarabilirsiniz.

Uygulamanın tüm kodları repoda bulunmakta. İndirdiğiniz projedeki "SqlServerBaglanti.cs" adındaki dosyada bağlantı parametresini kendi sql server ayarlarınıza göre güncelleyin. İsterseniz güncelleme sonrası projeye dahil olan setup projesini yeniden derleyerek sizde sorunsuz çalışacak olan projenin kurulum dosyasını elde edersiniz. Gerekli veritabanı dosyası hem repoda hem de setup projesinde bulunmaktadır.

### Genel Bakış
![](https://github.com/BatuhancanN/RentACar-WPFApplication/blob/main/RentACarApplication/ana%20sayfa.PNG)

### Veritabanı Şeması
![](https://github.com/BatuhancanN/RentACar-WPFApplication/blob/main/RentACarApplication/vt.PNG)

Uygulamanın sorunsuz çalışması için cihazınızda "Sql Server 2019" ve üstü kurulu olması gerekiyor. 
Kurulum sonrası masaüstüne gelen "RentACarDB.bak" dosyasını, sql server aracılığıyla sisteme entegre edin.

#### Tanıtım Videosu İçin Tıklayın
[<img src="https://github.com/BatuhancanN/RentACar-WPFApplication/blob/main/RentACarApplication/watch-video-button.png?raw=true" width="40%">](https://www.youtube.com/watch?v=k6XKBCwZQUk "Tanıtım Videosu")

#### Kurulum Videosu İçin Tıklayın
[<img src="https://github.com/BatuhancanN/RentACar-WPFApplication/blob/main/RentACarApplication/watch-video-button.png?raw=true" width="40%">](https://www.youtube.com/watch?v=raWy0Bda2aI "Kurulum Videosu")

##### Uygulamayı sıfırdan öğrenerek geliştirdim. Eksiklerini elimden geldiğince kapatıp olabildiğince sektörde işe yarayacak bir uygulama çıkarmaya çalıştım. Yaklaşık 1 ayı aşan bir sürede tamamlamam mümkün oldu. Kullanmak isteyenler bana ulaşabilirler.

İletişim: kaymayolu.rasyon0k@icloud.com & Instagram | batymeister
