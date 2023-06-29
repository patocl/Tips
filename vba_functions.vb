Sub CreateHyperlinks()
    Dim summarySheet As Worksheet, detailSheet As Worksheet
    Dim summaryLastRow As Long, detailLastRow As Long
    Dim summaryRange As Range, detailRange As Range
    Dim summaryCell As Range, detailCell As Range
    Dim summaryID As String
    Dim idDict As Object: Set idDict = CreateObject("Scripting.Dictionary")
    Dim detailDict As Object: Set detailDict = CreateObject("Scripting.Dictionary")
    Dim detailNameDict As Object: Set detailNameDict = CreateObject("Scripting.Dictionary")
    
    Set summarySheet = ThisWorkbook.Sheets("Summary")
    summaryLastRow = summarySheet.Cells(summarySheet.Rows.Count, "A").End(xlUp).Row
    Set summaryRange = summarySheet.Range("A2:A" & summaryLastRow)
    
    For Each summaryCell In summaryRange
        summaryID = summaryCell.Value
        If Not idDict.exists(summaryID) Then idDict.Add summaryID, summaryCell.Address
    Next summaryCell
    
    For Each detailSheet In ThisWorkbook.Sheets
        If detailSheet.Name Like "Detail*" Then
            detailDict.Add detailSheet.Name, detailSheet.Name
            detailNameDict.Add detailSheet.Name, detailSheet.Name
            
            detailLastRow = detailSheet.Cells(detailSheet.Rows.Count, "A").End(xlUp).Row
            Set detailRange = detailSheet.Range("A2:A" & detailLastRow)
            
            For Each detailCell In detailRange
                summaryID = detailCell.Value
                If idDict.exists(summaryID) Then
                    Set summaryCell = summarySheet.Range(idDict(summaryID))
                    detailCell.Hyperlinks.Add Anchor:=detailCell, Address:="", SubAddress:="'" & summarySheet.Name & "'!" & summaryCell.Address, TextToDisplay:=summaryID
                End If
            Next detailCell
        End If
    Next detailSheet
    
    For Each detailSheet In ThisWorkbook.Sheets
        If detailDict.exists(detailSheet.Name) Then
            Dim detailID As String: detailID = detailDict(detailSheet.Name)
            Dim detailColumnRange As Range: Set detailColumnRange = detailSheet.Range("B2:B" & detailLastRow)
            
            For Each detailCell In detailColumnRange
                detailCell.Hyperlinks.Add Anchor:=detailCell, Address:="", SubAddress:="'" & summarySheet.Name & "'!" & idDict(detailID), TextToDisplay:=detailID
            Next detailCell
        End If
    Next detailSheet
End Sub
