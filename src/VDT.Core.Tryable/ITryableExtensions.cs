namespace VDT.Core.Tryable;

public static class ITryableExtensions {
    public static TOut Execute<TOut>(this ITryable<Void, TOut> tryableBase) => tryableBase.Execute(Void.Instance);
}
