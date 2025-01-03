namespace VDT.Core.Tryable;

public record ErrorHandlerResult<TValue> (bool IsHandled, TValue? Value) {
    public static ErrorHandlerResult<TValue> Skipped => new(false, default);

    public static ErrorHandlerResult<TValue> Handled(TValue value) => new(true, value);
};
