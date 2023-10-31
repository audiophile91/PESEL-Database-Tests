using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using PESEL_Database_Tests.ViewModels.Generator.Filters.Person;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PESEL_Database_Tests.ViewModels.Generator
{
    public class GeneratorFiltersViewModel : ViewModelBase
    {
        public GeneratorFiltersViewModel(ObservableCollection<PersonModel> generatorRecords)
        {
            GeneratorRecords = generatorRecords;
            ActiveFilters = new();
            AgeFilter = new();
            BirthFilter = new();
            IDFilter = new();
            NameFilter = new();
            SexFilter = new();
            VoivodeshipFilter = new();
        }

        private ObservableCollection<PersonModel> GeneratorRecords { get; }

        private readonly List<Predicate<PersonModel>> ActiveFilters;

        private bool _display;

        public AgeFilter AgeFilter { get; }
        public BirthFilter BirthFilter { get; }
        public IDFilter IDFilter { get; }
        public NameFilter NameFilter { get; }
        public SexFilter SexFilter { get; }
        public VoivodeshipFilter VoivodeshipFilter { get; }

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
        public int FiltredResult { get; set; }

        public ICollectionView RecordsView => CollectionViewSource.GetDefaultView(GeneratorRecords);

        public void ApplyFilters()
        {
            ActiveFilters.Clear();

            AddActiveFilters();

            RecordsView.Filter = item => ActiveFilters.All(filter => filter((PersonModel)item));

            FiltredResult = RecordsView.Cast<PersonModel>().Count();
            OnPropertyChanged(nameof(FiltredResult));
        }
        public void AddActiveFilters()
        {
            if (AgeFilter.CanFilter)
            {
                ActiveFilters.Add(AgeFilter.Filter);
            }
            if (BirthFilter.CanFilter)
            {
                ActiveFilters.Add(BirthFilter.IsFilterModeExact ? BirthFilter.FilterExact : BirthFilter.FilterPartial);
            }
            if (IDFilter.CanFilter)
            {
                ActiveFilters.Add(IDFilter.Filter);
            }
            if (NameFilter.CanFilter)
            {
                NameFilter.FilterNames();
                ActiveFilters.Add(NameFilter.Filter);
            }
            if (SexFilter.CanFilter)
            {
                ActiveFilters.Add(SexFilter.Filter);
            }
            if (VoivodeshipFilter.CanFilter)
            {
                VoivodeshipFilter.FilterVoivodeships();
                ActiveFilters.Add(VoivodeshipFilter.Filter);
            }
        }
    }
}