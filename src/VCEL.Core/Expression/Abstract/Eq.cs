using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Eq : IBinary, IExpressionNode
    {
        public Eq(IExpressionNode left, IExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.Eq;

        public IExpressionNode Left { get; }
        public IExpressionNode Right { get; }
    }
}