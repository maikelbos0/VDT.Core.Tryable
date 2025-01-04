using System;

namespace VDT.Core.Tryable;

public interface IErrorHandler<TValue> {
    ErrorHandlerResult<TValue> Handle(Exception ex);
}
