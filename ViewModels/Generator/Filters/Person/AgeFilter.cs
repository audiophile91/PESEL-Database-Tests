using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Resources.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.ViewModels.Generator.Filters.Person
{
    public class AgeFilter
    {
        public bool IsActive { get; set; }

        public int IndexType { get; set; }
        public byte InputValue { get; set; }

        public bool CanFilter => IsActive;

        public bool Filter(PersonModel person)
        {
            return NumberFilter.Filter(IndexType, person.Age, InputValue);
        }
    }
}
