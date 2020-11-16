using System;
using System.Collections.Generic;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Core.Monad.Tasks;
using VCEL.Monad;

namespace VCEL.Expression
{
    public class ExpressionFactory<T> : IExpressionFactory<T>
    {
        public ExpressionFactory(
            IMonad<T> monad,
            IFunctions<T> functions = null)
        {
            Monad = monad;
            Functions = functions ?? new DefaultFunctions<T>();
        }

        public IMonad<T> Monad { get; }
        public IFunctions<T> Functions { get; }

        public virtual IExpression<T> Ternary(
            IExpression<T> conditional,
            IExpression<T> trueConditon,
            IExpression<T> falseCondition)
            => new Ternary<T>(Monad, conditional, trueConditon, falseCondition);

        public virtual IExpression<T> Let(
            IReadOnlyList<(string, IExpression<T>)> bindings,
            IExpression<T> expr)
            => new LetExpr<T>(Monad, bindings, expr);

        public virtual IExpression<T> Guard(
            IReadOnlyList<(IExpression<T>, IExpression<T>)> guardClauses,
            IExpression<T> otherwise = null)
            => new GuardExpr<T>(Monad, guardClauses, otherwise);

        public virtual IExpression<T> LessThan(IExpression<T> l, IExpression<T> r)
            => new LessThan<T>(Monad, l, r);

        public virtual IExpression<T> LessOrEqual(IExpression<T> l, IExpression<T> r)
            => new LessOrEqual<T>(Monad, l, r);
        public virtual IExpression<T> GreaterThan(IExpression<T> l, IExpression<T> r)
            => new GreaterThan<T>(Monad, l, r);
        public virtual IExpression<T> GreaterOrEqual(IExpression<T> l, IExpression<T> r)
            => new GreaterOrEqual<T>(Monad, l, r);

        public virtual IExpression<T> In(IExpression<T> l, ISet<object> set)
            => new InExpr<T>(Monad, l, set);
        public virtual IExpression<T> Between(IExpression<T> l, IExpression<T> r)
            => new BetweenExpr<T>(Monad, l, r);
        public virtual IExpression<T> Matches(IExpression<T> l, IExpression<T> r)
            => new MatchesExpr<T>(Monad, l, r);

        public virtual IExpression<T> Bool(bool b) => Value(b);
        public virtual IExpression<T> Double(double d) => Value(d);
        public virtual IExpression<T> Int(int i) => Value(i);
        public virtual IExpression<T> Long(long l) => Value(l);
        public virtual IExpression<T> String(string s) => Value(s);
        public virtual IExpression<T> DateTimeOffset(DateTimeOffset dateTimeOffset)
            => Value(dateTimeOffset);
        public virtual IExpression<T> TimeSpan(TimeSpan timeSpan)
            => Value(timeSpan);
        public virtual IExpression<T> Set(ISet<object> s) => Value(s);
        public virtual IExpression<T> Value(object o)
            => new ValueExpr<T>(Monad, o);

        public virtual IExpression<T> List(IReadOnlyList<IExpression<T>> exprs)
            => new ListExpr<T>(Monad, exprs);
        public virtual IExpression<T> Add(IExpression<T> l, IExpression<T> r)
            => new AddExpr<T>(Monad, l, r);
        public virtual IExpression<T> Multiply(IExpression<T> l, IExpression<T> r)
            => new MultExpr<T>(Monad, l, r);

        public virtual IExpression<T> Subtract(IExpression<T> l, IExpression<T> r)
            => new SubtractExpr<T>(Monad, l, r);
        public virtual IExpression<T> Divide(IExpression<T> l, IExpression<T> r)
            => new DivideExpr<T>(Monad, l, r);
        public virtual IExpression<T> Pow(IExpression<T> l, IExpression<T> r)
            => new PowExpr<T>(Monad, l, r);

        public virtual IExpression<T> And(IExpression<T> l, IExpression<T> r)
            => new AndExpr<T>(Monad, l, r);
        public virtual IExpression<T> Or(IExpression<T> l, IExpression<T> r)
            => new OrExpr<T>(Monad, l, r);

        public virtual IExpression<T> Not(IExpression<T> e)
            => new NotExpr<T>(Monad, e);

        public virtual IExpression<T> Function(
            string name,
            IReadOnlyList<IExpression<T>> args)
            => Functions.HasFunction(name)
                ? new FunctionExpr<T>(Monad, name, args, Functions.GetFunction(name))
                : throw new MissingFunctionException($"Missing function: '{name}'");

        public virtual IExpression<T> Property(string name)
            => new Property<T>(Monad, name);

        public virtual IExpression<T> Paren(IExpression<T> expr)
            => new ParenExpr<T>(Monad, expr);

        public virtual IExpression<T> UnaryMinus(IExpression<T> expression)
            => new UnaryMinusExpr<T>(Monad, expression);

        public virtual IExpression<T> Eq(IExpression<T> l, IExpression<T> r)
            => new EqExpr<T>(Monad, l, r);

        public virtual IExpression<T> NotEq(IExpression<T> l, IExpression<T> r)
            => new NotEqExpr<T>(Monad, l, r);

        public virtual IExpression<T> Null() => new NullExpr<T>(Monad);
        public IExpression<T> Member(IExpression<T> obj, IExpression<T> memberExpr)
            => new ObjectMember<T>(Monad, obj, memberExpr);
        public IExpression<T> This() => new ThisExpr<T>(Monad);
    }
}
