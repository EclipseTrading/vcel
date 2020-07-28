namespace VCEL.Core.Expression.Op
{
    public interface IBinaryOperator
    {
        object Evaluate(object l, object r);
    }
}
