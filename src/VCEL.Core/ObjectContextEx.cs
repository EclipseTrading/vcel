namespace VCEL
{
    public static class ObjectContextEx
    {
        public static TMonad Evaluate<TMonad>(this IExpression<TMonad> expr, object o)
        {
            var context = new ObjectContext<TMonad>(expr.Monad, o);
            return expr.Evaluate(context);
        }
    }
}
