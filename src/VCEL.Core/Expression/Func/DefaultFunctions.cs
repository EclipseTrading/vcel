﻿using System;
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
            RegisterEnsureTwoArgs("mod", (arg1, arg2) => VcelMath.Mod(arg1, arg2));
            RegisterEnsureOneArg("int", arg => VcelType.Integer(arg));
            RegisterEnsureOneArg("long", arg => VcelType.Long(arg));
            RegisterEnsureOneArg("double", arg => VcelType.Double(arg));
            RegisterEnsureOneArg("decimal", arg => VcelType.Decimal(arg));
            Register("string", args => 
            {
                switch(args.Length)
                {
                    case 1:
                        return VcelType.String(args[0]);
                    case 2:
                        return VcelType.String(args[0], args[1]);
                    default:
                        return null;
                }
            });
            RegisterEnsureOneArg("bool", arg => VcelType.Boolean(arg));
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
            RegisterEnsureOneArg("datetime", arg => VcelDateTime.ToDateTime(arg));
            RegisterEnsureOneArg("date", arg => VcelDateTime.ToDate(arg));

            Register("now", _ => DateTime.Now, TemporalDependency.Now);
            Register("today", _ => DateTime.Today, TemporalDependency.Today);
            FunctionHelper.RegisterEnsureArgs<T, object>("workday", args => VcelDateTime.Workday(VcelDateTime.ParseWorkdayParams(args)),
                Register, 2, 3, allowNullArgument: false);

            RegisterEnsureOneArg("lowercase", arg => arg?.ToString().ToLower());
            RegisterEnsureOneArg("uppercase", arg => arg?.ToString().ToUpper());

            Register("substring", Substring);
            RegisterEnsureTwoArgs("split", (arg1, arg2) => Split(arg1?.ToString() ?? string.Empty, arg2?.ToString() ?? string.Empty));
            RegisterEnsureThreeArgs("replace", (arg1, arg2, arg3) => Replace(arg1?.ToString() ?? string.Empty, arg2?.ToString() ?? string.Empty, arg3?.ToString() ?? string.Empty));
        }

        private static string? Substring(object?[] args)
        {
            switch (args.Length)
            {
                case 2:
                    {
                        var sourStr = args[0]?.ToString();
                        var startIndex = int.Parse(args[1]?.ToString());
                        return sourStr?.Substring(startIndex);
                    }
                case 3:
                    {
                        var sourStr = args[0]?.ToString();
                        var startIndex = int.Parse(args[1]?.ToString());
                        var strLength = int.Parse(args[2]?.ToString());
                        return sourStr?.Substring(startIndex, strLength);
                    }
                default:
                    return null;
            }
        }

        private string[] Split(string str, string separator)
        {
            return str.Split(separator[0]);
        }

        private string Replace(string source, string target, string replaceWith)
        {
            return source.Replace(target, replaceWith);
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
            Register(name, (args, _) => args?.Length != 1 || args[0] == null ? null : func(args[0]), new FuncDependency(name));
        }

        public void RegisterEnsureTwoArgs(string name, Func<object?, object?, object?> func)
        {
            Register(name, (args, _) => args.Length != 2 || (args[0] == null || args[1] == null) ? null : func(args[0], args[1]),
                new FuncDependency(name));
        }

        public void RegisterEnsureThreeArgs(string name, Func<object?, object?, object?, object?> func)
        {
            Register(name, (args, _) => args.Length != 3 || (args[0] == null || args[1] == null || args[2] == null) ? null : func(args[0], args[1], args[2]),
                new FuncDependency(name));
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
}
