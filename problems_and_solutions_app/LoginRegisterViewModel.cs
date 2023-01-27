using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace problems_and_solutions_app
{
    public class LoginRegisterViewModel : ViewModelBase
    {
        public UserControl _currentView;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public LoginRegisterViewModel() 
        {
            CurrentView = new LoginView();
        }

        public ICommand NavigateRegisterView => new RelayCommand(() => CurrentView = new RegisterView());

        public ICommand NavigateLoginView => new RelayCommand(() => CurrentView = new LoginView());


        

    }
}
