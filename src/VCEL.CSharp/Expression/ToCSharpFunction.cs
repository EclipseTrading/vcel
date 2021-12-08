using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.CSharp.Expression.Func;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpFunction : IExpression<string>
    {
        private readonly string name;
        private readonly IReadOnlyList<IExpression<string>> args;
        private readonly IFunctions<string> functions;

        public ToCSharpFunction(
            IMonad<string> monad,
            string name,
            IReadOnlyList<IExpression<string>> args,
            IFunctions<string> functions)
        {
            this.Monad = monad;
            this.name = name;
            this.args = args;
            this.functions = functions ?? new DefaultCSharpFunctions();
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            if (functions.HasFunction(name))
            {
                var function = functions.GetFunction(name);
                return function.Func.Invoke(args?.Select(s => (object)s.Evaluate(context))?.ToArray(), context).ToString();
            }

            // TODO should return missing function
            return "";
        }
    }
}