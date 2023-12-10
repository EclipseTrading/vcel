using System.Diagnostics.CodeAnalysis;
using VCEL.Monad;

namespace VCEL.Intrinsics;

public class IntrinsicContext(IReadOnlyList<Dictionary<string, object>> rows) : IContext<float[]>
{
    public bool TryGetAccessor(string propName, out IValueAccessor<float[]> accessor)
    {
        var extra = rows.Count % 8 == 0 ? 0 : 8 - rows.Count % 8; 
        
        var data = new float[rows.Count + extra];
        for (var i = 0; i < rows.Count; i++)
        {
            data[i] = (float)rows[i][propName];
        }
        
        accessor = new IntrinsicsAccessor(data);
        return true;
    }

    public IContext<float[]> OverrideName(string name, float[] br)
    {
        throw new NotImplementedException();
    }

    public IMonad<float[]> Monad => IntrinsicsMonad.Instance;
    public bool TryGetContext(object o, [UnscopedRef] out IContext<float[]> context)
    {
        context = null!;
        return false;
    }

    public float[] Value => Array.Empty<float>();
}

public class IntrinsicsAccessor(float[] data) : IValueAccessor<float[]>
{
    public float[] GetValue(IContext<float[]> _)
    {
        return data;
    }
}