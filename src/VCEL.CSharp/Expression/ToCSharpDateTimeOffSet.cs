using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpDateTimeOffSet : IExpression<string>
    {
        private readonly DateTimeOffset dateTimeOffset;

        public ToCSharpDateTimeOffSet(IMonad<string> monad, DateTimeOffset dateTimeOffset)
        {
            this.Monad = monad;
            this.dateTimeOffset = dateTimeOffset;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            return $@"(DateTimeOffset.Parse(""{dateTimeOffset:yyyy-MM-dd HH:mm:ss.fffzzz}""))";
        }
    }
}