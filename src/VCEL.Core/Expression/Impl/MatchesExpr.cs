using System.Collections.Generic;
using System.Text.RegularExpressions;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl;

public class MatchesExpr<T> : BinaryExprBase<T>
{
    private readonly Dictionary<string, Regex?> cache
        = new Dictionary<string, Regex?>();

    public MatchesExpr(
        IMonad<T> monad,
        IExpression<T> left,
        IExpression<T> right)
        : base(monad, left, right)
    {
    }

    public override T Evaluate(object? lv, object? rv)
    {
        if (lv is string ls && rv is string rs)
        {
            if (!cache.TryGetValue(rs, out var regex))
            {
                regex = RegexHelper.CreateRegexPattern(rs);
                cache[rs] = regex;
            }
            return Monad.Lift(regex?.IsMatch(ls) ?? false);
        }
        return Monad.Lift(false);
    }
}