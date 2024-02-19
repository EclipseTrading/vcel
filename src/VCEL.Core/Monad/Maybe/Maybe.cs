namespace VCEL.Monad.Maybe;

public interface IMaybe<out T>
{
    bool HasValue { get; }
    T Value { get; }
}

public readonly struct Maybe<T> : IMaybe<T>
{
    public Maybe(T value)
    {
        Value = value;
        HasValue = true;
    }

    public Maybe()
    {
        Value = default!;
        HasValue = false;
    }

    public bool HasValue { get; }
    public T Value { get; }
    public static Maybe<T> Some(T value) => new(value);
    public static Maybe<T> None { get; } = new();
}