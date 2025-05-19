using System.Reflection;
using VCEL.Core.Lang;

namespace VCEL.Cli;

public class Program
{
    private static void Main(string[] args)
    {
        var vcelVersion = Assembly.GetAssembly(typeof(VCExpression))?.GetName().Version?.ToString() ?? "Unknown";
        var repl = new VcelRepl(vcelVersion, new());

        repl.Run();
    }
}