﻿using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.JS.Expression
{
    internal class ToJsCodeInOp : BinaryExprBase<string>
    {
        private IMonad<string> monad;
        private IExpression<string> l;
        private object r;

        public ToJsCodeInOp(
            IMonad<string> monad,
            IExpression<string> l,
            IExpression<string> r)
            : base(monad, l, r)
        {
            this.monad = monad;
            this.l = l;
            this.r = r;
        }

        public override string Evaluate(object lv, object rv)
        {
            return $"{rv}.has({lv})";
        }
    }
}