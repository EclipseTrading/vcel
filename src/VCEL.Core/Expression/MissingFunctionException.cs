using System;

namespace VCEL.Expression;

internal sealed class MissingFunctionException(string message) : Exception(message);
