using Xunit;

namespace VDT.Core.Tryable.Tests;

public class ErrorHandlerResultTests {
    [Fact]
    public void Skipped() {
        var subject = ErrorHandlerResult<int>.Skipped;

        Assert.False(subject.IsHandled);
        Assert.Equal(0, subject.Value);
    }

    [Fact]
    public void Handled() {
        var subject = ErrorHandlerResult<int>.Handled(5);

        Assert.True(subject.IsHandled);
        Assert.Equal(5, subject.Value);
    }
}
