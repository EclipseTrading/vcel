using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using VCEL.CSharp;

namespace VCEL.Test;

public class CSharpCodeSnapshotTests
{
    public sealed class Snapshot
    {
        public string Name { get; set; } = "";
        public string Expression { get; set; } = "";
        public string CSharp { get; set; } = "";
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

            yield return new TestCaseData(snapshot.Expression, snapshot.CSharp)
                .SetName($"GeneratedCSharp_{snapshot.Name}");
        }
    }

    [TestCaseSource(nameof(Snapshots))]
    public void GeneratedCodeMatchesSnapshot(string expression, string expected)
    {
        var parsed = CSharpExpression.ParseCode(expression);
        Assert.That(parsed.Success, Is.True, $"Failed to parse snapshot expression: {expression}");
        var actual = parsed.Expression.Evaluate(new object());
        Assert.That(actual, Is.EqualTo(expected),
            $"Generated C# changed for: {expression}\nReview the change before editing Snapshots/CSharpCode.json.");
    }
}
