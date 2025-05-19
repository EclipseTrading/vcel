using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpLetExpr : IExpression<string>
{
    private readonly IReadOnlyList<(string, IExpression<string>)> bindings;
    private readonly IExpression<string> expr;

    public ToCSharpLetExpr(IMonad<string> monad, IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
    {
        this.Monad = monad;
        this.bindings = bindings;
        this.expr = expr;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        var propFuncs = this.bindings
            .Select(b => b.Item1)
            .Distinct()
            .ToDictionary(p => p, p => new Func<string>(() => p));

        var jsContext = new CSharpObjectContext(Monad, context.Value, propFuncs);

        var letCauses = this.bindings.Select(b => $"var {b.Item1} = {b.Item2.Evaluate(jsContext)}");
        var returnCause = this.expr.Evaluate(jsContext);

        var result = $"(new Func<object>(() => {{{string.Join("; ", letCauses)}; return ({returnCause});}})())";
        return result;
    }
}