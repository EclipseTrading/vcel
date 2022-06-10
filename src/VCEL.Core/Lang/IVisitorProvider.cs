namespace VCEL.Core.Lang
{
    public interface IVisitorProvider
    {
        VCELParserBaseVisitor<T> GetVisitor<T>();
    }
}
