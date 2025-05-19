using System.Runtime.Loader;
using VCEL.Core.Helper;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.CSharp.Expression.Func;
using VCEL.Expression;
using VCEL.Tool;

Console.WriteLine("VCEL TOOL");

const string expr = "A == 0 or B == 0 ? 0 : (A + B) / (A - B)";

var defaultFunctions = new DefaultCSharpFunctions();
var parser = new ExpressionParser<string>(new ToCSharpCodeFactory(ConcatCSharpMonad.Instance, defaultFunctions));

var result = parser.Parse(expr);
var expression = result.Expression;
var csharpExpr = expression.Evaluate(new CSharpObjectContext(ConcatCSharpMonad.Instance, Constants.DefaultContext));

var type = CodeGen.GenerateType("VcelTesting", CodeGen.GenerateFile("VcelTesting", csharpExpr),
    AssemblyLoadContext.Default);
// var testType = typeof(VcelTesting2);
var method = type.GetMethod("Calculate");
var func = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), null, method!);

var context = new Dictionary<string, object?>
{
    { "A", 1 },
    { "B", 7 },
};

func.Invoke(context);

Thread.Sleep(5000);
GC.Collect();
GC.WaitForPendingFinalizers();
Thread.Sleep(5000);

for (var i = 0; i < 1000000; i++)
{
    context["A"] = i;
    context["B"] = i + 1;

    var evaluation = func.Invoke(context);
}