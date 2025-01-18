using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public abstract class TryableBase<TValue, TFinally> {
    public Func<TValue> Function { get; set; }
    public IList<IErrorHandler<TValue>> ErrorHandlers { get; set; } = [];
    public Func<TValue>? DefaultErrorHandler { get; set; }
    public TFinally? CompleteHandler { get; set; }

    public TryableBase(Func<TValue> function) {
        Function = function;
    }

    public TryableBase<TValue, TFinally> Catch<TException>(Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(handler));
        return this;
    }

    public TryableBase<TValue, TFinally> Catch<TException>(Func<TException, bool> filter, Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(filter, handler));
        return this;
    }

    public TryableBase<TValue, TFinally> Catch(Func<TValue> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return this;
    }

    public TryableBase<TValue, TFinally> Finally(TFinally completeHandler) {
        CompleteHandler = completeHandler;
        return this;
    }

    public abstract TValue Execute();
}
