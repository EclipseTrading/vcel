using System.Collections.Generic;

namespace VCEL.Core.Lang
{
    public class ParseResult<T> : Result<IExpression<T>>
    {
        public ParseResult(IExpression<T> expression) : base(expression) { }
        public ParseResult(IReadOnlyList<ParseError> errors) : base(errors) { }
        public ParseResult(params ParseError[] errors) : base(errors) { }
    }
}
