using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class GreaterOrEqual : IBinary, IExpressionNode
    {
        public GreaterOrEqual(IExpressionNode left, IExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.GreaterOrEqual;

        public IExpressionNode Left { get; }
        public IExpressionNode Right { get; }
    }
}