using System;

namespace VDT.Core.Tryable;

public class Tryable<TIn, TOut> : TryableBase<TIn, TOut, Void> {
    public Tryable(Func<TIn, TOut> function) : base(function) { }

    public override TOut Execute(TIn value) {
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
