using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public static class TryableBuilder {
    public static Tryable<TValue> Try<TValue>(Func<TValue> function) => new(function);

    public static AsyncTryable<TValue> Try<TValue>(Func<Task<TValue>> function) => new(function);
}
