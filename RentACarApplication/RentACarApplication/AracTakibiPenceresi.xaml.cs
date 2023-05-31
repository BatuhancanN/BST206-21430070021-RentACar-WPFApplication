// IsTakibiPenceresi.xaml.cs

using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace RentACarApplication
{
    public partial class AracTakibiPenceresi : Window
    {
        SqlServerBaglanti sqlServerBaglanti = new SqlServerBaglanti();
        public ObservableCollection<Araba> Arabalar { get; set; }

        public AracTakibiPenceresi()
        {
            InitializeComponent();
            Arabalar = new ObservableCollection<Araba>();
            ArabaYukle();
            DataContext = this;
        }

        private void ArabaYukle()
        {
            

            using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
            {
                connection.Open();

                string query = "SELECT * FROM arabalar";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string plaka = reader.GetString(1);
                            string marka = reader.GetString(2);
                            string model = reader.GetString(3);
                            string tip = reader.GetString(4);
                            string renk = reader.GetString(5);
                            string vites = reader.GetString(6);
                            string yakit = reader.GetString(7);
                            decimal trip = reader.GetDecimal(8);
                            string musaitlik = reader.GetString(9);
                            decimal fiyat = reader.GetDecimal(10);

                            Araba araba = new Araba(id, plaka, marka, model, tip, renk, vites, yakit, trip, musaitlik, fiyat);
                            Arabalar.Add(araba);
                        }
                    }
                }
            }

            arabaListesi.ItemsSource = Arabalar;
        }
    }
}
