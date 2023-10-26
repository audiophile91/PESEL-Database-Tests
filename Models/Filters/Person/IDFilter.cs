using PESEL_Database_Tests.Resources.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Models.Filters.Person
{
    public class IDFilter
    {
        public bool IsActive { get; set; }

        public int IndexType { get; set; }
        public int InputValue { get; set; }

        public bool CanFilter => IsActive;

        public bool Filter(PersonModel person)
        {
            return NumberFilter.Filter(IndexType, person.ID, InputValue);
        }
    }
}
