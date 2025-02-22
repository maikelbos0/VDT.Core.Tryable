using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class AsyncTryableTests {
    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithoutFilterWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithInValueWithoutFilterWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithoutFilterWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Task<int>>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithInValueWithoutFilterWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, Task<int>>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception, Void.Instance);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithFilterWithoutInValueWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithFilterWithInValueWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithInValueWithFilterWithoutInValueWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public async Task CatchAddsErrorHandleWithInValueWithFilterWithInValueWithoutReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithFilterWithoutInValueWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Task<int>>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithoutInValueWithFilterWithInValueWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Task<int>>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public async Task CatchAddsErrorHandlerWithInValueWithFilterWithoutInValueWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, Task<int>>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task <int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public async Task CatchAddsErrorHandleWithInValueWithFilterWithInValueWithReturnTask() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, Task<int>>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5)) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, Task<int>>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, Task<int>>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        await errorHandler.Handler.Invoke(exception, Void.Instance);
        await handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public async Task CatchAddsDefaultErrorHandlerWithoutInValueWithoutReturnTask() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        await subject.DefaultErrorHandler.Invoke(Void.Instance);
        defaultErrorHandler.Received().Invoke();
    }

    [Fact]
    public async Task CatchAddsDefaultErrorHandlerWithInValueWithoutReturnTask() {
        var defaultErrorHandler = Substitute.For<Func<Void, int>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        await subject.DefaultErrorHandler.Invoke(Void.Instance);
        defaultErrorHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public async Task CatchAddsDefaultErrorHandlerWithoutInValueWithReturnTask() {
        var defaultErrorHandler = Substitute.For<Func<Task<int>>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        await subject.DefaultErrorHandler.Invoke(Void.Instance);
        await defaultErrorHandler.Received().Invoke();
    }

    [Fact]
    public async Task CatchAddsDefaultErrorHandlerWithInValueWithReturnTask() {
        var defaultErrorHandler = Substitute.For<Func<Void, Task<int>>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        await subject.DefaultErrorHandler.Invoke(Void.Instance);
        await defaultErrorHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public async Task FinallyAddsCompleteHandlerWithoutInValueWithoutReturnTask() {
        var completeHandler = Substitute.For<Action>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        await subject.CompleteHandler.Invoke(Void.Instance);
        completeHandler.Received().Invoke();
    }

    [Fact]
    public async Task FinallyAddsCompleteHandlerWithInValueWithoutReturnTask() {
        var completeHandler = Substitute.For<Action<Void>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        await subject.CompleteHandler.Invoke(Void.Instance);
        completeHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public async Task FinallyAddsCompleteHandlerWithoutInValueWithReturnTask() {
        var completeHandler = Substitute.For<Func<Task>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        await subject.CompleteHandler.Invoke(Void.Instance);
        await completeHandler.Received().Invoke();
    }

    [Fact]
    public async Task FinallyAddsCompleteHandlerWithInValueWithReturnTask() {
        var completeHandler = Substitute.For<Func<Void, Task>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        await subject.CompleteHandler.Invoke(Void.Instance);
        await completeHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public async Task ExecuteReturnsFunctionValueOnSuccess() {
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
    public async Task ExecuteThrowsOnErrorWithoutErrorHandlers() {
        var subject = new AsyncTryable<int, int>(_ => throw new Exception());

        await Assert.ThrowsAsync<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public async Task ExecuteThrowsOnErrorWithoutMatchingErrorHandlers() {
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
    public async Task ExecuteUsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int, Task<int>>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new AsyncTryable<int, int>(_ => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = await subject.Execute(5);

        Assert.Equal(15, result);
    }

    [Fact]
    public async Task ExecuteUsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
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
    public async Task ExecuteUsesFirstErrorHandlerThatReturnsHandledResultOnError() {
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
    public async Task ExecuteExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Func<int, Task>>();

        var subject = new AsyncTryable<int, int>(n => Task.FromResult(n * 2)) {
            CompleteHandler = completeHandler
        };

        var result = await subject.Execute(5);

        await completeHandler.Received().Invoke(5);
    }

    [Fact]
    public async Task ExecuteExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Func<int, Task>>();

        var subject = new AsyncTryable<int, int>(_ => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        await Assert.ThrowsAsync<Exception>(() => subject.Execute(5));

        await completeHandler.Received().Invoke(5);
    }
}
