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

    public abstract TValue Execute();
}
