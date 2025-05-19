using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract;

public class InSet : IExpressionNode
{
    public InSet(IExpressionNode left, ISet<object> set)
    {
        Left = left;
        Set = set;
    }

    [JsonProperty("$type")]
    public NodeType Type => NodeType.In;

    public IExpressionNode Left { get; }
    public ISet<object> Set { get; }
}