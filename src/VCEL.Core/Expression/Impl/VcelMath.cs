using System;
using System.Globalization;
using System.Linq;

namespace VCEL.Core.Expression.Impl;

public static class VcelMath
{
    public static double? Abs(object? arg) => arg == null ? null : Math.Abs(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Acos(object? arg) => arg == null ? null : Math.Acos(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Asin(object? arg) => arg == null ? null : Math.Asin(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Atan(object? arg) => arg == null ? null : Math.Atan(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Atan2(object? arg1, object? arg2)
    {
        return arg1 == null || arg2 == null
            ? null
            : Math.Atan2(Convert.ToDouble(arg1, CultureInfo.InvariantCulture), Convert.ToDouble(arg2, CultureInfo.InvariantCulture));
    }

    public static double? Ceiling(object? arg) => arg == null ? null : Math.Ceiling(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Cos(object? arg) => arg == null ? null : Math.Cos(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Cosh(object? arg) => arg == null ? null : Math.Cosh(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Exp(object? arg) => arg == null ? null : Math.Exp(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Floor(object? arg) => arg == null ? null : Math.Floor(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Log(object? arg) => arg == null ? null : Math.Log(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Log10(object? arg) => arg == null ? null : Math.Log10(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Pow(object? arg1, object? arg2)
    {
        return arg1 == null || arg2 == null
            ? null
            : Math.Pow(Convert.ToDouble(arg1, CultureInfo.InvariantCulture), Convert.ToDouble(arg2, CultureInfo.InvariantCulture));
    }

    public static double? Mod(object? arg1, object? arg2)
    {
        return arg1 == null || arg2 == null
            ? null
            : (Convert.ToDouble(arg1, CultureInfo.InvariantCulture) % Convert.ToDouble(arg2, CultureInfo.InvariantCulture));
    }

    public static object? Min(params object[] args) => args?.Min();

    public static object? Max(params object[] args) => args?.Max();

    public static double? Round(object? arg1, object? arg2 = null)
    {
        if (arg1 == null) return null;
        return arg2 == null
            ? Math.Round(Convert.ToDouble(arg1, CultureInfo.InvariantCulture))
            : Math.Round(Convert.ToDouble(arg1, CultureInfo.InvariantCulture), Convert.ToInt32(arg2, CultureInfo.InvariantCulture));
    }

    public static double? Sign(object? arg) => arg == null ? null : Math.Sign(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Sin(object? arg) => arg == null ? null : Math.Sin(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Sinh(object? arg) => arg == null ? null : Math.Sinh(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Sqrt(object? arg) => arg == null ? null : Math.Sqrt(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Tan(object? arg) => arg == null ? null : Math.Tan(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Tanh(object? arg) => arg == null ? null : Math.Tanh(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static double? Truncate(object? arg) => arg == null ? null : Math.Truncate(Convert.ToDouble(arg, CultureInfo.InvariantCulture));

    public static bool? IsNaN(object? arg) => arg == null ? null : double.IsNaN(Convert.ToDouble(arg, CultureInfo.InvariantCulture));
}