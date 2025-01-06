using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {
    [Fact]
    public void CatchAddsErrorHandlerWithoutFilter() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var subject = new Tryable<int>(() => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, int>>(subject.ErrorHandlers[1]);
        Assert.Equal(handler, errorHandler.Handler);
        Assert.Null(errorHandler.Filter);
    }

    [Fact]
    public void CatchAddsErrorHandlerWithFilter() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, bool>>();
        var subject = new Tryable<int>(() => 5) {
            ErrorHandlers = {
                Substitute.For<IErrorHandler<int>>()
            }
        };

        Assert.Equal(subject, subject.Catch(filter, handler));

        Assert.Equal(2, subject.ErrorHandlers.Count);

        var errorHandler = Assert.IsType<ErrorHandler<InvalidOperationException, int>>(subject.ErrorHandlers[1]);
        Assert.Equal(handler, errorHandler.Handler);
        Assert.Equal(filter, errorHandler.Filter);
    }

    [Fact]
    public void CatchAddsDefaultErrorHandler() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        var subject = new Tryable<int>(() => 5);

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.Equal(defaultErrorHandler, subject.DefaultErrorHandler);
    }

    [Fact]
    public void Finally() {
        var completeHandler = Substitute.For<Action>();
        var subject = new Tryable<int>(() => 5);

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.Equal(completeHandler, subject.CompleteHandler);
    }

    [Fact]
    public void ReturnsFunctionValueOnSuccess() {
        var errorHandler = Substitute.For<IErrorHandler<int>>();
        errorHandler.Handle(Arg.Any<Exception>()).Returns(new ErrorHandlerResult<int>(true, 7));
        var defaultErrorHandler = Substitute.For<Func<int>>();
        defaultErrorHandler.Invoke().Returns(10);
        var subject = new Tryable<int>(() => 5);

        var result = (int)subject;

        Assert.Equal(5, result);

        errorHandler.DidNotReceiveWithAnyArgs().Handle(default!);
        defaultErrorHandler.DidNotReceiveWithAnyArgs().Invoke();
    }

    [Fact]
    public void ThrowsOnErrorWithoutErrorHandlers() {
        var subject = new Tryable<int>(() => throw new Exception());

        Assert.Throws<Exception>(() => (int)subject);
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

        Assert.Throws<Exception>(() => (int)subject);
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        defaultErrorHandler.Invoke().Returns(10);

        var subject = new Tryable<int>(() => throw new Exception()) {
            DefaultErrorHandler = defaultErrorHandler
        };

        var result = (int)subject;

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

        var result = (int)subject;

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

        var result = (int)subject;

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

        var result = (int)subject;

        completeHandler.Received().Invoke();
    }

    [Fact]
    public void ExecutesCompleteHandlerOnError() {
        var completeHandler = Substitute.For<Action>();

        var subject = new Tryable<int>(() => throw new Exception()) {
            CompleteHandler = completeHandler
        };

        Assert.Throws<Exception>(() => (int)subject);

        completeHandler.Received().Invoke();
    }
}
