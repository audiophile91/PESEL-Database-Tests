using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Resources.Filters;
using PESEL_Database_Tests.Statics.Generators;
using PESEL_Database_Tests.ViewModels;
using PESEL_Database_Tests.ViewModels.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PESEL_Database_Tests.ViewModels.Filters.Person
{
    public class VoivodeshipFilter : ViewModelBase
    {
        public VoivodeshipFilter()
        {
            DisplayedVoivodeships = new();
            _filterItems = new();

            InitializeDictionaries();
            InitializeFilters();
        }

        private readonly Dictionary<int, bool> DisplayedVoivodeships;
        public IReadOnlyCollection<FilterItem> FilterItems => _filterItems;

        private readonly ObservableCollection<FilterItem> _filterItems;

        public bool IsActive { get; set; }

        private bool? _isActiveRoot = true;
        public bool? IsActiveRoot
        {
            get
            {
                return _isActiveRoot;
            }
            set
            {
                if (_isActiveRoot != null)
                {
                    _isActiveRoot ^= true;
                }
                else
                {
                    _isActiveRoot = value;
                }
                UpdateSubFilters();
                OnPropertyChanged(nameof(IsActiveRoot));
            }
        }

        public bool CanFilter => IsActive;

        public bool Filter(PersonModel person)
        {
            return DisplayedVoivodeships[person.VoivodeshipID];
        }

        public void FilterVoivodeships()
        {
            for (int i = 0; i < FilterItems.Count; i++)
            {
                DisplayedVoivodeships[i] = _filterItems[i].IsActive;
            }
        }
        public void InitializeDictionaries()
        {
            for (int i = 0; i < PersonGenerator.voivodeships.Length; i++)
            {
                DisplayedVoivodeships.Add(i, true);
            }
        }
        public void InitializeFilters()
        {
            foreach (string name in PersonGenerator.voivodeships)
            {
                var item = new FilterItem(name);
                item.PropertyChanged += UpdateRootFilter;
                _filterItems.Add(item);
            }
        }
        public void UpdateSubFilters()
        {
            if (IsActiveRoot != null)
            {
                foreach (FilterItem item in FilterItems)
                {
                    item.PropertyChanged -= UpdateRootFilter;
                    item.IsActive = (bool)IsActiveRoot;
                    item.PropertyChanged += UpdateRootFilter;
                }
            }
        }
        public void UpdateRootFilter(object? sender, PropertyChangedEventArgs e)
        {
            _isActiveRoot = FilterItems.Any(x => x.IsActive == true) && FilterItems.Any(x => x.IsActive == false) ? null : FilterItems.FirstOrDefault()?.IsActive;
            OnPropertyChanged(nameof(IsActiveRoot));
        }
    }
}
