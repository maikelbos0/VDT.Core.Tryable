namespace VDT.Core.Tryable;

public interface ITryable<TIn, TOut> {
    TOut Execute(TIn value);
}
