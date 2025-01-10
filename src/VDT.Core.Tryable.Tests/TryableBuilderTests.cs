using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using static VDT.Core.Tryable.TryableBuilder;

namespace VDT.Core.Tryable.Tests;

public class TryableBuilderTests {
    [Fact]
    public void TryCreatesTryable() {
        var function = Substitute.For<Func<int>>();
        var result = Try(function);

        Assert.IsType<Tryable<int>>(result);
        Assert.Equal(function, result.Function);
    }

    [Fact]
    public void TryCreatesAsyncTryable() {
        var function = Substitute.For<Func<Task<int>>>();
        var result = Try(function);

        Assert.IsType<AsyncTryable<int>>(result);
        Assert.Equal(function, result.Function);
    }
}
