using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Set : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.Set;

        public Set(ISet<IExpressionNode> value)
        {
            Value = value;
        }

        public ISet<IExpressionNode> Value { get; }
    }
}