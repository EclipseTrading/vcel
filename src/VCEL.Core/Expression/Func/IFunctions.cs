using System;

namespace VCEL.Core.Expression.Func
{
    public interface IFunctions
    {
        Func<object[], object> GetFunction(string name);
    }
}
