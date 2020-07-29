using System;

namespace VCEL.Core.Expression.Op
{
    public class DefaultOperators : Operators
    {
        public DefaultOperators()
        {
            // Arithmetic Operators
            add.RegisterType<int>((a, b) => a + b);
            add.RegisterType<long>((a, b) => a + b);
            add.RegisterType<float>((a, b) => a + b);
            add.RegisterType<decimal>((a, b) => a + b);
            add.RegisterType<double>((a, b) => a + b);
            add.RegisterType<string>((a, b) => a + b);
            add.RegisterType<TimeSpan>((a, b) => a + b);

            subtract.RegisterType<int>((a, b) => a - b);
            subtract.RegisterType<long>((a, b) => a - b);
            subtract.RegisterType<float>((a, b) => a - b);
            subtract.RegisterType<decimal>((a, b) => a - b);
            subtract.RegisterType<double>((a, b) => a - b);
            subtract.RegisterType<DateTime, TimeSpan>((a, b) => a - b);
            subtract.RegisterType<DateTimeOffset, TimeSpan>((a, b) => a - b);
            subtract.RegisterType<TimeSpan>((a, b) => a - b);
            subtract.RegisterType<DateTimeOffset, TimeSpan, DateTimeOffset>((a, b) => a - b);
            subtract.RegisterType<DateTime, TimeSpan, DateTime>((a, b) => a - b);

            mult.RegisterType<int>((a, b) => a * b);
            mult.RegisterType<long>((a, b) => a * b);
            mult.RegisterType<float>((a, b) => a * b);
            mult.RegisterType<decimal>((a, b) => a * b);
            mult.RegisterType<double>((a, b) => a * b);

            divide.RegisterType<int>((a, b) => a / b);
            divide.RegisterType<long>((a, b) => a / b);
            divide.RegisterType<float>((a, b) => a / b);
            divide.RegisterType<decimal>((a, b) => a / b);
            divide.RegisterType<double>((a, b) => a / b);

            pow.RegisterType<int, double>((a, b) => Math.Pow(a, b));
            pow.RegisterType<long, double>((a, b) => Math.Pow(a, b));
            pow.RegisterType<float, double>((a, b) => Math.Pow(a, b));
            pow.RegisterType<decimal, double>((a, b) => Math.Pow((double)a, (double)b));
            pow.RegisterType<double, double>((a, b) => Math.Pow(a, b));


            RegisterUpcastOrder(
                new Type[]
                {
                    typeof(byte),
                    typeof(short),
                    typeof(int),
                    typeof(long),
                    typeof(float),
                    typeof(double),
                    typeof(decimal)
                },
                add,
                subtract,
                mult,
                divide,
                pow);

            add.RegisterUpCast<byte, string>();
            add.RegisterUpCast<short, string>();
            add.RegisterUpCast<int, string>();
            add.RegisterUpCast<long, string>();
            add.RegisterUpCast<float, string>();
            add.RegisterUpCast<double, string>();
            add.RegisterUpCast<decimal, string>();
        }
    }
}
