using System;

namespace VDT.Core.Tryable;

public static class TryableBuilder {
    public static Tryable<TValue> Try<TValue>(Func<TValue> function) => new(function);
}
