using NUnit.Framework;
using System.Collections.Generic;
using VCEL.Core.Lang;

namespace VCEL.Test
{
    public class DictionaryAccessTests
    {
        [Test]
        public void AccessValueInDictionary()
        {
            var expr = VCExpression.ParseDefault(
                "(a + b) * c");

            var dic = new Dictionary<string, object>
            {
                { "a", 10 },
                { "b", 5 },
                { "c", 3 }
            };

            var result = expr.Expression.Evaluate(dic);
            Assert.That(result, Is.EqualTo(45));
        }

        [Test]
        public void AccessObjectInDictionary()
        {
            var expr = VCExpression.ParseDefault(
                "(a + b.b_value) * c");

            var dic = new Dictionary<string, object>
            {
                { "a", 10 },
                {
                    "b",
                    new
                    {
                        b_value = 5
                    }
                },
                { "c", 3 }
            };

            var result = expr.Expression.Evaluate(dic);
            Assert.That(result, Is.EqualTo(45));
        }

        [Test]
        public void AccessDictionaryInDictionary()
        {
            var expr = VCExpression.ParseDefault(
                "(a + b.b_value) * c");

            var dic = new Dictionary<string, object>
            {
                { "a", 10 },
                {
                    "b",
                    new Dictionary<string, object>()
                    {
                        { "b_value", 5 }
                    }
                },
                { "c", 3 }
            };

            var result = expr.Expression.Evaluate(dic);
            Assert.That(result, Is.EqualTo(45));
        }

        [Test]
        public void AccessDictionaryInDictionary_WithOtherCalculation()
        {
            var expr = VCExpression.ParseDefault(
                "(a + b.b_value) ^ 2 * c + 25");

            var dic = new Dictionary<string, object>
            {
                { "a", 10 },
                {
                    "b",
                    new Dictionary<string, object>()
                    {
                        { "b_value", 5 }
                    }
                },
                { "c", 3 }
            };

            var result = expr.Expression.Evaluate(dic);
            Assert.That(result, Is.EqualTo(700));
        }


        [Test]
        public void AccessDictionaryInDictionary_WithOverride()
        {
            var expr = VCExpression.ParseDefault(
                "let x = a + b.b_value in x");

            var dic = new Dictionary<string, object>
            {
                { "a", 10 },
                {
                    "b",
                    new Dictionary<string, object>()
                    {
                        { "b_value", 5 }
                    }
                }
            };

            var result = expr.Expression.Evaluate(dic);
            Assert.That(result, Is.EqualTo(15));
        }
    }
}

