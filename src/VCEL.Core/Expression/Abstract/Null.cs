using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Null : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.Null;
    }
}