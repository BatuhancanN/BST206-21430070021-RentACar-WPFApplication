using System.Collections.ObjectModel;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System;
using System.Globalization;

namespace RentACarApplication
{
    public partial class MainWindow : Window
    {
        SqlServerBaglanti sqlServerBaglanti = new SqlServerBaglanti();
        private ObservableCollection<Araba> arabalar;

        public MainWindow()
        {
            InitializeComponent();
            arabalar = new ObservableCollection<Araba>();
            ArabaYukle();
            DataContext = this;
        }

        private void ArabaYukle()
        {
            using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
            {
                connection.Open();

                string query = "SELECT * FROM arabalar where musaitlik <> 'ATIL DURUMDA'";
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
                            arabalar.Add(araba);
                        }
                    }
                }
            }

            arabaListesi.ItemsSource = arabalar;
        }

        public Araba SeciliAraba { get; set; }

        private void arabaListesi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SeciliAraba = (Araba)arabaListesi.SelectedItem;
        }

        private void ekleButonu_Click(object sender, RoutedEventArgs e)
        {
            EklePenceresi eklePenceresi = new EklePenceresi();
            eklePenceresi.ShowDialog();
            arabalar.Clear();
            ArabaYukle();
        }

        private void guncelleButonu_Click(object sender, RoutedEventArgs e)
        {
            if (SeciliAraba != null)
            {
                GuncellemePenceresi guncellemePenceresi = new GuncellemePenceresi(SeciliAraba);
                guncellemePenceresi.ShowDialog();
                arabalar.Clear();
                ArabaYukle();
            }
            else
            {
                MessageBox.Show("Lütfen bir araç seçin.");
            }
        }

        private void silButonu_Click(object sender, RoutedEventArgs e)
        {
            if (SeciliAraba != null)
            {
                MessageBoxResult result = MessageBox.Show("Silme işlemini onaylıyor musunuz?\nGeri döndürmek için teknik yardım almanız gerekebilir!", $"{SeciliAraba.Plaka} plakalı aracı silme onayı", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                        {
                            connection.Open();

                            string query = "UPDATE arabalar set musaitlik='ATIL DURUMDA' WHERE Id=@id";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@id", SeciliAraba.Id);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    arabalar.Clear();
                                    ArabaYukle();
                                    MessageBox.Show("Araç başarıyla silindi.\nVeri kaybına sebep olmamak için gerekli düzenlemeler yapıldı.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            }
            else MessageBox.Show("Lütfen bir araç seçin.");
        }

        private void kiralaButonu_Click(object sender, RoutedEventArgs e)
        {
            if (SeciliAraba != null)
            {
                if (SeciliAraba.Musaitlik == "Müsait")
                {
                    KiralaPenceresi kiralaPenceresi = new KiralaPenceresi();
                    kiralaPenceresi.SeciliAraba = SeciliAraba;
                    kiralaPenceresi.ShowDialog();

                    if (kiralaPenceresi.check)
                    {
                        string query1 = "UPDATE arabalar SET musaitlik = 'Kiraya Verildi' WHERE ID = @ID";

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand(query1, connection);
                                command.Parameters.AddWithValue("@Id", SeciliAraba.Id);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    decimal onOdeme = kiralaPenceresi.gun * SeciliAraba.Fiyat;

                                    string query2 = "INSERT INTO Kiralamalar (ArabaID, MusteriIDN, MusteriAdi, MusteriSoyadi, KiraSuresi, OnOdeme, Tarih) VALUES (@ArabaID, @MusteriIDN, @MusteriAdi, @MusteriSoyadi, @KiraSuresi, @OnOdeme, @Tarih)";

                                    command = new SqlCommand(query2, connection);

                                    command.Parameters.AddWithValue("@ArabaID", SeciliAraba.Id);
                                    command.Parameters.AddWithValue("@MusteriIDN", kiralaPenceresi.musteriID);
                                    command.Parameters.AddWithValue("@MusteriAdi", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kiralaPenceresi.musteriAd.Trim()));
                                    command.Parameters.AddWithValue("@MusteriSoyadi", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(kiralaPenceresi.musteriSoyad.Trim()));
                                    command.Parameters.AddWithValue("@KiraSuresi", kiralaPenceresi.gun);
                                    command.Parameters.AddWithValue("@OnOdeme", onOdeme);
                                    command.Parameters.AddWithValue("@Tarih", DateTime.Now);


                                    rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        arabalar.Clear();
                                        ArabaYukle();
                                        MessageBox.Show("Araç kiraya verildi ve kiralama bilgileri kaydedildi.");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                }
                else MessageBox.Show("Bu araç kirada.");
                
            }
            else MessageBox.Show("Lütfen bir araç seçin.");
        }

        private void teslimatButonu_Click(object sender, RoutedEventArgs e)
        {
            if (SeciliAraba != null)
            {
                if (SeciliAraba.Musaitlik == "Kiraya Verildi")
                {
                    TeslimatPenceresi teslimatPenceresi = new TeslimatPenceresi();
                    teslimatPenceresi.SeciliAraba = SeciliAraba;
                    teslimatPenceresi.ShowDialog();

                    if (teslimatPenceresi.check)
                    {
                        string query1 = "UPDATE arabalar SET musaitlik = 'İncelemede' WHERE ID = @ID";

                        try
                        {
                            using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                            {
                                connection.Open();

                                SqlCommand command = new SqlCommand(query1, connection);
                                command.Parameters.AddWithValue("@ID", SeciliAraba.Id);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    string query2 = "UPDATE arabalar SET Trip = @Trip WHERE ID = @ID";

                                    command = new SqlCommand(query2, connection);

                                    command.Parameters.AddWithValue("@ID", SeciliAraba.Id);
                                    command.Parameters.AddWithValue("@Trip", Decimal.Parse(teslimatPenceresi.yeniTripBox.Text, CultureInfo.InvariantCulture));

                                    rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        arabalar.Clear();
                                        ArabaYukle();
                                        MessageBox.Show("Araç teslim alındı ve bilgileri kaydedildi.");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Bu araç kirada değil.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir araç seçin.");
            }
        }

        private void hazirButonu_Click(object sender, RoutedEventArgs e)
        {
            if(SeciliAraba != null)
            {
                if(SeciliAraba.Musaitlik == "İncelemede" || SeciliAraba.Musaitlik == "Bakımda")
                {
                    string query1 = "UPDATE arabalar SET musaitlik = 'Müsait' WHERE ID = @ID";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                        {
                            connection.Open();

                            SqlCommand command = new SqlCommand(query1, connection);
                            command.Parameters.AddWithValue("@Id", SeciliAraba.Id);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                arabalar.Clear();
                                ArabaYukle();
                                MessageBox.Show("Araç müsait durumuna alındı.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
                else MessageBox.Show("Bu araç boşa çıkarmak için uygun değil.");
            }
            else MessageBox.Show("Lütfen bir araç seçin.");
        }

        private void bakimButonu_Click(object sender, RoutedEventArgs e)
        {
            if (SeciliAraba != null)
            {
                if (SeciliAraba.Musaitlik != "Kiraya Verildi")
                {
                    string query1 = "UPDATE arabalar SET musaitlik = 'Bakımda' WHERE ID = @ID";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                        {
                            connection.Open();

                            SqlCommand command = new SqlCommand(query1, connection);
                            command.Parameters.AddWithValue("@Id", SeciliAraba.Id);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                arabalar.Clear();
                                ArabaYukle();
                                MessageBox.Show("Araç bakıma alındı.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
                else MessageBox.Show("Bu araç bakım için uygun değil.");
            }
            else MessageBox.Show("Lütfen bir araç seçin.");
        }

        private void listeleButonu_Click(object sender, RoutedEventArgs e)
        {
            IsTakibiPenceresi isTakibiPenceresi = new IsTakibiPenceresi();
            isTakibiPenceresi.ShowDialog();
        }

        private void listele2Butonu_Click(object sender, RoutedEventArgs e)
        {
            AracTakibiPenceresi aracTakibiPenceresi = new AracTakibiPenceresi();
            aracTakibiPenceresi.ShowDialog();
        }
    }

    public class Araba : INotifyPropertyChanged
    {
        private string _plaka;
        private string _marka;
        private string _model;
        private string _tip;
        private string _renk;
        private string _vites;
        private string _yakit;
        private decimal _trip;
        private decimal _fiyat;
        private string _musaitlik;

        public int Id { get; set; }

        public string Plaka
        {
            get { return _plaka; }
            set
            {
                if (_plaka != value)
                {
                    _plaka = value;
                    OnPropertyChanged("Plaka");
                }
            }
        }

        public string Marka
        {
            get { return _marka; }
            set
            {
                if (_marka != value)
                {
                    _marka = value;
                    OnPropertyChanged("Marka");
                }
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        public string Tip
        {
            get { return _tip; }
            set
            {
                if (_tip != value)
                {
                    _tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }

        public string Renk
        {
            get { return _renk; }
            set
            {
                if (_renk != value)
                {
                    _renk = value;
                    OnPropertyChanged("Renk");
                }
            }
        }

        public string Vites
        {
            get { return _vites; }
            set
            {
                if (_vites != value)
                {
                    _vites = value;
                    OnPropertyChanged("Vites");
                }
            }
        }

        public string Yakit
        {
            get { return _yakit; }
            set
            {
                if (_yakit != value)
                {
                    _yakit = value;
                    OnPropertyChanged("Yakit");
                }
            }
        }

        public decimal Trip
        {
            get { return _trip; }
            set
            {
                if (_trip != value)
                {
                    _trip = value;
                    OnPropertyChanged("Trip");
                }
            }
        }

        public string Musaitlik
        {
            get { return _musaitlik; }
            set
            {
                if (_musaitlik != value)
                {
                    _musaitlik = value;
                    OnPropertyChanged("Musaitlik");
                }
            }
        }

        public decimal Fiyat
        {
            get { return _fiyat; }
            set
            {
                if (_fiyat != value)
                {
                    _fiyat = value;
                    OnPropertyChanged("Fiyat");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Araba(int id, string plaka, string marka, string model, string tip, string renk, string vites, string yakit, decimal trip, string musaitlik, decimal fiyat)
        {
            Id = id;
            Plaka = plaka;
            Marka = marka;
            Model = model;
            Tip = tip;
            Renk = renk;
            Vites = vites;
            Yakit = yakit;
            Trip = trip;
            Musaitlik = musaitlik;
            Fiyat = fiyat;
        }
    }
}
