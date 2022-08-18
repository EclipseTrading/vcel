using System;
using System.Collections.Generic;

namespace VCEL.Core.Expression.Impl
{
    public class WorkdayParams
    {
        public WorkdayParams(DateTime startDay, int noOfDays, IReadOnlyList<DateTime> skippedDates)
        {
            StartDay = startDay;
            NoOfDays = noOfDays;
            SkippedDates = skippedDates;
        }
        public DateTime StartDay { get; }
        public int NoOfDays { get; }
        public IReadOnlyList<DateTime> SkippedDates { get; }
    }
}
