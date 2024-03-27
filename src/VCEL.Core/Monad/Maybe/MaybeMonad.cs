﻿using System;

namespace VCEL.Monad.Maybe
{
    public class MaybeMonad : IMonad<Maybe<object>>
    {
        public Maybe<object> Unit => Maybe<object>.None;

        public Maybe<object> Lift<TValue>(TValue value) => Maybe<object>.Some(value!);

        public Maybe<object> Bind(Maybe<object> a, Func<object, Maybe<object>> f)
        {
            if (!a.HasValue)
            {
                return Maybe<object>.None;
            }

            return f(a.Value);
        }

        public Maybe<object> Bind(Maybe<object> m, IContext<Maybe<object>> context, Func<object?, IContext<Maybe<object>>, Maybe<object>> f)
        {
            if (!m.HasValue)
            {
                return Maybe<object>.None;
            }

            return f(m.Value, context);
        }

        public Maybe<object> Bind<TValue>(Maybe<object> m, IContext<Maybe<object>> context, Func<object?, IContext<Maybe<object>>, TValue, Maybe<object>> f, TValue value)
        {
            if (!m.HasValue)
            {
                return Maybe<object>.None;
            }

            return f(m.Value, context, value);
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
