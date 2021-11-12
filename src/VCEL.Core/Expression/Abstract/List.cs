using System.Collections.Generic;
using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class List : IExpressionNode
    {
        public List(IReadOnlyList<IExpressionNode> items)
        {
            Items = items;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.List;

        public IReadOnlyList<IExpressionNode> Items { get; }
    }
}