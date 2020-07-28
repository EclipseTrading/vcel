using System;
using System.Collections.Generic;
using VCEL.Core.Expression.Func;
using VCEL.Core.Expression.Impl;
using VCEL.Core.Expression.Op;
using VCEL.Monad;

namespace VCEL.Expression
{
    public class ExpressionFactory<T> : IExpressionFactory<T>
    {
        public ExpressionFactory(
            IMonad<T> monad,
            IOperators operators = null,
            IFunctions functions = null)
        {
            this.Monad = monad;
            this.Operators = operators ?? new DefaultOperators();
            Functions = functions ?? new DefaultFunctions();
        }

        public IMonad<T> Monad { get; }
        public IOperators Operators { get; }
        public IFunctions Functions { get; }

        public virtual IExpression<T> Ternary(
            IExpression<T> conditional,
            IExpression<T> trueConditon,
            IExpression<T> falseCondition)
            => new Ternary<T>(Monad, conditional, trueConditon, falseCondition);

        public IExpression<T> Let(
            IReadOnlyList<(string, IExpression<T>)> bindings,
            IExpression<T> expr)
            => new LetExpr<T>(Monad, bindings, expr);

        public IExpression<T> Guard(
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
        
        public virtual IExpression<T> In(IExpression<T> l, IExpression<T> r)
            => new InExpr<T>(Monad, l, r);
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
        public virtual IExpression<T> Value(object o)
            => new ValueExpr<T>(Monad, o);
        public virtual IExpression<T> List(IReadOnlyList<IExpression<T>> exprs)
            => new ListExpr<T>(Monad, exprs);

        public virtual IExpression<T> Add(IExpression<T> l, IExpression<T> r)
            => BinaryOp(Operators.Add, l, r);
        public virtual IExpression<T> Multiply(IExpression<T> l, IExpression<T> r)
            => BinaryOp(Operators.Multiply, l, r);

        public virtual IExpression<T> Subtract(IExpression<T> l, IExpression<T> r)
            => BinaryOp(Operators.Subtract, l, r);
        public virtual IExpression<T> Divide(IExpression<T> l, IExpression<T> r)
            => BinaryOp(Operators.Divide, l, r);
        public virtual IExpression<T> Pow(IExpression<T> l, IExpression<T> r)
            => BinaryOp(Operators.Pow, l, r);

        public virtual IExpression<T> BinaryOp(
            IBinaryOperator op,
            IExpression<T> l,
            IExpression<T> r)
            => new BinaryExpr<T>(op, Monad, l, r);

        public virtual IExpression<T> And(IExpression<T> l, IExpression<T> r)
            => BooleanOp(Operators.And, l, r);
        public virtual IExpression<T> Or(IExpression<T> l, IExpression<T> r)
            => BooleanOp(Operators.Or, l, r);

        public virtual IExpression<T> Not(IExpression<T> e)
            => new NotExpr<T>(Monad, e);

        public virtual IExpression<T> BooleanOp(
            BooleanOperator op,
            IExpression<T> l,
            IExpression<T> r)
            => new BooleanExpr<T>(op, Monad, l, r, CastBool);

        public virtual IExpression<T> Function(
            string name,
            IReadOnlyList<IExpression<T>> args)
            => new FunctionExpr<T>(Monad, name, args, Functions);

        public virtual IExpression<T> Property(string name)
            => new Property<T>(Monad, name);

        public virtual IExpression<T> Paren(IExpression<T> expr)
            => new ParenExpr<T>(Monad, expr);

        private T CastBool(T monad)
            => Monad.Bind(monad, v => Monad.Lift((v is bool b) ? b : false));

        public IExpression<T> LegacyFunction(IExpression<T> expression)
        {
            var func = expression as FunctionExpr<T>;
            return Function(func.Name.ToLower(), func.Args);
        }
        public IExpression<T> LegacyProperty(string name)
            => Function(name.ToLower(), new List<IExpression<T>>());

        public IExpression<T> UnaryMinus(IExpression<T> expression)
            => new UnaryMinusExpr<T>(Monad, expression);

        public IExpression<T> Eq(IExpression<T> l, IExpression<T> r)
            => new EqExpr<T>(Monad, l, r);

        public IExpression<T> NotEq(IExpression<T> l, IExpression<T> r)
            => new NotEqExpr<T>(Monad, l, r);

        public IExpression<T> Null() => new NullExpr<T>(Monad);
    }
}
