namespace PESEL_Database_Tests.Resources.Filters
{
    public static class NumberFilter
    {
        public readonly static string[] ItemsSource = new string[]
        {
            "Equal", "Not equal", "Greater than", "Greater than or equal", "Lesser than or equal", "Lesser than"
        };

        public static bool Filter<T>(int indexType, T sourceValue, T inputValue) where T : IComparable
        {
            switch ((FilterTypeNumber)indexType)
            {
                case FilterTypeNumber.Equal:
                    return sourceValue.CompareTo(inputValue) == 0;
                case FilterTypeNumber.NotEqual:
                    return sourceValue.CompareTo(inputValue) != 0;
                case FilterTypeNumber.GreaterThan:
                    return sourceValue.CompareTo(inputValue) > 0;
                case FilterTypeNumber.GreaterThanOrEqual:
                    return sourceValue.CompareTo(inputValue) >= 0;
                case FilterTypeNumber.LesserThanOrEqual:
                    return sourceValue.CompareTo(inputValue) <= 0;
                case FilterTypeNumber.LesserThan:
                    return sourceValue.CompareTo(inputValue) < 0;
                default:
                    throw new ArgumentException("Unknown filter type.");
            }
        }
    }

    public enum FilterTypeNumber
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LesserThanOrEqual,
        LesserThan
    }
}
