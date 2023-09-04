Imports System
Imports Polly

''' <summary>
''' Represents a base class for defining retry policies using Polly.
''' </summary>
Public Class RetryPolicyBase
    Private ReadOnly policy As Policy

    ''' <summary>
    ''' Initializes a new instance of the <see cref="RetryPolicyBase"/> class.
    ''' </summary>
    ''' <param name="retryCount">The maximum number of retry attempts.</param>
    ''' <param name="delayProvider">A function that provides the delay duration for each retry attempt.</param>
    ''' <param name="exceptionPredicate">A function that specifies which exceptions should trigger a retry.</param>
    Public Sub New(retryCount As Integer, delayProvider As Func(Of Integer, TimeSpan), exceptionPredicate As Func(Of Exception, Boolean))
        If retryCount <= 0 Then
            Throw New InvalidOperationException("Retry count must be greater than 0.")
        End If

        If delayProvider Is Nothing Then
            Throw New InvalidOperationException("Delay provider must be specified.")
        End If

        policy = Policy.Handle(exceptionPredicate) _
            .WaitAndRetry(retryCount, Function(attempt) delayProvider(attempt))
    End Sub

    ''' <summary>
    ''' Executes an action with the defined retry policy.
    ''' </summary>
    ''' <param name="action">The action to be executed with retry policy.</param>
    Public Sub Execute(action As Action)
        policy.Execute(action)
    End Sub
End Class


Imports Helpers
Imports System.Data.SqlClient

''' <summary>
''' Represents a retry policy for handling deadlocks in SQL Server using Polly.
''' </summary>
Public Class WaitAndRetryPolicyOnDeadlock
    Inherits RetryPolicyBase

    ''' <summary>
    ''' Initializes a new instance of the <see cref="WaitAndRetryPolicyOnDeadlock"/> class.
    ''' </summary>
    ''' <param name="retryCount">The maximum number of retry attempts for handling deadlocks.</param>
    ''' <param name="delayProvider">A function that provides the delay duration for each retry attempt.</param>
    Public Sub New(retryCount As Integer, delayProvider As Func(Of Integer, TimeSpan))
        MyBase.New(retryCount, delayProvider, AddressOf IsDeadlock)
    End Sub

    ''' <summary>
    ''' Checks if the provided exception is a SQL Server deadlock error.
    ''' </summary>
    ''' <param name="ex">The exception to check.</param>
    ''' <returns>True if the exception is a SQL Server deadlock error; otherwise, false.</returns>
    Private Function IsDeadlock(ex As Exception) As Boolean
        If TypeOf ex Is SqlException Then
            Dim sqlEx As SqlException = DirectCast(ex, SqlException)
            Return sqlEx.Number = 1205 ' Specific error number for SQL Server deadlock
        End If
        Return False
    End Function
End Class
