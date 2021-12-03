using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VCEL.Core.Expression.Impl;
using VCEL.Expression;
using VCEL.Monad;
using P = VCEL.Core.Lang.VCELParser;

namespace VCEL.Core.Expression
{
    public class ToStringExpressionFactory : IExpressionFactory<string>
    {
        private readonly IMonad<string> monad;
        private const string Indent = "    ";

        public ToStringExpressionFactory(IMonad<string> monad)
        {
            this.monad = monad;
        }

        public IExpression<string> Property(string name)
            => new ValueExpr<string>(monad, name);

        public IExpression<string> Add(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.PLUS, l, r);

        public IExpression<string> Subtract(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.MINUS, l, r);

        public IExpression<string> Pow(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.POW, l, r);

        public IExpression<string> Divide(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.DIVIDE, l, r);

        public IExpression<string> Multiply(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.MULTIPLY, l, r);

        public IExpression<string> Between(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.BETWEEN, l, r);

        public IExpression<string> Ternary(IExpression<string> conditional, IExpression<string> trueCond,
            IExpression<string> falseCond)
            => new ToStringValueExpr<(IExpression<string> conditional, IExpression<string> trueCond, IExpression<string> falseCond)>(
                monad, (conditional, trueCond, falseCond), (value, context) =>
                    $"{conditional.Evaluate(context)} {P.TokenName(P.QUEST)} {trueCond.Evaluate(context)} {P.TokenName(P.COLON)} {falseCond.Evaluate(context)}");

        public IExpression<string> Let(IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
            => new ToStringValueExpr<(IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)>(
                monad, (bindings, expr), (value, context) =>
                {
                    var bindingsString = string.Join($",\r\n{Indent}", bindings.Select(b => $"{b.Item1} = {b.Item2.Evaluate(context)}"));
                    return $"{P.TokenName(P.LET)}\r\n{Indent}{bindingsString}\r\n{P.TokenName(P.IN)} {expr.Evaluate(context)}";
                });

        public IExpression<string> Guard(IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses,
            IExpression<string> otherwise = null)
            => new ToStringValueExpr<(IReadOnlyList<(IExpression<string>, IExpression<string>)>, IExpression<string>? otherwise)>(monad,
                (guardClauses, otherwise), (value, context) =>
                {
                    var matchClauses = string.Join("\r\n", guardClauses.Select(c =>
                        $"{P.TokenName(P.BAR)} {c.Item1.Evaluate(context)} {P.TokenName(P.ASSIGN)} {c.Item2.Evaluate(context)}"));
                    var otherwiseClause = otherwise == null
                        ? string.Empty
                        : $"{P.TokenName(P.BAR)} {P.TokenName(P.OTHERWISE)} {otherwise.Evaluate(context)}";
                    return $"{P.TokenName(P.MATCH)}\r\n{matchClauses}\r\n{otherwiseClause}";
                });

        public IExpression<string> LessThan(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.LT, l, r);

        public IExpression<string> GreaterThan(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.GT, l, r);

        public IExpression<string> LessOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.LTE, l, r);

        public IExpression<string> GreaterOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.GTE, l, r);

        public IExpression<string> In(IExpression<string> l, ISet<object> set)
            => new ToStringBinaryOp(monad, P.IN, l, Set(set));

        public IExpression<string> Matches(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, "~", l, r);

        public IExpression<string> And(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, "and", l, r);

        public IExpression<string> Or(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, "or", l, r);

        public IExpression<string> Not(IExpression<string> e)
            => new ToStringValueExpr<IExpression<string>>(monad, e,
                (value, context) => $"{P.TokenName(P.NOT)}{value.Evaluate(context)}");

        public IExpression<string> Int(int i)
            => new ToStringValueExpr<int>(monad, i, (value, _) => value.ToString(CultureInfo.InvariantCulture));

        public IExpression<string> Long(long l)
            => new ToStringValueExpr<long>(monad, l, (value, _) => value.ToString(CultureInfo.InvariantCulture));

