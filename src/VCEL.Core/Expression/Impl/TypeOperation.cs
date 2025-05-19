using System;

namespace VCEL.Core.Expression.Impl;

public static class TypeOperation
{
    // public static bool EqualsChecked<T, T2>(T? left, T2? right)
    //     where T : class
    // {
    //     if (left == null || right == null)
    //     {
    //         return (left == null && right == null);
    //     }
    //
    //     if (left.GetType() == right.GetType())
    //     {
    //         return Equals(left, right);
    //     }
    //
    //     return EqualsMixedType(left, right);
    // }

    // public static bool EqualsChecked<TLeft, TRight>(TLeft left, TRight right)
    // {
    //     if (left == null || right == null)
    //     {
    //         return (left == null && right == null);
    //     }
    //
    //     // if (left.GetType() == right.GetType())
    //     // {
    //     //     return Equals(left, right);
    //     // }
    //
    //     return EqualsMixedType(left, right);
    // }

    public static bool EqualsChecked(object? left, object? right)
    {
        if (left == null || right == null)
        {
            return (left == null && right == null);
        }

        // if (left.GetType() == right.GetType())
        // {
        //     return left.Equals(right);
        // }

        return EqualsMixedType(left, right);
    }

    public static bool EqualsChecked(object? left, double right)
    {
        if (left == null)
        {
            return false;
        }

        if (left is double)
        {
            return right.Equals(left);
        }

        return EqualsMixedType(left, right);
    }

    public static bool EqualsChecked(object? left, int right)
    {
        if (left == null)
        {
            return false;
        }

        if (left is int)
        {
            return right.Equals(left);
        }

        return EqualsMixedType(left, right);
    }

    public static bool EqualsChecked(object? left, bool right)
    {
        if (left == null)
        {
            return false;
        }

        if (left is bool)
        {
            return right.Equals(left);
        }

        return EqualsMixedType(left, right);
    }


    // public static bool EqualsChecked<T2>(object? left, T2? right)
    //     where T2 : class
    // {
    //     if (left == null || right == null)
    //     {
    //         return (left == null && right == null);
    //     }
    //
    //     if (left.GetType() == right.GetType())
    //     {
    //         return Equals(left, right);
    //     }
    //
    //     return EqualsMixedType(left, right);
    // }

    // public static bool EqualsChecked<TLeft, TObject>(TLeft left, TObject? right)
    //     where TLeft : struct, IEquatable<TLeft>
    //     where TObject : class
    // {
    //     if (right == null)
    //     {
    //         return false;
    //     }
    //
    //     if (left.GetType() == right.GetType())
    //     {
    //         return left.Equals(right);
    //     }
    //
    //     return EqualsMixedType(left, right);
    // }

    // public static bool EqualsChecked<TRight>(object? left, TRight right)
    //     where TRight : struct, IEquatable<TRight>
    // {
    //     if (left == null)
    //     {
    //         return false;
    //     }
    //
    //     if (left.GetType() == right.GetType())
    //     {
    //         return right.Equals(left);
    //     }
    //
    //     return EqualsMixedType(left, right);
    // }

    // public static bool EqualsChecked<T>(T left, T right)
    //     where T : unmanaged, IEquatable<T> => left.Equals(right);

