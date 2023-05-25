using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using VCEL.Core.Expression.Abstract;
using VCEL.Core.Expression.Transformer;

namespace VCEL.Test.Expression.Transformer;

public class ExpressionNodeTransformerTests
{
    [Test]
    public void ShouldThrowWhenEmptyTransformers()
    {
        Assert.Throws<ArgumentException>(() =>
            ExpressionNodeTransformer.Transform(Bool.True, new Collection<IExpressionNodeVisitor>()));
    }
}