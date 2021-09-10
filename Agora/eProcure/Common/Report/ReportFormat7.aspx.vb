Imports eProcure.Component

Public Class ReportFormat7
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    'Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    'Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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

        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        Dim strParam As String = ""
        'Dim crParam As New Hashtable

        'crParam.Add("userIDParam", Session("UserID"))
        strParam = "&userIDParam=" & Session("UserID")
        Select Case UCase(ViewState("type"))
            Case "SKUSUMMARY"
                strReportName = "skuSummaryReport.rpt"
            Case "VENDORINFO"
                strReportName = "totalOfsupplier.rpt"
            Case "UNSPSC"
                strReportName = "hubunspscsummary.rpt"
            Case "PENDINVS"
                'Jules 2016.01.11 - EPROC FITR.
                Dim objDbcom As New EAD.DBCom
                Dim strTaxCodes As String = ""
                Dim decRecoverablePercentage As Decimal = 0
                'mimi - 17/01/11 - Fitr enhancement
                Dim strRecoverablePercentage As String
                Dim strIRRecoverablePercentage As String

                Dim strTaxSQL As String = "SELECT FTC_TAX_CODE FROM fitr_tax_code WHERE FTC_MODULE='eProcure' "
                Dim ds As DataSet = objDbcom.FillDs(strTaxSQL)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If strTaxCodes = "" Then
                            strTaxCodes = ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        Else
                            strTaxCodes &= "," & ds.Tables(0).Rows(i)("FTC_TAX_CODE")
                        End If
                    Next
                End If

                'mimi 09/01/2017 - fitr_mstr enhancement
                decRecoverablePercentage = objDbcom.GetVal("SELECT fm_fitr_recoverable FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'mimi - 17/01/11 - Fitr enhancement
                strRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_recoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")
                strIRRecoverablePercentage = objDbcom.GetVal("SELECT CONVERT(FLOOR(fm_fitr_irrecoverable * 100), CHAR(10)) FROM fitr_mstr WHERE fm_deleted='N' AND fm_coy_id='" & Session("CompanyID") & "'")

                'crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
                strReportName = "SummaryInvoicePendingFinalApproval.rpt"
                strParam = strParam & "&buyerCompanyIDParam=" & Session("CompanyID") & "&taxCodes=" & strTaxCodes & "&recPerc=" & decRecoverablePercentage & "&RecoverablePercentage=" & strRecoverablePercentage & "&IRRecoverablePercentage=" & strIRRecoverablePercentage
                'end - mimi
                'End.
        End Select

        'Session("Param") = crParam

        reportURL = "../Report/ReportViewer.aspx?rpt=" & strReportName & strParam & "&rptType=" & cboReportType.SelectedIndex

        Dim jscript As String = String.Empty
        jscript &= "<script language=""Javascript"">" & vbCrLf
        jscript &= "x = screen.width -200;" & vbCrLf
        jscript &= "y = screen.height - 200;" & vbCrLf
        jscript &= "var props = 'scrollBars=yes, resizable=yes, toolbar=no, menubar=no, location=no, directories=no, top=0, left=0, width=' + x + ', height=' + y ;" & vbCrLf
        jscript &= "window.location = """ & reportURL & """;" & vbCrLf
        jscript &= "//-->" & vbCrLf
        jscript &= "</script>" & vbCrLf
        Dim rndKey As New Random
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat7), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable

    '    crParam.Add("userIDParam", Session("UserID"))

    '    Select Case UCase(viewstate("type"))
    '        Case "SKUSUMMARY"
    '            ReportViewer.popCrystalReport(Me, "skuSummaryReport.rpt", crParam)
    '        Case "VENDORINFO"
    '            ReportViewer.popCrystalReport(Me, "totalOfsupplier.rpt", crParam)
    '        Case "UNSPSC"
    '            ReportViewer.popCrystalReport(Me, "hubunspscsummary.rpt", crParam)
    '        Case "PENDINVS"
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "SummaryInvoicePendingFinalApproval.rpt", crParam)
    '    End Select

    'End Sub
End Class
