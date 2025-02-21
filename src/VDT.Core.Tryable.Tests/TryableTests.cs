using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {

    [Fact]
    public void CatchAddsErrorHandlerWithoutInValueWithoutFilter() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public void CatchAddsErrorHandlerWithInValueWithoutFilter() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public void CatchAddsErrorHandlerWithoutInValueWithFilterWithoutInValue() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public void CatchAddsErrorHandlerWithoutInValueWithFilterWithInValue() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public void CatchAddsErrorHandlerWithInValueWithFilterWithoutInValue() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception);
    }

    [Fact]
    public void CatchAddsErrorHandleWithInValueWithFilterWithInValue() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new Tryable<Void, int>(_ => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<Void, int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, Void, int>>(subject.ErrorHandlers[1]);
        var exception = new InvalidOperationException();

        errorHandler.Handler.Invoke(exception, Void.Instance);
        handler.Received().Invoke(exception, Void.Instance);

        Assert.NotNull(errorHandler.Filter);
        errorHandler.Filter.Invoke(exception, Void.Instance);
        filter.Received().Invoke(exception, Void.Instance);
    }

    [Fact]
    public void CatchAddsDefaultErrorHandlerWithoutInValue() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        var subject = new Tryable<Void, int>(_ => 5);

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        subject.DefaultErrorHandler.Invoke(Void.Instance);
        defaultErrorHandler.Received().Invoke();
    }

    [Fact]
    public void CatchAddsDefaultErrorHandlerWithInValue() {
        var defaultErrorHandler = Substitute.For<Func<Void, int>>();
        var subject = new Tryable<Void, int>(_ => 5);

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.NotNull(subject.DefaultErrorHandler);
        subject.DefaultErrorHandler.Invoke(Void.Instance);
        defaultErrorHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public void FinallyWithoutInValue() {
        var completeHandler = Substitute.For<Action>();
        var subject = new Tryable<Void, int>(_ => 5);

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        subject.CompleteHandler.Invoke(Void.Instance);
        completeHandler.Received().Invoke();
    }

    [Fact]
    public void FinallyWithInValue() {
        var completeHandler = Substitute.For<Action<Void>>();
        var subject = new Tryable<Void, int>(_ => 5);

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        subject.CompleteHandler.Invoke(Void.Instance);
        completeHandler.Received().Invoke(Void.Instance);
    }

    [Fact]
    public void ExecuteReturnsFunctionValueOnSuccess() {
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
    public void ExecuteThrowsOnErrorWithoutErrorHandlers() {
        var subject = new Tryable<int, int>(_ => throw new Exception());

        Assert.Throws<Exception>(() => subject.Execute(5));
    }

    [Fact]
    public void ExecuteThrowsOnErrorWithoutMatchingErrorHandlers() {
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
    public void ExecuteUsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int, int>>();
        defaultErrorHandler.Invoke(Arg.Any<int>()).Returns(15);

        var subject = new Tryable<int, int>(_ => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = subject.Execute(5);

        Assert.Equal(15, result);
    }

    [Fact]
    public void ExecuteUsesDefaultErrorHandlerOnErrorWithoutMatchingErrorHandlers() {
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
    public void ExecuteUsesFirstErrorHandlerThatReturnsHandledResultOnError() {
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
    public void ExecuteExecutesCompleteHandlerOnSucces() {
        var completeHandler = Substitute.For<Action<int>>();

        var subject = new Tryable<int, int>(n => n * 2) {
            CompleteHandler = completeHandler
        };

        var result = subject.Execute(5);

        completeHandler.Received().Invoke(5);
    }

    [Fact]
    public void ExecuteExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Action<int>>();

        var subject = new Tryable<int, int>(_ => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        Assert.Throws<Exception>(() => subject.Execute(5));

        completeHandler.Received().Invoke(5);
    }
}
