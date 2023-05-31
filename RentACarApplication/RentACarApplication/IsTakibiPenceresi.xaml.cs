// IsTakibiPenceresi.xaml.cs

using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.IO;

namespace RentACarApplication
{
    public partial class IsTakibiPenceresi : Window
    {
        SqlServerBaglanti sqlServerBaglanti = new SqlServerBaglanti();
        public ObservableCollection<Kiralama> Kiralamalar { get; set; }

        public IsTakibiPenceresi()
        {
            InitializeComponent();
            Kiralamalar = new ObservableCollection<Kiralama>();
            KiraYukle();
            DataContext = this;
        }

        private void KiraYukle()
        {
            string query = "SELECT * FROM kiralamalar";

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                int arabaID = reader.GetInt32(1);
                                string musteriIDN = reader.GetString(2);
                                string musteriAdi = reader.GetString(3);
                                string musteriSoyadi = reader.GetString(4);
                                int kiraSuresi = reader.GetInt32(5);
                                decimal onOdeme = reader.GetDecimal(6);
                                DateTime tarih = reader.GetDateTime(7);

                                Kiralama kiralama = new Kiralama(id, arabaID, musteriIDN, musteriAdi, musteriSoyadi, kiraSuresi, onOdeme, tarih);
                                Kiralamalar.Add(kiralama);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Hata: " + ex.Message);
            }
            kiralamaListesi.ItemsSource = Kiralamalar;
        }
    }

    public class Kiralama
    {
        public int ID { get; set; }
        public int ArabaID { get; set; }
        public string MusteriIDN { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }
        public int KiraSuresi { get; set; }
        public decimal OnOdeme { get; set; }
        public DateTime Tarih { get; set; }

        public Kiralama(int id, int arabaID, string musteriIDN, string musteriAdi, string musteriSoyadi, int kiraSuresi, decimal onOdeme, DateTime tarih)
        {
            ID = id;
            ArabaID = arabaID;
            MusteriIDN = musteriIDN;
            MusteriAdi = musteriAdi;
            MusteriSoyadi = musteriSoyadi;
            KiraSuresi = kiraSuresi;
            OnOdeme = onOdeme;
            Tarih = tarih;
        }
    }
}
