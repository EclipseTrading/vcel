using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Expression
{
    internal class ToStringBinaryOp : BinaryExprBase<string>
    {
        private readonly string opName;

        public ToStringBinaryOp(
            IMonad<string> monad, 
            string opName,
            IExpression<string> left, 
            IExpression<string> right)
            : base(monad, left, right)
        {
            this.opName = opName;
        }

        public override string Evaluate(object lv, object rv)
            =>  $"{lv} {opName} {rv}";
    }
}