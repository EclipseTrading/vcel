using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Long : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.Long;

        public Long(long value)
        {
            Value = value;
        }

        public long Value { get; }
    }
}