using PESEL_Database_Tests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.ViewModels.Filters
{
    public class FilterItem : ViewModelBase
    {
        public FilterItem(string name)
        {
            IsActive = true;
            Name = name;
        }

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public string Name { get; }
    }
}
