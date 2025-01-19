using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public class AsyncTryable<TOut> : TryableBase<Void, Task<TOut>, Func<Task>> {
    public AsyncTryable(Func<Task<TOut>> function) : base(function) { }

    public override async Task<TOut> Execute(Void value) {
        try {
            return await Function();
        }
        catch (Exception ex) {
            foreach (var errorHandler in ErrorHandlers) {
                var result = errorHandler.Handle(ex);

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
