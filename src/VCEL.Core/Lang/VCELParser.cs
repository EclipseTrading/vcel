using System.Diagnostics.CodeAnalysis;

namespace VCEL.Core.Lang
{
    [ExcludeFromCodeCoverage]
    public partial class VCELParser
    {
        public static string TokenName(int tokenType)
        {
            return DefaultVocabulary.GetLiteralName(tokenType).Trim('\'');
        }
    }
}
