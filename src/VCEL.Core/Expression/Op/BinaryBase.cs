using System;

namespace VCEL.Core.Expression.Op
{
    public class BinaryBase
    {
        private readonly Func<object, object, object> evaluate;

        public BinaryBase(Func<object, object, object> evaluate)
        {
            this.evaluate = evaluate;
        }

        public object Evaluate(object l, object r) => evaluate(l, r);
    }
}
