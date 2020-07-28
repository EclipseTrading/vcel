﻿using NUnit.Framework;
using System;
using System.Diagnostics;
using VCEL.Core.Lang;
using VCEL.Monad.Maybe;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class RealWorldExpressions
    {
        [TestCase(Expressions.TernaryArith1)]
        [TestCase(Expressions.TernaryArith2)]
        [TestCase(Expressions.TernaryArith3)]
        [TestCase(Expressions.NestedTernary1)]
        [TestCase(Expressions.NestedTernary2)]
        [TestCase(Expressions.DateExprTotalDays)]
        [TestCase(Expressions.TimeExprTotalExconds)]
        [TestCase(Expressions.LiteralExpr3, true, "ORDER")]
        [TestCase(Expressions.LiteralExpr1, true, 1.0)]
        [TestCase(Expressions.LiteralExpr2, true, 1.0)]
        [TestCase(Expressions.ArithExpr14, true, 1)]
        [TestCase(Expressions.ArithExpr1)]
        [TestCase(Expressions.ArithExpr2)]
        [TestCase(Expressions.ArithExpr3)]
        [TestCase(Expressions.ArithExpr4)]
        [TestCase(Expressions.ArithExpr5)]
        [TestCase(Expressions.ArithExpr5)]
        [TestCase(Expressions.ArithExpr7)]
        [TestCase(Expressions.ArithExpr8)]
        [TestCase(Expressions.ArithExpr9)]
        [TestCase(Expressions.ArithExpr10)]
        [TestCase(Expressions.ArithExpr11)]
        [TestCase(Expressions.ArithExpr12)]
        [TestCase(Expressions.ArithExpr15)]
        [TestCase(Expressions.ArithExpr16)]
        [TestCase(Expressions.ArithExpr17)]
        [TestCase(Expressions.ArithExpr18)]
        [TestCase(Expressions.ArithExpr19)]
        [TestCase(Expressions.ArithExpr20)]
        [TestCase(Expressions.ArithExpr21)]
        [TestCase(Expressions.ArithExpr22)]
        [TestCase(Expressions.ArithExpr23)]
        [TestCase(Expressions.ArithExpr24)]
        [TestCase(Expressions.ArithExpr25)]
        [TestCase(Expressions.ArithExpr26)]
        [TestCase(Expressions.ArithExpr27)]
        [TestCase(Expressions.ArithExpr28)]
        public void EvalNothing(string exprStr, bool hasValue = false, object expected = null)
        {
            // Verifies that these expressions all return Maybe.Nothing when called with an empty expression
            var expr = VCExpression.ParseMaybe(exprStr);
            var result = expr.Evaluate(new { });
            Assert.That(result.HasValue, Is.EqualTo(hasValue));
            if (hasValue)
            {
                Assert.That(result.Value, Is.EqualTo(expected));
            }
        }

        [TestCase(-1.0, 100.0, 0.01, Expressions.NestedTernary1)]
        [TestCase(-3.0, 100.0, 0.01, Expressions.NestedTernary1)]
        [TestCase(-5.0, 100.0, 0.05, Expressions.NestedTernary1)]
        [TestCase(-15, 100.0, 0.15, Expressions.NestedTernary1)]
        [TestCase(-30, 100.0, 0.3, Expressions.NestedTernary1)]
        [TestCase(-45, 100.0, 0.5, Expressions.NestedTernary1)]
        [TestCase(-65, 100.0, 0.7, Expressions.NestedTernary1)]
        [TestCase(-80, 100.0, 0.85, Expressions.NestedTernary1)]
        [TestCase(-95, 100.0, 0.95, Expressions.NestedTernary1)]
        [TestCase(-99, 100.0, 0.99, Expressions.NestedTernary1)]
        [TestCase(null, 100.0, null, Expressions.NestedTernary1)]
        [TestCase(100 - 1.0, 100.0, 0.01, Expressions.NestedTernary1)]
        [TestCase(100 - 3.0, 100.0, 0.05, Expressions.NestedTernary1)]
        [TestCase(100 - 5.0, 100.0, 0.05, Expressions.NestedTernary1)]
        [TestCase(100 - 15, 100.0, 0.15, Expressions.NestedTernary1)]
        [TestCase(100 - 30, 100.0, 0.3, Expressions.NestedTernary1)]
        [TestCase(100 - 45, 100.0, 0.5, Expressions.NestedTernary1)]
        [TestCase(100 - 65, 100.0, 0.7, Expressions.NestedTernary1)]
        [TestCase(100 - 80, 100.0, 0.85, Expressions.NestedTernary1)]
        [TestCase(100 - 95, 100.0, 0.95, Expressions.NestedTernary1)]
        [TestCase(100 - 99, 100.0, 0.99, Expressions.NestedTernary1)]
        [TestCase(-1.0, 100.0, 0.01, Expressions.LetGuard)]
        [TestCase(-3.0, 100.0, 0.01, Expressions.LetGuard)]
        [TestCase(-5.0, 100.0, 0.05, Expressions.LetGuard)]
        [TestCase(-15, 100.0, 0.15, Expressions.LetGuard)]
        [TestCase(-30, 100.0, 0.3, Expressions.LetGuard)]
        [TestCase(-45, 100.0, 0.5, Expressions.LetGuard)]
        [TestCase(-65, 100.0, 0.7, Expressions.LetGuard)]
        [TestCase(-80, 100.0, 0.85, Expressions.LetGuard)]
        [TestCase(-95, 100.0, 0.95, Expressions.LetGuard)]
        [TestCase(-99, 100.0, 0.99, Expressions.LetGuard)]
        [TestCase(null, null, null, Expressions.LetGuard)]
        public void EvalUnitSwimDeltaExpr(
            object swimDelta,
            object optionEquivSplitPos,
            object expected,
            string exprString)
        {
            var o = new
            {
                PosSwimDelta = swimDelta,
                OptionEquivalentSplitPosition = optionEquivSplitPos
            };
            Maybe<object> result = null;
            var parser = VCExpression.MaybeParser();
            var sw = Stopwatch.StartNew();
            var expr = parser.Parse(exprString);
            var parseTime = sw.Elapsed;
            sw.Restart();
            for (var i = 0; i < 50; i++)
            {
                result = expr.Evaluate(o);
            }
            var evalTime = sw.Elapsed;
            Assert.That(result.Value, Is.EqualTo(expected));

            Console.WriteLine("Parse: " + parseTime.TotalMilliseconds + "ms");
            Console.WriteLine("Eval: " + evalTime.TotalMilliseconds * 20 + "µs");
        }
    }
}

