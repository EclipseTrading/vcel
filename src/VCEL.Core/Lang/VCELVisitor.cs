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
        {
            return Compose(context,
                a => a[0],
                Enumerable.Range(0, context.ChildCount - 1).ToArray());
        }

        public override ParseResult<T> VisitErrorNode([NotNull] IErrorNode node)
            => new ParseResult<T>(new ParseError("Error node matched", node.GetText(), node.Symbol.Line, node.Symbol.StartIndex, node.Symbol.StopIndex));

        public override ParseResult<T> VisitExpr([NotNull] VCELParser.ExprContext context)
            => CheckAndVisitChildren(context);

        public override ParseResult<T> VisitParen([NotNull] VCELParser.ParenContext context)
            => Compose(context, r => exprFactory.Paren(r), 1);

        public override ParseResult<T> VisitProperty([NotNull] VCELParser.PropertyContext context)
            => new ParseResult<T>(exprFactory.Property(context.GetText()));

        public override ParseResult<T> VisitLiteral([NotNull] VCELParser.LiteralContext context)
        {
            var parsedValue = ParseLiteral(context);
            return parsedValue.Success
                ? new ParseResult<T>(parsedValue.Parsed)
                : new ParseResult<T>(parsedValue.ParseErrors);
        }

        /// <summary>
        /// Parses the base literal types, we do not use the Visitor methods to allow reuse of this logic to create higher level literals such as setLiteral
        /// </summary>
        private ValueParseResult<T> ParseLiteral(VCELParser.LiteralContext context)
        {
            var doubleLiteral = context.doubleLiteral();
            if (doubleLiteral != null)
            {
                return double.TryParse(context.GetText(), out var d)
                    ? new ValueParseResult<T>(d, exprFactory.Double(d))
                    : new ValueParseResult<T>(new ParseError($"Unable to parse '{d}' as double", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
            }

            var floatLiteral = context.floatLiteral();
            if (floatLiteral != null)
            {
                return float.TryParse(context.GetText().Substring(0, context.GetText().Length - 1), out var f)
                    ? new ValueParseResult<T>(f, exprFactory.Value(f))
                    : new ValueParseResult<T>(new ParseError($"Unable to parse '{f} as float", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
            }

            var integerLiteral = context.integerLiteral();
            if (integerLiteral != null)
            {
                return int.TryParse(context.GetText(), out var i)
                    ? new ValueParseResult<T>(i, exprFactory.Int(i))
                    : new ValueParseResult<T>(new ParseError($"Unable to parse {i} as int", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
            }

            var longLiteral = context.longLiteral();
            if (longLiteral != null)
            {
                return long.TryParse(context.GetText().Substring(0, context.GetText().Length - 1), out var l)
                    ? new ValueParseResult<T>(l, exprFactory.Long(l))
                    : new ValueParseResult<T>(new ParseError($"Unable to parse {l} as long", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
            }

            var boolLiteral = context.boolLiteral();
            if (boolLiteral != null)
            {
                bool b = Equals(context.GetText(), "true");
                return new ValueParseResult<T>(b, exprFactory.Bool(b));
            }

            var stringLiteral = context.stringLiteral();
            if (stringLiteral != null)
            {
                string s = context.GetText().Substring(1, context.GetText().Length - 2);
                return new ValueParseResult<T>(s, exprFactory.String(s));
            }

            var dateLiteral = context.dateLiteral();
            if (dateLiteral != null)
            {

                var dateTimeLiteral = dateLiteral.dateTimeLiteral();
                if (dateTimeLiteral != null)
                {
                    return DateTimeOffset.TryParse(dateLiteral.GetText().Substring(1), out var d)
                        ? new ValueParseResult<T>(d, exprFactory.DateTimeOffset(d))
                        : new ValueParseResult<T>(new ParseError($"Unable to parse {d} as DateTimeOffset", dateLiteral.GetText(), dateLiteral.Start.Line, dateLiteral.Start.StartIndex, dateLiteral.Stop.StopIndex));
                }

                var timeLiteral = dateLiteral.timeLiteral();
                if (timeLiteral != null)
                {
                    return TimeSpan.TryParse(dateLiteral.GetText(), out var ts)
                        ? new ValueParseResult<T>(ts, exprFactory.TimeSpan(ts))
                        : new ValueParseResult<T>(new ParseError($"Unable to parse {ts} as TimeSpan", dateLiteral.GetText(), dateLiteral.Start.Line, dateLiteral.Start.StartIndex, dateLiteral.Stop.StopIndex));
                }
            }

            var nullLiteral = context.nullLiteral();
            if (nullLiteral != null)
            {
                return new ValueParseResult<T>(null, exprFactory.Null());
            }

            return new ValueParseResult<T>(new ParseError($"Unable to parse, unhandled literal type", context.GetText(), context.Start.Line, context.Start.StartIndex, context.Stop.StopIndex));
        }

        public override ParseResult<T> VisitPlusMinus([NotNull] VCELParser.PlusMinusContext context)
            => Compose(context, (a, b) => context.PLUS() != null ? exprFactory.Add(a, b) : exprFactory.Subtract(a, b), 0, 2);
        public override ParseResult<T> VisitMultDiv([NotNull] VCELParser.MultDivContext context)
            => Compose(context, (a, b) => context.MULTIPLY() != null ? exprFactory.Multiply(a, b) : exprFactory.Divide(a, b), 0, 2);
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
            => ComposeWithChildren(context, (inChildNodes) =>
            {
                var left = Visit(inChildNodes[0]);

                if (!left.Success)
                {
                    return (null, new ParseResult<T>(left.ParseErrors));
                }

                var setNode = inChildNodes[1];
                var setChildNodes = Enumerable.Range(0, setNode.ChildCount - 1).Select(n => setNode.GetChild(n));
                var errors = inChildNodes.OfType<ParserRuleContext>().Where(c => c.exception != null);
                if (errors.Any())
                {
                    return (null, new ParseResult<T>(
                        errors
                            .Select(e => (ex: e.exception, t: e.exception.OffendingToken))
                            .Select(e => new ParseError(e.ex.Message, e.t.Text, e.t.Line, e.t.StartIndex, e.t.StopIndex))
                            .ToList()));
                }

                var setItems = setChildNodes
                    .OfType<VCELParser.LiteralContext>()
                    .Select(setItem => ParseLiteral(setItem));

                if (setItems.Any(item => !item.Success))
                {
                    return (null, new ParseResult<T>(setItems.SelectMany(r => r.ParseErrors).ToList()));
                }

                var set = new HashSet<object>(setItems.Select(item => item.Value));

                return (exprFactory.In(left.Expression, set), null);
            }, 0, 2);

        public override ParseResult<T> VisitMatches([NotNull] VCELParser.MatchesContext context)
            => Compose(context, (a, b) => exprFactory.Matches(a, b), 0, 2);
        public override ParseResult<T> VisitBetween([NotNull] VCELParser.BetweenContext context)
            => Compose(context, (a, b) => exprFactory.Between(a, b), 0, 2);
        public override ParseResult<T> VisitBetweenArgs([NotNull] VCELParser.BetweenArgsContext context)
            => Compose(context, (a, b) => exprFactory.List(new[] { a, b }), 1, 3);
        public override ParseResult<T> VisitAnd([NotNull] VCELParser.AndContext context)
            => Compose(context, (a, b) => exprFactory.And(a, b), 0, 2);
        public override ParseResult<T> VisitOr([NotNull] VCELParser.OrContext context)
           => Compose(context, (a, b) => exprFactory.Or(a, b), 0, 2);

        public override ParseResult<T> VisitTernary([NotNull] VCELParser.TernaryContext context)
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
        public override ParseResult<T> VisitMemberExpr([NotNull] VCELParser.MemberExprContext context)
            => Compose(context, (a, b) => exprFactory.Member(a, b), 0, 2);

        public override ParseResult<T> VisitLegacyNode([NotNull] VCELParser.LegacyNodeContext context)
            => new ParseResult<T>(exprFactory.LegacyType(context.GetText().Substring(2, context.GetText().Length-3)));

        public override ParseResult<T> VisitNot([NotNull] VCELParser.NotContext context)
            => Compose(context, r => exprFactory.Not(r), 1);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>> f, int node)
            => Compose(context, r => f(r[0]), node);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>, IExpression<T>, IExpression<T>> f, int a, int b, int c)
            => Compose(context, r => f(r[0], r[1], r[2]), a, b, c);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IExpression<T>, IExpression<T>, IExpression<T>> f, int a, int b)
            => Compose(context, r => f(r[0], r[1]), a, b);

        private ParseResult<T> Compose(ParserRuleContext context, Func<IReadOnlyList<IExpression<T>>, IExpression<T>> f, params int[] nodes)
            => ComposeWithChildren(context, (childNodes) =>
            {
                var results = childNodes.Select(n => Visit(n)).ToList();
                if (results.Any(r => !r.Success))
                {
                    return (null, new ParseResult<T>(results.SelectMany(r => r.ParseErrors).ToList()));
                }

                return (f(results.Select(r => r.Expression).ToList()), null);
            }, nodes);

        private ParseResult<T> ComposeWithChildren(ParserRuleContext context, Func<IReadOnlyList<IParseTree>, (IExpression<T>, ParseResult<T>)> f, params int[] nodes)
        {
            if (context.exception != null)
            {
                var token = context.exception.OffendingToken;
                return new ParseResult<T>(new[] { new ParseError(context.exception.Message, token.Text, token.Line, token.StartIndex, token.StopIndex) });
            }

            var childNodes = nodes.Select(n => context.GetChild(n)).ToList();
            var errors = childNodes.OfType<ParserRuleContext>().Where(c => c.exception != null);
            if (errors.Any())
            {
                return new ParseResult<T>(
                    errors
                        .Select(e => (ex: e.exception, t: e.exception.OffendingToken))
                        .Select(e => new ParseError(e.ex.Message, e.t.Text, e.t.Line, e.t.StartIndex, e.t.StopIndex))
                        .ToList());
            }

            try
            {
                var processed = f(childNodes);
                if (processed.Item2 != null) return processed.Item2;
                var re = processed.Item1;
                return new ParseResult<T>(re);
            }
            catch (Exception ex)
            {
                var t = context.Start;
                return new ParseResult<T>(
                    new ParseError(ex.Message, t.Text, t.Line, t.StartIndex, t.StopIndex));
            }
        }

        private ParseResult<T> CheckAndVisitChildren(ParserRuleContext context)
        {
            if (context.exception != null)
            {
                var e = context.exception;
                var t = context.exception.OffendingToken;
                return new ParseResult<T>(new ParseError(e.Message, t.Text, t.Line, t.StartIndex, t.StopIndex));
            }

            return VisitChildren(context);
        }
    }
}
