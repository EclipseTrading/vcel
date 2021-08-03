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
            Register("abs", args => Verify(args) ? $"VcelMath.Abs({Join(args)})" : "null");
            Register("acos", args => Verify(args) ? $"VcelMath.Acos({Join(args)})" : "null");
            Register("asin", args => Verify(args) ? $"VcelMath.Asin({Join(args)})" : "null");
            Register("atan", args => Verify(args) ? $"VcelMath.Atan({Join(args)})" : "null");
            Register("atan2", args => Verify(args) ? $"VcelMath.Atan2({Join(args)})" : "null");
            Register("ceiling", args => Verify(args) ? $"VcelMath.Ceiling({Join(args)})" : "null");
            Register("cos", args => Verify(args) ? $"VcelMath.Cos({Join(args)})" : "null");
            Register("cosh", args => Verify(args) ? $"VcelMath.Cosh({Join(args)})" : "null");
            Register("exp", args => Verify(args) ? $"VcelMath.Exp({Join(args)})" : "null");
            Register("floor", args => Verify(args) ? $"VcelMath.Floor({Join(args)})" : "null");
            Register("log", args => Verify(args) ? $"VcelMath.Log({Join(args)})" : "null");
            Register("log10", args => Verify(args) ? $"VcelMath.Log10({Join(args)})" : "null");
            Register("max", args => Verify(args) ? $"VcelMath.Max({Join(args)})" : "null");
            Register("min", args => Verify(args) ? $"VcelMath.Min({Join(args)})" : "null");
            Register("pow", args => Verify(args) ? $"VcelMath.Pow({Join(args)})" : "null");
            Register("round", args => Verify(args) ? $"VcelMath.Round({Join(args)})" : "null");
            Register("sign", args => Verify(args) ? $"VcelMath.Sign({Join(args)})" : "null");
            Register("sin", args => Verify(args) ? $"VcelMath.Sin({Join(args)})" : "null");
            Register("sinh", args => Verify(args) ? $"VcelMath.Sinh({Join(args)})" : "null");
            Register("sqrt", args => Verify(args) ? $"VcelMath.Sqrt({Join(args)})" : "null");
            Register("tan", args => Verify(args) ? $"VcelMath.Tan({Join(args)})" : "null");
            Register("tanh", args => Verify(args) ? $"VcelMath.Tanh({Join(args)})" : "null");
            Register("truncate", args => Verify(args) ? $"VcelMath.Truncate({Join(args)})" : "null");

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
            => string.Join(",", args.Select(x => (string)x).Where(x => x!="null"));

        private static bool Verify(object[] args)
            => args.Select(x => (string) x).Count(x => x != "null") > 0;
    }
}
