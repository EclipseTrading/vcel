using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJSCodeInOp : BinaryExprBase<string>
    {
        private IMonad<string> monad;
        private IExpression<string> l;
        private object r;

        public ToJSCodeInOp(
            IMonad<string> monad,
            IExpression<string> l,
            IExpression<string> r)
            : base(monad, l, r)
        {
            this.monad = monad;
            this.l = l;
            this.r = r;
        }

        public override string Evaluate(object lv, object rv)
        {
            return $"{rv}.includes({lv})";
        }
    }
}