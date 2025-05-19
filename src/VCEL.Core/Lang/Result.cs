using System;
using System.Collections.Generic;

namespace VCEL.Core.Lang;

public class Result<T>
{
    public Result(T expression)
    {
        this.Expression = expression;
        this.ParseErrors = [];
        this.Success = true;
    }

    public Result(IReadOnlyList<ParseError> errors)
    {
        this.ParseErrors = errors;
        this.Expression = default!;
        this.Success = false;
    }

    public Result(params ParseError[] errors)
        : this((IReadOnlyList<ParseError>)errors) { }

    public bool Success { get; protected set; }
    public T Expression { get; protected set; }
    public IReadOnlyList<ParseError> ParseErrors { get; protected set; }
}