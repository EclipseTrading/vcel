using NUnit.Framework;
using VCEL.Core.Expression.JSParse;
using VCEL.Core.Expression.JSParse.Context;
using VCEL.Core.Lang;
using VCEL.Expression;

namespace VCEL.Test
{
    public class ToJsCodeTests
    {
        private ExpressionParser<string> parser;

        public ToJsCodeTests()
        {
            var jsParserfactory = new ToJSCodeFactory<string>(ConcatStringMonad.Instance);
            parser = new ExpressionParser<string>(jsParserfactory);
        }

        [TestCase("t == 'C'", "(vcelContext.t == 'C')")]
        [TestCase("(D > 500000 or D < -500000)", "((vcelContext.D > 500000) || (vcelContext.D < -500000))")]
        [TestCase("code matches '(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)'", "vcelContext.code.match('(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)')  != null")]
        [TestCase("(K == 'C' or K == 'AC')", "((vcelContext.K == 'C') || (vcelContext.K == 'AC'))")]
        [TestCase("(a < t and a > 0)", "((vcelContext.a < vcelContext.t) && (vcelContext.a > 0))")]
        [TestCase("s == 'ACTIVE'", "(vcelContext.s == 'ACTIVE')")]
        public void TestJsParser_ProdRulesExamples(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("(a + b) * c -(d - f)/g", "(((vcelContext.a + vcelContext.b) * vcelContext.c) - ((vcelContext.d - vcelContext.f) / vcelContext.g))")]
        public void TestJsParser_Paren(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase("!a", "!vcelContext.a")]
        [TestCase("-a", "-vcelContext.a")]
        public void TestJsParser_Op(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("@2020-03-04T08:35:15.341Z.Year", "(new Date(1583310915341)).getFullYear()")]
        [TestCase("@2020-03-04T08:35:15.341Z.Month", "(new Date(1583310915341)).getMonth()")]
        [TestCase("@2020-03-04T08:35:15.341Z.Day", "(new Date(1583310915341)).getDay()")]
        public void TestJsParser_Datetime(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("T(System.DateTime).Today", "(new Date()).getDate()")]
        [TestCase("T(System.DateTime).Now", "(new Date()).getTime()")]
        [TestCase("now()", "(Date.now())")]
        [TestCase("today()", "(new Date())")]
        [TestCase("abc.ToUpper()", "(vcelContext.abc.toUpperCase())")]
        [TestCase("abc.ToLower()", "(vcelContext.abc.toLowerCase())")]
        [TestCase("abc.StartsWith('c')", "(vcelContext.abc.startsWith('c'))")]
        [TestCase("(T(System.Math).Abs(UnderlyingPrice - Barrier))", "(Math.abs((vcelContext.UnderlyingPrice - vcelContext.Barrier)))")]
        public void TestJsParser_Functions(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("a > b", "(vcelContext.a > vcelContext.b)")]
        [TestCase("a >= b", "(vcelContext.a >= vcelContext.b)")]
        [TestCase("a < b", "(vcelContext.a < vcelContext.b)")]
        [TestCase("a <= b", "(vcelContext.a <= vcelContext.b)")]
        [TestCase("a + b", "(vcelContext.a + vcelContext.b)")]
        [TestCase("a - b", "(vcelContext.a - vcelContext.b)")]
        [TestCase("a / b", "(vcelContext.a / vcelContext.b)")]
        [TestCase("a * b", "(vcelContext.a * vcelContext.b)")]
        [TestCase("a == b", "(vcelContext.a == vcelContext.b)")]
        [TestCase("a != b", "(vcelContext.a != vcelContext.b)")]
        [TestCase("a ^ b", "(vcelContext.a ^ vcelContext.b)")]
        [TestCase("a and b", "(vcelContext.a && vcelContext.b)")]
        [TestCase("a or b", "(vcelContext.a || vcelContext.b)")]
        public void TestJsParser_BinaryOp(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase("a ? b : c", "(vcelContext.a ? vcelContext.b : vcelContext.c)")]
        [TestCase("(a + b - c > 10) ? d : f", "((((vcelContext.a + vcelContext.b) - vcelContext.c) > 10) ? vcelContext.d : vcelContext.f)")]
        public void TestJsParser_Ternary(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("buyerParticipants in {'MOR', 'BBY', 'CAM'}", "['MOR','BBY','CAM'].includes(vcelContext.buyerParticipants)")]
        public void TestJsParser_In(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase(@"
            match
                | A < 0.03  = 0.01
                | A < 0.1   = 0.05
                | A < 0.225 = 0.15
                | A < 0.4   = 0.3
                | A < 0.6   = 0.5
                | A < 0.775 = 0.7
                | A < 0.9   = 0.85
                | A < 0.97  = 0.95
                | otherwise 0.99",
            "(() => { " +
            "switch(true) " +
            "{case (vcelContext.A < 0.03): return 0.01; " +
            "case (vcelContext.A < 0.1): return 0.05; " +
            "case (vcelContext.A < 0.225): return 0.15; " +
            "case (vcelContext.A < 0.4): return 0.3; " +
            "case (vcelContext.A < 0.6): return 0.5; " +
            "case (vcelContext.A < 0.775): return 0.7; " +
            "case (vcelContext.A < 0.9): return 0.85; " +
            "case (vcelContext.A < 0.97): return 0.95; " +
            "default: return 0.99}})()")]
        public void TestJsParser_Match(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase("let x = 1, y = 2 + x in x + y + position",
            "(() => { let x = 1\n let y = (2 + x)\n return ((x + y) + vcelContext.position)})()")]

        [TestCase(@"
            let 
               p = a / l
            in (match
               | p < 0.5  = 'Low'
               | p < 0.75 = 'Normal'
               | p < 1.0  = 'Near Breach'
               | p < 1.25 = 'Breach'
               | otherwise 'Critical')
            ",
            "(() => { let p = (vcelContext.a / vcelContext.l)\n return (() => { " +
            "switch(true) {" +
            "case (p < 0.5): return 'Low'; " +
            "case (p < 0.75): return 'Normal'; " +
            "case (p < 1): return 'Near Breach'; " +
            "case (p < 1.25): return 'Breach'; " +
            "default: return 'Critical'}})()})()")]
        public void TesJSParser_Let(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JSObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

    }
}