using System;
using VCEL.Core.Helper;

namespace VCEL.CSharp
{
    // When changing the name or signature of any functions in this class, you need to find its usage by searching by text
    // They are used in code-gen csharp code so won't show up in normal 'usage' list in your IDE
    public static class CSharpHelper
    {
        public static bool IsBetween<T>(T item, T start, T end)
        {
            return UpCastCompare(item, start, ">=") && UpCastCompare(item, end, "<=");
        }

        public static bool IsNumber(object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        public static bool UpCastCompare(object? l, object? r, string opName)
        {
            if (!(l is IComparable) || !(r is IComparable))
                return false;

            var lv = l;
            var rv = r;
            if (lv.GetType() != rv.GetType())
            {
                if (!UpCastExtensions.UpCast(ref lv, ref rv))
                    return false;
            }

            switch (opName)
            {
                case ">":
                    return ((IComparable) lv).CompareTo(rv) > 0;
                case ">=":
                    return ((IComparable) lv).CompareTo(rv) >= 0;
                case "<":
                    return ((IComparable) lv).CompareTo(rv) < 0;
                case "<=":
                    return ((IComparable) lv).CompareTo(rv) <= 0;
            }

            return false;
        }
    }
}