using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Core.Expression.Func;
using VCEL.CSharp.Expression;
using VCEL.Expression;
using VCEL.Monad;

namespace VCEL.CSharp
{
    public class ToCSharpCodeFactory : ExpressionFactory<string>
    {
        public ToCSharpCodeFactory(
            IMonad<string> monad,
            IFunctions<string>? functions = null)
            : base(monad, functions)
        {
        }

        public override IExpression<string> Ternary(IExpression<string> conditional, IExpression<string> trueCondition, IExpression<string> falseCondition)
            => new ToCSharpTernary(Monad, conditional, trueCondition, falseCondition);

        public override IExpression<string> Let(IReadOnlyList<(string, IExpression<string>)> bindings, IExpression<string> expr)
            => new ToCSharpLetExpr(Monad, bindings, expr);

        public override IExpression<string> Guard(IReadOnlyList<(IExpression<string>, IExpression<string>)> guardClauses, IExpression<string>? otherwise = null)
            => new ToCSharpGuardExpr(Monad, guardClauses, otherwise);

        public override IExpression<string> In(IExpression<string> l, IExpression<string> r)
            => new ToCSharpInOp(Monad, l, r);

        public override IExpression<string> Spread(IExpression<string> expr)
            => new ToCSharpSpreadOp(Monad, expr);

        public override IExpression<string> Set(ISet<IExpression<string>> s)
            => new ToCSharpStringOp((context) => $"(new HashSet<object>{{{string.Join(",", s.Select(x => x.Evaluate(context)))}}})", Monad);

        public override IExpression<string> List(IReadOnlyList<IExpression<string>> exprs)
        {
            string GetItems(IContext<string> context) 
            {
                var lists = exprs.Select(e => e is ToCSharpSpreadOp spread 
                    ? $"(IEnumerable<object>)(object){e.Evaluate(context)}"
                    : $"new object [] {{ {e.Evaluate(context)} }}");
                return $@"(new IEnumerable<object>[] {{ {string.Join(", ", lists)} }}.SelectMany(e => e)).ToList()";
            }

            return new ToCSharpStringOp(GetItems, Monad);
        }

        public override IExpression<string> And(IExpression<string> l, IExpression<string> r)
            => new ToCSharpAndOp(Monad, l, r);

        public override IExpression<string> Or(IExpression<string> l, IExpression<string> r)
            => new ToCSharpOrOp(Monad, l, r);

        public override IExpression<string> GreaterThan(IExpression<string> l, IExpression<string> r)
            => new ToCSharpCompareOp(">", Monad, l, r);

        public override IExpression<string> GreaterOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToCSharpCompareOp(">=", Monad, l, r);

        public override IExpression<string> LessThan(IExpression<string> l, IExpression<string> r)
            => new ToCSharpCompareOp("<", Monad, l, r);

        public override IExpression<string> LessOrEqual(IExpression<string> l, IExpression<string> r)
            => new ToCSharpCompareOp("<=", Monad, l, r);

        public override IExpression<string> Add(IExpression<string> l, IExpression<string> r)
            => new ToCSharpBinaryOp("+", Monad, l, r);

        public override IExpression<string> Subtract(IExpression<string> l, IExpression<string> r)
            => new ToCSharpBinaryOp("-", Monad, l, r);

        public override IExpression<string> Divide(IExpression<string> l, IExpression<string> r)
            => new ToCSharpDivideOp(Monad, l, r);

        public override IExpression<string> Multiply(IExpression<string> l, IExpression<string> r)
            => new ToCSharpBinaryOp("*", Monad, l, r);

        public override IExpression<string> Eq(IExpression<string> l, IExpression<string> r)
            => new ToCSharpEqOp(Monad, l, r);

        public override IExpression<string> NotEq(IExpression<string> l, IExpression<string> r)
            => new ToCSharpNotEqOp(Monad, l, r);

        public override IExpression<string> Pow(IExpression<string> l, IExpression<string> r)
            => new ToCSharpPowOp(Monad, l, r);

        public override IExpression<string> Mod(IExpression<string> l, IExpression<string> r)
            => new ToCSharpModOp(Monad, l, r);
        
        public override IExpression<string> Bool(bool b)
            => new ToCSharpStringOp((context) => b ? "true" : "false", Monad);

        public override IExpression<string> String(string s)
            => new ToCSharpStringLiteralOp(s, Monad);

        public override IExpression<string> Null()
            => new ToCSharpStringOp((context) => "null", Monad);

        public override IExpression<string> Property(string name)
            => new ToCSharpPropertyOp(name, Monad);

        public override IExpression<string> UnaryMinus(IExpression<string> expression)
            => new ToCSharpStringOp((context) => $"-{expression.Evaluate(context)}", Monad);

        public override IExpression<string> Not(IExpression<string> e)
            => new ToCSharpStringOp((context) => $"!{e.Evaluate(context)}", Monad);

        public override IExpression<string> Matches(IExpression<string> l, IExpression<string> r)
            => new ToCSharpMatchesOp(Monad, l, r);

        public override IExpression<string> Function(string name, IReadOnlyList<IExpression<string>> args)
            => new ToCSharpFunction(Monad, name, args, Functions);

        public override IExpression<string> TimeSpan(TimeSpan timeSpan)
            => new ToCSharpTimeSpan(Monad, timeSpan);

        public override IExpression<string> DateTimeOffset(DateTimeOffset dateTimeOffset)
            => new ToCSharpDateTimeOffSet(Monad, dateTimeOffset);

        public override IExpression<string> Between(IExpression<string> l, IExpression<string> lower, IExpression<string> upper)
            => new ToCSharpBetweenOp(Monad, l, lower, upper);

        public override IExpression<string> Member(IExpression<string> l, IExpression<string> r)
            => new ToCSharpMemberOp(Monad, l, r);

        // This makes sure double value doesn't get down casted implicitly to integer.
        public override IExpression<string> Double(double d)
            => new ToCSharpStringOp((context) => $"{d:0.0#######################}", Monad);
    }
}
