Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace AgoraLegacy
    Public Class dgTemplate : Implements ITemplate
        Public colname As String
        Public ObjType As Integer

        Public Sub New(ByVal cname As String, ByVal objectType As Integer)
            colname = cname
            ObjType = objectType
        End Sub

        Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Select Case ObjType
                Case 1
                    Dim ctl As New TextBox
                    ctl.ID = colname
                    ctl.CssClass = "textbox"
                    container.Controls.Add(ctl)
                Case 2
                    Dim ctl As New DropDownList
                    ctl.ID = colname
                    ctl.CssClass = "ddl"
                    container.Controls.Add(ctl)
                Case 3
                    Dim ctl As New CheckBox
                    ctl.ID = colname
                    container.Controls.Add(ctl)
                Case 4
                    Dim ctl As New RadioButton
                    ctl.ID = colname
                    container.Controls.Add(ctl)
                Case 5
                    Dim ctl As New Label
                    ctl.ID = colname
                    container.Controls.Add(ctl)
            End Select


        End Sub
    End Class
End Namespace
