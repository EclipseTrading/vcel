using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal class ToJsValueOfOp : IExpression<string>
{
    private readonly string opName;
    private readonly IExpression<string> left;
    private readonly IExpression<string> right;

    public ToJsValueOfOp(string opName, IMonad<string> monad, IExpression<string> left, IExpression<string> right)
    {
        this.opName = opName;
        Monad = monad;
        this.left = left;
        this.right = right;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context) => 
        $"({WrapValueOf(left, context)} {opName} {WrapValueOf(right, context)})";

    private static string WrapValueOf(IExpression<string> expression, IContext<string> context)
    {
        var value = expression.Evaluate(context);
        return expression is ToJsPropertyOp 
            ? $"({value}?.valueOf() ?? null)" 
            : value;
    }
}