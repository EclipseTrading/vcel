using NUnit.Framework;
using System;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class ComparisonOpTests
    {
        [TestCase("Pos > 100000 or FreeTextFilter matches '(?i)123'", true)]
        [TestCase("Pos > 10 and FreeTextFilter matches '(?i)123'", true)]
        [TestCase("FreeTextFilter matches '(?i)123' and Pos > 10", true)]
        [TestCase("(FreeTextFilter matches '(?i)123') and Pos > 10", true)]
        public void ComparsionMatchWithBooleanExpr(string exprStr, bool expected)
        {
            var expr = VCExpression.ParseDefault(exprStr);

            var obj = new
            {
                FreeTextFilter = "123",
                Pos = 100
            };

            var result = expr.Expression.Evaluate(obj);
            Assert.AreEqual(expected, result);
        }

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
        [TestCase("null == null", true)]
        [TestCase("null == false", false)]
        [TestCase("false == null", false)]
        [TestCase("null == 4.2", false)]
        [TestCase("null == 'ABC'", false)]
        [TestCase("'ABC' == null", false)]
        public void Eq(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("'ABC' != 'ABC'", false)]
        [TestCase("'ABC' != 'DEF'", true)]
        [TestCase("'A' + 'BC' != 'ABC'", false)]
        [TestCase("'ABC' != 'A' + 'BC'", false)]
        [TestCase("'A' + 'BC' != 'AB' + 'C'", false)]
        [TestCase("'A' + 'BC' != 'AB' + 'F'", true)]
        [TestCase("null != 1", true)]
        [TestCase("1 != null", true)]
        [TestCase("null != 'ABC'", true)]
        [TestCase("'ABC' != null", true)]
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

        [TestCase("A < L")]
        [TestCase("X < Y")]
        [TestCase("null < null")]
        public void LessNone(string exprString)
            => CompareMaybeNone(exprString, new { A = (double?)null, L = (double?)null });

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
        [TestCase("'A' in {'C', 'B', 'A' }", true)]
        [TestCase("'A' in {'A', 'A', 'A' }", true)]
        [TestCase("'A' in {'B', 'B', 'B' }", false)]
        [TestCase("'A' in {'D', 'B', 'C' }", false)]
        [TestCase("'A' + 'B' in {'AB', 'B', 'C' }", true)]
        [TestCase("3 in {1, 2, 3 }", true)]
        [TestCase("4 in {1, 2, 3 }", false)]
        [TestCase("4 - 1 in {1, 2, 3 }", true)]
        [TestCase("4.1 in {1.1, 2.1, 4.1 }", true)]
        [TestCase("4.1 in { 1, 2, 4.1, null }", true)]
        [TestCase("4.1 in { 1, 2, null, 4.1 }", true)]
        [TestCase("4.1 in { 1, 2, null }", false)]
        public void In(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("null in null")]
        [TestCase("4.1 in null")]
        [TestCase("'A' in {'A', 'B', Value }")]
        [TestCase("'A' in Value")]
        [TestCase("'A' in 1")]
        [TestCase("'A' in 'A'")]
        public void InInvalid(string exprString)
        {
            var parsed = VCExpression.ParseDefault(exprString);
            Assert.False(parsed.Success);
            var parsedMaybe = VCExpression.ParseMaybe(exprString);
            Assert.False(parsedMaybe.Success);
        }

        [TestCase("null in {1.1, 2.1, 4.1 }")]
        [TestCase("null in { 1.1, null, 4.1 }")]
        public void InMaybe(string exprString)
        {
            var parsedMaybe = VCExpression.ParseMaybe(exprString);
            Assert.True(parsedMaybe.Success);
            var maybeResult = parsedMaybe.Expression.Evaluate(new { });
            Assert.False(maybeResult.HasValue);
        }

        [TestCase("'\\' matches '\\'", true)]
        [TestCase("'\\' ~ '\\'", true)]
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
            => CompareDefault(exprString, expected);

        [TestCase("'ABC' matches null")]
        [TestCase("null matches 'Y.*'")]
        [TestCase("'ABC' ~ null")]
        [TestCase("null ~ 'Y.*'")]
        [TestCase("(null + '') ~ 'Y.*'")]
        [TestCase("(null + '') ~ ('Y' + null)")]
        public void MatchesMaybeNone(string exprString)
            => CompareMaybeNone(exprString);

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
        public void Between(string exprString, bool expected)
            => Compare(exprString, expected);

        [TestCase("A between {0, 100}", true, 10.1)]
        [TestCase("A between {0, 100}", false, 120.0)]
        [TestCase("A between {0, 100}", false, -20.0)]
        public void BetweenWithDecimal(string exprString, bool expected, object a)
            => CompareResult(exprString, expected, new { A = Convert.ToDecimal(a) });

        [TestCase("A between {0, 100}", true, 10.1)]
        [TestCase("A between {0, 100}", false, 120.0)]
        [TestCase("A between {0, 100}", false, -20.0)]
        public void BetweenWithInt(string exprString, bool expected, object a)
            => CompareResult(exprString, expected, new { A = Convert.ToInt32(a) });

        [TestCase("A between {0, 100}", true, 10.1)]
        [TestCase("A between {0, 100}", false, 120.0)]
        [TestCase("A between {0, 100}", false, -20.0)]
        public void BetweenWithDouble(string exprString, bool expected, object a)
            => CompareResult(exprString, expected, new { A = Convert.ToDouble(a) });

        [TestCase("A between {0, 100}", true, 10.1)]
        [TestCase("A between {0, 100}", false, 120.0)]
        [TestCase("A between {0, 100}", false, -20.0)]
        public void BetweenWithLong(string exprString, bool expected, object a)
            => CompareResult(exprString, expected, new { A = Convert.ToInt64(a) });

        [TestCase("A between {@2020-01-01, @2020-01-03}", false, 10.1)]
        [TestCase("A between {@2020-01-01, @2020-01-03}", false, 10.1)]
        public void BetweenWithWrongTypeInObject(string exprString, bool expected, object a)
            => CompareResult(exprString, expected, new { A = Convert.ToDecimal(a) });

        [TestCase("A between {0, 100}", false)]
        [TestCase("A between {0, 100}", false)]
        public void BetweenWithWrongTypeInExpression(string exprString, bool expected)
            => CompareResult(exprString, expected, new { A = DateTime.Now });

        [TestCase("1 between { null, null }")]
        [TestCase("1 between { null, 2 }")]
        [TestCase("1 between { 0, null }")]
        [TestCase("null between {'A', 'C'}")]
        public void BetweenMaybeNone(string exprString)
            => CompareMaybeNone(exprString);

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
            var expr = VCExpression.ParseDefault(exprString);
            Assert.True(expr.Success);
            var result = expr.Expression.Evaluate(o ?? new { });
            Assert.That(result, Is.EqualTo(expected).Within(0.0001));

            var maybeExpr = VCExpression.ParseMaybe(exprString);
            Assert.True(maybeExpr.Success);
            var maybeResult = maybeExpr.Expression.Evaluate(o ?? new { });
            Assert.True(maybeResult.HasValue);
            Assert.That(maybeResult.Value, Is.EqualTo(expected).Within(0.0001));
        }

        private void CompareDefault(string exprString, bool expected, object o = null)
        {
            var expr = VCExpression.ParseDefault(exprString);
            Assert.True(expr.Success);
            var result = expr.Expression.Evaluate(o ?? new { });
            Assert.That(result, Is.EqualTo(expected).Within(0.0001));
        }

        private void CompareMaybeNone(string exprString, object o = null)
        {
            var expr = VCExpression.ParseMaybe(exprString).Expression;
            var result = expr.Evaluate(o ?? new { });
            Assert.That(result.HasValue, Is.EqualTo(false));
        }

        private void CompareResult(string exprString, bool expected, object o)
        {
            var expr = VCExpression.ParseDefault(exprString);
            Assert.True(expr.Success);

            var maybeExpr = VCExpression.ParseMaybe(exprString);
            Assert.True(maybeExpr.Success);

            var result = expr.Expression.Evaluate(o ?? new { });
            var maybeResult = maybeExpr.Expression.Evaluate(o ?? new { });

            Assert.True(maybeResult.HasValue);
            Assert.AreEqual(expected, maybeResult.Value);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CompareNoneAfterPropertyCache()
        {
            var o = new { };
            var expr = VCExpression.ParseMaybe("A + B");
            expr.Expression.Evaluate(o); // First Eval to Cache Property Infos
            var res = expr.Expression.Evaluate(o);
            Assert.That(res.HasValue, Is.False);
        }
    }
}
