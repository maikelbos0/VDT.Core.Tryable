using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {
    [Fact]
    public void ReturnsFunctionValueOnSuccess() {
        var defaultErrorHandler = Substitute.For<Func<Exception, int>>();
        var subject = new Tryable<int>(() => 5, defaultErrorHandler);

        var result = subject.Resolve();

        Assert.Equal(5, result);

        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke(default!);
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var exception = new Exception();
        var defaultErrorHandler = Substitute.For<Func<Exception, int>>();
        defaultErrorHandler.Invoke(exception).Returns(10);

        var subject = new Tryable<int>(() => throw exception, defaultErrorHandler);

        var result = subject.Resolve();

        Assert.Equal(10, result);
    }

    [Fact]
    public void ExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Action>();
        
        var subject = new Tryable<int>(() => 5, ex => 10) {
            CompleteHandler = completeHandler
        };

        subject.Resolve();

        completeHandler.Received().Invoke();
    }

    [Fact]
    public void ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Action>();

        var subject = new Tryable<int>(() => throw new Exception(), ex => 10) {
            CompleteHandler = completeHandler
        };

        subject.Resolve();

        completeHandler.Received().Invoke();
    }
}
