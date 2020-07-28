using NUnit.Framework;
using VCEL;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;

namespace VECL.Test
{
    public class LegacyFunctionTests
    {
        [TestCase("T(System.Math).Abs(-1)", 1)]
        public void EvalLegacyFunction(string exprString, object expected)
        {
            var exprFactory = new ExpressionFactory<object>(ExprMonad.Instance);
            var parser = new ExpressionParser<object>(exprFactory);
            var expr = parser.Parse(exprString);
            var result = expr.Evaluate(new { });
            Assert.That(result, Is.EqualTo(expected));
        }
    
    }
}
