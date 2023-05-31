using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// KiralaPenceresi.xaml etkileşim mantığı
    /// </summary>
    public partial class KiralaPenceresi : Window
    {

        public Araba SeciliAraba { get; set; }
        public int gun { get; set; }
        public string musteriID { get; set; }
        public string musteriAd { get; set; }
        public string musteriSoyad { get; set; }
        public bool check { get; set; }
        public KiralaPenceresi()
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
            if(!string.IsNullOrEmpty(tcBox.Text) && !string.IsNullOrEmpty(adBox.Text) && !string.IsNullOrEmpty(soyadBox.Text) && !string.IsNullOrEmpty(gunBox.Text))
            {
                if (tcBox.Text.Length == 11)
                {
                    decimal tutar = int.Parse(gunBox.Text) * SeciliAraba.Fiyat;
                    string tutarFormatted = tutar.ToString("N2");

                    MessageBoxResult result = MessageBox.Show($"Tutar : {tutarFormatted} \nLütfen müşteriden ödemeyi almadan onaylamayın.", "Onay", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        gun = int.Parse(gunBox.Text);
                        musteriID = tcBox.Text;
                        musteriAd = adBox.Text;
                        musteriSoyad = soyadBox.Text;

                        check = true;
                        Close();
                    }
                    else check = false;
                }
                else MessageBox.Show("Lütfen geçerli bir TC no girin.");
            }
            else
            {
                MessageBox.Show("Lütfen tüm girişleri doldurun.");
            }
        }
    }
}
