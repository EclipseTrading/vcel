﻿using System;
using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.JS;

namespace VCEL.Test
{
    public class ToJsCodeTests
    {
        private readonly ExpressionParser<string> parser;

        public ToJsCodeTests()
        {
            var jsParserfactory = new ToJsCodeFactory<string>(ConcatStringMonad.Instance);
            parser = new ExpressionParser<string>(jsParserfactory);
        }

        [TestCase("fd.startsWith('AAA')==false and P < Prev(P)", "(((vcelContext.fd ? vcelContext.fd.startsWith('AAA') : false) === false) && (vcelContext.P < (Prev(vcelContext.P))))")]
        [TestCase("t == 'C'", "(vcelContext.t?.valueOf() === 'C')")]
        [TestCase("t == s", "(vcelContext.t?.valueOf() === vcelContext.s?.valueOf())")]
        [TestCase("t !== s", "(vcelContext.t?.valueOf() !== vcelContext.s?.valueOf())")]
        [TestCase("(D > 500000 or D < -500000)", "((vcelContext.D > 500000) || (vcelContext.D < -500000))")]
        [TestCase("(D > 123456789)", "(vcelContext.D > 123456789)")]
        [TestCase("(D > 1234567890)", "(vcelContext.D > 1234567890)")]
        [TestCase("(D > 12345678901)", "(vcelContext.D > 12345678901)")]
        [TestCase("(D > 133148940000)", "(vcelContext.D > 133148940000)")]
        [TestCase("(D > 133148940000000)", "(vcelContext.D > 133148940000000)")]
        [TestCase("(D > 12345F)", "(vcelContext.D > 12345)")]
        [TestCase("(D > 12345.67F)", "(vcelContext.D > 12345.67)")]
        [TestCase("code matches '(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)'", "new RegExp(/(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)/gm).test(vcelContext.code)")]
        [TestCase("(K == 'C' or K == 'AC')", "((vcelContext.K?.valueOf() === 'C') || (vcelContext.K?.valueOf() === 'AC'))")]
        [TestCase("(a < t and a > 0)", "((vcelContext.a < vcelContext.t) && (vcelContext.a > 0))")]
        [TestCase("s == 'ACTIVE'", "(vcelContext.s?.valueOf() === 'ACTIVE')")]
        public void TestJsParser_ProdRulesExamples(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));
            
            Assert.AreEqual(result.Success, true);
            Console.WriteLine(parsedExpr);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("(a + b) * c -(d - f)/g", "(((vcelContext.a + vcelContext.b) * vcelContext.c) - ((vcelContext.d - vcelContext.f) / vcelContext.g))")]
        public void TestJsParser_Paren(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase("!a", "!vcelContext.a")]
        [TestCase("-a", "-vcelContext.a")]
        public void TestJsParser_Op(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("@2020-03-04T08:35:15.341Z.Year", "(new Date(1583310915341)).getFullYear()")]
        [TestCase("@2020-03-04T08:35:15.341Z.Month", "(new Date(1583310915341)).getMonth()")]
        [TestCase("@2020-03-04T08:35:15.341Z.Day", "(new Date(1583310915341)).getDay()")]
        public void TestJsParser_Datetime(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("now()", "(new Date())")]
        [TestCase("today()", "(new Date())")]
        [TestCase("abs(x)", "(Math.abs(vcelContext.x))")]
        [TestCase("acos(x)", "(Math.acos(vcelContext.x))")]
        [TestCase("asin(x)", "(Math.asin(vcelContext.x))")]
        [TestCase("atan(x)", "(Math.atan(vcelContext.x))")]
        [TestCase("atan2(y, x)", "(Math.atan2(vcelContext.y,vcelContext.x))")]
        [TestCase("ceiling(x)", "(Math.ceil(vcelContext.x))")]
        [TestCase("cos(x)", "(Math.cos(vcelContext.x))")]
        [TestCase("cosh(x)", "(Math.cosh(vcelContext.x))")]
        [TestCase("exp(x)", "(Math.exp(vcelContext.x))")]
        [TestCase("floor(x)", "(Math.floor(vcelContext.x))")]
        [TestCase("log(x)", "(Math.log(vcelContext.x))")]
        [TestCase("log10(x)", "(Math.log10(vcelContext.x))")]
        [TestCase("max(x)", "(Math.max(vcelContext.x))")]
        [TestCase("min(x)", "(Math.min(vcelContext.x))")]
        [TestCase("pow(x, 2)", "(Math.pow(vcelContext.x,2))")]
        [TestCase("round(x)", "(Math.round(vcelContext.x))")]
        [TestCase("sign(x)", "(Math.sign(vcelContext.x))")]
        [TestCase("sin(x)", "(Math.sin(vcelContext.x))")]
        [TestCase("sinh(x)", "(Math.sinh(vcelContext.x))")]
        [TestCase("sqrt(x)", "(Math.sqrt(vcelContext.x))")]
        [TestCase("tan(x)", "(Math.tan(vcelContext.x))")]
        [TestCase("tanh(x)", "(Math.tanh(vcelContext.x))")]
        [TestCase("truncate(x)", "(Math.trunc(vcelContext.x))")]
        [TestCase("abc.ToUpper()", "(vcelContext.abc ? vcelContext.abc.toUpperCase() : '')")]
        [TestCase("abc.ToLower()", "(vcelContext.abc ? vcelContext.abc.toLowerCase() : '')")]
        [TestCase("abc.StartsWith('c')", "(vcelContext.abc ? vcelContext.abc.startsWith('c') : false)")]
        [TestCase("(T(System.Math).Abs(UnderlyingPrice - Barrier))", "(Math.abs((vcelContext.UnderlyingPrice - vcelContext.Barrier)))")]
        public void TestJsParser_Functions(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

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
        [TestCase("a == b", "(vcelContext.a?.valueOf() === vcelContext.b?.valueOf())")]
        [TestCase("a != b", "(vcelContext.a?.valueOf() !== vcelContext.b?.valueOf())")]
        [TestCase("a ^ b", "(Math.pow(vcelContext.a, vcelContext.b))")]
        [TestCase("a and b", "(vcelContext.a && vcelContext.b)")]
        [TestCase("a or b", "(vcelContext.a || vcelContext.b)")]
        public void TestJsParser_BinaryOp(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("a ? b : c", "(vcelContext.a ? vcelContext.b : vcelContext.c)")]
        [TestCase("(a + b - c > 10) ? d : f", "((((vcelContext.a + vcelContext.b) - vcelContext.c) > 10) ? vcelContext.d : vcelContext.f)")]
        public void TestJsParser_Ternary(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }

        [TestCase("buyerParticipants in {'MOR', 'BBY', 'CAM'}", "(new Set(['MOR','BBY','CAM'])).has(vcelContext.buyerParticipants)")]
        public void TestJsParser_In(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

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
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }


        [TestCase("let x = 1, y = 2 + x in x + y + position",
            "(() => {let x = 1; let y = (2 + x); return ((x + y) + vcelContext.position);})()")]

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
            "(() => {let p = (vcelContext.a / vcelContext.l); return (() => { " +
            "switch(true) {" +
            "case (p < 0.5): return 'Low'; " +
            "case (p < 0.75): return 'Normal'; " +
            "case (p < 1): return 'Near Breach'; " +
            "case (p < 1.25): return 'Breach'; " +
            "default: return 'Critical'}})();})()")]
        public void TesJSParser_Let(string expr, string expected)
        {
            var result = parser.Parse(expr);
            var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(expected, parsedExpr);
        }
    }
}