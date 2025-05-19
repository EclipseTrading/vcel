using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using VCEL.Core.Expression.Abstract;
using VCEL.Core.Expression.Impl;
using VCEL.Core.Lang;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.Test.Expression.Abstract;

public class JsonSerializerTests
{
    private const string NumericAdditionJson =
        "{\"$type\":\"Add\",\"left\":{\"$type\":\"UnaryMinus\",\"expression\":{\"$type\":\"Int\",\"value\":1}},\"right\":{\"$type\":\"Double\",\"value\":1.1}}";

    [Test]
    public void ShouldWrite()
    {
        var expressionFactory = new ExpressionFactory<object?>(ExprMonad.Instance);

        var parser = new ExpressionParser<object?>(expressionFactory);

        var expression = parser.Parse("-1 + 1.1");

        var nodeMapper = new ExpressionNodeMapper<object?>(expressionFactory);

        var node = nodeMapper.ToExpressionNode(expression.Expression);

        var json = JsonConvert.SerializeObject(node, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new ExpressionNodeTypeConverter(),
                new StringEnumConverter(),
            },
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        });

        Assert.That(json, Is.EqualTo(NumericAdditionJson));
    }

    [Test]
    public void ShouldRead()
    {
        var node = JsonConvert.DeserializeObject<Add>(NumericAdditionJson, new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new ExpressionNodeTypeConverter() },
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        });

        Assert.That(node, Is.TypeOf<Add>());

        var nodeMapper = new ExpressionNodeMapper<object?>(new ExpressionFactory<object?>(ExprMonad.Instance));

        var expression = nodeMapper.ToExpression(node);

        Assert.That(expression, Is.TypeOf<AddExpr<object?>>());
        Assert.That(((AddExpr<object?>)expression).Left, Is.TypeOf<UnaryMinusExpr<object?>>());
        Assert.That(((AddExpr<object?>)expression).Right, Is.TypeOf<DoubleExpr<object?>>());
    }
}