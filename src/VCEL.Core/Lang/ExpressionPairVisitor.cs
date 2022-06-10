using System.Linq;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace VCEL.Core.Lang
{
    public class ExpressionPairVisitor<T> : BaseVisitor<Result<(IExpression<T> match, IExpression<T> assign)>>
    {
        public ExpressionPairVisitor(IVisitorProvider visitorProvider) : base(visitorProvider) { }

        public override Result<(IExpression<T> match, IExpression<T> assign)> VisitGuardClause([NotNull] VCELParser.GuardClauseContext context)
        {
            var match = Visit<ParseResult<T>>(context.test);
            var assign = Visit<ParseResult<T>>(context.assign);
            return Compose(match, assign);
        }

        public override Result<(IExpression<T>, IExpression<T>)> VisitBetweenArgs([NotNull] VCELParser.BetweenArgsContext context)
        {
            var lower = Visit<ParseResult<T>>(context.lower);
            var upper = Visit<ParseResult<T>>(context.upper);
            return Compose(lower, upper);
        }

        private Result<(IExpression<T>, IExpression<T>)> Compose(ParseResult<T> item1, ParseResult<T> item2)
        {
            if (!item1.Success || !item2.Success)
            {
                return new Result<(IExpression<T>, IExpression<T>)>(
                    item1.ParseErrors.Union(item2.ParseErrors).ToList());
            }

            return new Result<(IExpression<T> match, IExpression<T> assign)>((item1.Expression, item2.Expression));
        }
    }
}
