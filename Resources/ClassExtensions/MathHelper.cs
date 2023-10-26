using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Resources.ClassExtensions
{
    public static class MathHelper
    {
        public static bool EqualsOr(int comparedTo, params int[] values)
        {
            foreach (var v in values)
            {
                if (v == comparedTo)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
