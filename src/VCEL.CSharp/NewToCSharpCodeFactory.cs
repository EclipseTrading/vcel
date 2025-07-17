using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.CSharp.Expression;
using VCEL.Monad;

namespace VCEL.CSharp;
public class NewToCSharpCodeFactory : ToCSharpCodeFactory
{
    public NewToCSharpCodeFactory(IMonad<string> monad, IFunctions<string>? functions = null) : base(monad, functions)
    {
    }

    public override IExpression<string> Set(ISet<object> set) => new ToCSharpSetOp(Monad, set);

    public override IExpression<string> In(IExpression<string> l, IExpression<string> r) => (l, r) switch
    {
        (ToCSharpPropertyOp pro, ToCSharpSetOp set) when set.Set.All(i => i is string) => new ToCSharpInStringSetOp(Monad, pro, set),
        (ToCSharpPropertyOp pro, ToCSharpSetOp set) when set.Set.All(CSharpHelper.IsNumber) => new ToCSharpInNumericSetOp(Monad, pro, set),
        _ => base.In(l, r)
    };
}
