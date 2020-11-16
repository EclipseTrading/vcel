using System;
using System.Collections.Generic;

namespace VCEL.Expression
{
    public interface IExpressionFactory<T>
    {
        // Control
        IExpression<T> Ternary(IExpression<T> conditional, IExpression<T> trueConditon, IExpression<T> falseCondition);
        IExpression<T> Let(IReadOnlyList<(string, IExpression<T>)> bindings, IExpression<T> expr);
        IExpression<T> Guard(
            IReadOnlyList<(IExpression<T>, IExpression<T>)> guardClauses,
            IExpression<T> otherwise = null);

        // Equality
        IExpression<T> LessThan(IExpression<T> l, IExpression<T> r);
        IExpression<T> GreaterThan(IExpression<T> l, IExpression<T> r);
        IExpression<T> LessOrEqual(IExpression<T> l, IExpression<T> r);
        IExpression<T> GreaterOrEqual(IExpression<T> l, IExpression<T> r);
        IExpression<T> Between(IExpression<T> l, IExpression<T> r);
        IExpression<T> In(IExpression<T> l, ISet<object> set);
        IExpression<T> Matches(IExpression<T> l, IExpression<T> r);

        // Boolean
        IExpression<T> And(IExpression<T> l, IExpression<T> r);
        IExpression<T> Or(IExpression<T> l, IExpression<T> r);
        IExpression<T> Not(IExpression<T> e);

        // Primitive Types
        IExpression<T> Int(int i);
        IExpression<T> Long(long l);
        IExpression<T> Double(double d);
        IExpression<T> String(string s);
        IExpression<T> Bool(bool b);
        IExpression<T> DateTimeOffset(DateTimeOffset dateTimeOffset);
        IExpression<T> TimeSpan(TimeSpan timeSpan);
        IExpression<T> Set(ISet<object> set);
        IExpression<T> Value(object o);

        // Collections
        IExpression<T> List(IReadOnlyList<IExpression<T>> l);

        // Binary Operators
        IExpression<T> Add(IExpression<T> l, IExpression<T> r);
        IExpression<T> Multiply(IExpression<T> l, IExpression<T> r);
        IExpression<T> Subtract(IExpression<T> l, IExpression<T> r);
        IExpression<T> Divide(IExpression<T> l, IExpression<T> r);
        IExpression<T> Pow(IExpression<T> l, IExpression<T> r);

        IExpression<T> Paren(IExpression<T> expr);

        // Variables
        IExpression<T> Property(string name);

        // Functions
        IExpression<T> Function(string name, IReadOnlyList<IExpression<T>> args);
        IExpression<T> UnaryMinus(IExpression<T> expression);
        IExpression<T> Null();
        IExpression<T> Eq(IExpression<T> l, IExpression<T> r);
        IExpression<T> NotEq(IExpression<T> l, IExpression<T> r);
        IExpression<T> Member(IExpression<T> obj, IExpression<T> memberExpr);
        IExpression<T> This();
    }
}
