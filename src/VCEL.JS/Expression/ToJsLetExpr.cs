using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal sealed class ToJsLetExpr(
    IMonad<string> monad,
    IReadOnlyList<(string, IExpression<string>)> bindings,
    IExpression<string> expr
) : IExpression<string>
{
    public IMonad<string> Monad { get; } = monad;

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        var propFuncs = bindings
            .Select(b => b.Item1)
            .Distinct()
            .ToDictionary(p => p, p => new Func<string>(() => p));

        var jsObjectContext = context is JsObjectContext ctx
            ? ctx.WithOverrides(propFuncs)
            : new JsObjectContext(Monad, context.Value, propFuncs);

        var letCauses = bindings.Select(b => $"let {b.Item1} = {b.Item2.Evaluate(jsObjectContext)}");
        var returnCause = expr.Evaluate(jsObjectContext);

        var result = $"(() => {{{string.Join("; ", letCauses)}; return {returnCause};}})()";
        return result;
    }
}