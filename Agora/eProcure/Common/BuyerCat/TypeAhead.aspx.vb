Imports AppCommon
Imports eProcure.Component
Imports System.Text.RegularExpressions

Imports System.Drawing
Partial Public Class TypeAheadVendor
    Inherits System.Web.UI.Page
    Dim dDispatcher As New dispatcher


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim q As String
        q = Request.QueryString("q")
        Dim ds As New DataSet
        Dim objAdmin As New Admin
        ds = objAdmin.searchapprvendorforBIM()
        Dim vendorRow As DataRow
        For Each vendorRow In ds.Tables(0).Rows

            If InStr(LCase(vendorRow("CM_COY_NAME")), LCase(q)) <> False Then
                Response.Write(vendorRow("CM_COY_NAME"))
                Response.Write("|")
                Response.Write(vendorRow("CM_COY_ID"))
                Response.Write(vbCrLf)
            End If



        Next

    End Sub

End Class