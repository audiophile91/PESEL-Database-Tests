using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Resources.Filters
{
    public class StringFilter
    {
        public readonly static string[] ItemsSource = new string[]
        {
            "Equal", "Contains", "Starts with", "Ends with"
        };

        public static bool Filter(int indexType, string sourceValue, string inputValue)
        {
            switch ((FilterTypeString)indexType)
            {
                case FilterTypeString.Equal:
                    return sourceValue.Equals(inputValue);
                case FilterTypeString.Contains:
                    return sourceValue.Contains(inputValue);
                case FilterTypeString.StartsWith:
                    return sourceValue.StartsWith(inputValue);
                case FilterTypeString.EndsWith:
                    return sourceValue.EndsWith(inputValue);
                default:
                    throw new ArgumentException("Unknown filter type.");
            }
        }
    }

    public enum FilterTypeString
    {
        Equal,
        Contains,
        StartsWith,
        EndsWith
    }
}
