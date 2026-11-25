using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal class ToCSharpBinaryOp : BinaryExprBase<string>
{
    private readonly string opName;

    internal string Operation => opName switch
    {
        "+" => "Add",
        "-" => "Subtract",
        "*" => "Multiply",
        _ => throw new System.InvalidOperationException($"Unsupported arithmetic operator: {opName}")
    };

    public ToCSharpBinaryOp(
        string opName,
        IMonad<string> monad,
        IExpression<string> left,
        IExpression<string> right)
        : base(monad, left, right)
    {
        this.opName = opName;
    }

    public override string Evaluate(object? lv, object? rv)
        =>  $"({lv} {opName} {rv})";

    public override string Evaluate(IContext<string> context) => CSharpArithmeticCode.Evaluate(this, context);
}