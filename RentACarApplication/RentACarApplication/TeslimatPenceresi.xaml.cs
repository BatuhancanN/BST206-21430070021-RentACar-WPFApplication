using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RentACarApplication
{
    /// <summary>
    /// TeslimatPenceresi.xaml etkileşim mantığı
    /// </summary>
    public partial class TeslimatPenceresi : Window
    {
        public Araba SeciliAraba { get; set; }
        public decimal yeniTrip {get; set;}
        public bool check { get; set; }
        public TeslimatPenceresi()
        {
            InitializeComponent();
            check = false;
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
            if (!string.IsNullOrEmpty(yeniTripBox.Text))
            {
                decimal yeniTrip;
                if (decimal.TryParse(yeniTripBox.Text, out yeniTrip))
                {
                    if (yeniTrip > SeciliAraba.Trip)
                    {
                        check = true;
                        this.yeniTrip = yeniTrip;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Yeni trip bilgisi, eskisinden küçük olamaz.");
                    }
                }
                else
                {
                    MessageBox.Show("Geçersiz yeni trip bilgisi!");
                }
            }
            else
            {
                MessageBox.Show("Lütfen yeni trip bilgisini boş bırakmayın.");
            }
        }

    }
}
