Imports AgoraLegacy
'Imports Wheel.Components
Public Class ReportFormat12
    Inherits AgoraLegacy.AppBaseClass
    Public Const MsgInvalidDate As String = "Start Date Must Be Earlier Than Or Equal To End Date"
    Dim dispatcher As New dispatcher

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtEndDate As System.Web.UI.WebControls.TextBox
    Dim strType As String
    Protected WithEvents ValSDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents ValEDate As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cvDate As System.Web.UI.WebControls.CompareValidator
    Protected WithEvents Validationsummary1 As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtEDate As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label

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
        'Put user code to initialize the page here
        strType = Me.Request.QueryString("type")

        Select Case strType
            Case "VENSUB"
                lblHeader.Text = "Vendor Subscription Renewal"
            Case "SOF"
                lblHeader.Text = "Sales Order Form"
            Case "FRFL"
                lblHeader.Text = "Facility Renewal Fees Letter"
            Case "SFRF"
                lblHeader.Text = "Supplier Facility Renewal Form"
            Case "R9"
                lblHeader.Text = "R9 Detail PR Report Sort By Branch Code For Branch"
                Label2.Text = "Start Date"
            Case "R10"
                lblHeader.Text = "R10 Detail PR Report Sort By Branch Code For Head Office"
                Label2.Text = "Start Date"
        End Select
        lnkBack.NavigateUrl = Dispatcher.direct("Report", "ReportSelection.aspx", "pageid=" & strPageId)

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        Dim crParam As New Hashtable
        crParam.Add("userIDParam", Session("UserID"))

        Dim enddate As String = Format(CDate(txtEDate.Text), "MM/dd/yyyy")
        crParam.Add("dateToParam", enddate)

        Select Case strType           
            Case "VENSUB"
                ReportViewer.popCrystalReport(Me, "VendorSubcrpRenewal.rpt", crParam)
                'Michelle (CR0033)
            Case "SOF"
                ReportViewer.popCrystalReport(Me, "salesOrderForm.rpt", crParam)
            Case "FRFL"
                ReportViewer.popCrystalReport(Me, "FacilityRenewalFeesLetter.rpt", crParam)
            Case "SFRF"
                ReportViewer.popCrystalReport(Me, "supplierFacilityRenewalForm.rpt", crParam)
            Case "R9"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R9DetailPRReportSortByBranchCode-BR.rpt", crParam)
            Case "R10"
                crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))
                ReportViewer.popCrystalReport(Me, "R10DetailPRReportSortByBranchCode-HQ.rpt", crParam)
        End Select


    End Sub


End Class
