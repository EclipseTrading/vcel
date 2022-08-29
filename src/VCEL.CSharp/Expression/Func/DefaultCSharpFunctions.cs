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
            Register("abs", args => $"VcelMath.Abs({Join(args)})");
            Register("acos", args => $"VcelMath.Acos({Join(args)})");
            Register("asin", args => $"VcelMath.Asin({Join(args)})");
            Register("atan", args => $"VcelMath.Atan({Join(args)})");
            Register("atan2", args => $"VcelMath.Atan2({Join(args)})");
            Register("ceiling", args => $"VcelMath.Ceiling({Join(args)})");
            Register("cos", args => $"VcelMath.Cos({Join(args)})");
            Register("cosh", args => $"VcelMath.Cosh({Join(args)})");
            Register("exp", args => $"VcelMath.Exp({Join(args)})");
            Register("floor", args => $"VcelMath.Floor({Join(args)})");
            Register("log", args => $"VcelMath.Log({Join(args)})");
            Register("log10", args => $"VcelMath.Log10({Join(args)})");
            Register("max", args => $"VcelMath.Max({Join(args)})");
            Register("min", args => $"VcelMath.Min({Join(args)})");
            Register("pow", args => $"VcelMath.Pow({Join(args)})");
            Register("mod", args => $"VcelMath.Mod({Join(args)})");
            Register("round", args => $"VcelMath.Round({Join(args)})");
            Register("sign", args => $"VcelMath.Sign({Join(args)})");
            Register("sin", args => $"VcelMath.Sin({Join(args)})");
            Register("sinh", args => $"VcelMath.Sinh({Join(args)})");
            Register("sqrt", args => $"VcelMath.Sqrt({Join(args)})");
            Register("tan", args => $"VcelMath.Tan({Join(args)})");
            Register("tanh", args => $"VcelMath.Tanh({Join(args)})");
            Register("truncate", args => $"VcelMath.Truncate({Join(args)})");
            Register("int", args => $"VcelType.Integer({Join(args)})");
            Register("long", args => $"VcelType.Long({Join(args)})");
            Register("double", args => $"VcelType.Double({Join(args)})");
            Register("decimal", args => $"VcelType.Decimal({Join(args)})");
            Register("str", args => $"VcelType.String({Join(args)})");
            Register("string", args => $"VcelType.String({Join(args)})");
            Register("bool", args => $"VcelType.Boolean({Join(args)})");

            Register("now", args => $"DateTime.Now");
            Register("today", args => $"DateTime.Today");
            FunctionHelper.RegisterEnsureArgs<string, string>("workday",
                args => $"VcelDateTime.Workday(VcelDateTime.ParseWorkdayParams(new object[]{{{Join(args)}}}))",
                Register, 2, allowNullArgument: false);

            RegisterEnsureOneArg("lowercase", arg => $"{arg}.ToLower()");
            RegisterEnsureOneArg("uppercase", arg => $"{arg}.ToUpper()");
            RegisterEnsureOneArg("datetime", arg => $"VcelDateTime.ToDateTime({arg})");
            RegisterEnsureOneArg("date", arg => $"VcelDateTime.ToDate({arg})");

            Register("substring", Substring);
            RegisterEnsureTwoArgs("split", (arg1, arg2) => $"{arg1}.Split('{ToCSharpStringLiteralOp.UnWarpStringLiteral(arg2?.ToString() ?? "")}')");
            RegisterEnsureThreeArgs("replace", (arg1, arg2, arg3) => $"{arg1}.Replace({arg2}, {arg3})");
        }

        private static string? Substring(object?[] args)
        {
            switch (args.Length)
            {
                case 2:
                    {
                        var sourStr = args[0]?.ToString();
                        var startIndex = int.Parse(args[1]?.ToString());
                        return $"{sourStr}.Substring({startIndex})";
                    }
                case 3:
                    {
                        var sourStr = args[0]?.ToString();
                        var startIndex = int.Parse(args[1]?.ToString());
                        var strLength = int.Parse(args[2]?.ToString());
                        return $"{sourStr}.Substring({startIndex}, {strLength})";
                    }
            }

            return null;
        }

        public Function<string>? GetFunction(string name)
            => functions.TryGetValue(name, out var f) ? f : null;

        public bool HasFunction(string name)
            => functions.ContainsKey(name);

        public void Register(string name, Func<object?[], IContext<string>, string?> func)
            => this.Register(name, func, new FuncDependency(name));

        public void Register(string name, Func<object?[], string?> func)
            => this.Register(name, (args, _) => func(args), new FuncDependency(name));

        private void Register(string name, Func<object?[], IContext<string>, string?> func, params IDependency[] deps)
            => functions[name] = new Function<string>(func, deps);

        public void RegisterEnsureOneArg(string name, Func<object?, string?> func)
            => Register(name, (args, _) => args?.Length != 1 || args[0] == null ? null : func(args[0]), new FuncDependency(name));

        public void RegisterEnsureTwoArgs(string name, Func<object?, object?, string?> func)
            => Register(name, (args, _) => args.Length != 2 || (args[0] == null || args[1] == null) ? null : func(args[0], args[1]),
                new FuncDependency(name));

        public void RegisterEnsureThreeArgs(string name, Func<object?, object?, object?, string?> func)
            => Register(name, (args, _) => args.Length != 3 || (args[0] == null || args[1] == null || args[2] == null) ? null : func(args[0], args[1], args[2]),
                new FuncDependency(name));

        private static string Join(object?[] args)
            => string.Join(",", args.Select(s => (string?)s));
    }
}
