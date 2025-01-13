using System;

namespace VDT.Core.Tryable;

public class Tryable<TValue> : TryableBase<TValue, Action> {
    public static implicit operator TValue(Tryable<TValue> tryable) => tryable.Execute();

    public Tryable(Func<TValue> function) : base(function) {
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

    public override TValue Execute() {
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
