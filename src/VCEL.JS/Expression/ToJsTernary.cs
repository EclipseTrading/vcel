using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal class ToJsTernary : IExpression<string>
{
    private readonly IExpression<string> condition;
    private readonly IExpression<string> trueExpr;
    private readonly IExpression<string> falseExpr;

    public ToJsTernary(IMonad<string> monad, IExpression<string> condition, IExpression<string> trueExpr, IExpression<string> falseExpr)
    {
        this.Monad = monad;
        this.condition = condition;
        this.trueExpr = trueExpr;
        this.falseExpr = falseExpr;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return $"({condition.Evaluate(context)} ? {trueExpr.Evaluate(context)} : {falseExpr.Evaluate(context)})";
    }
}