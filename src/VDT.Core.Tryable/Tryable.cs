using System;

namespace VDT.Core.Tryable;

public class Tryable<TValue> {
    public Func<TValue> Function { get; set; }
    public Func<Exception, TValue> DefaultErrorHandler { get; set; }
    
    public Tryable(Func<TValue> function, Func<Exception, TValue> defaultErrorHandler) {
        Function = function;
        DefaultErrorHandler = defaultErrorHandler;
    }

    public TValue? Resolve() {
        try {
            return Function();
        }
        catch (Exception ex) {
            return DefaultErrorHandler(ex);
        }
    }
}
