using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ListExpr<TMonad> : IExpression<TMonad>
    {
        public List<IExpression<TMonad>> List { get; }

        public ListExpr(IMonad<TMonad> monad, IReadOnlyList<IExpression<TMonad>> values)
        {
            List = new List<IExpression<TMonad>>(values);
            Monad = monad;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies => List.SelectMany(i => i.Dependencies).Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            var result = List.Aggregate(
                Monad.Lift(new List<object?>()),
                (c, n) =>  Monad.Bind(
                        c,
                        n is NullExpr<TMonad> 
                            ? Monad.Lift<object?>(null) // We want to support lists with nulls even in a maybe context
                            : n.Evaluate(context),
                        (list, value) =>
                        {
                            var l = (List<object?>)list!;
                            if (n is SpreadExpr<TMonad> && value is IEnumerable<object> en)
                            {
                                l.AddRange(en);
                            }
                            else
                            {
                                l.Add(value);
                            }
                            return Monad.Lift(l);
                        }));
            return result;
        }
    }
}
