namespace VCEL.Monad
{
    public class M<T>
    {
        public M(T value)
        {
            this.Value = value;
        }

        public T Value { get;  }
    }

}
