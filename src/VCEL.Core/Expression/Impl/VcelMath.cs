using System;
using System.Linq;

namespace VCEL.Core.Expression.Impl
{
    public static class VcelMath
    {
        public static double? Abs(object arg) => arg == null ? default(double?) : Math.Abs(Convert.ToDouble(arg));

        public static double? Acos(object arg) => arg == null ? default(double?) : Math.Acos(Convert.ToDouble(arg));

        public static double? Asin(object arg) => arg == null ? default(double?) : Math.Asin(Convert.ToDouble(arg));

        public static double? Atan(object arg) => arg == null ? default(double?) : Math.Atan(Convert.ToDouble(arg));

        public static double? Atan2(object arg1, object arg2)
        {
            return arg1 == null || arg2 == null
                ? default(double?)
                : Math.Atan2(Convert.ToDouble(arg1), Convert.ToDouble(arg2));
        }

        public static double? Ceiling(object arg) => arg == null ? default(double?) : Math.Ceiling(Convert.ToDouble(arg));

        public static double? Cos(object arg) => arg == null ? default(double?) : Math.Cos(Convert.ToDouble(arg));

        public static double? Cosh(object arg) => arg == null ? default(double?) : Math.Cosh(Convert.ToDouble(arg));

        public static double? Exp(object arg) => arg == null ? default(double?) : Math.Exp(Convert.ToDouble(arg));

        public static double? Floor(object arg) => arg == null ? default(double?) : Math.Floor(Convert.ToDouble(arg));

        public static double? Log(object arg) => arg == null ? default(double?) : Math.Log(Convert.ToDouble(arg));

        public static double? Log10(object arg) => arg == null ? default(double?) : Math.Log10(Convert.ToDouble(arg));

        public static double? Pow(object arg1, object arg2)
        {
            return arg1 == null || arg2 == null
                ? default(double?)
                : Math.Pow(Convert.ToDouble(arg1), Convert.ToDouble(arg2));
        }

        public static T Min<T>(params T[] args)
        {
            return args == null ? default : args.Min();
        }

        public static T Max<T>(params T[] args)
        {
            return args == null ? default : args.Max();
        }

        public static double? Round(object arg1, object arg2 = null)
        {
            if (arg1 == null) return null;
            return arg2 == null
                ? Math.Round(Convert.ToDouble(arg1))
                : Math.Round(Convert.ToDouble(arg1), Convert.ToInt32(arg2));
        }

        public static double? Sign(object arg) => arg == null ? default(double?) : Math.Sign(Convert.ToDouble(arg));

        public static double? Sin(object arg) => arg == null ? default(double?) : Math.Sin(Convert.ToDouble(arg));

        public static double? Sinh(object arg) => arg == null ? default(double?) : Math.Sinh(Convert.ToDouble(arg));

        public static double? Sqrt(object arg) => arg == null ? default(double?) : Math.Sqrt(Convert.ToDouble(arg));

        public static double? Tan(object arg) => arg == null ? default(double?) : Math.Tan(Convert.ToDouble(arg));

        public static double? Tanh(object arg) => arg == null ? default(double?) : Math.Tanh(Convert.ToDouble(arg));

        public static double? Truncate(object arg) => arg == null ? default(double?) : Math.Truncate(Convert.ToDouble(arg));
    }
}
