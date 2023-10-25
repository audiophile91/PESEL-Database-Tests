using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using PESEL_Database_Tests.Statics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PESEL_Database_Tests.ViewModels.Database;

namespace PESEL_Database_Tests.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ListViewMainViewModel _listViewMainViewModel;
        public ListViewMainViewModel ListViewMainViewModel => _listViewMainViewModel;

        public MainViewModel()
        {
            _listViewMainViewModel = new();
        }  
    }
}
