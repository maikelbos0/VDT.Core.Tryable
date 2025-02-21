using System;
using System.Collections.Generic;

namespace VDT.Core.Tryable;

public class Tryable<TIn, TOut> : ITryable<TIn, TOut> {
    public Func<TIn, TOut> Function { get; set; }
    public IList<IErrorHandler<TIn, TOut>> ErrorHandlers { get; set; } = [];
    public Func<TIn, TOut>? DefaultErrorHandler { get; set; }
    public Action<TIn>? CompleteHandler { get; set; }

    public Tryable(Func<TIn, TOut> function) {
        Function = function;
    }

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, TOut> handler) where TException : Exception
        => Catch((TException exception, TIn _) => handler(exception));

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, TIn, TOut> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TIn, TOut>(handler));
        return this;
    }

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, bool> filter, Func<TException, TOut> handler) where TException : Exception
        => Catch((TException exception, TIn _) => filter(exception), (TException exception, TIn _) => handler(exception));

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, TIn, bool> filter, Func<TException, TOut> handler) where TException : Exception
        => Catch(filter, (TException exception, TIn _) => handler(exception));

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, bool> filter, Func<TException, TIn, TOut> handler) where TException : Exception
        => Catch((TException exception, TIn _) => filter(exception), handler);

    public Tryable<TIn, TOut> Catch<TException>(Func<TException, TIn, bool> filter, Func<TException, TIn, TOut> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TIn, TOut>(filter, handler));
        return this;
    }

    public Tryable<TIn, TOut> Catch(Func<TOut> defaultErrorHandler)
        => Catch(value => defaultErrorHandler());

    public Tryable<TIn, TOut> Catch(Func<TIn, TOut> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return this;
    }

    public Tryable<TIn, TOut> Finally(Action completeHandler)
        => Finally(_ => completeHandler());

    public Tryable<TIn, TOut> Finally(Action<TIn> completeHandler) {
        CompleteHandler = completeHandler;
        return this;
    }

    public TOut Execute(TIn value) {
        try {
            return Function(value);
        }
        catch (Exception ex) {
            foreach (var errorHandler in ErrorHandlers) {
                var result = errorHandler.Handle(ex, value);

                if (result.IsHandled) {
                    return result.Value;
                }
            }

            if (DefaultErrorHandler != null) {
                return DefaultErrorHandler(value);
            }

            throw;
        }
        finally {
            CompleteHandler?.Invoke(value);
        }
    }
}
