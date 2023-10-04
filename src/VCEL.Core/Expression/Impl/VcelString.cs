using System;

namespace VCEL.Core.Expression.Impl;

public static class VcelString
{
    public static string? Substring(string? source, int start) =>
        VcelIndexable.WithinBounds(start, source?.Length ?? 0)
            ? source?[start..]
            : null;

    public static string? Substring(string? source, int start, int length) =>
        VcelIndexable.WithinBounds(start, source?.Length ?? 0) &&
        VcelIndexable.WithinBounds(start + length, source?.Length ?? 0)
            ? source?[start..(start + length)]
            : null;

    public static string? Substring(object?[] args)
    {
        switch (args.Length)
        {
            case 2:
            {
                var sourceString = args[0]?.ToString();
                var startIndex = args[1]?.ToString() is { } startString && int.TryParse(startString, out var start)
                    ? start
                    : throw new ArgumentException("Invalid start index");
                return Substring(sourceString, startIndex);
            }
            case 3:
            {
                var sourceString = args[0]?.ToString();
                var startIndex = args[1]?.ToString() is { } startString && int.TryParse(startString, out var start)
                    ? start
                    : throw new ArgumentException("Invalid start index");
                var strLength = args[2]?.ToString() is { } lengthString && int.TryParse(lengthString, out var length)
                    ? length
                    : throw new ArgumentException("Invalid length");
                return Substring(sourceString, startIndex, strLength);
            }
            default:
                return null;
        }
    }

    public static string[]? Split(string? str, string? separator) => str is null
        ? null
        : separator is null
            ? new[] { str }
            : str.Split(new[] { separator }, StringSplitOptions.None);

    public static string? Replace(string? source, string? target, string? replaceWith) => target is null
        ? source
        : source?.Replace(target, replaceWith);

    public static string? Trim(string? str) => str?.Trim();
}
