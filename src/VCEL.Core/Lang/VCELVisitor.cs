using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Expression;

namespace VCEL.Core.Lang
{
    public class VCELVisitor<T> : VCELParserBaseVisitor<ParseResult<T>>
    {
        private readonly IExpressionFactory<T> exprFactory;

        public VCELVisitor(IExpressionFactory<T> exprFactory)
        {
            this.exprFactory = exprFactory;
        }

        public override ParseResult<T> VisitExpression([NotNull] VCELParser.ExpressionContext context)
            => Compose(context, a => a, 0);
        public override ParseResult<T> VisitParen([NotNull] VCELParser.ParenContext context)
            => Compose(context, r => exprFactory.Paren(r), 1);
        public override ParseResult<T> VisitProperty([NotNull] VCELParser.PropertyContext context)
            => new ParseResult<T>(exprFactory.Property(context.GetText()));
        public override ParseResult<T> VisitDoubleLiteral([NotNull] VCELParser.DoubleLiteralContext context)
           => double.TryParse(context.GetText(), out var d)
                ? new ParseResult<T>(exprFactory.Double(d))
                : new ParseResult<T>(new ParseError($"Unable to parse '{d}' as double", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        public override ParseResult<T> VisitFloatLiteral([NotNull] VCELParser.FloatLiteralContext context)
           => float.TryParse(context.GetText().Substring(0, context.GetText().Length - 1), out var f)
                ? new ParseResult<T>(exprFactory.Value(f))
                : new ParseResult<T>(new ParseError($"Unable to parse '{f} as float", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        public override ParseResult<T> VisitIntegerLiteral(VCELParser.IntegerLiteralContext context)
          => int.TryParse(context.GetText(), out var i) 
                ? new ParseResult<T>(exprFactory.Int(i))
                : new ParseResult<T>(new ParseError($"Unable to parse {i} as int", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        public override ParseResult<T> VisitLongLiteral([NotNull] VCELParser.LongLiteralContext context)
            => long.TryParse(context.GetText().Substring(0, context.GetText().Length - 1), out var l) 
                ? new ParseResult<T>(exprFactory.Long(l))
                : new ParseResult<T>(new ParseError($"Unable to parse {l} as long", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        public override ParseResult<T> VisitBoolLiteral([NotNull] VCELParser.BoolLiteralContext context)
            => new ParseResult<T>(exprFactory.Bool(Equals(context.GetText(), "true")));
        public override ParseResult<T> VisitStringLiteral([NotNull] VCELParser.StringLiteralContext context)
            => new ParseResult<T>(
                exprFactory.String(context.GetText().Substring(1, context.GetText().Length - 2)));
        public override ParseResult<T> VisitDateTimeLiteral([NotNull] VCELParser.DateTimeLiteralContext context)
           => DateTimeOffset.TryParse(context.GetText().Substring(1), out var d)
                ? new ParseResult<T>(exprFactory.DateTimeOffset(d))
                : new ParseResult<T>(new ParseError($"Unable to parse {d} as DateTimeOffset", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        public override ParseResult<T> VisitTimeLiteral([NotNull] VCELParser.TimeLiteralContext context)
            => TimeSpan.TryParse(context.GetText(), out var ts) 
                ? new ParseResult<T>(exprFactory.TimeSpan(ts))
                : new ParseResult<T>(new ParseError($"Unable to parse {ts} as TimeSpan", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));

        public override ParseResult<T> VisitPlus(VCELParser.PlusContext context)
            => Compose(context, (a, b) => exprFactory.Add(a, b), 0, 2);
        public override ParseResult<T> VisitMinus([NotNull] VCELParser.MinusContext context)
            => Compose(context, (a, b) => exprFactory.Subtract(a, b), 0, 2);
        public override ParseResult<T> VisitMult(VCELParser.MultContext context)
            => Compose(context, (a, b) => exprFactory.Multiply(a, b), 0, 2);
        public override ParseResult<T> VisitDiv([NotNull] VCELParser.DivContext context)
            => Compose(context, (a, b) => exprFactory.Divide(a, b), 0, 2);
        public override ParseResult<T> VisitPow([NotNull] VCELParser.PowContext context)
            => Compose(context, (a, b) => exprFactory.Pow(a, b), 0, 2);
        public override ParseResult<T> VisitEq([NotNull] VCELParser.EqContext context)
            => Compose(context, (a, b) => exprFactory.Eq(a, b), 0, 2);
        public override ParseResult<T> VisitNeq([NotNull] VCELParser.NeqContext context)
            => Compose(context, (a, b) => exprFactory.NotEq(a, b), 0, 2);
        public override ParseResult<T> VisitLT([NotNull] VCELParser.LTContext context)
            => Compose(context, (a, b) => exprFactory.LessThan(a, b), 0, 2);
        public override ParseResult<T> VisitLTE([NotNull] VCELParser.LTEContext context)
            => Compose(context, (a, b) => exprFactory.LessOrEqual(a, b), 0, 2);
        public override ParseResult<T> VisitGT([NotNull] VCELParser.GTContext context)
            => Compose(context, (a, b) => exprFactory.GreaterThan(a, b), 0, 2);
        public override ParseResult<T> VisitGTE([NotNull] VCELParser.GTEContext context)
            => Compose(context, (a, b) => exprFactory.GreaterOrEqual(a, b), 0, 2);
        public override ParseResult<T> VisitInOp([NotNull] VCELParser.InOpContext context)
            => Compose(context, (a, b) => exprFactory.In(a, b), 0, 2);
        public override ParseResult<T> VisitMatches([NotNull] VCELParser.MatchesContext context)
            => Compose(context, (a, b) => exprFactory.Matches(a, b), 0, 2);
        public override ParseResult<T> VisitBetween([NotNull] VCELParser.BetweenContext context)
            => Compose(context, (a, b) => exprFactory.Between(a, b), 0, 2);
        public override ParseResult<T> VisitBetweenArgs([NotNull] VCELParser.BetweenArgsContext context)
            => Compose(context, (a, b) => exprFactory.List(new[] { a,  b }), 1, 3);
        public override ParseResult<T> VisitAnd([NotNull] VCELParser.AndContext context)
            => Compose(context, (a, b) => exprFactory.And(a, b), 0, 2);
        public override ParseResult<T> VisitOr([NotNull] VCELParser.OrContext context)
           => Compose(context, (a, b) => exprFactory.Or(a, b), 0, 2);
        public override ParseResult<T> VisitListExpr(VCELParser.ListExprContext context)
           => Compose(
               context, 
               exprs => exprFactory.List(exprs), 
               Enumerable
                    .Range(0, (context.ChildCount - 1) / 2)
                    .Select(i => i * 2 + 1)
                    .ToArray());
        public override ParseResult<T> VisitTernary(VCELParser.TernaryContext context)
            => Compose(context, (a, b, c) => exprFactory.Ternary(a, b, c), 0, 2, 4);

        public override ParseResult<T> VisitLetexpr([NotNull] VCELParser.LetexprContext context)
        {
            var bindings = Enumerable
                .Range(0, (context.ChildCount - 2) / 2)
                .Select(i => i * 2 + 1)
                .Select(i => GetBinding(context.GetChild(i)))
                .ToList();

            var exp = Visit(context.GetChild(context.ChildCount - 1));

            if(bindings.Any(b => !b.Item2.Success) || !exp.Success)
            {
                return new ParseResult<T>(
                    bindings
                        .SelectMany(b => b.Item2.ParseErrors)
                        .Union(exp.ParseErrors)
                        .ToList());
            }

            return new ParseResult<T>(
                exprFactory.Let(
                    bindings.Select(b => (b.Item1, b.Item2.Expression)).ToList(), 
                    exp.Expression));

            (string, ParseResult<T>) GetBinding(IParseTree parseTree)
            {
                var id = parseTree.GetChild(0).GetText();
                var expr = Visit(parseTree.GetChild(2));
                return (id, expr);
            }
        }

        public override ParseResult<T> VisitUnaryMinus([NotNull] VCELParser.UnaryMinusContext context)
            => Compose(context, r => exprFactory.UnaryMinus(r), 1);

        public override ParseResult<T> VisitGuardExpr([NotNull] VCELParser.GuardExprContext context)
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

            if (clauses.Any(c => !c.Item1.Success || !c.Item2.Success)
                || (hasOtherwise && !otherwise.Success))
            {
                return new ParseResult<T>(
                    clauses
                        .SelectMany(
                            c => c.Item1.ParseErrors.Union(c.Item2.ParseErrors))
                        .Union(hasOtherwise ? otherwise.ParseErrors : new ParseError[0])
                        .ToList());
            }
            var clauseExprs = clauses.Select(c => (c.Item1.Expression, c.Item2.Expression)).ToList();
            var otherwiseExpr = hasOtherwise ? otherwise.Expression : null;

            return new ParseResult<T>(exprFactory.Guard(clauseExprs, otherwiseExpr));
        }

        public override ParseResult<T> VisitFunctionExpr([NotNull] VCELParser.FunctionExprContext context)
            => Compose(
                context,
                e => exprFactory.Function(context.GetChild(0).GetText(), e),
                context.ChildCount == 3
                    ? new int[0]
                    : Enumerable.Range(0, (context.ChildCount - 2) / 2).Select(i => i * 2 + 2).ToArray());

        public override ParseResult<T> VisitLegacyNode([NotNull] VCELParser.LegacyNodeContext context)
            => new ParseResult<T>(exprFactory.This());

        public override ParseResult<T> VisitNullLiteral([NotNull] VCELParser.NullLiteralContext context)
            => new ParseResult<T>(exprFactory.Null());

        public override ParseResult<T> VisitNot([NotNull] VCELParser.NotContext context)
            => Compose(context, r => exprFactory.Not(r), 1);
               
        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>> f, int node)
            => Compose(context, r => f(r[0]), node);


        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>, IExpression<T>, IExpression<T>> f, int a, int b, int c)
            => Compose(context, r => f(r[0], r[1], r[2]), a, b, c);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>, IExpression<T>> f, int a, int b)
            => Compose(context, r => f(r[0], r[1]), a, b);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IReadOnlyList<IExpression<T>>, IExpression<T>> f, params int[] nodes)
        {
            if (context.exception != null)
            {
                var token = context.exception.OffendingToken;
                return new ParseResult<T>(new[] { new ParseError(context.exception.Message, token.Text, token.Line, token.StartIndex, token.StopIndex) });
            }

            var results = nodes.Select(n => Visit(context.GetChild(n))).ToList();
            if (results.Any(r => !r.Success))
            {
                return new ParseResult<T>(results.SelectMany(r => r.ParseErrors).ToList());
            }

            return new ParseResult<T>(f(results.Select(r => r.Expression).ToList()));
        }
    }

}
