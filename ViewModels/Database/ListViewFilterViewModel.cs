using PESEL_Database_Tests.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.ViewModels.Database
{
    public class ListViewFilterViewModel : ViewModelBase
    {
		private ObservableCollection<PersonModel> RecordsView { get; }

        public ListViewFilterViewModel(ObservableCollection<PersonModel> recordsView)
        {
            RecordsView = recordsView;
        }

        private bool _display;
		public bool Display
		{
			get
			{
				return _display;
			}
			set
			{
				_display = value;
				OnPropertyChanged(nameof(Display));
			}
		}


		public void ApplyFilters()
		{

		}
	}
}
