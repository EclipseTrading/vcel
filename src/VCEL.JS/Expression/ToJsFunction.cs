using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsFunction : IExpression<string>
    {
        private static Dictionary<string, string> JSFunctionMap
             = new Dictionary<string, string>()
             {
                 { "abs" , "Math.abs" },
                 { "acos" , "Math.acos" },
                 { "asin" , "Math.asin" },
                 { "atan" , "Math.atan" },
                 { "atan2" , "Math.atan2" },
                 { "ceiling" , "Math.ceil" },
                 { "cos" , "Math.cos" },
                 { "cosh" , "Math.cosh" },
                 { "exp" , "Math.exp" },
                 { "floor" , "Math.floor" },
                 { "log" , "Math.log" },
                 { "log10" , "Math.log10" },
                 { "max" , "Math.max" },
                 { "min" , "Math.min" },
                 { "pow" , "Math.pow" },
                 { "round" , "Math.round" },
                 { "sign" , "Math.sign" },
                 { "sin" , "Math.sin" },
                 { "sinh" , "Math.sinh" },
                 { "sqrt" , "Math.sqrt" },
                 { "tan" , "Math.tan" },
                 { "tanh" , "Math.tanh" },
                 { "truncate" , "Math.trunc" },

                 { "now", "new Date" },
                 { "today", "new Date" },

                 // methods
                 { "startswith", "startsWith" },
                 { "substring", "substring" },
                 { "replace", "replace" },
                 { "tolower", "toLowerCase" },
                 { "toupper", "toUpperCase" }
             };

        private static Dictionary<string, string> JSFunctionDefaultResultMap
            = new Dictionary<string, string>()
             {
                { "startsWith", "false" },
                { "substring", "''" },
                { "toLowerCase", "''" },
                { "toUpperCase", "''" }
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
            if (JSFunctionMap.TryGetValue(name, out var jsFunc))
            {
                return string.IsNullOrEmpty(context.Value) || context.Value == "{ }"
                    ? $"({jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))"
                    : WarpVariableForNullChecking(context.Value, jsFunc, string.Join(",", args.Select(s => s.Evaluate(context))));
            }
            return $"({name}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
        }

        private string WarpVariableForNullChecking(string variable, string func, string args)
        {
            var defaultReturnVal = JSFunctionDefaultResultMap.TryGetValue(func, out var defaultVal)
                ? defaultVal
                : "false";

            return $"({variable} ? {variable}.{func}({args}) : {defaultReturnVal})";
        }
    }
}