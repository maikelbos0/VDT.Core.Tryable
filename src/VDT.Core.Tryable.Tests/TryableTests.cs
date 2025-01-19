using System;
using NSubstitute;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {
    [Fact]
    public void ReturnsFunctionValueOnSuccess() {
        var errorHandler = Substitute.For<IErrorHandler<int>>();
        errorHandler.Handle(Arg.Any<Exception>()).Returns(new ErrorHandlerResult<int>(true, 7));
        var defaultErrorHandler = Substitute.For<Func<int>>();
        defaultErrorHandler.Invoke().Returns(10);
        var subject = new Tryable<int>(() => 5) {
            ErrorHandlers = {
                errorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(Void.Instance);

        Assert.Equal(5, result);

        errorHandler.DidNotReceiveWithAnyArgs().Handle(default!);
        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke();
    }

    [Fact]
    public void ThrowsOnErrorWithoutErrorHandlers() {
        var subject = new Tryable<int>(() => throw new Exception());

        Assert.Throws<Exception>(() => subject.Execute(Void.Instance));
    }

    [Fact]
    public void ThrowsOnErrorWithoutMatchingErrorHandlers() {
        var skippedErrorHandler = Substitute.For<IErrorHandler<int>>();
        skippedErrorHandler.Handle(Arg.Any<Exception>()).Returns(new ErrorHandlerResult<int>(false, default));
        var subject = new Tryable<int>(() => throw new Exception()) {
            ErrorHandlers = {
                skippedErrorHandler
            }
        };

        Assert.Throws<Exception>(() => subject.Execute(Void.Instance));
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        defaultErrorHandler.Invoke().Returns(10);

        var subject = new Tryable<int>(() => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(Void.Instance);

        Assert.Equal(10, result);
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int>>();
        skippedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<int>(false, default));
        var defaultErrorHandler = Substitute.For<Func<int>>();
        defaultErrorHandler.Invoke().Returns(10);

        var subject = new Tryable<int>(() => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(Void.Instance);

        Assert.Equal(10, result);

        skippedErrorHandler.Received().Handle(exception);
        defaultErrorHandler.Received().Invoke();
    }

    [Fact]
    public void UsesFirstErrorHandlerThatReturnsHandledResultOnError() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int>>();
        skippedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<int>(false, default));
        var usedErrorHandler = Substitute.For<IErrorHandler<int>>();
        usedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<int>(true, 7));
        var unusedErrorHandler = Substitute.For<IErrorHandler<int>>();
        unusedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<int>(true, 8));
        var defaultErrorHandler = Substitute.For<Func<int>>();

        var subject = new Tryable<int>(() => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler,
                usedErrorHandler,
                unusedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(Void.Instance);

        Assert.Equal(7, result);

        skippedErrorHandler.Received().Handle(exception);
        usedErrorHandler.Received().Handle(exception);
        unusedErrorHandler.DidNotReceiveWithAnyArgs().Handle(default!);
        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke();
    }

    [Fact]
    public void ExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Action>();
        
        var subject = new Tryable<int>(() => 5) {
            CompleteHandler = completeHandler
        };

        var result = subject.Execute(Void.Instance);

        completeHandler.Received().Invoke();
    }

    [Fact]
    public void ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Action>();

        var subject = new Tryable<int>(() => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        Assert.Throws<Exception>(() => subject.Execute(Void.Instance));

        completeHandler.Received().Invoke();
    }
}
