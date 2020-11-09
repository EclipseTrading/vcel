using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Func
{
    public class DefaultFunctions<T> : IFunctions<T>
    {
        private readonly Dictionary<string, Function<T>> functions
            = new Dictionary<string, Function<T>>();

        public DefaultFunctions()
        {
            Register(
                "abs",
                args => args[0] == null
                    ? null
                    : Convert.ChangeType(Math.Abs(Convert.ToDouble(args[0])), args[0].GetType()));
            Register(
                "round",
                args => args.Length == 1 && args[0] != null
                    ? Math.Round(Convert.ToDouble(args[0]))
                    : (object)null);
            Register(
                "sqrt",
                args => args.Length == 1 && args[0] != null
                    ? Math.Sqrt(Convert.ToDouble(args[0]))
                    : (object)null);
            Register("min", Enumerable.Min);
            Register("max", Enumerable.Max);
            Register("now", _ => DateTime.Now, TemporalDependency.Now);
            Register("today", _ => DateTime.Today, TemporalDependency.Today);
        }

        public Function<T> GetFunction(string name)
            => functions.TryGetValue(name.ToLower(), out var f) ? f : null;

        public bool HasFunction(string name) => functions.ContainsKey(name.ToLower());

        public void Register(string name, Func<object[], IContext<T>, object> func)
            => this.Register(name, func, new FuncDependency(name));

        public void Register(string name, Func<object[], object> func)
            => this.Register(name, (args, _) => func(args), new FuncDependency(name));

        protected void Register(string name, Func<object[], IContext<T>, object> func, params IDependency[] deps)
            => functions[name.ToLower()] = new Function<T>(func, deps);

        protected void Register(string name, Func<object[], object> func, params IDependency[] deps)
            => functions[name.ToLower()] = new Function<T>((args, _) => func(args), deps);
    }

}
