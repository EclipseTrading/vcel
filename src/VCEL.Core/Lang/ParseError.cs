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

        public string GetExprError(string exprString)
        {
            if (exprString == null)
                return null;

            var lines = exprString.Split('\n', '\r');
            if(Line > lines.Length)
            {
                return $"Line: {Line} not found in '{exprString}'";
            }
            var lineStr = lines[Line - 1];
            if(Start >= lineStr.Length || Stop >= lineStr.Length)
            {
                return $"Invalid Start/Stop index in error message.";
            }
            return lineStr.Substring(0, Start) 
                + " >>> " 
                + lineStr.Substring(Start, Stop - Start + 1) 
                + " <<< " + lineStr.Substring(Stop + 1);
        }
    }
}
