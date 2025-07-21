using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;
internal class ToCSharpInNumericSetOp : IExpression<string>
{
    private static int setCounter;

    private string setMemberName;
    private readonly ToCSharpPropertyOp propertyExpr;

    public ToCSharpInNumericSetOp(
        IMonad<string> monad,
        ToCSharpPropertyOp propertyExpr,
        ToCSharpSetOp setExpr)
    {
        setMemberName = $"numeric_set_{Interlocked.Increment(ref setCounter)}";
        var memberDependency = new CSharpMemberDependency(
            setMemberName,
            $"private static readonly HashSet<double> {setMemberName} = [{string.Join(",", setExpr.Set.Select(x => $@"{x.ToString()}d"))}];");

        this.propertyExpr = propertyExpr;
        Monad = monad;
        Dependencies = propertyExpr.Dependencies.Union([memberDependency]);
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies { get; }

    public string Evaluate(IContext<string> context)
    {
        var contextPropName = $"({context.Value}.{propertyExpr.Name})";
        return $@"CSharpHelper.IsNumber({contextPropName}) ? {setMemberName}.Contains((double){contextPropName}) : false";
    }
}