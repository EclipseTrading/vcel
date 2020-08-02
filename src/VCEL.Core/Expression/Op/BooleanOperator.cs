using System;

namespace VCEL.Core.Expression.Op
{
    public class BooleanOperator : Operator
    {
        private Func<bool, bool, bool> combine;

        public BooleanOperator(string opChar, Func<bool, bool, bool> combine)
        {
            OpChar = opChar;
            this.combine = combine;
        }

        public string OpChar { get; }

        public bool Evaluate(bool l, bool r)
        {
            return combine(l, r);
        }

        public void SetFunc(Func<bool, bool, bool> combine)
        {
            this.combine = combine;
        }

        public override string ToString() => OpChar;
    }
}
