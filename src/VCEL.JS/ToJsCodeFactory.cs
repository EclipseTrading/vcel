using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.Expression;
using VCEL.JS.Expression;
using VCEL.Monad;

namespace VCEL.JS
{
    public class ToJsCodeFactory<IContext> : ExpressionFactory<string>
    {
        public ToJsCodeFactory(
            IMonad<string> monad,
            IFunctions<string> functions = null)
            : base(monad, functions)
        {
        }

        public override IExpression<string> Ternary(IExpression<string> conditional, IExpression<string> trueCondition, IExpression<string> falseCondition)
            => new ToJsTernary(Monad, conditional, trueCondition, falseCondition);

        public override IExpression<string> Let(IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
            => new ToJsLetExpr(Monad, bindings, expr);

        public override IExpression<string> Guard(IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses, IExpression<string> otherwise = null)
            => new ToJsGuardExpr(Monad, guardClauses, otherwise);

        public override IExpression<string> In(IExpression<string> l, ISet<object> set)
            => new ToJsCodeInOp(Monad, l, Set(set));

        public override IExpression<string> Set(ISet<object> s)
            => new ToJsStringOp((context) => $"(new Set([{string.Join(",", s.Select(str => $"'{str}'"))}]))", Monad);

        public override IExpression<string> And(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("&&", Monad, l, r);

        public override IExpression<string> Or(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("||", Monad, l, r);

        public override IExpression<string> GreaterThan(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp(">", Monad, l, r);

        public override IExpression<string> GreaterOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp(">=", Monad, l, r);

        public override IExpression<string> LessThan(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("<", Monad, l, r);

        public override IExpression<string> LessOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("<=", Monad, l, r);

        public override IExpression<string> Add(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("+", Monad, l, r);

        public override IExpression<string> Subtract(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("-", Monad, l, r);

        public override IExpression<string> Divide(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("/", Monad, l, r);

        public override IExpression<string> Multiply(IExpression<string> l, IExpression<string> r)
            => new ToJsCodeBinaryOp("*", Monad, l, r);

        public override IExpression<string> Eq(IExpression<string> l, IExpression<string> r)
            => new ToJsValueOfOp("===", Monad, l, r);

        public override IExpression<string> NotEq(IExpression<string> l, IExpression<string> r)
            => new ToJsValueOfOp("!==", Monad, l, r);

        public override IExpression<string> Pow(IExpression<string> l, IExpression<string> r)
            => new ToJsPowOp(Monad, l, r);

        public override IExpression<string> Bool(bool b)
            => new ToJsStringOp((context) => b ? "true" : "false", Monad);

        public override IExpression<string> String(string s)
            => new ToJsStringOp((context) => $"'{s}'", Monad);

        public override IExpression<string> Null()
            => new ToJsStringOp((context) => "null", Monad);

        public override IExpression<string> Property(string name)
            => new ToJsPropertyOp(name, Monad);

        public override IExpression<string> UnaryMinus(IExpression<string> expression)
            => new ToJsStringOp((context) => $"-{expression.Evaluate(context)}", Monad);

        public override IExpression<string> Not(IExpression<string> e)
            => new ToJsStringOp((context) => $"!{e.Evaluate(context)}", Monad);

        public override IExpression<string> Matches(IExpression<string> l, IExpression<string> r)
            => new ToJsMatchesOp(Monad, l, r);

        public override IExpression<string> Function(string name, IReadOnlyList<IExpression<string>> args)
            => new ToJsFunction(Monad, name, args);

        public override IExpression<string> TimeSpan(TimeSpan timeSpan)
            => new ToJsTimeSpan(Monad, timeSpan);

        public override IExpression<string> DateTimeOffset(DateTimeOffset dateTimeOffset)
            => new ToJsDateTimeOffSet(Monad, dateTimeOffset);

        public override IExpression<string> Between(IExpression<string> l, IExpression<string> r)
            => new ToJsBetweenExpr(Monad, l, r);
    }
}
