using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Lang;

public static class VisitorsExtensions
{
    public static T Visit<T>(this IVisitorProvider provider, IParseTree parseTree)
        => provider.GetVisitor<T>().Visit(parseTree);

    public static IReadOnlyList<TResult> Visit<TResult>(this IVisitorProvider provider, IReadOnlyList<IParseTree> parseTrees)
        => parseTrees.Select(t => provider.Visit<TResult>(t)).ToList();
}