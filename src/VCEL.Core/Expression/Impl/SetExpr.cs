using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class SetExpr<TMonad> : ValueExpr<TMonad, ISet<object>>
    {
        public SetExpr(IMonad<TMonad> monad, ISet<object> value) : base(monad, value)
        {
        }
    }
}