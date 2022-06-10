using Newtonsoft.Json;

namespace VCEL.Core.Expression.Abstract
{
    public class In : IExpressionNode
    {
        public In(IExpressionNode left, IExpressionNode list) 
        {
            Left = left;
            List = list;
        }
        
        [JsonProperty("$type")]
        public NodeType Type => NodeType.InExpression;

        public IExpressionNode Left { get; }
        public IExpressionNode List { get; }
    }
}