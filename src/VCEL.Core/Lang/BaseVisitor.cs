using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Lang
{
    public class BaseVisitor<T> : VCELParserBaseVisitor<T>
    {
        private readonly IVisitorProvider visitorProvider;

        public BaseVisitor(IVisitorProvider visitorProvider)
        {
            this.visitorProvider = visitorProvider;
        }

        public TOut Visit<TOut>(IParseTree parseTree)
        {
            if (parseTree == null)
            {
                throw new ArgumentException("Parse Tree was null");
            }
            var visitor = visitorProvider.GetVisitor<TOut>();
            return visitor.Visit(parseTree);
        }

        public IReadOnlyList<TOut> Visit<TOut>(IReadOnlyList<IParseTree> parseTrees)
            => parseTrees.Any()
                ? (IReadOnlyList<TOut>)parseTrees
                    .Select(c => Visit<TOut>(c))
                    .ToList()
                : Array.Empty<TOut>();
    }
}
