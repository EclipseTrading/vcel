namespace VCEL.Core.Lang;

public sealed class ParseError(string message, string token, int line, int start, int stop)
{
    public string Message { get; } = message;
    public string Token { get; } = token;
    public int Line { get; } = line;
    public int Start { get; } = start;
    public int Stop { get; } = stop;

    public string? GetExprError(string? exprString)
    {
        if (exprString == null)
            return null;

        return Start >= exprString.Length
            ? "Expected more input - found <EOF>"
            : $"{exprString[..Start]}>>>{exprString.Substring(Start, Stop - Start + 1)}<<<{exprString[(Stop + 1)..]}";
    }
}