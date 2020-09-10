using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using VCEL;
using VCEL.Core.Lang;
using VCEL.Core.Monad.List;
using VCEL.Core.Monad.Tasks;
using VCEL.Expression;
using VCEL.Monad;
using VCEL.Monad.Maybe;

namespace VECL.Test
{
    public class MonadTests
    {
        [Test]
        public void DefaultMonad()
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse("A + 0.5 + 0.5").Expression;
            var result = expr.Evaluate(new { A = 0.5d });

            Assert.That((double)result, Is.EqualTo(1.5).Within(0.00000001));
        }

        [Test]
        public void MaybeSome()
        {
            var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
            var parser = new ExpressionParser<Maybe<object>>(exprFactory);
            var expr = parser.Parse("A + 0.5 + 0.5").Expression;
            var result = expr.Evaluate(new { A = 0.5d });

            Assert.That(result.HasValue);
            Assert.That((double)result.Value, Is.EqualTo(1.5).Within(0.00000001));
        }

        [Test]
        public void MaybeNothing()
        {
            var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
            var parser = new ExpressionParser<Maybe<object>>(exprFactory);
            var expr = parser.Parse("A + 0.5 + 0.5").Expression;
            var result = expr.Evaluate(new { });

            Assert.That(result.HasValue, Is.False);
        }

        [Test]
        public async Task TaskM()
        {
            var exprFactory = new ExpressionFactory<Task<object>>(TaskMonad.Instance);
            var parser = new ExpressionParser<Task<object>>(exprFactory);
            var expr = parser.Parse("A + 0.5 + 0.5").Expression;

            var resultTask = expr.Evaluate(
                new { A = Task.Delay(500).ContinueWith(t => 0.5d) });
            var result = await resultTask;
            Assert.That(result, Is.EqualTo(1.5).Within(0.000000001));
        }

        [Test]
        public void ToStringM()
        {
            var exprFactory = new ToStringExpressionFactory<object>(ConcatStringMonad.Instance);

            var parser = new ExpressionParser<string>(exprFactory);
            var expr = parser.Parse("A + 0.5 + 0.5").Expression;
            var result = expr.Evaluate(new { A = 0.5d });

            Assert.That(result, Is.EqualTo("A + 0.5 + 0.5"));
        }

        [Test]
        public void DivideByZero()
        {
            var exprFactory = new MaybeExpressionFactory(MaybeMonad.Instance);
            var parser = new ExpressionParser<Maybe<object>>(exprFactory);
            var expr = parser.Parse("1/0").Expression;
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(Maybe<object>.None));
        }

        [Test]
        public void NaNTest()
        {
            var expr = VCExpression.ParseMaybe("A / B / C * 100");
            var res = expr.Expression.Evaluate(new { A = 1.0, B = 1.0, C = double.NaN });
            Assert.That(res.Value, Is.EqualTo(double.NaN));
        }
    }
}
