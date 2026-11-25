using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.CSharp.CodeGen;

namespace VCEL.Test;

public class CSharpArithmeticAllocationTests
{
    private const string RelativeDeviation = "(Value-(Baseline-Adjustment))/(Baseline-Adjustment)*10000";

    public sealed class Row
    {
        public object? Value { get; set; }
        public object? Baseline { get; set; }
        public object? Adjustment { get; set; }
        public object? Input_value => Value;
        public object? Reference_value => Baseline;
        public object? Calibration_adjustment => Adjustment;
    }

    public sealed class ObservedRow
    {
        public List<string> Reads { get; } = new();
        public object Value { get { Reads.Add("Value"); return 102d; } }
        public object Baseline { get { Reads.Add("Baseline"); return 101d; } }
        public object Adjustment { get { Reads.Add("Adjustment"); return 1d; } }
    }

    public sealed class CustomOperand
    {
        public static string operator +(CustomOperand left, int right) => $"int {right}";
        public static string operator +(CustomOperand left, long right) => $"long {right}";
    }

    public sealed class ConstantOperand
    {
        public static string operator +(ConstantOperand left, byte right) => $"byte {right}";
        public static string operator -(ConstantOperand left, short right) => $"short {right}";
        public static string operator *(ConstantOperand left, uint right) => $"uint {right}";
        public static string operator +(byte left, ConstantOperand right) => $"byte {left}";
    }

    public sealed class IntegralOperand
    {
        public static int operator +(IntegralOperand left, byte right) => right;
        public static int operator -(IntegralOperand value) => -7;
    }

    public sealed class ChangingRow
    {
        private int reads;
        public object Value => reads++ == 0 ? 1d : 5d;
    }

    public sealed class InvalidDivisionRow
    {
        public int RightReads { get; private set; }
        public object Value => "not a number";
        public object Baseline { get { RightReads++; return 2d; } }
    }

    [Test]
    public void DivisionConvertsTheLeftOperandBeforeReadingTheRight()
    {
        var row = new InvalidDivisionRow();
        Assert.Throws<RuntimeBinderException>(() => Compile("Value/Baseline")(row));
        Assert.That(row.RightReads, Is.Zero);
    }

    [TestCase("let x=Value+1 in [x,x]")]
    [TestCase("let x=Value+1,y=x in [x,y]")]
    public void RepeatedLetValuesKeepTheirBoxedIdentity(string text)
    {
        var result = Compile(text)(new Row { Value = 2d });
        var values = ((IEnumerable<object>)result!).ToArray();
        Assert.That(values[0], Is.SameAs(values[1]));
    }

    [TestCase("-Value", -7)]
    [TestCase("-(Value+2)", -2)]
    [TestCase("let x=Value+2 in x*x", 4)]
    [TestCase("let x=Value+2 in -x", -2)]
    public void UnaryAndLetKeepCustomOperatorResultTypes(string text, int expected)
    {
        Assert.That(Compile(text)(new Row { Value = new IntegralOperand() }),
            Is.EqualTo(expected).And.TypeOf<int>());
    }

    [TestCase("let x=Value+2 in x", "byte 2")]
    [TestCase("let x=Value+2 in x+'!'", "byte 2!")]
    public void LetPreservesNonNumericArithmeticResults(string text, string expected)
    {
        Assert.That(Compile(text)(new Row { Value = new ConstantOperand() }), Is.EqualTo(expected));
    }

    [TestCase("let x=Value-Baseline in let y=x+2 in y*3", 9d)]
    [TestCase("let x=Value-Baseline in x", 1d)]
    [TestCase("let x=Value-Baseline in abs(x)", 1d)]
    [TestCase("let __vcelLeft=Value-Baseline in __vcelLeft*2", 2d)]
    [TestCase("let __vcelValue_x=2,x=Value-Baseline in x+__vcelValue_x", 3d)]
    public void LetPreservesScopeAndNonArithmeticUses(string text, double expected)
    {
        Assert.That(Compile(text)(new Row { Value = 102d, Baseline = 101d }),
            Is.EqualTo(expected).And.TypeOf<double>());
    }

    [Test]
    public void LiteralLetBindingsKeepTheirOriginalIntegerArithmetic()
    {
        Assert.That(Compile("let x=2147483647 in x+1")(new Row()),
            Is.EqualTo(int.MinValue).And.TypeOf<int>());
    }

