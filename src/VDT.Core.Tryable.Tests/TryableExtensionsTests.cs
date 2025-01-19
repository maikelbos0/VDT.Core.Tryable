using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableExtensionsTests {
    [Fact]
    public void Execute() {
        var subject = new Tryable<Void, int>(_ => 5);

        var result = subject.Execute();

        Assert.Equal(5, result);
    }
}
