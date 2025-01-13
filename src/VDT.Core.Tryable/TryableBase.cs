using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public abstract class TryableBase<TValue, TFinally, TTryableBase> where TTryableBase : TryableBase<TValue, TFinally, TTryableBase> {
    public Func<TValue> Function { get; set; }
    public IList<IErrorHandler<TValue>> ErrorHandlers { get; set; } = [];
    public Func<TValue>? DefaultErrorHandler { get; set; }
    public TFinally? CompleteHandler { get; set; }

    public TryableBase(Func<TValue> function) {
        Function = function;
    }

    public TTryableBase Catch<TException>(Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(handler));
        return (TTryableBase)this;
    }

    public TTryableBase Catch<TException>(Func<TException, bool> filter, Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(filter, handler));
        return (TTryableBase)this;
    }

    public TTryableBase Catch(Func<TValue> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return (TTryableBase)this;
    }

    public TTryableBase Finally(TFinally completeHandler) {
        CompleteHandler = completeHandler;
        return (TTryableBase)this;
    }

    public abstract TValue Execute();
}
