using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace objekty
{
    public class court : INotifyPropertyChanged
    {
        string courtid; //PK redundant i guess
        public string CourtID { get { return courtid; } }

        string desc = "";
        public string Description { get { return desc; }
            set {
                SetField(ref desc, value);
            }
        }

        //cannot be null or negative (maybe later)
        int price;
        public int Price { get { return price; }
            set { 
                SetField(ref price, value);
            }
        }

        //must be before closes
        int opens;
        public int Opens { 
            get { 
                return opens; 
            } 
            set { 
                if(value < 24 && value < closes) SetField(ref opens, value);
            } 
        }
        //must be after opens
        int closes;
        public int Closes { 
            get { 
                return closes; 
            } 
            set { 
                if(value < 24 && value > opens) SetField(ref  closes, value);   
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void SetField(ref string field, string value, [CallerMemberName] string caller = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        private void SetField(ref int field, int value, [CallerMemberName] string caller = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }



        public court(string courtid, string desc, int price, int opens, int closes)
        {
            this.courtid = courtid;
            this.Description = desc;
            this.Price = price;
            
            this.Closes = closes;
            //becease of property input constraints
            this.Opens = opens;
        }


        public override string ToString()
        {
            return $"court: {courtid} {Description} {Price} {Opens} {Closes}";
        }

    }
}
