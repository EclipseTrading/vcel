using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsMatchesOp : BinaryExprBase<string>
    {
        private readonly IMonad<string> monad;

        public ToJsMatchesOp(
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
            : base(monad, left, right)
        {
            this.monad = monad;
        }

        public override string Evaluate(object lv, object rv)
        {
            return $"new RegExp({rv}).test({lv})";
        }
    }
}