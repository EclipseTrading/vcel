using System;
using System.Text.RegularExpressions;

namespace VCEL.Core.Helper
{
    internal static class RegexHelper
    {
        public static bool IsValidRegexPattern(string pattern)
        {
            if (IsValid(pattern))
            {
                return true;
            }
            else
            {
                var escapedPattern = Regex.Escape(pattern);
                return IsValid(escapedPattern);
            }
        }

        public static Regex? CreateRegexPattern(string pattern)
        {
            if (IsValid(pattern))
            {
                return new Regex(pattern);
            }
            else
            {
                var escapedPattern = Regex.Escape(pattern);
                return IsValid(escapedPattern)
                    ? new Regex(escapedPattern)
                    : null;
            }
        }

        private static bool IsValid(string pattern)
        {
            try
            {
                Regex.Match("", pattern);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
