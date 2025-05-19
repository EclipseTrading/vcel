using VCEL.CSharp;

namespace VCEL.Tool;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Reflection;
using System.Runtime.Loader;

public static class CodeGen
{
    private static readonly IList<string> Usings =
    [
        // ReSharper disable RedundantNameQualifier
        Path.Combine(Path.GetDirectoryName(typeof(Action).Assembly.Location)!, "netstandard.dll"),
        typeof(object).Assembly.Location,
        typeof(System.Exception).Assembly.Location,
        typeof(System.Action).Assembly.Location,
        typeof(System.Linq.Enumerable).Assembly.Location,
        typeof(System.Collections.IEnumerable).Assembly.Location,
        typeof(System.Collections.Generic.List<>).Assembly.Location,
        typeof(System.Runtime.CompilerServices.DynamicAttribute).Assembly.Location,
        typeof(System.Text.RegularExpressions.Regex).Assembly.Location,
        typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location,
        typeof(VCEL.Core.Expression.Impl.TypeOperation).Assembly.Location,
        typeof(VCEL.CSharp.CSharpHelper).Assembly.Location,
        Assembly.Load("System.Collections").Location,
        Assembly.Load("System.Runtime").Location,
    ];

    private static readonly IReadOnlyList<MetadataReference> AssemblyReferences = Usings
        .Distinct()
        .OrderBy(location => location)
        .Select(location => AssemblyMetadata.CreateFromFile(location).GetReference())
        .ToList();

    public static Type GenerateType(string name, string src, AssemblyLoadContext? assemblyContext)
    {
        var source = SourceText.From(src);
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(source, null, $"{name}.cs");
        var compilation = CSharpCompilation.Create($"{name}.dll",
            [syntaxTree],
            AssemblyReferences,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release)
        );
        byte[] image;
        using (var ms = new MemoryStream())
        {
            var emitResult = compilation.Emit(ms);
            if (!emitResult.Success)
            {
                throw new Exception($"Unable to compile {name}:\n{src}\n{string.Join("\n", emitResult.Diagnostics)}");
            }

            image = ms.ToArray();
        }

        var assembly = assemblyContext != null
            ? assemblyContext.LoadFromStream(new MemoryStream(image))
            : Assembly.Load(image);

        var type = assembly.GetType($"VCEL.Tool.Generated.{name}");
        if (type == null)
        {
            throw new TypeLoadException($"Unable to find type {name} in assembly");
        }

        return type!;
    }

    public static string GenerateFile(string name, string csharpExpr) =>
        $"""
          using System;
          using System.Linq;
          using System.Collections.Generic;
          using System.Runtime.CompilerServices;
          using System.Text.RegularExpressions;
          using VCEL.Core.Expression.Impl;
          using VCEL.Core.Helper;
          using VCEL.CSharp;

          namespace VCEL.Tool.Generated;

          {GenerateClass(name, csharpExpr)}
          """;

     private static string GenerateClass(string name, string csharpExpr) =>
         $$"""
           public static class {{name}}
           {
               public static object? Calculate(dynamic vcelContext)
               {
                   return {{csharpExpr}};
               }
           }
           """;
    
//     private static string GenerateClass(string name, string csharpExpr) =>
//         $$"""
//           public static class {{name}}
//           {
//               public static object? Calculate(dynamic vcelContext)
//               {
//                   return {{csharpExpr}} switch {
//                      true => {{nameof(BoxedConstants)}}.{{nameof(BoxedConstants.True)}},
//                      false => {{nameof(BoxedConstants)}}.{{nameof(BoxedConstants.False)}},
//                      var __result => __result,
//                   };
//               }
//           }
//           """;
}