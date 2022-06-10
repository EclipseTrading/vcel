using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl
{
    public static class VcelDateTime
    {
        public static DateTime Workday(IReadOnlyList<object> args)
        {
            var date = DateTime.Parse(args[0].ToString());
            var noOfDays = int.Parse(args[1].ToString());
            var absNoOfDays = Math.Abs(noOfDays);
            var addDay = noOfDays > 0 ? 1 : -1;
            var skipDates = args.Skip(2).Select(arg => arg.ToString()).Select(DateTime.Parse).ToHashSet();
            
            while (absNoOfDays > 0)
            {
                date = date.AddDays(addDay);

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        continue;
                    default:
                        if (!skipDates.Contains(date))
                        {
                            absNoOfDays--;
                        }
                        continue;
                }
            }
            
            return date;
        }
    }
}
