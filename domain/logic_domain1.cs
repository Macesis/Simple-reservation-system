using orm;
using objekty;
using System.Collections.ObjectModel;

namespace domain
{
    public class logic_domain1
    {
        async_mapper1 mapper1;

        public ObservableCollection<court> courts { get; set; }
        public ObservableCollection<user> users { get; set; }
        public ObservableCollection<reservation> reservations { get; set; }

        public logic_domain1(string path, string db_source)
        {
            mapper1 = new async_mapper1(path, db_source);
            if (mapper1.test() == false) throw new System.Exception("mapper1 not connected");


            mapper1.create_tables();
            fill_domain();

        }

        public async void fill_domain()
        {
            courts = new ObservableCollection<court>((await mapper1.readall("court")).Cast<court>().ToList());
            users = new ObservableCollection<user>((await mapper1.readall("user")).Cast<user>().ToList());
            reservations = new ObservableCollection<reservation>((await mapper1.readall("reservation")).Cast<reservation>().ToList());
        }

        //create 
        public async void add_court(court c)
        {
            courts.Add(c);
            await mapper1.create(c);
        }

        public async void add_user(user u)
        {
            users.Add(u);
            await mapper1.create(u);
        }

        public async void add_reservation(reservation r)
        {
            reservations.Add(r);
            await mapper1.create(r);
        }

        //delete
        public async void remove_reservation(reservation r)
        {
            reservations.Remove(r);
            await mapper1.delete(r);
        }

        //update
        public async void update_court(court c)
        {
            await mapper1.update(c);
        }

        public async void update_user(user u)
        {
            await mapper1.update(u);
        }

        public async void update_reservation(reservation r)
        {
            await mapper1.update(r);
        }


        public court? get_court(string id)
        {
			return courts.Where(c => c.CourtID == id).FirstOrDefault();
		}

        public user? get_user(string id)
        {
			return users.Where(u => u.UserID == id).FirstOrDefault();
		}

        //----------------------------- web part -----------------------------
        public List<reservation> get_reservations_uid(string userid)
        {
			return reservations.Where(r => r.User == userid).ToList();
		}

        public List<reservation> get_reservations_cid(string courtid)
        { 
            return reservations.Where(r => r.CourtID == courtid).ToList();
        }

        public reservation? get_reservation(string courtid, DateTime date)
        {
            return reservations.Where(r => r.CourtID == courtid && r.DateID == date).FirstOrDefault();
        }

        public void reserve(string courtid, DateTime date, int price, string userid)
        {
            if(reservations.Any(x => x.CourtID == courtid && x.DateID == date))
            {
				throw new Exception("Rezervace na tuto dobu již existuje!");
			}

            if(date < DateTime.Now)
            {
                throw new Exception("Nelze rezervovat v minulosti!");
            }

			reservation r = new reservation(courtid, date, price, userid);
			add_reservation(r);
		}



		//----------------------------- web part -----------------------------

		public void print_courts()
        {
            Console.WriteLine("--------- courts: ---------");
            foreach (court c in courts)
            {
                System.Console.WriteLine(c);
            }
        }

        public void print_users()
        {
            Console.WriteLine("--------- users: ---------");
            foreach (user u in users)
            {
                System.Console.WriteLine(u);
            }
        }

        public void print_reservation()
        {
            Console.WriteLine("--------- reservations: ---------");
            foreach (reservation r in reservations)
            {
                System.Console.WriteLine(r);
            }
        }

        public void print_all()
        {
            print_courts();
            print_users();
            print_reservation();
        }
    }
}