using System;

namespace VCEL.Core.Helper
{
    public static class UpCastEx
    {
        public static bool UpCast(ref object l, ref object r)
        {
            switch(l)
            {
                case double _:
                    switch(r)
                    {
                        case double _:
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
                    return false;
                case int i:
                    switch (r)
                    {
                        case int _:
                            return true;
                        case double _:
                            l = (double)i;
                            return true;
                        case float _:
                            l = (float)i;
                            return true;
                        case long _:
                            l = (long)i;
                            return true;
                        case decimal _:
                            l = (decimal)i;
                            return true;
                        case short t:
                            r = (int)t;
                            return true;
                        case byte t:
                            r = (int)t;
                            return true;
                    }
                    return false;
                case long lo:
                    switch (r)
                    {
                        case long _:
                            return true;
                        case double t:
                            l = (double)lo;
                            return true;
                        case float _:
                            l = (float)lo;
                            return true;
                        case int t:
                            r = (long)t;
                            return true;
                        case decimal _:
                            l = (decimal)lo;
                            return true;
                        case short t:
                            r = (long)t;
                            return true;
                        case byte t:
                            r = (long)t;
                            return true;
                    }
                    return false;
                case decimal d:
                    switch (r)
                    {
                        case decimal _:
                            return true;
                        case double _:
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
                    return false;
                case float f:
                    switch(r)
                    {
                        case float _:
                            return true;
                        case double t:
                            l = (double)f;
                            r = t;
                            return true;
                        case int t:
                            r = (float)t;
                            return true;
                        case long t:
                            r = (float)t;
                            return true;
                        case decimal t:
                            r = (float)t;
                            return true;
                        case short t:
                            r = (float)t;
                            return true;
                        case byte t:
                            r = (float)t;
                            return true;
                    }
                    return true;
                case short i:
                    switch (r)
                    {
                        case short _:
                            return true;
                        case double _:
                            l = (double)i;
                            return true;
                        case float _:
                            l = (float)i;
                            return true;
                        case long _:
                            l = (long)i;
                            return true;
                        case decimal _:
                            l = (decimal)i;
                            return true;
                        case int _:
                            l = (int)i;
                            return true;
                        case byte t:
                            r = (short)t;
                            return true;
                    }
                    return false;
                case byte i:
                    switch (r)
                    {
                        case byte _:
                            return true;
                        case double _:
                            l = (double)i;
                            return true;
                        case float _:
                            l = (float)i;
                            return true;
                        case long _:
                            l = (long)i;
                            return true;
                        case decimal _:
                            l = (decimal)i;
                            return true;
                        case int _:
                            l = (int)i;
                            return true;
                        case short t:
                            l = (short)i;
                            return true;
                    }
                    return false;
            }
            return false;
        }

    }
}
