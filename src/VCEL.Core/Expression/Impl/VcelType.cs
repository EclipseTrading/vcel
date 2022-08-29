using System;
using System.Linq;

namespace VCEL.Core.Expression.Impl {
    public static class VcelType  {        
        // String, Double, Integer, Long and Decimal for int(), double(), long(), decimal()
        public static int? Integer<T>(T arg) {
			if (arg == null) {
				return default(int?);	
			} else if (typeof(T) == typeof(string)) {
				return Convert.ToInt32(Convert.ToDouble(arg));	
			}
			return Convert.ToInt32(arg);
		}
        
        public static long? Long<T>(T arg) {
			if (arg == null) {
				return default(long?);	
			} else if (typeof(T) == typeof(string)) {
				return Convert.ToInt64(Convert.ToDouble(arg));	
			}
			return Convert.ToInt64(arg);
		}

        public static double? Double<T>(T arg) => arg == null ? default(double?) : Convert.ToDouble(arg);

        public static decimal? Decimal<T>(T arg) => arg == null ? default(decimal?) : Convert.ToDecimal(arg);

        // string, double, integer, long, decimal, boolean, datetime, date, time, duration for string()
        public static string? String<T>(T arg) => arg == null ? default(string?) : Convert.ToString(arg);

        public static bool? Boolean<T>(T arg) => arg == null ? default(bool?) : Convert.ToBoolean(arg);

    }
}