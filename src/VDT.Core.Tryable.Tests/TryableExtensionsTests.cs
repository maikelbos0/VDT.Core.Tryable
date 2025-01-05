using NSubstitute;
using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableExtensionsTests {
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

        Assert.Equal(subject, subject.Catch(handler, filter));

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
}
