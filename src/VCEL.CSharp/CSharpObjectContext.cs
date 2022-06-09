using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp
{
    public class CSharpObjectContext : ObjectContext<string>
    {
        private readonly IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc;

        public CSharpObjectContext(IMonad<string> monad, object obj, IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc = null)
            : base(monad, obj)
        {
            this.overridePropertyFunc = overridePropertyFunc;
        }

        public override bool TryGetAccessor(string propName, out IValueAccessor<string> accessor)
        {
            accessor = new CSharpPropertyValueAccessor(Monad, propName, overridePropertyFunc);
            return true;
        }

        public override bool TryGetContext(object o, out IContext<string> context)
        {
            context = new CSharpObjectContext(Monad, o, overridePropertyFunc);
            return true;
        }
    }
}
