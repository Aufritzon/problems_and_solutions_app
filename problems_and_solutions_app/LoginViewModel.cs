using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace problems_and_solutions_app
{
    public class LoginViewModel : ViewModelBase

    {
        private bool _canLogin;
        public bool CanLogin
        {
            get { return _canLogin; }
            set
            {
                _canLogin = value;
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public MySqlDatabase Database { get; set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                UpdateCanLogin();
            }
        } 


        private string _password;
        public string Password 
        {
            private get 
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                UpdateCanLogin();
            }
        }

        public LoginViewModel()
        {
            Database = new MySqlDatabase();
        }


        public ICommand LoginCommand => new RelayCommand(Login);
        
        private void Login()
        {
            if (Database.IsValidUserCredentials(Username, Password))
            {
                ViewModelLocator.MainViewModel.CurrentPage = new UserView(Username);
            }
            else
            {
                MessageBox.Show("Are you sure you're registered? ´Cause I can´t find you!", "Ooops", MessageBoxButton.OK);
            }

        }

        private void UpdateCanLogin()
        {
           CanLogin = !string.IsNullOrEmpty(Username)  && !string.IsNullOrEmpty(Password);
        }




    }
}
