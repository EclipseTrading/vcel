using System;

namespace VCEL.CSharp;

// Used by generated arithmetic to keep promoted numbers unboxed between operators.
public readonly struct CSharpArithmeticValue
{
    private enum ValueKind { Other, Number, Integer, Long, Float }

    private readonly ValueKind kind;
    private readonly double number;
    private readonly long integer;
    private readonly object? other;

    private CSharpArithmeticValue(double number, ValueKind kind = ValueKind.Number, long integer = 0)
    {
        this.number = number;
        this.kind = kind;
        this.integer = integer;
        other = null;
    }

    private CSharpArithmeticValue(object? value)
    {
        other = value;
        kind = ValueKind.Other;
        number = 0;
        integer = 0;
    }

    public static CSharpArithmeticValue Literal(int value) => new(value, ValueKind.Integer, value);
    public static CSharpArithmeticValue Literal(long value) => new(value, ValueKind.Long, value);
    public static CSharpArithmeticValue Literal(float value) => new(value, ValueKind.Float);
    public static CSharpArithmeticValue Literal(double value) => new(value);
    public static CSharpArithmeticValue Literal(double? value)
        => value.HasValue ? new CSharpArithmeticValue(value.Value) : new CSharpArithmeticValue(null);

    public static CSharpArithmeticValue Result(object? value)
        => value is double n ? new CSharpArithmeticValue(n, value) : new CSharpArithmeticValue(value);

    private CSharpArithmeticValue(double number, object boxed)
    {
        this.number = number;
        other = boxed;
        kind = ValueKind.Number;
        integer = 0;
    }

    public static CSharpArithmeticValue Negate(CSharpArithmeticValue value)
        => value.kind == ValueKind.Number
            ? new(-value.number)
            : Result((object?)(-(dynamic)value.ToObject()!));

    public static CSharpArithmeticValue DivisionOperand(CSharpArithmeticValue value)
        => value.kind == ValueKind.Number
            ? value
            : Literal((double?)(dynamic)value.ToObject()!);

    public object? ToObject() => kind switch
    {
        ValueKind.Number => other ?? number,
        ValueKind.Integer => (int)integer,
        ValueKind.Long => integer,
        ValueKind.Float => (float)number,
        _ => other
    };

    public static CSharpArithmeticValue Add(CSharpArithmeticValue left, CSharpArithmeticValue right,
        Func<object?, object?, object?> fallback)
        => left.kind != ValueKind.Other && right.kind != ValueKind.Other
            ? new(left.number + right.number)
            : Result(fallback(left.ToObject(), right.ToObject()));

    public static CSharpArithmeticValue Subtract(CSharpArithmeticValue left, CSharpArithmeticValue right,
        Func<object?, object?, object?> fallback)
        => left.kind != ValueKind.Other && right.kind != ValueKind.Other
            ? new(left.number - right.number)
            : Result(fallback(left.ToObject(), right.ToObject()));

    public static CSharpArithmeticValue Multiply(CSharpArithmeticValue left, CSharpArithmeticValue right,
        Func<object?, object?, object?> fallback)
        => left.kind != ValueKind.Other && right.kind != ValueKind.Other
            ? new(left.number * right.number)
            : Result(fallback(left.ToObject(), right.ToObject()));

    public static CSharpArithmeticValue Divide(CSharpArithmeticValue left, CSharpArithmeticValue right,
        Func<object?, object?, object?> fallback)
        => left.kind != ValueKind.Other && right.kind != ValueKind.Other
            ? new(left.number / right.number)
            : Result(fallback(left.ToObject(), right.ToObject()));
}
