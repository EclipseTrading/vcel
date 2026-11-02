using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using NUnit.Framework;
using VCEL.CSharp;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.Expression;

namespace VCEL.Test;

public class CSharpCodeSnapshotTests
{
    public sealed class Snapshot
    {
        public string Name { get; set; } = "";
        public string Expression { get; set; } = "";
        public string CSharp { get; set; } = "";
        public bool WithMembers { get; set; }
        public string[] Members { get; set; } = [];
    }

    public static IEnumerable<TestCaseData> Snapshots()
    {
        using var stream = typeof(CSharpCodeSnapshotTests).Assembly.GetManifestResourceStream(
            "VCEL.Test.Snapshots.CSharpCode.json")
            ?? throw new FileNotFoundException("Embedded C# code snapshots are missing.");
        var snapshots = JsonSerializer.Deserialize<Snapshot[]>(stream)
            ?? throw new InvalidDataException("C# code snapshots must contain an array.");
        if (snapshots.Length == 0)
        {
            throw new InvalidDataException("C# code snapshots must not be empty.");
        }

        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var snapshot in snapshots)
        {
            if (string.IsNullOrWhiteSpace(snapshot.Name) || !names.Add(snapshot.Name))
            {
                throw new InvalidDataException($"Missing or duplicate snapshot name: '{snapshot.Name}'.");
            }

            yield return new TestCaseData(snapshot)
                .SetName($"GeneratedCSharp_{snapshot.Name}");
        }
    }

    [TestCaseSource(nameof(Snapshots))]
    public void GeneratedCodeMatchesSnapshot(Snapshot snapshot)
    {
        var (actual, members) = Generate(snapshot.Expression, snapshot.WithMembers);
        Assert.That(actual, Is.EqualTo(snapshot.CSharp),
            $"Generated C# changed for: {snapshot.Expression}\nReview the change before editing Snapshots/CSharpCode.json.");
        Assert.That(members, Is.EqualTo(snapshot.Members));
    }

    internal static (string Code, string[] Members) Generate(string expression, bool withMembers)
    {
        if (!withMembers)
        {
            var parsed = CSharpExpression.ParseCode(expression);
            Assert.That(parsed.Success, Is.True, $"Failed to parse snapshot expression: {expression}");
            return (parsed.Expression.Evaluate(new object()), []);
        }

        var parser = new ExpressionParser<string>(new AdvanceToCSharpCodeFactory(ConcatStringMonad.Instance));
        var advanced = parser.Parse(expression);
        Assert.That(advanced.Success, Is.True, $"Failed to parse snapshot expression: {expression}");
        var dependencies = advanced.Expression.Dependencies.OfType<CSharpMemberDependency>().ToArray();
        var code = advanced.Expression.Evaluate(new CSharpObjectContext(ConcatStringMonad.Instance, Constants.DefaultContext));
        string Normalize(string value)
        {
            for (var i = 0; i < dependencies.Length; i++)
            {
                value = Regex.Replace(value, $@"\b{Regex.Escape(dependencies[i].Name)}\b", $"snapshot_set_{i}");
            }
            return value;
        }
        return (Normalize(code), dependencies.Select(member => Normalize(member.Declaration)).ToArray());
    }
}
