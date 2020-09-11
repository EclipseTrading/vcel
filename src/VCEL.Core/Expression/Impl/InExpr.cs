using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class InExpr<T> : IExpression<T>
    {
        public InExpr(
            IMonad<T> monad,
            IExpression<T> left,
            ISet<object> set)
        {
            Monad = monad;
            Left = left;
            Set = set;
        }

        public IExpression<T> Left { get; }
        public ISet<object> Set { get; }
        public IMonad<T> Monad { get; }
        public IEnumerable<IDependency> Dependencies => Left.Dependencies;

        public virtual T Evaluate(IContext<T> context)
        {
            var l = Left.Evaluate(context);
            return Monad.Bind(l, Contains);

            T Contains(object lv) => Monad.Lift(Set.Contains(lv));
        }
    }
}
