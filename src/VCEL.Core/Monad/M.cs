namespace VCEL.Monad
{
    public class M<T>
    {
        public M(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
