using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace RentACarApplication
{
    public partial class GuncellemePenceresi : Window
    {
        SqlServerBaglanti sqlServerBaglanti = new SqlServerBaglanti();
        private Araba seciliAraba;

        public GuncellemePenceresi(Araba araba)
        {
            InitializeComponent();
            seciliAraba = araba;
            DataContext = seciliAraba;
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
                decimal trip;
                decimal fiyat;

                if (!Decimal.TryParse(tripBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out trip))
                {
                    MessageBox.Show("Geçersiz TRİP değeri!", "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Decimal.TryParse(fiyatBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out fiyat))
                {
                    MessageBox.Show("Geçersiz fiyat değeri!", "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (plaka.Length > 9)
                {
                    MessageBox.Show("Geçersiz plaka formatı! Plaka en fazla 9 karakter olmalıdır.", "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlServerBaglanti.baglanti()))
                    {
                        connection.Open();

                        string query = "UPDATE Arabalar SET Plaka=@plaka, Marka=@marka, Model=@model, Tip=@tip, Renk=@renk, Vites=@vites, Yakit=@yakit, Trip=@trip, Musaitlik=@musaitlik, Fiyat=@fiyat WHERE Id=@id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@plaka", plaka);
                            command.Parameters.AddWithValue("@marka", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(markaBox.Text.Trim()));
                            command.Parameters.AddWithValue("@model", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modelBox.Text.Trim()));
                            command.Parameters.AddWithValue("@tip", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tipBox.Text.Trim()));
                            command.Parameters.AddWithValue("@renk", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(renkBox.Text.Trim()));
                            command.Parameters.AddWithValue("@vites", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vitesBox.Text.Trim()));
                            command.Parameters.AddWithValue("@yakit", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(yakitBox.Text.Trim()));
                            command.Parameters.AddWithValue("@trip", trip);
                            command.Parameters.AddWithValue("@musaitlik", seciliAraba.Musaitlik);
                            command.Parameters.AddWithValue("@fiyat", fiyat);
                            command.Parameters.AddWithValue("@id", seciliAraba.Id);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Close();
                                MessageBox.Show("Araç bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Araç bilgileri güncellenirken bir hata oluştu.", "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message, "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm girişleri doldurun.", "Hatalı İşlem Tespit Edildi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}