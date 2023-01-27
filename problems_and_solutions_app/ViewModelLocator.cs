using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace problems_and_solutions_app
{
    public static class ViewModelLocator
    {
        public static MainViewModel MainViewModel { get; private set; } = new MainViewModel();
    }
}
