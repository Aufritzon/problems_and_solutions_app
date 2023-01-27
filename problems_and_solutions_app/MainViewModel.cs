using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace problems_and_solutions_app
{
    public class MainViewModel : ViewModelBase
    {
		private UserControl _currentPage;

		public UserControl CurrentPage
		{
			get { return _currentPage; }
			set 
			{ 
				_currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
			}
		}

		public MainViewModel()
		{
			CurrentPage = new LoginRegisterView();
		}

	}
}
