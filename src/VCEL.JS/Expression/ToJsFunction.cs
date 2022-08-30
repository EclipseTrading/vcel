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
                 { "int", "Number" },
                 { "long", "Number" },
                 { "double", "Number" },
                 { "decimal", "Number" },
                 { "string", "String" },

                 { "now", "new Date" },
                 { "today", "new Date" },
                 { "datetime", "new Date" },
                 { "date", "new Date" }
             };

        private static Dictionary<string, string> JSMethodMap
            = new Dictionary<string, string>()
            {
                { "startswith", "startsWith" },
                { "substring", "substring" },
                { "replace", "replace" },
                { "lowercase", "toLowerCase" },
                { "uppercase", "toUpperCase" }
            };

        private static Dictionary<string, string> JSFunctionDefaultResultMap
            = new Dictionary<string, string>()
             {
                { "startsWith", "false" },
                { "substring", "''" },
                { "toLowerCase", "''" },
                { "toUpperCase", "''" },
                { "replace", "''" }
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
            if (JSMethodMap.TryGetValue(name, out var jsMethod))
            {
                if (IsContextEmpty(context) && args.Count == 0) return GetDefaultVal(jsMethod);
                return IsContextEmpty(context)
                    ? WarpVariableForNullChecking(args[0].Evaluate(context), jsMethod, string.Join(",", args.Skip(1).Select(s => s.Evaluate(context))))
                    : WarpVariableForNullChecking(context.Value, jsMethod, string.Join(",", args.Select(s => s.Evaluate(context))));
            }
            if (JSFunctionMap.TryGetValue(name, out var jsFunc))
            {
                return IsContextEmpty(context)
                    ? $"({jsFunc}({string.Join(",", args.Select(s => s.Evaluate(context)))}))"
                    : WarpVariableForNullChecking(context.Value, jsFunc, string.Join(",", args.Select(s => s.Evaluate(context))));
            }
            return $"({name}({string.Join(",", args.Select(s => s.Evaluate(context)))}))";
        }

        private bool IsContextEmpty(IContext<string> context)
        {
            return string.IsNullOrEmpty(context.Value) || context.Value == "{ }";
        }

        private string WarpVariableForNullChecking(string variable, string func, string args)
        {
            var defaultReturnVal = GetDefaultVal(func);
            return $"({variable} ? {variable}.{func}({args}) : {defaultReturnVal})";
        }

        private string GetDefaultVal(string func)
        {
            return JSFunctionDefaultResultMap.TryGetValue(func, out var defaultVal)
                ? defaultVal
                : "false";
        }
    }
}