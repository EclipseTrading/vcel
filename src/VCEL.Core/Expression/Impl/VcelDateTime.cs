using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl
{
    public static class VcelDateTime
    {
        public static WorkdayParams ParseWorkdayParams(IReadOnlyList<object> args)
        {
            var startDay = ParseDateTime(args[0]);
            if (!(args[1] is int noOfDays))
            {
                noOfDays = int.Parse(args[1].ToString());
            }
            var skippedDates = args.Count <= 2 ? Enumerable.Empty<DateTime>() : ParseDateTimes(args[2]);
            return new WorkdayParams(startDay, noOfDays, skippedDates);
        }

        public static DateTime Workday(WorkdayParams workdayParams)
        {
            return Workday(workdayParams.StartDay, workdayParams.NoOfDays, workdayParams.SkippedDates);
        }
        public static DateTime Workday(DateTime startDay, int noOfDays, IEnumerable<DateTime> skippedDates)
        {
            var date = startDay;
            var absNoOfDays = Math.Abs(noOfDays);
            var addDay = noOfDays > 0 ? 1 : -1;
            
            while (absNoOfDays > 0)
            {
                date = date.AddDays(addDay);

                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        continue;
                    default:
                        if (!skippedDates.Contains(date))
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

        public static DateTime? ToDateTime(object? arg) => arg switch
        {
            long msTimestamp => DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp).DateTime,
            double msTimestamp when msTimestamp > 0 && msTimestamp <= Int64.MaxValue =>
                DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(msTimestamp)).DateTime,
            DateTime dt => dt,
            DateTimeOffset dto => dto.DateTime,
            _ => default(DateTime?)
        };

        public static DateTime? ToDate(object? arg) => arg switch
        {
            long msTimestamp => DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp).Date,
            double msTimestamp when msTimestamp > 0 && msTimestamp <= Int64.MaxValue =>
                DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(msTimestamp)).Date,
            DateTime dt => dt.Date,
            DateTimeOffset dto => dto.Date,
            _ => default(DateTime?)
        };
    }
}
