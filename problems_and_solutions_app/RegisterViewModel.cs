using GalaSoft.MvvmLight.Command;
using MySqlConnector;
using System.Data;
using System;
using System.Windows.Input;
using System.Windows;
using System.Security.Cryptography;

namespace problems_and_solutions_app
{
    public class RegisterViewModel : ViewModelBase
    {

        private string _messageToUser;
        public string MessageToUser
        {
            get { return _messageToUser; }
            set
            {
                _messageToUser = value;
                OnPropertyChanged(nameof(MessageToUser));
            } 
        } 

        private string _messageToUserColor = "Red";
        public string MessageToUserColor
        {
            get { return _messageToUserColor; }
            set
            {
                _messageToUserColor = value;
                OnPropertyChanged(nameof(MessageToUserColor));
            }
        }


        private bool _canRegister;
        public bool CanRegister
        {
            get { return _canRegister; }
            set
            {
                _canRegister = value;
                OnPropertyChanged(nameof(CanRegister));
            }
        }


        private string _username;

        public string Username
        {
            get { return _username; }
            set 
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
                UpdateCanRegister();
                UpdateMessageToUser();

            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                UpdateCanRegister();
                UpdateMessageToUser();

            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                UpdateCanRegister();
                UpdateMessageToUser();

            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                UpdateCanRegister();
                UpdateMessageToUser();

            }
        }


        public ICommand RegisterCommand => new RelayCommand(Register);

        private void Register()
        {
            var database = new MySqlDatabase();

            if (database.UserExists(Username))
            {
                MessageBox.Show("Sorry, that username is taken, please try another.");
                Username = string.Empty;
            } else
            {
                database.AddUser(Username, Email, Password);
                ViewModelLocator.MainViewModel.CurrentPage = new LoginRegisterView();
            }

        }

        private void UpdateCanRegister()
        { 
            CanRegister = FieldsAreFilled() && Password == ConfirmPassword;
        }


        private bool FieldsAreFilled()
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword);
        }

        private void UpdateMessageToUser()
        {
            if(!FieldsAreFilled())
            {
                MessageToUser = "Form Incomplete, please fill all of the fields.";
                MessageToUserColor = "Red";
                return;
            } 
            
            if(Password != ConfirmPassword)
            {
                MessageToUser = "Passwords do not match.";
                MessageToUserColor = "Red";
                return;
            }

            MessageToUser = "Everything seems fine, please press register :D";
            MessageToUserColor = "Green";

        }


    }
}