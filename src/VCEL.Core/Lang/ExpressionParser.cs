using Antlr4.Runtime;
using System.Linq;
using VCEL.Expression;

namespace VCEL.Core.Lang
{
    public class ExpressionParser<TMonad> : IExpressionParser<TMonad>
    {
        private readonly IExpressionFactory<TMonad> expressionFactory;

        public ExpressionParser(IExpressionFactory<TMonad> expressionFactory)
        {
            this.expressionFactory = expressionFactory;
        }

        public IExpression<TMonad> Parse(string expression)
        {
            var inputStream = new AntlrInputStream(expression);
            var lexer = new VCELLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);

            var parser = new VCELParser(commonTokenStream);
            var expr = parser.expression();
            var visitor = new VCELVisitor<TMonad>(expressionFactory);
            return visitor.Visit(expr);
        }

        public string GetTokenTypes(CommonTokenStream commonTokenStream)
        {
            commonTokenStream.Fill();
            var tokens = commonTokenStream.GetTokens();
            commonTokenStream.Reset();

            return string.Join(" ", tokens.Select(t => $"[{VCELLexer.DefaultVocabulary.GetSymbolicName(t.Type)}:{t.Text}]"));
        }
    }
}
