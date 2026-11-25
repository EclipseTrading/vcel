using System;
using System.Collections.Generic;
using NUnit.Framework;
using VCEL.CSharp;

namespace VCEL.Test;

public class CSharpEqualityValueCompatibilityTests
{
    public sealed class Row
    {
        public object Flag { get; set; } = false;
        public object Code { get; set; } = "A";
    }

    [TestCase("match | false=(1==1)", false)]
    [TestCase("match | false=(1!=1)", false)]
    [TestCase("match | false=('A'=='A')", false)]
    [TestCase("match | false=('A'!='B')", false)]
    [TestCase("match | false=(null==null)", false)]
    [TestCase("match | false=(!(1==1))", false)]
    [TestCase("match | false=((1+2)==3)", false)]
    [TestCase("match | false=(int(1)==1)", false)]
    [TestCase("let value=1 in match | false=(value==1)", false)]
    [TestCase("let value='A' in match | false=(value=='A')", false)]
    [TestCase("let equal=1==1 in match | false=equal", false)]
    [TestCase("match | false=(1==1) | false=(2!=3)", false)]
    [TestCase("match | true=(1==1)", true)]
    [TestCase("match | true=(1!=1)", false)]
    [TestCase("match | false=(1==1) | otherwise true", true)]
    [TestCase("match | Flag=(Code=='A')", null)]
    [TestCase("match | Flag=(Code!='A')", null)]
    [TestCase("let value=Code in match | false=(value=='A')", null)]
    [TestCase("match | false=(int(Code=='A')==1)", null)]
    [TestCase("match | false=(1==1) | false=(Code=='A')", null)]
    [TestCase("match | false=(Code=='A') | false=(1==1)", false)]
    [TestCase("match | false=((Code=='A')==true)", null)]
    [TestCase("match | false=((1==1)==true)", false)]
    [TestCase("match | false=((Code=='A') && true)", null)]
    [TestCase("let x=Flag ? int(Code=='A') : 'other' in x", "other")]
    [TestCase("let x=(match | Flag=(Code=='A') | otherwise null) in x", null)]
    public void CompiledEqualityValuesPreservePrePr102Results(string text, object? expected)
    {
        var parsed = CSharpExpression.ParseDelegate(text);
        Assert.That(parsed.Success, Is.True, text);
        var result = parsed.Expression.Evaluate(new Row());
        Assert.That(result, Is.EqualTo(expected), text);
        Assert.That(result?.GetType(), Is.EqualTo(expected?.GetType()), text);
    }

    [TestCase("let x=1==1 in [x,x]", false, true)]
    [TestCase("let x=1!=1 in [x,x]", false, false)]
    [TestCase("let x=!(1==1) in [x,x]", false, false)]
    [TestCase("let value=1,x=value==1 in [x,x]", false, true)]
    [TestCase("let x=bool(1==1) in [x,x]", false, true)]
    [TestCase("let x=int(1==1) in [x,x]", false, 1)]
    [TestCase("let x=int(bool(1==1)) in [x,x]", false, 1)]
    [TestCase("let x=Code=='A' in [x,x]", true, true)]
    [TestCase("let x=Code!='A' in [x,x]", true, false)]
    [TestCase("let x=int(Code=='A') in [x,x]", true, 1)]
    [TestCase("let x=(Code=='A')==true in [x,x]", true, true)]
    [TestCase("let x=(Code=='A') && true in [x,x]", false, true)]
    [TestCase("let x=(Code=='B') || true in [x,x]", false, true)]
    public void LetEqualityValuesPreservePrePr102BoxingIdentity(
        string text, bool sharedBox, object expected)
    {
        var parsed = CSharpExpression.ParseDelegate(text);
        Assert.That(parsed.Success, Is.True, text);
        var result = parsed.Expression.Evaluate(new Row());
        Assert.That(result, Is.TypeOf<List<object>>());
        var items = (List<object>)result!;
        Assert.That(items, Has.Count.EqualTo(2));
        Assert.That(items[0], Is.EqualTo(expected).And.TypeOf(expected.GetType()));
        Assert.That(items[1], Is.EqualTo(expected).And.TypeOf(expected.GetType()));
        Assert.That(ReferenceEquals(items[0], items[1]), Is.EqualTo(sharedBox), text);
    }
}