    [TestCase("-Value")]
    [TestCase("-(Value-Baseline)")]
    [TestCase("let x=Value-Baseline in -x")]
    public void UnaryAndLetPreserveNullOperandErrors(string text)
    {
        Assert.Throws<RuntimeBinderException>(() => Compile(text)(new Row { Value = null, Baseline = 2d }));
    }

    [Test]
    public void LetArithmeticPropagatesNull()
    {
        Assert.That(Compile("let x=Value-Baseline in x*x")(new Row { Value = null, Baseline = 2d }), Is.Null);
    }

    [TestCase("Value+2", "byte 2")]
    [TestCase("Value+(2+3)", "byte 5")]
    [TestCase("2+Value", "byte 2")]
    [TestCase("Value-2", "short 2")]
    [TestCase("Value*2", "uint 2")]
    public void PreservesConstantConversionsForCustomOperators(string text, string expected)
    {
        Assert.That(Compile(text)(new Row { Value = new ConstantOperand() }), Is.EqualTo(expected));
    }

    [TestCase(RelativeDeviation, 200d)]
    [TestCase("((Input_value)-(Reference_value-Calibration_adjustment))/(Reference_value-Calibration_adjustment)*10000", 200d)]
    [TestCase("Baseline-Adjustment", 100d)]
    [TestCase("Value+Baseline*Adjustment", 203d)]
    [TestCase("Value*(2+3)", 510d)]
    [TestCase("Value*(5/2)", 255d)]
    [TestCase("-Value", -102d)]
    [TestCase("-(Value-Baseline)", -1d)]
    [TestCase("(-Value+Baseline)*Adjustment", -1d)]
    public void NumericArithmeticOnlyAllocatesTheReturnedBox(string text, double expected)
    {
        var evaluate = Compile(text);
        var row = new Row { Value = 102d, Baseline = 101d, Adjustment = 1d };
        for (var i = 0; i < 1000; i++)
        {
            evaluate(row);
        }

        const int iterations = 10000;
        object? result = null;
        var before = GC.GetAllocatedBytesForCurrentThread();
        for (var i = 0; i < iterations; i++)
        {
            result = evaluate(row);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.That(result, Is.EqualTo(expected));
        TestContext.WriteLine($"{text}: {allocated / (double)iterations} bytes/evaluation");
        Assert.That(allocated, Is.LessThanOrEqualTo(24L * iterations));
    }

    [Test]
    public void RelativeDeviationPreservesMixedNumericAndNullInputs()
    {
        var evaluate = Compile(RelativeDeviation);
        foreach (var row in new[]
                 {
                     new Row { Value = 102, Baseline = 101L, Adjustment = 1m },
                     new Row { Value = 102f, Baseline = (short)101, Adjustment = (byte)1 },
                     new Row { Value = 102d, Baseline = 101u, Adjustment = (ushort)1 },
                     new Row { Value = 102UL, Baseline = (sbyte)101, Adjustment = 1d }
                 })
        {
            Assert.That(evaluate(row), Is.EqualTo(200d));
        }

        Assert.That(evaluate(new Row { Value = null, Baseline = 101d, Adjustment = 1d }), Is.Null);
        Assert.That(evaluate(new Row { Value = 102d, Baseline = null, Adjustment = 1d }), Is.Null);
        Assert.That(evaluate(new Row { Value = 102d, Baseline = 101d, Adjustment = null }), Is.Null);
        Assert.That(evaluate(new Row { Value = 102d, Baseline = 1d, Adjustment = 1d }), Is.EqualTo(double.PositiveInfinity));
        Assert.That(evaluate(new Row { Value = 0d, Baseline = 1d, Adjustment = 1d }), Is.NaN);
        Assert.That(evaluate(new Row { Value = -102d, Baseline = 1d, Adjustment = 1d }), Is.EqualTo(double.NegativeInfinity));
        Assert.That(evaluate(new Row { Value = double.NaN, Baseline = 101d, Adjustment = 1d }), Is.NaN);
        Assert.That(evaluate(new Row { Value = double.PositiveInfinity, Baseline = 101d, Adjustment = 1d }),
            Is.EqualTo(double.PositiveInfinity));
    }

    [Test]
    public void ArithmeticPreservesNonNumericOperatorsAndLiteralTypes()
    {
        Assert.That(Compile("Value+Baseline")(new Row { Value = "value ", Baseline = 2d }), Is.EqualTo("value 2"));
        var date = new DateTime(2026, 1, 2);
        Assert.That(Compile("Value+Baseline")(new Row { Value = date, Baseline = TimeSpan.FromDays(1) }),
            Is.EqualTo(new DateTime(2026, 1, 3)));
        Assert.That(Compile("Value-Baseline")(new Row { Value = date, Baseline = new DateTime(2026, 1, 1) }),
            Is.EqualTo(TimeSpan.FromDays(1)));
        Assert.That(Compile("Value+Baseline")(new Row { Value = TimeSpan.FromHours(1), Baseline = TimeSpan.FromHours(2) }),
            Is.EqualTo(TimeSpan.FromHours(3)));
        Assert.That(Compile("Value+(5/2)")(new Row { Value = 0d }), Is.EqualTo(2.5d));
        Assert.That(Compile("Value+'2'")(new Row { Value = "value " }), Is.EqualTo("value 2"));
        Assert.That(Compile("Value+9007199254740993L")(new Row { Value = "id " }),
            Is.EqualTo("id 9007199254740993"));
        Assert.That(Compile("Value+Baseline")(new Row { Value = null, Baseline = 2d }), Is.Null);
        Assert.That(Compile("Value*2")(new Row { Value = null }), Is.Null);
        Assert.That(Compile("2/Value")(new Row { Value = null }), Is.Null);
        Assert.Throws<RuntimeBinderException>(() => Compile("Value-Baseline")(new Row { Value = "bad", Baseline = 1d }));
        Assert.That(Compile("Value+2")(new Row { Value = new CustomOperand() }), Is.EqualTo("int 2"));
        Assert.That(Compile("Value+2147483648L")(new Row { Value = new CustomOperand() }), Is.EqualTo("long 2147483648"));
        Assert.That(Compile("Value+'2'")(new Row { Value = 1d }), Is.EqualTo("12"));
    }

    [Test]
    public void SameCompiledExpressionHandlesChangingRuntimeTypes()
    {
        var evaluate = Compile("Value+Baseline");
        Assert.That(evaluate(new Row { Value = 2d, Baseline = 3d }), Is.EqualTo(5d).And.TypeOf<double>());
        Assert.That(evaluate(new Row { Value = "value ", Baseline = 3d }), Is.EqualTo("value 3"));
        Assert.That(evaluate(new Row { Value = null, Baseline = 3d }), Is.Null);
        Assert.That(evaluate(new Row { Value = TimeSpan.FromHours(1), Baseline = TimeSpan.FromHours(2) }),
            Is.EqualTo(TimeSpan.FromHours(3)));
        Assert.That(evaluate(new Row { Value = 4m, Baseline = 5L }), Is.EqualTo(9d).And.TypeOf<double>());
    }

    [Test]
    public void PreservesOriginalPropertyReadCountAndOrder()
    {
        var row = new ObservedRow();
        Assert.That(Compile(RelativeDeviation)(row), Is.EqualTo(200d));
        Assert.That(row.Reads, Is.EqualTo(new[]
        {
            "Value", "Value", "Baseline", "Baseline", "Adjustment", "Adjustment",
            "Baseline", "Baseline", "Adjustment", "Adjustment"
        }));
    }

    [Test]
    public void UsesTheSecondPropertyReadAsTheOperand()
    {
        Assert.That(Compile("Value+2")(new ChangingRow()), Is.EqualTo(7d));
    }

    [Test]
    public void ArithmeticStillComposesWithOtherLanguageFeatures()
    {
        var row = new Row { Value = 102d, Baseline = 101d, Adjustment = 1d };
        Assert.That(Compile("(Value-Baseline) > 0 ? Value-Baseline : 0")(row), Is.EqualTo(1d));
        Assert.That(Compile("match | Value-Baseline == 1 = Value+Baseline | otherwise 0")(row), Is.EqualTo(203d));
        Assert.That(Compile("let adjusted = Baseline-Adjustment in (Value-adjusted)/adjusted*10000")(row), Is.EqualTo(200d));
        Assert.That(Compile("abs(Baseline-Value)")(row), Is.EqualTo(1d));
        Assert.That(Compile("(Value-Baseline)^2")(row), Is.EqualTo(1d));
    }

    private static Func<object, object?> Compile(string text)
    {
        var parsed = CSharpExpression.ParseCode(text);
        Assert.That(parsed.Success, Is.True);
        var code = parsed.Expression.Evaluate(new object());
        var (type, errors) = CodeGenCSharpClass.Generate("ArithmeticTest", code);
        if (errors != null)
        {
            TestContext.WriteLine(code);
        }
        Assert.That(errors, Is.Null, () => string.Join(Environment.NewLine, errors!.Diagnostics));
        return type!.GetMethod("Evaluate")!.CreateDelegate<Func<object, object?>>();
    }
}
