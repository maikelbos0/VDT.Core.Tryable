using System.Threading.Tasks;
using Xunit;

namespace VDT.Core.Tryable.Tests;

// TODO fix
public class TryableBaseExtensionsTests {
    [Fact]
    public void ExecuteVoidTryable() {
        var subject = new Tryable<Void, int>(_ => 5);

        var result = subject.Execute();

        Assert.Equal(5, result);
    }

    //[Fact]
    //public async Task ExecuteVoidAsyncTryable() {
    //    var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

    //    var result = await subject.Execute();

    //    Assert.Equal(5, result);
    //}
}
