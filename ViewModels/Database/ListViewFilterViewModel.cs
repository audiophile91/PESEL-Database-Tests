using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PESEL_Database_Tests.ViewModels.Database
{
    public class ListViewFilterViewModel : ViewModelBase
    {
        private ObservableCollection<PersonModel> Records { get; }

        public ICollectionView RecordsView => CollectionViewSource.GetDefaultView(Records);

        private Dictionary<int, bool> ValidNamesMale;
        private Dictionary<int, bool> ValidNamesFemale;

        public ListViewFilterViewModel(ObservableCollection<PersonModel> records)
        {
            Records = records;
            InitializeDictionaries();
        }

        private List<Predicate<PersonModel>> ActiveFilters = new List<Predicate<PersonModel>>();

        private bool _display;
        private bool _isBirthExactSelected = true;
        private bool _isBirthPartialSelected;
        private bool _isMaleSelected = true;
        private bool _isFemaleSelected;

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
        public bool IsBirthExactSelected
        {
            get
            {
                return _isBirthExactSelected;
            }
            set
            {
                _isBirthExactSelected = value;
                _isBirthPartialSelected = !value;
                OnPropertyChanged(nameof(IsBirthExactSelected), nameof(IsBirthPartialSelected));
            }
        }
        public bool IsBirthPartialSelected
        {
            get
            {
                return _isBirthPartialSelected;
            }
            set
            {
                _isBirthPartialSelected = value;
                _isBirthExactSelected = !value;
                OnPropertyChanged(nameof(IsBirthExactSelected), nameof(IsBirthPartialSelected));
            }
        }
        public bool IsMaleSelected
        {
            get
            {
                return _isMaleSelected;
            }
            set
            {
                _isMaleSelected = value;
                _isFemaleSelected = !value;
                OnPropertyChanged(nameof(IsMaleSelected), nameof(IsFemaleSelected));
            }
        }
        public bool IsFemaleSelected
        {
            get
            {
                return _isFemaleSelected;
            }
            set
            {
                _isFemaleSelected = value;
                _isMaleSelected = !value;
                OnPropertyChanged(nameof(IsMaleSelected), nameof(IsFemaleSelected));
            }
        }

        public bool IsFilterIDActive { get; set; }
        public bool IsFilterSexActive { get; set; }
        public bool IsFilterNameActive { get; set; }
        public bool IsFilterLastnameActive { get; set; }
        public bool IsFilterBirthDateActive { get; set; }
        public bool IsFilterAgeActive { get; set; }

        public int FilterIDType { get; set; }
        public int FilterIDIntValue { get; set; }
        public int FilterNameType { get; set; }
        public int FilterAgeType { get; set; }
        public int FilterAgeIntValue { get; set; }
        public int FilterExactBirthDateType { get; set; }
        public int FilterPartialBirthDatePartType { get; set; }
        public int FilterPartialBirthDateType { get; set; }
        public int FilterBirthIntValue { get; set; }

        public DateTime SelectedBirthDate { get; set; } = DateTime.Today;

        public string? FilterIDValue { get; set; }
        public string? FilterNameValue { get; set; }
        public string? FilterAgeValue { get; set; }
        public string? FilterBirthValue { get; set; }

        private bool CanFilterID => IsFilterIDActive && !string.IsNullOrWhiteSpace(FilterIDValue) && int.TryParse(FilterIDValue, out _) && int.Parse(FilterIDValue) > 0;
        private bool CanFilterSex => IsFilterSexActive;
        private bool CanFilterName => IsFilterNameActive && !string.IsNullOrWhiteSpace(FilterNameValue);
        private bool CanFilterAge => IsFilterAgeActive && !string.IsNullOrWhiteSpace(FilterAgeValue) && int.TryParse(FilterAgeValue, out _) && int.Parse(FilterAgeValue) > 0;
        private bool CanFilterBirth => IsFilterBirthDateActive && (IsBirthExactSelected || int.TryParse(FilterBirthValue, out _));

        public void ApplyFilters()
        {
            ActiveFilters.Clear();

            if (CanFilterID)
            {
                FilterIDIntValue = int.Parse(FilterIDValue);
                ActiveFilters.Add(FilterID);
            }
            if (CanFilterSex) ActiveFilters.Add(FilterSex);
            if (CanFilterName)
            {
                GetValidNames();
                ActiveFilters.Add(FilterName);
            }
            if (CanFilterAge)
            {
                FilterAgeIntValue = int.Parse(FilterAgeValue);
                ActiveFilters.Add(FilterAge);
            }
            if (CanFilterBirth)
            {
                FilterBirthIntValue = int.Parse(FilterBirthValue);
                ActiveFilters.Add(FilterBirth);
            }

            RecordsView.Filter = item => ActiveFilters.All(filter => filter((PersonModel)item));
        }

        public bool FilterID(PersonModel person)
        {
            switch (FilterIDType)
            {
                case 0:
                    return person.ID == FilterIDIntValue;
                case 1:
                    return person.ID.ToString().Contains(FilterIDValue);
                case 2:
                    return person.ID.ToString().StartsWith(FilterIDValue);
                case 3:
                    return person.ID.ToString().EndsWith(FilterIDValue);
                case 4:
                    return person.ID > FilterIDIntValue;
                case 5:
                    return person.ID >= FilterIDIntValue;
                case 6:
                    return person.ID <= FilterIDIntValue;
                case 7:
                    return person.ID < FilterIDIntValue;
                default:
                    return true;
            }
        }
        public bool FilterSex(PersonModel person)
        {
            return IsMaleSelected ? person.IsMale : !person.IsMale;
        }
        public bool FilterName(PersonModel person)
        {
            return person.IsMale ? ValidNamesMale[person.NameID] : ValidNamesFemale[person.NameID];
        }
        public bool FilterAge(PersonModel person)
        {
            switch (FilterAgeType)
            {
                case 0:
                    return person.Age == FilterAgeIntValue;
                case 1:
                    return person.Age > FilterAgeIntValue;
                case 2:
                    return person.Age >= FilterAgeIntValue;
                case 3:
                    return person.Age <= FilterAgeIntValue;
                case 4:
                    return person.Age < FilterAgeIntValue;
                default:
                    return true;
            }
        }
        public bool FilterBirth(PersonModel person)
        {


            if (IsBirthExactSelected)
            {
                switch (FilterExactBirthDateType)
                {
                    case 0:
                        return person.BirthExactDate == SelectedBirthDate;
                    case 1:
                        return person.BirthExactDate > SelectedBirthDate;
                    case 2:
                        return person.BirthExactDate < SelectedBirthDate;
                    default:
                        return true;
                }
            }
            else
            {
                switch (FilterPartialBirthDatePartType)
                {
                    case 0:
                        {
                            switch (FilterPartialBirthDateType)
                            {
                                case 0:
                                    return person.BirthExactDate.Year == FilterBirthIntValue;
                                case 1:
                                    return person.BirthExactDate.Year > FilterBirthIntValue;
                                case 2:
                                    return person.BirthExactDate.Year < FilterBirthIntValue;
                                default:
                                    return true;
                            }
                        }

                    case 1:
                        {
                            switch (FilterPartialBirthDateType)
                            {
                                case 0:
                                    return person.BirthExactDate.Month == FilterBirthIntValue;
                                case 1:
                                    return person.BirthExactDate.Month > FilterBirthIntValue;
                                case 2:
                                    return person.BirthExactDate.Month < FilterBirthIntValue;
                                default:
                                    return true;
                            }
                        }
                    case 2:
                        {
                            switch (FilterPartialBirthDateType)
                            {
                                case 0:
                                    return person.BirthExactDate.Day == FilterBirthIntValue;
                                case 1:
                                    return person.BirthExactDate.Day > FilterBirthIntValue;
                                case 2:
                                    return person.BirthExactDate.Day < FilterBirthIntValue;
                                default:
                                    return true;
                            }
                        }
                    default:
                        return true;
                }
            }
        }

        public void GetValidNames()
        {
            ApplyStringFilter(ValidNamesMale, PersonGenerator.GetNamesMaleToLower);
            ApplyStringFilter(ValidNamesFemale, PersonGenerator.GetNamesFemaleToLower);
        }
        private void ApplyStringFilter(Dictionary<int, bool> conditionsValues, string[] namesArray)
        {
            var value = FilterNameValue.ToLower();

            for (int i = 0; i < conditionsValues.Count; i++)
            {
                switch (FilterNameType)
                {
                    case 0: conditionsValues[i] = namesArray[i].ToLower().Equals(value); break;
                    case 1: conditionsValues[i] = namesArray[i].ToLower().Contains(value); break;
                    case 2: conditionsValues[i] = namesArray[i].ToLower().StartsWith(value); break;
                    case 3: conditionsValues[i] = namesArray[i].ToLower().EndsWith(value); break;
                }
            }
        }
        public void InitializeDictionaries()
        {
            ValidNamesMale = new();
            ValidNamesFemale = new();

            for (int i = 0; i < PersonGenerator.namesMale.Length; i++)
            {
                ValidNamesMale.Add(i, true);
            }

            for (int i = 0; i < PersonGenerator.namesFemale.Length; i++)
            {
                ValidNamesFemale.Add(i, true);
            }
        }
    }
}