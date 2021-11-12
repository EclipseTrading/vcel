using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class This : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.This;
    }
}