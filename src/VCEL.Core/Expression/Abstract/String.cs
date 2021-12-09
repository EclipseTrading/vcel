using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class String : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.String;

        public String(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}