using System.Collections.Generic;
using VCEL.Core;

namespace VCEL;

public static class ObjectContextExtensions
{
    public static TMonad Evaluate<TMonad>(this IExpression<TMonad> expr, object o)
    {
        var context = new ObjectContext<TMonad>(expr.Monad, o);
        return expr.Evaluate(context);
    }

    public static TMonad Evaluate<TMonad>(this IExpression<TMonad> expr, IReadOnlyDictionary<string, object> o)
    {
        var context = new DictionaryContext<TMonad>(expr.Monad, o);
        return expr.Evaluate(context);
    }
}