using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public class Tryable<TValue> {
    public Func<TValue> Function { get; set; }
    public IList<IErrorHandler<TValue>> ErrorHandlers { get; set; } = [];
    public Func<TValue>? DefaultErrorHandler { get; set; }
    public Action? CompleteHandler { get; set; }

    public Tryable(Func<TValue> function) {
        Function = function;
    }

    public TValue? Resolve() {
        try {
            return Function();
        }
        catch (Exception ex) {
            foreach (var errorHandler in ErrorHandlers) {
                var result = errorHandler.Handle(ex);

                if (result.IsHandled) {
                    return result.Value;
                }
            }

            if (DefaultErrorHandler != null) {
                return DefaultErrorHandler();
            }

            throw;
        }
        finally {
            CompleteHandler?.Invoke();
        }
    }
}