        public IExpression<string> Double(double d)
            => new ToStringValueExpr<double>(monad, d, (value, _) => value.ToString(CultureInfo.InvariantCulture));

        public IExpression<string> String(string s)
            => new ToStringValueExpr<string>(monad, s, (value, _) => $"'{value}'");

        public IExpression<string> Bool(bool b)
            => new ToStringValueExpr<bool>(monad, b, (value, _) => P.TokenName(value ? P.TRUE : P.FALSE));

        public IExpression<string> DateTimeOffset(DateTimeOffset dateTimeOffset)
            => new ToStringValueExpr<DateTimeOffset>(monad, dateTimeOffset, (value, _) => value.ToString("@yyyy-MM-ddThh:mm:ss.fffK"));

        public IExpression<string> TimeSpan(TimeSpan timeSpan)
            => new ToStringValueExpr<TimeSpan>(monad, timeSpan,
                (value, _) => value.ToString(value.Days >= 1 ? "d'.'hh':'mm':'ss'.'fff" : "hh':'mm':'ss'.'fff"));

        public IExpression<string> Set(ISet<object> set)
            => new ToStringValueExpr<ISet<object>>(monad, set, (value, context) =>
            {
                var items = string.Join($"{P.TokenName(P.COMMA)} ", value.Select(item => item.ToString()));
                return $"{P.TokenName(P.OPEN_BRACE)} {items} {P.TokenName(P.CLOSE_BRACE)}";
            });

        public IExpression<string> Value(object? o)
            => new ToStringValueExpr<object?>(monad, o, (value, _) => value?.ToString() ?? P.TokenName(P.NULL));

        public IExpression<string> List(IReadOnlyList<IExpression<string>> l)
            => new ToStringValueExpr<IReadOnlyList<IExpression<string>>>(monad, l, (value, context) =>
            {
                var items = string.Join($"{P.TokenName(P.COMMA)} ", value.Select(item => item.Evaluate(context)));
                return $"{P.TokenName(P.OPEN_BRACE)} {items} {P.TokenName(P.CLOSE_BRACE)}";
            });

        public IExpression<string> Paren(IExpression<string> expr)
            => new ToStringValueExpr<IExpression<string>>(monad, expr, (value, context) =>
                $"{P.TokenName(P.LPAREN)}{value.Evaluate(context)}{P.TokenName(P.RPAREN)}");

        public IExpression<string> Function(string name, IReadOnlyList<IExpression<string>> args)
            => new ToStringValueExpr<(string name, IReadOnlyList<IExpression<string>> args)>(monad, (name, args),
                (value, context) =>
                {
                    var argsString = string.Join($"{P.TokenName(P.COMMA)} ", value.args.Select(arg => arg.Evaluate(context)));
                    return $"{name}{P.TokenName(P.LPAREN)}{argsString}{P.TokenName(P.RPAREN)}";
                });

        public IExpression<string> UnaryMinus(IExpression<string> expression)
            => new ToStringValueExpr<IExpression<string>>(monad, expression,
                (value, context) => $"{P.TokenName(P.MINUS)}{value.Evaluate(context)}");

        public IExpression<string> Null()
            => new ToStringValueExpr<object?>(monad, null, (value, _) => P.TokenName(P.NULL));

        public IExpression<string> Eq(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.EQ, l, r);

        public IExpression<string> NotEq(IExpression<string> l, IExpression<string> r)
            => new ToStringBinaryOp(monad, P.NEQ, l, r);

        public IExpression<string> Member(IExpression<string> obj, IExpression<string> memberExpr)
            => new ToStringValueExpr<(IExpression<string> obj, IExpression<string> memberExpr)>(monad, (obj, memberExpr),
                (value, context) => $"{value.obj.Evaluate(context)}{P.TokenName(P.DOT)}{value.memberExpr.Evaluate(context)}");
    }
}
