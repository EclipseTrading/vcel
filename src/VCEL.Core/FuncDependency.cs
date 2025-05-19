namespace VCEL;

public class FuncDependency : IDependency
{
    public FuncDependency(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override bool Equals(object? obj)
    {
        return obj is FuncDependency f && f.Name == this.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode() * 13;
    }
}