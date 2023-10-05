using System;
using System.Collections.Generic;
using System.Linq;

namespace VCEL.Core.Expression.Impl;

public static class VcelIndexable
{
    public static bool WithinBounds(int index, int length) => index >= 0 && index < length;

    public static int? Length(object? arg) => arg switch
    {
        string str => str.Length,
        object?[] array => array.Length,
        List<object?> list => list.Count,
        _ => null,
    };

    public static bool Contains(object? arg1, object? arg2) => arg1 switch
    {
        string str => str.Contains(arg2?.ToString() ?? string.Empty),
        object?[] array => array.Contains(arg2),
        List<object?> list => list.Contains(arg2),
        _ => false,
    };

    public static bool StartsWith(object? arg1, object? arg2) => arg1 switch
    {
        string str => str.StartsWith(arg2?.ToString() ?? string.Empty),
        object?[] array => array.FirstOrDefault()?.Equals(arg2) ?? false,
        List<object?> list => list.FirstOrDefault()?.Equals(arg2) ?? false,
        _ => false,
    };

    public static bool EndsWith(object? arg1, object? arg2) => arg1 switch
    {
        string str => arg2?.ToString() is { } value && str.EndsWith(value),
        object?[] array => array.LastOrDefault()?.Equals(arg2) ?? false,
        List<object?> list => list.LastOrDefault()?.Equals(arg2) ?? false,
        _ => false,
    };

    public static int? IndexOf(object? arg1, object? arg2) => arg1 switch
    {
        string str =>
            str.IndexOf(arg2?.ToString() ?? string.Empty, StringComparison.InvariantCulture) is var index and >= 0 ? index : null,
        object?[] array =>
            Array.IndexOf(array, arg2) is var index and >= 0 ? index : null,
        List<object?> list =>
            list.IndexOf(arg2) is var index and >= 0 ? index : null,
        _ => null,
    };

    public static int? LastIndexOf(object? arg1, object? arg2) => arg1 switch
    {
        string str =>
            str.LastIndexOf(arg2?.ToString() ?? string.Empty, StringComparison.InvariantCulture) is var index and >= 0 ? index : null,
        object?[] array =>
            Array.LastIndexOf(array, arg2) is var index and >= 0 ? index : null,
        List<object?> list =>
            list.LastIndexOf(arg2) is var index and >= 0 ? index : null,
        _ => null,
    };

    public static object? Reverse(object? arg) => arg switch
    {
        string str => new string(str.Reverse().ToArray()),
        object?[] array => array.Reverse().ToArray(),
        List<object?> list => list.Reverse<object?>().ToList(),
        _ => null,
    };

    public static object? Get(object? arg1, object? arg2) => (arg1, arg2) switch
    {
        (string str, { } index) when int.TryParse(index.ToString(), out var i) => i switch
        {
            < 0 when str.Length + i >= 0 => str.Substring(str.Length + i, 1),
            >= 0 when WithinBounds(i, str.Length) => str.Substring(i, 1),
            _ => null,
        },
        (object?[] array, { } index) when int.TryParse(index.ToString(), out var i) => i switch
        {
            < 0 when array.Length + i >= 0 => array[array.Length + i],
            >= 0 when WithinBounds(i, array.Length) => array[i],
            _ => null,
        },
        (IReadOnlyList<object?> list, { } index) when int.TryParse(index.ToString(), out var i) => i switch
        {
            < 0 when list.Count + i >= 0 => list[list.Count + i],
            >= 0 when WithinBounds(i, list.Count) => list[i],
            _ => null,
        },
        _ => null,
    };
}
