using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl
{
    public static class VcelDateTime
    {
        public static DateTime ShiftDay(IReadOnlyList<object> args)
        {
            var date = DateTime.Parse(args[0].ToString());
            var noOfDays = int.Parse(args[1].ToString());
            var skipDates = args.Skip(2).Select(arg => arg.ToString()).Select(DateTime.Parse).ToHashSet();
            
            while (noOfDays > 0)
            {
                date = date.AddDays(1);

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        continue;
                    default:
                        if (!skipDates.Contains(date))
                        {
                            noOfDays--;
                        }
                        continue;
                }
            }
            
            return date;
        }
    }
}
