Imports AgoraLegacy
Imports eProcure.Component

Public Class ReportFormat10
    Inherits AgoraLegacy.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtSDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtPerfectDay_time As System.Web.UI.WebControls.TextBox
    Protected WithEvents ValPerfectDay_time As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents lblPerfectDay_time As System.Web.UI.WebControls.Label
    Protected WithEvents cboReportType As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dtRadioBtn As System.Web.UI.WebControls.RadioButtonList

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
                Case "PO"
                    lblPerfectDay_time.Text = "Perfect Days"
                    lblHeader.Text = "Perfect Order Fulfillment"
                Case "VENDOR"
                    lblPerfectDay_time.Text = "Expected Cycle Time (Days)"
                    lblHeader.Text = "Vendor Cycle Time"
                Case "R7"
                    ValPerfectDay_time.ErrorMessage = "Item Code is required."
                    lblPerfectDay_time.Text = "Item Code"
                    lblHeader.Text = "R7 Summary Approved And Paid Invoices Report Sorted By Branch For Branch(Spec Item Expenditure)"
                Case "R8"
                    ValPerfectDay_time.ErrorMessage = "Item Code is required."
                    lblPerfectDay_time.Text = "Item Code"
                    lblHeader.Text = "R8 Summary Approved And Paid Invoices Report Sorted By Branch For Head Office(Spec Item Expenditure)"

                    'Jules 2016.01.28 - FITR enhancement.
                Case "R8_2"
                    ValPerfectDay_time.ErrorMessage = "Item Code is required."
                    lblPerfectDay_time.Text = "Item Code"
                    lblHeader.Text = "R8_2 Summary Approved And Paid Invoices Report Sorted By Branch For Branch and Head Office(Spec Item Expenditure)"
                    'End .
            End Select

            'Chee hong (07/02/2014) - Included options for capex & opex

            'Jules 2016.01.28 - FITR enhancement.
            'If UCase(ViewState("type")) = "R7" Or UCase(ViewState("type")) = "R8" Then
            If UCase(ViewState("type")) = "R7" Or UCase(ViewState("type")) = "R8" Or UCase(ViewState("type")) = "R8_2" Then
                tr2.Style("display") = ""
            Else
                tr2.Style("display") = "none"
            End If
        End If

        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim strReportName As String = ""
        Dim reportURL As String
        Dim strParam As String = ""
        'Dim crParam As New Hashtable
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

        'crParam.Add("userIDParam", Session("UserID"))
        'crParam.Add("buyercompanyIDParam", Session("CompanyID"))
        'crParam.Add("dateFromParam", startdate)
        'crParam.Add("dateToParam", Enddate)

        Select Case UCase(ViewState("type"))
            Case "PO"
                strReportName = "perfectOrderFulfillment.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&BcoyID=" & Session("CompanyID") & "&DateFrom=" & startdate & "&DateTo=" & Enddate & "&perfectDays=" & txtPerfectDay_time.Text
                'crParam.Add("perfectDays", txtPerfectDay_time.Text)
            Case "VENDOR"
                strReportName = "vendorCycleTime.rpt"
                strParam = "&userIDParam=" & Session("UserID") & "&BcoyID=" & Session("CompanyID") & "&DateFrom=" & startdate & "&DateTo=" & Enddate & "&expectedCycleTime=" & txtPerfectDay_time.Text
                'crParam.Add("expectedCycleTime", txtPerfectDay_time.Text)
            Case "R7"
                'crParam.Add("ItemIDParam", txtPerfectDay_time.Text)
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R7SummaryApprovedPaidInvoicesSortedByBranch-BR-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R7SummaryApprovedPaidInvoicesSortedByBranch-BR-Capex.rpt"
                Else
                    strReportName = "R7SummaryApprovedPaidInvoicesSortedByBranch-BR.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & Enddate & "&ItemIDParam=" & txtPerfectDay_time.Text
            Case "R8"
                'crParam.Add("ItemIDParam", txtPerfectDay_time.Text)
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ-Capex.rpt"
                Else
                    strReportName = "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & Enddate & "&ItemIDParam=" & txtPerfectDay_time.Text

                'Jules 2016.01.28 - FITR enhancement.
            Case "R8_2"
                If Me.dtRadioBtn.SelectedValue = "opex" Then
                    strReportName = "R8_2SummaryApprovedPaidInvoicesSortedByBranch-Opex.rpt"
                ElseIf Me.dtRadioBtn.SelectedValue = "capex" Then
                    strReportName = "R8_2SummaryApprovedPaidInvoicesSortedByBranch-Capex.rpt"
                    'Else
                    '    strReportName = "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt"
                End If
                strParam = "&userIDParam=" & Session("UserID") & "&buyerCompanyIDParam=" & Session("CompanyID") & "&dateFromParam=" & startdate & "&dateToParam=" & Enddate & "&ItemIDParam=" & txtPerfectDay_time.Text
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
        Me.ClientScript.RegisterStartupScript(GetType(ReportFormat10), rndKey.Next.ToString, jscript)
    End Sub

    'Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
    '    Dim crParam As New Hashtable
    '    Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
    '    Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

    '    crParam.Add("userIDParam", Session("UserID"))
    '    ' crParam.Add("companyIDParam", Session("CompanyID"))
    '    crParam.Add("dateFromParam", startdate)
    '    crParam.Add("dateToParam", Enddate)

    '    Select Case UCase(viewstate("type"))
    '        Case "PO"
    '            crParam.Add("perfectDays", txtPerfectDay_time.Text)
    '            crParam.Add("companyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "perfectOrderFulfillment.rpt", crParam)
    '        Case "VENDOR"
    '            crParam.Add("expectedCycleTime", txtPerfectDay_time.Text)
    '            crParam.Add("companyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "vendorCycleTime.rpt", crParam)
    '        Case "R7"
    '            crParam.Add("itemIDParam", txtPerfectDay_time.Text)
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R7SummaryApprovedPaidInvoicesSortedByBranch-BR.rpt", crParam)
    '        Case "R8"
    '            crParam.Add("itemIDParam", txtPerfectDay_time.Text)
    '            crParam.Add("buyerCompanyIDParam", Session("CompanyID"))
    '            ReportViewer.popCrystalReport(Me, "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt", crParam)
    '    End Select
    'End Sub
End Class
