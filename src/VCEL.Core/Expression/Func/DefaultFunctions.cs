using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Func
{
    public class DefaultFunctions : IFunctions
    {
        private readonly Dictionary<string, Function> functions
            = new Dictionary<string, Function>();

        public DefaultFunctions()
        {
            Register(
                "abs", 
                args => args[0] == null
                    ? null
                    : Convert.ChangeType(Math.Abs(Convert.ToDouble(args[0])), args[0].GetType()));
            Register("min", Enumerable.Min);
            Register("max", Enumerable.Max);
            Register("now", _ => DateTime.Now, TemporalDependency.Now);
            Register("today", _ => DateTime.Today, TemporalDependency.Today);
        }

        public Function GetFunction(string name)
        {
            return functions.TryGetValue(name.ToLower(), out var f) ? f : null;
        }

        public void Register(string name, Func<object[], object> func, params IDependency[] deps)
        {
            functions[name] = new Function(func, deps);
        }
    }

}
