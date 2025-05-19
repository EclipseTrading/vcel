using System.Text.RegularExpressions;
using VCEL.Core.Expression.Impl;
using VCEL.Monad;

namespace VCEL.CSharp.Expression;

internal sealed class ToCSharpBinaryOp(
    string opName,
    IMonad<string> monad,
    IExpression<string> left,
    IExpression<string> right)
    : BinaryExprBase<string>(monad, left, right)
{
    // private static int atomicNumber;

    public override string Evaluate(object? lv, object? rv)
    {
        return $"({lv} {opName} {rv})";
        // var lvStrName = $"ls{atomicNumber++}";
        // var rvStrName = $"rs{atomicNumber++}";
        // // var lvVarName = $"lv{atomicNumber++}";
        // // var rvVarName = $"rv{atomicNumber++}";
        // // if (opName == "+")
        // // {
        // //     // if strings start with ({cast here})value (regex match)
        // //     // var valueCast = Regex.Match(opName, @"\((?<cast>[^)]+)\)(?<value>.+)");
        // //     // if (valueCast.Success)
        // //     // {
        // //     //     var cast = valueCast.Groups["cast"].Value;
        // //     //     var value = valueCast.Groups["value"].Value;
        // //     //     return $"({cast}){lv} + ({cast}){rv}";
        // //     // }
        // //
        // //     return lv is "null" || rv is "null"
        // //         ? $"({lv} + {rv})" // Handle literal nulls gracefully
        // //         // : $"({lv} is {{ }} {lvVarName} && {lvVarName} is string {lvStrName} ? {lvStrName} + {rv} : {rv} is {{ }} {rvVarName} && {rvVarName} is string {rvStrName} ? {lv} + {rv} : ((double?)(object){lv} + (double?)(object){rv}))",
        // //         : $"({lv} is string {lvStrName} ? {lvStrName} + {rv} : {rv} is string {rvStrName} ? {lv} + {rv} : ((double?)(object){lv} + (double?)(object){rv}))";
        // // }
        //
        // if (opName == "+")
        // {
        //     return $"({lv} {opName} {rv})";
        // }
        //
        // return $"((double?){lv} {opName} (double?){rv})";
    }

    // public override string Evaluate(object? lv, object? rv) => opName switch
    // {
    //     "+" => lv is "null" || rv is "null"
    //         ? $"({lv} + {rv})" // Handle literal nulls gracefully
    //         :
    //     _ => $"((double?){lv} {opName} (double?){rv})",
    // };
}