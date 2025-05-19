using NUnit.Framework;
using System.Threading.Tasks;
using VCEL.Core.Expression;
using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;
using VCEL.Core.Monad.Tasks;
using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Test;

public class MonadTests
{
    [Test]
    public void DefaultMonad()
    {
        var exprFactory = new ExpressionFactory<object?>(ExprMonad.Instance);
        var parser = new ExpressionParser<object?>(exprFactory);
        var expr = parser.Parse("A + 0.5 + 0.5").Expression;
        var result = expr.Evaluate(new { A = 0.5d });

        Assert.That((double?)result, Is.EqualTo(1.5).Within(0.00000001));
    }

    [Test]
    public void MaybeSome()
    {
        var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
        var parser = new ExpressionParser<Maybe<object>>(exprFactory);
        var expr = parser.Parse("A + 0.5 + 0.5").Expression;
        var result = expr.Evaluate(new { A = 0.5d });

        Assert.That(result.HasValue);
        Assert.That((double)result.Value, Is.EqualTo(1.5).Within(0.00000001));
    }

    [Test]
    public void MaybeNothing()
    {
        var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
        var parser = new ExpressionParser<Maybe<object>>(exprFactory);
        var expr = parser.Parse("A + 0.5 + 0.5").Expression;
        var result = expr.Evaluate(new { });

        Assert.That(result.HasValue, Is.False);
    }

    [Test]
    public async Task TaskM()
    {
        var exprFactory = new ExpressionFactory<Task<object?>>(TaskMonad.Instance);
        var parser = new ExpressionParser<Task<object?>>(exprFactory);
        var expr = parser.Parse("A + 0.5 + 0.5").Expression;

        var resultTask = expr.Evaluate(
            new { A = Task.Delay(500).ContinueWith(t => 0.5d) });
        var result = await resultTask;
        Assert.That(result, Is.EqualTo(1.5).Within(0.000000001));
    }

    [TestCase("A + 0.5 + 0.5")]
    [TestCase("1 + 1 == 2")]
    [TestCase("(1 + 1) == 2")]
    [TestCase("2 == 2")]
    [TestCase("true == true")]
    [TestCase("1 + 1")]
    [TestCase("'hello' + 'world'")]
    [TestCase("-(1)")]
    [TestCase("column1 between { 1, 42 }")]
    [TestCase("column1 between { 1, 42 } ? 'a' : 'b'")]
    [TestCase("one == 1 ? 'a' : 'b'")]
    [TestCase("column1 in { 1, 2, 3, 4 }")]
    [TestCase("true and true")]
    [TestCase("@2020-01-01T09:00:00.123+08:00")]
    [TestCase("@2020-01-01T09:00:00.123+00:00")]
    [TestCase("09:00:00.123")]
    [TestCase("42.09:00:00.123")]
    [TestCase("1.00:00:00.000")]
    [TestCase("func(1, 2, 'hello')")]
    [TestCase(@"match
| value < 1 = 0
| value < 10 = 5
| value < 100 = 50
| otherwise value / 2")]
    [TestCase(@"let
    ud = PosSwimDelta / OptionEquivalentSplitPosition,
    d = (ud < 0 ? abs(ud) : 1 - abs(ud))
in match
| d <= 0.03 = 0.01
| d <= 0.1 = 0.05
| d <= 0.225 = 0.15
| d <= 0.4 = 0.3
| d <= 0.6 = 0.5
| d <= 0.775 = 0.7
| d <= 0.9 = 0.85
| d <= 0.97 = 0.95
| otherwise 0.99")]
    public void ToStringM(string expression)
    {
        var exprFactory = new ToStringExpressionFactory(ConcatCSharpMonad.Instance);

        var parser = new ExpressionParser<string>(exprFactory);
        var parsed = parser.Parse(expression);
        Assert.That(parsed.Success, string.Join('\n', parsed.ParseErrors), Is.True);

        var expr = parsed.Expression;
        var result = expr.Evaluate(new { A = 0.5d });

        Assert.That(result, Is.EqualTo(expression));
    }

    [Test]
    public void DivideByZero()
    {
        var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
        var parser = new ExpressionParser<Maybe<object>>(exprFactory);
        var expr = parser.Parse("1/0").Expression;
        var result = expr.Evaluate(new { });
        Assert.That(result, Is.EqualTo(Maybe<object>.None));
    }

    [Test]
    public void NaNTest()
    {
        var expr = VCExpression.ParseMaybe("A / B / C * 100");
        var res = expr.Expression.Evaluate(new { A = 1.0, B = 1.0, C = double.NaN });
        Assert.That(res.Value, Is.EqualTo(double.NaN));
    }

    [Test]
    public void FunctionWithNullArgumentTest()
    {
        var expr = VCExpression.ParseMaybe("max(1,2,3,null)");
        var res = expr.Expression.Evaluate(new { });
        Assert.That(res.Value, Is.EqualTo(3));
    }

    [Test]
    public void CustomFunctionWithNullArgumentsTest()
    {
        var func = new DefaultFunctions<Maybe<object>>();
        func.Register("GetValue", (args, context) =>
        {
            return args[0];
        });
        var expr = VCExpression.ParseMaybe("GetValue(1, null, 3, null)", func);
        var res = expr.Expression.Evaluate(new { });
        Assert.That(res.Value, Is.EqualTo(1));
    }
}