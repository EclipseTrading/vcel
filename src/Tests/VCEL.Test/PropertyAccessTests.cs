using NUnit.Framework;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class PropertyAccessTests
    {
        [Test]
        public void AccessWithDifferentObjects()
        {
            var expr = VCExpression.ParseDefault("a + 1");
            var o1 = new { a = 1 };
            var o2 = new { b = 2, a = 3 };

            var result1 = expr.Expression.Evaluate(o1);
            var result2 = expr.Expression.Evaluate(o2);

            Assert.That(result1, Is.EqualTo(2));
            Assert.That(result2, Is.EqualTo(4));
        }

    }
}

