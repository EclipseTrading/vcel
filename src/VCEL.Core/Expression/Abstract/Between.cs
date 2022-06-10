using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Between : IExpressionNode
    {
        public Between(IExpressionNode left, IExpressionNode lower, IExpressionNode upper)
        {
            Left = left;
            Lower = lower;
            Upper = upper;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.Between;

        public IExpressionNode Left { get; }
        public IExpressionNode Lower { get; }
        public IExpressionNode Upper { get; }
    }
}