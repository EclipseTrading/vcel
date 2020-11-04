using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJSFunction : IExpression<string>
    {
        private static Dictionary<string, string> JSFunctionMap
             = new Dictionary<string, string>()
             {
                 { "abs", "Math.abs" },
                 { "round", "Math.round" },
                 { "sqrt", "Math.round" },
                 { "min", "Math.min" },
                 { "max", "Math.max" },
                 { "now", "Date.now" },
                 { "today", "new Date" },
                 { "startswith", "startsWith" },
                 { "substring", "substring" },
                 { "tolower", "toLowerCase" },
                 { "toupper", "toUpperCase" }
             };
        private string name;
        private IReadOnlyList<IExpression<string>> args;

        public ToJSFunction(IMonad<string> monad, string name, IReadOnlyList<IExpression<string>> args)
        {
            this.Monad = monad;
            this.name = name;
            this.args = args;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            if (ToJSFunction.JSFunctionMap.TryGetValue(name.ToLower(), out var jsFunc))
            {
                return string.IsNullOrEmpty(context.Value) || context.Value == "{ }"
                    ? $"({jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))"
                    : $"({context.Value}.{jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
            }
            return $"({name}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
        }
    }
}