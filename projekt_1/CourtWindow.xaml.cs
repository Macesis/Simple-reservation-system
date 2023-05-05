using domain;
using objekty;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace projekt_1
{
    public delegate void SaveCourt(court u);
    /// <summary>
    /// Interakční logika pro CourtWindow.xaml
    /// </summary>
    public partial class CourtWindow : Window
    {
        public court tbc_court { get; set; }

        public string court_name { get; set; }
        bool create = true;

        public event SaveCourt save_court;

        public logic_domain1 domain1 { get; set; }

        public CourtWindow(MainWindow main)
        {
            InitializeComponent();
            this.domain1 = main.logic_Domain;

            this.DataContext = this;

            TextBox tb = this.CourtIDTextBox;
            tb.IsEnabled = true;
        }

        public CourtWindow(court c, MainWindow main)
        {
            InitializeComponent();

            tbc_court = c;
            court_name = c.CourtID;
            DescriptionTextBox.Text = c.Description;
            PriceTextBox.Text = c.Price.ToString();
            OpensTextBox.Text = c.Opens.ToString();
            ClosesTextBox.Text = c.Closes.ToString();

            this.domain1 = main.logic_Domain;

            this.DataContext = this;

            TextBox tb = this.CourtIDTextBox;
            tb.IsEnabled = false;

            create = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int open_time = int.Parse(OpensTextBox.Text);
            int close_time = int.Parse(ClosesTextBox.Text);
            if (open_time > 23 || open_time < 0 || close_time > 23 || close_time < 0)
            {
                MessageBox.Show("Čas musí být v rozmezí 0-23!");
                return;
            }

            if(open_time >= close_time) {
                MessageBox.Show("Kurt musí otevírat dříve než zavře!");
                return;
            }
            int price = int.Parse(PriceTextBox.Text);
            if(price < 0)
            {
                MessageBox.Show("Cena musí být kladná!");
                return;
            }

            //foreach(user u in )


            if (create)
            {
                foreach(court c in domain1.courts)
                {
                    if(c.CourtID == this.court_name)
                    {
                        MessageBox.Show("Kurt s tímto názvem již existuje");
                        return;
                    }
                }

                tbc_court = new court(CourtIDTextBox.Text, DescriptionTextBox.Text, price, open_time, close_time);
                save_court(tbc_court);
            }
            else
            {
                tbc_court.Description = DescriptionTextBox.Text;
                tbc_court.Price = int.Parse(PriceTextBox.Text);
                tbc_court.Opens = int.Parse(OpensTextBox.Text);
                tbc_court.Closes = int.Parse(ClosesTextBox.Text);
                save_court(tbc_court);
            }

            //save_court?.Invoke(tbc_court);
            this.Close();
        }
    }
}
