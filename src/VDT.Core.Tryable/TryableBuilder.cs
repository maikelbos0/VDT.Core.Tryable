using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public static class TryableBuilder {
    public static Tryable<Void, TOut> Try<TOut>(Func<TOut> function) => new(_ => function());

    public static Tryable<TIn, TOut> Try<TIn, TOut>(Func<TIn, TOut> function) => new(function);

    public static AsyncTryable<Void, TOut> Try<TOut>(Func<Task<TOut>> function) => new(_ => function());

    public static AsyncTryable<TIn, TOut> Try<TIn, TOut>(Func<TIn, Task<TOut>> function) => new(function);
}
