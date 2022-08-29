using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Func
{
    public static class FunctionHelper
    {
        public static void RegisterEnsureArgs<TContext, TOut>(string name, 
            Func<object?[], TOut> func,
            Action<string, Func<object?[], IContext<TContext>, TOut?>, IDependency[]> register,
            int? minArgumentCount = null, 
            int? maxArgumentCount = null, 
            bool allowNullArgument = false) where TOut : class
        {
            bool IsArgumentsValid(IReadOnlyCollection<object?> args)
            {
                if (minArgumentCount != null && args.Count < minArgumentCount)
                {
                    return false;
                }

                if (maxArgumentCount != null && args.Count > maxArgumentCount)
                {
                    return false;
                }

                if (!allowNullArgument && args.Any(arg => arg == null))
                {
                    return false;
                }
                return true;
            }

            register(name, (args, _) => IsArgumentsValid(args)? func(args) : default, new IDependency[] { new FuncDependency(name) });
        }
    }
}
