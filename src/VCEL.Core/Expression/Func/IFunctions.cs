﻿namespace VCEL.Core.Expression.Func
{
    public interface IFunctions
    {
        Function GetFunction(string name);
        bool HasFunction(string name);
    }
}
