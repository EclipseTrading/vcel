namespace VCEL.Core.Lang
{
    public interface IExpressionParser<TMonad>
    {
        IExpression<TMonad> Parse(string expression);
    }
}
