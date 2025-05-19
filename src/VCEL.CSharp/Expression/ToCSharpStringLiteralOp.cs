using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpStringLiteralOp(string str, IMonad<string> monad) : IExpression<string>
{
    private readonly string str = ToLiteral(str);
    private static readonly CodeGeneratorOptions CodeGeneratorOptions = new();
    public IMonad<string> Monad { get; } = monad;

    public IEnumerable<IDependency> Dependencies => throw new NotImplementedException();

    public string Evaluate(IContext<string> context)
    {
        return $"@{this.str}";
    }

    private static string ToLiteral(string input)
    {
        using (var writer = new StringWriter())
        {
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, CodeGeneratorOptions);
                return writer.ToString();
            }
        }
    }
}