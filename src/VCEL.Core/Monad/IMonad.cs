using System;

namespace VCEL.Monad;

public interface IMonad<TMonad>
{
    /// <summary>
    /// Unit is the identity element of the monad.
    /// </summary>
    TMonad Unit { get; }

    /// <summary>
    /// Lift is a way to wrap a value in a monad.
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    TMonad Lift<TValue>(TValue value);

    /// <summary>
    /// Bind is the core operation of a monad. It allows us to chain together operations that return a monad.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    TMonad Bind(TMonad m, Func<object?, TMonad> f);

    /// <summary>
    /// Bind is the core operation of a monad. It allows us to chain together operations that return a monad.
    /// This version of bind is used when we want to pass the context and avoid closure allocations.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="context"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    TMonad Bind(TMonad m, IContext<TMonad> context, Func<object?, IContext<TMonad>, TMonad> f);

    /// <summary>
    /// This bind implementation is used when we short-circuit the evaluation of the monad.
    /// It exists to allow the context to be passed to the function in a way that avoids closure allocations.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="context"></param>
    /// <param name="f"></param>
    /// <param name="value"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    TMonad Bind<TValue>(TMonad m, IContext<TMonad> context, Func<object?, IContext<TMonad>, TValue, TMonad> f,
        TValue value);

    /// <summary>
    /// Bind is the core operation of a monad. It allows us to chain together operations that return a monad.
    /// This version of bind is used when we have two monads that we want to combine.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    TMonad Bind(TMonad a, TMonad b, Func<object?, object?, TMonad> f);
}