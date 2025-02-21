using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {
    [Fact]
    public void ReturnsFunctionValueOnSuccess() {
        var errorHandler = Substitute.For<IErrorHandler<int, int>>();
        errorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<int>(true, 7));
        var defaultErrorHandler = Substitute.For<Func<int, int>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);
        var subject = new Tryable<int, int>(n => n * 2) {
            ErrorHandlers = {
                errorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(5);

        Assert.Equal(10, result);

        errorHandler.DidNotReceiveWithAnyArgs().Handle(default!, default);
        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void ThrowsOnErrorWithoutErrorHandlers() {
        var subject = new Tryable<int, int>(_ => throw new Exception());

        Assert.Throws<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public void ThrowsOnErrorWithoutMatchingErrorHandlers() {
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, int>>();
        skippedErrorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<int>(false, default));
        var subject = new Tryable<int, int>(_ => throw new Exception()) {
            ErrorHandlers = {
                skippedErrorHandler
            }
        };

        Assert.Throws<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int, int>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new Tryable<int, int>(_ => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(5);

        Assert.Equal(15, result);
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, int>>();
        skippedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<int>(false, default));
        var defaultErrorHandler = Substitute.For<Func<int, int>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new Tryable<int, int>(_ => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(5);

        Assert.Equal(15, result);

        skippedErrorHandler.Received().Handle(exception, 5);
        defaultErrorHandler.Received().Invoke(5);
    }

    [Fact]
    public void UsesFirstErrorHandlerThatReturnsHandledResultOnError() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, int>>();
        skippedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<int>(false, default));
        var usedErrorHandler = Substitute.For<IErrorHandler<int, int>>();
        usedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<int>(true, 7));
        var unusedErrorHandler = Substitute.For<IErrorHandler<int, int>>();
        unusedErrorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<int>(true, 8));
        var defaultErrorHandler = Substitute.For<Func<int, int>>();

        var subject = new Tryable<int, int>(_ => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler,
                usedErrorHandler,
                unusedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(5);

        Assert.Equal(7, result);

        skippedErrorHandler.Received().Handle(exception, 5);
        usedErrorHandler.Received().Handle(exception, 5);
        unusedErrorHandler.DidNotReceiveWithAnyArgs().Handle(default!, default);
        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public void ExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Func<int, Void>>();

        var subject = new Tryable<int, int>(n => n * 2) {
            CompleteHandler = completeHandler
        };

        var result = subject.Execute(5);

        completeHandler.Received().Invoke(5);
    }

    [Fact]
    public void ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Func<int, Void>>();

        var subject = new Tryable<int, int>(_ => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        Assert.Throws<Exception>(() => subject.Execute(5));

        completeHandler.Received().Invoke(5);
    }
}
