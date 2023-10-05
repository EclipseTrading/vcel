using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl;

public static class VcelDateTime
{
    public static DateTime Today() => DateTime.Today;

    public static DateTime? Today(object? arg) => arg switch
    {
        null => Today(),
        int dayOffset => DateTime.Today.AddDays(dayOffset),
        string dayOffsetString when int.TryParse(dayOffsetString, out var dayOffset) => DateTime.Today.AddDays(dayOffset),
        _ => null,
    };

    public static DateTime? Today(object?[] args) => args.Length == 0
        ? Today()
        : Today(args[0]);

    public static WorkdayParams ParseWorkdayParams(IReadOnlyList<object?> args)
    {
        var firstArg = args[0];
        var secondArg = args[1];

        var startDay = ParseDateTime(firstArg);
        if (!(secondArg is int noOfDays))
        {
            noOfDays = int.TryParse(secondArg?.ToString(), out var parsed) ? parsed : 0;
        }

        var skippedDates = args.Count <= 2 ? Enumerable.Empty<DateTime>() : ParseDateTimes(args[2]);
        return new WorkdayParams(startDay, noOfDays, skippedDates.ToList());
    }

    public static DateTime Workday(WorkdayParams workdayParams)
    {
        return Workday(workdayParams.StartDay, workdayParams.NoOfDays, workdayParams.SkippedDates);
    }

    public static DateTime Workday(DateTime startDay, int noOfDays, IReadOnlyList<DateTime> skippedDates)
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

    private static IEnumerable<DateTime> ParseDateTimes(object? value)
    {
        if (value is IEnumerable<object> enumerable)
        {
            return enumerable.Select(val => ParseDateTime(val)).ToHashSet();
        }

        return Enumerable.Empty<DateTime>();
    }

    private static DateTime ParseDateTime(object? value)
    {
        return value switch
        {
            DateTime date => date,
            DateTimeOffset dto => dto.DateTime,
            _ when DateTime.TryParse(value?.ToString(), out var date) => date,
            _ => throw new ArgumentException($"{value} is not a valid date time"),
        };
    }

    public static DateTime? ToDateTime(object? arg) => arg switch
    {
        long msTimestamp => DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp).DateTime,
        double msTimestamp when msTimestamp > 0 && msTimestamp <= Int64.MaxValue =>
            DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(msTimestamp)).DateTime,
        DateTime dt => dt,
        DateTimeOffset dto => dto.DateTime,
        string str when DateTime.TryParse(str, out var dt) => dt,
        _ => default(DateTime?),
    };

    public static DateTime? ToDate(object? arg) => arg switch
    {
        long msTimestamp => DateTimeOffset.FromUnixTimeMilliseconds(msTimestamp).Date,
        double msTimestamp when msTimestamp > 0 && msTimestamp <= Int64.MaxValue =>
            DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(msTimestamp)).Date,
        DateTime dt => dt.Date,
        DateTimeOffset dto => dto.Date,
        string str when DateTime.TryParse(str, out var dt) => dt.Date,
        _ => default(DateTime?),
    };
}
