using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace objekty
{
    public class user : INotifyPropertyChanged
    {
        string userid = "";  //PK
        public string UserID { get { return userid; } }

        string pwd = "";
        public string Password { get { return pwd; } 
            set {
                SetField(ref pwd, value);
            } 
        }

        string fname = "";
        public string FirstName { get { return fname; }
            set { 
                SetField(ref fname, value);
            }
        }

        string lname = "";
        public string LastName { get { return lname; }
            set { 
                SetField(ref lname, value);
            }
        } 

        //maybe add format check later
        string email = "";
        public string Email { get { return email; }  
            set { 
                //if(IsValidEmail(email)) { email = value; }
                //email = value;
                SetField(ref email, value);
                //else throw exception?
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void SetField(ref string field, string value, [CallerMemberName] string caller = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }



        public user(string userid, string pwd, string fname, string lname, string email)
        {
            this.userid = userid;
            this.Password = pwd;
            this.FirstName = fname;
            this.LastName = lname;
            this.Email = email;
        }

        public override string ToString()
        {
            return $"user: {userid} {Password} {FirstName} {LastName} {Email}";
        }
    }
}