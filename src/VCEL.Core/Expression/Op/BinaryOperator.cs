using System;
using System.Collections.Generic;

namespace VCEL.Core.Expression.Op
{
    public class BinaryOperator : Operator, IBinaryOperator
    {
        private readonly Dictionary<Type, BinaryBase> operators = new Dictionary<Type, BinaryBase>();
        private readonly Dictionary<(Type, Type), BinaryBase> dualTypeOperators = new Dictionary<(Type, Type), BinaryBase>();
        private readonly Dictionary<(Type, Type), Type> upcasts = new Dictionary<(Type, Type), Type>();

        public string OpChar { get; }

        public BinaryOperator(string opChar)
        {
            OpChar = opChar;
        }

        public bool Supports(Type type) => operators.ContainsKey(type);

        public object Evaluate(object l, object r)
        {
            var lType = l?.GetType();
            var rType = r?.GetType();

            if(lType != null
                && lType == rType
                && operators.TryGetValue(l.GetType(), out var oper))
            {
                return oper.Evaluate(l, r);
            }

            if(lType != null
                && rType != null
                && dualTypeOperators.TryGetValue((lType, rType), out var dop))
            {
                return dop.Evaluate(l, r);
            }

            if(!TryUpCast(l, r, out var ul, out var ur, out var t)
            || t == null
            || !operators.TryGetValue(t, out var op))
            {
                return null;
            }
            return op.Evaluate(ul, ur);
        }

        public void RegisterType<TLeft, TRight, TResult>(Func<TLeft, TRight, TResult> append)
        {
            dualTypeOperators[(typeof(TLeft), typeof(TRight))] = new Binary<TLeft, TRight, TResult>(append);
        }

        public void RegisterType<T, TResult>(Func<T, T, TResult> append)
        {
            operators[typeof(T)] = new Binary<T, TResult>(append);
        }

        public void RegisterType<T>(Func<T, T, T> append)
        {
            operators[typeof(T)] = new Binary<T, T>(append);
        }

        public void RegisterUpCastOrder(params Type[] types)
        {
            for(var i = 0; i < types.Length; i++)
            {
                for(var j = i + 1; j < types.Length; j++)
                {
                    RegisterUpCast(types[i], types[j], types[j]);
                }
            }
        }

        public void RegisterUpCast<TA, TB>() => RegisterUpCast(typeof(TA), typeof(TB), typeof(TB));
        public void RegisterUpCast<TA, TB, TC>() => RegisterUpCast(typeof(TA), typeof(TB), typeof(TC));

        public void RegisterUpCast(Type a, Type b, Type c)
        {
            upcasts[(a, b)] = c;
            upcasts[(b, a)] = c;
        }

        public bool TryUpCast(object l, object r, out object ul, out object ur, out Type type)
        {
            ul = null;
            ur = null;
            type = null;

            if(l == null && r == null || l?.GetType() == r?.GetType())
            {
                ul = l;
                ur = r;
                type = l?.GetType();
                return true;
            }

            if(l == null || r == null)
            {
                return false;
            }

            var lt = l.GetType();
            var rt = r.GetType();

            if(upcasts.TryGetValue((lt, rt), out type))
            {
                ul = Convert.ChangeType(l, type);
                ur = Convert.ChangeType(r, type);
                return true;
            }

            return false;
        }

        public override string ToString() => OpChar;
    }
}
