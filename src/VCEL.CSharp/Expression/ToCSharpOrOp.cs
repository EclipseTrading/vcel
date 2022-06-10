using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpOrOp : BinaryExprBase<string>
    {
        public ToCSharpOrOp(
            IMonad<string> monad,
            IExpression<string> left,
            IExpression<string> right)
            : base(monad, left, right)
        {
        }

        public override string Evaluate(object? lv, object? rv)
        {
            return Equals(lv, "null")
                ? "null"
                : $"(((bool){lv}) ? true : ((bool?){rv}))";
        }
    }
}