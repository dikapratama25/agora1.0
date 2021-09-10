Imports AgoraLegacy
Imports Microsoft.Web.UI.WebControls
Imports SSO.Component
Imports MySql.Data.MySqlClient
Partial Public Class DashboardTypeAhead
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q As String
        q = Request.QueryString("q")
        Dim ds As New DataSet
        Dim objDash As New Dashboard
        ds = objDash.GetDashboardPanelName()
        Dim DashRow As DataRow
        For Each DashRow In ds.Tables(0).Rows

            If InStr(LCase(DashRow("DM_PANEL_NAME")), LCase(q)) <> False Then
                Response.Write(DashRow("DM_PANEL_NAME"))
                Response.Write(vbCrLf)
            End If



        Next
    End Sub

End Class