    private static bool EqualsMixedType<T, T2>(T left, T2 right) => left switch
    {
        double doubleLeft => right switch
        {
            double doubleRight => doubleLeft == doubleRight,
            int intRight => doubleLeft == intRight,
            long longRight => doubleLeft == longRight,
            decimal decimalRight => doubleLeft is < (double)decimal.MaxValue and > (double)decimal.MinValue &&
                                    (decimal)doubleLeft == decimalRight,
            float floatRight => doubleLeft == floatRight,
            short shortRight => doubleLeft == shortRight,
            uint uintRight => doubleLeft == uintRight,
            ulong ulongRight => doubleLeft == ulongRight,
            ushort ushortRight => doubleLeft == ushortRight,
            _ => Equals(doubleLeft, right),
        },
        int intLeft => right switch
        {
            double doubleRight => intLeft == doubleRight,
            int intRight => intLeft == intRight,
            long longRight => intLeft == longRight,
            decimal decimalRight => intLeft == decimalRight,
            float floatRight => intLeft == floatRight,
            short shortRight => intLeft == shortRight,
            uint uintRight => intLeft == uintRight,
            ulong ulongRight => intLeft >= 0 && (ulong)intLeft == ulongRight,
            ushort ushortRight => intLeft == ushortRight,
            _ => Equals(intLeft, right),
        },
        long longLeft => right switch
        {
            double doubleRight => longLeft == doubleRight,
            int intRight => longLeft == intRight,
            long longRight => longLeft == longRight,
            decimal decimalRight => longLeft == decimalRight,
            float floatRight => longLeft == floatRight,
            short shortRight => longLeft == shortRight,
            uint uintRight => longLeft == uintRight,
            ulong ulongRight => longLeft >= 0 && (ulong)longLeft == ulongRight,
            ushort ushortRight => longLeft == ushortRight,
            _ => Equals(longLeft, right),
        },
        decimal decimalLeft => right switch
        {
            double doubleRight => doubleRight is < (double)decimal.MaxValue and > (double)decimal.MinValue &&
                                  decimalLeft == (decimal)doubleRight,
            int intRight => decimalLeft == intRight,
            long longRight => decimalLeft == longRight,
            decimal decimalRight => decimalLeft == decimalRight,
            float floatRight => floatRight is < (float)decimal.MaxValue and > (float)decimal.MinValue &&
                                decimalLeft == (decimal)floatRight,
            short shortRight => decimalLeft == shortRight,
            uint uintRight => decimalLeft == uintRight,
            ulong ulongRight => decimalLeft == ulongRight,
            ushort ushortRight => decimalLeft == ushortRight,
            _ => Equals(decimalLeft, right),
        },
        float floatLeft => right switch
        {
            double doubleRight => floatLeft == doubleRight,
            int intRight => floatLeft == intRight,
            long longRight => floatLeft == longRight,
            decimal decimalRight => floatLeft is < (float)decimal.MaxValue and > (float)decimal.MinValue &&
                                    (decimal)floatLeft == decimalRight,
            float floatRight => floatLeft == floatRight,
            short shortRight => floatLeft == shortRight,
            uint uintRight => floatLeft == uintRight,
            ulong ulongRight => floatLeft == ulongRight,
            ushort ushortRight => floatLeft == ushortRight,
            _ => Equals(floatLeft, right),
        },
        short shortLeft => right switch
        {
            double doubleRight => shortLeft == doubleRight,
            int intRight => shortLeft == intRight,
            long longRight => shortLeft == longRight,
            decimal decimalRight => shortLeft == decimalRight,
            float floatRight => shortLeft == floatRight,
            short shortRight => shortLeft == shortRight,
            uint uintRight => shortLeft == uintRight,
            ulong ulongRight => shortLeft >= 0 && (ulong)shortLeft == ulongRight,
            ushort ushortRight => shortLeft == ushortRight,
            _ => Equals(shortLeft, right),
        },
        uint uintLeft => right switch
        {
            double doubleRight => uintLeft == doubleRight,
            int intRight => uintLeft == intRight,
            long longRight => uintLeft == longRight,
            decimal decimalRight => uintLeft == decimalRight,
            float floatRight => uintLeft == floatRight,
            short shortRight => uintLeft == shortRight,
            uint uintRight => uintLeft == uintRight,
            ulong ulongRight => uintLeft == ulongRight,
            ushort ushortRight => uintLeft == ushortRight,
            _ => Equals(uintLeft, right),
        },
        ulong ulongLeft => right switch
        {
            double doubleRight => ulongLeft == doubleRight,
            int intRight => intRight >= 0 && ulongLeft == (ulong)intRight,
            long longRight => longRight >= 0 && ulongLeft == (ulong)longRight,
            decimal decimalRight => ulongLeft == decimalRight,
            float floatRight => ulongLeft == floatRight,
            short shortRight => shortRight >= 0 && ulongLeft == (ulong)shortRight,
            uint uintRight => ulongLeft == uintRight,
            ulong ulongRight => ulongLeft == ulongRight,
            ushort ushortRight => ulongLeft == ushortRight,
            _ => Equals(ulongLeft, right),
        },
        ushort ushortLeft => right switch
        {
            double doubleRight => ushortLeft == doubleRight,
            int intRight => ushortLeft == intRight,
            long longRight => ushortLeft == longRight,
            decimal decimalRight => ushortLeft == decimalRight,
            float floatRight => ushortLeft == floatRight,
            short shortRight => ushortLeft == shortRight,
            uint uintRight => ushortLeft == uintRight,
            ulong ulongRight => ushortLeft == ulongRight,
            ushort ushortRight => ushortLeft == ushortRight,
            _ => Equals(ushortLeft, right),
        },
        _ => Equals(left, right),
    };
}