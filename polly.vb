Imports System
Imports Polly

Public Class RetryAndWaitPolicyBuilder(Of T)
    Implements IPolicyBuilder(Of T)

    Private retryCount As Integer = 0
    Private sleepDurationProvider As Func(Of Integer, TimeSpan)
    Private exceptionType As Type

    Public Function Build() As ISyncPolicy(Of T) Implements IPolicyBuilder(Of T).Build
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

    Public Function HandleException(Of TException As Exception)() As IPolicyBuilder(Of T) Implements IPolicyBuilder(Of T).HandleException
        exceptionType = GetType(TException)
        Return Me
    End Function

    Public Function Retry(ByVal retryCount As Integer, ByVal retryCondition As Func(Of Exception, Integer, Boolean)) As IPolicyBuilder(Of T) Implements IPolicyBuilder(Of T).Retry
        Me.retryCount = retryCount
        Return Me
    End Function

    Public Function WaitAndRetry(ByVal retryCount As Integer, ByVal sleepDurationProvider As Func(Of Integer, TimeSpan)) As IPolicyBuilder(Of T) Implements IPolicyBuilder(Of T).WaitAndRetry
        Me.retryCount = retryCount
        Me.sleepDurationProvider = sleepDurationProvider
        Return Me
    End Function
End Class

Public Interface IPolicyBuilder(Of T)
    Function Build() As ISyncPolicy(Of T)
    Function HandleException(Of TException As Exception)() As IPolicyBuilder(Of T)
    Function Retry(ByVal retryCount As Integer, ByVal retryCondition As Func(Of Exception, Integer, Boolean)) As IPolicyBuilder(Of T)
    Function WaitAndRetry(ByVal retryCount As Integer, ByVal sleepDurationProvider As Func(Of Integer, TimeSpan)) As IPolicyBuilder(Of T)
End Interface

