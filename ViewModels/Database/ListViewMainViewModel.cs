using PESEL_Database_Tests.Commands;
using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PESEL_Database_Tests.ViewModels.Database
{
    public class ListViewMainViewModel : ViewModelBase
    {
        private ListViewFilterViewModel _filterViewModel;
        private ListViewViewModel _listViewViewModel;

        public ListViewFilterViewModel FilterViewModel => _filterViewModel;
        public ListViewViewModel ListViewViewModel => _listViewViewModel;

        public ListViewMainViewModel()
        {
            _listViewViewModel = new();
            _filterViewModel = new(ListViewViewModel.Records);
        }

        private string _switchLabel = "Filters";
        public string SwitchLabel
        {
            get
            {
                return _switchLabel;
            }
            set
            {
                _switchLabel = value;
                OnPropertyChanged(nameof(SwitchLabel));
            }
        }

        public RelayCommand GenerateDatabaseButton => new RelayCommand(execute => ListViewViewModel.GenerateDatabase(), canExecute => ListViewViewModel.CanGenerateData);
        public RelayCommand FilterDisplayCommand => new RelayCommand(execute => SwitchFilterView(), canExecute => ListViewViewModel.Records.Count != 0);

        public void SwitchFilterView()
        {
            ListViewViewModel.Display ^= true;
            FilterViewModel.Display ^= true;
            SwitchLabel = FilterViewModel.Display ? "Database" : "Filters";
            
            if (ListViewViewModel.Display)
            {
                FilterViewModel.ApplyFilters();
            }           
        }
    }
}
