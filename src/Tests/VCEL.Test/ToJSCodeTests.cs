using NUnit.Framework;
using System;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.JS;

namespace VCEL.Test;

public class ToJsCodeTests
{
    private readonly ExpressionParser<string> parser;

    public ToJsCodeTests()
    {
        var jsParserfactory = new ToJsCodeFactory<string>(ConcatStringMonad.Instance);
        parser = new ExpressionParser<string>(jsParserfactory);
    }

    [TestCase("_row.day / 365 + Increment", "((vcelContext._row.day / 365) + vcelContext.Increment)")]
    [TestCase("let x = (3.0 * (_row.A > 0 ? _row.A: 1) / 100) in (Get('B', 'C') * (1- 2.718^(-x)))/x * D",
        "(() => {let x = ((3 * ((vcelContext._row.A > 0) ? vcelContext._row.A : 1)) / 100); return ((((Get('B','C')) * (1 - (Math.pow(2.718, -x)))) / x) * vcelContext.D);})()")]
    [TestCase("fd.startswith('AAA')==false and P < Prev(P)",
        "(((vcelContext.fd?.startsWith('AAA') ?? false) === false) && (vcelContext.P < (Prev(vcelContext.P))))")]
    [TestCase("t == 'C'", "((vcelContext.t?.valueOf() ?? null) === 'C')")]
    [TestCase("t == s", "((vcelContext.t?.valueOf() ?? null) === (vcelContext.s?.valueOf() ?? null))")]
    [TestCase("t != s", "((vcelContext.t?.valueOf() ?? null) !== (vcelContext.s?.valueOf() ?? null))")]
    [TestCase("(D > 500000 or D < -500000)", "((vcelContext.D > 500000) || (vcelContext.D < -500000))")]
    [TestCase("(D > 123456789)", "(vcelContext.D > 123456789)")]
    [TestCase("(D > 1234567890)", "(vcelContext.D > 1234567890)")]
    [TestCase("(_.env.user == 'jdoe2')", "(underscoreContext._.env.user === 'jdoe2')")]
    [TestCase("(env._.user == 'jdoe2')", "(vcelContext.env._.user === 'jdoe2')")]
    [TestCase("(D > 12345678901)", "(vcelContext.D > 12345678901)")]
    [TestCase("(D > 133148940000)", "(vcelContext.D > 133148940000)")]
    [TestCase("(D > 133148940000000)", "(vcelContext.D > 133148940000000)")]
    [TestCase("(D > 12345F)", "(vcelContext.D > 12345)")]
    [TestCase("(D > 12345.67F)", "(vcelContext.D > 12345.67)")]
    [TestCase("code matches '(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)'", "new RegExp(/(?:.+,|^)([0-9]\\d\\d)(?:,.+|$)/gm).test(vcelContext.code)")]
    [TestCase("(K == 'C' or K == 'AC')", "(((vcelContext.K?.valueOf() ?? null) === 'C') || ((vcelContext.K?.valueOf() ?? null) === 'AC'))")]
    [TestCase("(a < t and a > 0)", "((vcelContext.a < vcelContext.t) && (vcelContext.a > 0))")]
    [TestCase("s == null", "((vcelContext.s?.valueOf() ?? null) === null)")]
    [TestCase("s != null", "((vcelContext.s?.valueOf() ?? null) !== null)")]
    [TestCase("s != null and s == 'ACTIVE'",
        "(((vcelContext.s?.valueOf() ?? null) !== null) && ((vcelContext.s?.valueOf() ?? null) === 'ACTIVE'))")]
    [TestCase("$test != 1", "((vcelContext.$test?.valueOf() ?? null) !== 1)")]
    public void TestJsParser_ProdRulesExamples(string expr, string expected)
    {
        var result = parser.Parse(expr);
        var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

        Assert.AreEqual(result.Success, true);
        Console.WriteLine(parsedExpr);
        Assert.AreEqual(expected, parsedExpr);
    }

    [TestCase("(a + b) * c -(d - f)/g",
        "(((vcelContext.a + vcelContext.b) * vcelContext.c) - ((vcelContext.d - vcelContext.f) / vcelContext.g))")]
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

