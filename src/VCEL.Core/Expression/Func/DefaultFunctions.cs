using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Impl;

namespace VCEL.Core.Expression.Func;

public class DefaultFunctions<T> : IFunctions<T>
{
    private readonly Dictionary<string, Function<T>> functions = new();

    public DefaultFunctions()
    {
        RegisterEnsureOneArg("abs", arg => VcelMath.Abs(arg));
        RegisterEnsureOneArg("acos", arg => VcelMath.Acos(arg));
        RegisterEnsureOneArg("asin", arg => VcelMath.Asin(arg));
        RegisterEnsureOneArg("atan", arg => VcelMath.Atan(arg));
        RegisterEnsureTwoArgs("atan2", (arg1, arg2) => VcelMath.Atan2(arg1, arg2));
        RegisterEnsureOneArg("ceiling", arg => VcelMath.Ceiling(arg));
        RegisterEnsureOneArg("cos", arg => VcelMath.Cos(arg));
        RegisterEnsureOneArg("cosh", arg => VcelMath.Cosh(arg));
        RegisterEnsureOneArg("exp", arg => VcelMath.Exp(arg));
        RegisterEnsureOneArg("floor", arg => VcelMath.Floor(arg));
        RegisterEnsureOneArg("log", arg => VcelMath.Log(arg));
        RegisterEnsureOneArg("log10", arg => VcelMath.Log10(arg));
        Register("max", Enumerable.Max);
        Register("min", Enumerable.Min);
        RegisterEnsureTwoArgs("pow", (arg1, arg2) => VcelMath.Pow(arg1, arg2));
        RegisterEnsureTwoArgs("mod", (arg1, arg2) => VcelMath.Mod(arg1, arg2));
        RegisterEnsureOneArg("int", arg => VcelType.Integer(arg));
        RegisterEnsureOneArg("long", arg => VcelType.Long(arg));
        RegisterEnsureOneArg("double", arg => VcelType.Double(arg));
        RegisterEnsureOneArg("decimal", arg => VcelType.Decimal(arg));
        Register("string", args => args.Length switch
        {
            1 => VcelType.String(args[0]),
            2 => VcelType.String(args[0], args[1]),
            _ => (object?)null,
        });
        RegisterEnsureOneArg("bool", arg => VcelType.Boolean(arg));
        Register("round", args => args.Length switch
        {
            1 => VcelMath.Round(args[0]),
            2 => VcelMath.Round(args[0], args[1]),
            _ => (object?)null,
        });
        RegisterEnsureOneArg("sign", arg => VcelMath.Sign(arg));
        RegisterEnsureOneArg("sin", arg => VcelMath.Sin(arg));
        RegisterEnsureOneArg("sinh", arg => VcelMath.Sinh(arg));
        RegisterEnsureOneArg("sqrt", arg => VcelMath.Sqrt(arg));
        RegisterEnsureOneArg("tan", arg => VcelMath.Tan(arg));
        RegisterEnsureOneArg("tanh", arg => VcelMath.Tanh(arg));
        RegisterEnsureOneArg("truncate", arg => VcelMath.Truncate(arg));
        RegisterEnsureOneArg("datetime", arg => VcelDateTime.ToDateTime(arg));
        RegisterEnsureOneArg("date", arg => VcelDateTime.ToDate(arg));

        Register("now", _ => DateTime.Now, TemporalDependency.Now);
        Register("today", args => VcelDateTime.Today(args), TemporalDependency.Today);
        FunctionHelper.RegisterEnsureArgs<T, object>("workday", args => VcelDateTime.Workday(VcelDateTime.ParseWorkdayParams(args)),
            Register, 2, 3, allowNullArgument: false);

        RegisterEnsureOneArg("lowercase", arg => arg?.ToString()?.ToLower());
        RegisterEnsureOneArg("uppercase", arg => arg?.ToString()?.ToUpper());

        Register("substring", VcelString.Substring);
        RegisterEnsureTwoArgsAllowNull<object?, object?, object?[]>("split", (arg1, arg2) => VcelString.Split(arg1?.ToString(), arg2?.ToString()));
        RegisterEnsureThreeArgsAllowNull("replace", (arg1, arg2, arg3) => VcelString.Replace(arg1?.ToString(), arg2?.ToString(), arg3?.ToString()));
        RegisterEnsureOneArg("trim", arg => VcelString.Trim(arg?.ToString()));

        RegisterEnsureOneArgAllowNull<object?, int?>("length", VcelIndexable.Length);
        RegisterEnsureTwoArgsAllowNull<object?, object?, bool>("contains", VcelIndexable.Contains);
        RegisterEnsureTwoArgsAllowNull<object?, object?, bool>("startsWith", VcelIndexable.StartsWith);
        RegisterEnsureTwoArgsAllowNull<object?, object?, bool>("endsWith", VcelIndexable.EndsWith);
        RegisterEnsureTwoArgsAllowNull<object?, object?, int?>("indexOf", VcelIndexable.IndexOf);
        RegisterEnsureTwoArgsAllowNull<object?, object?, int?>("lastIndexOf", VcelIndexable.LastIndexOf);
        RegisterEnsureOneArg("reverse", VcelIndexable.Reverse);
        RegisterEnsureTwoArgsAllowNull<object?, object?, object?>("get", VcelIndexable.Get);
    }

