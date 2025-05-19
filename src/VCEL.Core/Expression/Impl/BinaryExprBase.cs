using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public abstract class BinaryExprBase<T> : IExpression<T>
{
    public BinaryExprBase(
        IMonad<T> monad,
        IExpression<T> left,
        IExpression<T> right)
    {
        Monad = monad;
        Left = left;
        Right = right;
        EvaluateFunc = Evaluate;
    }

    private System.Func<object?, object?, T> EvaluateFunc { get; }
    public IExpression<T> Left { get; }
    public IExpression<T> Right { get; }
    public IMonad<T> Monad { get; }
    public IEnumerable<IDependency> Dependencies
        => Left.Dependencies.Union(Right.Dependencies).Distinct();

    public virtual T Evaluate(IContext<T> context)
    {
        var l = Left.Evaluate(context);
        var r = Right.Evaluate(context);
        return Monad.Bind(l, r, EvaluateFunc);
    }

    public abstract T Evaluate(object? lv, object? rv);

}