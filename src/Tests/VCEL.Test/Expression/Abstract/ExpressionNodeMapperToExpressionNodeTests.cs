using System.Collections.Generic;
using System.Collections.Immutable;
using NUnit.Framework;
using VCEL.Core.Expression.Abstract;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Expression;
using VCEL.Monad;
using List = VCEL.Core.Expression.Abstract.List;

namespace VCEL.Test.Expression.Abstract;

public class ExpressionNodeMapperToExpressionNodeTests
{
    private readonly NullExpr<object?> nullExpr = new(new ExprMonad());

    [Test]
    public void ShouldMapToExpressionTernary()
    {
        var ternaryExpr = ToExpressionNode(new Ternary<object?>(ExprMonad.Instance, nullExpr, nullExpr, nullExpr));
        Assert.That(ternaryExpr, Is.TypeOf<Ternary>());
        Assert.That(((Ternary)ternaryExpr).Condition, Is.TypeOf<Null>());
        Assert.That(((Ternary)ternaryExpr).TrueExpression, Is.TypeOf<Null>());
        Assert.That(((Ternary)ternaryExpr).FalseExpression, Is.TypeOf<Null>());
    }

    [Test]
    public void ShouldMapToExpressionTernaryWithValues()
    {
        var ternaryExpr = ToExpressionNode(new Ternary<object?>(ExprMonad.Instance, new ValueExpr<object?, object?>(ExprMonad.Instance, true),
            new ValueExpr<object?, object?>(ExprMonad.Instance, 11), new ValueExpr<object?, object?>(ExprMonad.Instance, 12)));
        Assert.That(ternaryExpr, Is.TypeOf<Ternary>());
        Assert.That(((Ternary)ternaryExpr).Condition, Is.TypeOf<Value>());
        Assert.That(((Value)((Ternary)ternaryExpr).Condition).ValueProperty, Is.EqualTo(true));
        Assert.That(((Ternary)ternaryExpr).TrueExpression, Is.TypeOf<Value>());
        Assert.That(((Value)((Ternary)ternaryExpr).TrueExpression).ValueProperty, Is.EqualTo(11));
        Assert.That(((Ternary)ternaryExpr).FalseExpression, Is.TypeOf<Value>());
        Assert.That(((Value)((Ternary)ternaryExpr).FalseExpression).ValueProperty, Is.EqualTo(12));
    }

    [Test]
    public void ShouldMapToExpressionLet()
    {
        var letExpr = ToExpressionNode(new LetExpr<object?>(ExprMonad.Instance, ImmutableArray<(string, IExpression<object?>)>.Empty,
            nullExpr));
        Assert.That(letExpr, Is.TypeOf<Let>());
    }

    [Test]
    public void ShouldMapToExpressionGuard()
    {
        var guardExpr = ToExpressionNode(new GuardExpr<object?>(ExprMonad.Instance,
            ImmutableArray<(IExpression<object?>, IExpression<object?>)>.Empty, nullExpr));
        Assert.That(guardExpr, Is.TypeOf<Guard>());
    }

