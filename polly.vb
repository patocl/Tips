Imports Polly
Public Interface IPolicyBuilder
    Function Build() As ISyncPolicy
    Function HandleException(Of TException As Exception)() As IPolicyBuilder
    Function Retry(ByVal retryCount As Integer, ByVal retryCondition As Func(Of Exception, Integer, Boolean)) As IPolicyBuilder
    Function WaitAndRetry(ByVal retryCount As Integer, ByVal sleepDurationProvider As Func(Of Integer, TimeSpan)) As IPolicyBuilder
End Interface

Imports Polly
Public Class RetryAndWaitPolicyBuilder
    Implements IPolicyBuilder

    Private retryCount As Integer = 0
    Private sleepDurationProvider As Func(Of Integer, TimeSpan)
    Private exceptionType As Type

    Public Function Build() As ISyncPolicy Implements IPolicyBuilder.Build
        If retryCount <= 0 Then
            Throw New InvalidOperationException("Retry count must be greater than 0.")
        End If

        If sleepDurationProvider Is Nothing Then
            Throw New InvalidOperationException("Sleep duration provider must be specified.")
        End If

        If exceptionType Is Nothing Then
            Throw New InvalidOperationException("Exception type to handle must be specified.")
        End If

        Return Policy.Handle(exceptionType).WaitAndRetry(retryCount, sleepDurationProvider)
    End Function

    Public Function HandleException(Of TException As Exception)() As IPolicyBuilder Implements IPolicyBuilder.HandleException
        exceptionType = GetType(TException)
        Return Me
    End Function

    Public Function Retry(ByVal retryCount As Integer, ByVal retryCondition As Func(Of Exception, Integer, Boolean)) As IPolicyBuilder Implements IPolicyBuilder.Retry
        Me.retryCount = retryCount
        Return Me
    End Function

    Public Function WaitAndRetry(ByVal retryCount As Integer, ByVal sleepDurationProvider As Func(Of Integer, TimeSpan)) As IPolicyBuilder Implements IPolicyBuilder.WaitAndRetry
        Me.retryCount = retryCount
        Me.sleepDurationProvider = sleepDurationProvider
        Return Me
    End Function
End Class

