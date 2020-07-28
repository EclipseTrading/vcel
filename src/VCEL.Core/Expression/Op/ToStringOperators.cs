namespace VCEL.Core.Expression.Op
{
    public class ToStringOperators : Operators
    {
        public ToStringOperators()
        {
            RegisterTypes<string>((a, b, op) => $"{a} {op.OpChar} {b}", add, subtract, mult, divide);
        }
    }
}
