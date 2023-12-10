using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Intrinsics.Expression;

public class IntrinsicAddOp : BinaryExprBase<float[]>
{
    public IntrinsicAddOp(IMonad<float[]> monad, IExpression<float[]> left, IExpression<float[]> right) 
        : base(monad, left, right) { }

    public override float[] Evaluate(object? lv, object? rv)
    {
        if (lv is not float[] lf || rv is not float[] rf
            || lf.Length == 0 
            || lf.Length != rf.Length)
            return Array.Empty<float>();
        
        var results =  new float[lf.Length + lf.Length  % 8];
            
        var resultVectors = MemoryMarshal.Cast<float, Vector256<float>>(results);

        ReadOnlySpan<Vector256<float>> inA = MemoryMarshal.Cast<float, Vector256<float>>(lf);
        ReadOnlySpan<Vector256<float>> inB = MemoryMarshal.Cast<float, Vector256<float>>(rf);
 
        for(var i = 0; i < inA.Length; i++)
        {
            resultVectors[i] = Avx.Add(inA[i], inB[i]);
        }

        return results;
    }
}

