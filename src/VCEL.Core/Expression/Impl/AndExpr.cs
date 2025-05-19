using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class AndExpr<TMonad> : BooleanExpr<TMonad>
{
    public AndExpr(IMonad<TMonad> monad, IExpression<TMonad> left, IExpression<TMonad> right) 
        : base(monad, left, right)
    {
    }

    public override bool Evaluate(bool l, bool r) => l && r;

    public override bool ShortCircuit(bool l, out bool res)
    {
        res = false;
        return !l;
    }
}