    [Test]
    public void ShouldMapToExpressionLessThan()
    {
        var lessThanExpr = ToExpressionNode(new LessThan<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(lessThanExpr, Is.TypeOf<LessThan>());
    }

    [Test]
    public void ShouldMapToExpressionGreaterThan()
    {
        var greaterThanExpr = ToExpressionNode(new GreaterThan<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(greaterThanExpr, Is.TypeOf<GreaterThan>());
    }

    [Test]
    public void ShouldMapToExpressionLessOrEqual()
    {
        var lessOrEqualExpr = ToExpressionNode(new LessOrEqual<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(lessOrEqualExpr, Is.TypeOf<LessOrEqual>());
    }

    [Test]
    public void ShouldMapToExpressionGreaterOrEqual()
    {
        var greaterOrEqualExpr = ToExpressionNode(new GreaterOrEqual<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(greaterOrEqualExpr, Is.TypeOf<GreaterOrEqual>());
    }

    [Test]
    public void ShouldMapToExpressionBetween()
    {
        var betweenExpr = ToExpressionNode(new BetweenExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr, nullExpr));
        Assert.That(betweenExpr, Is.TypeOf<Between>());
    }

    [Test]
    public void ShouldMapToExpressionInSet()
    {
        var inExpr = ToExpressionNode(new InSetExpr<object?>(ExprMonad.Instance, nullExpr, new HashSet<object>()));
        Assert.That(inExpr, Is.TypeOf<InSet>());
    }

    [Test]
    public void ShouldMapToExpressionIn() 
    {
        var inExpr = ToExpressionNode(new InExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(inExpr, Is.TypeOf<In>());
    }

    [Test]
    public void ShouldMapToExpressionSpread() 
    {
        var spreadExpr = ToExpressionNode(new SpreadExpr<object?>(ExprMonad.Instance, nullExpr));
        Assert.That(spreadExpr, Is.TypeOf<Spread>());
    }

    [Test]
    public void ShouldMapToExpressionMatches()
    {
        var matchesExpr = ToExpressionNode(new MatchesExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(matchesExpr, Is.TypeOf<Matches>());
    }

    [Test]
    public void ShouldMapToExpressionAnd()
    {
        var andExpr = ToExpressionNode(new AndExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(andExpr, Is.TypeOf<And>());
    }

    [Test]
    public void ShouldMapToExpressionOr()
    {
        var orExpr = ToExpressionNode(new OrExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(orExpr, Is.TypeOf<Or>());
    }

    [Test]
    public void ShouldMapToExpressionNot()
    {
        var notExpr = ToExpressionNode(new NotExpr<object?>(ExprMonad.Instance, nullExpr));
        Assert.That(notExpr, Is.TypeOf<Not>());
    }

    [Test]
    public void ShouldMapToExpressionValue()
    {
        var valueExpr = ToExpressionNode(new ValueExpr<object?, object?>(ExprMonad.Instance, 42));
        Assert.That(valueExpr, Is.TypeOf<Value>());
    }

    [Test]
    public void ShouldMapToExpressionList()
    {
        var listExpr = ToExpressionNode(new ListExpr<object?>(ExprMonad.Instance, ImmutableArray<IExpression<object?>>.Empty));
        Assert.That(listExpr, Is.TypeOf<List>());
    }

    [Test]
    public void ShouldMapToExpressionAdd()
    {
        var addExpr = ToExpressionNode(new AddExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(addExpr, Is.TypeOf<Add>());
    }

    [Test]
    public void ShouldMapToExpressionMultiply()
    {
        var multiplyExpr = ToExpressionNode(new MultExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(multiplyExpr, Is.TypeOf<Multiply>());
    }

    [Test]
    public void ShouldMapToExpressionSubtract()
    {
        var subtractExpr = ToExpressionNode(new SubtractExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(subtractExpr, Is.TypeOf<Subtract>());
    }

    [Test]
    public void ShouldMapToExpressionDivide()
    {
        var divideExpr = ToExpressionNode(new DivideExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(divideExpr, Is.TypeOf<Divide>());
    }

    [Test]
    public void ShouldMapToExpressionPow()
    {
        var powExpr = ToExpressionNode(new PowExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(powExpr, Is.TypeOf<Pow>());
    }

    [Test]
    public void ShouldMapToExpressionParen()
    {
        var parenExpr = ToExpressionNode(new ParenExpr<object?>(ExprMonad.Instance, nullExpr));
        Assert.That(parenExpr, Is.TypeOf<Paren>());
    }

    [Test]
    public void ShouldMapToExpressionProperty()
    {
        var propertyExpr = ToExpressionNode(new Property<object?>(ExprMonad.Instance, "Test"));
        Assert.That(propertyExpr, Is.TypeOf<Property>());
    }

    [Test]
    public void ShouldMapToExpressionFunction()
    {
        var functionExpr = ToExpressionNode(new FunctionExpr<object?>(ExprMonad.Instance, "abs",
            ImmutableArray<IExpression<object?>>.Empty, new Function<object?>((_, _) => null)));
        Assert.That(functionExpr, Is.TypeOf<Function>());
    }

    [Test]
    public void ShouldMapToExpressionUnaryMinus()
    {
        var unaryMinusExpr = ToExpressionNode(new UnaryMinusExpr<object?>(ExprMonad.Instance, nullExpr));
        Assert.That(unaryMinusExpr, Is.TypeOf<UnaryMinus>());
    }

    [Test]
    public void ShouldMapToExpressionNull()
    {
        var nullExpr = ToExpressionNode(new NullExpr<object?>(ExprMonad.Instance));
        Assert.That(nullExpr, Is.TypeOf<Null>());
    }

    [Test]
    public void ShouldMapToExpressionEq()
    {
        var eqExpr = ToExpressionNode(new EqExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(eqExpr, Is.TypeOf<Eq>());
    }

    [Test]
    public void ShouldMapToExpressionNotEq()
    {
        var notEqExpr = ToExpressionNode(new NotEqExpr<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(notEqExpr, Is.TypeOf<NotEq>());
    }

    [Test]
    public void ShouldMapToExpressionObjectMember()
    {
        var objectMemberExpr = ToExpressionNode(new ObjectMember<object?>(ExprMonad.Instance, nullExpr, nullExpr));
        Assert.That(objectMemberExpr, Is.TypeOf<ObjectMember>());
    }

    private static IExpressionNode ToExpressionNode(IExpression<object?> expression)
    {
        var expressionFactory = new ExpressionFactory<object?>(ExprMonad.Instance, new DefaultFunctions<object?>());
        var nodeMapper = new ExpressionNodeMapper<object?>(expressionFactory);
        return nodeMapper.ToExpressionNode(expression);
    }
}