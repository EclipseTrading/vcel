using System;

namespace VCEL.Core.Helper;

public static class UpCastExtensions
{
    public static bool UpCast(ref object l, ref object r)
    {
        switch (l)
        {
            case double:
                switch (r)
                {
                    case double:
                        return true;
                    case int t:
                        r = (double)t;
                        return true;
                    case long t:
                        r = (double)t;
                        return true;
                    case decimal t:
                        r = (double)t;
                        return true;
                    case float t:
                        r = (double)t;
                        return true;
                    case short t:
                        r = (double)t;
                        return true;
                    case byte t:
                        r = (double)t;
                        return true;
                }

                break;
            case int i:
                switch (r)
                {
                    case int:
                        return true;
                    case double:
                        l = (double)i;
                        return true;
                    case float:
                        l = (float)i;
                        return true;
                    case long:
                        l = (long)i;
                        return true;
                    case decimal:
                        l = (decimal)i;
                        return true;
                    case short t:
                        r = (int)t;
                        return true;
                    case byte t:
                        r = (int)t;
                        return true;
                }

                break;
            case long lo:
                switch (r)
                {
                    case long:
                        return true;
                    case double t:
                        l = (double)lo;
                        return true;
                    case float:
                        l = (float)lo;
                        return true;
                    case int t:
                        r = (long)t;
                        return true;
                    case decimal:
                        l = (decimal)lo;
                        return true;
                    case short t:
                        r = (long)t;
                        return true;
                    case byte t:
                        r = (long)t;
                        return true;
                }

                break;
            case decimal d:
                switch (r)
                {
                    case decimal:
                        return true;
                    case double:
                        l = (double)d;
                        return true;
                    case float t:
                        r = (decimal)t;
                        return true;
                    case int t:
                        r = (decimal)t;
                        return true;
                    case long t:
                        r = (decimal)t;
                        return true;
                    case short t:
                        r = (decimal)t;
                        return true;
                    case byte t:
                        r = (decimal)t;
                        return true;
                }

                break;
            case float f:
                switch (r)
                {
                    case float:
                        break;
                    case double t:
                        l = (double)f;
                        r = t;
                        break;
                    case int t:
                        r = (float)t;
                        break;
                    case long t:
                        r = (float)t;
                        break;
                    case decimal t:
                        r = (float)t;
                        break;
                    case short t:
                        r = (float)t;
                        break;
                    case byte t:
                        r = (float)t;
                        break;
                }

                return true;
            case short i:
                switch (r)
                {
                    case short:
                        return true;
                    case double:
                        l = (double)i;
                        return true;
                    case float:
                        l = (float)i;
                        return true;
                    case long:
                        l = (long)i;
                        return true;
                    case decimal:
                        l = (decimal)i;
                        return true;
                    case int:
                        l = (int)i;
                        return true;
                    case byte t:
                        r = (short)t;
                        return true;
                }

                break;
            case byte i:
                switch (r)
                {
                    case byte:
                        return true;
                    case double:
                        l = (double)i;
                        return true;
                    case float:
                        l = (float)i;
                        return true;
                    case long:
                        l = (long)i;
                        return true;
                    case decimal:
                        l = (decimal)i;
                        return true;
                    case int:
                        l = (int)i;
                        return true;
                    case short t:
                        l = (short)i;
                        return true;
                }

                break;
        }

        return false;
    }

