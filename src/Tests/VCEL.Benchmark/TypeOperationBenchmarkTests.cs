using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using VCEL.Core.Expression.Impl;

namespace VCEL.Benchmark;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class TypeOperationBenchmarkTests
{
    (object? Left, object? Right) nulls = (null, null);
    [Benchmark]
    public bool NullEquals() => TypeOperation.EqualsChecked(nulls.Left, nulls.Right);
    [Benchmark]
    public bool NullEqualsChecked() => TypeOperation.EqualsChecked(nulls.Left, nulls.Right);

    (short Left, short Right) shorts = (5, 5);
    [Benchmark]
    public bool ShortEquals() => TypeOperation.EqualsChecked(shorts.Left, shorts.Right);
    [Benchmark]
    public bool ShortEqualsChecked() => TypeOperation.EqualsChecked(shorts.Left, shorts.Right);

    (ushort Left, ushort Right) ushorts = (5, 5);
    [Benchmark]
    public bool UshortEquals() => TypeOperation.EqualsChecked(ushorts.Left, ushorts.Right);
    [Benchmark]
    public bool UshortEqualsChecked() => TypeOperation.EqualsChecked(ushorts.Left, ushorts.Right);

    (int Left, int Right) ints = (5, 5);
    [Benchmark(Baseline = true)]
    public bool IntEquals() => TypeOperation.EqualsChecked(ints.Left, ints.Right);
    [Benchmark]
    public bool IntEqualsChecked() => TypeOperation.EqualsChecked(ints.Left, ints.Right);

    (uint Left, uint Right) uints = (5, 5);
    [Benchmark]
    public bool UintEquals() => TypeOperation.EqualsChecked(uints.Left, uints.Right);
    [Benchmark]
    public bool UintEqualsChecked() => TypeOperation.EqualsChecked(uints.Left, uints.Right);

    (long Left, long Right) longs = (5, 5);
    [Benchmark]
    public bool LongEquals() => TypeOperation.EqualsChecked(longs.Left, longs.Right);
    [Benchmark]
    public bool LongEqualsChecked() => TypeOperation.EqualsChecked(longs.Left, longs.Right);

    (ulong Left, ulong Right) ulongs = (5, 5);
    [Benchmark]
    public bool UlongEquals() => TypeOperation.EqualsChecked(ulongs.Left, ulongs.Right);
    [Benchmark]
    public bool UlongEqualsChecked() => TypeOperation.EqualsChecked(ulongs.Left, ulongs.Right);

    (double Left, double Right) doubles = (5, 5);
    [Benchmark]
    public bool DoubleEquals() => TypeOperation.EqualsChecked(doubles.Left, doubles.Right);
    [Benchmark]
    public bool DoubleEqualsChecked() => TypeOperation.EqualsChecked(doubles.Left, doubles.Right);

    (decimal Left, decimal Right) decimals = (5, 5);
    [Benchmark]
    public bool DecimalEquals() => TypeOperation.EqualsChecked(decimals.Left, decimals.Right);
    [Benchmark]
    public bool DecimalEqualsChecked() => TypeOperation.EqualsChecked(decimals.Left, decimals.Right);

    (string Left, string Right) strings = ("5", "5");
    [Benchmark]
    public bool StringEquals() => TypeOperation.EqualsChecked(strings.Left, strings.Right);
    [Benchmark]
    public bool StringEqualsChecked() => TypeOperation.EqualsChecked(strings.Left, strings.Right);

    (decimal Left, int Right) decimalints = (5, 5);
    [Benchmark]
    public bool DecimalIntEquals() => TypeOperation.EqualsChecked(decimalints.Left, decimalints.Right);
    [Benchmark]
    public bool DecimalIntEqualsChecked() => TypeOperation.EqualsChecked(decimalints.Left, decimalints.Right);

    (int Left, decimal Right) intdecimals = (5, 5);
    [Benchmark]
    public bool IntDecimalEquals() => TypeOperation.EqualsChecked(intdecimals.Left, intdecimals.Right);
    [Benchmark]
    public bool IntDecimalEqualsChecked() => TypeOperation.EqualsChecked(intdecimals.Left, intdecimals.Right);

    (ulong Left, float Right) ulongfloats = (5, 5);
    [Benchmark]
    public bool ULongFloatEquals() => TypeOperation.EqualsChecked(ulongfloats.Left, ulongfloats.Right);
    [Benchmark]
    public bool ULongFloatEqualsChecked() => TypeOperation.EqualsChecked(ulongfloats.Left, ulongfloats.Right);

    (float Left, ulong Right) floatulongs = (5, 5);
    [Benchmark]
    public bool FloatUlongEquals() => TypeOperation.EqualsChecked(floatulongs.Left, floatulongs.Right);
    [Benchmark]
    public bool FloatUlongEqualsChecked() => TypeOperation.EqualsChecked(floatulongs.Left, floatulongs.Right);
}