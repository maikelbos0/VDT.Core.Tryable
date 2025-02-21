using System;
using System.Threading.Tasks;
using Xunit;
using static VDT.Core.Tryable.TryableBuilder;

namespace VDT.Core.Tryable.Tests;

public class IntegrationTests {
    [Theory]
    [InlineData(10, 2, 10, 50)]
    [InlineData(10, 0, 1, double.PositiveInfinity)]
    [InlineData(-10, 0, 1, double.NegativeInfinity)]
    [InlineData(1000000, 1, 1000000, double.NaN)]
    public void Tryable(int numerator, int denominator, int multiplier, double expectedResult) {
        checked {
            var isComplete = false;

            var result = Try(((int Numerator, int Denominator, int Multiplier) value) => (double)(value.Numerator / value.Denominator * value.Multiplier))
                .Catch<DivideByZeroException>((ex, value) => value.Numerator < 0, ex => double.NegativeInfinity)
                .Catch<DivideByZeroException>(ex => double.PositiveInfinity)
                .Catch(() => double.NaN)
                .Finally(_ => {
                    isComplete = true;
                    return Void.Instance;
                })
                .Execute((numerator, denominator, multiplier));

            Assert.Equal(expectedResult, result);
            Assert.True(isComplete);
        }
    }

    [Theory]
    [InlineData(10, 2, 10, 50)]
    [InlineData(10, 0, 1, double.PositiveInfinity)]
    [InlineData(-10, 0, 1, double.NegativeInfinity)]
    [InlineData(1000000, 1, 1000000, double.NaN)]
    public void VoidTryable(int numerator, int denominator, int multiplier, double expectedResult) {
        checked {
            var isComplete = false;

            var result = Try(() => (double)(numerator / denominator * multiplier))
                .Catch<DivideByZeroException>(ex => numerator < 0, ex => double.NegativeInfinity)
                .Catch<DivideByZeroException>(ex => double.PositiveInfinity)
                .Catch(() => double.NaN)
                .Finally(_ => {
                    isComplete = true;
                    return Void.Instance;
                })
                .Execute();

            Assert.Equal(expectedResult, result);
            Assert.True(isComplete);
        }
    }

    [Theory]
    [InlineData(10, 2, 10, 50)]
    [InlineData(10, 0, 1, double.PositiveInfinity)]
    [InlineData(-10, 0, 1, double.NegativeInfinity)]
    [InlineData(1000000, 1, 1000000, double.NaN)]
    public async Task AsyncTryable(int numerator, int denominator, int multiplier, double expectedResult) {
        checked {
            var isComplete = false;

            var result = await Try(((int Numerator, int Denominator, int Multiplier) value) => Task.FromResult((double)(value.Numerator / value.Denominator * value.Multiplier)))
                .Catch<DivideByZeroException>((ex, value) => value.Numerator < 0, ex => Task.FromResult(double.NegativeInfinity))
                .Catch<DivideByZeroException>(ex => Task.FromResult(double.PositiveInfinity))
                .Catch(() => Task.FromResult(double.NaN))
                .Finally(_ => {
                    isComplete = true;
                    return Task.CompletedTask;
                })
                .Execute((numerator, denominator, multiplier));

            Assert.Equal(expectedResult, result);
            Assert.True(isComplete);
        }
    }

    [Theory]
    [InlineData(10, 2, 10, 50)]
    [InlineData(10, 0, 1, double.PositiveInfinity)]
    [InlineData(-10, 0, 1, double.NegativeInfinity)]
    [InlineData(1000000, 1, 1000000, double.NaN)]
    public async Task VoidAsyncTryable(int numerator, int denominator, int multiplier, double expectedResult) {
        checked {
            var isComplete = false;

            var result = await Try(() => Task.FromResult((double)(numerator / denominator * multiplier)))
                .Catch<DivideByZeroException>(ex => numerator < 0, ex => Task.FromResult(double.NegativeInfinity))
                .Catch<DivideByZeroException>(ex => Task.FromResult(double.PositiveInfinity))
                .Catch(() => Task.FromResult(double.NaN))
                .Finally(_ => {
                    isComplete = true;
                    return Task.CompletedTask;
                })
                .Execute();

            Assert.Equal(expectedResult, result);
            Assert.True(isComplete);
        }
    }
}
