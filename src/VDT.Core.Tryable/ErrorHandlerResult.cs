using System.Diagnostics.CodeAnalysis;

namespace VDT.Core.Tryable;

public record ErrorHandlerResult<TOut> ([property:MemberNotNullWhen(true, nameof(ErrorHandlerResult<TOut>.Value))] bool IsHandled, TOut? Value) {
    public static ErrorHandlerResult<TOut> Skipped => new(false, default);

    public static ErrorHandlerResult<TOut> Handled(TOut value) => new(true, value);
};
