namespace VDT.Core.Tryable;

public static class TryableBaseExtensions {
    public static TOut Execute<TOut, TFinally>(this TryableBase<Void, TOut, TFinally> tryableBase) => tryableBase.Execute(Void.Instance);
}
