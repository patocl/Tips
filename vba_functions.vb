Sub CreateHyperlinks()
    Dim summarySheet As Worksheet, detailSheet As Worksheet
    Dim summaryLastRow As Long, detailLastRow As Long
    Dim summaryRange As Range, detailRange As Range
    Dim summaryCell As Range, detailCell As Range
    Dim summaryID As String
    Dim idDict As Object: Set idDict = CreateObject("Scripting.Dictionary")
    Dim detailDict As Object: Set detailDict = CreateObject("Scripting.Dictionary")
    Dim detailNameDict As Object: Set detailNameDict = CreateObject("Scripting.Dictionary")
    Dim detailColumns As Long
    Dim detailColumn As Long
    
    Set summarySheet = ThisWorkbook.Sheets("Summary")
    summaryLastRow = summarySheet.Cells(summarySheet.Rows.Count, "A").End(xlUp).Row
    Set summaryRange = summarySheet.Range("A2:A" & summaryLastRow)
    
    For Each summaryCell In summaryRange
        summaryID = summaryCell.Value
        If Not idDict.Exists(summaryID) Then idDict.Add summaryID, summaryCell.Address
    Next summaryCell
    
    detailColumns = 1 ' Starting column index for detail links
    
    For Each detailSheet In ThisWorkbook.Sheets
        If detailSheet.Name Like "Detail*" Then
            detailDict.Add detailSheet.Name, detailSheet.Name
            detailNameDict.Add detailSheet.Name, detailSheet.Name
            
            detailLastRow = detailSheet.Cells(detailSheet.Rows.Count, "A").End(xlUp).Row
            Set detailRange = detailSheet.Range("A2:A" & detailLastRow)
            
            For Each detailCell In detailRange
                summaryID = detailCell.Value
                If idDict.Exists(summaryID) Then
                    Set summaryCell = summarySheet.Range(idDict(summaryID))
                    summaryCell.Offset(0, detailColumns).Hyperlinks.Add Anchor:=summaryCell.Offset(0, detailColumns), Address:="", SubAddress:="'" & detailSheet.Name & "'!A1", TextToDisplay:=summaryID
                    detailCell.Hyperlinks.Add Anchor:=detailCell.Offset(0, 1), Address:="", SubAddress:="'" & summarySheet.Name & "'!" & summaryCell.Offset(0, detailColumns).Address, TextToDisplay:="Go to Summary"
                End If
            Next detailCell
            
            ' Create hyperlink from detail sheet to summary sheet in column A
            detailSheet.Range("A1").Hyperlinks.Add Anchor:=detailSheet.Range("A1"), Address:="", SubAddress:="'" & summarySheet.Name & "'!A1", TextToDisplay:="Go to Summary"
            
            detailColumns = detailColumns + 1 ' Increment the detail column index
        End If
    Next detailSheet
End Sub

Sub ApplyAutoFilter()
    Dim summarySheet As Worksheet
    Dim detailSheet As Worksheet
    
    Set summarySheet = ThisWorkbook.Sheets("Summary")
    
    Dim summaryRange As Range
    Set summaryRange = summarySheet.Range("A1").CurrentRegion
    
    summaryRange.AutoFilter Field:=1, Criteria1:="<>", Operator:=xlFilterValues
    
    For Each detailSheet In ThisWorkbook.Sheets
        If detailSheet.Name Like "Detail*" Then
            detailSheet.Range("A1").AutoFilter Field:=1, Criteria1:="<>", Operator:=xlFilterValues
        End If
    Next detailSheet
    
    summarySheet.AutoFilterMode = False
End Sub