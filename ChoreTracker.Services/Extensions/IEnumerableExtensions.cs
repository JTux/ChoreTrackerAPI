using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string ToSingleString(this IEnumerable<string> enumerable)
        {
            if (enumerable.Count() == 1)
                return enumerable.ElementAt(0);

            var newString = "";

            foreach (var str in enumerable)
            {
                newString += $"{str}{((str == enumerable.ElementAt(enumerable.Count() - 1)) ? "" : " ")}";
            }

            return newString;
        }
    }
}
