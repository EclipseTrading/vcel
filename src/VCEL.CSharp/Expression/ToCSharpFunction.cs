using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.CSharp.Expression.Func;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpFunction(
    IMonad<string> monad,
    string name,
    IReadOnlyList<IExpression<string>> args,
    IFunctions<string>? functions
) : IExpression<string>
{
    private readonly IFunctions<string> functions = functions ?? new DefaultCSharpFunctions();

    public IMonad<string> Monad { get; } = monad;

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        if (functions.HasFunction(name))
        {
            var function = functions.GetFunction(name)!;
            var argValues = args.Select(object? (s) => s.Evaluate(context)).ToArray();
            return function.Func.Invoke(argValues, context)?.ToString() ?? "";
        }

        // TODO should return missing function
        return "";
    }
}