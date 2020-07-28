using System;

namespace VCEL.Core.Expression.Op
{
    public class Operators : IOperators
    {
        protected readonly BinaryOperator add = new BinaryOperator("+");
        protected readonly BinaryOperator mult = new BinaryOperator("*");
        protected readonly BinaryOperator divide = new BinaryOperator("/");
        protected readonly BinaryOperator subtract = new BinaryOperator("-");
        protected readonly BinaryOperator pow = new BinaryOperator("^");

        public IBinaryOperator Add => add;
        public IBinaryOperator Multiply => mult;
        public IBinaryOperator Subtract => new Subtract(subtract);
        public IBinaryOperator Divide => divide;
        public IBinaryOperator Pow => pow;
        public BooleanOperator And { get; } = new BooleanOperator("and", (a, b) => a && b);
        public BooleanOperator Or { get; } = new BooleanOperator("or", (a, b) => a || b);

        public void RegisterTypes<T>(Func<T, T, BinaryOperator, T> eval, params BinaryOperator[] ops)
        {
            foreach (var op in ops)
            {
                op.RegisterType<T, T>((a, b) => eval(a, b, op));
            }
        }

        public void RegisterUpcasts(Type a, Type b, Type type, params BinaryOperator[] ops)
        {
            foreach (var op in ops)
            {
                op.RegisterUpCast(a, b, type);
            }
        }
        public void RegisterUpcastOrder(Type[] types, params BinaryOperator[] ops)
        {
            foreach (var op in ops)
            {
                op.RegisterUpCastOrder(types);
            }
        }
    }
}
