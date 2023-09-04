using System;
using Polly;
using Xunit;

public class RetryAndWaitPolicyBuilderTests
{
    [Fact]
    public void Build_WithValidParameters_ReturnsPolicy()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();
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
        var builder = new RetryAndWaitPolicyBuilder<string>();
        builder.HandleException<TimeoutException>().Retry(3, (ex, attempt) => true).WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutSleepDurationProvider_ThrowsException()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();
        builder.HandleException<TimeoutException>().Retry(3, (ex, attempt) => true).WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithoutExceptionType_ThrowsException()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();
        builder.Retry(3, (ex, attempt) => true).WaitAndRetry(3, attempt => TimeSpan.FromSeconds(1));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
    
    [Fact]
    public void HandleException_SetsExceptionType()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();

        // Act
        builder.HandleException<TimeoutException>();

        // Assert
        Assert.Equal(typeof(TimeoutException), builder.GetType().GetField("exceptionType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(builder));
    }

    [Fact]
    public void Retry_SetsRetryCount()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();

        // Act
        builder.Retry(3, (ex, attempt) => true);

        // Assert
        Assert.Equal(3, builder.GetType().GetField("retryCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(builder));
    }

    [Fact]
    public void WaitAndRetry_SetsRetryCountAndSleepDurationProvider()
    {
        // Arrange
        var builder = new RetryAndWaitPolicyBuilder<string>();
        var sleepDurationProvider = new Func<int, TimeSpan>(attempt => TimeSpan.FromSeconds(1));

        // Act
        builder.WaitAndRetry(3, sleepDurationProvider);

        // Assert
        var retryCount = builder.GetType().GetField("retryCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(builder);
        var provider = builder.GetType().GetField("sleepDurationProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(builder);

        Assert.Equal(3, retryCount);
        Assert.Equal(sleepDurationProvider, provider);
    }
}
