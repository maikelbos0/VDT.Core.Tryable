using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class AsyncTryableTests {
    [Fact]
    public async Task ReturnsFunctionValueOnSuccess() {
        var errorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        errorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(7)));
        var defaultErrorHandler = Substitute.For<Func<int, Task<int>>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);
        var subject = new AsyncTryable<int, int>(n => Task.FromResult(n * 2)) {
            ErrorHandlers = {
                errorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject.Execute(5);

        Assert.Equal(10, result);

        errorHandler.DidNotReceiveWithAnyArgs().Handle(default!, default);
        await defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public async Task ThrowsOnErrorWithoutErrorHandlers() {
        var subject = new AsyncTryable<int, int>(_ => throw new Exception());

        await Assert.ThrowsAsync<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public async Task ThrowsOnErrorWithoutMatchingErrorHandlers() {
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        skippedErrorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var subject = new AsyncTryable<int, int>(_ => throw new Exception()) {
            ErrorHandlers = {
                skippedErrorHandler
            }
        };

        await Assert.ThrowsAsync<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public async Task UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int, Task<int>>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new AsyncTryable<int, int>(_ => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject.Execute(5);

        Assert.Equal(15, result);
    }

    [Fact]
    public async Task UsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        skippedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var defaultErrorHandler = Substitute.For<Func<int, Task<int>>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new AsyncTryable<int, int>(_ => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject.Execute(5);

        Assert.Equal(15, result);

        skippedErrorHandler.Received().Handle(exception, 5);
        await defaultErrorHandler.Received().Invoke(5);
    }

    [Fact]
    public async Task UsesFirstErrorHandlerThatReturnsHandledResultOnError() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        skippedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var usedErrorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        usedErrorHandler.Handle(exception, 5).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(7)));
        var unusedErrorHandler = Substitute.For<IErrorHandler<int, Task<int>>>();
        unusedErrorHandler.Handle(Arg.Any<Exception>(), Arg.Any<int>()).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(8)));
        var defaultErrorHandler = Substitute.For<Func<int, Task<int>>>();

        var subject = new AsyncTryable<int, int>(_ => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler,
                usedErrorHandler,
                unusedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject.Execute(5);

        Assert.Equal(7, result);

        skippedErrorHandler.Received().Handle(exception, 5);
        usedErrorHandler.Received().Handle(exception, 5);
        unusedErrorHandler.DidNotReceiveWithAnyArgs().Handle(default!, default);
        await defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke(default);
    }

    [Fact]
    public async Task ExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Func<Task>>();

        var subject = new AsyncTryable<int, int>(n => Task.FromResult(n * 2)) {
            CompleteHandler = completeHandler
        };

        var result = await subject.Execute(5);

        await completeHandler.Received().Invoke();
    }

    [Fact]
    public async Task ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Func<Task>>();

        var subject = new AsyncTryable<int, int>(_ => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        await Assert.ThrowsAsync<Exception>(() => subject.Execute(5));

        await completeHandler.Received().Invoke();
    }
}
