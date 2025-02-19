using System.Threading.Tasks;
using System;

namespace VDT.Core.Tryable;

public static class AsyncTryableExtensions {
    public static TryableBase<TIn, Task<TOut>, Func<TIn, Task>> Finally<TIn, TOut>(this TryableBase<TIn, Task<TOut>, Func<TIn, Task>> tryableBase, Func<Task> completeHandler)
        => tryableBase.Finally(_ => completeHandler());

    public static TryableBase<TIn, Task<TOut>, Func<TIn, Task>> Finally<TIn, TOut>(this TryableBase<TIn, Task<TOut>, Func<TIn, Task>> tryableBase, Action<TIn> completeHandler)
        => tryableBase.Finally(value => {
            completeHandler(value);
            return Task.CompletedTask;
        });

    public static TryableBase<TIn, Task<TOut>, Func<TIn, Task>> Finally<TIn, TOut>(this TryableBase<TIn, Task<TOut>, Func<TIn, Task>> tryableBase, Action completeHandler)
        => tryableBase.Finally(_ => {
            completeHandler();
            return Task.CompletedTask;
        });
}
