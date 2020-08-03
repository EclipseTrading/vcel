using Antlr4.Runtime;
using System.Linq;

namespace VCEL.Core.Lang
{
    public static class AntlrEx
    {
        public static string GetTokenTypes(this CommonTokenStream commonTokenStream)
        {
            commonTokenStream.Fill();
            var tokens = commonTokenStream.GetTokens();
            commonTokenStream.Reset();

            return string.Join(" ", tokens.Select(t => $"[{VCELLexer.DefaultVocabulary.GetSymbolicName(t.Type)}:{t.Text}]"));
        }
    }
}
