﻿using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class Add : IBinary, IExpressionNode
{
    public Add(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.Add;

    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }
}