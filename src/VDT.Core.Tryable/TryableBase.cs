using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public abstract class TryableBase<TIn, TOut, TFinally> {
    public Func<TOut> Function { get; set; }
    public IList<IErrorHandler<TOut>> ErrorHandlers { get; set; } = [];
    public Func<TOut>? DefaultErrorHandler { get; set; }
    public TFinally? CompleteHandler { get; set; }

    public TryableBase(Func<TOut> function) {
        Function = function;
    }

    public TryableBase<TIn, TOut, TFinally> Catch<TException>(Func<TException, TOut> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TOut>(handler));
        return this;
    }

    public TryableBase<TIn, TOut, TFinally> Catch<TException>(Func<TException, bool> filter, Func<TException, TOut> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TOut>(filter, handler));
        return this;
    }

    public TryableBase<TIn, TOut, TFinally> Catch(Func<TOut> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return this;
    }

    public TryableBase<TIn, TOut, TFinally> Finally(TFinally completeHandler) {
        CompleteHandler = completeHandler;
        return this;
    }

    public abstract TOut Execute(TIn value);
}
