using System.Collections.Generic;

namespace VCEL.Core.Lang;

public class ValueParseResult<T> : ParseResult<T>
{
    public ValueParseResult(IExpression<T> expression, object value)
        : base(expression)
    {
        this.Value = value;
    }

    public ValueParseResult(IReadOnlyList<ParseError> errors)
        : base(errors)
    {
        this.ParseErrors = errors;
        this.Value = null!;
    }

    public ValueParseResult(params ParseError[] errors)
        : this((IReadOnlyList<ParseError>)errors) { }

    public object Value { get; }

}