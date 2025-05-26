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

    public override IExpression<string> Set(ISet<object> set)
     => new ToCSharpSetOp(set, Monad);

    public override IExpression<string> List(IReadOnlyList<IExpression<string>> l)
    {
        string GetItems(IContext<string> context)
        {
            var lists = l.Select(e => e switch
            {
                ToCSharpSpreadOp spread => $"(IEnumerable<object>)(object){e.Evaluate(context)}",
                ToCSharpSetOp set => $".. {e.Evaluate(context)}",
                _ => $"new object [] {{ {e.Evaluate(context)} }}"
            });

            return $@"(new IEnumerable<object>[] {{ {string.Join(", ", lists)} }}.SelectMany(e => e)).ToList()";
        }

        return new ToCSharpStringOp(GetItems, Monad);
    }
}
