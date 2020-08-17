using System;
using System.Collections.Generic;

namespace VCEL.Core.Expression.Func
{
    public class Function
    {
        public Function(Func<object[], object> func, params IDependency[] dependencies)
        {
            this.Func = func;
            this.Dependencies = dependencies;
        }

        public Func<object[], object> Func { get; }
        public IEnumerable<IDependency> Dependencies { get; }
    }
}
