Imports AgoraLegacy

Public Class ReportFormat7
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim dispatcher As New dispatcher

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MyBase.Page_Load(sender, e)

        If Not IsPostBack Then
            viewstate("type") = Request.QueryString("type")

            Select Case UCase(viewstate("type"))
                Case "SKUSUMMARY"
                    lblHeader.Text = "SKU Summary Report"
                Case "VENDORINFO"
                    lblHeader.Text = "Vendor Information List"
                Case "UNSPSC"
                    lblHeader.Text = "UNSPSC Summary Report"
                Case "PENDINVS"
                    lblHeader.Text = "Summary Invoices Pending Approval Sorted By Vendor As At Today"
            End Select
        End If

        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable

        crParam.Add("userIDParam", Session("UserID"))

        Select Case UCase(viewstate("type"))
            Case "SKUSUMMARY"
                ReportViewer.popCrystalReport(Me, "skuSummaryReport.rpt", crParam)
            Case "VENDORINFO"
                ReportViewer.popCrystalReport(Me, "totalOfsupplier.rpt", crParam)
            Case "UNSPSC"
                ReportViewer.popCrystalReport(Me, "hubunspscsummary.rpt", crParam)
            Case "PENDINVS"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "SummaryInvoicePendingFinalApproval.rpt", crParam)
        End Select

    End Sub
End Class
