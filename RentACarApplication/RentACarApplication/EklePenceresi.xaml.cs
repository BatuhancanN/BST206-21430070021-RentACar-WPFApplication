using System;
using System.Windows;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Input;

namespace RentACarApplication
{
    public partial class EklePenceresi : Window
    {
        SqlServerBaglanti sqlServerBaglanti = new SqlServerBaglanti();
        public EklePenceresi()
        {
            InitializeComponent();
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                kaydetButonu.Focus();
                kaydetButonu_Click(sender, e);
            }
        }

        private void kaydetButonu_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(plakaBox.Text) && !string.IsNullOrEmpty(markaBox.Text) && !string.IsNullOrEmpty(modelBox.Text) &&
                !string.IsNullOrEmpty(tipBox.Text) && !string.IsNullOrEmpty(renkBox.Text) && !string.IsNullOrEmpty(vitesBox.Text) &&
                !string.IsNullOrEmpty(yakitBox.Text) && !string.IsNullOrEmpty(tripBox.Text) && !string.IsNullOrEmpty(fiyatBox.Text))
            {
                string plaka = plakaBox.Text.Trim().Replace(" ", "").ToUpperInvariant();
                string marka = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(markaBox.Text.Trim());
                string model = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modelBox.Text.Trim());
                string tip = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tipBox.Text.Trim());
                string renk = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(renkBox.Text.Trim());
                string vites = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vitesBox.Text.Trim());
                string yakit = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(yakitBox.Text.Trim());
                decimal trip;
                decimal fiyat;

                if (!Decimal.TryParse(tripBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out trip))
                {
                    MessageBox.Show("Geçersiz TRİP değeri!");
                    return;
                }

                if (!Decimal.TryParse(fiyatBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out fiyat))
                {
                    MessageBox.Show("Geçersiz fiyat değeri!");
                    return;
                }

                if (plaka.Length > 9)
                {
                    MessageBox.Show("Geçersiz plaka formatı! Plaka en fazla 9 karakter olmalıdır.");
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                    {
                        connection.Open();

                        string query = "INSERT INTO arabalar (plaka, marka, model, tip, renk, vites, yakit, trip, musaitlik, fiyat) VALUES (@Plaka, @Marka, @Model, @Tip, @Renk, @Vites, @Yakit, @Trip, 'Müsait', @Fiyat)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Plaka", plaka);
                            command.Parameters.AddWithValue("@Marka", marka);
                            command.Parameters.AddWithValue("@Model", model);
                            command.Parameters.AddWithValue("@Tip", tip);
                            command.Parameters.AddWithValue("@Renk", renk);
                            command.Parameters.AddWithValue("@Vites", vites);
                            command.Parameters.AddWithValue("@Yakit", yakit);
                            command.Parameters.AddWithValue("@Trip", trip);
                            command.Parameters.AddWithValue("@Fiyat", fiyat);

                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                this.Close();
                                MessageBox.Show("Araba başarıyla kaydedildi.");
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
                MessageBox.Show("Lütfen tüm girişleri doldurun.");
            }
        }
    }
}
