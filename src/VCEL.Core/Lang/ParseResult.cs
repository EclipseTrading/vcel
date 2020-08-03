using System.Collections.Generic;

namespace VCEL.Core.Lang
{
    public class ParseResult<T>
    {
        public ParseResult(IExpression<T> expression)
        {
            this.Expression = expression;
            this.ParseErrors = new ParseError[0];
            this.Success = true;
        }

        public ParseResult(IReadOnlyList<ParseError> errors)
        {
            this.ParseErrors = errors;
            this.Expression = null;
            this.Success = false;
        }

        public ParseResult(params ParseError[] errors)
            : this((IReadOnlyList<ParseError>)errors) { }

        public bool Success { get; }
        public IExpression<T> Expression { get; }
        public IReadOnlyList<ParseError> ParseErrors { get; }

    }
}
