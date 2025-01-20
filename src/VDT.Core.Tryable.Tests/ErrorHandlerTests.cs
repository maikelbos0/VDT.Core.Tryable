using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class ErrorHandlerTests {
    [Fact]
    public void DoesNotHandleWrongExceptionType() {
        var subject = new ErrorHandler<InvalidOperationException, int, int>((ex, n) => n * 2);

        var result = subject.Handle(new Exception(), 5);

        Assert.False(result.IsHandled);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void DoesNotHandleWhenFilterReturnsFalse() {
        var subject = new ErrorHandler<Exception, int, int>((ex, n) => n % 2 == 0, (ex, n) => n * 2);

        var result = subject.Handle(new Exception(), 5);

        Assert.False(result.IsHandled);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void HandlesCorrectExceptionTypeWithoutFilter() {
        var subject = new ErrorHandler<InvalidOperationException, int, int>((ex, n) => n * 2);

        var result = subject.Handle(new InvalidOperationException(), 5);

        Assert.True(result.IsHandled);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void HandlesCorrectExceptionTypeWhenFilterReturnsTrue() {
        var subject = new ErrorHandler<Exception, int, int>((ex, n) => n % 2 != 0, (ex, n) => n * 2);

        var result = subject.Handle(new Exception(), 5);

        Assert.True(result.IsHandled);
        Assert.Equal(10, result.Value);
    }
}
