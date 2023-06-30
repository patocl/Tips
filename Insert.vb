Sub UpdateOrInsertRecordsBatch()
    Dim conn As ADODB.Connection
    Dim rs As ADODB.Recordset
    Dim sql As String
    
    ' Set the connection string
    Dim connectionString As String
    connectionString = "Provider=SQLOLEDB;Data Source=NombreServidor;Initial Catalog=NombreBaseDatos;User ID=NombreUsuario;Password=Contraseña;"
    
    ' Create a connection to the database
    Set conn = New ADODB.Connection
    conn.Open connectionString
    
    ' Get the recordset with the data to update/insert
    Set rs = GetRecordsetToUpdate(conn) ' Replace with your own recordset retrieval logic
    
    ' Create a table variable to hold the data
    sql = "DECLARE @TempTable TABLE (id INT, descripcion VARCHAR(255))"
    conn.Execute sql
    
    ' Build the insert query using a single query instead of loop
    Dim values As String
    values = ""
    
    rs.MoveFirst
    While Not rs.EOF
        values = values & "(" & rs.Fields("id").Value & ", '" & rs.Fields("descripcion").Value & "'), "
        rs.MoveNext
    Wend
    
    ' Remove the trailing comma and space
    values = Left(values, Len(values) - 2)
    
    ' Insert the values into the table variable
    sql = "INSERT INTO @TempTable (id, descripcion) VALUES " & values
    conn.Execute sql
    
    ' Update or insert records from the table variable to the persistent table
    sql = "MERGE INTO TuTabla AS target " & _
          "USING @TempTable AS source ON (target.id = source.id) " & _
          "WHEN MATCHED THEN " & _
          "    UPDATE SET target.descripcion = source.descripcion " & _
          "WHEN NOT MATCHED BY TARGET THEN " & _
          "    INSERT (id, descripcion) VALUES (source.id, source.descripcion);"
    conn.Execute sql
    
    ' Close the connection and release resources
    rs.Close
    conn.Close
    Set rs = Nothing
    Set conn = Nothing
End Sub


Function CloneRecordset(originalRecordset As Recordset) As Recordset
    Dim clonedRecordset As Recordset
    Set clonedRecordset = New Recordset
    
    Dim field As Field
    For Each field In originalRecordset.Fields
        clonedRecordset.Fields.Append field.Name, field.Type, field.DefinedSize, field.Attributes
    Next field
    
    clonedRecordset.CursorLocation = adUseClient
    clonedRecordset.Open
    
    originalRecordset.MoveFirst
    Do Until originalRecordset.EOF
        clonedRecordset.AddNew
        For Each field In originalRecordset.Fields
            clonedRecordset.Fields(field.Name).Value = field.Value
        Next field
        clonedRecordset.Update
        originalRecordset.MoveNext
    Loop
    
    Set CloneRecordset = clonedRecordset
End Function


