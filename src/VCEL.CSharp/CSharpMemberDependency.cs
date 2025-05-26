namespace VCEL.CSharp;
public record CSharpMemberDependency : IDependency
{
    public CSharpMemberDependency(string memberName, string declaration)
    {
        Name = memberName;
        Declaration = declaration;
    }

    public string Name { get; }
    public string Declaration { get; }
}
