using System;
using System.Collections.Generic;
using NUnit.Framework;
using VCEL.CSharp;

namespace VCEL.Test;

public class CSharpBooleanValueCompatibilityTests
{
    public sealed class Row
    {
        public object Flag { get; set; } = false;
        public object Value { get; set; } = 1d;
        public object Other { get; set; } = 2d;
        public int Reads { get; private set; }
        public object Fail { get { Reads++; throw new InvalidOperationException("unselected"); } }
    }

    [TestCase("match | false=(Value<Other)", null)]
    [TestCase("match | false=(Value<=Other)", null)]
    [TestCase("match | false=(Value>Other)", null)]
    [TestCase("match | false=(Value>=Other)", null)]
    [TestCase("match | false=(!(Value<Other))", null)]
    [TestCase("match | false=(Value in {'A','B'})", null)]
    [TestCase("match | false=(Value in {1,2})", false)]
    [TestCase("match | false=(1<2)", false)]
    [TestCase("match | false=(!(1<2))", false)]
    [TestCase("match | false=(int(1)<2)", false)]
    [TestCase("let value=1 in match | false=(value<2)", false)]
    [TestCase("let value=Value in match | false=(value<2)", null)]
    [TestCase("match | false=(1<2) | false=(Value<Other)", null)]
    [TestCase("match | false=(Value<Other) | false=(1<2)", false)]
    [TestCase("match | true=(Value<Other)", true)]
    [TestCase("match | true=(Value>Other)", false)]
    [TestCase("match | false=(Value<Other) | otherwise false", false)]
    [TestCase("match | false=(Value in {'A','B'}) | otherwise 'missing'", "missing")]
    [TestCase("let x=Flag ? (Value<Other) : null in x", null)]
    [TestCase("let x=Flag ? int(Value<Other) : 'other' in x", "other")]
    [TestCase("let x=int(Value<Other),y=Flag ? x : 'other' in y", "other")]
    [TestCase("let x=Flag ? int(bool(Value<Other)) : 'other' in x", "other")]
    [TestCase("match | false=(int(Value<Other)==1)", null)]
    public void ValuesPreservePrePr106ResultsAndTypes(string text, object? expected)
    {
        var parsed = CSharpExpression.ParseDelegateWithMembers(text);
        Assert.That(parsed.Success, Is.True, text);
        var result = parsed.Expression.Evaluate(new Row());
        Assert.That(result, Is.EqualTo(expected), text);
        Assert.That(result?.GetType(), Is.EqualTo(expected?.GetType()), text);
    }

    [TestCase("let x=Flag ? (Value<Other) : null in x", true)]
    [TestCase("let x=Flag ? int(Value<Other) : 'other' in x", 1)]
    [TestCase("let x=Flag ? int(bool(Value<Other)) : 'other' in x", 1)]
    public void SelectedComparisonValuesPreservePrePr106Types(string text, object expected)
    {
        var parsed = CSharpExpression.ParseDelegateWithMembers(text);
        Assert.That(parsed.Success, Is.True, text);
        var result = parsed.Expression.Evaluate(new Row { Flag = true });
        Assert.That(result, Is.EqualTo(expected).And.TypeOf(expected.GetType()));
    }

    [TestCase("let x=Value<Other in [x,x]", true, true)]
    [TestCase("let x=!(Value<Other) in [x,x]", true, false)]
    [TestCase("let x=int(Value<Other) in [x,x]", true, 1)]
    [TestCase("let x=1<2 in [x,x]", false, true)]
    [TestCase("let x=int(1<2) in [x,x]", false, 1)]
    public void ComparisonBindingsPreservePrePr106BoxingIdentity(string text, bool sharedBox, object expected)
    {
        var parsed = CSharpExpression.ParseDelegateWithMembers(text);
        Assert.That(parsed.Success, Is.True, text);
        var result = parsed.Expression.Evaluate(new Row());
        Assert.That(result, Is.TypeOf<List<object>>());
        var items = (List<object>)result!;
        Assert.That(items, Has.Count.EqualTo(2));
        Assert.That(items[0], Is.EqualTo(expected).And.TypeOf(expected.GetType()));
        Assert.That(items[1], Is.EqualTo(expected).And.TypeOf(expected.GetType()));
        Assert.That(ReferenceEquals(items[0], items[1]), Is.EqualTo(sharedBox));
    }

    [TestCase("let x=Flag ? (Value in {'A','B'}) : null in x")]
    [TestCase("let x=Value in {'A','B'} in [x,x]")]
    [TestCase("match | false=(!(Value in {'A','B'}))")]
    [TestCase("let x=Flag ? (1<2) : null in x")]
    [TestCase("let x=Flag ? int(1<2) : 'other' in x")]
    public void PreExistingCompilationFailuresRemainFailures(string text)
    {
        var parsed = CSharpExpression.ParseDelegateWithMembers(text);
        Assert.That(parsed.Success, Is.False, text);
        Assert.That(parsed.ParseErrors, Is.Not.Empty);
    }

    [TestCase("match | false=(Fail<Other)")]
    [TestCase("match | false=(Fail in {'A','B'})")]
    public void UnselectedBooleanValuesPreserveNullWithoutReadingOperands(string text)
    {
        var parsed = CSharpExpression.ParseDelegateWithMembers(text);
        Assert.That(parsed.Success, Is.True, text);
        var row = new Row();
        Assert.That(parsed.Expression.Evaluate(row), Is.Null);
        Assert.That(row.Reads, Is.Zero);
    }
}
