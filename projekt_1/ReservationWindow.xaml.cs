using domain;
using objekty;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public delegate void SaveReservation(reservation r);

	/// <summary>
	/// Interakční logika pro ReservationWindow.xaml
	/// </summary>
	/// 
	public partial class ReservationWindow : Window
	{
		//to be created reservation
		public reservation tbc_reservation { get; set; }

		string sel_court;
		public string Selected_Court { get { return sel_court; }
			set
			{
				sel_court = value;
				fill_hours();
			}
		}

		DateTime sel_date;
		public DateTime Selected_Date { get { return sel_date; }
			set
			{
				sel_date = value;
				fill_hours();
			}
		}

		public int Selected_Time { get; set; }

		public string Selected_User { get; set; }

		bool create = true;

		public event SaveReservation save_reservation;

		public logic_domain1 Domain1 { get; set; }

		public ReservationWindow(MainWindow main)
		{
			InitializeComponent();

			this.Domain1 = main.logic_Domain;

			this.CourtComboBox.ItemsSource = this.Domain1.courts.Select(cour => cour.CourtID).ToList();

			Selected_Date = DateTime.Now;
			Selected_Time = DateTime.Now.Hour;

			this.UserComboBox.ItemsSource = this.Domain1.users.Select(u => u.UserID).ToList();

			this.DataContext = this;
		}

		public ReservationWindow(reservation res, MainWindow main)
		{
			InitializeComponent();
			tbc_reservation = res;

			this.Domain1 = main.logic_Domain;

			this.CourtComboBox.ItemsSource = this.Domain1.courts.Select(cour => cour.CourtID).ToList();
			Selected_Court = res.CourtID;

			Selected_Date = res.DateID.Date;
			Selected_Time = res.DateID.Hour;

			this.UserComboBox.ItemsSource = this.Domain1.users.Select(u => u.UserID).ToList();
			Selected_User = res.User;

			this.DataContext = this;
			this.create = false;
		}

		public void fill_hours()
		{
			court? c = this.Domain1.get_court(sel_court);
			if(c == null) { return; }
			List<int> hours = Enumerable.Range(c.Opens, c.Closes - c.Opens).ToList();
			List<int> f_hours = new List<int>();
			//todo remove taken hours
			foreach (int i in hours)
			{
				if(!Domain1.reservations.Any(res => res.DateID.Hour == i && res.DateID.Date == Selected_Date && res.CourtID == c.CourtID && res != tbc_reservation)){
					f_hours.Add(i);
				}
			}
			this.TimeComboBox.ItemsSource = f_hours;
			Selected_Time = f_hours[0];
			TimeComboBox.SelectedIndex = 0;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(Selected_Court == null || Selected_User == null) 
			{
				MessageBox.Show("Neplatná data");
				return; 
			}
			DateTime joined = Selected_Date.Date.AddHours(Selected_Time);
			if(create)
			{
				tbc_reservation = new reservation(Selected_Court, joined, this.Domain1.get_court(sel_court).Price, Selected_User);
			}
			else
			{
				if(tbc_reservation.CourtID != Selected_Court || tbc_reservation.DateID != joined)
				{
					this.Domain1.remove_reservation(tbc_reservation);
					tbc_reservation = new reservation(Selected_Court, joined, this.Domain1.get_court(sel_court).Price, Selected_User);
					this.Domain1.add_reservation(tbc_reservation);
				}
				else
				{
					tbc_reservation.User = Selected_User;
				}
			}
			save_reservation?.Invoke(tbc_reservation);
			this.Close();
		}
	}
}
