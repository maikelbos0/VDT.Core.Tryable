using System;

namespace VDT.Core.Tryable;

public interface IErrorHandler<TIn, TOut> {
    ErrorHandlerResult<TOut> Handle(Exception ex, TIn value);
}
