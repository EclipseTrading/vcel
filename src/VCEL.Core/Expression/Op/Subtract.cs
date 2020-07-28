namespace VCEL.Core.Expression.Op
{
    public class Subtract : Operator, IBinaryOperator
    {
        private readonly IBinaryOperator defaultOp;

        public Subtract(IBinaryOperator defaultOp)
        {
            this.defaultOp = defaultOp;
        }

        public object Evaluate(object l, object r)
        {
            switch(l)
            {
                case double dl when r is double dr:
                    return dl - dr;
                case int il when r is int ir:
                    return il - ir;
                case decimal dl when r is decimal dr:
                    return dl - dr;
                case long ll when r is long lr:
                    return ll - lr;
                case long ll when r is int ir:
                    return ll - ir;
                case int il when r is long lr:
                    return il - lr;
            }
            return defaultOp.Evaluate(l, r);
        }
    }
}
