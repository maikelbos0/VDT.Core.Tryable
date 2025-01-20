using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableBaseTests {
    public class TestTryable : TryableBase<Void, int, Action> {
        public TestTryable() : base(_ => throw new NotImplementedException()) { }

        public override int Execute(Void _) => throw new NotImplementedException();
    }

    [Fact]
    public void CatchAddsErrorHandlerWithoutInValueWithoutFilter() {
        var handler = Substitute.For<Func<InvalidOperationException, int>>();
        var subject = new TestTryable() {
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
        var subject = new TestTryable() {
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
        var subject = new TestTryable() {
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
        var subject = new TestTryable() {
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
        var subject = new TestTryable() {
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
    public void CatchAddsErrorHandleWithInValuerWithFilterWithInValue() {
        var handler = Substitute.For<Func<InvalidOperationException, Void, int>>();
        var filter = Substitute.For<Func<InvalidOperationException, Void, bool>>();
        var subject = new TestTryable() {
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
    public void CatchAddsDefaultErrorHandler() {
        var defaultErrorHandler = Substitute.For<Func<int>>();
        var subject = new TestTryable();

        Assert.Equal(subject, subject.Catch(defaultErrorHandler));

        Assert.Equal(defaultErrorHandler, subject.DefaultErrorHandler);
    }

    [Fact]
    public void Finally() {
        var completeHandler = Substitute.For<Action>();
        var subject = new TestTryable();

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.Equal(completeHandler, subject.CompleteHandler);
    }
}
