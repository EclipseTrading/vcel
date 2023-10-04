using NUnit.Framework;
using System;
using System.Linq;
using VCEL.Test.Shared;

namespace VCEL.Test;

public class FunctionTests
{
    [TestCase("abs(null)", null)]
    [TestCase("acos(null)", null)]
    [TestCase("asin(null)", null)]
    [TestCase("atan(null)", null)]
    [TestCase("atan2(null, null)", null)]
    [TestCase("ceiling(null)", null)]
    [TestCase("cos(null)", null)]
    [TestCase("cosh(null)", null)]
    [TestCase("exp(null)", null)]
    [TestCase("floor(null)", null)]
    [TestCase("log(null)", null)]
    [TestCase("log10(null)", null)]
    [TestCase("pow(null, null)", null)]
    [TestCase("mod(null, null)", null)]
    [TestCase("round(null)", null)]
    [TestCase("round(null, null)", null)]
    [TestCase("sign(null)", null)]
    [TestCase("sin(null)", null)]
    [TestCase("sinh(null)", null)]
    [TestCase("sqrt(null)", null)]
    [TestCase("tan(null)", null)]
    [TestCase("tanh(null)", null)]
    [TestCase("truncate(null)", null)]
    public void EvalNullDefaultFunction(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("abs(A)", null)]
    [TestCase("acos(A)", null)]
    [TestCase("asin(A)", null)]
    [TestCase("atan(A)", null)]
    [TestCase("atan2(A, A)", null)]
    [TestCase("ceiling(A)", null)]
    [TestCase("cos(A)", null)]
    [TestCase("cosh(A)", null)]
    [TestCase("exp(A)", null)]
    [TestCase("floor(A)", null)]
    [TestCase("log(A)", null)]
    [TestCase("log10(A)", null)]
    [TestCase("pow(A, A)", null)]
    [TestCase("mod(A, A)", null)]
    [TestCase("round(A)", null)]
    [TestCase("round(A, A)", null)]
    [TestCase("sign(A)", null)]
    [TestCase("sin(A)", null)]
    [TestCase("sinh(A)", null)]
    [TestCase("sqrt(A)", null)]
    [TestCase("tan(A)", null)]
    [TestCase("tanh(A)", null)]
    [TestCase("truncate(A)", null)]
    public void EvalNullDependencyDefaultFunction(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { A = (object?)null });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("abs(1)", 1)]
    [TestCase("abs(-1)", 1)]
    [TestCase("abs(1.1)", 1.1)]
    [TestCase("abs(-1.1)", 1.1)]
    [TestCase("acos(0.5)", 1.0471975511965979)]
    [TestCase("asin(0.5)", 0.52359877559829893)]
    [TestCase("atan(0.5)", 0.46364760900080609)]
    [TestCase("atan2(1, 0.5)", 1.1071487177940904)]
    [TestCase("ceiling(1.1)", 2)]
    [TestCase("cos(0.5)", 0.87758256189037276)]
    [TestCase("cosh(0.5)", 1.1276259652063807)]
    [TestCase("exp(0.5)", 1.6487212707001282)]
    [TestCase("floor(0.5)", 0)]
    [TestCase("floor(-0.5)", -1)]
    [TestCase("log(0.5)", -0.69314718055994529)]
    [TestCase("log10(0.5)", -0.3010299956639812)]
    [TestCase("pow(0.5, 2)", 0.25)]
    [TestCase("mod(5, 2)", 1)]
    [TestCase("mod(6, 2)", 0)]
    [TestCase("mod(2.5, 2)", 0.5)]
    [TestCase("round(0.5)", 0)]
    [TestCase("round(0.6)", 1)]
    [TestCase("round(-0.5)", 0)]
    [TestCase("round(-0.6)", -1)]
    [TestCase("sign(0.5)", 1)]
    [TestCase("sign(-0.1)", -1)]
    [TestCase("sign(42)", 1)]
    [TestCase("sin(0.5)", 0.479425538604203)]
    [TestCase("sinh(0.5)", 0.52109530549374738)]
    [TestCase("sqrt(0.5)", 0.70710678118654757)]
    [TestCase("tan(0.5)", 0.54630248984379048)]
    [TestCase("tanh(0.5)", 0.46211715726000974)]
    [TestCase("truncate(0.5)", 0)]
    [TestCase("truncate(0.9)", 0)]
    [TestCase("truncate(-1.9)", -1)]
    public void EvalMathematicsFunction(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("min(4, 3, 7)", 3)]
    [TestCase("min(4.5, 3.1, 7.5)", 3.1)]
    [TestCase("min('C', 'B', 'A')", "A")]
    [TestCase("min(4, null, 7)", 4)]
    [TestCase("min(null, null, 7)", 7)]
    [TestCase("min(null)", null)]
    [TestCase("min(null, null, null)", null)]
    [TestCase("min(a)", 1.0)]
    [TestCase("min(b)", null)]
    [TestCase("min(a, b, c)", 1.0)]
    [TestCase("max(4, 3, 7)", 7)]
    [TestCase("max(4.5, 3.0, 7.5)", 7.5)]
    [TestCase("max('C', 'B', 'A')", "C")]
    [TestCase("max(4, null, 7)", 7)]
    [TestCase("max(null, null, 7)", 7)]
    [TestCase("max(null)", null)]
    [TestCase("max(null, null, null)", null)]
    [TestCase("max(a)", 1.0)]
    [TestCase("max(b)", null)]
    [TestCase("max(a, b, c)", 3.0)]
    public void EvalMinMaxFunction(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { a = 1.0, b = (double?)null, c = 3.0 });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("min(@2019-11-01, @2020-01-01)", "2019-11-01")]
    [TestCase("min(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2019-11-01T09:15:35Z")]
    [TestCase("max(@2019-11-01, @2020-01-01)", "2020-01-01")]
    [TestCase("max(@2019-11-01T09:15:35Z, @2020-01-01T09:15:35Z)", "2020-01-01T09:15:35Z")]
    public void EvalDateFunction(string exprString, string dateTime)
    {
        var expected = DateTimeOffset.Parse(dateTime);
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [Test]
    public void EvalNow()
    {
        var exprString = "now()";
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var value = expr.Evaluate(new { });
            Assert.That(value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
        }
    }

    [TestCase("today()", 0)]
    [TestCase("today(1)", 1)]
    [TestCase("today(-1)", -1)]
    [TestCase("today(100)", 100)]
    public void EvalToday(string exprString, int offset)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var value = expr.Evaluate(new { });
            Assert.That(value, Is.EqualTo(DateTime.Today.AddDays(offset)));
        }
    }

    [TestCase("lowercase('ABCD')", "abcd")]
    [TestCase("lowercase(null)", null)]
    [TestCase("uppercase('abcd')", "ABCD")]
    [TestCase("uppercase(null)", null)]
    [TestCase("substring('test_substring', 5)", "substring")]
    [TestCase("substring('test_substring', -5)", null)]
    [TestCase("substring('test_substring', 0, 4)", "test")]
    [TestCase("substring('test_substring', 0, 0)", "")]
    [TestCase("substring('test_substring', 0, 100)", null)]
    [TestCase("substring('test_substring', 100, 100)", null)]
    [TestCase("substring('test_substring', -1, 100)", null)]
    [TestCase("substring('test_substring', -1, -1)", null)]
    [TestCase("substring(null, 0, 4)", null)]
    [TestCase("split('test_split', '_')", new[] { "test", "split" })]
    [TestCase("split('test_split', null)", new[] { "test_split" })]
    [TestCase("split(null, '_')", null)]
    [TestCase("split(null, null)", null)]
    [TestCase("replace('test_replace', '_replace', '')", "test")]
    [TestCase("replace('test_replace', '_replace', '_test')", "test_test")]
    [TestCase("replace(null, '_replace', '_test')", null)]
    [TestCase("replace('test_replace', null, '_test')", "test_replace")]
    [TestCase("replace('test_replace', '_replace', null)", "test")]
    [TestCase("replace(null, null, null)", null)]
    [TestCase("trim(' test_trim ')", "test_trim")]
    [TestCase("trim(null)", null)]
    [TestCase("trim('')", "")]
    [TestCase("length('test_length')", 11)]
    [TestCase("length(null)", null)]
    [TestCase("length('')", 0)]
    [TestCase("contains('test_contains', 'test')", true)]
    [TestCase("contains('test_contains', 'test1')", false)]
    [TestCase("contains(null, 'test1')", false)]
    [TestCase("startsWith('test_startswith', 'test')", true)]
    [TestCase("startsWith('test_startswith', 'test1')", false)]
    [TestCase("startsWith(null, 'test1')", false)]
    [TestCase("endsWith('test_endswith', 'endswith')", true)]
    [TestCase("endsWith('test_endswith', 'endswith1')", false)]
    [TestCase("endsWith('test_endswith', 'test_endswith')", true)]
    [TestCase("endsWith('test_endswith', 'test')", false)]
    [TestCase("endsWith(null, 'endswith1')", false)]
    [TestCase("endsWith('test_endswith', null)", false)]
    [TestCase("indexOf('test_indexof', 'index')", 5)]
    [TestCase("indexOf('test_indexof', 'index1')", null)]
    [TestCase("indexOf(null, 'index1')", null)]
    [TestCase("lastIndexOf('test_lastindexof', 'last')", 5)]
    [TestCase("lastIndexOf('test_lastindexof', 'last1')", null)]
    [TestCase("lastIndexOf('test_lastindexoflast', 'last')", 16)]
    [TestCase("lastIndexOf(null, 'last1')", null)]
    [TestCase("reverse('test_reverse')", "esrever_tset")]
    [TestCase("reverse(null)", null)]
    [TestCase("reverse('')", "")]
    [TestCase("replace('test_replace', '_replace', '')", "test")]
    [TestCase("replace('test_replace', '_replace', '_test')", "test_test")]
    [TestCase("replace(null, '_replace', '_test')", null)]
    [TestCase("replace('test_replace', null, '_test')", "test_replace")]
    [TestCase("replace('test_replace', '_replace', null)", "test")]
    [TestCase("replace(null, null, null)", null)]
    [TestCase("get('test_get', 0)", "t")]
    [TestCase("get('test_get', 1)", "e")]
    [TestCase("get('test_get', 6)", "e")]
    [TestCase("get('test_get', -1)", "t")]
    [TestCase("get('test_get', -2)", "e")]
    [TestCase("get('test_get', 100)", null)]
    [TestCase("get('test_get', -100)", null)]
    [TestCase("get(null, 0)", null)]
    [TestCase("get(null, null)", null)]
    [TestCase("get('test_get', null)", null)]
    [TestCase("get(null, 'test_get')", null)]
    [TestCase("get('test_get', 'test_get')", null)]
    public void EvalStringFunctions(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            if (parseResult is { Success: false } or { ParseErrors.Count: > 0 })
            {
                Assert.Fail(
                    $"Failed to parse expression: {exprString}{Environment.NewLine}{Environment.NewLine}{string.Join(Environment.NewLine, parseResult.ParseErrors.Select(e => e.Message))}");
            }

            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("length([])", 0)]
    [TestCase("length([1, 2, 3])", 3)]
    [TestCase("length([null])", 1)]
    [TestCase("length(null)", null)]
    [TestCase("length([...[1], 2)", 2)]
    [TestCase("contains([1, 2, 3], 1)", true)]
    [TestCase("contains([1, 2, 3], 4)", false)]
    [TestCase("contains([1, 2, 3], null)", false)]
    [TestCase("contains(null, 1)", false)]
    [TestCase("contains(null, null)", false)]
    [TestCase("startsWith([1, 2, 3], 1)", true)]
    [TestCase("startsWith([1, 2, 3], 4)", false)]
    [TestCase("startsWith([1, 2, 3], null)", false)]
    [TestCase("startsWith(null, 1)", false)]
    [TestCase("startsWith(null, null)", false)]
    [TestCase("endsWith([1, 2, 3], 3)", true)]
    [TestCase("endsWith([1, 2, 3], 4)", false)]
    [TestCase("endsWith([1, 2, 3], null)", false)]
    [TestCase("endsWith(null, 1)", false)]
    [TestCase("endsWith(null, null)", false)]
    [TestCase("indexOf([1, 2, 3], 2)", 1)]
    [TestCase("indexOf([1, 2, 3], 4)", null)]
    [TestCase("indexOf([1, 2, 3], null)", null)]
    [TestCase("indexOf(null, 1)", null)]
    [TestCase("indexOf(null, null)", null)]
    [TestCase("lastIndexOf([1, 2, 3], 2)", 1)]
    [TestCase("lastIndexOf([1, 2, 3], 4)", null)]
    [TestCase("lastIndexOf([1, 2, 3], null)", null)]
    [TestCase("lastIndexOf(null, 1)", null)]
    [TestCase("lastIndexOf(null, null)", null)]
    [TestCase("get([1, 2, 3], 0)", 1)]
    [TestCase("get([1, 2, 3], 1)", 2)]
    [TestCase("get([1, 2, 3], 2)", 3)]
    [TestCase("get([1, 2, 3], 3)", null)]
    [TestCase("get([1, 2, 3], -1)", 3)]
    [TestCase("get([1, 2, 3], -4)", null)]
    [TestCase("get([1, 2, 3], 100)", null)]
    [TestCase("get([1, 2, 3], null)", null)]
    [TestCase("get([null], 0)", null)]
    [TestCase("get([null], 1)", null)]
    [TestCase("get([null], -1)", null)]
    [TestCase("get([null], null)", null)]
    [TestCase("get([], null)", null)]
    [TestCase("get([], 0)", null)]
    [TestCase("get([], 1)", null)]
    [TestCase("get([], -1)", null)]
    [TestCase("get(null, 0)", null)]
    [TestCase("get(null, null)", null)]
    [TestCase("reverse([1, 2, 3])", new[] { 3, 2, 1 })]
    [TestCase("reverse([null])", new object?[] { null })]
    [TestCase("reverse(null)", null)]
    [TestCase("reverse([])", new object[0])]
    [TestCase("reverse([1])", new[] { 1 })]
    public void EvalListFunctions(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("lowercase(A)", "ABCD", "abcd")]
    [TestCase("uppercase(A)", "abcd", "ABCD")]
    [TestCase("substring(A, 5)", "test_substring", "substring")]
    [TestCase("substring(A, 0, 4)", "test_substring", "test")]
    [TestCase("split(A, '_')", "test_split", new string[] { "test", "split" })]
    [TestCase("replace(A, '_replace', '')", "test_replace", "test")]
    [TestCase("replace(A, '_replace', '_test')", "test_replace", "test_test")]
    public void EvalStringFunctions_Property(string exprString, string source, object expected)
    {
        var vcelContext = new { A = source };
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(vcelContext);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("workday(@2022-11-04, 0)", "2022-11-4")]
    [TestCase("workday(@2022-11-04, 1)", "2022-11-7")]
    [TestCase("workday(@2022-11-04, 2)", "2022-11-8")]
    [TestCase("workday(@2022-11-04, 7)", "2022-11-15")]
    [TestCase("workday(@2022-11-04, -1)", "2022-11-3")]
    [TestCase("workday(@2022-11-04, 7, [ @2022-11-09 ])", "2022-11-16")]
    [TestCase("workday(@2022-11-04, 7, [ @2022-11-09, @2022-11-16 ])", "2022-11-17")]
    [TestCase("workday(@2022-11-04, 7, [ @2022-11-09, @2022-11-16, @2022-11-22 ])", "2022-11-17")]
    [TestCase("workday(@2022-11-04, 7, [ @2022-11-09, @2022-11-10, @2022-11-11, @2022-11-16, @2022-11-22 ])", "2022-11-21")]
    [TestCase("workday(@2022-11-05, 0", "2022-11-05")]
    [TestCase("workday(startDay1, 0)", "2022-11-4")]
    [TestCase("workday(startDay1, 1)", "2022-11-7")]
    [TestCase("workday(startDay1, 2)", "2022-11-8")]
    [TestCase("workday(startDay1, 7)", "2022-11-15")]
    [TestCase("workday(startDay1, 7, [ holiday1 ])", "2022-11-16")]
    [TestCase("workday(startDay1, 7, [ holiday1, holiday2 ])", "2022-11-17")]
    [TestCase("workday(startDay1, 7, [ holiday1, holiday2, @2022-11-22 ])", "2022-11-17")]
    [TestCase("workday(startDay1, 7, [ holiday1, @2022-11-10, @2022-11-11, holiday2, @2022-11-22 ])", "2022-11-21")]
    [TestCase("workday(startDay2, 0)", "2022-11-05")]
    public void EvalWorkdayFunction(string exprString, string expected)
    {
        var expDate = DateTime.Parse(expected);
        var context = new
        {
            startDay1 = new DateTime(2022, 11, 4),
            startDay2 = new DateTime(2022, 11, 5),
            holiday1 = new DateTime(2022, 11, 9),
            holiday2 = new DateTime(2022, 11, 16),
        };
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(context);
            Assert.That(result, Is.EqualTo(expDate));
        }
    }

    [TestCase("date(1659409442000)", "2022-08-02")]
    [TestCase("datetime(1659409442000)", "2022-08-02 03:04:02")]
    [TestCase("date('2022-08-02')", "2022-08-02")]
    [TestCase("date(l)", "2022-08-02")]
    [TestCase("datetime('2022-08-02 03:04:02')", "2022-08-02 03:04:02")]
    [TestCase("datetime(l)", "2022-08-02 03:04:02")]
    [TestCase("date(dMin)", "")]
    [TestCase("date(dMax)", "")]
    [TestCase("datetime(dMin)", "")]
    [TestCase("datetime(dMax)", "")]
    [TestCase("date(date)", "2022-11-05")]
    [TestCase("date(datetime)", "2022-11-09")]
    [TestCase("datetime(datetime)", "2022-11-09 11:10:00")]
    public void EvalToDateTimeFunctions(string exprString, string dateTime)
    {
        DateTime? expected = dateTime == "" ? null : DateTime.Parse(dateTime);
        var context = new
        {
            dMin = double.MinValue,
            dMax = double.MaxValue,
            l = 1659409442000,
            date = new DateTime(2022, 11, 5),
            datetime = new DateTime(2022, 11, 9, 11, 10, 0)
        };
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(context);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("int(3)", 3)]
    [TestCase("int(3.4)", 3)]
    [TestCase("int(2.6)", 3)]
    [TestCase("int('3')", 3)]
    [TestCase("int('3.1')", 3)]
    [TestCase("long(3)", 3L)]
    [TestCase("long(3.4)", 3L)]
    [TestCase("long(2.6)", 3L)]
    [TestCase("long('3')", 3L)]
    [TestCase("long('3.1')", 3L)]
    [TestCase("double(3)", 3.0)]
    [TestCase("double(3.4)", 3.4)]
    [TestCase("double(2.6)", 2.6)]
    [TestCase("double('3')", 3.0)]
    [TestCase("double('3.1')", 3.1)]
    [TestCase("double('3.1234567891011121314151617181920212223242526')", 3.123456789101112)]
    [TestCase("string(3)", "3")]
    [TestCase("string(3.1)", "3.1")]
    [TestCase("string('False')", "False")]
    [TestCase("string('3.1234567891011121314151617181920212223242526')", "3.1234567891011121314151617181920212223242526")]
    [TestCase("string('hello world')", "hello world")]
    [TestCase("bool(1)", true)]
    [TestCase("bool(0)", false)]
    [TestCase("bool('true')", true)]
    [TestCase("bool('false')", false)]
    [TestCase("bool('True')", true)]
    [TestCase("bool('False')", false)]
    public void EvalTypeConversionFunctions(string exprString, object expected)
    {
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("string(datetime, 'yyyy-MM-dd')", "2022-11-09")]
    [TestCase("string(datetime, 'dd-MM-yyyy')", "09-11-2022")]
    [TestCase("string(datetime, 'MM-dd-yyyy')", "11-09-2022")]
    [TestCase("string(datetime, 'MMMM dd')", "November 09")]
    [TestCase("string(datetime, 'dd MMM')", "09 Nov")]
    [TestCase("string(datetime, 'dd/MM/yyyy')", "09/11/2022")]
    [TestCase("string(datetime, 'dd-MM-yyyy HH:mm:ss')", "09-11-2022 11:10:00")]
    [TestCase("string(datetime, 'dd-MM-yyyy HH:mm')", "09-11-2022 11:10")]
    [TestCase("string(datetime, 'h:mm tt')", "11:10 AM")]
    [TestCase("string(datetime, 'dd/MM/yyyy h:mm tt')", "09/11/2022 11:10 AM")]
    [TestCase("string(datetime, 'dd-MM-yyyy h:mm tt')", "09-11-2022 11:10 AM")]
    [TestCase("string(datetimeoffset, 'yyyy-MM-dd')", "2022-11-09")]
    [TestCase("string(datetimeoffset, 'dd-MM-yyyy')", "09-11-2022")]
    [TestCase("string(datetimeoffset, 'MM-dd-yyyy')", "11-09-2022")]
    [TestCase("string(datetimeoffset, 'MMMM dd')", "November 09")]
    [TestCase("string(datetimeoffset, 'dd MMM')", "09 Nov")]
    [TestCase("string(datetimeoffset, 'dd/MM/yyyy')", "09/11/2022")]
    [TestCase("string(datetimeoffset, 'dd-MM-yyyy HH:mm:ss')", "09-11-2022 11:10:00")]
    [TestCase("string(datetimeoffset, 'dd-MM-yyyy HH:mm')", "09-11-2022 11:10")]
    [TestCase("string(datetimeoffset, 'h:mm tt')", "11:10 AM")]
    [TestCase("string(datetimeoffset, 'dd/MM/yyyy h:mm tt')", "09/11/2022 11:10 AM")]
    [TestCase("string(datetimeoffset, 'dd-MM-yyyy h:mm tt')", "09-11-2022 11:10 AM")]
    public void EvalStringToDateTimeFunction(string exprString, string expected)
    {
        var context = new
        {
            datetime = new DateTime(2022, 11, 9, 11, 10, 0),
            datetimeoffset = new DateTimeOffset(new DateTime(2022, 11, 9, 11, 10, 0))
        };
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(context);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestCase("decimal(3)", "3.0")]
    [TestCase("decimal(3.4)", "3.4")]
    [TestCase("decimal(2.6)", "2.6")]
    [TestCase("decimal('3')", "3.0")]
    [TestCase("decimal('3.1')", "3.1")]
    [TestCase("decimal('3.1234567891011121314151617181920212223242526')", "3.1234567891011121314151617182")]
    public void EvalDecimalTypeConversionFunction(string exprString, object expected)
    {
        Decimal expct = Convert.ToDecimal(expected);
        foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
        {
            var expr = parseResult.Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expct));
        }
    }
}
