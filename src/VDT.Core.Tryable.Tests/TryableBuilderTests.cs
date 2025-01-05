using NSubstitute;
using System;
using Xunit;
using static VDT.Core.Tryable.TryableBuilder;

namespace VDT.Core.Tryable.Tests;

public class TryableBuilderTests {
    [Fact]
    public void TryCreatesTryable() {
        var function = Substitute.For<Func<int>>();
        var result = Try(function);

        Assert.Equal(function, result.Function);
    }
}
