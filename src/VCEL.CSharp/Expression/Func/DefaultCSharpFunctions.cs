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
            Register("abs", args => $"(Math.Abs({Join(args)}))");
            Register("round", args => $"(Math.Round({Join(args)}))");
            Register("sqrt", args => $"(Math.Sqrt({Join(args)}))");
            Register("min", args => $"(Enumerable.Min(new List<object>{{{Join(args)}}}))");
            Register("max", args => $"(Enumerable.Max(new List<object>{{{Join(args)}}}))");
            Register("startswith", args => $"(StartsWith({Join(args)}))");
            Register("substring", args => $"(Substring({Join(args)}))");
            Register("replace", args => $"(Replace({Join(args)}))");
            Register("tolower", args => $"(ToLower({Join(args)}))");
            Register("toupper", args => $"(ToUpper({Join(args)}))");

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
