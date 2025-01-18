﻿using System;
using System.Threading.Tasks;

namespace VDT.Core.Tryable;

public class AsyncTryable<TValue> : TryableBase<Task<TValue>, Func<Task>, AsyncTryable<TValue>> {
    public AsyncTryable(Func<Task<TValue>> function) : base(function) { }

    public override async Task<TValue> Execute() {
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
