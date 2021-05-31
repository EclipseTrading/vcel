using NUnit.Framework;
using System.Linq;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class ParseErrorTests
    {
        [TestCase("A in", "A >>>in<<<")]
        [TestCase("x()", ">>>x<<<()")]
        [TestCase("1.", "1>>>.<<<")]
        [TestCase("1.1.1", "1.1>>>.1<<<")]
        [TestCase(".1.1", ".1>>>.1<<<")]
        [TestCase(@"
A + B @ C 
* D
", @"
A + B @ >>>C<<< 
* D
")]
        public void ParseErrorTest(string exprString, string exprError)
        {
            var parseResult = VCExpression.ParseDefault(exprString);
            Assert.That(parseResult.Success, Is.False);
            var exprMessage = string.Join(
                "\n",
                parseResult.ParseErrors.Select(e => e.GetExprError(exprString)));
            Assert.That(exprMessage, Is.EqualTo(exprError));
        }
    }
}
