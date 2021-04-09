namespace VCEL.Core.Expression.Impl
{
    public static class TypeOperation
    {
        public static bool EqualsChecked(object left, object right)
        {
            if (left == null || right == null)
            {
                return (left == null && right == null);
            }
            if (left.GetType() == right.GetType())
            {
                return Equals(left, right);
            }
            return EqualsMixedType(left, right);
        }

        private static bool EqualsMixedType(object left, object right)
        {
            switch (left)
            {
                case double doubleLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return doubleLeft == doubleRight;
                            case int intRight: return doubleLeft == intRight;
                            case long longRight: return doubleLeft == longRight;
                            case decimal decimalRight: return doubleLeft < (double)decimal.MaxValue && doubleLeft > (double)decimal.MinValue && (decimal)doubleLeft == decimalRight;
                            case float floatRight: return doubleLeft == floatRight;
                            case short shortRight: return doubleLeft == shortRight;
                            case uint uintRight: return doubleLeft == uintRight;
                            case ulong ulongRight: return doubleLeft == ulongRight;
                            case ushort ushortRight: return doubleLeft == ushortRight;

                            default: return Equals(doubleLeft, right);
                        }
                    }
                case int intLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return intLeft == doubleRight;
                            case int intRight: return intLeft == intRight;
                            case long longRight: return intLeft == longRight;
                            case decimal decimalRight: return intLeft == decimalRight;
                            case float floatRight: return intLeft == floatRight;
                            case short shortRight: return intLeft == shortRight;
                            case uint uintRight: return intLeft == uintRight;
                            case ulong ulongRight: return intLeft >= 0 && (ulong)intLeft == ulongRight;
                            case ushort ushortRight: return intLeft == ushortRight;

                            default: return Equals(intLeft, right);
                        }
                    }
                case long longLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return longLeft == doubleRight;
                            case int intRight: return longLeft == intRight;
                            case long longRight: return longLeft == longRight;
                            case decimal decimalRight: return longLeft == decimalRight;
                            case float floatRight: return longLeft == floatRight;
                            case short shortRight: return longLeft == shortRight;
                            case uint uintRight: return longLeft == uintRight;
                            case ulong ulongRight: return longLeft >= 0 && (ulong)longLeft == ulongRight;
                            case ushort ushortRight: return longLeft == ushortRight;

                            default: return Equals(longLeft, right);
                        }
                    }
                case decimal decimalLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return doubleRight < (double)decimal.MaxValue && doubleRight > (double)decimal.MinValue && decimalLeft == (decimal)doubleRight;
                            case int intRight: return decimalLeft == intRight;
                            case long longRight: return decimalLeft == longRight;
                            case decimal decimalRight: return decimalLeft == decimalRight;
                            case float floatRight: return floatRight < (float)decimal.MaxValue && floatRight > (float)decimal.MinValue && decimalLeft == (decimal)floatRight;
                            case short shortRight: return decimalLeft == shortRight;
                            case uint uintRight: return decimalLeft == uintRight;
                            case ulong ulongRight: return decimalLeft == ulongRight;
                            case ushort ushortRight: return decimalLeft == ushortRight;

                            default: return Equals(decimalLeft, right);
                        }
                    }
                case float floatLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return floatLeft == doubleRight;
                            case int intRight: return floatLeft == intRight;
                            case long longRight: return floatLeft == longRight;
                            case decimal decimalRight: return floatLeft < (float)decimal.MaxValue && floatLeft > (float)decimal.MinValue && (decimal)floatLeft == decimalRight;
                            case float floatRight: return floatLeft == floatRight;
                            case short shortRight: return floatLeft == shortRight;
                            case uint uintRight: return floatLeft == uintRight;
                            case ulong ulongRight: return floatLeft == ulongRight;
                            case ushort ushortRight: return floatLeft == ushortRight;

                            default: return Equals(floatLeft, right);
                        }
                    }
                case short shortLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return shortLeft == doubleRight;
                            case int intRight: return shortLeft == intRight;
                            case long longRight: return shortLeft == longRight;
                            case decimal decimalRight: return shortLeft == decimalRight;
                            case float floatRight: return shortLeft == floatRight;
                            case short shortRight: return shortLeft == shortRight;
                            case uint uintRight: return shortLeft == uintRight;
                            case ulong ulongRight: return shortLeft >= 0 && (ulong)shortLeft == ulongRight;
                            case ushort ushortRight: return shortLeft == ushortRight;

                            default: return Equals(shortLeft, right);
                        }
                    }
                case uint uintLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return uintLeft == doubleRight;
                            case int intRight: return uintLeft == intRight;
                            case long longRight: return uintLeft == longRight;
                            case decimal decimalRight: return uintLeft == decimalRight;
                            case float floatRight: return uintLeft == floatRight;
                            case short shortRight: return uintLeft == shortRight;
                            case uint uintRight: return uintLeft == uintRight;
                            case ulong ulongRight: return uintLeft == ulongRight;
                            case ushort ushortRight: return uintLeft == ushortRight;

                            default: return Equals(uintLeft, right);
                        }
                    }
                case ulong ulongLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return ulongLeft == doubleRight;
                            case int intRight: return intRight >= 0 && ulongLeft == (ulong)intRight;
                            case long longRight: return longRight >= 0 && ulongLeft == (ulong)longRight;
                            case decimal decimalRight: return ulongLeft == decimalRight;
                            case float floatRight: return ulongLeft == floatRight;
                            case short shortRight: return shortRight >= 0 && ulongLeft == (ulong)shortRight;
                            case uint uintRight: return ulongLeft == uintRight;
                            case ulong ulongRight: return ulongLeft == ulongRight;
                            case ushort ushortRight: return ulongLeft == ushortRight;

                            default: return Equals(ulongLeft, right);
                        }
                    }
                case ushort ushortLeft:
                    {
                        switch (right)
                        {
                            case double doubleRight: return ushortLeft == doubleRight;
                            case int intRight: return ushortLeft == intRight;
                            case long longRight: return ushortLeft == longRight;
                            case decimal decimalRight: return ushortLeft == decimalRight;
                            case float floatRight: return ushortLeft == floatRight;
                            case short shortRight: return ushortLeft == shortRight;
                            case uint uintRight: return ushortLeft == uintRight;
                            case ulong ulongRight: return ushortLeft == ulongRight;
                            case ushort ushortRight: return ushortLeft == ushortRight;

                            default: return Equals(ushortLeft, right);
                        }
                    }

                default: return Equals(left, right);
            }
        }
    }
}
