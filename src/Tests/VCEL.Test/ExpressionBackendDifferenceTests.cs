using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VCEL.Test;

public class ExpressionBackendDifferenceTests
{
    public enum Backend { CSharp, Expression, Maybe }

    public sealed class Row
    {
        public object? Category { get; init; } = "A";
        public object? Input { get; init; } = 12d;
        public object? Reference { get; init; } = 10d;
        public object? Amount { get; init; } = 20d;
        public object? Divisor { get; init; } = 2d;
        public object? Value { get; init; } = 2d;
        public object? Flag { get; init; } = false;
    }

    private sealed record Expected(object? Value, string? CompilationDiagnostic = null, Type? ExceptionType = null);
    private sealed record BackendExpectations(Expected CSharp, Expected Expression, Expected Maybe);

    private static IEnumerable<TestCaseData> RelativeChangeCases()
    {
        const string text = "Category in {'A','B'} ? (Input-Reference)/Reference*10000 : null";
        foreach (var data in Cases("RelativeChange", "Division", "NonZeroReferenceAgrees", text,
            new Row(),
            csharp: new(2000d),
            expression: new(2000d),
            maybe: new(2000d)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "ZeroReferenceReturnsInfinityOnlyOutsideMaybe", text,
            new Row { Reference = 0d },
            csharp: new(double.PositiveInfinity),
            expression: new(double.PositiveInfinity),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "ZeroOverZeroReturnsNaNOnlyOutsideMaybe", text,
            new Row { Input = 0d, Reference = 0d },
            csharp: new(double.NaN),
            expression: new(double.NaN),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "NegativeInfinityDoesNotTriggerRuntimeFallback", text,
            new Row { Input = -12d, Reference = 0d },
            csharp: new(double.NegativeInfinity),
            expression: new(double.NegativeInfinity),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "NullPropagation", "NullReferenceAgrees", text,
            new Row { Reference = null },
            csharp: new(null),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "ShortCircuit", "UnselectedZeroDivisionAgrees", text,
            new Row { Category = "OTHER", Reference = 0d },
            csharp: new(null),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "StringReferenceThrowsOnlyInCSharp", text,
            new Row { Reference = "0" },
            csharp: new(null, ExceptionType: typeof(RuntimeBinderException)),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "DecimalZeroThrowsInInterpreterRatherThanProducingNull",
            "Input/Reference", new Row { Input = 12m, Reference = 0m },
            csharp: new(double.PositiveInfinity),
            expression: new(null, ExceptionType: typeof(DivideByZeroException)),
            maybe: new(null, ExceptionType: typeof(DivideByZeroException))))
            yield return data;
        foreach (var data in Cases("RelativeChange", "Division", "LongZeroIsNotRejectedByMaybe",
            "Input/Reference", new Row { Input = 12L, Reference = 0L },
            csharp: new(double.PositiveInfinity),
            expression: new(double.PositiveInfinity),
            maybe: new(double.PositiveInfinity)))
            yield return data;
    }

