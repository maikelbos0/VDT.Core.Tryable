using System;

namespace VDT.Core.Tryable;

public class Tryable<TValue> : TryableBase<TValue, Action, Tryable<TValue>> {
    public static implicit operator TValue(Tryable<TValue> tryable) => tryable.Execute();

    public Tryable(Func<TValue> function) : base(function) {
        Function = function;
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