    public Function<T>? GetFunction(string name)
    {
        return functions.TryGetValue(name, out var f) ? f : null;
    }

    public bool HasFunction(string name)
    {
        return functions.ContainsKey(name);
    }

    public void Register(string name, Func<object?[], IContext<T>, object?> func)
    {
        Register(name, func, new FuncDependency(name));
    }

    public void Register(string name, Func<object?[], object?> func)
    {
        Register(name, (args, _) => func(args), new FuncDependency(name));
    }

    public void RegisterEnsureOneArg(string name, Func<object?, object?> func)
    {
        Register(name, (args, _) => args.Length != 1 || args[0] == null ? null : func(args[0]), new FuncDependency(name));
    }

    public void RegisterEnsureOneArgAllowNull<TArg1, TReturn>(string name, Func<TArg1?, TReturn> func)
    {
        Register(name, (args, _) => args.Length != 1 ? null : func((TArg1?)args[0]), new FuncDependency(name));
    }

    public void RegisterEnsureTwoArgs(string name, Func<object?, object?, object?> func)
    {
        Register(name, (args, _) => args.Length != 2 || args[0] == null || args[1] == null ? null : func(args[0], args[1]),
            new FuncDependency(name));
    }

    public void RegisterEnsureTwoArgsAllowNull<TArg1, TArg2, TReturn>(string name, Func<TArg1?, TArg2?, TReturn> func)
    {
        Register(name, (args, _) => args.Length != 2 ? null : func((TArg1?)args[0], (TArg2?)args[1]), new FuncDependency(name));
    }

    public void RegisterEnsureThreeArgs(string name, Func<object?, object?, object?, object?> func)
    {
        Register(name, (args, _) => args.Length != 3 || args[0] == null || args[1] == null || args[2] == null
                ? null
                : func(args[0], args[1], args[2]),
            new FuncDependency(name));
    }

    public void RegisterEnsureThreeArgsAllowNull(string name, Func<object?, object?, object?, object?> func)
    {
        Register(name, (args, _) => args.Length != 3 ? null : func(args[0], args[1], args[2]), new FuncDependency(name));
    }

    protected void Register(string name, Func<object?[], IContext<T>, object?> func, params IDependency[] deps)
    {
        functions[name] = new Function<T>(func, deps);
    }

    protected void Register(string name, Func<object?[], object?> func, params IDependency[] deps)
    {
        functions[name] = new Function<T>((args, _) => func(args), deps);
    }
}