    [TestCaseSource(nameof(RelativeChangeCases))]
    public void RelativeChangeCalculation(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> CappedRatioCases()
    {
        const string text = "Category in {'A','B'} ? (Amount/Divisor<500 ? Amount/Divisor : 500) : 0";
        foreach (var data in Cases("CappedRatio", "Comparison", "BelowCapAgrees", text,
            new Row(),
            csharp: new(10d),
            expression: new(10d),
            maybe: new(10d)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "Comparison", "AboveCapAgrees", text,
            new Row { Amount = 1200d },
            csharp: new(500),
            expression: new(500),
            maybe: new(500)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "Division", "ZeroDivisorProducesCapRatherThanNull", text,
            new Row { Divisor = 0d },
            csharp: new(500),
            expression: new(500),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "NullPropagation", "NullAmountProducesCapOnlyInCSharp", text,
            new Row { Amount = null },
            csharp: new(500),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "NullPropagation", "NullDivisorProducesCapOnlyInCSharp", text,
            new Row { Divisor = null },
            csharp: new(500),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "Division", "NaNBranchSelection", text,
            new Row { Amount = 0d, Divisor = 0d },
            csharp: new(double.NaN),
            expression: new(double.NaN),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("CappedRatio", "ShortCircuit", "UnselectedNullCalculationAgrees", text,
            new Row { Category = "OTHER", Amount = null, Divisor = 0d },
            csharp: new(0),
            expression: new(0),
            maybe: new(0)))
            yield return data;
    }

    [TestCaseSource(nameof(CappedRatioCases))]
    public void CappedRatioCalculation(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> NumericResultCases()
    {
        const string select = "Category in {'A','B'} ? Value : null";
        foreach (var data in Cases("NumericResults", "PropertyPromotion", "DoubleSelectionAgrees", select,
            new Row(),
            csharp: new(2d),
            expression: new(2d),
            maybe: new(2d)))
            yield return data;
        foreach (var data in Cases("NumericResults", "PropertyPromotion", "IntegerSelectionChangesType", select,
            new Row { Value = 2 },
            csharp: new(2d),
            expression: new(2),
            maybe: new(2)))
            yield return data;
        foreach (var data in Cases("NumericResults", "PropertyPromotion", "DecimalSelectionLosesPrecision", select,
            new Row { Value = 1.234567890123456789m },
            csharp: new(1.2345678901234567d),
            expression: new(1.234567890123456789m),
            maybe: new(1.234567890123456789m)))
            yield return data;
        foreach (var data in Cases("NumericResults", "PropertyPromotion", "LargeLongSelectionChangesValue", select,
            new Row { Value = 9007199254740993L },
            csharp: new(9007199254740992d),
            expression: new(9007199254740993L),
            maybe: new(9007199254740993L)))
            yield return data;
        foreach (var data in Cases("NumericResults", "PropertyPromotion", "NullSelectionAgrees", select,
            new Row { Value = null },
            csharp: new(null),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("NumericResults", "Arithmetic", "IntegerMultiplicationChangesType",
            "Amount*Input", new Row { Amount = 2, Input = 3 },
            csharp: new(6d),
            expression: new(6),
            maybe: new(6)))
            yield return data;
        foreach (var data in Cases("NumericResults", "Arithmetic", "DecimalMultiplicationChangesPrecision",
            "Amount*Input", new Row { Amount = 1.234567890123456789m, Input = 1.234567890123456789m },
            csharp: new(1.5241578753238834d),
            expression: new(1.5241578753238836750190519988m),
            maybe: new(1.5241578753238836750190519988m)))
            yield return data;
        foreach (var data in Cases("NumericResults", "Ternary", "UnmatchedMixedLiteralsPromoteInteger",
            "Category in {'A','B'} ? 1.5 : 1", new Row { Category = "OTHER" },
            csharp: new(1d),
            expression: new(1),
            maybe: new(1)))
            yield return data;
        foreach (var data in Cases("NumericResults", "Ternary", "MatchedMixedLiteralsAgree",
            "Category in {'A','B'} ? 1.5 : 1", new Row(),
            csharp: new(1.5d),
            expression: new(1.5d),
            maybe: new(1.5d)))
            yield return data;
        foreach (var data in Cases("NumericResults", "Division", "DecimalDivisionChangesType",
            "Amount/Divisor", new Row { Amount = 1m, Divisor = 2m },
            csharp: new(0.5d),
            expression: new(0.5m),
            maybe: new(0.5m)))
            yield return data;
    }

    [TestCaseSource(nameof(NumericResultCases))]
    public void NumericResults(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> MembershipCases()
    {
        const string text = "Category in {'A','B'} ? 1 : 0";
        foreach (var data in Cases("Classification", "StringMembership", "MatchedStringAgrees", text,
            new Row(),
            csharp: new(1),
            expression: new(1),
            maybe: new(1)))
            yield return data;
        foreach (var data in Cases("Classification", "StringMembership", "UnmatchedStringAgrees", text,
            new Row { Category = "OTHER" },
            csharp: new(0),
            expression: new(0),
            maybe: new(0)))
            yield return data;
        foreach (var data in Cases("Classification", "StringMembership", "NullOperandAgrees", text,
            new Row { Category = null },
            csharp: new(0),
            expression: new(0),
            maybe: new(0)))
            yield return data;
        foreach (var data in Cases("Classification", "StringMembership", "CollectionOperandIsNotAnyMembership", text,
            new Row { Category = new List<object> { "A" } },
            csharp: new(0),
            expression: new(0),
            maybe: new(0)))
            yield return data;
        foreach (var data in Cases("Classification", "NumericMembership", "IntegerMembershipAgrees",
            "Value in {1,2} ? 1 : 0", new Row { Value = 2 },
            csharp: new(1),
            expression: new(1),
            maybe: new(1)))
            yield return data;
        foreach (var data in Cases("Classification", "NumericMembership", "DoubleMembership",
            "Value in {1,2} ? 1 : 0", new Row { Value = 2d },
            csharp: new(1),
            expression: new(0),
            maybe: new(0)))
            yield return data;
    }

    [TestCaseSource(nameof(MembershipCases))]
    public void Classification(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> ComparisonCases()
    {
        foreach (var data in Cases("Branching", "Comparison", "ComparableOperandsAgree",
            "Value<10", new Row(),
            csharp: new(true),
            expression: new(true),
            maybe: new(true)))
            yield return data;
        foreach (var data in Cases("Branching", "Comparison", "NullComparisonReturnsFalseOnlyInCSharp",
            "Value<10", new Row { Value = null },
            csharp: new(false),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "Comparison", "IncompatibleComparisonReturnsFalseOnlyInCSharp",
            "Value<10", new Row { Value = "2" },
            csharp: new(false),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "Ternary", "NullConditionSelectsElseOnlyInCSharp",
            "Value<10 ? 1 : 2", new Row { Value = null },
            csharp: new(2),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "Guard", "NullGuardSelectsOtherwiseOnlyInCSharp",
            "match | Value<10=1 | otherwise 2", new Row { Value = null },
            csharp: new(2),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "Guard", "UnmatchedStaticBooleanResultHasDifferentDefault",
            "match | false=(1<2)", new Row(),
            csharp: new(false),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "Guard", "UnmatchedDynamicBooleanResultAgrees",
            "match | false=(Value<10)", new Row(),
            csharp: new(null),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Branching", "BooleanComposition", "MatchAndGuardCannotCompile",
            "match | (Value==2 && Amount==20)=1 | otherwise 2", new Row(),
            csharp: new(null, "Cannot implicitly convert type 'bool?' to 'bool'"),
            expression: new(1),
            maybe: new(1)))
            yield return data;
        foreach (var data in Cases("Branching", "BooleanComposition", "UnmatchedMatchAndGuardStillCannotCompile",
            "match | (Value==2 && Amount==20)=1 | otherwise 2", new Row { Value = 3d },
            csharp: new(null, "Cannot implicitly convert type 'bool?' to 'bool'"),
            expression: new(2),
            maybe: new(2)))
            yield return data;
    }

    [TestCaseSource(nameof(ComparisonCases))]
    public void ComparisonsAndBranching(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> TimeBucketCases()
    {
        const string text = "let x=round((Input-Reference).TotalSeconds/60,0) in x";
        foreach (var data in Cases("TimeBucket", "MemberAccess", "DateSubtractionAndRoundingAgree", text,
            new Row
            {
                Input = new DateTime(2026, 1, 1, 10, 0, 0),
                Reference = new DateTime(2026, 1, 1, 9, 30, 0)
            },
            csharp: new(30d),
            expression: new(30d),
            maybe: new(30d)))
            yield return data;
        foreach (var data in Cases("TimeBucket", "MemberAccess", "NullDateThrowsOnlyInCSharp", text,
            new Row { Input = null, Reference = new DateTime(2026, 1, 1, 9, 30, 0) },
            csharp: new(null, ExceptionType: typeof(RuntimeBinderException)),
            expression: new(null),
            maybe: new(null)))
            yield return data;
    }

    [TestCaseSource(nameof(TimeBucketCases))]
    public void TimeBuckets(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> BindingCases()
    {
        foreach (var data in Cases("Bindings", "Let", "StringSetDependencyCannotCompile",
            "let x=Category in {'A','B'} in x", new Row(),
            csharp: new(null, "str_set_"),
            expression: new(true),
            maybe: new(true)))
            yield return data;
        foreach (var data in Cases("Bindings", "Negation", "NumericSetDependencyCannotCompile",
            "!(Value in {1,2})", new Row { Value = 2 },
            csharp: new(null, "numeric_set_"),
            expression: new(false),
            maybe: new(false)))
            yield return data;
        foreach (var data in Cases("Bindings", "Ternary", "StaticBoolNullBindingCannotCompile",
            "let x=Flag ? (1<2) : null in x", new Row(),
            csharp: new(null, "'bool' and '<null>'"),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Bindings", "Ternary", "SelectedStaticBoolNullBindingCannotCompile",
            "let x=Flag ? (1<2) : null in x", new Row { Flag = true },
            csharp: new(null, "'bool' and '<null>'"),
            expression: new(true),
            maybe: new(true)))
            yield return data;
        foreach (var data in Cases("Bindings", "Let", "NullInitializerCannotCompile",
            "let x=null in x", new Row(),
            csharp: new(null, "Cannot assign <null>"),
            expression: new(null),
            maybe: new(null)))
            yield return data;
        foreach (var data in Cases("Bindings", "Let", "PropertyBindingAgrees",
            "let x=Value in x", new Row(),
            csharp: new(2d),
            expression: new(2d),
            maybe: new(2d)))
            yield return data;
    }

    [TestCaseSource(nameof(BindingCases))]
    public void BindingsAndCompilation(string text, Row row, object expected)
        => AssertBackends(text, row, (BackendExpectations)expected);

    private static IEnumerable<TestCaseData> Cases(string useCase, string feature, string name, string text,
        Row row, Expected csharp, Expected expression, Expected maybe)
    {
        yield return new TestCaseData(text, row, new BackendExpectations(csharp, expression, maybe))
            .SetName($"{useCase}_{feature}_{name}")
            .SetCategory($"UseCase.{useCase}")
            .SetCategory($"Feature.{feature}");
    }

    private static void AssertBackends(string text, Row row, BackendExpectations expected)
    {
        Assert.Multiple(() =>
        {
            AssertOutcome(Backend.CSharp, text, row, expected.CSharp);
            AssertOutcome(Backend.Expression, text, row, expected.Expression);
            AssertOutcome(Backend.Maybe, text, row, expected.Maybe);
        });
    }

    private static void AssertOutcome(Backend backend, string text, Row row, Expected expected)
    {
        switch (backend)
        {
            case Backend.CSharp:
                var syntax = new ExpressionParser<string>(
                    new AdvanceToCSharpCodeFactory(ConcatStringMonad.Instance)).Parse(text);
                Assert.That(syntax.Success, Is.True, text);
                var compiled = CSharpExpression.ParseDelegateWithMembers(text);
                if (expected.CompilationDiagnostic != null)
                {
                    Assert.That(compiled.Success, Is.False, text);
                    Assert.That(compiled.ParseErrors.Any(error =>
                        error.Message.Contains(expected.CompilationDiagnostic, StringComparison.Ordinal)), Is.True,
                        string.Join(Environment.NewLine, compiled.ParseErrors.Select(error => error.Message)));
                    return;
                }
                Assert.That(compiled.Success, Is.True,
                    string.Join(Environment.NewLine, compiled.ParseErrors.Select(error => error.Message)));
                AssertEvaluation(() => compiled.Expression.Evaluate(row), expected, backend);
                break;
            case Backend.Expression:
                var expression = new ExpressionParser<object?>(
                    new ExpressionFactory<object?>(ExprMonad.Instance)).Parse(text);
                Assert.That(expression.Success, Is.True, text);
                AssertEvaluation(() => expression.Expression.Evaluate(row), expected, backend);
                break;
            case Backend.Maybe:
                var maybe = new ExpressionParser<Maybe<object>>(
                    new MaybeExpressionFactory(MaybeMonad.Instance)).Parse(text);
                Assert.That(maybe.Success, Is.True, text);
                AssertEvaluation(() =>
                {
                    var value = maybe.Expression.Evaluate(row);
                    return value.HasValue ? value.Value : null;
                }, expected, backend);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(backend), backend, null);
        }
    }

    private static void AssertEvaluation(Func<object?> evaluate, Expected expected, Backend backend)
    {
        if (expected.ExceptionType != null)
        {
            Assert.That(evaluate, Throws.TypeOf(expected.ExceptionType), $"{backend}: runtime exception");
            return;
        }

        var value = evaluate();
        Assert.That(value, Is.EqualTo(expected.Value), $"{backend}: result");
        Assert.That(value?.GetType(), Is.EqualTo(expected.Value?.GetType()), $"{backend}: result type");
    }
}
