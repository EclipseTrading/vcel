using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class TimeSpan : IExpressionNode
    {
        [JsonProperty("$type")]
        public NodeType Type => NodeType.TimeSpan;

        public TimeSpan(System.TimeSpan value)
        {
            Value = value;
        }

        public System.TimeSpan Value { get; }
    }
}