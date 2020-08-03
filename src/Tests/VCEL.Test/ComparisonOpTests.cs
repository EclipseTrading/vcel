using NUnit.Framework;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class ComparisonOpTests
    {
        [TestCase("'ABC' == 'ABC'", true)]
        [TestCase("'ABC' == 'DEF'", false)]
        [TestCase("'A' + 'BC' == 'ABC'", true)]
        [TestCase("'ABC' == 'A' + 'BC'", true)]
        [TestCase("'A' + 'BC' == 'AB' + 'C'", true)]
        [TestCase("'A' + 'BC' == 'AB' + 'F'", false)]
        [TestCase("5 == 5", true)]
        [TestCase("5 == 4", false)]
        [TestCase("5.0 == 5.0", true)]
        [TestCase("5.0 == 5.1", false)]
        [TestCase("5 + 1 == 1 + 5", true)]
        [TestCase("true == true", true)]
        [TestCase("false == true", false)]
        [TestCase("false == false", true)]
        public void Eq(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("'ABC' != 'ABC'", false)]
        [TestCase("'ABC' != 'DEF'", true)]
        [TestCase("'A' + 'BC' != 'ABC'", false)]
        [TestCase("'ABC' != 'A' + 'BC'", false)]
        [TestCase("'A' + 'BC' != 'AB' + 'C'", false)]
        [TestCase("'A' + 'BC' != 'AB' + 'F'", true)]
        public void NotEq(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("1 > 0", true)]
        [TestCase("0 > 1", false)]
        [TestCase("1.9 > 0.9", true)]
        [TestCase("0.9 > 1.9", false)]
        [TestCase("'DEF' > 'ABC'", true)]
        [TestCase("'ABC' > 'DEF'", false)]
        [TestCase("'ABC' > 'ABC'", false)]
        [TestCase("2L > 1", true)]
        [TestCase("2 > 1L", true)]
        [TestCase("2.0 > 1", true)]
        [TestCase("2 > 1.0", true)]
        [TestCase("2L > 1.0", true)]
        [TestCase("2.0 > 1L", true)]

        public void Greater(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("0 > null")]
        [TestCase("null > 0")]
        [TestCase("null > null")]
        [TestCase("'X' > 3")]
        public void GreaterNone(string exprString)
            => CompareMaybeNone(exprString);


        [TestCase("1 < 0", false)]
        [TestCase("0 < 1", true)]
        [TestCase("1.9 < 0.9", false)]
        [TestCase("0.9 < 1.9", true)]
        [TestCase("'DEF' < 'ABC'", false)]
        [TestCase("'ABC' < 'DEF'", true)]
        [TestCase("'ABC' < 'ABC'", false)]
        public void Less(string exprString, bool expected)
            => Compare(exprString, expected);


        [TestCase("1 >= 0", true)]
        [TestCase("0 >= 1", false)]
        [TestCase("1.9 >= 0.9", true)]
        [TestCase("0.9 >= 1.9", false)]
        [TestCase("'DEF' >= 'ABC'", true)]
        [TestCase("'ABC' >= 'DEF'", false)]
        [TestCase("'ABC' >= 'ABC'", true)]
        public void GreaterOrEqual(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("1 <= 0", false)]
        [TestCase("0 <= 1", true)]
        [TestCase("1.9 <= 0.9", false)]
        [TestCase("0.9 <= 1.9", true)]
        [TestCase("'DEF' <= 'ABC'", false)]
        [TestCase("'ABC' <= 'DEF'", true)]
        [TestCase("'ABC' <= 'ABC'", true)]
        public void LessOrEqual(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("'A' in {'A', 'B', 'C' }", true)]
        [TestCase("'A' in {'D', 'B', 'C' }", false)]
        [TestCase("'A' + 'B' in {'AB', 'B', 'C' }", true)]
        [TestCase("3 in {1, 2, 3 }", true)]
        [TestCase("4 in {1, 2, 3 }", false)]
        [TestCase("4 - 1 in {1, 2, 3 }", true)]
        [TestCase("4.1 in {1.1, 2.1, 4.1 }", true)]
        [TestCase("null in {1.1, 2.1, 4.1 }", false)]
        [TestCase("4.1 in null", false)]
        [TestCase("null in null", false)]
        [TestCase("4.1 in { 1, 2, null, 4.1 }", true)]
        [TestCase("4.1 in { 1, 2, null }", false)]
        public void In(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("'ABC' matches 'A.*'", true)]
        [TestCase("'A' + 'BC' matches 'A.*'", true)]
        [TestCase("'A' + 'BC' matches 'A' + '.*'", true)]
        [TestCase("'X' matches 'Y.*'", false)]
        [TestCase("'ABC' ~ 'A.*'", true)]
        [TestCase("'X' ~ 'Y.*'", false)]
        [TestCase("'ABC' matches null", false)]
        [TestCase("null matches 'Y.*'", false)]
        [TestCase("'ABC' ~ null", false)]
        [TestCase("null ~ 'Y.*'", false)]
        [TestCase("(null + '') ~ 'Y.*'", false)]
        [TestCase("(null + '') ~ ('Y' + null)", false)]
        public void Matches(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("2 between {1, 3}", true)]
        [TestCase("1 between {1, 3}", true)]
        [TestCase("3 between {1, 3}", true)]
        [TestCase("0 between {1, 3}", false)]
        [TestCase("4 between {1, 3}", false)]
        [TestCase("2.0 between {1.1, 3.3}", true)]
        [TestCase("1.1 between {1.1, 3.3}", true)]
        [TestCase("4.4 between {1.1, 3.3}", false)]
        [TestCase("@2020-01-02 between {@2020-01-01, @2020-01-03}", true)]
        [TestCase("@2020-01-04 between {@2020-01-01, @2020-01-03}", false)]
        [TestCase("null between {'A', 'C'}", false)]
        [TestCase("1 between { null, null }", false)]
        public void Between(string exprString, bool expected)
            => Compare(exprString, expected);


        [TestCase(2.0, 1.0f, true)]
        [TestCase(2.0, 1L, true)]
        [TestCase(2.0, 1, true)]
        [TestCase(2.0, (short)1, true)]
        [TestCase(2.0, (byte)1, true)]
        [TestCase(2.0f, 1.0d, true)]
        [TestCase(2.0f, 1L, true)]
        [TestCase(2.0f, 1, true)]
        [TestCase(2.0f, (short)1, true)]
        [TestCase(2.0f, (byte)1, true)]
        [TestCase(2L, 1.0d, true)]
        [TestCase(2L, 1.0f, true)]
        [TestCase(2L, 1, true)]
        [TestCase(2L, (short)1, true)]
        [TestCase(2L, (byte)1, true)]
        [TestCase(2, 1.0d, true)]
        [TestCase(2, 1L, true)]
        [TestCase(2, 1.0f, true)]
        [TestCase(2, (short)1, true)]
        [TestCase(2, (byte)1, true)]
        [TestCase((short)2, 1.0d, true)]
        [TestCase((short)2, 1L, true)]
        [TestCase((short)2, 1, true)]
        [TestCase((short)2, 1.0f, true)]
        [TestCase((short)2, (byte)1, true)]
        [TestCase((byte)2, 1.0d, true)]
        [TestCase((byte)2, 1L, true)]
        [TestCase((byte)2, 1, true)]
        [TestCase((byte)2, 1.0f, true)]
        [TestCase((byte)2, (short)1, true)]
        public void CompareTypes(object l, object r, bool expected)
            => Compare("A > B", expected, new { A = l, B = r });

        [TestCase(2, 1.0, true)]
        [TestCase(2, 1.0f, true)]
        [TestCase(2, 1L, true)]
        [TestCase(2, 1, true)]
        [TestCase(2, (short)1, true)]
        [TestCase(2, (byte)1, true)]
        [TestCase(2, 3.0, false)]
        [TestCase(2, 3.0f, false)]
        [TestCase(2, 3L, false)]
        [TestCase(2, 3, false)]
        [TestCase(2, (short)3, false)]
        [TestCase(2, (byte)3, false)]
        public void CompareDecimalLeft(double a, object b, bool expected)
            => Compare("A > B", expected, new { A = (decimal)a, B = b });


        [TestCase(2, 1.0, true)]
        [TestCase(2, 1.0f, true)]
        [TestCase(2, 1L, true)]
        [TestCase(2, 1, true)]
        [TestCase(2, (short)1, true)]
        [TestCase(2, (byte)1, true)]
        [TestCase(2, 3.0, false)]
        [TestCase(2, 3.0f, false)]
        [TestCase(2, 3L, false)]
        [TestCase(2, 3, false)]
        [TestCase(2, (short)3, false)]
        [TestCase(2, (byte)3, false)]
        public void CompareDecimalRight(double a, object b, bool expected)
            => Compare("B < A", expected, new { A = (decimal)a, B = b });

        private void Compare(string exprString, bool expected, object o = null)
        {
            var expr = VCExpression.ParseDefault(exprString).Expression;
            var result = expr.Evaluate(o ?? new { });
            Assert.That(result, Is.EqualTo(expected).Within(0.0001));
        }
        private void CompareMaybeNone(string exprString)
        {
            var expr = VCExpression.ParseMaybe(exprString).Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result.HasValue, Is.EqualTo(false));
        }
    }
}
