using System.Collections.Generic;
using VCEL.Expression;

namespace VCEL.Core.Lang
{
    public class VCELVisitors<T> : IVisitorProvider
    {
        private readonly IDictionary<System.Type, object> providers = new Dictionary<System.Type, object>();

        public VCELVisitors()
        {
        }

        public VCELVisitors(IExpressionFactory<T> expressionFactory) 
        {
            providers[typeof(ParseResult<T>)] = new VCELVisitor<T>(expressionFactory, this);
            providers[typeof(Result<(IExpression<T>, IExpression<T>)>)] = new GuardClauseVisitor<T>(this);
        }        

        public VCELParserBaseVisitor<TType> GetVisitor<TType>()
                => providers.TryGetValue(typeof(TType), out var visitor)
                    && visitor is VCELParserBaseVisitor<TType> tVisitor
                    ? tVisitor
                    : throw new System.NotSupportedException($"No visitor for type {typeof(T)}");
    }
}
