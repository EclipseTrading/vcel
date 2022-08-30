using System;

namespace VCEL.Core.Expression.Impl {
    public static class VcelType  {        
        public static int? Integer<T>(T arg) {
            if (arg == null) {
                return default(int?);	
            }
            return Convert.ToInt32(Convert.ToDouble(arg));	
        }
        
        public static long? Long<T>(T arg) {
            if (arg == null) {
                return default(long?);	
            } 
            return Convert.ToInt64(Convert.ToDouble(arg));	
        }

        public static double? Double<T>(T arg) => arg == null ? default(double?) : Convert.ToDouble(arg);

        public static decimal? Decimal<T>(T arg) => arg == null ? default(decimal?) : Convert.ToDecimal(arg);

        public static string? String<T>(T arg, object? format = null) { 
            if (arg == null) {
                return default(string?);	
            } 
            if (arg is DateTime dt && format != null) {
                return dt.ToString((string)format);
            } else if (arg is DateTimeOffset dto && format != null) {
                return dto.ToString((string)format);	
            }
            return Convert.ToString(arg);
        }

        public static bool? Boolean<T>(T arg) => arg == null ? default(bool?) : Convert.ToBoolean(arg);
    }
}