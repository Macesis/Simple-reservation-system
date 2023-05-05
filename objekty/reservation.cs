using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace objekty
{
    public class reservation
    {

        //---------------------------------------------------- Primary key
        //court court; //FK
        string courtid;
        public string CourtID { get { return courtid; } }

        DateTime date;
        public DateTime DateID { get { return date; } }
        //must be between court.opens and court.closes
        //---------------------------------------------------- Primary key

        public int Price { get; set; }

        //user user; //FK
        public string User { get; set; }

        public reservation(string courtid, DateTime date, int Price, string userid)
        {
           
            this.courtid = courtid;
            this.date = date;

            this.Price = Price;
            this.User = userid;
        }

        public override string ToString()
        {
            return $"reservation: {courtid} {date} {Price} {User}";
        }
    }
}
