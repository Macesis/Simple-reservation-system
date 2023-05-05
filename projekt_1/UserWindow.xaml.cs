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
    public delegate void SaveUser(user u);
    /// <summary>
    /// Interakční logika pro UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        //to be created user
        public user tbc_user { get; set; }

        public string user_name { get; set; }


        bool create = true;

        public event SaveUser save_user;

        public logic_domain1 Domain1 { get; set; }
        
        //create constructor
        public UserWindow(MainWindow main)
        {
            InitializeComponent();

            this.Domain1 = main.logic_Domain;

            this.DataContext = this;

            TextBox tb = this.UserIDTextBox;
            tb.IsEnabled = true;
        }

        //update constructor
        public UserWindow(user u, MainWindow main)
        {
            InitializeComponent();
            tbc_user = u;
            user_name = u.UserID;
            PasswordTextBox.Text = u.Password;
            FirstNameTextBox.Text = u.FirstName;
            LastNameTextBox.Text = u.LastName;
            EmailTextBox.Text = u.Email;

            this.Domain1 = main.logic_Domain;

            this.DataContext = this;

            TextBox tb = this.UserIDTextBox;
            tb.IsEnabled = false;

            create = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (create)
            {
                foreach (user u in this.Domain1.users)
                {
                    if (u.UserID == this.user_name)
                    {
                        MessageBox.Show("Uživatel s tímto přihlašovacím jménem už existuje");
                        return;
                    }
                }
                tbc_user = new user(UserIDTextBox.Text, PasswordTextBox.Text, FirstNameTextBox.Text, LastNameTextBox.Text, EmailTextBox.Text);
            }
            else
            {
                tbc_user.Password = PasswordTextBox.Text;
                tbc_user.FirstName = FirstNameTextBox.Text;
                tbc_user.LastName = LastNameTextBox.Text;
                tbc_user.Email = EmailTextBox.Text;
            }

            save_user?.Invoke(tbc_user);
            this.Close();
        }
    }
}
