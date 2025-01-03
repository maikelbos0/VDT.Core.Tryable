﻿using System;
using Xunit;

namespace VDT.Core.Tryable.Tests;

public class TryableTests {
    [Fact]
    public void ReturnsFunctionValueOnSuccess() {
        var subject = new Tryable<int>(() => 5, ex => 10);

        var result = subject.Resolve();

        Assert.Equal(5, result);
    }

    [Fact]
    public void UsesDefaultErrorHandlerOnErrorWithoutErrorHandlers() {
        var exception = new Exception();
        var subject = new Tryable<int>(() => throw exception, ex => {
            Assert.Equal(exception, ex);
            return 10;
        });

        var result = subject.Resolve();

        Assert.Equal(10, result);
    }
}