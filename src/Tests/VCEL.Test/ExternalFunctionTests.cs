using NUnit.Framework;
using VCEL.Core.Expression.Func;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad.Maybe;

namespace VCEL.Test;

public class ExternalFunctionTests
{
    private ExpressionParser<Maybe<object>> parser;

    public ExternalFunctionTests()
    {
        var funcs = new DefaultFunctions<Maybe<object>>();
        funcs.Register("GetValue", (args, context) =>
        {
            var maybe = context.Value;
            if (maybe.HasValue && maybe.Value is TestObj o)
            {
                return o.MemberFunction();
            }

            return "Fail";
        });

        parser = new ExpressionParser<Maybe<object>>(
            new MaybeExpressionFactory(
                MaybeMonad.Instance,
                funcs));
    }

    [Test]
    public void AccessContextMemberFunction()
    {
        var parseResult = parser.Parse("GetValue()");
        Assert.AreEqual(true, parseResult.Success);


        var obj = new TestObj("AccessContextMemberFunction");
        var result = parseResult.Expression.Evaluate(obj);

        Assert.AreEqual(true, result.HasValue);
        Assert.AreEqual("AccessContextMemberFunction", result.Value);
    }
}

internal class TestObj
{
    private readonly string resultStr;

    public TestObj(string resultStr)
    {
        this.resultStr = resultStr;
    }

    public string MemberFunction()
    {
        return resultStr;
    }
}