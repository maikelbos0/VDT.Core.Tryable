using System;

namespace VDT.Core.Tryable;

public class ErrorHandler<TException, TValue> : IErrorHandler<TValue> where TException : Exception {
    public Func<TException, TValue> Function { get; set; }
    public Func<TException, bool>? Filter { get; set; }

    public ErrorHandler(Func<TException, TValue> function) : this(function, null) { }

    public ErrorHandler(Func<TException, TValue> function, Func<TException, bool>? filter) {
        Function = function;
        Filter = filter;
    }

    public ErrorHandlerResult<TValue> Handle(Exception ex) {
        if (ex is not TException exception) {
            return ErrorHandlerResult<TValue>.Skipped;
        }

        if (Filter != null && !Filter(exception)) {
            return ErrorHandlerResult<TValue>.Skipped;
        }

        return ErrorHandlerResult<TValue>.Handled(Function(exception));
    }
}
