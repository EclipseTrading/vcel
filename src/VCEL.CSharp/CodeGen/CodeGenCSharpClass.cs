using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CSharp.RuntimeBinder;
using VCEL.Core.Expression.Impl;

namespace VCEL.CSharp.CodeGen;

public static class CodeGenCSharpClass
{
    public static (Type?, EmitResult?) Generate(string name, string csharpExpr)
    {
        var src = GenerateFile(name, csharpExpr);
        return GenerateType(name, src);
    }

    private static string GenerateFile(string name, string csharpExpr)
    {
        return $$"""

                                 using System.Collections.Generic;
                                 using System.Runtime.CompilerServices;
                                 using System.Linq;
                                 using System;
                                 using System.Text.RegularExpressions;
                                 using VCEL.Core.Expression.Impl;
                                 using VCEL.Core.Helper;
                                 using VCEL.CSharp;

                                 namespace VCEL.CSharp.CodeGen
                                 {
                                     {{GenerateClass(name, csharpExpr)}}
                                 }
                 """;
    }

    private static string GenerateClass(string name, string csharpExpr)
    {
        return $$"""

                                     public class {{name}}
                                     {
                                         public static object Evaluate(dynamic vcelContext)
                                         {
                                             return {{csharpExpr}};
                                             //return {{csharpExpr}} switch {
                                             //   true => {{nameof(BoxedConstants)}}.{{nameof(BoxedConstants.True)}},
                                             //   false => {{nameof(BoxedConstants)}}.{{nameof(BoxedConstants.False)}},
                                             //   var __result => __result,
                                             //};
                                         }
                                     }
                 """;
    }

    private static (Type?, EmitResult?) GenerateType(string? name, string src)
    {
        var frameworkPath = Path.GetDirectoryName(typeof(Action).Assembly.Location);
        Debug.Assert(frameworkPath != null, nameof(frameworkPath) + " != null");

        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "netstandard.dll")),
            MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "System.Collections.dll")),
            MetadataReference.CreateFromFile(Path.Combine(frameworkPath, "System.Linq.dll")),
            MetadataReference.CreateFromFile(typeof(Action).Assembly.Location), // System.Core.dll
            MetadataReference.CreateFromFile(typeof(Uri).Assembly.Location), // System.dll
            MetadataReference.CreateFromFile(typeof(DynamicAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(CSharpArgumentInfo).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(TypeOperation).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Regex).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(CSharpHelper).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location)
        };

        var source = SourceText.From(src);
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(source, null, $"{name}.cs");
        var compilation = CSharpCompilation.Create($"{name}.dll",
            [syntaxTree],
            refs,
            new CSharpCompilationOptions(
                    outputKind: OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release)
                .WithSpecificDiagnosticOptions(new Dictionary<string, ReportDiagnostic>
                {
                    { "CS0183", ReportDiagnostic.Suppress },
                    { "CS0184", ReportDiagnostic.Suppress },
                })
        );
        byte[]? image = null;
        using (var ms = new MemoryStream())
        {
            var emitResult = compilation.Emit(ms);
            if (!emitResult.Success)
            {
                return (null, emitResult);
            }

            image = ms.ToArray();
        }

        var assembly = Assembly.Load(image);
        var type = assembly.GetType($"VCEL.CSharp.CodeGen.{name}");
        return (type, null);
    }
}