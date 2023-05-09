using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class SetExpr<TMonad> : IExpression<TMonad>
    {
        public SetExpr(IMonad<TMonad> monad, ISet<IExpression<TMonad>> values)
        {
            Set = values;
            Monad = monad;
        }
        public ISet<IExpression<TMonad>> Set { get; }
        public IMonad<TMonad> Monad { get; }
        
        public IEnumerable<IDependency> Dependencies => Set.SelectMany(i => i.Dependencies).Distinct();
        
        public TMonad Evaluate(IContext<TMonad> context)
        {
            var result = Set.Aggregate(
                Monad.Lift(new HashSet<object?>()),
                (c, n) =>  Monad.Bind(
                    c,
                    n is NullExpr<TMonad> 
                        ? Monad.Lift(null!) // We want to support lists with nulls even in a maybe context
                        : n.Evaluate(context),
                    (set, value) =>
                    {
                        var s = (ISet<object?>)set!;
                        s.Add(value);
                        return Monad.Lift(s);
                    }));
            return result;
        }
    }
}