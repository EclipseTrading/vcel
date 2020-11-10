using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsTernary : IExpression<string>
    {
        private IExpression<string> conditional;
        private IExpression<string> trueConditon;
        private IExpression<string> falseCondition;

        public ToJsTernary(IMonad<string> monad, IExpression<string> conditional, IExpression<string> trueConditon, IExpression<string> falseCondition)
        {
            this.Monad = monad;
            this.conditional = conditional;
            this.trueConditon = trueConditon;
            this.falseCondition = falseCondition;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new System.NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            return $"({conditional.Evaluate(context)} ? {trueConditon.Evaluate(context)} : {falseCondition.Evaluate(context)})";
        }
    }
}