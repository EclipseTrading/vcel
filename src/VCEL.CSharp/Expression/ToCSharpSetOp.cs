using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;
internal class ToCSharpSetOp : IExpression<string>
{
    private readonly CSharpMemberDependency memberDependency;
    private static int setCounter;

    public ToCSharpSetOp(IMonad<string> monad, ISet<object> set)
    {
        var memberName = $"set_{Interlocked.Increment(ref setCounter)}";

        Monad = monad;
        Set = set;
        this.memberDependency = new CSharpMemberDependency(
            memberName,
            $"private static readonly HashSet<object> {memberName} = [{string.Join(",", set.Select(x => x is string ? $@"""{x}""" : x?.ToString() ?? "\"null\""))}];");
    }

    public IMonad<string> Monad { get; }
    public ISet<object> Set { get; }

    public IEnumerable<IDependency> Dependencies => [memberDependency];

    public string Evaluate(IContext<string> context)
    {
        return memberDependency.Name;
    }
}