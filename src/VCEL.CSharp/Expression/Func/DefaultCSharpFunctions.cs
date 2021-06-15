using System;
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
            Register("abs", args => $"Math.Abs({Join(args)})");
            Register("acos", args => $"Math.Acos({Join(args)})");
            Register("asin", args => $"Math.Asin({Join(args)})");
            Register("atan", args => $"Math.Atan({Join(args)})");
            Register("atan2", args => $"Math.Atan2({Join(args)})");
            Register("ceiling", args => $"Math.Ceiling({Join(args)})");
            Register("cos", args => $"Math.Cos({Join(args)})");
            Register("cosh", args => $"Math.Cosh({Join(args)})");
            Register("exp", args => $"Math.Exp({Join(args)})");
            Register("floor", args => $"Math.Floor({Join(args)})");
            Register("log", args => $"Math.Log({Join(args)})");
            Register("log10", args => $"Math.Log10({Join(args)})");
            Register("max", args => $"Enumerable.Max(new List<object>{{{Join(args)}}})");
            Register("min", args => $"Enumerable.Min(new List<object>{{{Join(args)}}})");
            Register("pow", args => $"Math.Pow({Join(args)})");
            Register("round", args => $"Math.Round({Join(args)})");
            Register("sign", args => $"Math.Sign({Join(args)})");
            Register("sin", args => $"Math.Sin({Join(args)})");
            Register("sinh", args => $"Math.Sinh({Join(args)})");
            Register("sqrt", args => $"Math.Sqrt({Join(args)})");
            Register("tan", args => $"Math.Tan({Join(args)})");
            Register("tanh", args => $"Math.Tanh({Join(args)})");
            Register("truncate", args => $"Math.Truncate({Join(args)})");

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
