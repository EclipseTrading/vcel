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
            RegisterEnsureOneArg("abs", arg => Math.Abs(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("acos", arg => Math.Acos(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("asin", arg => Math.Asin(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("atan", arg => Math.Atan(Convert.ToDouble(arg)));
            RegisterEnsureTwoArgs("atan2", (arg1, arg2) => Math.Atan2(Convert.ToDouble(arg1), Convert.ToDouble(arg2)));
            RegisterEnsureOneArg("ceiling", arg => Math.Ceiling(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("cos", arg => Math.Cos(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("cosh", arg => Math.Cosh(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("exp", arg => Math.Exp(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("floor", arg => Math.Floor(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("log", arg => Math.Log(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("log10", arg => Math.Log10(Convert.ToDouble(arg)));
            Register("max", Enumerable.Max);
            Register("min", Enumerable.Min);
            RegisterEnsureTwoArgs("pow", (arg1, arg2) => Math.Pow(Convert.ToDouble(arg1), Convert.ToDouble(arg2)));
            Register(
                "round",
                args =>
                {
                    if (args.Length == 1 && args[0] != null)
                    {
                        return Math.Round(Convert.ToDouble(args[0]));
                    }
                    else if (args.Length == 2 && args[0] != null && args[1] != null)
                    {
                        return Math.Round(Convert.ToDouble(args[0]), Convert.ToInt32(args[1]));
                    }

                    return (object)null;
                });
            RegisterEnsureOneArg("sign", arg => Math.Sign(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("sin", arg => Math.Sin(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("sinh", arg => Math.Sinh(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("sqrt", arg => Math.Sqrt(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("tan", arg => Math.Tan(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("tanh", arg => Math.Tanh(Convert.ToDouble(arg)));
            RegisterEnsureOneArg("truncate", arg => Math.Truncate(Convert.ToDouble(arg)));

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

        public void RegisterEnsureOneArg(string name, Func<object, object> func)
            => this.Register(name, (args, _) => args.Length == 1 && args[0] == null ? null : func(args[0]), new FuncDependency(name));

        public void RegisterEnsureTwoArgs(string name, Func<object, object, object> func)
            => this.Register(name, (args, _) => args.Length == 2 && args[0] == null || args[1] == null ? null : func(args[0], args[1]),
                new FuncDependency(name));

        protected void Register(string name, Func<object[], IContext<T>, object> func, params IDependency[] deps)
            => functions[name.ToLower()] = new Function<T>(func, deps);

        protected void Register(string name, Func<object[], object> func, params IDependency[] deps)
            => functions[name.ToLower()] = new Function<T>((args, _) => func(args), deps);
    }
}
