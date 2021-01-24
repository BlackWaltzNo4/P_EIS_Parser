using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPApp_Server.Filter
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparisonType)
        {
            return source?.IndexOf(toCheck, comparisonType) >= 0;
        }
    }
}
