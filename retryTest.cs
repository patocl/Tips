using Xunit;
using Moq;
using Polly;
using NLog;
using FluentAssertions;

public class WaitRetryPolicyOnSqlDeadlockTests
{
    [Fact]
    public void Execute_WithCompanyExceptionAndSqlDeadlock_ShouldLogDeadlock()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var contextInfo = new ContextInfo { Action = "TestAction" };

        var policy = Policy.Handle<CompanyException>()
                           .WaitAndRetry(1, _ => TimeSpan.FromMilliseconds(100), (ex, _, _, _) =>
                           {
                               if (ex is CompanyException companyException && companyException.IsSqlDeadLockException())
                               {
                                   loggerMock.Object.Log("Deadlock");
                               }
                               else
                               {
                                   loggerMock.Object.Log("Other Exception");
                               }
                           });

        var retryPolicy = new WaitRetryPolicyOnSqlDeadlock(1, 100, loggerMock.Object)
        {
            ContextInfo = contextInfo
        };

        // Act
        var result = retryPolicy.Execute(() => throw new CompanyException());

        // Assert
        loggerMock.Verify(logger => logger.Log("Deadlock"), Times.Once);
        loggerMock.Verify(logger => logger.Log("Other Exception"), Times.Never);
    }

    [Fact]
    public void Execute_WithCompanyExceptionAndNonSqlDeadlock_ShouldLogOtherException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var contextInfo = new ContextInfo { Action = "TestAction" };

        var policy = Policy.Handle<CompanyException>()
                           .WaitAndRetry(1, _ => TimeSpan.FromMilliseconds(100), (ex, _, _, _) =>
                           {
                               if (ex is CompanyException companyException && companyException.IsSqlDeadLockException())
                               {
                                   loggerMock.Object.Log("Deadlock");
                               }
                               else
                               {
                                   loggerMock.Object.Log("Other Exception");
                               }
                           });

        var retryPolicy = new WaitRetryPolicyOnSqlDeadlock(1, 100, loggerMock.Object)
        {
            ContextInfo = contextInfo
        };

        // Act
        var result = retryPolicy.Execute(() => throw new CompanyException(false));

        // Assert
        loggerMock.Verify(logger => logger.Log("Deadlock"), Times.Never);
        loggerMock.Verify(logger => logger.Log("Other Exception"), Times.Once);
    }

    [Fact]
    public void Execute_WithNoException_ShouldNotLogAnyException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var contextInfo = new ContextInfo { Action = "TestAction" };

        var policy = Policy.Handle<CompanyException>()
                           .WaitAndRetry(1, _ => TimeSpan.FromMilliseconds(100), (ex, _, _, _) =>
                           {
                               if (ex is CompanyException companyException && companyException.IsSqlDeadLockException())
                               {
                                   loggerMock.Object.Log("Deadlock");
                               }
                               else
                               {
                                   loggerMock.Object.Log("Other Exception");
                               }
                           });

        var retryPolicy = new WaitRetryPolicyOnSqlDeadlock(1, 100, loggerMock.Object)
        {
            ContextInfo = contextInfo
        };

        // Act
        var result = retryPolicy.Execute(() => { /* No exception thrown */ });

        // Assert
        loggerMock.Verify(logger => logger.Log("Deadlock"), Times.Never);
        loggerMock.Verify(logger => logger.Log("Other Exception"), Times.Never);
    }
}
