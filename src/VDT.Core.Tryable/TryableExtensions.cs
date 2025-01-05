using System;

namespace VDT.Core.Tryable;

public static class TryableExtensions {
    public static Tryable<TValue> Catch<TValue>(this Tryable<TValue> tryable, Func<TValue> defaultErrorHandler) {
        tryable.DefaultErrorHandler = defaultErrorHandler;
        return tryable;
    }
}
