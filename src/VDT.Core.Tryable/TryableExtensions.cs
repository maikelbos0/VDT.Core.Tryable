namespace VDT.Core.Tryable;

public static class TryableExtensions {
    public static TOut Execute<TOut>(this Tryable<Void, TOut> tryable) => tryable.Execute(Void.Instance);
}
