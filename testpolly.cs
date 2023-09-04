using System;
using Polly;
using Xunit;

public class RetryAndWaitPolicyBuilderTests
{
    [Fact]
    public void Build_WithValidParameters_ReturnsPolicy()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();
        builder.HandleException<TimeoutException>().Retry(3, (ex, attempt) => true).WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act
        var policy = builder.Build();

        // Assert
        Assert.NotNull(policy);
    }

    [Fact]
    public void Build_WithoutRetryCount_ThrowsException()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();
        builder.HandleException<TimeoutException>().WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutSleepDurationProvider_ThrowsException()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();
        builder.HandleException<TimeoutException>().Retry(3, (ex, attempt) => true);

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutExceptionType_ThrowsException()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();
        builder.Retry(3, (ex, attempt) => true).WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void HandleException_SetsExceptionType()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();

        // Act
        builder.HandleException<TimeoutException>();

        // Assert
        var privateField = typeof(RetryAndWaitPolicyBuilder).GetField("exceptionType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var exceptionType = privateField.GetValue(builder) as Type;

        Assert.Equal(typeof(TimeoutException), exceptionType);
    }

    [Fact]
    public void Retry_SetsRetryCount()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();

        // Act
        builder.Retry(3, (ex, attempt) => true);

        // Assert
        var privateField = typeof(RetryAndWaitPolicyBuilder).GetField("retryCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var retryCount = (int)privateField.GetValue(builder);

        Assert.Equal(3, retryCount);
    }

    [Fact]
    public void WaitAndRetry_SetsRetryCountAndSleepDurationProvider()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder();
        var sleepDurationProvider = new Func<int, TimeSpan>(attempt => TimeSpan.FromSeconds(1));

        // Act
        builder.WaitAndRetry(3, sleepDurationProvider);

        // Assert
        var privateRetryField = typeof(RetryAndWaitPolicyBuilder).GetField("retryCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var retryCount = (int)privateRetryField.GetValue(builder);

        var privateSleepField = typeof(RetryAndWaitPolicyBuilder).GetField("sleepDurationProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var provider = privateSleepField.GetValue(builder) as Func<int, TimeSpan>;

        Assert.Equal(3, retryCount);
        Assert.Equal(sleepDurationProvider, provider);
    }
}