    [TestCase("!value.a.b.c", "!vcelContext.value.a.b.c")]
    [TestCase("-value.a.b.c", "-vcelContext.value.a.b.c")]
    [TestCase("value.a.b.c", "vcelContext.value.a.b.c")]
    [TestCase("value.a.b.c > 100", "(vcelContext.value.a.b.c > 100)")]
    [TestCase("value.a.b.c.startswith('AAA')", "(vcelContext.value.a.b.c?.startsWith('AAA') ?? false)")]
    public void TestJsPropertyAccess(string expr, string expected)
    {
        var result = parser.Parse(expr);
        var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

        Assert.AreEqual(result.Success, true);
        Console.WriteLine(parsedExpr);
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
    [TestCase("mod(x, 2)", "(mod(vcelContext.x,2))")]
    [TestCase("int(x)", "(Math.floor(Number(vcelContext.x)))")]
    [TestCase("long(x)", "(Math.floor(Number(vcelContext.x)))")]
    [TestCase("double(x)", "(Number(vcelContext.x))")]
    [TestCase("decimal(x)", "(Number(vcelContext.x))")]
    [TestCase("round(x)", "(Math.round(vcelContext.x))")]
    [TestCase("sign(x)", "(Math.sign(vcelContext.x))")]
    [TestCase("sin(x)", "(Math.sin(vcelContext.x))")]
    [TestCase("sinh(x)", "(Math.sinh(vcelContext.x))")]
    [TestCase("sqrt(x)", "(Math.sqrt(vcelContext.x))")]
    [TestCase("tan(x)", "(Math.tan(vcelContext.x))")]
    [TestCase("tanh(x)", "(Math.tanh(vcelContext.x))")]
    [TestCase("truncate(x)", "(Math.trunc(vcelContext.x))")]
    [TestCase("string(x)", "(String(vcelContext.x))")]
    [TestCase("string(date(1583310915341))", "(String((new Date(1583310915341))))")]
    [TestCase("now()", "(new Date())")]
    [TestCase("today()", "(new Date())")]
    [TestCase("datetime(x)", "(new Date(vcelContext.x))")]
    [TestCase("date(x)", "(new Date(vcelContext.x))")]
    // FIXME, we should not allow methods and use functions instead
    [TestCase("abc.uppercase()", "(vcelContext.abc?.toUpperCase() ?? undefined)")]
    // FIXME, we should not allow methods and use functions instead
    [TestCase("abc.lowercase()", "(vcelContext.abc?.toLowerCase() ?? undefined)")]
    // FIXME, we should not allow methods and use functions instead
    [TestCase("abc.startswith('c')", "(vcelContext.abc?.startsWith('c') ?? false)")]
    [TestCase("(abs(UnderlyingPrice - Barrier))", "(Math.abs((vcelContext.UnderlyingPrice - vcelContext.Barrier)))")]
    [TestCase("uppercase(abc)", "(vcelContext.abc?.toUpperCase() ?? undefined)")]
    [TestCase("lowercase(abc)", "(vcelContext.abc?.toLowerCase() ?? undefined)")]
    [TestCase("uppercase('abc')", "('abc'?.toUpperCase() ?? undefined)")]
    [TestCase("lowercase('abc')", "('abc'?.toLowerCase() ?? undefined)")]
    [TestCase("substring(abc, 1, 2)", "(vcelContext.abc?.substring(1,2) ?? undefined)")]
    [TestCase("substring(abc, 2)", "(vcelContext.abc?.substring(2) ?? undefined)")]
    [TestCase("substring('test', 1, 2)", "('test'?.substring(1,2) ?? undefined)")]
    [TestCase("substring('test', 2)", "('test'?.substring(2) ?? undefined)")]
    [TestCase("substring(abc, 1, 2)", "(vcelContext.abc?.substring(1,2) ?? undefined)")]
    [TestCase("substring(abc, 2)", "(vcelContext.abc?.substring(2) ?? undefined)")]
    [TestCase("substring('test', 1, 2)", "('test'?.substring(1,2) ?? undefined)")]
    [TestCase("substring('test', 2)", "('test'?.substring(2) ?? undefined)")]
    [TestCase("replace('abc', 'a', 'b')", "('abc'?.replace('a','b') ?? undefined)")]
    [TestCase("replace(abc, 'a', 'b')", "(vcelContext.abc?.replace('a','b') ?? undefined)")]
    [TestCase("startswith(abc, 'c')", "(vcelContext.abc?.startsWith('c') ?? false)")]
    [TestCase("split(abc, 'c')", "(vcelContext.abc?.split('c') ?? undefined)")]
    [TestCase("split('abc', 'c')", "('abc'?.split('c') ?? undefined)")]
    [TestCase("split(abc, 'c')", "(vcelContext.abc?.split('c') ?? undefined)")]
    [TestCase("replace(abc, 'a', 'b')", "(vcelContext.abc?.replace('a','b') ?? undefined)")]
    [TestCase("replace('abc', 'a', 'b')", "('abc'?.replace('a','b') ?? undefined)")]
    [TestCase("trim(abc)", "(vcelContext.abc?.trim() ?? undefined)")]
    [TestCase("trim('abc')", "('abc'?.trim() ?? undefined)")]
    [TestCase("length(abc)", "(vcelContext.abc?.length ?? 0)")]
    [TestCase("length('abc')", "('abc'?.length ?? 0)")]
    [TestCase("contains(abc, 'a')", "(vcelContext.abc?.includes('a') ?? false)")]
    [TestCase("contains('abc', 'a')", "('abc'?.includes('a') ?? false)")]
    [TestCase("startsWith(abc, 'a')", "(vcelContext.abc?.startsWith('a') ?? false)")]
    [TestCase("startsWith('abc', 'a')", "('abc'?.startsWith('a') ?? false)")]
    [TestCase("endsWith(abc, 'a')", "(vcelContext.abc?.endsWith('a') ?? false)")]
    [TestCase("endsWith('abc', 'a')", "('abc'?.endsWith('a') ?? false)")]
    [TestCase("indexOf(abc, 'a')", "(vcelContext.abc?.indexOf('a') ?? undefined)")]
    [TestCase("indexOf('abc', 'a')", "('abc'?.indexOf('a') ?? undefined)")]
    [TestCase("lastIndexOf(abc, 'a')", "(vcelContext.abc?.lastIndexOf('a') ?? undefined)")]
    [TestCase("lastIndexOf('abc', 'a')", "('abc'?.lastIndexOf('a') ?? undefined)")]
    [TestCase("reverse(abc)", "(vcelContext.abc?.split('').reverse().join('') ?? undefined)")]
    [TestCase("reverse('abc')", "('abc'?.split('').reverse().join('') ?? undefined)")]
    [TestCase("get(abc, 1)", "(vcelContext.abc?.[1] ?? undefined)")]
    [TestCase("get('abc', 1)", "('abc'?.[1] ?? undefined)")]
    [TestCase("get(abc, 1)", "(vcelContext.abc?.[1] ?? undefined)")]
    [TestCase("get('abc', 1)", "('abc'?.[1] ?? undefined)")]
    [TestCase("get(abc, 1)", "(vcelContext.abc?.[1] ?? undefined)")]
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
    [TestCase("a == b", "((vcelContext.a?.valueOf() ?? null) === (vcelContext.b?.valueOf() ?? null))")]
    [TestCase("a != b", "((vcelContext.a?.valueOf() ?? null) !== (vcelContext.b?.valueOf() ?? null))")]
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

    [TestCase("buyerParticipants in {'MOR', 'BBY', 'CAM'}", "['MOR','BBY','CAM'].includes(vcelContext.buyerParticipants)")]
    [TestCase("buyerParticipants in ['MOR', 'BBY', 'CAM']", "['MOR','BBY','CAM'].includes(vcelContext.buyerParticipants)")]
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
            let p = a / l
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

    [TestCase("2 between {1, 3}", "2 >= 1 && 2 <= 3")]
    [TestCase("1 between {1, 3}", "1 >= 1 && 1 <= 3")]
    [TestCase("3 between {1, 3}", "3 >= 1 && 3 <= 3")]
    [TestCase("0 between {1, 3}", "0 >= 1 && 0 <= 3")]
    [TestCase("4 between {1, 3}", "4 >= 1 && 4 <= 3")]
    [TestCase("2.0 between {1.1, 3.3}", "2 >= 1.1 && 2 <= 3.3")]
    [TestCase("1.1 between {1.1, 3.3}", "1.1 >= 1.1 && 1.1 <= 3.3")]
    [TestCase("4.4 between {1.1, 3.3}", "4.4 >= 1.1 && 4.4 <= 3.3")]
    [TestCase("@2020-01-02T00:00:00+08:00 between {@2020-01-01T00:00:00+08:00, @2020-01-03T00:00:00+08:00}",
        "(new Date(1577894400000)) >= (new Date(1577808000000)) && (new Date(1577894400000)) <= (new Date(1577980800000))")]
    [TestCase("@2020-01-04T00:00:00+08:00 between {@2020-01-01T00:00:00+08:00, @2020-01-03T00:00:00+08:00}",
        "(new Date(1578067200000)) >= (new Date(1577808000000)) && (new Date(1578067200000)) <= (new Date(1577980800000))")]
    public void TestJsBetween(string expr, string expected)
    {
        var result = parser.Parse(expr);
        var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

        Assert.AreEqual(result.Success, true);
        Assert.AreEqual(expected, parsedExpr);
    }

    [TestCase("2 in [ ...a, b, 'c' ]", "[...vcelContext.a,vcelContext.b,'c'].includes(2)")]
    public void TestJsList(string expr, string expected)
    {
        var result = parser.Parse(expr);
        var parsedExpr = result.Expression.Evaluate(new JsObjectContext(ConcatStringMonad.Instance, new { }));

        Assert.AreEqual(result.Success, true);
        Assert.AreEqual(expected, parsedExpr);
    }
}
