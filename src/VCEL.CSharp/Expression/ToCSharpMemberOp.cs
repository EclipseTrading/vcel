using VCEL.Core.Expression.Impl;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpMemberOp : BinaryExprBase<string>
    {
        public ToCSharpMemberOp(
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
            : base(monad, left, right)
        {
        }

        public override string Evaluate(object lv, object rv)
        {
            return rv.ToString().Replace(Constants.DefaultContext, lv.ToString());
        }
    }
}