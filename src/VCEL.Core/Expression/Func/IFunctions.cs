namespace VCEL.Core.Expression.Func
{
    public interface IFunctions<TMonad>
    {
        bool HasFunction(string name);
        Function<TMonad> GetFunction(string name);
    }
}
