using System;

namespace VCEL.Expression;

internal class MissingFunctionException(string message) : Exception(message);
