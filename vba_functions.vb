Option Explicit

Sub ReplaceValuesInDetails()
    Dim wsSummary As Worksheet, wsDetail As Worksheet
    Dim infoTable As Range, detailRange As Range, searchColumn As Range, replaceColumn As Range
    Dim replacements As Object
    Dim i As Long
    Dim searchValue As Variant, replaceValue As Variant
    Dim cell As Range, searchKey As Variant

    Set wsSummary = ThisWorkbook.Worksheets("Summary")
    
    For Each wsDetail In ThisWorkbook.Worksheets
        If wsDetail.Name <> "Summary" Then
            Set infoTable = wsSummary.Range("A:B")
            Set detailRange = wsDetail.UsedRange
            Set searchColumn = detailRange.Columns("A")
            Set replaceColumn = detailRange.Columns("B")
            Set replacements = GetReplacementsDictionary(infoTable)
            
            ReplaceValuesInColumn searchColumn, replaceColumn, replacements
            
            Set replacements = Nothing
            
            SortSheet wsDetail, "A", 2
        End If
    Next wsDetail
    
    MsgBox "Replacement completed."
End Sub

Function GetReplacementsDictionary(infoTable As Range) As Object
    Dim replacements As Object
    Dim i As Long
    Dim searchValue As Variant, replaceValue As Variant
    
    Set replacements = CreateObject("Scripting.Dictionary")
    
    For i = 2 To infoTable.Rows.Count
        searchValue = infoTable.Cells(i, 1).Value
        replaceValue = infoTable.Cells(i, 2).Value
        
        If Not IsError(searchValue) And Not IsError(replaceValue) Then
            If Not IsEmpty(searchValue) Then replacements(searchValue) = replaceValue
        End If
    Next i
    
    Set GetReplacementsDictionary = replacements
End Function

Sub ReplaceValuesInColumn(searchColumn As Range, replaceColumn As Range, replacements As Object)
    Dim cell As Range
    Dim searchKey As Variant
    
    For Each cell In searchColumn.Cells
        If cell.Row > 1 Then
            searchKey = cell.Value
            
            If Not IsError(searchKey) And Not IsEmpty(searchKey) Then
                If replacements.Exists(searchKey) Then cell.Offset(0, 1).Value = replacements(searchKey)
            End If
        End If
    Next cell
End Sub

Sub SortSheet(ws As Worksheet, sortColumn As String, startRow As Long)
    With ws.Sort
        .SortFields.Clear
        .SortFields.Add Key:=ws.Columns(sortColumn), SortOn:=xlSortOnValues, Order:=xlAscending, DataOption:=xlSortNormal
        .SetRange ws.Range(sortColumn & startRow & ":" & ws.Cells(ws.Rows.Count, sortColumn).End(xlUp).Address)
        .Header = xlNo
        .MatchCase = False
        .Orientation = xlTopToBottom
        .SortMethod = xlPinYin
        .Apply
    End With
End Sub
