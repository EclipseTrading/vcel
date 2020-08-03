using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Func
{
    public class DefaultFunctions : IFunctions
    {
        private readonly Dictionary<string, Func<object[], object>> functions
            = new Dictionary<string, Func<object[], object>>();

        public DefaultFunctions()
        {
            Register("abs", args =>
                args[0] == null
                    ? null
                    : Convert.ChangeType(Math.Abs(Convert.ToDouble(args[0])), args[0].GetType()));
            Register("min", Enumerable.Min);
            Register("max", Enumerable.Max);
            Register("now", _ => DateTime.Now);
            Register("today", _ => DateTime.Today);
        }

        public Func<object[], object> GetFunction(string name)
        {
            return functions.TryGetValue(name.ToLower(), out var f) ? f : null;
        }

        public void Register(string name, Func<object[], object> func)
        {
            functions[name] = func;
        }
    }

}
