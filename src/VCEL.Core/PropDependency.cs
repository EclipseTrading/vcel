using System.Collections.Generic;

namespace VCEL
{
    public class PropDependency : IDependency
    {
        public PropDependency(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is PropDependency dependency &&
                   Name == dependency.Name;
        }
        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
