using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class AsyncTryableExtensionsTests {
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
    public async Task FinallyAddsCompleteHandlerWithInValueWithoutReturnTask() {
        var completeHandler = Substitute.For<Action<Void>>();
        var subject = new AsyncTryable<Void, int>(_ => Task.FromResult(5));

        Assert.Equal(subject, subject.Finally(completeHandler));

        Assert.NotNull(subject.CompleteHandler);
        await subject.CompleteHandler.Invoke(Void.Instance);
        completeHandler.Received().Invoke(Void.Instance);
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
}
