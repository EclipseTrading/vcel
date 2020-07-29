namespace VCEL.Core.Expression.Op
{
    public interface IOperators
    {
        IBinaryOperator Add { get; }
        IBinaryOperator Multiply { get; }
        IBinaryOperator Subtract { get; }
        IBinaryOperator Divide { get; }
        IBinaryOperator Pow { get; }

        BooleanOperator And { get; }
        BooleanOperator Or { get; }
    }
}
