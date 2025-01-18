using System;

namespace VDT.Core.Tryable;

public class ErrorHandler<TException, TOut> : IErrorHandler<TOut> where TException : Exception {
    public Func<TException, TOut> Handler { get; set; }
    public Func<TException, bool>? Filter { get; set; }

    public ErrorHandler(Func<TException, TOut> handler) : this(null, handler) { }

    public ErrorHandler(Func<TException, bool>? filter, Func<TException, TOut> handler) {
        Filter = filter;
        Handler = handler;
    }

    public ErrorHandlerResult<TOut> Handle(Exception ex) {
        if (ex is not TException exception) {
            return ErrorHandlerResult<TOut>.Skipped;
        }

        if (Filter != null && !Filter(exception)) {
            return ErrorHandlerResult<TOut>.Skipped;
        }

        return ErrorHandlerResult<TOut>.Handled(Handler(exception));
    }
}
