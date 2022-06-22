using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp
{
    public class CSharpPropertyValueAccessor : IValueAccessor<string>
    {
        private readonly IMonad<string> monad;
        private readonly string propName;
        private readonly IReadOnlyDictionary<string, Func<string>>? overridePropFunc;

        public CSharpPropertyValueAccessor(
            IMonad<string> monad,
            string propName,
            IReadOnlyDictionary<string, Func<string>>? overridePropFunc = null)
        {
            this.monad = monad;
            this.propName = propName;
            this.overridePropFunc = overridePropFunc;
        }

        public string GetValue(IContext<string> context)
        {
            if (overridePropFunc != null && overridePropFunc.TryGetValue(propName, out var func))
            {
                return monad.Lift(func());
            }

            var finalPropOrMethod = $"({context.Value}.{propName})";
            return monad.Lift($"(CSharpHelper.IsNumber({finalPropOrMethod}) " +
                            $"? ((double)({finalPropOrMethod})) " +
                            $": ({finalPropOrMethod}))");
        }
    }
}