Imports AgoraLegacy
Imports Microsoft.Web.UI.WebControls
Imports SSO.Component
Imports MySql.Data.MySqlClient
Partial Public Class TypeAhead
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim q As String
        q = Request.QueryString("q")
        Dim ds As New DataSet
        Dim objPO As New Companies
        ds = objPO.getAllCompanyTypeAhead()
        Dim CompanyRow As DataRow
        For Each CompanyRow In ds.Tables("CompanyListName").Rows

            If InStr(LCase(CompanyRow("CM_COY_NAME")), LCase(q)) <> False Then
                Response.Write(CompanyRow("CM_COY_NAME"))
                Response.Write(vbCrLf)
            End If



        Next
    End Sub

End Class