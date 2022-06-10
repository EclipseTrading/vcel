using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl
{
    public static class VcelDateTime
    {
        public static DateTime Workday(IReadOnlyList<object> args)
        {
            var date = ParseDateTime(args[0]);
            if (!(args[1] is int noOfDays))
            {
                noOfDays = int.Parse(args[1].ToString());
            }

            var absNoOfDays = Math.Abs(noOfDays);
            var addDay = noOfDays > 0 ? 1 : -1;
            var skipDates = args.Count <= 2 ? Enumerable.Empty<DateTime>() : ParseDateTimes(args[2]);
            
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

        private static IEnumerable<DateTime> ParseDateTimes(object value)
        {
            if (value is IEnumerable<object> enumerable)
            {
                return enumerable.Select(val => ParseDateTime(val)).ToHashSet();
            }

            return Enumerable.Empty<DateTime>();
        }

        private static DateTime ParseDateTime(object value)
        {
            return value switch
            {
                DateTime date => date,
                DateTimeOffset dto => dto.DateTime,
                _ => DateTime.Parse(value.ToString())
            };
        }
    }
}
