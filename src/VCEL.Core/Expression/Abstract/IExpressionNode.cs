namespace VCEL.Core.Expression.Abstract
{
    public enum NodeType
    {
        Ternary,
        Let,
        Guard,
        LessThan,
        GreaterThan,
        LessOrEqual,
        GreaterOrEqual,
        Between,
        In,
        Matches,
        And,
        Or,
        Not,
        Value,
        List,
        Add,
        Multiply,
        Subtract,
        Divide,
        Pow,
        Paren,
        Property,
        Function,
        UnaryMinus,
        Null,
        Eq,
        NotEq,
        ObjectMember,
        Bool,
        DateTimeOffset,
        Double,
        Int,
        Long,
        Set,
        String,
        TimeSpan,
    }

    public interface IExpressionNode
    {
        NodeType Type { get; }
    }
}
