using System;
using System.Collections.Generic;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsValueOfOp : IExpression<string>
    {
        private readonly string opName;
        private readonly IExpression<string> left;
        private readonly IExpression<string> right;

        public ToJsValueOfOp(string opName, IMonad<string> monad, IExpression<string> left, IExpression<string> right)
        {
            this.opName = opName;
            Monad = monad;
            this.left = left;
            this.right = right;
        }

        public IMonad<string> Monad { get; }

        public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

        public string Evaluate(IContext<string> context)
        {
            var lv = left.Evaluate(context);
            var rv = right.Evaluate(context);

            return left is ToJsPropertyOp 
                ? $"(({lv} === null || {lv} === void 0 ? void 0 : {lv}.valueOf()) {opName} {rv})" 
                : $"({lv} {opName} {rv})";
        }
    }
}