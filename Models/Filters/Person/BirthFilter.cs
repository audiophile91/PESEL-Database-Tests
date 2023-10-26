using PESEL_Database_Tests.Resources.Filters;
using PESEL_Database_Tests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Models.Filters.Person
{
    public class BirthFilter : ViewModelBase
    {
        private bool _isSelectedExact = true;

        public bool IsActive { get; set; }
        public bool IsSelectedExact
        {
            get
            {
                return _isSelectedExact;
            }
            set
            {
                _isSelectedExact = value;
                OnPropertyChanged(nameof(IsSelectedExact));
            }
        }

        public int IndexType { get; set; }
        public int IndexTypePartial { get; set; }

        public short InputValue { get; set; }

        public DateTime SelectedBirthDate { get; set; } = DateTime.Today;

        public bool CanFilter => IsActive;
        public bool IsFilterModeExact => IsSelectedExact;

        public bool FilterExact(PersonModel person)
        {
            return DateFilter.Filter(IndexType, GetPersonBirthDate(person), SelectedBirthDate);
        }
        public bool FilterPartial(PersonModel person)
        {
            return DateFilter.Filter(IndexType, GetPartialValue(person), InputValue);
        }

        private short GetPartialValue(PersonModel person)
        {
            var date = GetPersonBirthDate(person);

            return IndexTypePartial switch
            {
                0 => (short)date.Year,
                1 => (short)date.Month,
                2 => (short)date.Day,
                _ => 0
            };
        }
       
        private DateTime GetPersonBirthDate(PersonModel person) => person.BirthExactDate;
    }
}
