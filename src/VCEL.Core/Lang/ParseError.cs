using System.IO;

namespace VCEL.Core.Lang
{
    public class ParseError
    {
        public ParseError(string message, string token, int line, int start, int stop)
        {
            Message = message;
            Token = token;
            Line = line;
            Start = start;
            Stop = stop;
        }

        public string Message { get; }
        public string Token { get; }
        public int Line { get; }
        public int Start { get; }
        public int Stop { get; }

        public string? GetExprError(string? exprString)
        {
            if (exprString == null)
                return null;

            if(Start >= exprString.Length)
            {
                return "Expected more input - found <EOF>";
            }

            return exprString.Substring(0, Start) 
                + ">>>" 
                + exprString.Substring(Start, Stop - Start + 1) 
                + "<<<" + exprString.Substring(Stop + 1);
        }
    }
}
