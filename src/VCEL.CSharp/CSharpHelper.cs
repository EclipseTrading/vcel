using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using VCEL.Core.Helper;

namespace VCEL.CSharp;

// When changing the name or signature of any functions in this class, you need to find its usage by searching by text
// They are used in code-gen csharp code so won't show up in normal 'usage' list in your IDE
public static class CSharpHelper
{
    public static bool IsBetween(object item, object start, object end) =>
        UpCastCompareGreaterThanOrEqual(item, start) && UpCastCompareLessThanOrEqual(item, end);

    public static bool IsNumber(object value) =>
        value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;

    public static bool IsNumberOut(object value, out object number)
    {
        number = value;
        return value is sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal;
    }

    public static bool IsConvertibleOut(object? value, [NotNullWhen(true)] out IConvertible? convertible)
    {
        convertible = value as IConvertible;
        return convertible != null;
    }

    [OverloadResolutionPriority(-1)]
    public static bool TryToDouble<T>(T value, out double result) where T : INumberBase<T>
    {
        try
        {
            result = double.CreateChecked(value);
            return true;
        }
        catch (OverflowException)
        {
            result = 0;
            return false;
        }
    }

    [OverloadResolutionPriority(2)]
    public static bool TryToDouble(string value, out double result)
    {
        if (double.TryParse(value, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out result))
        {
            return true;
        }

        result = 0;
        return false;
    }

    [OverloadResolutionPriority(1)]
    public static bool TryToDouble(IConvertible value, out double result)
    {
        try
        {
            result = value.ToDouble(System.Globalization.CultureInfo.InvariantCulture);
            return true;
        }
        catch (InvalidCastException)
        {
            result = 0;
            return false;
        }
    }

    [OverloadResolutionPriority(0)]
    public static bool TryToDouble(object value, out double result)
    {
        if (value is IConvertible convertible)
        {
            try
            {
                result = convertible.ToDouble(System.Globalization.CultureInfo.InvariantCulture);
                return true;
            }
            catch (InvalidCastException)
            {
                result = 0;
                return false;
            }
        }

        result = 0;
        return false;
    }

    public static bool UpCastCompareGreaterThan<TLeft, TRight>(TLeft l, TRight r)
    {
        if (l is not IComparable lc || r is not IComparable rc) return false;

        return UpCastExtensions.TryCompareTo(lc, rc, out var result) && result > 0;
    }

    public static bool UpCastCompareLessThan<TLeft, TRight>(TLeft l, TRight r)
    {
        if (l is not IComparable lc || r is not IComparable rc) return false;

        return UpCastExtensions.TryCompareTo(lc, rc, out var result) && result < 0;
    }

    public static bool UpCastCompareGreaterThanOrEqual<TLeft, TRight>(TLeft l, TRight r)
    {
        if (l is not IComparable lc || r is not IComparable rc) return false;

        return UpCastExtensions.TryCompareTo(lc, rc, out var result) && result >= 0;
    }

    public static bool UpCastCompareLessThanOrEqual<TLeft, TRight>(TLeft l, TRight r)
    {
        if (l is not IComparable lc || r is not IComparable rc) return false;

        return UpCastExtensions.TryCompareTo(lc, rc, out var result) && result <= 0;
    }
}