namespace VCEL
{
    public interface IValueAccessor<T>
    {
        T GetValue(IContext<T> context);
    }
}