    public static bool TryCompareTo(IComparable l, IComparable r, out int result)
    {
        result = 0;
        switch (l)
        {
            case double ld:
                switch (r)
                {
                    case double t:
                        result = ld.CompareTo(t);
                        return true;
                    case int t:
                        result = ld.CompareTo(t);
                        return true;
                    case long t:
                        result = ld.CompareTo(t);
                        return true;
                    case decimal t:
                        result = ld.CompareTo((double)t);
                        return true;
                    case float t:
                        result = ld.CompareTo(t);
                        return true;
                    case short t:
                        result = ld.CompareTo(t);
                        return true;
                    case byte t:
                        result = ld.CompareTo(t);
                        return true;
                }

                break;
            case int i:
                switch (r)
                {
                    case int t:
                        result = i.CompareTo(t);
                        return true;
                    case double t:
                        result = i.CompareTo((int)t);
                        return true;
                    case float t:
                        result = i.CompareTo((int)t);
                        return true;
                    case long t:
                        result = i.CompareTo((int)t);
                        return true;
                    case decimal t:
                        result = i.CompareTo((int)t);
                        return true;
                    case short t:
                        result = i.CompareTo(t);
                        return true;
                    case byte t:
                        result = i.CompareTo(t);
                        return true;
                }

                break;
            case long lo:
                switch (r)
                {
                    case long t:
                        result = lo.CompareTo(t);
                        return true;
                    case double t:
                        result = lo.CompareTo((long)t);
                        return true;
                    case float t:
                        result = lo.CompareTo((long)t);
                        return true;
                    case int t:
                        result = lo.CompareTo(t);
                        return true;
                    case decimal t:
                        result = lo.CompareTo((long)t);
                        return true;
                    case short t:
                        result = lo.CompareTo(t);
                        return true;
                    case byte t:
                        result = lo.CompareTo(t);
                        return true;
                }

                break;
            case decimal d:
                switch (r)
                {
                    case decimal t:
                        result = d.CompareTo(t);
                        return true;
                    case double t:
                        result = d.CompareTo((decimal)t);
                        return true;
                    case float t:
                        result = d.CompareTo((decimal)t);
                        return true;
                    case int t:
                        result = d.CompareTo(t);
                        return true;
                    case long t:
                        result = d.CompareTo(t);
                        return true;
                    case short t:
                        result = d.CompareTo(t);
                        return true;
                    case byte t:
                        result = d.CompareTo(t);
                        return true;
                }

                break;
            case float f:
                switch (r)
                {
                    case float t:
                        result = f.CompareTo(t);
                        return true;
                    case double t:
                        result = f.CompareTo((float)t);
                        return true;
                    case int t:
                        result = f.CompareTo(t);
                        return true;
                    case long t:
                        result = f.CompareTo(t);
                        return true;
                    case decimal t:
                        result = f.CompareTo((float)t);
                        return true;
                    case short t:
                        result = f.CompareTo(t);
                        return true;
                    case byte t:
                        result = f.CompareTo(t);
                        return true;
                }

                break;
            case short i:
                switch (r)
                {
                    case short t:
                        result = i.CompareTo(t);
                        return true;
                    case double t:
                        result = i.CompareTo((short)t);
                        return true;
                    case float t:
                        result = i.CompareTo((short)t);
                        return true;
                    case long t:
                        result = i.CompareTo((short)t);
                        return true;
                    case decimal t:
                        result = i.CompareTo((short)t);
                        return true;
                    case int t:
                        result = i.CompareTo((short)t);
                        return true;
                    case byte t:
                        result = i.CompareTo(t);
                        return true;
                }

                break;
            case byte i:
                switch (r)
                {
                    case byte t:
                        result = i.CompareTo(t);
                        return true;
                    case double t:
                        result = i.CompareTo((byte)t);
                        return true;
                    case float t:
                        result = i.CompareTo((byte)t);
                        return true;
                    case long t:
                        result = i.CompareTo((byte)t);
                        return true;
                    case decimal t:
                        result = i.CompareTo((byte)t);
                        return true;
                    case int t:
                        result = i.CompareTo((byte)t);
                        return true;
                    case short t:
                        result = i.CompareTo((byte)t);
                        return true;
                }

                break;

            case string s:
                switch (r)
                {
                    case string t:
                        result = String.Compare(s, t, StringComparison.Ordinal);
                        return true;
                }

                break;
        }

        if (l.GetType() != r.GetType()) return false;

        result = l.CompareTo(r);
        return true;
    }
}