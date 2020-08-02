namespace VCEL.Monad.Maybe
{
    public class Maybe<T>
    {
        public Maybe(T value)
        {
            Value = value;
            HasValue = true;
        }

        internal Maybe() { }

        public bool HasValue { get; }
        public T Value { get; }
        public static Maybe<T> Some(T value) => new Maybe<T>(value);
        public static Maybe<T> None { get; } = new Maybe<T>();
    }
}
