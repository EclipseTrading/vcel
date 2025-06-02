using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpLetExpr(
    IMonad<string> monad,
    IReadOnlyList<(string, IExpression<string>)> bindings,
    IExpression<string> expr)
    : IExpression<string>
{
    public IMonad<string> Monad { get; } = monad;

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        var propFuncs = bindings
            .Select(b => b.Item1)
            .Distinct()
            .ToDictionary(p => p, p => new Func<string>(() => p));

        var cSharpObjectContext = context is CSharpObjectContext ctx
            ? ctx.WithOverrides(propFuncs)
            : new CSharpObjectContext(Monad, context.Value, propFuncs);

        var letCauses = bindings.Select(b => $"var {b.Item1} = {b.Item2.Evaluate(cSharpObjectContext)}");
        var returnCause = expr.Evaluate(cSharpObjectContext);

        var result =
            $"(new Func<dynamic, object>({cSharpObjectContext.Value} => {{{string.Join("; ", letCauses)}; return ({returnCause});}})({cSharpObjectContext.Value}))";
        return result;
    }
}