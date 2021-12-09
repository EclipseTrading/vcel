using System.Collections.Generic;
using System.Collections.Immutable;
using NUnit.Framework;
using VCEL.Core.Expression.Abstract;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Expression;
using VCEL.Monad;
using List = VCEL.Core.Expression.Abstract.List;

namespace VCEL.Test.Expression.Abstract
{
    public class ExpressionNodeMapperToExpressionTests
    {
        [Test]
        public void ShouldMapToExpressionTernary()
        {
            var ternaryExpr = ToExpression(new Ternary(new Null(), new Null(), new Null()));
            Assert.That(ternaryExpr, Is.TypeOf<Ternary<object>>());
            Assert.That(((Ternary<object>)ternaryExpr).ConditionExpr, Is.TypeOf<NullExpr<object>>());
            Assert.That(((Ternary<object>)ternaryExpr).TrueExpr, Is.TypeOf<NullExpr<object>>());
            Assert.That(((Ternary<object>)ternaryExpr).FalseExpr, Is.TypeOf<NullExpr<object>>());
        }
        
        [Test]
        public void ShouldMapToExpressionTernaryWithValues()
        {
            var ternaryExpr = ToExpression(new Ternary(new Value(true), new Value(11), new Value(12)));
            Assert.That(ternaryExpr, Is.TypeOf<Ternary<object>>());
            Assert.That(((Ternary<object>)ternaryExpr).ConditionExpr, Is.TypeOf<ValueExpr<object, object>>());
            Assert.That(((ValueExpr<object, object>)((Ternary<object>)ternaryExpr).ConditionExpr).Value, Is.EqualTo(true));
            Assert.That(((Ternary<object>)ternaryExpr).TrueExpr, Is.TypeOf<ValueExpr<object, object>>());
            Assert.That(((ValueExpr<object, object>)((Ternary<object>)ternaryExpr).TrueExpr).Value, Is.EqualTo(11));
            Assert.That(((Ternary<object>)ternaryExpr).FalseExpr, Is.TypeOf<ValueExpr<object, object>>());
            Assert.That(((ValueExpr<object, object>)((Ternary<object>)ternaryExpr).FalseExpr).Value, Is.EqualTo(12));
        }

