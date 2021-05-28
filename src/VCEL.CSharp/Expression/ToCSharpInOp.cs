using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpInOp : BinaryExprBase<string>
    {
        public ToCSharpInOp(
            IMonad<string> monad,
            IExpression<string> l,
            IExpression<string> r)
            : base(monad, l, r)
        {
        }

        public override string Evaluate(object lv, object rv)
        {
            return $"{rv}.Contains({lv})";
        }
    }
}