using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Impl;

namespace VCEL.Core.Expression.Func
{
    public class DefaultFunctions<T> : IFunctions<T>
    {
        private readonly Dictionary<string, Function<T>> functions
            = new Dictionary<string, Function<T>>();

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
            Register("round", args =>
            {
                switch (args.Length)
                {
                    case 1:
                        return VcelMath.Round(args[0]);
                    case 2:
                        return VcelMath.Round(args[0], args[1]);
                    default:
                        return null;
                }
            });
            RegisterEnsureOneArg("sign", arg => VcelMath.Sign(arg));
            RegisterEnsureOneArg("sin", arg => VcelMath.Sin(arg));
            RegisterEnsureOneArg("sinh", arg => VcelMath.Sinh(arg));
            RegisterEnsureOneArg("sqrt", arg => VcelMath.Sqrt(arg));
            RegisterEnsureOneArg("tan", arg => VcelMath.Tan(arg));
            RegisterEnsureOneArg("tanh", arg => VcelMath.Tanh(arg));
            RegisterEnsureOneArg("truncate", arg => VcelMath.Truncate(arg));

            Register("now", _ => DateTime.Now, TemporalDependency.Now);
            Register("today", _ => DateTime.Today, TemporalDependency.Today);
        }

        public Function<T>? GetFunction(string name)
        {
            return functions.TryGetValue(name.ToLower(), out var f) ? f : null;
        }

        public bool HasFunction(string name)
        {
            return functions.ContainsKey(name.ToLower());
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
            Register(name, (args, _) => args?.Length == 1 && args[0] == null ? null : func(args?[0]), new FuncDependency(name));
        }

        public void RegisterEnsureTwoArgs(string name, Func<object?, object?, object?> func)
        {
            Register(name, (args, _) => args.Length != 2 || (args[0] == null || args[1] == null) ? null : func(args[0], args[1]),
                new FuncDependency(name));
        }

        protected void Register(string name, Func<object?[], IContext<T>, object?> func, params IDependency[] deps)
        {
            functions[name.ToLower()] = new Function<T>(func, deps);
        }

        protected void Register(string name, Func<object?[], object?> func, params IDependency[] deps)
        {
            functions[name.ToLower()] = new Function<T>((args, _) => func(args), deps);
        }
    }
}
