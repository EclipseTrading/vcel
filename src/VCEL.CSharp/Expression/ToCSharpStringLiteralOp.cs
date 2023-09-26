using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using VCEL.Monad;

namespace VCEL.CSharp.Expression
{
    internal class ToCSharpStringLiteralOp : IExpression<string>
    {
        private readonly string str;
        private static readonly CodeGeneratorOptions CodeGeneratorOptions = new();
        public IMonad<string> Monad { get; }

        public ToCSharpStringLiteralOp(string str, IMonad<string> monad)
        {
            Monad = monad;
            this.str = ToLiteral(str);
        }

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

        public static string UnWarpStringLiteral(string str)
        {
            return str.TrimStart('@').Trim('\"');
        }
    }
}
