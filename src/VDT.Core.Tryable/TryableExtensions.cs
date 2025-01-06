using System;

namespace VDT.Core.Tryable;

public static class TryableExtensions {
    public static Tryable<TValue> Catch<TException, TValue>(this Tryable<TValue> tryable, Func<TException, TValue> handler) where TException : Exception {
        tryable.ErrorHandlers.Add(new ErrorHandler<TException, TValue>(handler));
        return tryable;
    }

    public static Tryable<TValue> Catch<TException, TValue>(this Tryable<TValue> tryable, Func<TException, bool> filter, Func<TException, TValue> handler) where TException : Exception {
        tryable.ErrorHandlers.Add(new ErrorHandler<TException, TValue>(filter, handler));
        return tryable;
    }

    public static Tryable<TValue> Catch<TValue>(this Tryable<TValue> tryable, Func<TValue> defaultErrorHandler) {
        tryable.DefaultErrorHandler = defaultErrorHandler;
        return tryable;
    }

    public static Tryable<TValue> Finally<TValue>(this Tryable<TValue> tryable, Action completeHandler) {
        tryable.CompleteHandler = completeHandler;
        return tryable;
    }
}
