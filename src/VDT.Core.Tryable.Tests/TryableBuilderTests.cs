using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using static VDT.Core.Tryable.TryableBuilder;

namespace VDT.Core.Tryable.Tests;

public class TryableBuilderTests {
    [Fact]
    public void TryCreatesTryable() {
        var function = Substitute.For<Func<int, int>>();
        var result = Try(function);

        Assert.IsType<Tryable<int, int>>(result);

        result.Function(5);

        function.Received().Invoke(5);
    }
    [Fact]
    public void TryCreatesVoidTryable() {
        var function = Substitute.For<Func<int>>();
        var result = Try(function);

        Assert.IsType<Tryable<Void, int>>(result);

        result.Function(Void.Instance);

        function.Received().Invoke();
    }

    [Fact]
    public void TryCreatesAsyncTryable() {
        var function = Substitute.For<Func<Task<int>>>();
        var result = Try(function);

        Assert.IsType<AsyncTryable<int>>(result);

        result.Function(Void.Instance);

        function.Received().Invoke();
    }
}
