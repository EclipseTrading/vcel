using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VCEL.Core;
using VCEL.Core.Expression;
using VCEL.Expression;

namespace VCEL.Test.Expression
{
    public class ToStringExpressionFactoryTests
    {
        private IExpressionFactory<string> factory;
        private IContext<string> context;

        [SetUp]
        public void Setup()
        {
            factory = new ToStringExpressionFactory(ConcatStringMonad.Instance);
            context = new DictionaryContext<string>(ConcatStringMonad.Instance, new Dictionary<string, object>());
        }

        [Test]
        public void ShouldCreatePropertyExpression()
        {
            var outcome = factory.Property("foo");
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo"));
        }

        [Test]
        public void ShouldCreateAddExpression()
        {
            var outcome = factory.Add(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo + bar"));
        }

        [Test]
        public void ShouldCreateSubtractExpression()
        {
            var outcome = factory.Subtract(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo - bar"));
        }

        [Test]
        public void ShouldCreatePowExpression()
        {
            var outcome = factory.Pow(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo ^ bar"));
        }

        [Test]
        public void ShouldCreateDivideExpression()
        {
            var outcome = factory.Divide(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo / bar"));
        }

        [Test]
        public void ShouldCreateMultiplyExpression()
        {
            var outcome = factory.Multiply(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo * bar"));
        }

        [Test]
        public void ShouldCreateBetweenExpression()
        {
            var outcome = factory.Between(factory.Property("foo"), factory.Set(new[] { "bar", "baz" }.ToHashSet<object>()));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo between { bar, baz }"));
        }

        [Test]
        public void ShouldCreateTernaryExpression()
        {
            var outcome = factory.Ternary(factory.Property("foo"), factory.Property("bar"), factory.Property("baz"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo ? bar : baz"));
        }

        [Test]
        public void ShouldCreateLetExpression()
        {
            var outcome = factory.Let(new[] { ("foo", factory.Property("bar")) }, factory.Property("baz"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo($"let{Environment.NewLine}    foo = bar{Environment.NewLine}in baz"));
        }

        [Test]
        public void ShouldCreateGuardExpression()
        {
            var outcome = factory.Guard(new[] { (factory.Property("foo"), factory.Property("bar")) }, factory.Property("baz"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo($"match{Environment.NewLine}| foo = bar{Environment.NewLine}| otherwise baz"));
        }

        [Test]
        public void ShouldCreateLessThanExpression()
        {
            var outcome = factory.LessThan(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo < bar"));
        }

        [Test]
        public void ShouldCreateGreaterThanExpression()
        {
            var outcome = factory.GreaterThan(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo > bar"));
        }

        [Test]
        public void ShouldCreateLessOrEqualExpression()
        {
            var outcome = factory.LessOrEqual(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo <= bar"));
        }

        [Test]
        public void ShouldCreateGreaterOrEqualExpression()
        {
            var outcome = factory.GreaterOrEqual(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo >= bar"));
        }

        [Test]
        public void ShouldCreateInExpression()
        {
            var outcome = factory.In(factory.Property("foo"), new[] { "bar", "baz" }.ToHashSet<object>());
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo in { bar, baz }"));
        }

        [Test]
        public void ShouldCreateMatchesExpression()
        {
            var outcome = factory.Matches(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo ~ bar"));
        }

        [Test]
        public void ShouldCreateAndExpression()
        {
            var outcome = factory.And(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo and bar"));
        }

        [Test]
        public void ShouldCreateOrExpression()
        {
            var outcome = factory.Or(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo or bar"));
        }

        [Test]
        public void ShouldCreateNotExpression()
        {
            var outcome = factory.Not(factory.Property("foo"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("!foo"));
        }

        [Test]
        public void ShouldCreateIntExpression()
        {
            var outcome = factory.Int(42);
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("42"));
        }

        [Test]
        public void ShouldCreateLongExpression()
        {
            var outcome = factory.Long(42);
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("42"));
        }

        [Test]
        public void ShouldCreateDoubleExpression()
        {
            var outcome = factory.Double(42.1);
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("42.1"));
        }

        [Test]
        public void ShouldCreateStringExpression()
        {
            var outcome = factory.String("foo");
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("'foo'"));
        }

        [Test]
        public void ShouldCreateDateTimeOffsetExpression()
        {
            var outcome = factory.DateTimeOffset(new DateTimeOffset(new DateTime(2012, 1, 1), TimeSpan.Zero));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("@2012-01-01T12:00:00.000+00:00"));
        }

        [Test]
        public void ShouldCreateTimeSpanExpression()
        {
            var outcome = factory.TimeSpan(new TimeSpan(1, 2, 3));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("01:02:03.000"));
        }

        [Test]
        public void ShouldCreateSetExpression()
        {
            var outcome = factory.Set(new[] { "foo", "bar" }.ToHashSet<object>());
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("{ foo, bar }"));
        }

        [Test]
        public void ShouldCreateValueExpression()
        {
            var outcome = factory.Value("foo");
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo"));
        }

        [Test]
        public void ShouldCreateBoolExpression()
        {
            var outcome = factory.Bool(true);
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("true"));
        }

        [Test]
        public void ShouldCreateListExpression()
        {
            var outcome = factory.List(new[] { factory.Property("foo"), factory.Property("bar") });
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("{ foo, bar }"));
        }

        [Test]
        public void ShouldCreateParenExpression()
        {
            var outcome = factory.Paren(factory.Property("foo"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("(foo)"));
        }

        [Test]
        public void ShouldCreateFunctionExpression()
        {
            var outcome = factory.Function("foo", new[] { factory.Property("bar") });
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo(bar)"));
        }

        [Test]
        public void ShouldCreateUnaryMinusExpression()
        {
            var outcome = factory.UnaryMinus(factory.Property("foo"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("-foo"));
        }

        [Test]
        public void ShouldCreateNullExpression()
        {
            var outcome = factory.Null();
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("null"));
        }

        [Test]
        public void ShouldCreateEqExpression()
        {
            var outcome = factory.Eq(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo == bar"));
        }

        [Test]
        public void ShouldCreateNotEqExpression()
        {
            var outcome = factory.NotEq(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo != bar"));
        }

        [Test]
        public void ShouldCreateMemberExpression()
        {
            var outcome = factory.Member(factory.Property("foo"), factory.Property("bar"));
            var evaluated = outcome.Evaluate(context);
            Assert.That(evaluated, Is.EqualTo("foo.bar"));
        }
    }
}
