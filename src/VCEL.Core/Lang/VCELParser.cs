using System.Diagnostics.CodeAnalysis;

namespace VCEL.Core.Lang;

[ExcludeFromCodeCoverage]
[SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case")]
public partial class VCELParser
{
    public static string TokenName(int tokenType)
    {
        return DefaultVocabulary.GetLiteralName(tokenType).Trim('\'');
    }
}