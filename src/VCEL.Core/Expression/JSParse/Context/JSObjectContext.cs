using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse.Context
{
    public class JSObjectContext : ObjectContext<string>
    {
        private readonly IReadOnlyDictionary<string, Func<string>> overridePropertyFunc;

        public JSObjectContext(IMonad<string> monad, object obj, IReadOnlyDictionary<string, Func<string>> overridePropertyFunc = null)
            : base(monad, obj)
        {
            this.overridePropertyFunc = overridePropertyFunc;
        }

        public override bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
        {
            accessor = new JSPropertyValueAccessor(Monad, propName, overridePropertyFunc);
            return true;
        }

        public override bool TryGetContext(object o, out IContext<string> context)
        {
            context = new JSObjectContext(Monad, o, overridePropertyFunc);
            return true;
        }
    }
}
