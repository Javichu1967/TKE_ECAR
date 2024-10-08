using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK_ECAR.Framework
{
    public static class DateTimeExtensions
    {
         
        public static IEnumerable<DateTime> Days( DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public static IEnumerable<DateTime> Days(this IEnumerable<DateTime> source, DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            { 
                if (source.Contains(day))
                {
                    yield return day;
                }
            }
                
        }
    }
}
