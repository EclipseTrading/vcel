using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Between : IBinary, IExpressionNode
    {
        public Between(IExpressionNode left, IExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.Between;

        public IExpressionNode Left { get; }
        public IExpressionNode Right { get; }
    }
}