using VCEL.Monad;

namespace VCEL.Intrinsics;

public class IntrinsicsMonad : IMonad<float[]>
{
    public float[] Unit => Array.Empty<float>();
    public float[] Lift(object? value)
    {
        return value switch
        {
            float[] arr => arr,
            float f => new[] { f },
            _ => throw new ArgumentException()
        };
    }

    public float[] Bind(float[] a, Func<object?, float[]> f)
    {
        return f(a);
    }

    public float[] Bind(float[] a, float[] b, Func<object?, object?, float[]> f)
    {
        return f(a, b);
    }
    
    
    public static IntrinsicsMonad Instance { get; } = new();
}