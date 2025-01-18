using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public static class TryableBuilder {
    public static Tryable<TOut> Try<TOut>(Func<TOut> function) => new(function);

    public static AsyncTryable<TOut> Try<TOut>(Func<Task<TOut>> function) => new(function);
}
