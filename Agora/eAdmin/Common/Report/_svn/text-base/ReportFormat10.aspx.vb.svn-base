Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat10
    Inherits AppCommon.AppBaseClass

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

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Dim dispatcher As New Dispatcher.dispatcher

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
                    ValPerfectDay_time.ErrorMessage = "Item ID is required."
                    lblPerfectDay_time.Text = "Item ID"
                    lblHeader.Text = "R7 Summary Approved And Paid Invoices Report Sorted By Branch For Branch(Spec Item Expenditure)"
                Case "R8"
                    ValPerfectDay_time.ErrorMessage = "Item ID is required."
                    lblPerfectDay_time.Text = "Item ID"
                    lblHeader.Text = "R8 Summary Approved And Paid Invoices Report Sorted By Branch For Head Office(Spec Item Expenditure)"
            End Select
        End If

        lnkBack.NavigateUrl = dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable
        Dim startdate As String = Format(CDate(txtSDate.Text), "MM/dd/yyyy")
        Dim Enddate As String = Format(CDate(txtEndDate.Text), "MM/dd/yyyy")

        crParam.Add("userIDParam", Session("UserID"))
        '    crParam.Add("companyIDParam", Session("CompanyIdToken"))
        crParam.Add("dateFromParam", startdate)
        crParam.Add("dateToParam", Enddate)

        Select Case UCase(viewstate("type"))
            Case "PO"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                crParam.Add("perfectDays", txtPerfectDay_time.Text)
                ReportViewer.popCrystalReport(Me, "perfectOrderFulfillment.rpt", crParam)
            Case "VENDOR"
                crParam.Add("companyIDParam", Session("CompanyIdToken"))
                crParam.Add("expectedCycleTime", txtPerfectDay_time.Text)
                ReportViewer.popCrystalReport(Me, "vendorCycleTime.rpt", crParam)
            Case "R7"
                crParam.Add("ItemIDParam", txtPerfectDay_time.Text)
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R7SummaryApprovedPaidInvoicesSortedByBranch-BR.rpt", crParam)
            Case "R8"
                crParam.Add("ItemIDParam", txtPerfectDay_time.Text)
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R8SummaryApprovedPaidInvoicesSortedByBranch-HQ.rpt", crParam)
        End Select
    End Sub
End Class
