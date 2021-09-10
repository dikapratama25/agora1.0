Imports AgoraLegacy
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Partial Public Class subcat
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim id As String
        id = Request.Form("parent_id")
        Dim str As String
        Dim row As DataRow
        Dim ds As DataSet
        Dim objCat As New BuyerCat
        ds = objCat.getsubcat(id)


        str = "<select name=""sub_category""  id=""sub_category_id"">"

        For Each row In ds.Tables(0).Rows
            If row("LM_SUB_LOCATION") = Request.QueryString("select") Then
                str &= "<option value=" & row("LM_SUB_LOCATION") & " selected=""selected"">" & row("LM_SUB_LOCATION") & "</option>"
            Else
                If ds.Tables(0).Rows.Count = 1 Then
                    str &= "<option value="" selected=""selected"">--Select--</option>"
                End If
            End If
            str &= "<option value=" & row("LM_SUB_LOCATION") & ">" & row("LM_SUB_LOCATION") & "</option>"
        Next
        str &= "</select>"
        Response.Write(str)


    End Sub

End Class