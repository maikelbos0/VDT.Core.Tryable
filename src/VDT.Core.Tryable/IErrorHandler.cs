using System;

namespace VDT.Core.Tryable;

public interface IErrorHandler<TOut> {
    ErrorHandlerResult<TOut> Handle(Exception ex);
}
