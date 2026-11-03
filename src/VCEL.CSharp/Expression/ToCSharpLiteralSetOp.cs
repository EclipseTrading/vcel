using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpLiteralSetOp(IMonad<string> monad, ISet<object> values) : IExpression<string>
{
    public IMonad<string> Monad { get; } = monad;
    public IEnumerable<IDependency> Dependencies => [];

    public string Evaluate(IContext<string> context)
        => $"(new HashSet<object>{{{string.Join(",", values.Select(ItemCode))}}})";

    internal string Membership(string value)
    {
        if (values.Count == 0)
        {
            return "false";
        }

        // Formatting can collapse distinct parsed values, such as 1 and 1.0.
        var comparisons = values.Select(ItemCode).Distinct()
            .Select(item => $"TypeOperation.EqualsChecked((object)({value}), (object)({item}))");
        return $"({string.Join(" || ", comparisons)})";
    }

    private static string ItemCode(object? value)
        => value is string ? $@"""{value}""" : value?.ToString() ?? "\"null\"";
}
