namespace VCEL.Core.Expression.Abstract
{
    public interface IBinary
    {
        IExpressionNode Left { get; }
        IExpressionNode Right { get; }
    }
}