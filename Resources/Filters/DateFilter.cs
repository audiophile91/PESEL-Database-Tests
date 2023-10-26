using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Resources.Filters
{
    public class DateFilter
    {
        public readonly static string[] ItemsSource = new string[]
        {
            "Exact", "Different than", "After", "After than or exact", "Before than or exact", "Before" 
        };

        public static bool Filter<T>(int indexType, T sourceValue, T inputValue) where T : IComparable
        {
            switch ((FilterTypeDate)indexType)
            {
                case FilterTypeDate.Exact:
                    return sourceValue.CompareTo(inputValue) == 0;
                case FilterTypeDate.DifferentThan:
                    return sourceValue.CompareTo(inputValue) != 0;
                case FilterTypeDate.After:
                    return sourceValue.CompareTo(inputValue) > 0;
                case FilterTypeDate.AfterOrExact:
                    return sourceValue.CompareTo(inputValue) >= 0;
                case FilterTypeDate.BeforeOrExact:
                    return sourceValue.CompareTo(inputValue) <= 0;
                case FilterTypeDate.Before:
                    return sourceValue.CompareTo(inputValue) < 0;
                default:
                    throw new ArgumentException("Unknown filter type.");
            }
        }
    }

    public enum FilterTypeDate
    {
        Exact,
        DifferentThan,
        After,
        AfterOrExact,
        BeforeOrExact,
        Before,
    }
}
