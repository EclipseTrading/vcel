using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpGuardExpr : IExpression<string>
{
    private readonly IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses;
    private readonly IExpression<string>? otherwise;

    public ToCSharpGuardExpr(
        IMonad<string> monad,
        IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses,
        IExpression<string>? otherwise)
    {
        this.Monad = monad;
        this.guardClauses = guardClauses;
        this.otherwise = otherwise;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        var guardCases = guardClauses.Select(gc => $"({gc.Item1.Evaluate(context)}) ? ({gc.Item2.Evaluate(context)}) : ").ToList();
        var otherCase =  otherwise == null ? "default" : $"({otherwise.Evaluate(context)})";
        guardCases.Add(otherCase);

        var result = $"({string.Join(" ", guardCases.ToArray())})";
        return result;
    }
}