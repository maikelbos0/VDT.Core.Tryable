using System;

namespace VDT.Core.Tryable;

public class Tryable<TOut> : TryableBase<Void, TOut, Action> {
    public Tryable(Func<TOut> function) : base(function) { }

    public override TOut Execute(Void value) {
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
