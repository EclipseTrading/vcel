using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsTimeSpan : IExpression<string>
    {
        private TimeSpan timeSpan;

        public ToJsTimeSpan(IMonad<string> monad, TimeSpan timeSpan)
        {
            this.Monad = monad;
            this.timeSpan = timeSpan;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            var totalMs = timeSpan.TotalMilliseconds;
            return $"{totalMs}";
        }
    }
}