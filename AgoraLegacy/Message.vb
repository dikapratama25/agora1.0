Public Class Message
    'Session("MsgRecordSave") = "Record saved."
    'Session("MsgRecordDelete") = "Record deleted."
    'Session("MsgForDeleteButton") = "Are you sure that you want to permanently delete this item(s)?"
    'Session("MsgNoRecord") = "No record found."
    'Session("MsgRecordDuplicate") = "Duplicate record found."
    'Session("MsgRecordNotSave") = "Record not saved."
    'Session("MsgRecordNotDelete") = "Record not delete."
    'Session("RecordDeleteNotAllowed") = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)s"
    'MsgInvalidDate As String = "Start Date Must Be Earlier Than Or Equal To End Date"

    Public Function GetMessage(ByVal iMessage As Integer)
        Select Case iMessage
            Case 1001
                GetMessage = "Record saved."
            Case 1002
                GetMessage = "Record deleted."
            Case 1003
                GetMessage = "Are you sure that you want to permanently delete this item(s)?"
            Case 1004
                GetMessage = "No record found."
            Case 1005
                GetMessage = "Duplicate record found."
            Case 1006
                GetMessage = "Record not saved."
            Case 1007
                GetMessage = "Record not delete."
            Case 1008
                GetMessage = "Deletion is not allowed.""&vbCrLf&""It has outstanding PR(s)s."
            Case 1009
                GetMessage = "Start Date Must Be Earlier Than Or Equal To End Date"
            Case Else
                GetMessage = ""
        End Select
    End Function
End Class
