namespace VCEL.Core.Expression.Abstract
{
    public class Spread : IExpressionNode 
    {
        public Spread(IExpressionNode list) 
        {
            List = list;
        }

        public IExpressionNode List { get; }

        public NodeType Type => NodeType.Spread;
    }
}