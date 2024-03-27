using System;
using System.Runtime.Serialization;

namespace VCEL.Expression
{
    internal class MissingFunctionException : Exception
    {
        public MissingFunctionException()
        {
        }

        public MissingFunctionException(string message) : base(message)
        {
        }

        public MissingFunctionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [Obsolete("Obsolete")]
        protected MissingFunctionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}