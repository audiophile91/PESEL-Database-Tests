using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Resources.Filters;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.ViewModels.Generator.Filters.Person
{
    public class NameFilter
    {
        private readonly Dictionary<int, bool> ValidNamesMale;
        private readonly Dictionary<int, bool> ValidNamesFemale;

        public NameFilter()
        {
            ValidNamesMale = new();
            ValidNamesFemale = new();

            InitializeDictionaries();
        }

        public bool IsActive { get; set; }

        public int IndexType { get; set; }

        public string? InputValue { get; set; }

        public bool CanFilter => IsActive && !string.IsNullOrWhiteSpace(InputValue);

        public bool Filter(PersonModel person)
        {
            return person.IsMale ? ValidNamesMale[person.NameID] : ValidNamesFemale[person.NameID];
        }

        public void FilterNames()
        {
            ApplyStringFilterToDictionary(ValidNamesMale, PersonGenerator.GetNamesMaleToLower);
            ApplyStringFilterToDictionary(ValidNamesFemale, PersonGenerator.GetNamesFemaleToLower);
        }
        private void ApplyStringFilterToDictionary(Dictionary<int, bool> conditionsValues, string[] namesArray)
        {
            if (InputValue != null)
            {
                var value = InputValue.ToLower();

                for (int i = 0; i < conditionsValues.Count; i++)
                {
                    conditionsValues[i] = StringFilter.Filter(IndexType, namesArray[i], value);
                }
            }
        }
        public void InitializeDictionaries()
        {
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
