using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.JS.Expression;

internal class ToJsFunction : IExpression<string>
{
    private static readonly Dictionary<string, Func<IContext<string>, IReadOnlyList<IExpression<string>>, string>> JsFunctions = new()
    {
        { "abs", (context, args) => Func(context, args, "Math.abs") },
        { "acos", (context, args) => Func(context, args, "Math.acos") },
        { "asin", (context, args) => Func(context, args, "Math.asin") },
        { "atan", (context, args) => Func(context, args, "Math.atan") },
        { "atan2", (context, args) => Func(context, args, "Math.atan2") },
        { "ceiling", (context, args) => Func(context, args, "Math.ceil") },
        { "cos", (context, args) => Func(context, args, "Math.cos") },
        { "cosh", (context, args) => Func(context, args, "Math.cosh") },
        { "exp", (context, args) => Func(context, args, "Math.exp") },
        { "floor", (context, args) => Func(context, args, "Math.floor") },
        { "log", (context, args) => Func(context, args, "Math.log") },
        { "log10", (context, args) => Func(context, args, "Math.log10") },
        { "max", (context, args) => Func(context, args, "Math.max") },
        { "min", (context, args) => Func(context, args, "Math.min") },
        { "pow", (context, args) => Func(context, args, "Math.pow") },
        { "round", (context, args) => Func(context, args, "Math.round") },
        { "sign", (context, args) => Func(context, args, "Math.sign") },
        { "sin", (context, args) => Func(context, args, "Math.sin") },
        { "sinh", (context, args) => Func(context, args, "Math.sinh") },
        { "sqrt", (context, args) => Func(context, args, "Math.sqrt") },
        { "tan", (context, args) => Func(context, args, "Math.tan") },
        { "tanh", (context, args) => Func(context, args, "Math.tanh") },
        { "truncate", (context, args) => Func(context, args, "Math.trunc") },
        { "double", (context, args) => Func(context, args, "Number") },
        { "decimal", (context, args) => Func(context, args, "Number") },
        { "string", (context, args) => Func(context, args, "String") },

        { "now", (context, args) => Func(context, args, "new Date") },
        { "today", (context, args) => Func(context, args, "new Date") },
        { "datetime", (context, args) => Func(context, args, "new Date") },
        { "date", (context, args) => Func(context, args, "new Date") },

        { "int", (context, args) => NestedFunc(context, args, "Math.floor(Number") },
        { "long", (context, args) => NestedFunc(context, args, "Math.floor(Number") },

        { "lowercase", (context, args) => Method(context, args, "toLowerCase") },
        { "uppercase", (context, args) => Method(context, args, "toUpperCase") },
        { "substring", (context, args) => Method(context, args, "substring") },
        { "split", (context, args) => Method(context, args, "split") },
        { "replace", (context, args) => Method(context, args, "replace") },
        { "trim", (context, args) => Method(context, args, "trim") },
        { "length", (context, args) => Property(context, args, "length", "0") },
        { "contains", (context, args) => Method(context, args, "includes", "false") },
        { "startswith", (context, args) => Method(context, args, "startsWith", "false") }, // Keeping for backward compatability
        { "startsWith", (context, args) => Method(context, args, "startsWith", "false") },
        { "endsWith", (context, args) => Method(context, args, "endsWith", "false") },
        { "indexOf", (context, args) => Method(context, args, "indexOf") },
        { "lastIndexOf", (context, args) => Method(context, args, "lastIndexOf") },
        { "reverse", (context, args) => Method(context, args, _ => "split('').reverse().join('')") },

        { "get", (context, args) => IndexedAccessor(context, args) },
    };

    private readonly string name;
    private readonly IReadOnlyList<IExpression<string>> args;

    public ToJsFunction(IMonad<string> monad, string name, IReadOnlyList<IExpression<string>> args)
    {
        this.Monad = monad;
        this.name = name;
        this.args = args;
    }

    public IMonad<string> Monad { get; }

    public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        if (JsFunctions.TryGetValue(name, out var jsFunctionCreator))
        {
            return jsFunctionCreator(context, args);
        }

        return $"({name}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
    }

    private static string NestedFunc(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs, string jsDoubleFunc,
        string defaultValue = "undefined")
    {
        return IsContextEmpty(context)
            ? $"({jsDoubleFunc}({string.Join(",", functionArgs.Select(s => s.Evaluate(context)))})))"
            : CheckedMethodCall(context.Value, jsDoubleFunc,
                string.Join(",", functionArgs.Select(s => s.Evaluate(context))), defaultValue);
    }

    private static string Func(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs, string jsMethodName,
        string defaultValue = "undefined")
    {
        return IsContextEmpty(context)
            ? $"({jsMethodName}({string.Join(",", functionArgs.Select(s => s.Evaluate(context)))}))"
            : CheckedMethodCall(context.Value, jsMethodName, string.Join(",", functionArgs.Select(s => s.Evaluate(context))), defaultValue);
    }

    private static string Method(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs, string jsFunctionName,
        string defaultValue = "undefined")
    {
        return IsContextEmpty(context) && functionArgs.Count == 0
            ? defaultValue
            : IsContextEmpty(context)
                ? CheckedMethodCall(functionArgs[0].Evaluate(context), jsFunctionName,
                    string.Join(",", functionArgs.Skip(1).Select(s => s.Evaluate(context))), defaultValue)
                : CheckedMethodCall(context.Value, jsFunctionName, string.Join(",", functionArgs.Select(s => s.Evaluate(context))),
                    defaultValue);
    }

    private static string Method(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs,
        Func<string, string> jsFunctionName, string defaultValue = "undefined")
    {
        return IsContextEmpty(context) && functionArgs.Count == 0
            ? defaultValue
            : IsContextEmpty(context)
                ? CheckedAccess(functionArgs[0].Evaluate(context),
                    jsFunctionName(string.Join(",", functionArgs.Skip(1).Select(s => s.Evaluate(context)))), defaultValue)
                : CheckedAccess(context.Value, jsFunctionName(string.Join(",", functionArgs.Select(s => s.Evaluate(context)))),
                    defaultValue);
    }

    private static string IndexedAccessor(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs,
        string defaultValue = "undefined")
    {
        return $"({functionArgs[0].Evaluate(context)}?.[{functionArgs[1].Evaluate(context)}] ?? {defaultValue})";
    }

    private static string Property(IContext<string> context, IReadOnlyList<IExpression<string>> functionArgs, string propertyName,
        string defaultValue = "undefined")
    {
        return CheckedAccess(functionArgs[0].Evaluate(context), propertyName, defaultValue);
    }

    private static bool IsContextEmpty(IContext<string> context)
    {
        return string.IsNullOrEmpty(context.Value) || context.Value == "{ }";
    }

    private static string CheckedMethodCall(string variable, string method, string methodArgs, string defaultValue)
    {
        return $"({variable}?.{method}({methodArgs}) ?? {defaultValue})";
    }

    private static string CheckedAccess(string variable, string method, string defaultValue)
    {
        return $"({variable}?.{method} ?? {defaultValue})";
    }
}
