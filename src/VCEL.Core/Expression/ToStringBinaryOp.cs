using VCEL.Core.Expression.Impl;
using VCEL.Core.Lang;
using VCEL.Monad;

namespace VCEL.Core.Expression
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
        
        public ToStringBinaryOp(
            IMonad<string> monad, 
            int tokenType,
            IExpression<string> left, 
            IExpression<string> right)
            : base(monad, left, right)
        {
            opName = VCELParser.TokenName(tokenType);
        }

        public override string Evaluate(object? lv, object? rv)
            =>  $"{lv} {opName} {rv}";
    }
}