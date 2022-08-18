using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsCodeInOp : BinaryExprBase<string>
    {
        private readonly string jsMethod;

        public ToJsCodeInOp(
            IMonad<string> monad,
            IExpression<string> l,
            IExpression<string> r,
            string jsMethod)
            : base(monad, l, r)
        {
            this.jsMethod = jsMethod;
        }

        public override string Evaluate(object? lv, object? rv)
        {
            return $"{rv}.{jsMethod}({lv})";
        }
    }
}