using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VCEL;
using VCEL.CSharp;
using VCEL.CSharp.Expression;
using VCEL.Monad;

internal class ToCSharpInStringSetOp : IExpression<string>
{
    private static int setCounter;

    private string setMemberName;
    private readonly ToCSharpPropertyOp propertyExpr;

    public ToCSharpInStringSetOp(
        IMonad<string> monad,
        ToCSharpPropertyOp propertyExpr,
        ToCSharpSetOp setExpr)
    {
        setMemberName = $"str_set_{Interlocked.Increment(ref setCounter)}";
        var memberDependency = new CSharpMemberDependency(
            setMemberName,
            $"private static readonly HashSet<string> {setMemberName} = [{string.Join(",", setExpr.Set.Select(x => $@"""{x}"""))}];");

        this.propertyExpr = propertyExpr;
        Monad = monad;
        Dependencies = propertyExpr.Dependencies.Union([memberDependency]);
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies { get; }

    public string Evaluate(IContext<string> context)
    {
        var contextPropName = $"({context.Value}.{propertyExpr.Name})";
        return $@"{contextPropName} is string ? {setMemberName}.Contains({contextPropName}.ToString()) : false";
    }
}