using System;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad;

namespace VCEL.Core.Monad.List
{
    public class ListMonad<T> : IMonad<List<T>>
    {
        public List<T> Unit { get; } = new List<T>(new List<T>());
        public List<T> Lift<TValue>(TValue value)
        {
            return value is T t ? new List<T> { t } : new List<T>();
        }

        public List<T> Bind(List<T> m, Func<object?, List<T>> f)
        {
            var results = m
                .Select(e => f(e))
                .SelectMany(l => l)
                .ToList();

            return new List<T>(results);
        }

        public List<T> Bind(List<T> m, IContext<List<T>> context, Func<object?, IContext<List<T>>, List<T>> f)
        {
            var results = m
                .Select(e => f(e, context))
                .SelectMany(l => l)
                .ToList();

            return new List<T>(results);
        }

        public List<T> Bind<TValue>(List<T> m, IContext<List<T>> context, Func<object?, IContext<List<T>>, TValue, List<T>> f, TValue value)
        {
            var results = m
                .Select(e => f(e, context, value))
                .SelectMany(l => l)
                .ToList();

            return new List<T>(results);
        }

        public List<T> Bind(List<T> m, List<T> b, Func<object?, object?, List<T>> f)
            => BindExtensions.Bind(m, b, f, this);

#pragma warning disable CA1000
        public static ListMonad<T> Instance { get; } = new ListMonad<T>();
#pragma warning restore CA1000
    }
}
