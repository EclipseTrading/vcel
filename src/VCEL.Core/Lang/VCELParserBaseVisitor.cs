using System.Diagnostics.CodeAnalysis;

namespace VCEL.Core.Lang;

[ExcludeFromCodeCoverage]
[SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix")]
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
public partial class VCELParserBaseVisitor<Result>;
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
