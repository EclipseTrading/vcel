namespace VCEL.Core.Expression.Impl
{
    public static class TypeOperation
    {
        public static bool EqualsMixedType(object left, object right)
        {
            switch (left)
            {
                case short shortLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return shortLeft == shortRight;
                            case ushort ushortRight: return shortLeft == ushortRight;
                            case int intRight: return shortLeft == intRight;
                            case uint uintRight: return shortLeft == uintRight;
                            case long longRight: return shortLeft == longRight;
                            case ulong ulongRight: return shortLeft >= 0 && (ulong)shortLeft == ulongRight;
                            case float floatRight: return shortLeft == floatRight;
                            case double doubleRight: return shortLeft == doubleRight;
                            case decimal decimalRight: return shortLeft == decimalRight;

                            default: return Equals(shortLeft, right);
                        }
                    }
                case ushort ushortLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return ushortLeft == shortRight;
                            case ushort ushortRight: return ushortLeft == ushortRight;
                            case int intRight: return ushortLeft == intRight;
                            case uint uintRight: return ushortLeft == uintRight;
                            case long longRight: return ushortLeft == longRight;
                            case ulong ulongRight: return ushortLeft == ulongRight;
                            case float floatRight: return ushortLeft == floatRight;
                            case double doubleRight: return ushortLeft == doubleRight;
                            case decimal decimalRight: return ushortLeft == decimalRight;

                            default: return Equals(ushortLeft, right);
                        }
                    }
                case int intLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return intLeft == shortRight;
                            case ushort ushortRight: return intLeft == ushortRight;
                            case int intRight: return intLeft == intRight;
                            case uint uintRight: return intLeft == uintRight;
                            case long longRight: return intLeft == longRight;
                            case ulong ulongRight: return intLeft >= 0 && (ulong)intLeft == ulongRight;
                            case float floatRight: return intLeft == floatRight;
                            case double doubleRight: return intLeft == doubleRight;
                            case decimal decimalRight: return intLeft == decimalRight;

                            default: return Equals(intLeft, right);
                        }
                    }
                case uint uintLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return uintLeft == shortRight;
                            case ushort ushortRight: return uintLeft == ushortRight;
                            case int intRight: return uintLeft == intRight;
                            case uint uintRight: return uintLeft == uintRight;
                            case long longRight: return uintLeft == longRight;
                            case ulong ulongRight: return uintLeft == ulongRight;
                            case float floatRight: return uintLeft == floatRight;
                            case double doubleRight: return uintLeft == doubleRight;
                            case decimal decimalRight: return uintLeft == decimalRight;

                            default: return Equals(uintLeft, right);
                        }
                    }
                case long longLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return longLeft == shortRight;
                            case ushort ushortRight: return longLeft == ushortRight;
                            case int intRight: return longLeft == intRight;
                            case uint uintRight: return longLeft == uintRight;
                            case long longRight: return longLeft == longRight;
                            case ulong ulongRight: return longLeft >= 0 && (ulong)longLeft == ulongRight;
                            case float floatRight: return longLeft == floatRight;
                            case double doubleRight: return longLeft == doubleRight;
                            case decimal decimalRight: return longLeft == decimalRight;

                            default: return Equals(longLeft, right);
                        }
                    }
                case ulong ulongLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return shortRight >= 0 && ulongLeft == (ulong)shortRight;
                            case ushort ushortRight: return ulongLeft == ushortRight;
                            case int intRight: return intRight >= 0 && ulongLeft == (ulong)intRight;
                            case uint uintRight: return ulongLeft == uintRight;
                            case long longRight: return longRight >= 0 && ulongLeft == (ulong)longRight;
                            case ulong ulongRight: return ulongLeft == ulongRight;
                            case float floatRight: return ulongLeft == floatRight;
                            case double doubleRight: return ulongLeft == doubleRight;
                            case decimal decimalRight: return ulongLeft == decimalRight;

                            default: return Equals(ulongLeft, right);
                        }
                    }
                case float floatLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return floatLeft == shortRight;
                            case ushort ushortRight: return floatLeft == ushortRight;
                            case int intRight: return floatLeft == intRight;
                            case uint uintRight: return floatLeft == uintRight;
                            case long longRight: return floatLeft == longRight;
                            case ulong ulongRight: return floatLeft == ulongRight;
                            case float floatRight: return floatLeft == floatRight;
                            case double doubleRight: return floatLeft == doubleRight;
                            case decimal decimalRight: return floatLeft < (float)decimal.MaxValue && floatLeft > (float)decimal.MinValue && (decimal)floatLeft == decimalRight;

                            default: return Equals(floatLeft, right);
                        }
                    }
                case double doubleLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return doubleLeft == shortRight;
                            case ushort ushortRight: return doubleLeft == ushortRight;
                            case int intRight: return doubleLeft == intRight;
                            case uint uintRight: return doubleLeft == uintRight;
                            case long longRight: return doubleLeft == longRight;
                            case ulong ulongRight: return doubleLeft == ulongRight;
                            case float floatRight: return doubleLeft == floatRight;
                            case double doubleRight: return doubleLeft == doubleRight;
                            case decimal decimalRight: return doubleLeft < (double)decimal.MaxValue && doubleLeft > (double)decimal.MinValue && (decimal)doubleLeft == decimalRight;

                            default: return Equals(doubleLeft, right);
                        }
                    }
                case decimal decimalLeft:
                    {
                        switch (right)
                        {
                            case short shortRight: return decimalLeft == shortRight;
                            case ushort ushortRight: return decimalLeft == ushortRight;
                            case int intRight: return decimalLeft == intRight;
                            case uint uintRight: return decimalLeft == uintRight;
                            case long longRight: return decimalLeft == longRight;
                            case ulong ulongRight: return decimalLeft == ulongRight;
                            case float floatRight: return floatRight < (float)decimal.MaxValue && floatRight > (float)decimal.MinValue && decimalLeft == (decimal)floatRight;
                            case double doubleRight: return doubleRight < (double)decimal.MaxValue && doubleRight > (double)decimal.MinValue && decimalLeft == (decimal)doubleRight;
                            case decimal decimalRight: return decimalLeft == decimalRight;

                            default: return Equals(decimalLeft, right);
                        }
                    }

                default: return Equals(left, right);
            }
        }
    }
}
