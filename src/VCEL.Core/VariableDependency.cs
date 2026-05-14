using System.Collections.Generic;

namespace VCEL;

public sealed class VariableDependency(string variableName) : IDependency
{
    public string VariableName { get; } = variableName;
    public string Name { get; } = variableName;

    public override bool Equals(object? obj)
    {
        return obj is VariableDependency dependency &&
               VariableName == dependency.VariableName;
    }
    public override int GetHashCode()
    {
        return 539060726 + EqualityComparer<string>.Default.GetHashCode(VariableName);
    }
}