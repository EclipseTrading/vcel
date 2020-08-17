using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.Monad;

namespace VCEL.Core.Expression.Impl
{
    public class FunctionExpr<TMonad> : IExpression<TMonad>
    {
        private readonly Function function;
        public FunctionExpr(
            IMonad<TMonad> monad,
            string name,
            IReadOnlyList<IExpression<TMonad>> args,
            Function function)
        {
            Monad = monad;
            Name = name;
            Args = args;
            this.function = function;
        }

        public IMonad<TMonad> Monad { get; }
        public string Name { get; }
        public IReadOnlyList<IExpression<TMonad>> Args { get; }
        public IEnumerable<IDependency> Dependencies 
            => function.Dependencies.Union(Args.SelectMany(a => a.Dependencies)).Distinct();

        public TMonad Evaluate(IContext<TMonad> context)
        {
            return Eval(Args.Select(arg => arg.Evaluate(context)).ToList());

            TMonad Eval(IReadOnlyList<TMonad> args)
            {
                var resolved = new object[args.Count];
                return EvalInner(0);

                TMonad EvalInner(int i)
                    => i < args.Count
                        ? Monad.Bind(
                            args[i],
                            o =>
                            {
                                resolved[i] = o;
                                return EvalInner(i + 1);
                            })
                        : Monad.Lift(function.Func(resolved.ToArray()));
            }
        }

        public override string ToString()
            => $"{Name}({string.Join(", ", Args)})";
    }
}
