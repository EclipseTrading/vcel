using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpTernary : IExpression<string>
{
    private readonly IExpression<string> condition;
    private readonly IExpression<string> trueExpr;
    private readonly IExpression<string> falseExpr;

    public ToCSharpTernary(
        IMonad<string> monad,
        IExpression<string> condition,
        IExpression<string> trueExpr,
        IExpression<string> falseExpr)
    {
        this.Monad = monad;
        this.condition = condition;
        this.trueExpr = trueExpr;
        this.falseExpr = falseExpr;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => condition.Dependencies.Union(trueExpr.Dependencies).Union(falseExpr.Dependencies);

    public string Evaluate(IContext<string> context)
    {
        return $"(((bool?)({condition.Evaluate(context)}) ?? false) ? {EvaluateValue(trueExpr, context)} : {EvaluateValue(falseExpr, context)})";
    }

    internal static string EvaluateValue(IExpression<string> expression, IContext<string> context)
    {
        var code = expression.Evaluate(context);
        while (expression is ParenExpr<string> or ToCSharpNotOp)
        {
            expression = expression is ParenExpr<string> paren ? paren.Expr : ((ToCSharpNotOp)expression).Operand;
        }

        // Equality used to be dynamic; retain that type at value boundaries, not in conditions.
        return expression is ToCSharpEqOp or ToCSharpNotEqOp ? $"((dynamic)({code}))" : code;
    }
}