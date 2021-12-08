using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Set : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.Set;

        public Set(ISet<object> value)
        {
            Value = value;
        }

        public ISet<object> Value { get; }
    }
}