        [Test]
        public void ShouldMapToExpressionLet()
        {
            var letExpr = ToExpression(new Let(ImmutableArray<(string Binding, IExpressionNode Expression)>.Empty, new Null()));
            Assert.That(letExpr, Is.TypeOf<LetExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionGuard()
        {
            var guardExpr = ToExpression(new Guard(ImmutableArray<(IExpressionNode Condition, IExpressionNode Expression)>.Empty,
                new Null()));
            Assert.That(guardExpr, Is.TypeOf<GuardExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionLessThan()
        {
            var lessThanExpr = ToExpression(new LessThan(new Null(), new Null()));
            Assert.That(lessThanExpr, Is.TypeOf<LessThan<object>>());
        }

        [Test]
        public void ShouldMapToExpressionGreaterThan()
        {
            var greaterThanExpr = ToExpression(new GreaterThan(new Null(), new Null()));
            Assert.That(greaterThanExpr, Is.TypeOf<GreaterThan<object>>());
        }

        [Test]
        public void ShouldMapToExpressionLessOrEqual()
        {
            var lessOrEqualExpr = ToExpression(new LessOrEqual(new Null(), new Null()));
            Assert.That(lessOrEqualExpr, Is.TypeOf<LessOrEqual<object>>());
        }

        [Test]
        public void ShouldMapToExpressionGreaterOrEqual()
        {
            var greaterOrEqualExpr = ToExpression(new GreaterOrEqual(new Null(), new Null()));
            Assert.That(greaterOrEqualExpr, Is.TypeOf<GreaterOrEqual<object>>());
        }

        [Test]
        public void ShouldMapToExpressionBetween()
        {
            var betweenExpr = ToExpression(new Between(new Null(), new Null()));
            Assert.That(betweenExpr, Is.TypeOf<BetweenExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionIn()
        {
            var inExpr = ToExpression(new In(new Null(), new HashSet<object>()));
            Assert.That(inExpr, Is.TypeOf<InExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionMatches()
        {
            var matchesExpr = ToExpression(new Matches(new Null(), new Null()));
            Assert.That(matchesExpr, Is.TypeOf<MatchesExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionAnd()
        {
            var andExpr = ToExpression(new And(new Null(), new Null()));
            Assert.That(andExpr, Is.TypeOf<AndExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionOr()
        {
            var orExpr = ToExpression(new Or(new Null(), new Null()));
            Assert.That(orExpr, Is.TypeOf<OrExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionNot()
        {
            var notExpr = ToExpression(new Not(new Null()));
            Assert.That(notExpr, Is.TypeOf<NotExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionValue()
        {
            var valueExpr = ToExpression(new Value(42));
            Assert.That(valueExpr, Is.TypeOf<ValueExpr<object, object>>());
        }

        [Test]
        public void ShouldMapToExpressionList()
        {
            var listExpr = ToExpression(new List(ImmutableArray<IExpressionNode>.Empty));
            Assert.That(listExpr, Is.TypeOf<ListExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionAdd()
        {
            var addExpr = ToExpression(new Add(new Null(), new Null()));
            Assert.That(addExpr, Is.TypeOf<AddExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionMultiply()
        {
            var multiplyExpr = ToExpression(new Multiply(new Null(), new Null()));
            Assert.That(multiplyExpr, Is.TypeOf<MultExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionSubtract()
        {
            var subtractExpr = ToExpression(new Subtract(new Null(), new Null()));
            Assert.That(subtractExpr, Is.TypeOf<SubtractExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionDivide()
        {
            var divideExpr = ToExpression(new Divide(new Null(), new Null()));
            Assert.That(divideExpr, Is.TypeOf<DivideExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionPow()
        {
            var powExpr = ToExpression(new Pow(new Null(), new Null()));
            Assert.That(powExpr, Is.TypeOf<PowExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionParen()
        {
            var parenExpr = ToExpression(new Paren(new Null()));
            Assert.That(parenExpr, Is.TypeOf<ParenExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionProperty()
        {
            var propertyExpr = ToExpression(new Property("Test"));
            Assert.That(propertyExpr, Is.TypeOf<Property<object>>());
        }

        [Test]
        public void ShouldMapToExpressionFunction()
        {
            var functionExpr = ToExpression(new Function("abs", ImmutableArray<IExpressionNode>.Empty));
            Assert.That(functionExpr, Is.TypeOf<FunctionExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionUnaryMinus()
        {
            var unaryMinusExpr = ToExpression(new UnaryMinus(new Null()));
            Assert.That(unaryMinusExpr, Is.TypeOf<UnaryMinusExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionNull()
        {
            var nullExpr = ToExpression(new Null());
            Assert.That(nullExpr, Is.TypeOf<NullExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionEq()
        {
            var eqExpr = ToExpression(new Eq(new Null(), new Null()));
            Assert.That(eqExpr, Is.TypeOf<EqExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionNotEq()
        {
            var notEqExpr = ToExpression(new NotEq(new Null(), new Null()));
            Assert.That(notEqExpr, Is.TypeOf<NotEqExpr<object>>());
        }

        [Test]
        public void ShouldMapToExpressionObjectMember()
        {
            var objectMemberExpr = ToExpression(new ObjectMember(new Null(), new Null()));
            Assert.That(objectMemberExpr, Is.TypeOf<ObjectMember<object>>());
        }

        private static IExpression<object> ToExpression(IExpressionNode expressionNode)
        {
            var expressionFactory = new ExpressionFactory<object>(ExprMonad.Instance, new DefaultFunctions<object>());
            var nodeMapper = new ExpressionNodeMapper<object>(expressionFactory);
            return nodeMapper.ToExpression(expressionNode);
        }
    }
}
