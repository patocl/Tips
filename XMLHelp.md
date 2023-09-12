''' <summary>
''' Checks if the specified value is not null and throws an ArgumentNullException if it is.
''' </summary>
''' <typeparam name="T">The type of the value to check.</typeparam>
''' <param name="value">The value to check for null.</param>
''' <param name="name">The name of the parameter (used in the exception message).</param>
''' <exception cref="ArgumentNullException">Thrown if the <paramref name="value"/> is null.</exception>

''' <summary>
''' Checks if the specified string is not null or empty and throws an ArgumentException if it is.
''' </summary>
''' <param name="value">The string to check for null or empty.</param>
''' <param name="name">The name of the parameter (used in the exception message).</param>
''' <exception cref="ArgumentException">Thrown if the <paramref name="value"/> is null or empty.</exception>
Public Sub NotNullOrEmpty(value As String, name As String)
    If String.IsNullOrEmpty(value) Then
        Throw New ArgumentException("The string parameter '" & name & "' cannot be null or empty.")
    End If
End Sub

''' <summary>
''' Checks if the specified integer value is greater than zero and throws an ArgumentException if it is not.
''' </summary>
''' <param name="value">The integer value to check.</param>
''' <param name="name">The name of the parameter (used in the exception message).</param>
''' <exception cref="ArgumentException">Thrown if the <paramref name="value"/> is not greater than zero.</exception>
Public Sub GreaterThanZero(value As Integer, name As String)
    If value <= 0 Then
        Throw New ArgumentException("The integer parameter '" & name & "' must be greater than zero.")
    End If
End Sub

        
''' <summary>
''' Checks if the specified IEnumerable is not null and contains at least one element, and throws an ArgumentException if it does not.
''' </summary>
''' <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
''' <param name="list">The IEnumerable to check for emptiness.</param>
''' <param name="name">The name of the parameter (used in the exception message).</param>
''' <exception cref="ArgumentException">Thrown if the <paramref name="list"/> is null or empty.</exception>
Public Sub NotEmptyList(Of T)(list As IEnumerable(Of T), name As String)
    If list Is Nothing OrElse Not list.Any() Then
        Throw New ArgumentException("The IEnumerable parameter '" & name & "' must not be null or empty.")
    End If
End Sub
            