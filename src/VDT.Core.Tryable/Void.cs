namespace VDT.Core.Tryable;

public class Void {
    public static Void Instance { get; } = new Void();

    private Void() { }
}
