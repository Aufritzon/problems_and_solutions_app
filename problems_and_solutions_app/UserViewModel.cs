using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace problems_and_solutions_app
{
    public class UserViewModel : ViewModelBase
    {

        private MySqlDatabase database = new MySqlDatabase();

        private bool _canEdit = true;

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                OnPropertyChanged(nameof(CanEdit));
            }
        }


        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private ObservableCollection<Problem> _problems;
        public ObservableCollection<Problem> Problems
        {
            get
            {
                return _problems;
            }
            set
            {
                _problems = value;
                OnPropertyChanged(nameof(Problems));
            }
        }

        private string _newProblemText;

        public string NewProblemText
        {
            get { return _newProblemText; }
            set
            {
                _newProblemText = value;
                OnPropertyChanged(nameof(NewProblemText));
            }
        }

        private string _newSolutionText;
        public string NewSolutionText
        {
            get { return _newSolutionText; }
            set
            {
                _newSolutionText = value;
                OnPropertyChanged(nameof(NewSolutionText));
            }
        }

        private Problem _selectedProblem;
        public Problem SelectedProblem
        {
            get { return _selectedProblem; }
            set
            {
                _selectedProblem = value;
                if (value != null)
                {
                    Solutions = new ObservableCollection<Solution>(database.GetSolutions(value.Id));
                    OnPropertyChanged(nameof(SelectedProblem));
                }
            }
        }


        private Solution _selectedSolution;
        public Solution SelectedSolution
        {
            get { return _selectedSolution; }
            set
            {
                _selectedSolution = value;
                OnPropertyChanged(nameof(SelectedSolution));
            }
        }

        private ObservableCollection<Solution> _solutions;
        public ObservableCollection<Solution> Solutions
        {
            get { return _solutions; }
            set
            {
                _solutions = value;
                OnPropertyChanged(nameof(Solutions));
            }
        }



        //public ICommand NavigateToUserProblemsTable => new RelayCommand(() => CurrentTable = new UserProblemsTable(Username));

        public UserViewModel(string username)
        {
            Username = username;
            Problems = new ObservableCollection<Problem>(database.GetProblems(username));
        }


        private void UpdateProblemsOnCanEdit()
        {
            
            if (CanEdit)
            {
                UpdateProblems();
    }
            else
            {
                UpdateAllProblems();
}

        }

        public ICommand AddProblem => new RelayCommand(() =>
        {

            database.AddProblem(NewProblemText, Username);

            UpdateProblemsOnCanEdit();
        });




        public ICommand AddSolution => new RelayCommand(() =>
        {
            if (SelectedProblem == null)
            {
                System.Windows.MessageBox.Show("Please select a problem");
                return;
            }
            database.AddSolution(NewSolutionText, SelectedProblem.Id, Username);
            UpdateSolutions();
        });


        public ICommand ViewAllProblems => new RelayCommand(() =>
        {
            UpdateAllProblems();
        });

        public ICommand ViewYourProblems => new RelayCommand(() =>
        {
            UpdateProblems();
        });


        public ICommand EditProblem => new RelayCommand(() =>
        {
            if (SelectedProblem == null)
            {
                System.Windows.MessageBox.Show("Please select a problem");
                return;
            }

            database.UpdateProblemText(SelectedProblem.Id, NewProblemText);

            UpdateProblemsOnCanEdit();

        });

        public ICommand DeleteProblem => new RelayCommand(() =>
        {
            if (SelectedProblem == null)
            {
                System.Windows.MessageBox.Show("Please select a problem");
                return;
            }

            database.DeleteProblem(SelectedProblem.Id);

            UpdateProblemsOnCanEdit();
        });

        public ICommand SearchProblem => new RelayCommand(() =>
        {
            if (CanEdit)
            {
                Problems = new ObservableCollection<Problem> (database.GetProblemsByUsernameLike(NewProblemText,Username));
            } else
            {
                Problems = new ObservableCollection<Problem> (database.GetProblemsLike(NewProblemText));
            }
        });

        public ICommand ClearProblem => new RelayCommand(() =>
        {
            NewProblemText= string.Empty;
        });

        public ICommand ViewSolution => new RelayCommand(() =>
        {
            if (SelectedSolution == null)
            {
                System.Windows.MessageBox.Show("Please select a solution.");
                return;
            }

            NewSolutionText = SelectedSolution.SolutionText;
        });


        public ICommand ClearSolution => new RelayCommand(() =>
        {
            NewSolutionText = string.Empty;
            
        });

        public ICommand LogOut => new RelayCommand(() =>
        {
            ViewModelLocator.MainViewModel.CurrentPage = new LoginRegisterView();

        });


        private void UpdateProblems()
        {
            Problems = new ObservableCollection<Problem>(database.GetProblems(Username));
            CanEdit = true;
        }

        private void UpdateAllProblems()
        {
            Problems = new ObservableCollection<Problem>(database.GetProblems());
            CanEdit = false;
        }


        private void UpdateSolutions()
        {
            Solutions = new ObservableCollection<Solution>(database.GetSolutions(SelectedProblem.Id));
        }
    
    }
}
