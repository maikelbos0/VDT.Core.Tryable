using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public class AsyncTryable<TIn, TOut> : ITryable<TIn, Task<TOut>> {
    public Func<TIn, Task<TOut>> Function { get; set; }
    public IList<IErrorHandler<TIn, Task<TOut>>> ErrorHandlers { get; set; } = [];
    public Func<TIn, Task<TOut>>? DefaultErrorHandler { get; set; }
    public Func<TIn, Task>? CompleteHandler { get; set; }

    public AsyncTryable(Func<TIn, Task<TOut>> function) {
        Function = function;
    }

    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, Task<TOut>> handler) where TException : Exception
        => Catch((TException exception, TIn _) => handler(exception));

    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, TIn, Task<TOut>> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TIn, Task<TOut>>(handler));
        return this;
    }
    
    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, bool> filter, Func<TException, Task<TOut>> handler) where TException : Exception
        => Catch((TException exception, TIn _) => filter(exception), (TException exception, TIn _) => handler(exception));

    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, TIn, bool> filter, Func<TException, Task<TOut>> handler) where TException : Exception
        => Catch(filter, (TException exception, TIn _) => handler(exception));

    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, bool> filter, Func<TException, TIn, Task<TOut>> handler) where TException : Exception
        => Catch((TException exception, TIn _) => filter(exception), handler);

    public AsyncTryable<TIn, TOut> Catch<TException>(Func<TException, TIn, bool> filter, Func<TException, TIn, Task<TOut>> handler) where TException : Exception {
        ErrorHandlers.Add(new ErrorHandler<TException, TIn, Task<TOut>>(filter, handler));
        return this;
    }

    public AsyncTryable<TIn, TOut> Catch(Func<Task<TOut>> defaultErrorHandler)
        => Catch(value => defaultErrorHandler());

    public AsyncTryable<TIn, TOut> Catch(Func<TIn, Task<TOut>> defaultErrorHandler) {
        DefaultErrorHandler = defaultErrorHandler;
        return this;
    }

    public AsyncTryable<TIn, TOut> Finally(Func<TIn, Task> completeHandler) {
        CompleteHandler = completeHandler;
        return this;
    }

    public async Task<TOut> Execute(TIn value) {
        try {
            return await Function(value);
        }
        catch (Exception ex) {
            foreach (var errorHandler in ErrorHandlers) {
                var result = errorHandler.Handle(ex, value);

                if (result.IsHandled) {
                    return await result.Value;
                }
            }

            if (DefaultErrorHandler != null) {
                return await DefaultErrorHandler(value);
            }

            throw;
        }
        finally {
            if (CompleteHandler != null) {
                await CompleteHandler.Invoke(value);
            }
        }
    }
}
