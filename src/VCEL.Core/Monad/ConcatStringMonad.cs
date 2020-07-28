using VCEL.Monad;

namespace VCEL.Expression
{
    using System;
    using Monad = M<string>;

    public class ConcatStringMonad : IMonad<M<string>>
    {
        public Monad Unit => new Monad("");
        public Monad Bind(Monad m, Func<object, Monad> f) => new Monad(f(m.Value).Value);
        public Monad Lift(object value) => new Monad(value.ToString());
        public static ConcatStringMonad Instance { get; } = new ConcatStringMonad();
    }
}
