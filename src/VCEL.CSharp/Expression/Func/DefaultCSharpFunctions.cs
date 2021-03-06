﻿using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;

namespace VCEL.CSharp.Expression.Func
{
    public class DefaultCSharpFunctions : IFunctions<string>
    {
        private readonly Dictionary<string, Function<string>> functions = new Dictionary<string, Function<string>>();

        public DefaultCSharpFunctions()
        {
            Register("abs", args => $"VcelMath.Abs({Join(args)})");
            Register("acos", args => $"VcelMath.Acos({Join(args)})");
            Register("asin", args => $"VcelMath.Asin({Join(args)})");
            Register("atan", args => $"VcelMath.Atan({Join(args)})");
            Register("atan2", args => $"VcelMath.Atan2({Join(args)})");
            Register("ceiling", args => $"VcelMath.Ceiling({Join(args)})");
            Register("cos", args => $"VcelMath.Cos({Join(args)})");
            Register("cosh", args => $"VcelMath.Cosh({Join(args)})");
            Register("exp", args => $"VcelMath.Exp({Join(args)})");
            Register("floor", args => $"VcelMath.Floor({Join(args)})");
            Register("log", args => $"VcelMath.Log({Join(args)})");
            Register("log10", args => $"VcelMath.Log10({Join(args)})");
            Register("max", args => $"VcelMath.Max({Join(args)})");
            Register("min", args => $"VcelMath.Min({Join(args)})");
            Register("pow", args => $"VcelMath.Pow({Join(args)})");
            Register("round", args => $"VcelMath.Round({Join(args)})");
            Register("sign", args => $"VcelMath.Sign({Join(args)})");
            Register("sin", args => $"VcelMath.Sin({Join(args)})");
            Register("sinh", args => $"VcelMath.Sinh({Join(args)})");
            Register("sqrt", args => $"VcelMath.Sqrt({Join(args)})");
            Register("tan", args => $"VcelMath.Tan({Join(args)})");
            Register("tanh", args => $"VcelMath.Tanh({Join(args)})");
            Register("truncate", args => $"VcelMath.Truncate({Join(args)})");

            Register("now", args => $"DateTime.Now");
            Register("today", args => $"DateTime.Today");
        }

        public Function<string> GetFunction(string name)
            => functions.TryGetValue(name.ToLower(), out var f) ? f : null;

        public bool HasFunction(string name)
            => functions.ContainsKey(name.ToLower());

        public void Register(string name, Func<object[], IContext<string>, string> func)
            => this.Register(name, func, new FuncDependency(name));

        public void Register(string name, Func<object[], string> func)
            => this.Register(name, (args, _) => func(args), new FuncDependency(name));

        private void Register(string name, Func<object[], IContext<string>, string> func, params IDependency[] deps)
            => functions[name.ToLower()] = new Function<string>(func, deps);

        private static string Join(object[] args)
            => string.Join(",", args.Select(s => (string) s));
    }
}
