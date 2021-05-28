using NUnit.Framework;
using VCEL.Core.Lang;
using VCEL.Test.Shared;

namespace VCEL.Test
{
    public class MemberAccessTests
    {
        [TestCase("@2020-01-01.Year", 2020)]
        [TestCase("@2020-01-01.Month", 1)]
        [TestCase("(@2020-01-05 - @2020-01-03).Days", 2)]
        [TestCase("(00:01:00 - 00:00:30).TotalSeconds", 30)]
        [TestCase("'Test'.Length", 4)]
        // Member access behaves like evaluating a sub-expression
        //  with the base object serving as the context - so you
        //  can do interesting expressions that have vb 'with' like
        //  semantics
        [TestCase("01:01:05.(Seconds + Minutes * 60 + Hours * 60 * 60)", 3665)]
        [TestCase("@2020-01-01.(Month + Year)", 2021)]
        public void EvalMember(string exprString, object expected)
        {
            foreach (var parseResult in CompositeExpression.ParseMultiple(exprString))
            {
                var expr = parseResult.Expression;
                var result = expr.Evaluate(new { });
                Assert.That(result, Is.EqualTo(expected));
            }
        }
    }
}
