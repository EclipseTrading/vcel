using System.Collections.Generic;

namespace VCEL.Core.Lang
{
    public class ValueParseResult<T> : ParseResult<T>
    {
        public ValueParseResult(IExpression<T> expression, object value)
        {
            this.Value = value;
            this.Expression = expression;
            this.ParseErrors = new ParseError[0];
            this.Success = true;
        }

        public ValueParseResult(IReadOnlyList<ParseError> errors)
        {
            this.ParseErrors = errors;
            this.Value = null;
            this.Success = false;
        }

        public ValueParseResult(params ParseError[] errors)
            : this((IReadOnlyList<ParseError>)errors) { }

        public object Value { get; }

    }
}
