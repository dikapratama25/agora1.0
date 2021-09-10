Imports AppCommon
Imports eProcure.Component

Public Class ReportFormat9
    Inherits AppCommon.AppBaseClass

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblHeader As System.Web.UI.WebControls.Label
    Protected WithEvents label2 As System.Web.UI.WebControls.Label
    Protected WithEvents cboUserName As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ValUserName As System.Web.UI.WebControls.RequiredFieldValidator
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents vldSumm As System.Web.UI.WebControls.ValidationSummary
    Protected WithEvents lnkBack As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdClear As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label

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
            BindData()

            Select Case UCase(viewstate("type"))
                Case "R13O"
                    lblHeader.Text = "R13 Opex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Branch"
                Case "R13C"
                    lblHeader.Text = "R13 Capex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Branch"
                Case "R14O"
                    lblHeader.Text = "R14 Opex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Head Office"
                Case "R14C"
                    lblHeader.Text = "R14 Capex Purchase Requisition Pending Approval By Approver Sort By Branch Code For Head Office"
                Case "R15O"
                    lblHeader.Text = "R15 Opex Invoices Pending Approval For Approver Sort By Vendor"
                Case "R15C"
                    lblHeader.Text = "R15 Capex Invoices Pending Approval For Approver Sort By Vendor"
            End Select
        End If
        lnkBack.NavigateUrl = "ReportSelection.aspx?pageid=" & strPageId
    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        Dim crParam As New Hashtable

        crParam.Add("userIDParam", Session("UserID"))
        crParam.Add("buyerCompanyIDParam", Session("CompanyIdToken"))

        Select Case UCase(viewstate("type"))
            Case "R13O"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R13OpexPRPendingApprvByBranch-Branch.rpt", crParam)
            Case "R13C"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R13CapexPRPendingApprvByBranch-Branch.rpt", crParam)
            Case "R14O"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R14OpexPRPendingApprvByBranch-HQ.rpt", crParam)
            Case "R14C"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R14CapexPRPendingApprvByBranch-HQ.rpt", crParam)
            Case "R15O"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R15OpexInvoicePendingApproval.rpt", crParam)
            Case "R15C"
                crParam.Add("FOFMID", cboUserName.SelectedValue)
                ReportViewer.popCrystalReport(Me, "R15CapexInvoicePendingApproval.rpt", crParam)
        End Select

    End Sub

    Public Function BindData()
        Dim cbolist As New ListItem
        cbolist.Value = ""
        cbolist.Text = "---Select---"

        If UCase(viewstate("type")) = "R15O" Or UCase(viewstate("type")) = "R15C" Then 'For FO & FM
            Common.FillDefault(cboUserName, "USER_MSTR, APPROVAL_GRP_FM WHERE UM_COY_ID='" & Session("CompanyID") & "' AND " & _
                "(AGFM_FM = UM_USER_ID OR AGFM_A_FM = UM_USER_ID) " & _
                "UNION SELECT * FROM USER_MSTR, APPROVAL_GRP_FO ", "UM_USER_NAME", "UM_USER_ID", , "UM_COY_ID='" & Session("CompanyID") & "' and (AGFO_FO = UM_USER_ID OR AGFM_A_FO = UM_USER_ID)")
        Else 'For PR Approval
            Common.FillDefault(cboUserName, "USER_MSTR, APPROVAL_GRP_AO", "UM_USER_NAME", "UM_USER_ID", , "UM_COY_ID='" & Session("CompanyID") & "' and (AGA_AO = UM_USER_ID OR AGA_A_AO = UM_USER_ID)")
            cboUserName.Items.Insert(0, cbolist)
        End If
    End Function

End Class