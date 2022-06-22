using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Int : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.Int;

        public Int(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}