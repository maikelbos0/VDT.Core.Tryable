using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class ErrorHandlerTests {
    [Fact]
    public void DoesNotHandleWrongExceptionType() {
        var subject = new ErrorHandler<InvalidOperationException, int>(ex => 10);

        var result = subject.Handle(new Exception());

        Assert.False(result.IsHandled);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void DoesNotHandleWhenFilterReturnsFalse() {
        var subject = new ErrorHandler<Exception, int>(ex => false, ex => 10);

        var result = subject.Handle(new Exception());

        Assert.False(result.IsHandled);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void HandlesCorrectExceptionTypeWithoutFilter() {
        var subject = new ErrorHandler<InvalidOperationException, int>(ex => 10);

        var result = subject.Handle(new InvalidOperationException());

        Assert.True(result.IsHandled);
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void HandlesCorrectExceptionTypeWhenFilterReturnsTrue() {
        var subject = new ErrorHandler<Exception, int>(ex => true, ex => 10);

        var result = subject.Handle(new Exception());

        Assert.True(result.IsHandled);
        Assert.Equal(10, result.Value);
    }
}
