using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    internal class ToJsFunction : IExpression<string>
    {
        private static Dictionary<string, string> JSFunctionMap
             = new Dictionary<string, string>()
             {
                 { "abs", "Math.abs" },
                 { "round", "Math.round" },
                 { "sqrt", "Math.sqrt" },
                 { "min", "Math.min" },
                 { "max", "Math.max" },
                 { "now", "new Date" },
                 { "today", "new Date" },
                 { "startswith", "startsWith" },
                 { "substring", "substring" },
                 { "replace", "replace" },
                 { "tolower", "toLowerCase" },
                 { "toupper", "toUpperCase" }
             };
        private string name;
        private IReadOnlyList<IExpression<string>> args;

        public ToJsFunction(IMonad<string> monad, string name, IReadOnlyList<IExpression<string>> args)
        {
            this.Monad = monad;
            this.name = name;
            this.args = args;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            if (ToJsFunction.JSFunctionMap.TryGetValue(name.ToLower(), out var jsFunc))
            {
                return string.IsNullOrEmpty(context.Value) || context.Value == "{ }"
                    ? $"({jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))"
                    : $"({context.Value}.{jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
            }
            return $"({name}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
        }
    }
}