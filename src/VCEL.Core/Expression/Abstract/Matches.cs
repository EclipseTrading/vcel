using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class Matches : IBinary, IExpressionNode
    {
        public Matches(IExpressionNode left, IExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        [JsonProperty("$type")]
        public NodeType Type => NodeType.Matches;

        public IExpressionNode Left { get; }
        public IExpressionNode Right { get; }
    }
}