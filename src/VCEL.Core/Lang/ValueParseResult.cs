using System.Collections.Generic;

namespace VCEL.Core.Lang
{
    public class ValueParseResult<T>
    {
        public ValueParseResult(object value, IExpression<T> parsed)
        {
            this.Value = value;
            this.Parsed = parsed;
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

        public bool Success { get; }
        public object Value { get; }
        public IExpression<T> Parsed { get; }
        public IReadOnlyList<ParseError> ParseErrors { get; }

    }
}
