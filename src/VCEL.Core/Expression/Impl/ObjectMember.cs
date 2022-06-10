using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ObjectMember<TMonad> : IExpression<TMonad>
    {
        public IExpression<TMonad> Obj { get; }
        public IExpression<TMonad> Member { get; }

        public ObjectMember(IMonad<TMonad> monad, 
            IExpression<TMonad> obj, 
            IExpression<TMonad> member) 
        {
            Monad = monad;
            Obj = obj;
            Member = member;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies 
            => Obj.Dependencies.Union(Member.Dependencies).Distinct();

        public TMonad Evaluate(IContext<TMonad> context) 
        {
            var mo = this.Obj.Evaluate(context);
            return Monad.Bind(mo, BindMember);

            TMonad BindMember(object? o)
            {
                return o != null && context.TryGetContext(o, out var c)
                    ? Member.Evaluate(c)
                    : Monad.Unit;
            }
        }
    }
}
