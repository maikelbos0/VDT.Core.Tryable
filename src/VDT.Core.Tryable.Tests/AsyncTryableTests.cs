using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class AsyncTryableTests {
    [Fact]
    public async Task ReturnsFunctionValueOnSuccess() {
        var errorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        errorHandler.Handle(Arg.Any<Exception>()).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(7)));
        var defaultErrorHandler = Substitute.For<Func<Task<int>>>();
        defaultErrorHandler.Invoke().Returns(10);
        var subject = new AsyncTryable<int>(() => Task.FromResult(5)) {
            ErrorHandlers = {
                errorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject;

        Assert.Equal(5, result);

        errorHandler.DidNotReceiveWithAnyArgs().Handle(default!);
        await defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke();
    }

    [Fact]
    public async Task ThrowsOnErrorWithoutErrorHandlers() {
        var subject = new AsyncTryable<int>(() => throw new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await subject);
    }

    [Fact]
    public async Task ThrowsOnErrorWithoutMatchingErrorHandlers() {
        var skippedErrorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        skippedErrorHandler.Handle(Arg.Any<Exception>()).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var subject = new AsyncTryable<int>(() => throw new Exception()) {
            ErrorHandlers = {
                skippedErrorHandler
            }
        };

        await Assert.ThrowsAsync<Exception>(async () => await subject);
    }

    [Fact]
    public async Task UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<Task<int>>>();
        defaultErrorHandler.Invoke().Returns(10);

        var subject = new AsyncTryable<int>(() => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject;

        Assert.Equal(10, result);
    }

    [Fact]
    public async Task UsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        skippedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var defaultErrorHandler = Substitute.For<Func<Task<int>>>();
        defaultErrorHandler.Invoke().Returns(10);

        var subject = new AsyncTryable<int>(() => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject;

        Assert.Equal(10, result);

        skippedErrorHandler.Received().Handle(exception);
        await defaultErrorHandler.Received().Invoke();
    }

    [Fact]
    public async Task UsesFirstErrorHandlerThatReturnsHandledResultOnError() {
        var exception = new Exception();
        var skippedErrorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        skippedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<Task<int>>(false, default));
        var usedErrorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        usedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(7)));
        var unusedErrorHandler = Substitute.For<IErrorHandler<Task<int>>>();
        unusedErrorHandler.Handle(exception).Returns(new ErrorHandlerResult<Task<int>>(true, Task.FromResult(8)));
        var defaultErrorHandler = Substitute.For<Func<Task<int>>>();

        var subject = new AsyncTryable<int>(() => throw exception) {
            ErrorHandlers = {
                skippedErrorHandler,
                usedErrorHandler,
                unusedErrorHandler
            },
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject;

        Assert.Equal(7, result);

        skippedErrorHandler.Received().Handle(exception);
        usedErrorHandler.Received().Handle(exception);
        unusedErrorHandler.DidNotReceiveWithAnyArgs().Handle(default!);
        await defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke();
    }

    [Fact]
    public async Task ExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Action>();

        var subject = new AsyncTryable<int>(() => Task.FromResult(5)) {
            CompleteHandler = completeHandler
        };

        var result = await subject;

        completeHandler.Received().Invoke();
    }

    [Fact]
    public async Task ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Action>();

        var subject = new AsyncTryable<int>(() => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        await Assert.ThrowsAsync<Exception>(async () => await subject);

        completeHandler.Received().Invoke();
    }
}
