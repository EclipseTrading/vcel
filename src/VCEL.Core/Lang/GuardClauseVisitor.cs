using System.Linq;
using Antlr4.Runtime.Misc;

namespace VCEL.Core.Lang
{
    public class GuardClauseVisitor<T> : BaseVisitor<Result<(IExpression<T> match, IExpression<T> assign)>>
    {
        public GuardClauseVisitor(IVisitorProvider visitorProvider) : base(visitorProvider) { }

        public override Result<(IExpression<T> match, IExpression<T> assign)> VisitGuardClause([NotNull] VCELParser.GuardClauseContext context)
        {
            var match = Visit<ParseResult<T>>(context.test);
            var assign = Visit<ParseResult<T>>(context.assign);
            if(!match.Success || !assign.Success) 
            {
                return new Result<(IExpression<T>, IExpression<T>)>(
                    match.ParseErrors.Union(assign.ParseErrors).ToList());
            }

            return new Result<(IExpression<T> match, IExpression<T> assign)>((match.Expression, assign.Expression));
        }
    }
}
