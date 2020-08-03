using Antlr4.Runtime;
using VCEL.Expression;

namespace VCEL.Core.Lang
{
    public class ExpressionParser<T> : IExpressionParser<T>
    {
        private readonly IExpressionFactory<T> expressionFactory;

        public ExpressionParser(IExpressionFactory<T> expressionFactory)
        {
            this.expressionFactory = expressionFactory;
        }

        public ParseResult<T> Parse(string expression)
        {
            var inputStream = new AntlrInputStream(expression);
            var lexer = new VCELLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);

            var parser = new VCELParser(commonTokenStream);
            var expr = parser.expression();
            var visitor = new VCELVisitor<T>(expressionFactory);
            return visitor.Visit(expr);
        }
    }
}
