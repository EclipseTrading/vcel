using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using VCEL.Expression;

namespace VCEL.Core.Lang
{
    public class VCELVisitor<T> : VCELParserBaseVisitor<IExpression<T>>
    {
        private readonly IExpressionFactory<T> exprFactory;

        public VCELVisitor(IExpressionFactory<T> exprFactory)
        {
            this.exprFactory = exprFactory;
        }

        public override IExpression<T> VisitExpression([NotNull] VCELParser.ExpressionContext context)
            => Visit(context.GetChild(0));
        public override IExpression<T> VisitParen([NotNull] VCELParser.ParenContext context)
            => exprFactory.Paren(base.Visit(context.GetChild(1)));
        public override IExpression<T> VisitProperty([NotNull] VCELParser.PropertyContext context)
            => exprFactory.Property(context.GetText());
        public override IExpression<T> VisitDoubleLiteral([NotNull] VCELParser.DoubleLiteralContext context)
           => exprFactory.Double(double.Parse(context.GetText()));
        public override IExpression<T> VisitFloatLiteral([NotNull] VCELParser.FloatLiteralContext context)
            => exprFactory.Value(float.Parse(context.GetText().Substring(0, context.GetText().Length - 1)));
        public override IExpression<T> VisitIntegerLiteral(VCELParser.IntegerLiteralContext context)
          => exprFactory.Int(int.Parse(context.GetText()));
        public override IExpression<T> VisitLongLiteral([NotNull] VCELParser.LongLiteralContext context)
            => exprFactory.Long(long.Parse(context.GetText().Substring(0, context.GetText().Length - 1)));
        public override IExpression<T> VisitBoolLiteral([NotNull] VCELParser.BoolLiteralContext context)
            => exprFactory.Bool(Equals(context.GetText(), "true"));
        public override IExpression<T> VisitStringLiteral([NotNull] VCELParser.StringLiteralContext context)
            => exprFactory.String(context.GetText().Substring(1, context.GetText().Length - 2));
        public override IExpression<T> VisitDateTimeLiteral([NotNull] VCELParser.DateTimeLiteralContext context)
           => exprFactory.DateTimeOffset(DateTimeOffset.Parse(context.GetText().Substring(1)));
        public override IExpression<T> VisitTimeLiteral([NotNull] VCELParser.TimeLiteralContext context)
            => exprFactory.TimeSpan(TimeSpan.Parse(context.GetText()));

        public override IExpression<T> VisitPlus(VCELParser.PlusContext context)
            => exprFactory.Add(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitMinus([NotNull] VCELParser.MinusContext context)
            => exprFactory.Subtract(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitMult(VCELParser.MultContext context)
            => exprFactory.Multiply(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitDiv([NotNull] VCELParser.DivContext context)
            => exprFactory.Divide(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitPow([NotNull] VCELParser.PowContext context)
            => exprFactory.Pow(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitEq([NotNull] VCELParser.EqContext context)
            => exprFactory.Eq(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitNeq([NotNull] VCELParser.NeqContext context)
            => exprFactory.NotEq(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitLT([NotNull] VCELParser.LTContext context)
            => exprFactory.LessThan(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitLTE([NotNull] VCELParser.LTEContext context)
            => exprFactory.LessOrEqual(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitGT([NotNull] VCELParser.GTContext context)
            => exprFactory.GreaterThan(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitGTE([NotNull] VCELParser.GTEContext context)
            => exprFactory.GreaterOrEqual(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitIn([NotNull] VCELParser.InContext context)
            => exprFactory.In(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitMatches([NotNull] VCELParser.MatchesContext context)
            => exprFactory.Matches(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));

        public override IExpression<T> VisitBetween([NotNull] VCELParser.BetweenContext context)
            => exprFactory.Between(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitBetweenArgs([NotNull] VCELParser.BetweenArgsContext context)
            => exprFactory.List(new[] {
                Visit(context.GetChild(1)),
                Visit(context.GetChild(3)) 
            });
        public override IExpression<T> VisitAnd([NotNull] VCELParser.AndContext context)
            => exprFactory.And(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitOr([NotNull] VCELParser.OrContext context)
           => exprFactory.Or(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)));
        public override IExpression<T> VisitListExpr(VCELParser.ListExprContext context)
            => exprFactory.List(
                Enumerable
                    .Range(0, (context.ChildCount - 1) / 2)
                    .Select(i => Visit(context.GetChild(i * 2 + 1)))
                    .ToList());
        public override IExpression<T> VisitTernary(VCELParser.TernaryContext context)
            => exprFactory.Ternary(
                Visit(context.GetChild(0)),
                Visit(context.GetChild(2)),
                Visit(context.GetChild(4)));

        public override IExpression<T> VisitLetexpr([NotNull] VCELParser.LetexprContext context)
        {
            var bindings = Enumerable
                .Range(0, (context.ChildCount - 2) / 2)
                .Select(i => i * 2 + 1)
                .Select(i => GetBinding(context.GetChild(i)))
                .ToList();

            var exp = Visit(context.GetChild(context.ChildCount - 1));
            return exprFactory.Let(bindings, exp);

            (string, IExpression<T>) GetBinding(IParseTree parseTree)
            {
                var id = parseTree.GetChild(0).GetText();
                var expr = Visit(parseTree.GetChild(2));
                return (id, expr);
            }
        }

        public override IExpression<T> VisitUnaryMinus([NotNull] VCELParser.UnaryMinusContext context)
            => exprFactory.UnaryMinus(Visit(context.GetChild(1)));

        public override IExpression<T> VisitGuardExpr([NotNull] VCELParser.GuardExprContext context)
        {
            var hasOtherwise = context.ChildCount % 4 == 0;
            var clauseCount = (context.ChildCount - (hasOtherwise ? 4 : 1)) / 4;
            var clauses = Enumerable
                    .Range(0, clauseCount)
                    .Select(i => (c: i * 4 + 2, v: i * 4 + 4))
                    .Select(i => (
                        Visit(context.GetChild(i.c)),
                        Visit(context.GetChild(i.v))))
                    .ToList();
            var otherwise = hasOtherwise ? Visit(context.GetChild(context.ChildCount - 1)) : null;
            return exprFactory.Guard(clauses, otherwise);
        }

        public override IExpression<T> VisitFunction([NotNull] VCELParser.FunctionContext context)
            => exprFactory.Function(
                context.GetChild(0).GetText(),
                context.ChildCount == 3
                    ? new List<IExpression<T>>()
                    : Enumerable
                        .Range(0, (context.ChildCount - 2) / 2)
                        .Select(i => Visit(context.GetChild(i * 2 + 2)))
                        .ToList());

        public override IExpression<T> VisitLegacyMath([NotNull] VCELParser.LegacyMathContext context)
            => exprFactory.LegacyFunction(Visit(context.GetChild(2)));

        public override IExpression<T> VisitLegacyDateTime([NotNull] VCELParser.LegacyDateTimeContext context)
            => exprFactory.LegacyProperty(context.GetChild(2).GetText());
        public override IExpression<T> VisitNullLiteral([NotNull] VCELParser.NullLiteralContext context)
            => exprFactory.Null();

        public override IExpression<T> VisitNot([NotNull] VCELParser.NotContext context)
            => exprFactory.Not(Visit(context.GetChild(1)));
    }

}
