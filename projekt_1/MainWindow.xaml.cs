using domain;
using objekty;
using orm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projekt_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public logic_domain1 logic_Domain { get; set; }
        public MainWindow()
        {

            InitializeComponent();

            string path = "../../../../objekty/bin/Debug/net6.0/objekty.dll";
            string db_source = "Data Source=../../../../mydb.db";
            logic_Domain = new logic_domain1(path, db_source);

            //user jenda1 = new user("jen01", "jenheslo", "Jenda1", "Nejen1", "nejendajedna@centrum.cz");
            //logic_Domain.add_user(jenda1);

            //court kurt1 = new court("kur01", "Kurt na floorbal", 100, 8, 20);
            //logic_Domain.add_court(kurt1);

            //reservation rezervace1 = new reservation("kur01", DateTime.Now, 100, "jen01");
            //logic_Domain.add_reservation(rezervace1);
            //reservation rezervace2 = new reservation("kur01", DateTime.Now.AddSeconds(2), 100, "jen01");
            //logic_Domain.add_reservation(rezervace2);

            //user jenda2 = new user("jen02", "jenheslo", "Jenda2", "Nejen2", "nejendadva@centrum.cz");
            //logic_Domain.add_user(jenda2);

            //court kurt2 = new court("kur02", "Kurt na hokej", 100, 8, 20);
            //logic_Domain.add_court(kurt2);

            //reservation rezervace3 = new reservation("kur02", DateTime.Now, 100, "jen02");
            //logic_Domain.add_reservation(rezervace3);
            //reservation rezervace4 = new reservation("kur02", DateTime.Now.AddSeconds(2), 100, "jen02");
            //logic_Domain.add_reservation(rezervace4);



            this.DataContext = logic_Domain;
        }

        private void edit_user(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("edit user");
            //Button btn = sender as Button;
            UserWindow uw = new UserWindow((user)this.users.SelectedItem,this);
            uw.save_user += x => this.logic_Domain.update_user(x);
            uw.ShowDialog();
        }
        private void new_user(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("new user");
            UserWindow uw = new UserWindow(this);
            //cw.Show(); //obyčejne okono
            uw.save_user += x => this.logic_Domain.add_user(x);
            uw.ShowDialog();
        }


        private void edit_court(object sender, RoutedEventArgs e)
        {
            CourtWindow cw = new CourtWindow((court)this.courts.SelectedItem,this);
            cw.save_court += x => this.logic_Domain.update_court(x);
            cw.ShowDialog();
        }
        private void new_court(object sender, RoutedEventArgs e)
        {
            CourtWindow cw = new CourtWindow(this);
            cw.save_court += x => this.logic_Domain.add_court(x);
            cw.ShowDialog();
        }



		private void edit_reservation(object sender, RoutedEventArgs e)
		{
            ReservationWindow rw = new ReservationWindow((reservation)this.reservations.SelectedItem, this);
            rw.save_reservation += x => this.logic_Domain.update_reservation(x);
            rw.ShowDialog();
		}

        private void delete_reservation(object sender, RoutedEventArgs e)
        {
            this.logic_Domain.remove_reservation((reservation)this.reservations.SelectedItem);
        }

		private void new_reservation(object sender, RoutedEventArgs e)
		{
            ReservationWindow rw = new ReservationWindow(this);
            rw.save_reservation += x => this.logic_Domain.add_reservation(x);
            rw.ShowDialog();
		}
	}
}
