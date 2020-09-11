using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class BetweenExpr<T> : IExpression<T>
    {
        public BetweenExpr(
            IMonad<T> monad,
            IExpression<T> left,
            IExpression<T> right)
        {
            Monad = monad;
            Left = left;
            Right = right;
        }
        public IExpression<T> Left { get; }
        public IExpression<T> Right { get; }
        public IMonad<T> Monad { get; }
        public IEnumerable<IDependency> Dependencies
            => Left.Dependencies.Union(Right.Dependencies).Distinct();

        public T Evaluate(IContext<T> context)
        {
            var lv = Left.Evaluate(context);
            var values = Right.Evaluate(context);

            return Monad.Bind(lv, values, EvaluateBetween);
        }

        private T EvaluateBetween(object l, object v)
        {
            if (l is IComparable left && v is IList list && list.Count == 2)
            {
                if (list[0] is T f && list[1] is T s)
                {
                    return Monad.Bind(f, s, EvaluateBetweenItems);

                    T EvaluateBetweenItems(object first, object second)
                    {
                        var frCmp = left.CompareTo(first);
                        var toCmp = left.CompareTo(second);
                        return Monad.Lift(frCmp >= 0 && toCmp <= 0);
                    }
                }
            }
            return Monad.Lift(false);
        }
    }
}
