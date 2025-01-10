using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public class Tryable<TValue> {
    public static implicit operator TValue(Tryable<TValue> tryable) => tryable.Execute();

    public Func<TValue> Function { get; set; }
    public IList<IErrorHandler<TValue>> ErrorHandlers { get; set; } = [];
    public Func<TValue>? DefaultErrorHandler { get; set; }
    public Action? CompleteHandler { get; set; }

    public Tryable(Func<TValue> function) {
        Function = function;
    }

    public Tryable<TValue> Catch<TException>(Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(handler));
        return this;
    }

    public Tryable<TValue> Catch<TException>(Func<TException, bool> filter, Func<TException, TValue> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TValue>(filter, handler));
        return this;
    }

    public Tryable<TValue> Catch(Func<TValue> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return this;
    }

    public Tryable<TValue> Finally(Action completeHandler) {
        CompleteHandler = completeHandler;
        return this;
    }

    public virtual TValue Execute() {
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
