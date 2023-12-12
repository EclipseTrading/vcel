using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.Intrinsics.Expression;

public static class AvxGeneric
{
    public static Vector256<T> Add<T>(Vector256<T> a, Vector256<T> b)
        where T : struct
    {
        return (a, b) switch
        {
            (Vector256<float> af, Vector256<float> bf) => (Vector256<T>)(object)Avx.Add(af, bf),
            _ => throw new ArgumentException()
        };
    }
    
    public static Vector256<T> Multiply<T>(Vector256<T> a, Vector256<T> b)
        where T : struct
    {
        return (a, b) switch
        {
            (Vector256<float> af, Vector256<float> bf) => (Vector256<T>)(object)Avx.Multiply(af, bf),
            _ => throw new ArgumentException()
        };
    }


    public static ReadOnlyMemory<T> BinaryOp<T>(
        object? lv, 
        object? rv,
        Func<Vector256<T>, Vector256<T>, Vector256<T>> op)
        where T : struct
    {
        if (lv is not ReadOnlyMemory<T> lf || rv is not ReadOnlyMemory<T> rf
                                           || lf.Length == 0 
                                           || lf.Length != rf.Length)
            return ReadOnlyMemory<T>.Empty;
        
        var results =  new T[lf.Length + lf.Length  % 8];
        var resultVectors = MemoryMarshal.Cast<T, Vector256<T>>(results);
        
        ReadOnlySpan<Vector256<T>> inA = MemoryMarshal.Cast<T, Vector256<T>>(lf.Span);
        ReadOnlySpan<Vector256<T>> inB = MemoryMarshal.Cast<T, Vector256<T>>(rf.Span);
 
        for(var i = 0; i < inA.Length; i++)
        {
            resultVectors[i] = op(inA[i], inB[i]);
        }

        return results.AsMemory();
    }

}

public class IntrinsicBinaryOp<T>(
    IMonad<ReadOnlyMemory<T>> monad,
    IExpression<ReadOnlyMemory<T>> left,
    IExpression<ReadOnlyMemory<T>> right,
    Func<Vector256<T>, Vector256<T>, Vector256<T>> op)
    : BinaryExprBase<ReadOnlyMemory<T>>(monad, left, right)
    where T : struct
{
    public override ReadOnlyMemory<T> Evaluate(object? lv, object? rv)
        => AvxGeneric.BinaryOp(lv, rv, op);
}
