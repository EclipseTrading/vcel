using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Monad.List
{
    public class ListMonad<T> : IMonad<List<T>>
    {
        public List<T> Unit { get; } = new List<T>(new List<T>());
        public List<T> Lift(object value)
        {
            return value is T t ? new List<T> { t } : new List<T>();
        }

        public List<T> Bind(List<T> m, Func<object, List<T>> f)
        {
            var results = m
                .Select(e => f(e))
                .SelectMany(l => l)
                .ToList();

            return new List<T>(results);
        }

        public List<T> Bind(List<T> a, List<T> b, Func<object, object, List<T>> f)
            => BindEx.Bind(a, b, f, this);

        public static ListMonad<T> Instance { get; } = new ListMonad<T>();
    }
}
