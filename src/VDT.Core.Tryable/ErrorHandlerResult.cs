using System.Diagnostics.CodeAnalysis;

namespace VDT.Core.Tryable;

public record ErrorHandlerResult<TValue> ([property:MemberNotNullWhen(true, nameof(ErrorHandlerResult<TValue>.Value))] bool IsHandled, TValue? Value) {
    public static ErrorHandlerResult<TValue> Skipped => new(false, default);

    public static ErrorHandlerResult<TValue> Handled(TValue value) => new(true, value);
};
