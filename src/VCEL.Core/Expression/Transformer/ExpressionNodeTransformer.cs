using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Abstract;

namespace VCEL.Core.Expression.Transformer;

public static class ExpressionNodeTransformer
{
    public static IExpressionNode Transform(IExpressionNode root, IReadOnlyList<IExpressionNodeVisitor> transformers)
    {
        if (transformers.Count < 1)
            throw new ArgumentException("Must provide at least one transformer.", nameof(transformers));

        return transformers.Aggregate(root, (node, visitor) => node.Accept(visitor));
    }
}