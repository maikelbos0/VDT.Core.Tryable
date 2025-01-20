using System;

namespace VDT.Core.Tryable;

public class ErrorHandler<TException, TIn, TOut> : IErrorHandler<TIn, TOut> where TException : Exception {
    public Func<TException, TIn, TOut> Handler { get; set; }
    public Func<TException, TIn, bool>? Filter { get; set; }

    public ErrorHandler(Func<TException, TIn, TOut> handler) : this(null, handler) { }

    public ErrorHandler(Func<TException, TIn, bool>? filter, Func<TException, TIn, TOut> handler) {
        Filter = filter;
        Handler = handler;
    }

    public ErrorHandlerResult<TOut> Handle(Exception ex, TIn value) {
        if (ex is not TException exception) {
            return ErrorHandlerResult<TOut>.Skipped;
        }

        if (Filter != null && !Filter(exception, value)) {
            return ErrorHandlerResult<TOut>.Skipped;
        }

        return ErrorHandlerResult<TOut>.Handled(Handler(exception, value));
    }
}
