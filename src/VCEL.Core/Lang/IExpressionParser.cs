namespace VCEL.Core.Lang
{
    public interface IExpressionParser<T>
    {
        ParseResult<T> Parse(string expression);
    }
}
