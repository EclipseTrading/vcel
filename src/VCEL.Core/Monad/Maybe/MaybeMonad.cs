using System;

namespace VCEL.Monad.Maybe
{
    public class MaybeMonad : IMonad<Maybe<object>>
    {
        public Maybe<object> Unit => Maybe<object>.None;

        public Maybe<object> Lift(object? value) => Maybe<object>.Some(value!);

        public Maybe<object> Bind(Maybe<object> a, Func<object, Maybe<object>> f)
        {
            if(!a.HasValue)
            {
                return Maybe<object>.None;
            }
            return f(a.Value);
        }
        public Maybe<object> Bind(Maybe<object> a, Maybe<object> b, Func<object, object, Maybe<object>> f)
        {
            if (!a.HasValue || !b.HasValue)
            {
                return Maybe<object>.None;
            }
            return f(a.Value, b.Value);
        }

        public static MaybeMonad Instance { get; } = new MaybeMonad();
    }
}
