using System;

namespace VCEL.Core.Expression.Op
{
    public class Binary<T, TResult> : Binary<T, T, TResult>
    {
        public Binary(Func<T, T, TResult> evaluate)
            : base(evaluate)
        {
        }
    }

    public class Binary<TLeft, TRight, TResult> : BinaryBase
    {
        public Binary(Func<TLeft, TRight, TResult> evaluate)
               : base((a, b) => evaluate((TLeft)a, (TRight)b))
        {
        }
        public TResult Evaluate(TLeft l, TRight r) => (TResult)base.Evaluate(l, r);

    }
}
