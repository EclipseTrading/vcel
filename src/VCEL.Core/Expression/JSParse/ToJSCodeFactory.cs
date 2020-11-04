using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.Core.Expression.JSParse
{
    public class ToJSCodeFactory<IContext> : ExpressionFactory<string>
    {
        public ToJSCodeFactory(
            IMonad<string> monad,
            IFunctions functions = null)
            : base(monad, functions)
        {
        }

        public override IExpression<string> Ternary(IExpression<string> conditional, IExpression<string> trueConditon, IExpression<string> falseCondition)
            => new ToJSTernary(Monad, conditional, trueConditon, falseCondition);

        public override IExpression<string> Let(IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
            => new ToJSLetExpr(Monad, bindings, expr);

        public override IExpression<string> Guard(IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses, IExpression<string> otherwise = null)
            => new ToJSGuardExpr(Monad, guardClauses, otherwise);

        public override IExpression<string> In(IExpression<string> l, ISet<object> set)
            => new ToJSCodeInOp(Monad, l, Set(set));

        public override IExpression<string> Set(ISet<object> s)
            => new ToJSStringOp((context) => $"[{string.Join(",", s.Select(str => $"'{str}'"))}]", Monad);

        public override IExpression<string> And(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("&&", Monad, l, r);

        public override IExpression<string> Or(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("||", Monad, l, r);

        public override IExpression<string> GreaterThan(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp(">", Monad, l, r);

        public override IExpression<string> GreaterOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp(">=", Monad, l, r);

        public override IExpression<string> LessThan(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("<", Monad, l, r);

        public override IExpression<string> LessOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("<=", Monad, l, r);

        public override IExpression<string> Add(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("+", Monad, l, r);

        public override IExpression<string> Subtract(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("-", Monad, l, r);

        public override IExpression<string> Divide(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("/", Monad, l, r);

        public override IExpression<string> Multiply(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("*", Monad, l, r);

        public override IExpression<string> Eq(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("==", Monad, l, r);

        public override IExpression<string> NotEq(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("!=", Monad, l, r);

        public override IExpression<string> Pow(IExpression<string> l, IExpression<string> r)
            => new ToJSCodeBinaryOp("^", Monad, l, r);

        public override IExpression<string> Bool(bool b)
            => new ToJSStringOp((context) => b ? "true" : "false", Monad);

        public override IExpression<string> String(string s)
            => new ToJSStringOp((context) => $"'{s}'", Monad);

        public override IExpression<string> Null()
            => new ToJSStringOp((context) => "null", Monad);

        public override IExpression<string> Property(string name)
            => new ToJSPropertyOp(name, Monad);

        public override IExpression<string> UnaryMinus(IExpression<string> expression)
            => new ToJSStringOp((context) => $"-{expression.Evaluate(context)}", Monad);

        public override IExpression<string> Not(IExpression<string> e)
            => new ToJSStringOp((context) => $"!{e.Evaluate(context)}", Monad);

        public override IExpression<string> Matches(IExpression<string> l, IExpression<string> r)
            => new ToJSMatchesOp(Monad, l, r);

        public override IExpression<string> Function(string name, IReadOnlyList<IExpression<string>> args)
            => new ToJSFunction(Monad, name, args);

        public override IExpression<string> TimeSpan(TimeSpan timeSpan)
            => new ToJSTimeSpan(Monad, timeSpan);

        public override IExpression<string> DateTimeOffset(DateTimeOffset dateTimeOffset)
            => new ToJSDateTimeOffSet(Monad, dateTimeOffset);

        public override IExpression<string> LegacyType(string typeName)
        {
            switch (typeName)
            {
                case "DateTime":
                case "System.DateTime":
                    return Value("(new Date())");
                case "System.Math":
                case "Math":
                    return new ThisExpr<string>(Monad);
            }
            return Null();
        }
    }
}
