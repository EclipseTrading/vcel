using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpCompareOp : BinaryExprBase<string>
    {
        private readonly string opName;

        public ToCSharpCompareOp(
            string opName,
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
            : base(monad, left, right)
        {
            this.opName = opName;
        }

        public override string Evaluate(object? lv, object? rv)
        {
            return $@"(CSharpHelper.UpCastCompare({lv}, {rv}, ""{opName}""))";
        }
    }
}