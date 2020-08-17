using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class ObjectMember<TMonad> : IExpression<TMonad>
    {
        private readonly IExpression<TMonad> obj;
        private readonly IExpression<TMonad> member;

        public ObjectMember(IMonad<TMonad> monad, 
            IExpression<TMonad> obj, 
            IExpression<TMonad> member) 
        {
            this.Monad = monad;
            this.obj = obj;
            this.member = member;
        }

        public IMonad<TMonad> Monad { get; }

        public IEnumerable<IDependency> Dependencies 
            => obj.Dependencies.Union(member.Dependencies).Distinct();

        public TMonad Evaluate(IContext<TMonad> context) 
        {
            var mo = this.obj.Evaluate(context);
            return Monad.Bind(mo, BindMember);

            TMonad BindMember(object o)
            {
                return context.TryGetContext(o, out var c)
                    ? member.Evaluate(c)
                    : Monad.Unit;
            }
        }
    }
}
