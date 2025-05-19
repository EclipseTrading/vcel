namespace VCEL.Core.Expression.Abstract;

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
    InExpression,
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
    Spread,
}

public interface IExpressionNode
{
    NodeType Type { get; }
}