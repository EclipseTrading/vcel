using System;
using System.Globalization;

namespace VCEL.Core.Expression.Impl;

public static class VcelType  {        
    public static int? Integer<T>(T arg) {
        if (arg == null) {
            return null;	
        }
        return Convert.ToInt32(Convert.ToDouble(arg, CultureInfo.InvariantCulture));	
    }
        
    public static long? Long<T>(T arg) {
        if (arg == null) {
            return null;	
        } 
        return Convert.ToInt64(Convert.ToDouble(arg, CultureInfo.InvariantCulture));	
    }

    public static double? Double<T>(T arg) => arg == null ? null : Convert.ToDouble(arg, CultureInfo.InvariantCulture);

    public static decimal? Decimal<T>(T arg) => arg == null ? null : Convert.ToDecimal(arg, CultureInfo.InvariantCulture);

    public static string? String<T>(T arg, object? format = null) { 
        if (arg == null) {
            return null;	
        } 
        if (arg is DateTime dt && format != null) {
            return dt.ToString((string)format, CultureInfo.InvariantCulture);
        } else if (arg is DateTimeOffset dto && format != null) {
            return dto.ToString((string)format, CultureInfo.InvariantCulture);	
        }
        return Convert.ToString(arg, CultureInfo.InvariantCulture);
    }

    public static bool? Boolean<T>(T arg) => arg == null ? null : Convert.ToBoolean(arg, CultureInfo.InvariantCulture);
}