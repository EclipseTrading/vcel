using System;
using System.Globalization;

namespace VCEL.Core.Expression.Impl;

public static class VcelType
{
    public static int? Integer<T>(T arg) =>
        arg == null ? null : Convert.ToInt32(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static long? Long<T>(T arg) =>
        arg == null ? null : Convert.ToInt64(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Double<T>(T arg) => arg == null ? null : Convert.ToDouble(arg, CultureInfo.InvariantCulture);

    public static decimal? Decimal<T>(T arg) =>
        arg == null ? null : Convert.ToDecimal(arg, CultureInfo.InvariantCulture);

    public static string? String<T>(T arg, object? format = null) => arg switch
    {
        null => null,
        DateTime dt when format != null => dt.ToString((string)format, CultureInfo.InvariantCulture),
        DateTimeOffset dto when format != null => dto.ToString((string)format, CultureInfo.InvariantCulture),
        _ => Convert.ToString(arg, CultureInfo.InvariantCulture),
    };

    public static bool? Boolean<T>(T arg) => arg == null ? null : Convert.ToBoolean(arg, CultureInfo.InvariantCulture);
}