using PESEL_Database_Tests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Models.Filters.Person
{
    public class SexFilter : ViewModelBase
    {
        private bool _isSelectedMale;
        
        public bool IsActive { get; set; }
        public bool IsSelectedMale
        {
            get
            {
                return _isSelectedMale;
            }
            set
            {
                _isSelectedMale = value;
                OnPropertyChanged(nameof(IsSelectedMale));
            }
        }

        public bool CanFilter => IsActive;

        public bool Filter(PersonModel person)
        {
            return IsSelectedMale ? person.IsMale : !person.IsMale;
        }
    }
}
