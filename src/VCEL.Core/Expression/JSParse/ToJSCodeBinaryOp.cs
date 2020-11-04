using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJSCodeBinaryOp : BinaryExprBase<string>
    {
        private readonly string opName;

        public ToJSCodeBinaryOp(
            string opName,
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
            : base(monad, left, right)
        {
            this.opName = opName;
        }

        public override string Evaluate(object lv, object rv)
            => $"({lv} {opName} {rv})";
    }
}