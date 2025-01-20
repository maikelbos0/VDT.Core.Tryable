using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public class AsyncTryable<TIn, TOut> : TryableBase<TIn, Task<TOut>, Func<Task>> {
    public AsyncTryable(Func<TIn, Task<TOut>> function) : base(function) { }

    public override async Task<TOut> Execute(TIn value) {
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
                return await DefaultErrorHandler();
            }

            throw;
        }
        finally {
            if (CompleteHandler != null) {
                await CompleteHandler.Invoke();
            }
        }
    }
}
